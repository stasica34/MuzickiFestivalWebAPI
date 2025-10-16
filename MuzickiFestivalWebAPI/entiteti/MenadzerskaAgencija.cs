using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Muzicki_festival.Entiteti
{
    public class MenadzerskaAgencija
    {
        public virtual int ID { get; set; }
        public virtual string NAZIV { get; set; }
        public virtual string ADRESA { get; set; }
        public virtual string KONTAKT_OSOBA { get; set; }
        //poverizvanj 1:n sa izvodjacem
        public virtual IList<Izvodjac> Izvodjaci { get; set; }

        public virtual IList<KontaktPodaciMenadzerskeAgencije> KONTAKT_PODACI { get; set; }
        public MenadzerskaAgencija()
        {
            Izvodjaci = new List<Izvodjac>();
            KONTAKT_PODACI = new List<KontaktPodaciMenadzerskeAgencije>();
        }
        public override string ToString()
        {
            return NAZIV;
        }
    }
}
