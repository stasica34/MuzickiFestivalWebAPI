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
            //mapiranje tabele
            //ovo moze da se izostavi ukoliko se domenski entitet i baza zovu isto to je implicitno mapiranje
            //ali preporuka mapirati eksplicitno
            Table("DOGADJAJ");

            //mapiranje primarnog kljuca - on se mapira posebno, i mora s ereci kako ce se mapirati primarni kljuc - najcesce 
            //ovde sa trigerom, i ako je sa trigerom u bazi onda mora i ovde
            //kada imamo prirodni kljuc - assigned - najcesce
            //sequence - kroz sekvecnu direktno, ne kroz triger - najcesce 
            //Identity - autonumber za genrisanje surogat kljuceva
            //increment - nije bas najsjajnije rsenje
            Id(x => x.ID, "ID").GeneratedBy.TriggerIdentity();


            //mapiranje svojstva
            //adresa propertija iz tabela sa bazom
            //ukoliko su properti zovu isto kao kolenu, moze naziv kolene da se izostavi
            //ne mora svi properti da se mapira, mapira se sve sa onim iz baze, ili ono sto je posebno za realan svet

            Map(x => x.NAZIV, "NAZIV").Not.Nullable();
            Map(x => x.TIP, "TIP").Not.Nullable();
            Map(x => x.OPIS, "OPIS").Nullable();
            Map(x => x.DATUM_VREME_POCETKA, "DATUM_VREME_POCETKA").Not.Nullable();
            Map(x => x.DATUM_VREME_KRAJA, "DATUM_VREME_KRAJA").Not.Nullable();

            //mapiranje 1:n veze sa lokacijom
            References(x => x.Lokacija)
                .Column("LOKACIJA_ID")
                .Not.Nullable()
                .Cascade.None();

            //mapiranje n:m sa izvodjacem
            HasManyToMany(x => x.Izvodjaci)
                .Table("NASTUPA")
                .ParentKeyColumn("DOGADJAJ_ID")//grupa
                .ChildKeyColumn("IZVODJAC_ID")//posetilac
                .Cascade.All()
                .Inverse();//inverzna strana veze
                           //inverzija moze da bude na obe strane
                           //ali biramo tamo inverziju gde nam vise odgovara
            //mapiranje n:m sa ulaznicom
            HasManyToMany(x => x.Ulaznica)
                .Table("OMOGUCAVA_ULAZ_NA")
                .ParentKeyColumn("DOGADJAJ_ID")
                .ChildKeyColumn("ID_ULAZNICE")
                .Cascade.All();
        }
    }
}