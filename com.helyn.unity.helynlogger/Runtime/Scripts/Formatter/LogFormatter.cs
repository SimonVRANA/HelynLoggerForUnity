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
		[HideInCallstack]
		public static string FormatLogMessage(LogType logType,
											  string category,
											  string message,
											  LoggerSettings settings)
		{
			if (string.IsNullOrWhiteSpace(settings.ConsoleLogFormat))
			{
				settings.ConsoleLogFormat = "[{timestamp:yyyy-MM-dd HH:mm:ss.fff}] {level} {category}: {message}";
			}

			string result = settings.ConsoleLogFormat;

			// Timestamp with optional format
			result = Regex.Replace(
				result,
				@"\{timestamp(?::(?<fmt>[^}]+))?\}",
				match =>
				{
					string fmt = match.Groups["fmt"].Value;
					if (string.IsNullOrEmpty(fmt))
					{
						fmt = "yyyy-MM-dd HH:mm:ss.fff"; // default
					}

					return DateTime.Now.ToString(fmt);
				},
				RegexOptions.IgnoreCase
			);

			// Log level (with optional coloring)
			string logTypeString = logType.ToString();
			if (settings.ColorLogLevel)
			{
				string color = GetColorForLogType(logType, settings);

				if (!string.IsNullOrWhiteSpace(color))
				{
					logTypeString = $"<color={color}>{logTypeString}</color>";
				}
			}
			result = result.Replace("{level}", logTypeString);

			// Category
			result = result.Replace("{category}", category ?? string.Empty);

			// Message
			result = result.Replace("{message}", message ?? string.Empty);

			// Thread ID
			if (result.Contains("{threadId}"))
			{
				result = result.Replace("{threadId}", Thread.CurrentThread.ManagedThreadId.ToString());
			}

			// Task ID
			if (result.Contains("{taskId}"))
			{
				result = result.Replace("{taskId}", Task.CurrentId?.ToString() ?? "null");
			}

			return result;
		}

		[HideInCallstack]
		private static string GetColorForLogType(LogType level, LoggerSettings settings)
		{
			if (!settings.ColorLogLevel)
			{
				return string.Empty;
			}

			return level switch
			{
				LogType.Log => settings.LogColor,
				LogType.Warning => settings.WarningColor,
				LogType.Error => settings.ErrorColor,
				LogType.Assert => settings.AssertColor,
				LogType.Exception => settings.ExceptionColor,
				_ => settings.NoneColor
			};
		}
	}
}