using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Muzicki_festival.Entiteti
{
    public class AgencijaOrganizator
    {
        public virtual int ID { get; protected set; }
        public virtual string NAZIV { get; set; }
        public virtual string ADRESA { get; set; }

        //1:n veza sa grupama
        public virtual IList<Grupa> Grupe { get; set; }
        public AgencijaOrganizator() {
            Grupe = new List<Grupa>();
        }
    }
}
