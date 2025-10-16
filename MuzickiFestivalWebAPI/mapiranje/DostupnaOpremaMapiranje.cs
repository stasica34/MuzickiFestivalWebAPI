using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Muzicki_festival.Mapiranje
{
    public class DostupnaOpremaMapiranje : ClassMap<Muzicki_festival.Entiteti.DostupnaOprema>
    {
        public DostupnaOpremaMapiranje()
        {
            Table("DOSTUPNOST_OPREME");
            Id(x => x.ID).GeneratedBy.TriggerIdentity();
            Map(x => x.NAZIV, "NAZIV").Not.Nullable();
            References(x => x.Lokacija)
                .Column("LOKACIJA_ID")
                .LazyLoad()
                .Not.Nullable()
                .Cascade.None();
        }
    }
}
