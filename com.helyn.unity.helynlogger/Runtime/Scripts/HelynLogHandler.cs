// This code has been made by Simon VRANA.
// Please ask by email (simon.vrana.pro@gmail.com) before reusing for commercial purpose.

using System;
using System.Diagnostics;
using UnityEngine;

namespace Helyn.Logger
{
	public class HelynLogHandler : ILogHandler
	{
		private LoggerSettings settings;
		private LogFilter logFilter;

		private HelynFileLogHandler fileLogHandler = new();
		private HelynUnityLogHandler unityLogHandler = new();

		public HelynLogHandler()
		{
			settings = LoggerSettingsLoader.LoadSettings();

			logFilter = new LogFilter(settings.Filter, settings.DefaultLogLevel);
		}

		[HideInCallstack]
		public void LogException(Exception exception, UnityEngine.Object context)
		{
			if (!logFilter.ShouldLog(GetCategory(context), LogType.Exception))
			{
				return;
			}

			if (settings.EnableFileLogging)
			{
				fileLogHandler.LogException(exception, context);
			}
			if (settings.EnableConsoleLogging)
			{
				unityLogHandler.LogException(exception, context);
			}
		}

		[HideInCallstack]
		public void LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args)
		{
			if (!logFilter.ShouldLog(GetCategory(context), logType))
			{
				return;
			}

			if (settings.EnableFileLogging)
			{
				fileLogHandler.LogFormat(logType, context, format, args);
			}
			if (settings.EnableConsoleLogging)
			{
				unityLogHandler.LogFormat(logType, context, format, args);
			}
		}

		private string GetCategory(UnityEngine.Object context)
		{
			// 1. Context is King: If explicit context exists, use it.
			if (context != null)
			{
				return context.GetType().FullName;
			}

			// 2. Stack Trace Fallback
			// We start looking from the current frame up.
			StackTrace stackTrace = new(fNeedFileInfo: false);
			StackFrame[] frames = stackTrace.GetFrames();

			if (frames == null)
			{
				return "Native/Global"; // Happens with some native Unity crashes
			}

			foreach (StackFrame frame in frames)
			{
				System.Reflection.MethodBase method = frame.GetMethod();
				Type type = method.DeclaringType;

				if (type == null)
				{
					continue;
				}

				// SKIP only the logging plumbing
				if (type == typeof(HelynLogHandler) ||          // this
					type == typeof(UnityEngine.Debug) ||        // Unity Debug
					type == typeof(UnityEngine.Logger) ||       // Unity Loggertype == typeof(UnityEngine.Assertions.Assert) || // Unity Assertions
					type.Name == "DebugLogHandler")             // Handle Unity LogHandler internal class by name (just in case)
				{
					continue;
				}

				// The first class that isn't a logger is our Source.
				return type.FullName;
			}

			return "Native/Global";
		}
	}
}