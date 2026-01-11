// This code has been made by Simon VRANA.
// Please ask by email (simon.vrana.pro@gmail.com) before reusing for commercial purpose.

using System;
using UnityEditor.PackageManager;
using UnityEngine;

namespace Helyn.Logger
{
	public static class HelynUnityLogHandler
	{
		[HideInCallstack]
		internal static void LogException(Exception exception, UnityEngine.Object context)
		{
			throw new NotImplementedException();
		}

		[HideInCallstack]
		internal static void LogFormat(LogType logType, UnityEngine.Object context, string format, object[] args)
		{
			context.
			throw new NotImplementedException();
		}

		[HideInCallstack]
		private static void Log()
		{
			string fullMessage = LogFormatter.FormatLogMessage(logType, categoryName, message, settings);
			switch (logLevel)
			{
				case LogLevel.Critical:
				case LogLevel.Error:
					Debug.LogError(fullMessage);
					break;

				case LogLevel.Warning:
					Debug.LogWarning(fullMessage);
					break;

				default:
					Debug.Log(fullMessage);
					break;
			}

			if (exception != null)
			{
				Debug.LogException(exception);
			}
		}
	}
}