using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Muzicki_festival.Entiteti
{
    //izvede iz klase ulaznica- specijalizacija
    public class Jednodnevna: Ulaznica
    {
        public virtual DateTime DAN_VAZENJA { get; set; }

        public Jednodnevna() { }
    }
}
