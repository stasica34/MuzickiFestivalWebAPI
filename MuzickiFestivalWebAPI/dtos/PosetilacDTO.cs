using Muzicki_festival.Entiteti;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public TipUlaznice? UlaznicaTip { get; set; } // da ne menjam konstrukor valjda na jedno mesto treba ovaj podatak samo
    
        public PosetilacView(int id, string ime, string prezime, string email, string telefon)
        {
            Id = id;
            Ime = ime;
            Prezime = prezime;
            Email = email;
            Telefon = telefon;
        }
    }

    public class PosetilacBasic
    {
        public int Id { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string Email { get; set; }
        public string Telefon { get; set; }

        public UlaznicaBasic Ulaznica { get; set; }
        public GrupaBasic? Grupa { get; set; }
        public PosetilacBasic() { }

        public PosetilacBasic(int id, string ime, string prezime, string email, string telelfon, UlaznicaBasic ulaznica, GrupaBasic grupa)
        {
            Id = id;
            Ime = ime;
            Prezime = prezime;
            Email = email;
            Telefon = telelfon;
            Ulaznica = ulaznica;
            Grupa = grupa;
        }
    }
}
