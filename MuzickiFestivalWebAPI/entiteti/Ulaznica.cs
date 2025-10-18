using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Muzicki_festival.Entiteti
{
    public enum TipUlaznice
    {
        JEDNODNEVNA,
        VISEDNEVNA,
        VIP,
        AKREDITACIJA
    }

    public class Ulaznica
    {
        public virtual int ID_ULAZNICE { get; protected set; }
        public virtual float OSNOVNA_CENA { get; set; }
        public virtual string NACIN_PLACANJA { get; set; }
        public virtual DateTime DATUM_KUPOVINE { get; set; }
        public virtual TipUlaznice TIP_ULAZNICE { get; set; }
        //veza 1:1 sa posetiocem
        public virtual Posetilac KUPAC_ID { get; set; }

        public virtual Dogadjaj Dogadjaj { get; set; }
        public Ulaznica()
        {

        }
    }
}
