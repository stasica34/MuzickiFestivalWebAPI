using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Muzicki_festival.Entiteti
{
    public class Dogadjaj
    {
        //jednostavni property
        public virtual int ID { get; protected set; }
        //protected je da ne dozvoljavamo da se ID dodaje programski, jer je za generisanje ID  koristi baza
        public virtual string? NAZIV { get; set; }
        public virtual string? TIP { get; set; }
        public virtual string? OPIS { get; set; }
        public virtual DateTime DATUM_VREME_POCETKA { get; set; }
        public virtual DateTime DATUM_VREME_KRAJA { get; set; }

        //virtual properti - mogu da se overriduju, mora da budu virtual zbog mapiranja
        //konstructor
        //veza 1:n ka lokaciji
        public virtual Lokacija? Lokacija { get; set; }

        //veza n:m sa izvodjacem
        public virtual IList<Izvodjac> Izvodjaci { get; set; }
        //vwza n:m sa ulaznicom
        public virtual IList<Ulaznica> Ulaznica { get; set; }
        public Dogadjaj()
        {
            Izvodjaci = new List<Izvodjac>();
            Ulaznica = new List<Ulaznica>();
        }
    }
}
