using Muzicki_festival.Entiteti;
using NHibernate.Mapping.ByCode.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Muzicki_festival.DTOs
{
    public abstract class IzvodjacView
    {
        public int Id { get;set; }
        public string? Ime { get; set; } 
        public string? Drzava_porekla { get; set; }
        public string? Email { get; set;}
        public string? Kontakt_osoba { get; set; }
        public string? Telefon { get; set; }
        public string? Zanr { get; set; }
        public IzvodjacTip tipIzvodajaca { get; set; }

        public IzvodjacView(Izvodjac i)
        {
            Id = i.ID;
            Ime = i.IME;
            Drzava_porekla = i.DRZAVA_POREKLA;
            Email = i.EMAIL;
            Kontakt_osoba = i.KONTAKT_OSOBA;
            Telefon = i.TELEFON;
            Zanr = i.Zanr;
            tipIzvodajaca = i.TIP_IZVODJACA;
        }

        //protected konstruktor koji ce koristiti izvedene klase
        protected IzvodjacView(int id, string ime, string drzava_porekla, string email, string kontakt_osoba, string telefon, string zanr, IzvodjacTip bEND)
        {
            Id = id;
            Ime = ime;
            Drzava_porekla = drzava_porekla;
            Email = email;
            Kontakt_osoba = kontakt_osoba;
            Telefon = telefon;
            Zanr = zanr;
            BEND = bEND;
        }

        public IzvodjacTip BEND { get; }
    }

    public class BendView : IzvodjacView
    {
        public int Broj_clanova { get; set; }
        public BendView(int id, string ime, string drzava_porekla, string email, string kontakt_osoba, string telefon, string zanr, int broj_clanova)
            : base(id, ime, drzava_porekla, email, kontakt_osoba, telefon, zanr, IzvodjacTip.BEND)
        {
            Broj_clanova = broj_clanova;
        }
    }

    public class Solo_umetnikView : IzvodjacView
    {
        public string Svira_instrument { get; set; }
        public string Tip_instrumenta { get; set; }
        public Solo_umetnikView(int id, string ime, string drzava_porekla, string email, string kontakt_osoba, string telefon, string zanr,string svira_instrument, string tip_instrumenta)
            : base(id, ime, drzava_porekla, email, kontakt_osoba, telefon, zanr, IzvodjacTip.SOLO_UMETNIK)
        {
            Svira_instrument = svira_instrument;
            Tip_instrumenta = tip_instrumenta;
        }
    }
    public abstract class IzvodjacBasic
    {
        public int Id { get; set; }
        public string Ime { get; set; }
        public string Drzava_porekla { get; set; }
        public string Email { get; set; }
        public string Kontakt_osoba { get; set; }
        public string Telefon { get; set; }
        public string Zanr { get; set; }
        public IzvodjacTip TipIzvodajac {  get; set; } = new IzvodjacTip();
        public MenadzerskaAgencijaBasic MenadzerskaAgencija;
        public DogadjajReferenceDTO Dogadjaj { get; set; }
        public IzvodjacBasic() { }
        public IzvodjacBasic(int id, string ime,string drzava_poreka, string email,string kontakt_osoba, string telefon, string zanr, IzvodjacTip tipIzvodjaca, MenadzerskaAgencijaBasic menadzerskaAgencija)
        {
            Id = id;
            Ime = ime;
            Drzava_porekla = drzava_poreka;
            Email = email;
            Kontakt_osoba = kontakt_osoba;
            Telefon = telefon;
            IzvodjacTip TipIzvodajaca = tipIzvodjaca;
            MenadzerskaAgencija = menadzerskaAgencija;
            Zanr = zanr;
        }
    }

    public class Solo_UmetnikBasic : IzvodjacBasic
    {
        public string Svira_instrument { get; set; }
        public string Tip_instrumenta { get; set; }

        public Solo_UmetnikBasic() { }
        public Solo_UmetnikBasic(int id, string ime, string drzava_poreka, string email, string kontakt_osoba, string telefon, string zanr, MenadzerskaAgencijaBasic menadzerskaAgencija, string svira_instrument, string tip_instrumenta)
            :base(id, ime, drzava_poreka, email, kontakt_osoba, telefon, zanr, IzvodjacTip.SOLO_UMETNIK, menadzerskaAgencija)
        {
            Svira_instrument = svira_instrument;
            Tip_instrumenta = tip_instrumenta;
        }
    }

    public class BendBasic : IzvodjacBasic
    {
        public BendBasic() { }
        public BendBasic(int id, string ime, string drzava_poreka, string email, string kontakt_osoba, string telefon, string zanr, MenadzerskaAgencijaBasic menadzerskaAgencija)
            : base(id, ime, drzava_poreka, email, kontakt_osoba, telefon, zanr, IzvodjacTip.BEND, menadzerskaAgencija)
        {

        }

    }

    public class ClanBendaView
    {
        public int Id { get; set; }
        public string Ime { get; set; }
        public string Instrument { get; set; }
        public string Uloga { get; set; }

        public ClanBendaView(int id, string ime, string instrument, string uloga)
        {
            Id = id;
            Ime = ime;
            Instrument = instrument;
            Uloga = uloga;
        }
    }

    public class ClanBendaBasic
    {
        public int Id { get; set; }
        public string Ime { get; set; }
        public string Instrument { get; set; }
        public string Uloga { get; set; }
        public BendBasic Bend { get; set; }

        public ClanBendaBasic() { }
        public ClanBendaBasic(int id, string ime, string instrument, string uloga, BendBasic bend)
        {
            Id = id;
            Ime = ime;
            Instrument = instrument;
            Bend = bend;
            Uloga = uloga;
        }
    }
}
