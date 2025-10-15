using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Util;
using System.Security.Cryptography;
using NHibernate;
using Muzicki_festival.Entiteti;
using Muzicki_festival.DTOs;
using ISession = NHibernate.ISession;

namespace Muzicki_festival
{
    public class DTOManager
    {
        #region Izvodjac

        public static IList<IzvodjacView> VratiSveIzvodjace()
        {
            try
            {
                ISession s = DataLayer.GetSession();

                IList<Izvodjac> izvodjaci = s.Query<Izvodjac>().ToList();
                IList<IzvodjacView> izvodjaciView = new List<IzvodjacView>();

                foreach (Izvodjac i in izvodjaci)
                {
                    switch (i.TIP_IZVODJACA)
                    {
                        case IzvodjacTip.SOLO_UMETNIK:
                            Solo_Umetnik u = i as Solo_Umetnik;
                            izvodjaciView.Add(new Solo_umetnikView(u.ID, u.IME, i.DRZAVA_POREKLA, u.EMAIL, u.KONTAKT_OSOBA, u.TELEFON, u.Zanr, u.SVIRA_INSTRUMENT, u.TIP_INSTRUMENTA));
                            break;

                        case IzvodjacTip.BEND:
                            Bend b = i as Bend;
                            izvodjaciView.Add(new BendView(b.ID, b.IME, b.DRZAVA_POREKLA, b.EMAIL, b.KONTAKT_OSOBA, b.TELEFON, b.Zanr, b.BROJ_CLANOVA));
                            break;
                    }
                }

                s.Close();

                return izvodjaciView;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<IzvodjacView>();
            }
        }

