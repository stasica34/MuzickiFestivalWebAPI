using FluentNHibernate.Mapping;
using Muzicki_festival.Entiteti;
using NHibernate.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Muzicki_festival.Mapiranje
{
    public class LokacijaMapiranja : ClassMap<Muzicki_festival.Entiteti.Lokacija>
    {
        public LokacijaMapiranja()
        {
            Table("LOKACIJA");
            Id(x => x.ID).GeneratedBy.Identity();
            Map(x => x.OPIS, "OPIS").Not.Nullable();
            Map(x => x.MAX_KAPACITET, "MAX_KAPACITET").Nullable();
            Map(x => x.NAZIV, "NAZIV").Not.Nullable();
            Map(x => x.GPS_KOORDINATE, "GPS_KOORDINATE").Not.Nullable();
            Map(x => x.TIP_LOKACIJE).CustomType<EnumStringType<TipLokacije>>().Not.Nullable();
            HasMany(x => x.Dogadjaji)
                .KeyColumn("LOKACIJA_ID")
                .Inverse()
                .Cascade.All()
                .LazyLoad();
            HasMany(x => x.DOSTUPNA_OPREMA)
                .KeyColumn("LOKACIJA_ID")
                .Inverse()
                .Cascade.All()
                .LazyLoad();
        }
    }

    public class ZatvorenaLokacijaMapiranje : SubclassMap<Muzicki_festival.Entiteti.ZatvorenaLokacija>
    {
        public ZatvorenaLokacijaMapiranje()
        {
            Table("ZATVORENA_LOKACIJA");
            KeyColumn("ID");
            Map(x => x.TIP_PROSTORA, "TIP_PROSTORA").Not.Nullable();
            Map(x => x.KLIMA, "KLIMA").Not.Nullable();
            Map(x => x.DOSTUPNOST_SEDENJA, "DOSTUPNOST_SEDENJA").Not.Nullable();
        }
    }
    public class OtvorenaLokacijaMapiranje : SubclassMap<Muzicki_festival.Entiteti.OtvorenaLokacija>
    {
        public OtvorenaLokacijaMapiranje()
        {
            Table("OTVORENA_LOKACIJA");
            KeyColumn("ID");
        }
    }
    public class KombinovanaLokacijaMapiranje : SubclassMap<Muzicki_festival.Entiteti.KombinovanaLokacija>
    {
        public KombinovanaLokacijaMapiranje()
        {
            Table("KOMBINOVANA_LOKACIJA");
            KeyColumn("ID");
            Map(x => x.TIP_PROSTORA, "TIP_PROSTORA").Not.Nullable();
            Map(x => x.KLIMA, "KLIMA").Not.Nullable();
            Map(x => x.DOSTUPNOST_SEDENJA, "DOSTUPNOST_SEDENJA").Not.Nullable();
        }
    }
}
