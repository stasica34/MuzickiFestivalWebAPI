using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Muzicki_festival.Mapiranje
{
    /*
    public class OtvorenaLokacijaMapiranje:ClassMap<Muzicki_festival.Entiteti.OtvorenaLokacija>
    {
        public OtvorenaLokacijaMapiranje()
        {
            Table("OTVORENA_LOKACIJA");
            Id(x => x.ID, "OTVORENA_ID").GeneratedBy.Identity();

            //visevrednosni atribut
            HasMany(x => x.DOSTUPNOST_OPREME)
                .Table("DOSTUPNOST_OPREME")
                .KeyColumn("OTVORENA_ID")
                .Element("OPREMA")
                .Cascade.All();

        }
    }
    */
}
