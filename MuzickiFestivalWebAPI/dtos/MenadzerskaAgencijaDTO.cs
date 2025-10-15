using FluentNHibernate.Mapping;
using Muzicki_festival.Entiteti;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Muzicki_festival.DTOs
{
    public class MenadzerskaAgencijaView
    {
        public int ID { get; set; }
        public string? Naziv { get; set; }
        public string? Adresa { get; set; }
        public string? KontaktOsoba { get; set; }

        public MenadzerskaAgencijaView(int id, string naziv, string adresa, string kontaktOsoba)
        {
            ID = id;
            Naziv = naziv;
            Adresa = adresa;
            KontaktOsoba = kontaktOsoba;
        }

        public override string ToString()
        {
            return Naziv;
        }
    }

    public class MenadzerskaAgencijaBasic
    {
        public int ID { get; set; }
        public string Naziv { get; set; }
        public string Adresa { get; set; }
        public string KontaktOsoba { get; set; }

        public IList<IzvodjacBasic> Izvodjaci = new List<IzvodjacBasic>();
        public IList<MenadzerskaAgencijaKontaktView>? KontaktPodaci { get; set; }

        public MenadzerskaAgencijaBasic() { }
        public MenadzerskaAgencijaBasic(int id, string naziv, string adresa, string kontaktOsoba, List<IzvodjacBasic> izvodjaci)
        {
            ID = id;
            Naziv = naziv;
            Adresa = adresa;
            KontaktOsoba = kontaktOsoba;
            Izvodjaci = izvodjaci;
        }
    }

    public class MenadzerskaAgencijaKontaktView
    {
        public int ID { get; set; }
        //[JsonPropertyName("tip")]
        // public TipKontakta TIP_KONTAKTA { get; set; }
        public TipKontakta Tip { get; set; }
        public string Vrednost { get; set; }

        public MenadzerskaAgencijaKontaktView() { }
        public MenadzerskaAgencijaKontaktView(int id, TipKontakta tip, string vrednost)
        {
            ID = id;
            Tip = tip;
            Vrednost = vrednost;
        }
    }

    public class MenadzerskaAgencijaKontaktBasic
    {
        public int ID { get; set; }
        public TipKontakta TIP_KONTAKTA { get; set; }
        public string Vrednost { get; set; }
        public MenadzerskaAgencijaBasic MenadzerkaAgencija { get; set; }

        public MenadzerskaAgencijaKontaktBasic() { }
        public MenadzerskaAgencijaKontaktBasic(int id, TipKontakta tip, string vrednost, MenadzerskaAgencijaBasic menadzerskaAgencija)
        {
            ID = id;
            TIP_KONTAKTA = tip;
            Vrednost = vrednost;
            MenadzerkaAgencija = menadzerskaAgencija;
        }
    }
}
