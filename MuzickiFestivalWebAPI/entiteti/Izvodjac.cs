
using System.Collections.Generic;

namespace Muzicki_festival.Entiteti
{
    public enum IzvodjacTip
    {
        SOLO_UMETNIK ,
        BEND 
    }

    public class Izvodjac
    {
        public virtual int ID { get; set; }
        public virtual string IME { get; set; }
        public virtual string DRZAVA_POREKLA { get; set; }

        public virtual string EMAIL { get; set; }

        public virtual string KONTAKT_OSOBA { get; set; }

        public virtual IzvodjacTip TIP_IZVODJACA { get; set; }

        public virtual string TELEFON { get; set; }
        public virtual string Zanr { get; set; }
        public virtual IList<string> Lista_tehnickih_zahteva { get; set; }
        public virtual MenadzerskaAgencija MenadzerskaAgencija { get; set; }
        //veza n:m sa dogadjem
        public virtual IList<Dogadjaj> Dogadjaji { get; set; }

        public Izvodjac()
        {
            Dogadjaji = new List<Dogadjaj>();
            Lista_tehnickih_zahteva = new List<string>();
        }
        public override string ToString()
        {
            return $"{IME} ({DRZAVA_POREKLA})";
        }
    }

    public class Bend : Izvodjac
    {
        public virtual int BROJ_CLANOVA { get; set; }

        //povezivanje 1:n clanovi
        public virtual IList<Clan> Clanovi { get; set; }
        public Bend()
        {
            Clanovi = new List<Clan>();
        }

    }
    public class Solo_Umetnik : Izvodjac
    {
        public virtual string SVIRA_INSTRUMENT { get; set; }
        public virtual string TIP_INSTRUMENTA { get; set; }
        public virtual IList<string> VOKALNE_SPOSOBNOSTI { get; set; } //visevrednostni atribut
        public Solo_Umetnik()
        {
            VOKALNE_SPOSOBNOSTI = new List<string>();
        }
    }
}
