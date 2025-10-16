using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Mapping;
using Muzicki_festival.Entiteti;
using NHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Muzicki_festival.Mapiranje
{
    public class GrupaMapiranje:ClassMap<Muzicki_festival.Entiteti.Grupa>
    {
        public GrupaMapiranje()
        {
            Table("GRUPA");
            Id(x => x.ID_GRUPE, "ID_GRUPE").GeneratedBy.TriggerIdentity();
            Map(x => x.NAZIV, "NAZIV");
            //mapiranje  1:N sa AgencijaOrganizator
            References(x => x.AgencijaID, "AGENCIJA_ORGANIZATOR_ID").LazyLoad().Cascade.None();

            //mapiranje
            HasMany(x=> x.Clanovi).KeyColumn("GRUPA_ID").LazyLoad().Cascade.None();
        }
    }
}
