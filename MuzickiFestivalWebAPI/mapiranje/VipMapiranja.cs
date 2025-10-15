using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Muzicki_festival.Mapiranje
{
    public class VipMapiranja:SubclassMap<Muzicki_festival.Entiteti.Vip>
    {
        public VipMapiranja()
        {
            Table("VIP");
            KeyColumn("ID_ULAZNICE");
            //visevrednosni atribut
            HasMany(x => x.Pogodnosti)
                .Table("VIP_POGODNOSTI")
                .KeyColumn(("ID_ULAZNICE"))
                .Element("POGODNOST")//naziv tabele iz baze
                .Cascade.All();
        }
    }
}
