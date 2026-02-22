// This code has been made by Simon VRANA.
// Please ask by email (simon.vrana.pro@gmail.com) before reusing for commercial purpose.

using System;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Helyn.Logger
{
	public class LogFormatter
	{
		private static readonly Regex timestampRegex = new(@"\{timestamp(?::(?<fmt>[^}]+))?\}",
															RegexOptions.IgnoreCase | RegexOptions.Compiled);

		[HideInCallstack]
		public static string FormatLogMessage(HelynLogLevel logLevel,
											  string category,
											  string message,
											  LogFormat format)
		{
			if (string.IsNullOrWhiteSpace(format.Format))
			{
				format.Format = "[{timestamp:yyyy-MM-dd HH:mm:ss.fff}] {level} {category}: {message}";
			}

			string messageFormat = format.Format;

			// Timestamp with optional format
			messageFormat = timestampRegex.Replace(messageFormat, match =>
			{
				string fmt = match.Groups["fmt"].Value;

				if (string.IsNullOrEmpty(fmt))
				{
					fmt = "yyyy-MM-dd HH:mm:ss.fff";
				}

				return DateTime.Now.ToString(fmt);
			});

			// Log level (with optional coloring)
			string logTypeString = logLevel.ToString();
			if (format.EnableColorLogLevel)
			{
				string color = GetColorForLogType(logLevel, format);

				if (!string.IsNullOrWhiteSpace(color))
				{
					logTypeString = $"<color=#{color}>{logTypeString}</color>";
				}
			}
			messageFormat = messageFormat.Replace("{level}", logTypeString);

			// Category
			messageFormat = messageFormat.Replace("{category}", category ?? string.Empty);

			// Message
			messageFormat = messageFormat.Replace("{message}", message ?? string.Empty);

			// Thread ID
			if (messageFormat.Contains("{threadId}"))
			{
				messageFormat = messageFormat.Replace("{threadId}", Thread.CurrentThread.ManagedThreadId.ToString());
			}

			// Task ID
			if (messageFormat.Contains("{taskId}"))
			{
				messageFormat = messageFormat.Replace("{taskId}", Task.CurrentId?.ToString() ?? "null");
			}

			return messageFormat;
		}

		[HideInCallstack]
		private static string GetColorForLogType(HelynLogLevel level, LogFormat format)
		{
			Color levelColor = level switch
			{
				HelynLogLevel.Trace => format.TraceColor,
				HelynLogLevel.Log => format.LogColor,
				HelynLogLevel.Warning => format.WarningColor,
				HelynLogLevel.Error => format.ErrorColor,
				HelynLogLevel.Assert => format.AssertColor,
				HelynLogLevel.Exception => format.ExceptionColor,
				_ => format.NoneColor
			};
			return ColorUtility.ToHtmlStringRGB(levelColor);
		}
	}
}