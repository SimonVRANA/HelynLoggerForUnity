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

			return new Color(
				(float)jsonObject["r"],
				(float)jsonObject["g"],
				(float)jsonObject["b"],
				(float)jsonObject["a"]
			);
		}
	}
}
