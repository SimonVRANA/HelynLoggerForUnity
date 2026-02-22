// This code has been made by Simon VRANA.
// Please ask by email (simon.vrana.pro@gmail.com) before reusing for commercial purpose.

using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Helyn.Logger
{
	public static class HelynFileLogHandler
	{
		public static LogFormat Format { get; internal set; }

		private static string logFilePath;

		public static string LogFilePath
		{
			get => logFilePath;
			internal set
			{
				if (string.IsNullOrWhiteSpace(value))
				{
					throw new ArgumentException("LogFilePath cannot be null or empty.");
				}

				string finalPath = Path.IsPathRooted(value) ? value
															: Path.Combine(Application.persistentDataPath, value);

				// Handle timestamp placeholder in the file name
				if (finalPath.Contains("{timestamp}"))
				{
					string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
					finalPath = finalPath.Replace("{timestamp}", timestamp);
				}

				// Ensure directory exists
				string dir = Path.GetDirectoryName(finalPath);
				if (!string.IsNullOrEmpty(dir))
				{
					try
					{
						Directory.CreateDirectory(dir);
					}
					catch
					{
						// best effort
					}
				}

				// Delete file if exists for fresh start (best effort)
				if (File.Exists(finalPath))
				{
					try
					{
						File.Delete(finalPath);
					}
					catch
					{
						// ignore
					}
				}

				logFilePath = finalPath;
			}
		}

		// Queue-based background writer to preserve order
		private static readonly ConcurrentQueue<string> writeQueue = new();

		private static readonly SemaphoreSlim queueSignal = new(0);
		private static readonly object fileLock = new();
		private static readonly CancellationTokenSource cts = new();
		private static readonly Task backgroundWriterTask;
		private const int maxPendingMessages = 10000;

		static HelynFileLogHandler()
		{
			// Start background writer
			backgroundWriterTask = Task.Run(BackgroundWriterLoop, cts.Token);

			// Flush on application quit
			try
			{
				Application.quitting += () =>
				{
					ShutdownAndFlushAsync().GetAwaiter().GetResult();
				};
			}
			catch
			{
				// ignore if Application.quitting subscription fails
			}
		}

		private static async Task BackgroundWriterLoop()
		{
			CancellationToken token = cts.Token;
			try
			{
				while (!token.IsCancellationRequested)
				{
					await queueSignal.WaitAsync(token).ConfigureAwait(false);

					// Dequeue and write everything currently queued to reduce syscalls
					while (writeQueue.TryDequeue(out string? message))
					{
						try
						{
							if (string.IsNullOrEmpty(logFilePath))
							{
								continue;
							}

							lock (fileLock)
							{
								File.AppendAllText(logFilePath, message + Environment.NewLine);
							}
						}
						catch
						{
							// Never throw from logging
						}
					}
				}
			}
			catch (OperationCanceledException)
			{
				// expected during shutdown
			}

			while (writeQueue.TryDequeue(out string? remaining))
			{
				try
				{
					if (!string.IsNullOrEmpty(logFilePath))
					{
						lock (fileLock)
						{
							File.AppendAllText(logFilePath, remaining + Environment.NewLine);
						}
					}
				}
				catch { }
			}
		}

		private static async Task ShutdownAndFlushAsync()
		{
			try
			{
				cts.Cancel();
				queueSignal.Release();
				await backgroundWriterTask.ConfigureAwait(false);
			}
			catch { }
		}

		[HideInCallstack]
		internal static void LogException(string categoryName, Exception exception, UnityEngine.Object context)
		{
			try
			{
				string unityStyle = StackTraceUtility.ExtractStringFromException(exception);
				string formattedMessage = LogFormatter.FormatLogMessage(HelynLogLevel.Exception,
																		categoryName,
																		unityStyle,
																			Format);
				EnqueueWrite(formattedMessage);
			}
			catch
			{
				// Never throw from logging
			}
		}

		[HideInCallstack]
		internal static void LogFormat(HelynLogLevel logType, string categoryName, UnityEngine.Object context, string format, object[] args)
		{
			string finalMessage = format;
			try
			{
				finalMessage = (args != null && args.Length > 0) ? string.Format(format, args)
																 : format;
			}
			catch
			{
				finalMessage = format;
			}

			string formatedMessage = LogFormatter.FormatLogMessage(logType, categoryName, finalMessage, Format);
			EnqueueWrite(formatedMessage);
		}

		private static void EnqueueWrite(string message)
		{
			if (writeQueue.Count > maxPendingMessages)
			{
				_ = writeQueue.TryDequeue(out _);
			}

			writeQueue.Enqueue(message);
			queueSignal.Release();
		}
	}
}