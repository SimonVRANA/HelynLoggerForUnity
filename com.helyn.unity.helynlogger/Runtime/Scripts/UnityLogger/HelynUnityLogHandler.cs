// This code has been made by Simon VRANA.
// Please ask by email (simon.vrana.pro@gmail.com) before reusing for commercial purpose.

using System;
using UnityEngine;

namespace Helyn.Logger
{
	public static class HelynUnityLogHandler
	{
		public static ILogHandler DefaultLogHandler { get; internal set; }
		public static LogFormat Format { get; internal set; }

		[HideInCallstack]
		internal static void LogException(string categoryName, Exception exception, UnityEngine.Object context)
		{
			string header = LogFormatter.FormatLogMessage(LogType.Exception, categoryName, "Exception Triggered:", Format);
			DefaultLogHandler.LogFormat(LogType.Exception, context, "{0}", header);
			DefaultLogHandler.LogException(exception, context);
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

			DefaultLogHandler.LogFormat(logType, context, "{0}", LogFormatter.FormatLogMessage(logType, categoryName, finalMessage, Format));
		}
	}
}