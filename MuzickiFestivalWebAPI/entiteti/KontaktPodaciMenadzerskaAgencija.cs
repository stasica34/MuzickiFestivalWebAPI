using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Muzicki_festival.Entiteti
{
    public class KontaktPodaciMenadzerskaAgencija
    {
        public virtual int MENADZERSKA_AGENCIJA_ID { get; set; }
        public virtual string? EMAIL { get; set; }
        public virtual string? TELEFON { get; set; }
        public virtual MenadzerskaAgencija? AGENCIJA { get; set; }

        public KontaktPodaciMenadzerskaAgencija() { }
        public override string ToString()
        {
            return $"Email: {EMAIL}, Tel: {TELEFON}";
        }

    }
}
