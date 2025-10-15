using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Muzicki_festival.Mapiranje
{
    public class VisednevnaMapiranje:SubclassMap<Muzicki_festival.Entiteti.Visednevna>
    {
        public VisednevnaMapiranje()
        {
            Table("VISEDNEVNA");
            KeyColumn("ID_ULAZNICE");
            Map(x => x.BROJ_DANA, "BROJ_DANA");
            //visevrednosni atribut
            HasMany(x => x.Dani).
                Table("VISEDNEVNA_DANI")
                .KeyColumn(("ID_ULAZNICE"))
                .Element("DAN_VAZENJA").
                Cascade.All();
        }
    }
}
