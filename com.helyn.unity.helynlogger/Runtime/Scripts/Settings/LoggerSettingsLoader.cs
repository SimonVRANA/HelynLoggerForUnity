// This code has been made by Simon VRANA.
// Please ask by email (simon.vrana.pro@gmail.com) before reusing for commercial purpose.

using Newtonsoft.Json;
using System.IO;
using UnityEngine;

namespace Helyn.Logger
{
	public static class LoggerSettingsLoader
	{
		private static string SettingsBasePath => Path.Combine(Application.streamingAssetsPath, "HelynLogger");
		private const string settingsFileName = "HelynLoggerSettings.json";
		private static string SettingsFilePath => System.IO.Path.Combine(SettingsBasePath, settingsFileName);

		public static LoggerSettings LoadSettings()
		{
			if (!System.IO.File.Exists(SettingsFilePath))
			{
				SaveSettings(null);
			}

			try
			{
				string json = System.IO.File.ReadAllText(SettingsFilePath);
				LoggerSettings settings = JsonConvert.DeserializeObject<LoggerSettings>(json);
				return settings ?? new LoggerSettings();
			}
			catch
			{
				Debug.LogWarning("No valid logger settings found, using default settings. Creating a default settings.json file.");

				SaveSettings(new LoggerSettings());

				try
				{
					string json = System.IO.File.ReadAllText(SettingsFilePath);
					LoggerSettings settings = JsonConvert.DeserializeObject<LoggerSettings>(json);
					return settings ?? new LoggerSettings();
				}
				catch (System.Exception secondException)
				{
					Debug.LogError("Failed to load default logger settings after creating default settings file.");
					Debug.LogException(secondException);
					return LoadSettings();
				}
			}
		}

		public static void SaveSettings(LoggerSettings settings)
		{
			settings ??= new LoggerSettings();

			try
			{
				string directory = System.IO.Path.GetDirectoryName(SettingsFilePath);
				if (string.IsNullOrEmpty(directory))
				{
					directory = SettingsBasePath;
				}

				if (!System.IO.Directory.Exists(directory))
				{
					System.IO.Directory.CreateDirectory(directory);
				}

				string json = JsonConvert.SerializeObject(settings, Formatting.Indented);
				System.IO.File.WriteAllText(SettingsFilePath, json);
			}
			catch (System.Exception ex)
			{
				Debug.LogError($"Error saving logger settings to {SettingsFilePath}: {ex.Message}");
			}
		}
	}
}