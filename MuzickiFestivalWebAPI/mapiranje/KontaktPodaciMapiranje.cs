using FluentNHibernate.Mapping;
using Muzicki_festival.Entiteti;

public class KontaktPodaciMapiranje : ClassMap<KontaktPodaciMenadzerskaAgencija>
{
    public KontaktPodaciMapiranje()
    {
        Table("KONTAKT_PODACI");

        Id(x => x.MENADZERSKA_AGENCIJA_ID, "MENADZERSKA_AGENCIJA_ID").GeneratedBy.Assigned(); 
        Map(x => x.EMAIL, "EMAIL").Nullable();
        Map(x => x.TELEFON, "TELEFON").Nullable();

        References(x => x.AGENCIJA)
            .Column("MENADZERSKA_AGENCIJA_ID")
            .Not.Insert()
            .Not.Update()
            .LazyLoad();
    }
}
