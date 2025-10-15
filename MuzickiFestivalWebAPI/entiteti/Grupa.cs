using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Muzicki_festival.Entiteti
{
    public class Grupa
    {
        public virtual int ID_GRUPE { get; protected set; }
        public virtual string? NAZIV { get; set; }
        //1:n veza sa AgencijaOrganizator
        public virtual AgencijaOrganizator? AgencijaID { get; set; }
        public virtual IList<JeClan> JeClan { get; set; }
        public Grupa() {
            JeClan = new List<JeClan>();
        }
    }
}
