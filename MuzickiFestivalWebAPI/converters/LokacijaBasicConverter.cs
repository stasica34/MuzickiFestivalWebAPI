using Muzicki_festival.DTOs;
using Muzicki_festival.Entiteti;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;


//drugi nacin za serijalizaciju i deserijalizaciju
public class LokacijaBasicConverter : JsonConverter<LokacijaBasic>
{
    public override LokacijaBasic? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using (JsonDocument document = JsonDocument.ParseValue(ref reader))
        {
            var root = document.RootElement;

            if (!root.TryGetProperty("tipLokacije", out JsonElement tipElement))
                throw new JsonException("Polje 'tipLokacije' je obavezno.");

            TipLokacije tip = (TipLokacije)tipElement.GetInt32();
            //u json se pise kao tiplokacije kao int
            string json = root.GetRawText();

            return tip switch
            {
                TipLokacije.ZATVORENA => JsonSerializer.Deserialize<ZatvorenaLokacijaBasic>(json, options), //0
                TipLokacije.OTVORENA => JsonSerializer.Deserialize<OtvorenaLokacijaBasic>(json, options),//1
                TipLokacije.KOMBINOVANA => JsonSerializer.Deserialize<KombinovanaLokacijaBasic>(json, options), //2
                _ => throw new JsonException("Nepoznat tip lokacije.")
            };
        }
    }

    public override void Write(Utf8JsonWriter writer, LokacijaBasic value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, (object)value, value.GetType(), options);
    }
}
