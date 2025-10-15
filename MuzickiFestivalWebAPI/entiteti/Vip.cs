using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Muzicki_festival.Entiteti
{
    public class Vip:Ulaznica
    {
        //povezivanje visevrednostnog atributa
        public virtual IList<string> Pogodnosti { get; set; } //visevrednostni ka VIP
        public Vip() 
        {
            Pogodnosti = new List<string>();
        }   
    }
}
