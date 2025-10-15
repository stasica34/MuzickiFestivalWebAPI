using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;
using Muzicki_festival.Entiteti;
namespace Muzicki_festival.Mapiranje
{
    //public class BendMapiranja:SubclassMap<Muzicki_festival.Entiteti.Bend>
    //{
    //    public BendMapiranja()
    //    {
    //        Table("BEND");
    //        KeyColumn("ID");
    //        Map(x => x.BROJ_CLANOVA, "BROJ_CLANOVA").Not.Nullable();
    //        HasMany(x => x.Clanovi)
    //            .KeyColumn("BEND_ID")//fk tabela u tabeli Clan
    //            .Inverse()
    //            .Cascade.All().LazyLoad();
    //    }
    //}
}
