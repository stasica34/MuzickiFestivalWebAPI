using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Muzicki_festival.Entiteti
{
    public class Posetilac
    {
        public virtual int ID { get; protected set; }
        public virtual string? IME { get; set; }
        public virtual string? PREZIME { get; set; }
        public virtual string? EMAIL { get; set; }
        public virtual string? Telefon { get; set; }
        public virtual Ulaznica? Ulaznica { get; set; }
        //veza n:m
        public virtual IList<JeClan> JeClan { get; set; } 
        public Posetilac()
        {
            //inicijalizacija liste ulaznica
            JeClan  = new List<JeClan>();
        }
        public override string ToString()
        {
            return IME + " " + PREZIME;
        }
    }
}
