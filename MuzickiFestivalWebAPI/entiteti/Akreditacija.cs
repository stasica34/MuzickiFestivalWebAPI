using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Muzicki_festival.Entiteti
{
    public enum TipAkreditacije
    {
        SPONZOR,
        PRESS,
        PARTNER
    }

    public class Akreditacija : Ulaznica
    {
        public virtual TipAkreditacije TIP { get; set; }
        public Akreditacija() { }
    }
}

