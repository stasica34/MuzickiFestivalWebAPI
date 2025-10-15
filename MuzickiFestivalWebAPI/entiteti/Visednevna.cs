using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Muzicki_festival.Entiteti
{
    public class Visednevna : Ulaznica
    {
        public virtual int BROJ_DANA { get; set; }
        //visevrednsoti atributi - lista dana
        public virtual IList<DateTime> Dani { get; set; }
        public Visednevna()
        {
            Dani = new List<DateTime>();
        }
    }
}
