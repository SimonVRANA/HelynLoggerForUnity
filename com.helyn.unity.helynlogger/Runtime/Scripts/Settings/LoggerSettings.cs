// This code has been made by Simon VRANA.
// Please ask by email (simon.vrana.pro@gmail.com) before reusing for commercial purpose.

using Newtonsoft.Json;
using System.IO;
using UnityEngine;

namespace Helyn.Logger
{
	public record LoggerSettings
	{
		public string ConfigFolderPath { get; set; } = "HelynLogger";
		public string ConfigFileName { get; set; } = "HelynLoggerConfig.json";

		[JsonIgnore]
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

		public HelynLogLevel DefaultLogLevel => HelynLogLevel.Log;
		public string Filter { get; set; } = "{}";

		#region Unity console

		public bool EnableConsoleLogging { get; set; } = true;
		public LogFormat ConsoleLogFormat { get; set; } = new();

		#endregion Unity console

		#region File

		public bool EnableFileLogging { get; set; } = false;
		public string LogFilePath { get; set; } = "logs/log.txt";
		public LogFormat FileLogFormat { get; set; } = new();

		#endregion File
	}
}