using Muzicki_festival.DTOs;
using Muzicki_festival.Entiteti;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Muzicki_festival.DTOs
{
    public class PosetilacView
    {
        public int Id { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string Email { get; set; }
        public string Telefon { get; set; }

        public TipUlaznice UlaznicaTip { get; set; } // da ne menjam konstrukor valjda na jedno mesto treba ovaj podatak samo
    
        public PosetilacView(int id, string ime, string prezime, string email, string telefon)
        {
            Id = id;
            Ime = ime;
            Prezime = prezime;
            Email = email;
            Telefon = telefon;
        }
    }

}
public class PosetilacBasic
{
    public int Id { get; set; }
    public string Ime { get; set; }
    public string Prezime { get; set; }
    public string? Email { get; set; }
    public string? Telefon { get; set; }

    [JsonIgnore]
    public UlaznicaBasic? Ulaznica { get; set; }

    [JsonPropertyName("ulaznica")]
    public string? UlaznicaTip
    {
        get => Ulaznica?.Tip.ToString();
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                Ulaznica = null;
                return;
            }

            if (!Enum.TryParse<TipUlaznice>(value, true, out var tip))
                throw new JsonException($"Nepoznat tip ulaznice: {value}");

            DogadjajBasic defaultDogadjaj = new DogadjajBasic(); 

            Ulaznica = tip switch
            {
                TipUlaznice.JEDNODNEVNA => new JednodnevnaBasic(0, 0, "", DateTime.Now, defaultDogadjaj, DateTime.Now),
                TipUlaznice.VISEDNEVNA => new ViseDnevnaBasic(0, 0, "", DateTime.Now, defaultDogadjaj, new List<DateTime>()),
                TipUlaznice.VIP => new VIPBasic(0, 0, "", DateTime.Now, defaultDogadjaj, new List<string>()),
                TipUlaznice.AKREDITACIJA => new AkreditacijaBasic(0, 0, "", DateTime.Now, defaultDogadjaj, TipAkreditacije.PRESS),
                _ => throw new JsonException($"Nepoznat tip ulaznice: {value}")
            };
        }
    }

    public GrupaBasic? Grupa { get; set; }

    public PosetilacBasic() { }

    public PosetilacBasic(int id, string ime, string prezime, string email, string telefon, UlaznicaBasic ulaznica, GrupaBasic grupa)
    {
        Id = id;
        Ime = ime;
        Prezime = prezime;
        Email = email;
        Telefon = telefon;
        Ulaznica = ulaznica;
        Grupa = grupa;
    }
}
