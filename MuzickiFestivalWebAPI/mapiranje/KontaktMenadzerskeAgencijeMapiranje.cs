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
    public class KontaktMenadzerskeAgencijeMapiranje : ClassMap<Muzicki_festival.Entiteti.KontaktPodaciMenadzerskeAgencije>
    {
        public KontaktMenadzerskeAgencijeMapiranje()
        {
            Table("KONTAKT_PODACI_MENADZERSKE");
            Id(x => x.ID, "ID").GeneratedBy.TriggerIdentity();
            Map(x => x.TIP_KONTAKTA, "TIP_KONTAKTA").CustomType<EnumStringType<TipKontakta>>().Not.Nullable();
            Map(x => x.VREDNOST, "VREDNOST").Not.Nullable();
            References(x => x.MENADZERSKA_AGENCIJA).Column("MENADZERSKA_AGENCIJA_ID").LazyLoad().Cascade.None();
        }

    }
}
