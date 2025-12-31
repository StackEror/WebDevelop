using Newtonsoft.Json;

namespace WebDevelopment.Client.JsonConverters;

public class EnumNameOnlyConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        var type = Nullable.GetUnderlyingType(objectType) ?? objectType;
        return type.IsEnum;
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        if (value == null)
        {
            writer.WriteNull();
            return;
        }

        writer.WriteValue(value.ToString());
    }

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        var enumType = Nullable.GetUnderlyingType(objectType) ?? objectType;

        if (reader.TokenType == JsonToken.Null)
            return null;

        if (reader.TokenType == JsonToken.String)
        {
            var enumString = (string)reader.Value!;
            return Enum.Parse(enumType, enumString, ignoreCase: true);
        }

        throw new JsonSerializationException($"Unexpected token {reader.TokenType} when parsing enum.");
    }
}
