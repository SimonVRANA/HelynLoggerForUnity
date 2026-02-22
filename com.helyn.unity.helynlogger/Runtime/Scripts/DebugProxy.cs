// This code has been made by Simon VRANA.
// Please ask by email (simon.vrana.pro@gmail.com) before reusing for commercial purpose.

using UnityEngine;

namespace Helyn.Logger
{
	public static class DebugProxy
	{
		public static void Log(object message) => Debug.Log(message);

		public static void Log(object message, Object context) => Debug.Log(message, context);

		public static void LogFormat(string format, params object[] args) => Debug.LogFormat(format, args);

		public static void LogWarning(object message) => Debug.LogWarning(message);

		public static void LogWarning(object message, Object context) => Debug.LogWarning(message, context);

		public static void LogError(object message) => Debug.LogError(message);

		public static void LogError(object message, Object context) => Debug.LogError(message, context);

		public static void LogException(System.Exception exception) => Debug.LogException(exception);

		public static void LogException(System.Exception exception, Object context) => Debug.LogException(exception, context);

		public static void LogTrace(string message) => LogTrace(null, message);

		public static void LogTrace(Object context, string format, params object[] args)
		{
			if (HelynLogHandler.Instance != null
				&& HelynLogHandler.Instance.LogFilter != null)
			{
				if (HelynLogHandler.Instance.LogFilter.ShouldLog(HelynLogHandler.GetCategory(context), HelynLogLevel.Trace))
				{
					HelynLogHandler.Instance.LogFormat(HelynLogLevel.Trace, context, format, args);
				}
			}
			else
			{
				string finalMessage = format;
				try
				{
					finalMessage = (args != null && args.Length > 0) ? string.Format(format, args) : format;
				}
				catch
				{
					finalMessage = format;
				}
				UnityEngine.Debug.Log(finalMessage, context);
			}
		}

		public static void LogTrace(System.Func<string> messageMethod) => LogTrace(null, messageMethod);

		public static void LogTrace(Object context, System.Func<string> messageMethod, params object[] args)
		{
			if (messageMethod == null)
			{
				return;
			}
			string message;
			try
			{
				message = messageMethod();
			}
			catch
			{
				message = string.Empty;
			}
			if (string.IsNullOrEmpty(message))
			{
				return;
			}

			HelynLogHandler logHandler = HelynLogHandler.Instance;
			LogFilter logFilter = logHandler?.LogFilter;
			if (logHandler != null
				&& logFilter != null)
			{
				if (logFilter.ShouldLog(HelynLogHandler.GetCategory(context), HelynLogLevel.Trace))
				{
					logHandler.LogFormat(HelynLogLevel.Trace, context, message, args);
				}
			}
			else
			{
				string finalMessage = message;
				try
				{
					finalMessage = (args != null && args.Length > 0) ? string.Format(message, args) : message;
				}
				catch
				{
					finalMessage = message;
				}
				UnityEngine.Debug.Log(finalMessage, context);
			}
		}
	}
}