using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;
using Muzicki_festival.Entiteti;
using NHibernate.Type;

namespace Muzicki_festival.Mapiranje
{
    public class UlaznicaMapiranjaL:ClassMap<Muzicki_festival.Entiteti.Ulaznica>
    {
        public UlaznicaMapiranjaL()
        {
            Table("ULAZNICA");
            Id(x => x.ID_ULAZNICE, "ID_ULAZNICE").GeneratedBy.SequenceIdentity("ULAZNICA_PK"); //mora ovako
            Map(x => x.OSNOVNA_CENA, "OSNOVNA_CENA").Not.Nullable().Check("OSNOVNA_CENA >= 0");
            Map(x => x.NACIN_PLACANJA, "NACIN_PLACANJA").Not.Nullable().Check("NACIN_PLACANJA IN ('Gotovina', 'Kartica', 'Online')");
            Map(x => x.DATUM_KUPOVINE, "DATUM_KUPOVINE").Not.Nullable();
            Map(x => x.TIP_ULAZNICE, "TIP_ULAZNICE").CustomType<EnumStringType<TipUlaznice>>().Not.Nullable().Check("TIP_ULAZNICE IN ('JEDNODNEVNA', 'VISEDNEVNA', 'VIP', 'AKREDITACIJA')");

            References(x => x.KUPAC_ID)
                    .Column("KUPAC_ID")
                    .UniqueKey("UK_ULAZNICA_POSETIOC")
                    .Not.Nullable()
                    .Cascade.All()
                    .LazyLoad();


            References(x => x.Dogadjaj, "DOGADJAJ_ID").Cascade.None().LazyLoad();
        }
    }

    public class VipMapiranja : SubclassMap<Muzicki_festival.Entiteti.Vip>
    {
        public VipMapiranja()
        {
            Table("VIP");
            KeyColumn("ID_ULAZNICE");

            HasMany(x => x.Pogodnosti)
                .Table("VIP_POGODNOSTI")
                .KeyColumn(("ID_ULAZNICE"))
                .Element("POGODNOST")
                .Cascade.All();
        }
    }

    public class JednodnevnaMapiranje : SubclassMap<Muzicki_festival.Entiteti.Jednodnevna>
    {
        public JednodnevnaMapiranje()
        {
            Table("JEDNODNEVNA");
            KeyColumn("ID_ULAZNICE");

            Map(x => x.DAN_VAZENJA, "DAN_VAZENJA").Not.Nullable();
        }
    }

    public class VisednevnaMapiranje : SubclassMap<Muzicki_festival.Entiteti.Visednevna>
    {
        public VisednevnaMapiranje()
        {
            Table("VISEDNEVNA");
            KeyColumn("ID_ULAZNICE");
            Map(x => x.BROJ_DANA, "BROJ_DANA").Not.Nullable().Check("BROJ_DANA > 1"); ;
            HasMany(x => x.Dani).
                Table("VISEDNEVNA_DANI")
                .KeyColumn(("ID_ULAZNICE"))
                .Element("DAN_VAZENJA")
                .Cascade.AllDeleteOrphan();
        }
    }

    public class AkreditacijaMapiranje : SubclassMap<Muzicki_festival.Entiteti.Akreditacija>
    {
        public AkreditacijaMapiranje()
        {
            Table("AKREDITACIJA");
            KeyColumn("ID_ULAZNICE");

            Map(x => x.TIP, "TIP").CustomType<EnumStringType<TipAkreditacije>>().Check("TIP IN ('SPONZOR', 'PRESS', 'PARTNER')"); ;
        }
    }
}
