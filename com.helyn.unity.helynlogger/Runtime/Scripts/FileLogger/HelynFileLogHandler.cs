// This code has been made by Simon VRANA.
// Please ask by email (simon.vrana.pro@gmail.com) before reusing for commercial purpose.

using System;
using System.IO;
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
				Directory.CreateDirectory(Path.GetDirectoryName(finalPath)!);

				// Delete file if exists for fresh start
				if (File.Exists(finalPath))
				{
					File.Delete(finalPath);
				}

				logFilePath = finalPath;
			}
		}

		private static readonly object fileLock = new();

		[HideInCallstack]
		internal static void LogException(string categoryName, Exception exception, UnityEngine.Object context)
		{
			try
			{
				string unityStyle = StackTraceUtility.ExtractStringFromException(exception);
				string formattedMessage = LogFormatter.FormatLogMessage(LogType.Exception,
																		categoryName,
																		unityStyle,
																		Format);

				lock (fileLock)
				{
					File.AppendAllText(LogFilePath, formattedMessage + Environment.NewLine);
				}
			}
			catch
			{
				// Never throw from logging
			}
		}

		[HideInCallstack]
		internal static void LogFormat(LogType logType, string categoryName, UnityEngine.Object context, string format, object[] args)
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

			try
			{
				lock (fileLock)
				{
					File.AppendAllText(LogFilePath, formatedMessage + Environment.NewLine);
				}
			}
			catch
			{
				// NEVER throw inside logging. Silent failure is standard.
			}
		}
	}
}