// This code has been made by Simon VRANA.
// Please ask by email (simon.vrana.pro@gmail.com) before reusing for commercial purpose.

using System;
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
            if (settings.EnableFileLogging)
            {
                fileLogHandler.LogFormat(logType, context, format, args);
            }
            if (settings.EnableConsoleLogging)
            {
                unityLogHandler.LogFormat(logType, context, format, args);
            }
        }
    }
}
