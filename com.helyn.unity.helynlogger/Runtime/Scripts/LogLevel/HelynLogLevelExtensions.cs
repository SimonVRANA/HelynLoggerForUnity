// This code has been made by Simon VRANA.
// Please ask by email (simon.vrana.pro@gmail.com) before reusing for commercial purpose.

using UnityEngine;

namespace Helyn.Logger
{
	public static class HelynLogLevelExtensions
	{
		public static LogType ToUnityLogType(this HelynLogLevel level)
		{
			return level switch
			{
				HelynLogLevel.Trace => LogType.Log,
				HelynLogLevel.Log => LogType.Log,
				HelynLogLevel.Warning => LogType.Warning,
				HelynLogLevel.Error => LogType.Error,
				HelynLogLevel.Assert => LogType.Assert,
				HelynLogLevel.Exception => LogType.Exception,
				_ => LogType.Log
			};
		}

		public static HelynLogLevel ToHelynLogLevel(this LogType unityType)
		{
			return unityType switch
			{
				LogType.Exception => HelynLogLevel.Exception,
				LogType.Error => HelynLogLevel.Error,
				LogType.Assert => HelynLogLevel.Assert,
				LogType.Warning => HelynLogLevel.Warning,
				LogType.Log => HelynLogLevel.Log,
				_ => HelynLogLevel.Log
			};
		}
	}
}