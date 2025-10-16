using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Muzicki_festival.Entiteti
{
    public enum TipKontakta
    {
        EMAIL,
        TELEFON
    }

    public class KontaktPodaciMenadzerskeAgencije
    {
        public virtual int ID { get; set; }
        public virtual TipKontakta TIP_KONTAKTA { get; set; }
        public virtual string VREDNOST { get; set; }
        public virtual MenadzerskaAgencija MENADZERSKA_AGENCIJA { get; set; }
    }
}
