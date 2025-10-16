using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Muzicki_festival.Entiteti
{
    public class Clan
    {
        public virtual int ID { get; set; }
        //visevrednosni atribut
        public virtual string ULOGA { get; set; }
        public virtual string IME { get; set; }
        public virtual string INSTRUMENT { get; set; }

        public virtual Bend BEND { get; set; }

        public Clan()
        {
        }
    }
}
