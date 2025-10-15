using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Muzicki_festival.Entiteti
{
    public class JeClanID
    {
        public virtual Posetilac? Posetilac { get; set; }
        public virtual Grupa? Grupa { get; set; }

        public override bool Equals(object obj)
        {
            if(object.ReferenceEquals(this, obj)) 
                return true;
            //da li su this i prosledjen paramet isti po referenci
            if (obj.GetType() != typeof(JeClanID))
                return false;

            JeClan receiveobject = (JeClan)obj;

            if ((Posetilac.ID == receiveobject.Posetilac.ID) &&
                Grupa.ID_GRUPE == receiveobject.Grupa.ID_GRUPE)
                return true;
            else
                return false;

            //var other = obj as JeClanID;
            //if (other == null)
            //    return false;

            //return Equals(Posetilac, other.Posetilac) &&
            //       Equals(Grupa, other.Grupa);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
