// This code has been made by Simon VRANA.
// Please ask by email (simon.vrana.pro@gmail.com) before reusing for commercial purpose.

using Newtonsoft.Json;
using System.Collections.Concurrent;

namespace Helyn.Logger
{
	public class LogFilter
	{
		private HelynLogLevel defaultLogLevel = HelynLogLevel.Log;

		private ConcurrentDictionary<string, HelynLogLevel> logLevelForCategories = new();
		private readonly ConcurrentDictionary<string, HelynLogLevel> resolvedLogTypes = new();

		public LogFilter(string filterJson, HelynLogLevel defaultLogLevel)
		{
			UpdateSettings(filterJson, defaultLogLevel);
		}

		public void UpdateSettings(string filterJson, HelynLogLevel newDefaultLevel)
		{
			this.defaultLogLevel = newDefaultLevel;

			ConcurrentDictionary<string, HelynLogLevel> newRules = JsonConvert.DeserializeObject<ConcurrentDictionary<string, HelynLogLevel>>(filterJson);
			if (newRules != null)
			{
				logLevelForCategories = newRules;
			}

			resolvedLogTypes.Clear();
		}

		public bool ShouldLog(string category, HelynLogLevel incomingLogType)
		{
			// 1. Check Cache
			if (resolvedLogTypes.TryGetValue(category, out HelynLogLevel cachedThreshold))
			{
				return IsSeverityHighEnough(incomingLogType, cachedThreshold);
			}

			// 2. Calculate & Cache
			HelynLogLevel foundThreshold = FindEffectiveLogLevel(category);
			resolvedLogTypes.TryAdd(category, foundThreshold);

			return IsSeverityHighEnough(incomingLogType, foundThreshold);
		}

		private HelynLogLevel FindEffectiveLogLevel(string category)
		{
			string currentSearch = category;
			while (!string.IsNullOrEmpty(currentSearch))
			{
				if (logLevelForCategories.TryGetValue(currentSearch, out HelynLogLevel threshold))
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

		private bool IsSeverityHighEnough(HelynLogLevel incoming, HelynLogLevel threshold)
		{
			return GetSeverity(incoming) >= GetSeverity(threshold);
		}

		private int GetSeverity(HelynLogLevel logType)
		{
			return logType switch
			{
				HelynLogLevel.Exception => 6,
				HelynLogLevel.Error => 5,
				HelynLogLevel.Assert => 5, // Treat Assert like Error
				HelynLogLevel.Warning => 4,
				HelynLogLevel.Log => 3,
				HelynLogLevel.Trace => 2,
				_ => 1
			};
		}
	}
}