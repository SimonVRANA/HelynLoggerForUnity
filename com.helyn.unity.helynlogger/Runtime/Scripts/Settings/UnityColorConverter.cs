// This code has been made by Simon VRANA.
// Please ask by email (simon.vrana.pro@gmail.com) before reusing for commercial purpose.

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using UnityEngine;

namespace Helyn.Logger
{
	public class UnityColorConverter : JsonConverter<Color>
	{
		public override void WriteJson(JsonWriter writer, Color value, JsonSerializer serializer)
		{
			writer.WriteStartObject();
			writer.WritePropertyName("r"); writer.WriteValue(value.r);
			writer.WritePropertyName("g"); writer.WriteValue(value.g);
			writer.WritePropertyName("b"); writer.WriteValue(value.b);
			writer.WritePropertyName("a"); writer.WriteValue(value.a);
			writer.WriteEndObject();
		}

		public override Color ReadJson(JsonReader reader, Type objectType, Color existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			JObject jsonObject = JObject.Load(reader);

			float r = jsonObject["r"]?.Value<float>() ?? 0f;
			float g = jsonObject["g"]?.Value<float>() ?? 0f;
			float b = jsonObject["b"]?.Value<float>() ?? 0f;
			float a = jsonObject["a"]?.Value<float>() ?? 1f;

			return new Color(r, g, b, a);
		}
	}
}