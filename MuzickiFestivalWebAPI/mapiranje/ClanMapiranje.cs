using FluentNHibernate.Mapping;
using Muzicki_festival.Entiteti;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Muzicki_festival.Mapiranje
{
    public class ClanMapiranje:ClassMap<Muzicki_festival.Entiteti.Clan>
    {
        public ClanMapiranje()
        {
            Table("CLAN");
            Id(x => x.ID).Column("CLAN_ID").GeneratedBy.TriggerIdentity();
            Map(x => x.IME, "IME");
            Map(x => x.INSTRUMENT, "INSTRUMENT");
            Map(x => x.ULOGA).Column("ULOGA");

            References(x => x.BEND, "BEND_ID").LazyLoad().Cascade.All();
            //visevrednosni atribut
        }
    }
}