        public static IzvodjacView VratiIzvodjaca(int id)
        {
            try
            {
                ISession s = DataLayer.GetSession();
                Izvodjac i = s.Get<Izvodjac>(id);
                IzvodjacView iv = null;
                if (i != null)
                {
                    switch (i.TIP_IZVODJACA)
                    {
                        case IzvodjacTip.SOLO_UMETNIK:
                            Solo_Umetnik u = i as Solo_Umetnik;
                            iv = new Solo_umetnikView(u.ID, u.IME, i.DRZAVA_POREKLA, u.EMAIL, u.KONTAKT_OSOBA, u.TELEFON, u.Zanr, u.SVIRA_INSTRUMENT, u.TIP_INSTRUMENTA);
                            break;
                        case IzvodjacTip.BEND:
                            Bend b = i as Bend;
                            iv = new BendView(b.ID, b.IME, b.DRZAVA_POREKLA, b.EMAIL, b.KONTAKT_OSOBA, b.TELEFON, b.Zanr, b.BROJ_CLANOVA);
                            break;
                    }
                }
                s.Close();
                return iv;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public static IzvodjacView DodajIzvodjaca(IzvodjacBasic i)
        {
            try
            {
                ISession s = DataLayer.GetSession();
                MenadzerskaAgencija ma = s.Get<MenadzerskaAgencija>(i.MenadzerskaAgencija.ID);
                Dogadjaj d = s.Get<Dogadjaj>(i.Dogadjaj.Id);

                if (ma == null || d == null)
                    return null;

                int id;
                switch (i.TipIzvodajac)
                {
                    case IzvodjacTip.SOLO_UMETNIK:
                        Solo_Umetnik novi = new Solo_Umetnik
                        {
                            IME = i.Ime,
                            DRZAVA_POREKLA = i.Drzava_porekla,
                            EMAIL = i.Email,
                            TELEFON = i.Telefon,
                            TIP_IZVODJACA = i.TipIzvodajac,
                            Zanr = i.Zanr,
                            KONTAKT_OSOBA = i.Kontakt_osoba,
                            SVIRA_INSTRUMENT = (i as Solo_UmetnikBasic).Svira_instrument,
                            TIP_INSTRUMENTA = (i as Solo_UmetnikBasic).Tip_instrumenta,
                            MenadzerskaAgencija = ma,
                        };

                        id = (int)s.Save(novi);
                        s.Flush();

                        ma.Izvodjaci.Add(novi);
                        s.Update(ma);
                        s.Flush();

                        d.Izvodjaci.Add(novi);
                        s.Update(d);
                        s.Flush();

                        novi.Dogadjaji.Add(d);
                        s.Update(novi);
                        s.Flush();

                        s.Close();

                        return new Solo_umetnikView(id, novi.IME, novi.DRZAVA_POREKLA, novi.EMAIL, novi.KONTAKT_OSOBA, novi.TELEFON, novi.Zanr, novi.SVIRA_INSTRUMENT, novi.TIP_INSTRUMENTA);
                    case IzvodjacTip.BEND:
                        Bend bend = new Bend
                        {
                            IME = i.Ime,
                            DRZAVA_POREKLA = i.Drzava_porekla,
                            EMAIL = i.Email,
                            TELEFON = i.Telefon,
                            TIP_IZVODJACA = i.TipIzvodajac,
                            KONTAKT_OSOBA = i.Kontakt_osoba,
                            BROJ_CLANOVA = 0,
                            Zanr = i.Zanr,
                            MenadzerskaAgencija = ma
                        };

                        id = (int)s.Save(bend);
                        s.Flush();

                        ma.Izvodjaci.Add(bend);
                        s.Flush();

                        d.Izvodjaci.Add(bend);
                        s.Update(d);
                        s.Flush();

                        bend.Dogadjaji.Add(d);
                        s.Update(bend);
                        s.Flush();

                        s.Close();
                        return new BendView(id, bend.IME, bend.DRZAVA_POREKLA, bend.EMAIL, bend.KONTAKT_OSOBA, bend.TELEFON, bend.Zanr, bend.BROJ_CLANOVA);
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public static void ObrisiIzvodjaca(int id)
        {
            try
            {
                ISession s = DataLayer.GetSession();
                Izvodjac i = s.Get<Izvodjac>(id);
                if (i != null)
                {
                    s.Delete(i);
                    s.Flush();
                }
                s.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static IList<ClanBendaView> VratiClanoveBenda(int bendId)
        {
            try
            {
                ISession s = DataLayer.GetSession();
                Bend b = s.Get<Bend>(bendId);

                if (b == null)
                    return new List<ClanBendaView>();

                List<ClanBendaView> clanovi = new List<ClanBendaView>();

                foreach (var c in b.Clanovi)
                {
                    clanovi.Add(new ClanBendaView(c.ID, c.IME, c.INSTRUMENT, c.ULOGA));
                }

                s.Close();
                return clanovi;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<ClanBendaView>();
            }
        }

        public static ClanBendaView DodajClanaBendu(ClanBendaBasic cb)
        {
            try
            {
                ISession s = DataLayer.GetSession();
                Bend bend = s.Get<Bend>(cb.Bend.Id);

                if (bend == null)
                    return null;

                Clan c = new Clan
                {
                    IME = cb.Ime,
                    INSTRUMENT = cb.Instrument,
                    BEND = bend,
                    ULOGA = cb.Uloga
                };

                bend.Clanovi.Add(c);
                bend.BROJ_CLANOVA += 1;
                s.Update(bend);
                s.Flush();
                s.Close();

                return new ClanBendaView(c.ID, c.IME, c.INSTRUMENT, c.ULOGA);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        public static bool ObrisiClana(ClanBendaBasic cb)
        {
            try
            {
                ISession s = DataLayer.GetSession();
                Bend bend = s.Get<Bend>(cb.Bend.Id);

                if (bend == null)
                    return false;

                Clan c = s.Get<Clan>(cb.Id);

                bool ret = bend.Clanovi.Remove(c);
                if (ret)
                    bend.BROJ_CLANOVA -= 1;

                s.Update(bend);
                s.Flush();

                s.Update(c);
                s.Flush();

                s.Delete(c);
                s.Flush();

                s.Close();

                return ret;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public static bool IzmeniIzvodjaca(IzvodjacBasic i)
        {
            try
            {
                ISession s = DataLayer.GetSession();

                Izvodjac izv = s.Get<Izvodjac>(i.Id);

                if (izv == null)
                    return false;

                if (izv.MenadzerskaAgencija.ID != i.MenadzerskaAgencija.ID)
                {
                    MenadzerskaAgencija ma = s.Get<MenadzerskaAgencija>(izv.MenadzerskaAgencija.ID);

                    ma.Izvodjaci.Remove(izv);
                    s.Update(ma);
                    s.Flush();

                    MenadzerskaAgencija novaMa = s.Get<MenadzerskaAgencija>(i.MenadzerskaAgencija.ID);
                    novaMa.Izvodjaci.Add(izv);
                    s.Update(novaMa);

                    izv.MenadzerskaAgencija = novaMa;
                    s.Update(izv);

                    s.Flush();
                }

                switch (izv.TIP_IZVODJACA)
                {
                    case IzvodjacTip.BEND:
                        Bend b = izv as Bend;
                        BendBasic basic = i as BendBasic;

                        b.DRZAVA_POREKLA = basic.Drzava_porekla;
                        b.IME = basic.Ime;
                        b.EMAIL = basic.Email;
                        b.TELEFON = basic.Telefon;
                        b.KONTAKT_OSOBA = basic.Kontakt_osoba;

                        s.Update(b);
                        s.Flush();
                        s.Close();

                        return true;
                    case IzvodjacTip.SOLO_UMETNIK:
                        Solo_Umetnik su = izv as Solo_Umetnik;
                        Solo_UmetnikBasic sbasic = i as Solo_UmetnikBasic;

                        su.DRZAVA_POREKLA = i.Drzava_porekla;
                        su.IME = sbasic.Ime;
                        su.EMAIL = sbasic.Email;
                        su.TELEFON = sbasic.Telefon;
                        su.KONTAKT_OSOBA = sbasic.Kontakt_osoba;
                        su.SVIRA_INSTRUMENT = sbasic.Svira_instrument;
                        su.TIP_INSTRUMENTA = sbasic.Svira_instrument;

                        s.Update(su);
                        s.Flush();
                        s.Close();

                        return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public static IList<string> VratiTehnickeZahteve(int Id)
        {
            try
            {
                ISession s = DataLayer.GetSession();
                Izvodjac i = s.Get<Izvodjac>(Id);

                if (i == null)
                {
                    return new List<string>();
                }

                // isto ko za vokalne, zbog lazyLoad i zatvaranje sesije
                List<string> list = new List<string>();
                foreach (var z in i.Lista_tehnickih_zahteva)
                    list.Add(z);

                s.Close();

                return list;
            }
            catch (Exception e)
            {
               Console.WriteLine(e.Message);
                return new List<string>();
            }
        }

        public static bool DodajTehnickiZahtev(int Id, string zahtev)
        {
            try
            {
                ISession s = DataLayer.GetSession();
                Izvodjac i = s.Get<Izvodjac>(Id);

                if (i == null)
                {
                    return false;
                }

                i.Lista_tehnickih_zahteva.Add(zahtev);
                s.Update(i);
                s.Flush();
                s.Close();

                return true;
            }
            catch (Exception e)
            {
               Console.WriteLine(e.Message);
                return false;
            }
        }

        public static bool ObrisiTehnickiZahtev(int Id, string zahtev)
        {
            try
            {
                ISession s = DataLayer.GetSession();
                Izvodjac i = s.Get<Izvodjac>(Id);

                if (i == null)
                {
                    return false;
                }

                bool ret = i.Lista_tehnickih_zahteva.Remove(zahtev);
                s.Update(i);
                s.Flush();
                s.Close();

                return ret;
            }
            catch (Exception e)
            {
               Console.WriteLine(e.Message);
                return false;
            }
        }

        public static IList<string> VratiVokalneSposobnosti(int Id)
        {
            try
            {
                ISession s = DataLayer.GetSession();
                Solo_Umetnik su = s.Get<Solo_Umetnik>(Id);

                if (su == null)
                {
                    return new List<string>();
                }

                // Ovako jer u suprotnom puca kada se pristupi a zatvori se sesija zbog LazyLoad
                // bolje ovo nego da nonstop stoji otvorena sesija
                List<string> list = new List<string>();
                foreach (var sp in su.VOKALNE_SPOSOBNOSTI)
                    list.Add(sp);

                s.Close();
                return list;
            }
            catch (Exception e)
            {
               Console.WriteLine(e.Message);
                return new List<string>();
            }
        }

        public static bool DodajVokalnuSposobnost(int Id, string sposobnost)
        {
            try
            {
                ISession s = DataLayer.GetSession();
                Solo_Umetnik su = s.Get<Solo_Umetnik>(Id);

                if (su == null)
                {
                    return false;
                }

                su.VOKALNE_SPOSOBNOSTI.Add(sposobnost);

                s.Update(su);
                s.Flush();
                s.Close();

                return true;

            }
            catch (Exception e)
            {
               Console.WriteLine(e.Message);
                return false;
            }
        }

        public static bool ObrisiVokalnuSposobnost(int Id, string sposobnost)
        {
            try
            {
                ISession s = DataLayer.GetSession();
                Solo_Umetnik su = s.Get<Solo_Umetnik>(Id);

                if (su == null)
                {
                    return false;
                }

                bool ret = su.VOKALNE_SPOSOBNOSTI.Remove(sposobnost);
                s.Update(su);
                s.Flush();
                s.Close();

                return ret;

            }
            catch (Exception e)
            {
               Console.WriteLine(e.Message);
                return false;
            }
        }

        #endregion

        #region Lokacija

        public static IList<LokacijaView> VratiSveLokacije()
        {
            IList<LokacijaView> lokacijeView = new List<LokacijaView>();
            try
            {

                ISession s = DataLayer.GetSession();
                IList<Lokacija> lokacije = s.Query<Lokacija>().ToList();

                foreach (Lokacija l in lokacije)
                {
                    switch (l.TIP_LOKACIJE)
                    {
                        case TipLokacije.OTVORENA:
                            OtvorenaLokacija o = l as OtvorenaLokacija;
                            lokacijeView.Add(new OtvorenaLokacijaView(o.ID, o.OPIS, o.NAZIV, o.GPS_KOORDINATE, o.MAX_KAPACITET ?? 0));
                            break;
                        case TipLokacije.ZATVORENA:
                            ZatvorenaLokacija z = l as ZatvorenaLokacija;
                            lokacijeView.Add(new ZatvorenaLokacijaView(z.ID, z.OPIS, z.NAZIV, z.GPS_KOORDINATE, z.MAX_KAPACITET ?? 0, z.TIP_PROSTORA, z.KLIMA, z.DOSTUPNOST_SEDENJA));
                            break;
                        case TipLokacije.KOMBINOVANA:
                            KombinovanaLokacija k = l as KombinovanaLokacija;
                            lokacijeView.Add(new KombinovanaLokacijaView(k.ID, k.OPIS, k.NAZIV, k.GPS_KOORDINATE, k.MAX_KAPACITET ?? 0, k.TIP_PROSTORA, k.KLIMA, k.DOSTUPNOST_SEDENJA));
                            break;
                    }
                }

                s.Close();
                return lokacijeView;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return lokacijeView;
            }
        }

        public static LokacijaView DodajLokaciju(LokacijaBasic l)
        {
            try
            {
                ISession s = DataLayer.GetSession();
                int id;
                LokacijaView ret = null;
                switch (l.TipLokacije)
                {
                    case TipLokacije.OTVORENA:
                        OtvorenaLokacija o = new OtvorenaLokacija()
                        {
                            NAZIV = l.Naziv,
                            GPS_KOORDINATE = l.Gps_koordinate,
                            MAX_KAPACITET = l.Kapacitet,
                            OPIS = l.Opis,
                            TIP_LOKACIJE = TipLokacije.OTVORENA
                        };

                        id = (int)s.Save(o);
                        ret = new OtvorenaLokacijaView(id, o.OPIS, o.NAZIV, o.GPS_KOORDINATE, o.MAX_KAPACITET ?? 0);
                        break;
                    case TipLokacije.ZATVORENA:
                        ZatvorenaLokacija z = new ZatvorenaLokacija()
                        {
                            NAZIV = l.Naziv,
                            GPS_KOORDINATE = l.Gps_koordinate,
                            MAX_KAPACITET = l.Kapacitet,
                            OPIS = l.Opis,
                            TIP_LOKACIJE = TipLokacije.ZATVORENA,
                            TIP_PROSTORA = (l as ZatvorenaLokacijaBasic).Tip_prostora,
                            KLIMA = (l as ZatvorenaLokacijaBasic).Klima,
                            DOSTUPNOST_SEDENJA = (l as ZatvorenaLokacijaBasic).Dostupnost_sedenja
                        };

                        id = (int)s.Save(z);
                        ret = new ZatvorenaLokacijaView(id, z.OPIS, z.NAZIV, z.GPS_KOORDINATE, z.MAX_KAPACITET ?? 0, z.TIP_PROSTORA, z.KLIMA, z.DOSTUPNOST_SEDENJA);
                        break;
                    case TipLokacije.KOMBINOVANA:
                        KombinovanaLokacija k = new KombinovanaLokacija()
                        {
                            NAZIV = l.Naziv,
                            GPS_KOORDINATE = l.Gps_koordinate,
                            MAX_KAPACITET = l.Kapacitet,
                            OPIS = l.Opis,
                            TIP_LOKACIJE = TipLokacije.KOMBINOVANA,
                            TIP_PROSTORA = (l as KombinovanaLokacijaBasic).Tip_prostora,
                            KLIMA = (l as KombinovanaLokacijaBasic).Klima,
                            DOSTUPNOST_SEDENJA = (l as KombinovanaLokacijaBasic).Dostupnost_sedenja
                        };

                        id = (int)s.Save(k);
                        ret = new KombinovanaLokacijaView(id, k.OPIS, k.NAZIV, k.GPS_KOORDINATE, k.MAX_KAPACITET ?? 0, k.TIP_PROSTORA, k.KLIMA, k.DOSTUPNOST_SEDENJA);
                        break;
                }

                s.Flush();
                s.Close();

                return ret;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }

        public static LokacijaView VratiLokaciju(int id)
        {
            try
            {
                ISession s = DataLayer.GetSession();
                Lokacija l = s.Get<Lokacija>(id);
                LokacijaView lv = null;
                if (l != null)
                {
                    switch (l.TIP_LOKACIJE)
                    {
                        case TipLokacije.OTVORENA:
                            OtvorenaLokacija o = l as OtvorenaLokacija;
                            lv = new OtvorenaLokacijaView(o.ID, o.OPIS, o.NAZIV, o.GPS_KOORDINATE, o.MAX_KAPACITET ?? 0);
                            break;
                        case TipLokacije.ZATVORENA:
                            ZatvorenaLokacija z = l as ZatvorenaLokacija;
                            lv = new ZatvorenaLokacijaView(z.ID, z.OPIS, z.NAZIV, z.GPS_KOORDINATE, z.MAX_KAPACITET ?? 0, z.TIP_PROSTORA, z.KLIMA, z.DOSTUPNOST_SEDENJA);
                            break;
                        case TipLokacije.KOMBINOVANA:
                            KombinovanaLokacija k = l as KombinovanaLokacija;
                            lv = new KombinovanaLokacijaView(k.ID, k.OPIS, k.NAZIV, k.GPS_KOORDINATE, k.MAX_KAPACITET ?? 0, k.TIP_PROSTORA, k.KLIMA, k.DOSTUPNOST_SEDENJA);
                            break;
                    }
                }
                s.Close();
                return lv;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        public static bool IzmeniLokaciju(LokacijaBasic nova)
        {
            try
            {
                ISession s = DataLayer.GetSession();
                Lokacija l = s.Get<Lokacija>(nova.Id);
                bool ret = false;
                if (l != null)
                {
                    switch (nova.TipLokacije)
                    {
                        case TipLokacije.OTVORENA:
                            OtvorenaLokacija o = l as OtvorenaLokacija;
                            o.NAZIV = nova.Naziv;
                            o.GPS_KOORDINATE = nova.Gps_koordinate;
                            o.MAX_KAPACITET = nova.Kapacitet;
                            o.OPIS = nova.Opis;
                            s.Update(o);
                            ret = true;
                            break;
                        case TipLokacije.ZATVORENA:
                            ZatvorenaLokacija z = l as ZatvorenaLokacija;
                            z.NAZIV = nova.Naziv;
                            z.GPS_KOORDINATE = nova.Gps_koordinate;
                            z.MAX_KAPACITET = nova.Kapacitet;
                            z.OPIS = nova.Opis;
                            z.TIP_PROSTORA = (nova as ZatvorenaLokacijaBasic).Tip_prostora;
                            z.KLIMA = (nova as ZatvorenaLokacijaBasic).Klima;
                            z.DOSTUPNOST_SEDENJA = (nova as ZatvorenaLokacijaBasic).Dostupnost_sedenja;
                            s.Update(z);
                            ret = true;
                            break;
                        case TipLokacije.KOMBINOVANA:
                            KombinovanaLokacija k = l as KombinovanaLokacija;
                            k.NAZIV = nova.Naziv;
                            k.GPS_KOORDINATE = nova.Gps_koordinate;
                            k.MAX_KAPACITET = nova.Kapacitet;
                            k.OPIS = nova.Opis;
                            k.TIP_PROSTORA = (nova as KombinovanaLokacijaBasic).Tip_prostora;
                            k.KLIMA = (nova as KombinovanaLokacijaBasic).Klima;
                            k.DOSTUPNOST_SEDENJA = (nova as KombinovanaLokacijaBasic).Dostupnost_sedenja;
                            s.Update(k);
                            ret = true;
                            break;
                    }
                }
                s.Flush();
                s.Close();

                return ret;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return false;
            }
        }

        #endregion

        #region DostupnaOprema

        public static IList<DostupnaOpremaView> VratiSvuDostupnuOpremu(int idLokacije)
        {
            IList<DostupnaOpremaView> opremaView = new List<DostupnaOpremaView>();
            try
            {
                ISession s = DataLayer.GetSession();
                IList<DostupnaOprema> oprema = s.Query<DostupnaOprema>()
                    .Where(d => d.Lokacija.ID == idLokacije)
                    .ToList();

                foreach (DostupnaOprema o in oprema)
                {
                    opremaView.Add(new DostupnaOpremaView(o.ID, o.NAZIV));
                }
                s.Close();
                return opremaView;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return opremaView;
            }
        }

        public static DostupnaOpremaView DodajDostupnuOpremu(DostupnaOpremaBasic o)
        {
            try
            {
                ISession s = DataLayer.GetSession();

                using (var transaction = s.BeginTransaction())
                {
                    Lokacija l = s.Get<Lokacija>(o.Lokacija.Id);
                    if (l == null) return null;

                    DostupnaOprema oprema = new DostupnaOprema
                    {
                        NAZIV = o.Naziv
                    };

                    l.DOSTUPNA_OPREMA.Add(oprema); // dodaš u kolekciju
                    oprema.Lokacija = l;           // obavezno postavi reference

                    s.SaveOrUpdate(l);             // SaveOrUpdate samo lokaciju
                    transaction.Commit();

                    return new DostupnaOpremaView(oprema.ID, oprema.NAZIV);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public static bool ObrisiDostupnuOpremu(DostupnaOpremaBasic o)
        {
            try
            {
                ISession s = DataLayer.GetSession();

                using (var transaction = s.BeginTransaction())
                {
                    Lokacija l = s.Get<Lokacija>(o.Lokacija.Id);
                    if (l == null)
                        return false;

                    DostupnaOprema oprema = s.Get<DostupnaOprema>(o.Id);

                    if (oprema == null || oprema.Lokacija.ID != l.ID)
                        return false;

                    bool ret = l.DOSTUPNA_OPREMA.Remove(oprema);
                    s.Delete(oprema);
                    s.SaveOrUpdate(l);
                    transaction.Commit();

                    return ret;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        #endregion

        #region MenadzerskaAgencija

        public static IList<MenadzerskaAgencijaView> VratiSveMenadzerskeAgencije()
        {
            try
            {
                ISession s = DataLayer.GetSession();
                IList<MenadzerskaAgencija> agencije = s.Query<MenadzerskaAgencija>().ToList();
                IList<MenadzerskaAgencijaView> agencijeView = new List<MenadzerskaAgencijaView>();
                foreach (MenadzerskaAgencija ma in agencije)
                {
                    agencijeView.Add(new MenadzerskaAgencijaView(ma.ID, ma.NAZIV, ma.ADRESA, ma.KONTAKT_OSOBA));
                }

                s.Close();
                return agencijeView;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<MenadzerskaAgencijaView>();
            }
        }

        public static MenadzerskaAgencijaView VratiMenadzerskuAgenciju(int id)
        {
            try
            {
                ISession s = DataLayer.GetSession();
                MenadzerskaAgencija ma = s.Get<MenadzerskaAgencija>(id);
                MenadzerskaAgencijaView mav = null;
                if (ma != null)
                {
                    mav = new MenadzerskaAgencijaView(ma.ID, ma.NAZIV, ma.ADRESA, ma.KONTAKT_OSOBA);
                }
                s.Close();
                return mav;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public static MenadzerskaAgencijaView DodajMenadzerskuAgenciju(MenadzerskaAgencijaBasic ma)
        {
            try
            {
                ISession s = DataLayer.GetSession();
                MenadzerskaAgencija nova = new MenadzerskaAgencija()
                {
                    NAZIV = ma.Naziv,
                    ADRESA = ma.Adresa,
                    KONTAKT_OSOBA = ma.KontaktOsoba
                };
                int id = (int)s.Save(nova);
                s.Flush();
                s.Close();
                return new MenadzerskaAgencijaView(id, nova.NAZIV, nova.ADRESA, nova.KONTAKT_OSOBA);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        public static MenadzerskaAgencijaView VratiMenadzerskuIzvodjaca(int idIzvodjaca)
        {
            try
            {
                ISession s = DataLayer.GetSession();
                MenadzerskaAgencija ma = s.Query<MenadzerskaAgencija>()
                    .Where(m => m.Izvodjaci.Any(i => i.ID == idIzvodjaca)).FirstOrDefault();

                if (ma == null)
                    return null;

                return new MenadzerskaAgencijaView(ma.ID, ma.NAZIV, ma.ADRESA, ma.KONTAKT_OSOBA);
            }
            catch (Exception e)
            {
               Console.WriteLine(e.Message);
                return null;
            }
        }
        public static bool IzmeniMenadzerskuAgenciju(MenadzerskaAgencijaBasic mb)
        {
            try
            {
                ISession s = DataLayer.GetSession();
                MenadzerskaAgencija ma = s.Get<MenadzerskaAgencija>(mb.ID);

                if (ma == null)
                    return false;

                ma.NAZIV = mb.Naziv;
                ma.ADRESA = mb.Adresa;
                ma.KONTAKT_OSOBA = mb.KontaktOsoba;

                s.Update(ma);
                s.Flush();
                s.Close();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return false;
            }
        }

        public static void ObrisiMenadzerskuAgenciju(int id)
        {
            try
            {
                ISession s = DataLayer.GetSession();
                MenadzerskaAgencija ma = s.Get<MenadzerskaAgencija>(id);
                if (ma != null)
                {
                    s.Delete(ma);
                    s.Flush();
                }
                s.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static IList<IzvodjacView> VratiIzvodjaceMenadzerskeAgencije(int idMenadzerskeAgencije)
        {
            try
            {
                ISession s = DataLayer.GetSession();
                MenadzerskaAgencija ma = s.Get<MenadzerskaAgencija>(idMenadzerskeAgencije);
                IList<IzvodjacView> izvodjaciView = new List<IzvodjacView>();
                if (ma != null)
                {
                    foreach (Izvodjac i in ma.Izvodjaci)
                    {
                        switch (i.TIP_IZVODJACA)
                        {
                            case IzvodjacTip.SOLO_UMETNIK:
                                Solo_Umetnik u = i as Solo_Umetnik;
                                izvodjaciView.Add(new Solo_umetnikView(u.ID, u.IME, i.DRZAVA_POREKLA, u.EMAIL, u.KONTAKT_OSOBA, u.TELEFON, u.Zanr, u.SVIRA_INSTRUMENT, u.TIP_INSTRUMENTA));
                                break;
                            case IzvodjacTip.BEND:
                                Bend b = i as Bend;
                                izvodjaciView.Add(new BendView(b.ID, b.IME, b.DRZAVA_POREKLA, b.EMAIL, b.KONTAKT_OSOBA, b.TELEFON, b.Zanr, b.BROJ_CLANOVA));
                                break;
                        }
                    }
                }
                s.Close();
                return izvodjaciView;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<IzvodjacView>();
            }
        }

        public static IList<MenadzerskaAgencijaKontaktView> VratiSveKontaktPodatke(int menadzerksaAgencijaId)
        {
            try
            {
                ISession s = DataLayer.GetSession();
                IList<KontaktPodaciMenadzerskeAgencije> kontaktPodaci = s.Query<KontaktPodaciMenadzerskeAgencije>()
                    .Where(k => k.MENADZERSKA_AGENCIJA.ID == menadzerksaAgencijaId)
                    .ToList();
                IList<MenadzerskaAgencijaKontaktView> kontaktPodaciView = new List<MenadzerskaAgencijaKontaktView>();
                foreach (KontaktPodaciMenadzerskeAgencije k in kontaktPodaci)
                {
                    kontaktPodaciView.Add(new MenadzerskaAgencijaKontaktView(k.ID, k.TIP_KONTAKTA, k.VREDNOST));
                }
                s.Close();
                return kontaktPodaciView;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<MenadzerskaAgencijaKontaktView>();
            }
        }

        public static MenadzerskaAgencijaKontaktView DodajKontaktMenadzerskeAgencije(MenadzerskaAgencijaKontaktBasic k)
        {
            try
            {
                ISession s = DataLayer.GetSession();
                MenadzerskaAgencija ma = s.Get<MenadzerskaAgencija>(k.MenadzerkaAgencija.ID);
                if (ma == null)
                    return null;

                KontaktPodaciMenadzerskeAgencije novi = new KontaktPodaciMenadzerskeAgencije
                {
                    TIP_KONTAKTA = k.TIP_KONTAKTA,
                    VREDNOST = k.Vrednost,
                    MENADZERSKA_AGENCIJA = ma
                };

                ma.KONTAKT_PODACI.Add(novi);
                s.SaveOrUpdate(ma);
                s.Flush();
                s.Close();

                return new MenadzerskaAgencijaKontaktView(novi.ID, novi.TIP_KONTAKTA, novi.VREDNOST);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public static bool ObrisiKontaktMenadzerskeAgencije(MenadzerskaAgencijaKontaktBasic k)
        {
            try
            {
                ISession s = DataLayer.GetSession();
                MenadzerskaAgencija ma = s.Get<MenadzerskaAgencija>(k.MenadzerkaAgencija.ID);
                if (ma == null)
                    return false;

                KontaktPodaciMenadzerskeAgencije kontakt = s.Get<KontaktPodaciMenadzerskeAgencije>(k.ID);
                if (kontakt == null || kontakt.MENADZERSKA_AGENCIJA.ID != ma.ID)
                    return false;

                bool ret = ma.KONTAKT_PODACI.Remove(kontakt);
                s.Delete(kontakt);
                s.SaveOrUpdate(ma);
                s.Flush();
                s.Close();

                return ret;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        #endregion

        #region Dogadjaj

        public static IList<DogadjajView> VratiSveDogadjaje()
        {
            try
            {
                ISession s = DataLayer.GetSession();
                List<Dogadjaj> dogadjaji = s.Query<Dogadjaj>().ToList();

                List<DogadjajView> dogadjajViews = new List<DogadjajView>();
                foreach (var d in dogadjaji)
                {
                    dogadjajViews.Add(new DogadjajView(d.ID, d.NAZIV, d.TIP, d.OPIS, d.DATUM_VREME_POCETKA, d.DATUM_VREME_KRAJA, d.Lokacija.NAZIV));
                }

                s.Close();

                return dogadjajViews;
            }
            catch (Exception e)
            {
               Console.WriteLine(e.Message);
                return new List<DogadjajView>();
            }
        }

        public static DogadjajView DodajDogadjaj(DogadjajBasic db)
        {
            try
            {
                ISession s = DataLayer.GetSession();
                Lokacija l = s.Get<Lokacija>(db.Lokacija.Id);

                if (l == null)
                {
                    throw new Exception("Nepostojeca lokacija!");
                }

                Dogadjaj d = new Dogadjaj
                {
                    NAZIV = db.Naziv,
                    OPIS = db.Opis,
                    DATUM_VREME_POCETKA = db.DatumPocetka,
                    DATUM_VREME_KRAJA = db.DatumKraja,
                    TIP = db.Tip,
                    Lokacija = l
                };

                int id = (int)s.Save(d);
                s.Flush();
                s.Close();

                return new DogadjajView(id, db.Naziv, db.Tip, db.Opis, db.DatumPocetka, db.DatumKraja, db.Lokacija.Naziv);
            }
            catch (Exception e)
            {
               Console.WriteLine(e.Message);
                return null;
            }
        }

        public static IList<PosetilacView> VratiPosetioceDogadjaja(int dogadjajId)
        {
            try
            {
                ISession s = DataLayer.GetSession();
                Dogadjaj d = s.Get<Dogadjaj>(dogadjajId);
                if (d == null)
                {
                    return new List<PosetilacView>();
                }

                List<PosetilacView> posetilacViews = new List<PosetilacView>();
                foreach (var u in d.Ulaznica)
                {
                    var p = new PosetilacView(u.KUPAC_ID.ID, u.KUPAC_ID.IME, u.KUPAC_ID.PREZIME, u.KUPAC_ID.EMAIL, u.KUPAC_ID.Telefon);
                    p.UlaznicaTip = u.TIP_ULAZNICE;
                    posetilacViews.Add(p);
                }

                return posetilacViews;
            }
            catch (Exception e)
            {
               Console.WriteLine(e.Message);
                return new List<PosetilacView>();
            }
        }

        public static IList<IzvodjacView> VratiSveIzvodjaceDogadjaja(int dogadjajId)
        {
            try
            {
                ISession s = DataLayer.GetSession();
                Dogadjaj d = s.Get<Dogadjaj>(dogadjajId);
                if (d == null)
                {
                    return new List<IzvodjacView>();
                }

                List<IzvodjacView> izvodjacViews = new List<IzvodjacView>();
                foreach (var i in d.Izvodjaci)
                {
                    switch (i.TIP_IZVODJACA)
                    {
                        case IzvodjacTip.SOLO_UMETNIK:
                            var u = i as Solo_Umetnik;
                            izvodjacViews.Add(new Solo_umetnikView(u.ID, u.IME, u.DRZAVA_POREKLA, u.EMAIL, u.KONTAKT_OSOBA, u.TELEFON, u.Zanr, u.SVIRA_INSTRUMENT, u.TIP_INSTRUMENTA));
                            break;
                        case IzvodjacTip.BEND:
                            var b = i as Bend;
                            izvodjacViews.Add(new BendView(b.ID, b.IME, b.DRZAVA_POREKLA, b.EMAIL, b.KONTAKT_OSOBA, b.TELEFON, b.Zanr, b.BROJ_CLANOVA));
                            break;
                    }
                }

                return izvodjacViews;
            }
            catch (Exception e)

            {
               Console.WriteLine(e.Message);
                return new List<IzvodjacView>();
            }
        }

        public static bool DodajIzvodjacaNaDogadjaj(int dogadjajId, int izvodjacId)
        {
            try
            {
                ISession s = DataLayer.GetSession();
                Dogadjaj d = s.Get<Dogadjaj>(dogadjajId);
                Izvodjac i = s.Get<Izvodjac>(izvodjacId);

                if (d == null || i == null)
                {
                    return false;
                }

                if (d.Izvodjaci.Contains(i))
                    return true;

                d.Izvodjaci.Add(i);
                i.Dogadjaji.Add(d);

                s.Update(d);
                s.Update(i);

                s.Flush();
                s.Close();

                return true;
            }
            catch (Exception e)
            {
               Console.WriteLine(e.Message);
                return false;
            }
        }

        #endregion

        #region Posetilac

        public static PosetilacView DodajPosetioca(PosetilacBasic pb)
        {
            try
            {
                ISession s = DataLayer.GetSession();
                Dogadjaj d = s.Get<Dogadjaj>(pb.Ulaznica.Dogadjaj.Id);

                if (d == null)
                {
                    throw new Exception("Nepostojeci dogadjaj");
                }

                Ulaznica u;
                switch (pb.Ulaznica.TipUlaznice)
                {
                    case TipUlaznice.JEDNODNEVNA:
                        u = new Jednodnevna
                        {
                            OSNOVNA_CENA = pb.Ulaznica.OsnovnaCena,
                            NACIN_PLACANJA = pb.Ulaznica.NacinPlacanja,
                            DATUM_KUPOVINE = pb.Ulaznica.DatumKupovine,
                            TIP_ULAZNICE = TipUlaznice.JEDNODNEVNA,
                            Dogadjaj = d,
                            DAN_VAZENJA = (pb.Ulaznica as JednodnevnaBasic).DatumVazenja
                        };
                        break;
                    case TipUlaznice.VISEDNEVNA:
                        u = new Visednevna
                        {
                            OSNOVNA_CENA = pb.Ulaznica.OsnovnaCena,
                            NACIN_PLACANJA = pb.Ulaznica.NacinPlacanja,
                            DATUM_KUPOVINE = pb.Ulaznica.DatumKupovine,
                            TIP_ULAZNICE = TipUlaznice.JEDNODNEVNA,
                            Dogadjaj = d,
                            Dani = (pb.Ulaznica as ViseDnevnaBasic).DatumiVazenja
                        };
                        break;
                    case TipUlaznice.VIP:
                        u = new Vip
                        {
                            OSNOVNA_CENA = pb.Ulaznica.OsnovnaCena,
                            NACIN_PLACANJA = pb.Ulaznica.NacinPlacanja,
                            DATUM_KUPOVINE = pb.Ulaznica.DatumKupovine,
                            TIP_ULAZNICE = TipUlaznice.JEDNODNEVNA,
                            Dogadjaj = d,
                            Pogodnosti = (pb.Ulaznica as VIPBasic).Pogodnosti,
                        };
                        break;
                    case TipUlaznice.AKREDITACIJA:
                        u = new Akreditacija
                        {
                            OSNOVNA_CENA = pb.Ulaznica.OsnovnaCena,
                            NACIN_PLACANJA = pb.Ulaznica.NacinPlacanja,
                            DATUM_KUPOVINE = pb.Ulaznica.DatumKupovine,
                            TIP_ULAZNICE = TipUlaznice.JEDNODNEVNA,
                            Dogadjaj = d,
                            TIP = (pb.Ulaznica as AkreditacijaBasic).Tip
                        };
                        break;
                    default:
                        throw new Exception("Nepravilna ulaznica!");
                }

                Posetilac p = new Posetilac
                {
                    IME = pb.Ime,
                    PREZIME = pb.Prezime,
                    EMAIL = pb.Email,
                    Telefon = pb.Telefon,
                    Ulaznica = u
                };

                u.KUPAC_ID = p;

                int pId = (int)s.Save(p);
                int UiD = (int)s.Save(u);
                s.Flush();
                s.Close();

                return new PosetilacView(pId, pb.Ime, pb.Prezime, pb.Email, pb.Telefon);
            }
            catch (Exception e)
            {
               Console.WriteLine(e.Message);
                return null;
            }
        }


        #endregion
    }
}
