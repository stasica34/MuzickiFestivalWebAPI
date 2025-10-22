using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Muzicki_festival.Entiteti
{
    public enum TipLokacije
    {
        ZATVORENA,
        OTVORENA,
        KOMBINOVANA
    }

    public abstract class Lokacija
    {
        public virtual int ID { get; set; }
        public virtual string? OPIS { get; set; }
        public virtual int MAX_KAPACITET { get; set; }
        public virtual string NAZIV { get; set; }
        public virtual string GPS_KOORDINATE { get; set; }
        public virtual TipLokacije TIP_LOKACIJE { get; set; }
        public virtual IList<Dogadjaj> Dogadjaji { get; set; }

        // Ovo koriste samo kombinovana i otvorena ali mora da bude ovde jer u suprotnom ne radi mapiranje
        public virtual IList<DostupnaOprema> DOSTUPNA_OPREMA { get; set; }
        public Lokacija()
        {
            Dogadjaji = new List<Dogadjaj>();
        }
        public override string ToString()
        {
            return $"{NAZIV} - {GPS_KOORDINATE}";
        }
    }

    public class ZatvorenaLokacija : Lokacija
    {
        public virtual string TIP_PROSTORA { get; set; }
        public virtual string KLIMA { get; set; }
        public virtual string DOSTUPNOST_SEDENJA { get; set; }

        public ZatvorenaLokacija()
            : base()
        {
            DOSTUPNA_OPREMA = new List<DostupnaOprema>().AsReadOnly(); // da osigura da zatvorena lokacija nema opremu
        }
    }

    public class OtvorenaLokacija : Lokacija
    {
        public OtvorenaLokacija()
            : base()
        {
            DOSTUPNA_OPREMA = new List<DostupnaOprema>();
        }
    }

    public class KombinovanaLokacija : Lokacija
    {
        public virtual string TIP_PROSTORA { get; set; }
        public virtual string KLIMA { get; set; }
        public virtual string DOSTUPNOST_SEDENJA { get; set; }

        public KombinovanaLokacija()
            : base()
        {
            DOSTUPNA_OPREMA = new List<DostupnaOprema>();
        }
    }
}
