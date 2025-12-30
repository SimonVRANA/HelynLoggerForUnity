// This code has been made by Simon VRANA.
// Please ask by email (simon.vrana.pro@gmail.com) before reusing for commercial purpose.

using System.IO;
using UnityEngine;

namespace Helyn.Logger
{
	public class LoggerSettings
	{
		public string ConfigFolderPath { get; set; } = "HelynLogger";
		public string ConfigFileName { get; set; } = "HelynLoggerConfig.json";

		public string ConfigFilePath
		{
			get
			{
				if (string.IsNullOrEmpty(ConfigFolderPath))
				{
					return Application.streamingAssetsPath;
				}
				if (Path.IsPathRooted(ConfigFolderPath))
				{
					return ConfigFolderPath;
				}
				return Path.Combine(Application.streamingAssetsPath, ConfigFolderPath);
			}
		}

		#region Unity console

		public bool EnableConsoleLogging { get; set; } = true;
		public string ConsoleLogFormat { get; set; } = "[{timestamp:yyyy-MM-dd HH:mm:ss.fff}] {level} {category}: {message}";
		public bool ColorLogLevel { get; set; } = false;
		public string TraceColor { get; set; } = "white";
		public string DebugColor { get; set; } = "white";
		public string InformationColor { get; set; } = "white";
		public string WarningColor { get; set; } = "yellow";
		public string ErrorColor { get; set; } = "red";
		public string CriticalColor { get; set; } = "magenta";
		public string NoneColor { get; set; } = "white";

		#endregion Unity console

		#region File

		public bool EnableFileLogging { get; set; } = false;
		public string LogFilePath { get; set; } = "logs/log.txt";
		public string FileLogFormat { get; set; } = "[{timestamp:yyyy-MM-dd HH:mm:ss.fff}] {level} {category}: {message}";

		#endregion File
	}
}