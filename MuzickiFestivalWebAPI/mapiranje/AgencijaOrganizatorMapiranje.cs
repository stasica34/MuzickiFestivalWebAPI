using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Mapping;
using Muzicki_festival.Entiteti;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Muzicki_festival.Mapiranje
{
    public class AgencijaOrganizatorMapiranje:ClassMap<Muzicki_festival.Entiteti.AgencijaOrganizator>
    {
        public AgencijaOrganizatorMapiranje()
        {
            Table("AGENCIJA_ORGANIZATOR");
            Id(x => x.ID, "ID").GeneratedBy.TriggerIdentity();
            Map(x => x.NAZIV, "NAZIV").Not.Nullable().Unique();
            Map(x => x.ADRESA, "ADRESA").Not.Nullable();
            HasMany(x => x.Grupe)
                .KeyColumn("AGENCIJA_ORGANIZATOR_ID")//fk kolona u tabeli Grupa
                .Inverse()
                .Cascade.All();
        }
    }

}
