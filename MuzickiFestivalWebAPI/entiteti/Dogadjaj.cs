using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Muzicki_festival.Entiteti
{
    public class Dogadjaj
    {
        public virtual int ID { get; protected set; }
        public virtual string NAZIV { get; set; }
        public virtual string TIP { get; set; }
        public virtual string OPIS { get; set; }
        public virtual DateTime DATUM_VREME_POCETKA { get; set; }
        public virtual DateTime DATUM_VREME_KRAJA { get; set; }
        public virtual Lokacija Lokacija { get; set; }
        public virtual IList<Izvodjac> Izvodjaci { get; set; }
        public virtual IList<Ulaznica> Ulaznica { get; set; }
        public Dogadjaj()
        {
            Izvodjaci = new List<Izvodjac>();
            Ulaznica = new List<Ulaznica>();
        }
    }
}
