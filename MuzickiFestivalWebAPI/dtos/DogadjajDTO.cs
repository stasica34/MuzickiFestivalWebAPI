using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Muzicki_festival.DTOs
{
    public class DogadjajView
    {
        public int Id { get; set; }
        public string Naziv {  get; set; }
        public string Tip { get; set; }
        public string Opis { get; set; }

        public DateTime DatumPocetka { get; set; }
        public DateTime DatumKraja { get; set; }

        public string LokacijaNaziv { get; set; }

        public DogadjajView(int id, string naziv, string tip, string opis, DateTime datumPocetka, DateTime datumKraja, string lokacijaNaziv)
        {
            Id = id;
            Naziv = naziv;
            Tip = tip;
            Opis = opis;
            DatumPocetka = datumPocetka;
            DatumKraja = datumKraja;
            LokacijaNaziv = lokacijaNaziv;
        }
    }

    public class DogadjajBasic
    {
        public int Id { get; set;}
        public string Naziv { get; set;}
        public string Tip { get; set;}
        public string Opis { get; set;}

        public DateTime DatumPocetka { get; set;}
        public DateTime DatumKraja { get; set;}

        public LokacijaBasic Lokacija { get; set;}
        public IList<IzvodjacBasic>? Izvodjaci { get; set;}

        public DogadjajBasic() { }
        public DogadjajBasic(int id, string naziv, string tip, string opis, DateTime datumPocetka, DateTime datumKraja, LokacijaBasic lokacija, IList<IzvodjacBasic> izvodjaci)
        {
            Id = id;
            Naziv = naziv;
            Tip = tip;
            Opis = opis;
            DatumPocetka = datumPocetka;
            DatumKraja = datumKraja;
            Lokacija = lokacija;
            Izvodjaci = izvodjaci;
        }
    }

}
