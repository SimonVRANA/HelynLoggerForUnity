// This code has been made by Simon VRANA.
// Please ask by email (simon.vrana.pro@gmail.com) before reusing for commercial purpose.

using Newtonsoft.Json;
using UnityEngine;

namespace Helyn.Logger
{
	public record LogFormat
	{
		public string Format { get; set; } = "[{timestamp:yyyy-MM-dd HH:mm:ss.fff}] {level} {category}: {message}";
		public bool EnableColorLogLevel { get; set; } = false;
		[JsonConverter(typeof(UnityColorConverter))]
		public Color LogColor { get; set; } = Color.white;
		[JsonConverter(typeof(UnityColorConverter))]
		public Color WarningColor { get; set; } = Color.yellow;
		[JsonConverter(typeof(UnityColorConverter))]
		public Color ErrorColor { get; set; } = Color.red;
		[JsonConverter(typeof(UnityColorConverter))]
		public Color AssertColor { get; set; } = Color.red;
		[JsonConverter(typeof(UnityColorConverter))]
		public Color ExceptionColor { get; set; } = Color.magenta;
		[JsonConverter(typeof(UnityColorConverter))]
		public Color NoneColor { get; set; } = Color.white;
	}
}
