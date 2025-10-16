using FluentNHibernate.Mapping;
using Muzicki_festival.Entiteti;
using NHibernate;
using NHibernate.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Muzicki_festival.Mapiranje
{
    public class IzvodjacMapiranje : ClassMap<Muzicki_festival.Entiteti.Izvodjac>
    {
        public IzvodjacMapiranje()
        {
            Table("IZVODJAC");
            Schema("S19184");
            Id(x => x.ID, "ID").GeneratedBy.SequenceIdentity("IZVODJAC_PK");
            Map(x => x.IME, "IME").Not.Nullable();
            Map(x => x.DRZAVA_POREKLA, "DRZAVA_POREKLA").Not.Nullable();
            Map(x => x.EMAIL, "EMAIL").Not.Nullable();
            Map(x => x.KONTAKT_OSOBA, "KONTAKT_OSOBA").Nullable();
            Map(x => x.TIP_IZVODJACA, "TIP_IZVODJACA").CustomType<EnumStringType<TipIzvodjaca>>().Not.Nullable();
            //mapiranje 1:n sa menadzerskom agencijom
            References(x => x.MenadzerskaAgencija).Column("MENADZERSKA_AGENCIJA_ID").LazyLoad().Cascade.None();
            //mapiranje n:m
            HasManyToMany(x => x.Dogadjaji)
              .Table("NASTUPA")
              .ParentKeyColumn("IZVODJAC_ID")
              .ChildKeyColumn("DOGADJAJ_ID")
              .Cascade.All();

            //svuda gde mi je nasladjivanje da uradim i sa discriminator sa tipom
            //visevrednosni atribut
            Map(x => x.TELEFON, "TELEFON");
            Map(x => x.Zanr, "ZANR");

            HasMany(x => x.Lista_tehnickih_zahteva)
               .Table("IZVODJAC_TEHNICKI_ZAHTEVI")
               .KeyColumn(("IZVODJAC_ID"))
               .Element("ZAHTEV")
               .Cascade.All();
        }

        public class SoloUmetnikMapiranje : SubclassMap<Muzicki_festival.Entiteti.Solo_Umetnik>
        {
            public SoloUmetnikMapiranje()
            {
                Table("SOLO_UMETNIK");
                KeyColumn("ID");
                Map(x => x.SVIRA_INSTRUMENT, "SVIRA_INSTRUMENT").Not.Nullable();
                Map(x => x.TIP_INSTRUMENTA, "TIP_INSTRUMENTA").Not.Nullable();

                //visevrednosni atribut
                HasMany(x => x.VOKALNE_SPOSOBNOSTI)
                    .Table("VOKALNE_SPOSOBNOSTI")
                    .KeyColumn(("ID_SOLOUMETNIK"))
                    .Element("NAZIV")
                    .Cascade.All();
            }
        }
        public class BendMapiranja : SubclassMap<Muzicki_festival.Entiteti.Bend>
        {
            public BendMapiranja()
            {
                Table("BEND");
                KeyColumn("ID");
                Map(x => x.BROJ_CLANOVA, "BROJ_CLANOVA").Not.Nullable();
                HasMany(x => x.Clanovi)
                    .KeyColumn("BEND_ID")//fk tabela u tabeli Clan
                    .Inverse()
                    .Cascade.All().LazyLoad();
            }

        }
    }
}
