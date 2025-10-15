using Muzicki_festival.Entiteti;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Muzicki_festival.DTOs
{
    public class DogadjajView
    {
        private string naziv1;
        private string naziv2;

        public int Id { get; set; }
        public string? Naziv { get; set; }
        public string? Tip { get; set; }
        public string? Opis { get; set; }
        public DateTime DatumPocetka { get; set; }
        public DateTime DatumKraja { get; set; }

        public DogadjajView(Dogadjaj st)
        {
            this.Id = st.ID;
            this.Naziv = st.NAZIV;
            this.Tip = st.TIP;
            this.Opis = st.OPIS;
            this.DatumPocetka = st.DATUM_VREME_POCETKA;
            this.DatumKraja = st.DATUM_VREME_KRAJA;
        }
        public DogadjajView(int id, string naziv1, string tip, string opis, DateTime datumPocetka, DateTime datumKraja, string naziv2)
        {
            Id = id;
            this.naziv1 = naziv1;
            Tip = tip;
            Opis = opis;
            DatumPocetka = datumPocetka;
            DatumKraja = datumKraja;
            this.naziv2 = naziv2;
        }
    }

    public class DogadjajBasic
    {
        //mora sa get i set da bi se dodavalo preko API-ja
        public DogadjajBasic Id { get; set; }
        public string Naziv { get; set; }
        public string Tip { get; set; }
        public string Opis { get; set; }
        public DateTime DatumPocetka { get; set; }
        public DateTime DatumKraja { get; set; }
        public LokacijaBasic Lokacija { get; set; }

        public DogadjajBasic(string naziv, string tip, string opis, DateTime datumPocetka, DateTime datumKraja, LokacijaBasic lokacija)
        {
            Naziv = naziv;
            Tip = tip;
            Opis = opis;
            DatumPocetka = datumPocetka;
            DatumKraja = datumKraja;
            Lokacija = lokacija;
        }
    }

}
