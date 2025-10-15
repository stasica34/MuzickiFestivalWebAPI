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
        public int Id;
        public string Ime;
        public string Prezime;
        public string Email;
        public string Telefon;
        internal TipUlaznice UlaznicaTip;

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
        public int Id;
        public string Ime;
        public string Prezime;
        public string Email;
        public string Telefon;

        public UlaznicaBasic Ulaznica;

        public PosetilacBasic(int id, string ime, string prezime, string email, string telelfon, UlaznicaBasic ulaznica)
        {
            Id = id;
            Ime = ime;
            Prezime = prezime;
            Email = email;
            Telefon = telelfon;
            Ulaznica = ulaznica;
        }
    }
}
