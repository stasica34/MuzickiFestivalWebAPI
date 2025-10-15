using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Mapping;
using Muzicki_festival.Entiteti;
using FluentNHibernate.Mapping;

namespace Muzicki_festival.Mapiranje
{
    //public class SoloUmetnikMapiranje: SubclassMap<Muzicki_festival.Entiteti.Solo_Umetnik>
    //{
    //    public SoloUmetnikMapiranje()
    //    {
    //        Table("SOLO_UMETNIK");
    //        KeyColumn("ID");
    //        Map(x => x.SVIRA_INSTRUMENT, "SVIRA_INSTRUMENT").Not.Nullable();
    //        Map(x => x.TIP_INSTRUMENTA, "TIP_INSTRUMENTA").Not.Nullable();

    //        //visevrednosni atribut
    //        HasMany(x => x.VOKALNE_SPOSOBNOSTI)
    //            .Table("VOKALNE_SPOSOBNOSTI")
    //            .KeyColumn(("ID_SOLOUMETNIK"))
    //            .Element("NAZIV")
    //            .Cascade.All();
    //    }
    //}
}
