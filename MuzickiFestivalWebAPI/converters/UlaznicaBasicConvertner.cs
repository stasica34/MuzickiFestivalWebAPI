using Muzicki_festival.DTOs;
using Muzicki_festival.Entiteti;
using System.Reflection.Metadata;
using System.Text.Json;
using System.Text.Json.Serialization;

public class UlaznicaBasicConverter : JsonConverter<UlaznicaBasic>
{
    public override UlaznicaBasic Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
            throw new JsonException();

        string? tipUlazniceStr = null;
        Dictionary<string, JsonElement> properties = new();

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
                break;

            if (reader.TokenType == JsonTokenType.PropertyName)
            {
                string propertyName = reader.GetString()!;
                reader.Read();

                if (propertyName.Equals("tipUlaznice", StringComparison.OrdinalIgnoreCase))
                {
                    tipUlazniceStr = reader.GetString();
                }
                else
                {
                    properties.Add(propertyName, JsonElement.ParseValue(ref reader));
                }
            }
        }

        if (string.IsNullOrEmpty(tipUlazniceStr))
            throw new JsonException("Polje 'tipUlaznice' je obavezno.");

        if (!Enum.TryParse<TipUlaznice>(tipUlazniceStr, true, out var tip))
            throw new JsonException($"Nepoznat tip ulaznice: {tipUlazniceStr}");

        var jsonObject = JsonSerializer.Serialize(properties);

        return tip switch
        {
            TipUlaznice.JEDNODNEVNA => JsonSerializer.Deserialize<JednodnevnaBasic>(jsonObject, options)!,
            TipUlaznice.VISEDNEVNA => JsonSerializer.Deserialize<ViseDnevnaBasic>(jsonObject, options)!,
            TipUlaznice.VIP => JsonSerializer.Deserialize<VIPBasic>(jsonObject, options)!,
            TipUlaznice.AKREDITACIJA => JsonSerializer.Deserialize<AkreditacijaBasic>(jsonObject, options)!,
            _ => throw new JsonException($"Nepoznat tip ulaznice: {tipUlazniceStr}")
        };
    }

    public override void Write(Utf8JsonWriter writer, UlaznicaBasic value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, (object)value, value.GetType(), options);
    }
}
