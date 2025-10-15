using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Muzicki_festival.Entiteti;

namespace Muzicki_festival.Mapiranje
{
    public class JednodnevnaMapiranje:SubclassMap<Muzicki_festival.Entiteti.Jednodnevna>
    {
        public JednodnevnaMapiranje()
        {
            Table("JEDNODNEVNA");
            //primarni kljuc jednodnvene ulaznice
            ///je primarni kljuc ulaznice
            //nemamo razloga da opet deifinisemo primarni kljc
            //je da je kljucna kolicna koja je
            KeyColumn("ID_ULAZNICE");
            //ostali atributi u tabeli jednodnevna
            Map(x => x.DAN_VAZENJA, "DAN_VAZENJA").Not.Nullable();
        }
    }
}
