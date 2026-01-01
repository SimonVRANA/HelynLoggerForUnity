// This code has been made by Simon VRANA.
// Please ask by email (simon.vrana.pro@gmail.com) before reusing for commercial purpose.

using Newtonsoft.Json;
using System.Collections.Concurrent;
using UnityEngine;

namespace Helyn.Logger
{
	public class LogFilter
	{
		private LogType defaultLogLevel = LogType.Log;

		private ConcurrentDictionary<string, LogType> logLevelForCategories = new();
		private readonly ConcurrentDictionary<string, LogType> resolvedLogTypes = new();

		public LogFilter(string filterJson, LogType defaultLogLevel)
		{
			UpdateSettings(filterJson, defaultLogLevel);
		}

		public void UpdateSettings(string filterJson, LogType newDefaultLevel)
		{
			this.defaultLogLevel = newDefaultLevel;

			ConcurrentDictionary<string, LogType> newRules = JsonConvert.DeserializeObject<ConcurrentDictionary<string, LogType>>(filterJson);
			if (newRules != null)
			{
				logLevelForCategories = newRules;
			}

			resolvedLogTypes.Clear();
		}

		public bool ShouldLog(string category, LogType incomingLogType)
		{
			// 1. Check Cache
			if (resolvedLogTypes.TryGetValue(category, out LogType cachedThreshold))
			{
				return IsSeverityHighEnough(incomingLogType, cachedThreshold);
			}

			// 2. Calculate & Cache
			LogType foundThreshold = FindEffectiveLogLevel(category);
			resolvedLogTypes.TryAdd(category, foundThreshold);

			return IsSeverityHighEnough(incomingLogType, foundThreshold);
		}

		private LogType FindEffectiveLogLevel(string category)
		{
			string currentSearch = category;
			while (!string.IsNullOrEmpty(currentSearch))
			{
				if (logLevelForCategories.TryGetValue(currentSearch, out LogType threshold))
				{
					return threshold;
				}

				int lastDot = currentSearch.LastIndexOf('.');
				if (lastDot > -1)
				{
					currentSearch = currentSearch.Substring(0, lastDot);
				}
				else
				{
					break;
				}
			}
			return defaultLogLevel;
		}

		// Unity LogType Enum is not sorted by severity!
		// Error=0, Assert=1, Warning=2, Log=3, Exception=4
		private bool IsSeverityHighEnough(LogType incoming, LogType threshold)
		{
			return GetSeverity(incoming) >= GetSeverity(threshold);
		}

		private int GetSeverity(LogType logType)
		{
			return logType switch
			{
				LogType.Exception => 5,
				LogType.Error => 4,
				LogType.Assert => 4, // Treat Assert like Error
				LogType.Warning => 3,
				LogType.Log => 2,
				_ => 1
			};
		}
	}
}