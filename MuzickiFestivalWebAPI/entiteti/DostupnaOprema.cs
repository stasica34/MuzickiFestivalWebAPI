using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Muzicki_festival.Entiteti
{
    public class DostupnaOprema
    {
        public virtual int ID { get; set; }
        public virtual string NAZIV { get; set; }
        public virtual Lokacija Lokacija { get; set; }
    }
}
