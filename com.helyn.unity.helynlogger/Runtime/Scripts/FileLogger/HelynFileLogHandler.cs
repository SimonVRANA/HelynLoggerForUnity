// This code has been made by Simon VRANA.
// Please ask by email (simon.vrana.pro@gmail.com) before reusing for commercial purpose.

using System;
using UnityEngine;

namespace Helyn.Logger
{
	public class HelynFileLogHandler
	{
		[HideInCallstack]
		internal void LogException(Exception exception, UnityEngine.Object context)
		{
			throw new NotImplementedException();
		}

		[HideInCallstack]
		internal void LogFormat(LogType logType, UnityEngine.Object context, string format, object[] args)
		{
			throw new NotImplementedException();
		}
	}
}