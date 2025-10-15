using FluentNHibernate.Mapping;
using Muzicki_festival.Entiteti;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace Muzicki_festival.DTOs
{
    public abstract class LokacijaView
    {
        public int Id { get; set; }
        public string? Opis { get; set; }
        public string? Naziv { get; set; }
        public string? Gps_koordinate { get; set; }  
        public int? Kapacitet { get; set; }
        
        public TipLokacije TipLokacije { get; set; }
        public LokacijaView(Lokacija l) 
        {
            Id = l.ID;
            Opis = l.OPIS;
            Naziv = l.NAZIV;
            Gps_koordinate = l.GPS_KOORDINATE;
            Kapacitet = l.MAX_KAPACITET;
            TipLokacije = l.TIP_LOKACIJE;
        }
        protected LokacijaView(int id, string opis, string naziv, string gps_koordinate, int kapacitet, TipLokacije tipLokacije) {
            Id = id;
            Opis = opis;
            Naziv = naziv;
            Gps_koordinate = gps_koordinate;
            Kapacitet = kapacitet;
            TipLokacije = tipLokacije;
        }
    }

    public class ZatvorenaLokacijaView : LokacijaView
    {
        public string? Tip_prostora { get; set; }
        public string? Klima { get; set; }
        public string? Dostupnost_sedenja { get; set; }

        public ZatvorenaLokacijaView(int id, string opis, string naziv, string gps_koordinate, int kapacitet, string tip_prostora, string klima, string dostupnost_sedenja)
            : base(id, opis, naziv, gps_koordinate, kapacitet, TipLokacije.ZATVORENA)
        {
            Tip_prostora = tip_prostora;
            Klima = klima;
            Dostupnost_sedenja = dostupnost_sedenja;
        }
    }

    public class OtvorenaLokacijaView : LokacijaView
    {
        public OtvorenaLokacijaView(int id, string opis, string naziv, string gps_koordinate, int kapacitet)
            : base(id, opis, naziv, gps_koordinate, kapacitet, TipLokacije.OTVORENA)
        {

        }
    }

    public class KombinovanaLokacijaView : LokacijaView
    {
        public string? Tip_prostora { get; set; }
        public string? Klima { get; set; }
        public string? Dostupnost_sedenja { get; set; }
        public KombinovanaLokacijaView(int id, string opis, string naziv, string gps_koordinate, int kapacitet, string tip_prostora, string klima, string dostupnost_sedenja)
            : base(id, opis, naziv, gps_koordinate, kapacitet, TipLokacije.KOMBINOVANA)
        {
            Tip_prostora = tip_prostora;
            Klima = klima;
            Dostupnost_sedenja = dostupnost_sedenja;
        }
    }

    //jedan nacin za serijalizaciju i deserijalizaciju apstraktne klase
    //[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
    //[JsonDerivedType(typeof(ZatvorenaLokacijaBasic), typeDiscriminator: "zatvorena")]
    //[JsonDerivedType(typeof(OtvorenaLokacijaBasic), typeDiscriminator: "otvorena")]
    //[JsonDerivedType(typeof(KombinovanaLokacijaBasic), typeDiscriminator: "kombinovana")]
    public abstract class LokacijaBasic
    {
        public int Id { get; set; }
        public string Opis { get; set; }
        public string Naziv { get; set; }
        public string Gps_koordinate { get; set; }
        public int Kapacitet { get; set; }

        public TipLokacije TipLokacije { get; set; }    

        // public IList<DogadjajView> Dogadjaji { get; set; }

        public LokacijaBasic() { }
        public LokacijaBasic(int id, string opis, string naziv, string gps_koordinate, int kapacitet, TipLokacije tipLokacije)
        {
            Id = id;
            Opis = opis;
            Naziv = naziv;
            Gps_koordinate = gps_koordinate;
            Kapacitet = kapacitet;
            TipLokacije = tipLokacije;
        }
    }

    public class ZatvorenaLokacijaBasic : LokacijaBasic
    {
        public string? Tip_prostora { get; set; }
        public string? Klima { get; set; }
        public string? Dostupnost_sedenja { get; set; }

        public ZatvorenaLokacijaBasic() { }
        public ZatvorenaLokacijaBasic(int id, string opis, string naziv, string gps_koordinate, int kapacitet, string tip_prostora, string klima, string dostupnost_sedenja)
            : base(id, opis, naziv, gps_koordinate, kapacitet, TipLokacije.ZATVORENA)
        {
            Tip_prostora = tip_prostora;
            Klima = klima;
            Dostupnost_sedenja = dostupnost_sedenja;
        }
    }
    public class OtvorenaLokacijaBasic : LokacijaBasic
    {
        public OtvorenaLokacijaBasic() { }
        public OtvorenaLokacijaBasic(int id, string opis, string naziv, string gps_koordinate, int kapacitet)
            : base(id, opis, naziv, gps_koordinate, kapacitet, TipLokacije.OTVORENA)
        {

        }
    }

    public class KombinovanaLokacijaBasic : LokacijaBasic
    {
        public string? Tip_prostora { get; set; }
        public string? Klima { get; set; }
        public string? Dostupnost_sedenja { get; set; }
        public KombinovanaLokacijaBasic () { }
        public KombinovanaLokacijaBasic(int id, string opis, string naziv, string gps_koordinate, int kapacitet, string tip_prostora, string klima, string dostupnost_sedenja)
            : base(id, opis, naziv, gps_koordinate, kapacitet, TipLokacije.KOMBINOVANA)
        {
            Tip_prostora = tip_prostora;
            Klima = klima;
            Dostupnost_sedenja = dostupnost_sedenja;
        }
    }
}
