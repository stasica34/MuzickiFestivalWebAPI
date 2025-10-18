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
        private IList<DateTime> _dani;
        public virtual IList<DateTime> Dani
        {
            get => _dani;
            set
            {
                _dani = value;
                BROJ_DANA = _dani?.Count ?? 0;
            }
        }
        public Visednevna()
        {
            Dani = new List<DateTime>();
            BROJ_DANA = Dani.Count;
        }
    }
}
