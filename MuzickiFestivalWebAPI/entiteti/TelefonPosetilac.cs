using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Muzicki_festival.Entiteti
{
    public class TelefonPosetilac
    {
        public virtual string? TELEFON { get; set; }
        //veza ka posetiocu - visevrednsotiatirbut
        public virtual Posetilac? Posetioci { get; set; }
        public TelefonPosetilac() {
        }
    }
}
