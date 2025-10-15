using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Muzicki_festival.Entiteti
{
    public class JeClan
    {
        public virtual JeClanID ID { get; set; }

        public virtual DateTime? Datum_od { get; set; }
        public virtual DateTime? Datum_do { get; set; }
        public virtual string? Status { get; set; }

        public JeClan()
        {
            //eksplicitno Id mora da bude inicajalizovan na neku vrednost
            ID = new JeClanID();
        }

        public virtual Posetilac Posetilac
        {
            get => ID.Posetilac;
            set => ID.Posetilac = value;
        }

        public virtual Grupa Grupa
        {
            get => ID.Grupa;
            set => ID.Grupa = value;
        }
    }
}
