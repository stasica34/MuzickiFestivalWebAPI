using Muzicki_festival.DTOs;
using Muzicki_festival.Entiteti;
using System.Text.Json;
using System.Text.Json.Nodes; 
using System.Text.Json.Serialization;

public class UlaznicaBasicConverter : JsonConverter<UlaznicaBasic>
{
    public override UlaznicaBasic Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        JsonNode? node = JsonNode.Parse(ref reader);

        if (node == null)
            throw new JsonException("JSON objekat za ulaznicu je neispravan.");
        JsonNode? tipUlazniceNode = node["tipUlaznice"];

        if (tipUlazniceNode == null || tipUlazniceNode.GetValueKind() != JsonValueKind.String)
            throw new JsonException("Polje 'tipUlaznice' je obavezno ili neispravnog formata.");

        string tipUlazniceStr = tipUlazniceNode.GetValue<string>();

        if (!Enum.TryParse<TipUlaznice>(tipUlazniceStr, true, out var tip))
            throw new JsonException($"Nepoznat tip ulaznice: {tipUlazniceStr}");
        string jsonObject = node.ToJsonString();
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