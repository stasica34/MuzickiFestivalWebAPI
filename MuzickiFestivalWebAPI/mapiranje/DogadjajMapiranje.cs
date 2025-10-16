using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;
using Muzicki_festival.Entiteti;

namespace Muzicki_festival.Mapiranje
{
    public class DogadjajMapiranje : ClassMap<Muzicki_festival.Entiteti.Dogadjaj>
    {
        public DogadjajMapiranje()
        {

            Table("DOGADJAJ");

            Id(x => x.ID, "ID").GeneratedBy.TriggerIdentity();

            Map(x => x.NAZIV, "NAZIV").Not.Nullable();
            Map(x => x.TIP, "TIP").Not.Nullable();
            Map(x => x.OPIS, "OPIS").Nullable();
            Map(x => x.DATUM_VREME_POCETKA, "DATUM_VREME_POCETKA").Not.Nullable();
            Map(x => x.DATUM_VREME_KRAJA, "DATUM_VREME_KRAJA").Not.Nullable();

            References(x => x.Lokacija)
                .Column("LOKACIJA_ID")
                .Not.Nullable()
                .Cascade.None();

            HasManyToMany(x => x.Izvodjaci)
                .Table("NASTUPA")
                .ParentKeyColumn("DOGADJAJ_ID")
                .ChildKeyColumn("IZVODJAC_ID")
                .Cascade.None();

            HasMany(x => x.Ulaznica)
                .KeyColumn("DOGADJAJ_ID")
                .Cascade.AllDeleteOrphan()
                .Inverse()
                .LazyLoad();
        }
    }
}