namespace MeetingService.Enums;
using System;
using Newtonsoft.Json;

public enum BacklogType
{
    Project,
    Sprint
}

public class BacklogTypeConverter : Newtonsoft.Json.JsonConverter<BacklogType>
{
    public override BacklogType ReadJson(JsonReader reader, Type objectType, BacklogType existingValue, bool hasExistingValue, Newtonsoft.Json.JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Null)
        {
            return default;
        }

        if (reader.TokenType == JsonToken.Integer)
        {
            var intValue = Convert.ToInt32(reader.Value);
            return (BacklogType)intValue;
        }

        throw new JsonSerializationException($"Unexpected token type '{reader.TokenType}' for enum '{objectType.Name}'.");
    }

    public override void WriteJson(JsonWriter writer, BacklogType value, Newtonsoft.Json.JsonSerializer serializer)
    {
        writer.WriteValue((int)value);
    }
}
