// This code has been made by Simon VRANA.
// Please ask by email (simon.vrana.pro@gmail.com) before reusing for commercial purpose.

using Newtonsoft.Json;
using System.Collections.Concurrent;
using UnityEngine;

namespace Helyn.Logger
{
    public class LogFilter
    {
        private readonly LogType defaultLogLevel = LogType.Log;

        private ConcurrentDictionary<string, LogType> logLevelForCategories = new();

        public LogFilter(string filterJson, LogType defaultLogLevel)
        {
            this.defaultLogLevel = defaultLogLevel;
            try
            {
                ConcurrentDictionary<string, LogType> filterDict = JsonConvert.DeserializeObject<ConcurrentDictionary<string, LogType>>(filterJson);
                if (filterDict != null)
                {
                    logLevelForCategories = filterDict;
                }
            }
            catch
            {
                Debug.LogError("Failed to parse log filter JSON. Using default log level for all logs.");
            }
        }

        public bool ShouldLog(string category, LogType logType)
        {
            if (logLevelForCategories.TryGetValue(category, out LogType categoryLogType))
            {
                return logType >= categoryLogType;
            }
            return logType >= defaultLogLevel;
        }
    }
}
