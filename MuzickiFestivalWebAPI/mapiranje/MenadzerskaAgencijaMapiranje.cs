using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Muzicki_festival.Mapiranje
{
    public class MenadzerskaAgencijaMapiranje : ClassMap<Muzicki_festival.Entiteti.MenadzerskaAgencija>
    {
        public MenadzerskaAgencijaMapiranje()
        {
            Table("MENADZERSKA_AGENCIJA");
            Id(x => x.ID, "ID").GeneratedBy.Identity();
            Map(x => x.NAZIV, "NAZIV").Not.Nullable();
            Map(x => x.ADRESA, "ADRESA").Not.Nullable();
            Map(x => x.KONTAKT_OSOBA, "KONTAKT_OSOBA").Not.Nullable();
            HasMany(x => x.Izvodjaci)
                .KeyColumn("MENADZERSKA_AGENCIJA_ID") //FK kolona u tabeli Izvodjac
                .Inverse()
                .Cascade.All();
            HasMany(x => x.KONTAKT_PODACI)
                .KeyColumn("MENADZERSKA_AGENCIJA_ID")
                .Inverse()
                .Cascade.All();
        }
    }
}
