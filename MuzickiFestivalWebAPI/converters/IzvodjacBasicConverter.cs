using Muzicki_festival.DTOs;
using Muzicki_festival.Entiteti;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

public class IzvodjacBasicConverter : JsonConverter<IzvodjacBasic>
{
    public override IzvodjacBasic? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using (JsonDocument document = JsonDocument.ParseValue(ref reader))
        {
            var root = document.RootElement;

            if (!root.TryGetProperty("tipIzvodjaca", out JsonElement tipElement))
                throw new JsonException("Polje 'tipIzvodjaca' je obavezno.");

            string tipString = tipElement.GetString() ?? throw new JsonException("tipIzvodjaca ne može biti null.");
            
            if (!Enum.TryParse<IzvodjacTip>(tipString, true, out var tip))
                throw new JsonException($"Nepoznat tip izvodjaca: {tipString}");

            string json = root.GetRawText();

            return tip switch
            {
                IzvodjacTip.SOLO_UMETNIK => JsonSerializer.Deserialize<Solo_UmetnikBasic>(json, options),
                IzvodjacTip.BEND => JsonSerializer.Deserialize<BendBasic>(json, options),
                _ => throw new JsonException("Nepoznat tip izvođača.")
            };
        }
    }

    public override void Write(Utf8JsonWriter writer, IzvodjacBasic value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, (object)value, value.GetType(), options);
    }
}
