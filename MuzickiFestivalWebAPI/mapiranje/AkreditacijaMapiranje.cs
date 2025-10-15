using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Mapping;
using Muzicki_festival.Entiteti;
using FluentNHibernate.Mapping;
using NHibernate.Type;
namespace Muzicki_festival.Mapiranje
{
    public class AkreditacijaMapiranje:SubclassMap<Muzicki_festival.Entiteti.Akreditacija>
    {
        public AkreditacijaMapiranje()
        {
            Table("AKREDITACIJA");
            KeyColumn("ID_ULAZNICE");

            Map(x => x.TIP, "TIP").CustomType<EnumStringType<TipAkreditacije>>();
        }
    }
}
