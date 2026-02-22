// This code has been made by Simon VRANA.
// Please ask by email (simon.vrana.pro@gmail.com) before reusing for commercial purpose.

using UnityEngine;

namespace Helyn.Logger
{
	public static class DebugProxy
	{
		[HideInCallstack]
		public static void Log(object message) => UnityEngine.Debug.Log(message);

		[HideInCallstack]
		public static void Log(object message, Object context) => UnityEngine.Debug.Log(message, context);

		[HideInCallstack]
		public static void LogFormat(string format, params object[] args) => UnityEngine.Debug.LogFormat(format, args);

		[HideInCallstack]
		public static void LogWarning(object message) => UnityEngine.Debug.LogWarning(message);

		[HideInCallstack]
		public static void LogWarning(object message, Object context) => UnityEngine.Debug.LogWarning(message, context);

		[HideInCallstack]
		public static void LogError(object message) => UnityEngine.Debug.LogError(message);

		[HideInCallstack]
		public static void LogError(object message, Object context) => UnityEngine.Debug.LogError(message, context);

		[HideInCallstack]
		public static void LogException(System.Exception exception) => UnityEngine.Debug.LogException(exception);

		[HideInCallstack]
		public static void LogException(System.Exception exception, Object context) => UnityEngine.Debug.LogException(exception, context);

		[HideInCallstack]
		public static void LogAssertion(object message) => UnityEngine.Debug.LogAssertion(message);

		[HideInCallstack]
		public static void LogAssertion(object message, Object context) => UnityEngine.Debug.LogAssertion(message, context);

		[HideInCallstack]
		public static void LogTrace(string message) => LogTrace(null, message);

		[HideInCallstack]
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
			}
		}

		[HideInCallstack]
		public static void LogTrace(System.Func<string> messageMethod) => LogTrace(null, messageMethod);

		[HideInCallstack]
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