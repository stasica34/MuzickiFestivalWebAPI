using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;
using Muzicki_festival.Entiteti;

namespace Muzicki_festival.Mapiranje
{
    public class JeCLanMapiranje:ClassMap<Muzicki_festival.Entiteti.JeClan>
    {
        public JeCLanMapiranje()
        {
            Table("JE_CLAN");

            CompositeId<JeClanID>(x => x.ID)
                .KeyReference(x => x.Posetilac, "POSETILAC_ID")
                .KeyReference(x => x.Grupa, "ID_GRUPE");

            Map(x => x.Datum_od, "DATUM_OD");
            Map(x => x.Datum_do, "DATUM_DO");
            Map(x => x.Status, "STATUS");
        }
    }
}
