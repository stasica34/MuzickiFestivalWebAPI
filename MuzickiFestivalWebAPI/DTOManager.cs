using Muzicki_festival.DTOs;
using Muzicki_festival.Entiteti;
using NHibernate;
using NHibernate.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using ISession = NHibernate.ISession;

namespace Muzicki_festival
{
    public class DTOManager
    {
        #region Greske
        public class ValidacijaIzuzetka : Exception
        {
            public string KljucGreske { get; }

            public ValidacijaIzuzetka(string kljucGreske, string poruka) : base(poruka)
            {
                KljucGreske = kljucGreske;
            }
        }
        #endregion
        #region Izvodjac

        public static IList<IzvodjacView> VratiSveIzvodjace()
        {
            try
            {
                ISession s = DataLayer.GetSession();

                IList<Izvodjac> izvodjaci = s.Query<Izvodjac>().OrderBy(a => a.IME).ToList();
                IList<IzvodjacView> izvodjaciView = new List<IzvodjacView>();

                foreach (Izvodjac i in izvodjaci)
                {
                    switch (i.TIP_IZVODJACA)
                    {
                        //puca ako je prazna tabela
                        //    case IzvodjacTip.SOLO_UMETNIK:
                        //        Solo_Umetnik u = i as Solo_Umetnik;
                        //        izvodjaciView.Add(new Solo_umetnikView(u.ID, u.IME, i.DRZAVA_POREKLA, u.EMAIL, u.KONTAKT_OSOBA, u.TELEFON, u.Zanr, u.SVIRA_INSTRUMENT, u.TIP_INSTRUMENTA));
                        //        break;

                        //    case IzvodjacTip.BEND:
                        //        Bend b = i as Bend;
                        //        izvodjaciView.Add(new BendView(b.ID, b.IME, b.DRZAVA_POREKLA, b.EMAIL, b.KONTAKT_OSOBA, b.TELEFON, b.Zanr, b.BROJ_CLANOVA));
                        //        break;
                        case IzvodjacTip.SOLO_UMETNIK:
                            Solo_Umetnik u = i as Solo_Umetnik;
                            if (u != null)
                            {
                                izvodjaciView.Add(new Solo_umetnikView(
                                    u.ID,
                                    u.IME ?? "",
                                    u.DRZAVA_POREKLA ?? "",
                                    u.EMAIL ?? "",
                                    u.KONTAKT_OSOBA ?? "",
                                    u.TELEFON ?? "",
                                    u.Zanr ?? "",
                                    u.SVIRA_INSTRUMENT ?? "",
                                    u.TIP_INSTRUMENTA ?? ""
                                ));
                            }
                            break;

                        case IzvodjacTip.BEND:
                            Bend b = i as Bend;
                            if (b != null)
                            {
                                izvodjaciView.Add(new BendView(
                                    b.ID,
                                    b.IME ?? "",
                                    b.DRZAVA_POREKLA ?? "",
                                    b.EMAIL ?? "",
                                    b.KONTAKT_OSOBA ?? "",
                                    b.TELEFON ?? "",
                                    b.Zanr ?? "",
                                    b.BROJ_CLANOVA
                                ));
                            }
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

                if (ma == null)
                    return null;

                int id;
                switch (i.tipIzvodjaca)
                {
                    case IzvodjacTip.SOLO_UMETNIK:
                        Solo_Umetnik novi = new Solo_Umetnik
                        {
                            IME = i.Ime,
                            DRZAVA_POREKLA = i.Drzava_porekla,
                            EMAIL = i.Email,
                            TELEFON = i.Telefon,
                            TIP_IZVODJACA = i.tipIzvodjaca,
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

                        s.Close();

                        return new Solo_umetnikView(id, novi.IME, novi.DRZAVA_POREKLA, novi.EMAIL, novi.KONTAKT_OSOBA, novi.TELEFON, novi.Zanr, novi.SVIRA_INSTRUMENT, novi.TIP_INSTRUMENTA);
                    case IzvodjacTip.BEND:
                        Bend bend = new Bend
                        {
                            IME = i.Ime,
                            DRZAVA_POREKLA = i.Drzava_porekla,
                            EMAIL = i.Email,
                            TELEFON = i.Telefon,
                            TIP_IZVODJACA = i.tipIzvodjaca,
                            KONTAKT_OSOBA = i.Kontakt_osoba,
                            BROJ_CLANOVA = 0,
                            Zanr = i.Zanr,
                            MenadzerskaAgencija = ma
                        };

                        id = (int)s.Save(bend);
                        ma.Izvodjaci.Add(bend);
                        s.Flush();

                        s.Close();
                        return new BendView(id, bend.IME, bend.DRZAVA_POREKLA, bend.EMAIL, bend.KONTAKT_OSOBA, bend.TELEFON, bend.Zanr, bend.BROJ_CLANOVA);
                }

                return null;
            }
            catch (NHibernate.Exceptions.GenericADOException ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message != null)
                {
                    string poruka = ex.InnerException.Message;
                    string kljucGreske = "";
                    string porukaKlijentu = "";
                    if (poruka.Contains("CHK_IZVODJAC_IME"))
                    {
                        kljucGreske = "IME";
                        porukaKlijentu = "Ime izvođača sadrži nedozvoljene znakove. Dozvoljena su samo slova.";
                    }
                    else if (poruka.Contains("CHK_IZVODJAC_TELEFON"))
                    {
                        kljucGreske = "TELEFON";
                        porukaKlijentu = "Telefon izvođača mora sadržati tačno 10 cifara.";
                    }
                    else if (poruka.Contains("CHK_IZVODJAC_ZANR"))
                    {
                        kljucGreske = "ZANR";
                        porukaKlijentu = "Žanr prihvata samo slova.";
                    }
                    else if (poruka.Contains("CHK_IZVODJAC_DRZAVA"))
                    {
                        kljucGreske = "DRZAVA";
                        porukaKlijentu = "Država izvođača sadrži nedozvoljene znakove. Dozvoljena su samo slova i razmaci.";
                    }
                    else if (poruka.Contains("CHK_IZVODJAC_KONTAKT_OSOBA"))
                    {
                        kljucGreske = "KONTAKT_OSOBA";
                        porukaKlijentu = "Kontakt osoba sadrži nedozvoljene znakove. Dozvoljena su samo slova, razmaci i crtica.";
                    }
                    else if (poruka.Contains("CK_IZVODJAC_TIP"))
                    {
                        kljucGreske = "TIP";
                        porukaKlijentu = "Morati izabrati tip.";
                    }
                    else if (poruka.Contains("UK_IZVODJAC_EMAIL"))
                    {
                        kljucGreske = "EMAIL";
                        porukaKlijentu = "Email izvođača već postoji! Unesite jedinstvenu email adresu.";
                    }
                    else if (poruka.Contains("UK_IZVODJAC_TELEFON"))
                    {
                        kljucGreske = "TELEFON";
                        porukaKlijentu = "Telefon izvođača već postoji! Unesite jedinstven broj telefona.";
                    }
                    else if (poruka.Contains("UQ_IZVODJAC_IME_DRZAVA_TIP"))
                    {
                        kljucGreske = "DUPLIKAT";
                        porukaKlijentu = "Izvođač sa istim imenom, državom porekla i tipom već postoji u bazi!";
                    }
                    else if (poruka.Contains("CHK_SOLO_TIP_INSTRUMENTA"))
                    {
                        kljucGreske = "TIP_INSTRUMENTA";
                        porukaKlijentu = "Za solo umetnika morate uneti instrument koji svira, u formi slova.";
                    }
                    else if (poruka.Contains("CHK_SOLO_SVIRA_INSTRUMENT"))
                    {
                        kljucGreske = "SVIRA_INSTRUMENT";
                        porukaKlijentu = "Za solo umetnika morate da izaberete da li svira instrument.";
                    }

                    if (!string.IsNullOrEmpty(kljucGreske))
                    {
                        throw new ValidacijaIzuzetka(kljucGreske, porukaKlijentu);
                    }
                    else
                    {
                        throw new Exception("Greška u bazi podataka. Detalji: " + ex.InnerException.Message);
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public static bool ObrisiIzvodjaca(int id)
        {
            try
            {
                using (ISession s = DataLayer.GetSession())
                {
                    Console.WriteLine($"Tražim izvođača sa ID: {id}");
                    Izvodjac i = s.Get<Izvodjac>(id);

                    if (i == null)
                    {
                        Console.WriteLine("Izvođač nije pronađen!");
                        return false;
                    }

                    Console.WriteLine($"Brisanje izvođača: {i.IME}");
                    s.Delete(i);
                    s.Flush();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Greška prilikom brisanja: {ex.Message}");
                return false;
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
            catch (NHibernate.Exceptions.GenericADOException ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message != null)
                {
                    string poruka = ex.InnerException.Message;
                    string kljucGreske = "";
                    string porukaKlijentu = "";

                    if (poruka.Contains("CHK_CLAN_INSTRUMENT"))
                    {
                        kljucGreske = "INSTRUMENT";
                        porukaKlijentu = "Naziv instrumenta nije u formatu slova.";
                    }
                    else if (poruka.Contains("CHK_CLAN_ULOGA"))
                    {
                        kljucGreske = "ULOGA";
                        porukaKlijentu = "Uloga člana nije u formatu slova.";
                    }
                    else if (poruka.Contains("CHK_CLAN_IME"))
                    {
                        kljucGreske = "IME";
                        porukaKlijentu = "Ime člana nije u formatu slova.";
                    }
                    else if (poruka.Contains("CHK_CLAN"))
                    {
                        kljucGreske = "DUPLIKAT_CLAN";
                        porukaKlijentu = "Član benda koji svira isti instrument ne može biti u više bendova istovremeno.";
                    }

                    if (!string.IsNullOrEmpty(kljucGreske))
                    {
                        throw new ValidacijaIzuzetka(kljucGreske, porukaKlijentu);
                    }
                    else
                    {
                        throw new Exception("Greška u bazi podataka. Detalji: " + ex.InnerException.Message);
                    }
                }
                else
                {
                    throw new Exception("Došlo je do greške u komunikaciji sa bazom.");
                }
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
                        b.Zanr = basic.Zanr;

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
                        su.TIP_INSTRUMENTA = sbasic.Tip_instrumenta;
                        su.Zanr = sbasic.Zanr;

                        s.Update(su);
                        s.Flush();
                        s.Close();

                        return true;
                }

                return false;
            }
            catch (NHibernate.Exceptions.GenericADOException ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message != null)
                {
                    string poruka = ex.InnerException.Message;
                    string kljucGreske = "";
                    string porukaKlijentu = "";
                    if (poruka.Contains("CHK_IZVODJAC_IME"))
                    {
                        kljucGreske = "IME";
                        porukaKlijentu = "Ime izvođača sadrži nedozvoljene znakove. Dozvoljena su samo slova.";
                    }
                    else if (poruka.Contains("CHK_IZVODJAC_TELEFON"))
                    {
                        kljucGreske = "TELEFON";
                        porukaKlijentu = "Telefon izvođača mora sadržati tačno 10 cifara.";
                    }
                    else if (poruka.Contains("CHK_IZVODJAC_ZANR"))
                    {
                        kljucGreske = "ZANR";
                        porukaKlijentu = "Žanr prihvata samo slova.";
                    }
                    else if (poruka.Contains("CHK_IZVODJAC_DRZAVA"))
                    {
                        kljucGreske = "DRZAVA";
                        porukaKlijentu = "Država izvođača sadrži nedozvoljene znakove. Dozvoljena su samo slova i razmaci.";
                    }
                    else if (poruka.Contains("CHK_IZVODJAC_KONTAKT_OSOBA"))
                    {
                        kljucGreske = "KONTAKT_OSOBA";
                        porukaKlijentu = "Kontakt osoba sadrži nedozvoljene znakove. Dozvoljena su samo slova, razmaci i crtica.";
                    }
                    else if (poruka.Contains("CHK_IZVODJAC_EMAIL"))
                    {
                        kljucGreske = "EMAIL";
                        porukaKlijentu = "Email mora biti u adekvatnom formatu (slova, brojevi, @).";
                    }
                    else if (poruka.Contains("CK_IZVODJAC_TIP"))
                    {
                        kljucGreske = "TIP";
                        porukaKlijentu = "Tip izvođača nije ispravno izabran!";
                    }
                    else if (poruka.Contains("UK_IZVODJAC_EMAIL"))
                    {
                        kljucGreske = "EMAIL";
                        porukaKlijentu = "Email izvođača već postoji! Unesite jedinstvenu email adresu.";
                    }
                    else if (poruka.Contains("UK_IZVODJAC_TELEFON"))
                    {
                        kljucGreske = "TELEFON";
                        porukaKlijentu = "Telefon izvođača već postoji! Unesite jedinstven broj telefona.";
                    }
                    else if (poruka.Contains("UQ_IZVODJAC_IME_DRZAVA_TIP"))
                    {
                        kljucGreske = "DUPLIKAT";
                        porukaKlijentu = "Izvođač sa istim imenom, državom porekla i tipom već postoji u bazi!";
                    }
                    else if (poruka.Contains("CHK_SOLO_TIP_INSTRUMENTA"))
                    {
                        kljucGreske = "TIP_INSTRUMENTA";
                        porukaKlijentu = "Za solo umetnika morate uneti instrument koji svira, u formi slova.";
                    }
                    else if (poruka.Contains("CHK_SOLO_SVIRA_INSTRUMENT"))
                    {
                        kljucGreske = "SVIRA_INSTRUMENT";
                        porukaKlijentu = "Za solo umetnika morate da izaberete da li svira instrument.";
                    }

                    if (!string.IsNullOrEmpty(kljucGreske))
                    {
                        throw new ValidacijaIzuzetka(kljucGreske, porukaKlijentu);
                    }
                    else
                    {
                        throw new Exception("Greška u bazi podataka. Detalji: " + ex.InnerException.Message);
                    }
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
                s.Flush();
                s.Close();

                return true;
            }
            catch (NHibernate.Exceptions.GenericADOException ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message != null)
                {
                    string poruka = ex.InnerException.Message;
                    string kljucGreske = "";
                    string porukaKlijentu = "";
                    if (poruka.Contains("CHK_IZVODJAC_ZAHTEV"))
                    {
                        kljucGreske = "ZAHTEV_SADRZAJ";
                        porukaKlijentu = "Zahtev mora da bude u formi slova ili brojeva i ne sme da sadrži nedozvoljene simbole.";
                    }
                    if (!string.IsNullOrEmpty(kljucGreske))
                    {
                        throw new ValidacijaIzuzetka(kljucGreske, porukaKlijentu);
                    }
                    else
                    {
                        throw new Exception("Greška u bazi podataka: Nepoznata validacija. Detalji: " + poruka);
                    }
                }
                else
                {
                    throw new Exception("Došlo je do greške u komunikaciji sa bazom.");
                }
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
            catch (NHibernate.Exceptions.GenericADOException ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message != null)
                {
                    string poruka = ex.InnerException.Message;
                    string kljucGreske = "";
                    string porukaKlijentu = "";

                    if (poruka.Contains("CHK_VOKALNI_NAZIV"))
                    {
                        kljucGreske = "VOKALNI_NAZIV";
                        porukaKlijentu = "Naziv vokalnih sposobnosti mora da bude u formi slova.";
                    }

                    if (!string.IsNullOrEmpty(kljucGreske))
                    {
                        throw new ValidacijaIzuzetka(kljucGreske, porukaKlijentu);
                    }
                    else
                    {
                        throw new Exception("Greška u bazi podataka: Nepoznata validacija. Detalji: " + poruka);
                    }
                }
                else
                {
                    throw new Exception("Došlo je do greške u komunikaciji sa bazom.");
                }
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
                IList<Lokacija> lokacije = s.Query<Lokacija>().OrderBy(a => a.NAZIV).ToList();

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
            catch (NHibernate.Exceptions.GenericADOException ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message != null)
                {
                    string poruka = ex.InnerException.Message;
                    string kljucGreske = "";
                    string porukaKlijentu = "";
                    if (poruka.Contains("CHK_LOKACIJA_MAX_KAPACITET"))
                    {
                        kljucGreske = "KAPACITET";
                        porukaKlijentu = "Maksimalni kapacitet mora biti veći od 0.";
                    }
                    else if (poruka.Contains("CHK_LOKACIJA_NAZIV"))
                    {
                        kljucGreske = "NAZIV";
                        porukaKlijentu = "Naziv lokacije je u formatu slova.";
                    }
                    else if (poruka.Contains("CHK_LOKACIJA_OPIS"))
                    {
                        kljucGreske = "OPIS";
                        porukaKlijentu = "Opis lokacije je u formatu slova.";
                    }
                    else if (poruka.Contains("CHK_LOKACIJA_TIP"))
                    {
                        kljucGreske = "TIP_LOKACIJE";
                        porukaKlijentu = "Izaberite adekvatan tip lokacije.";
                    }
                    else if (poruka.Contains("CHK_LOKACIJA_GPS_NOV"))
                    {
                        kljucGreske = "GPS_KOORDINATE";
                        porukaKlijentu = "GPS koordinate moraju biti u formatu brojeva.";
                    }
                    else if (poruka.Contains("UQ_LOKACIJA_NAZIV_GPS"))
                    {
                        kljucGreske = "DUPLIKAT";
                        porukaKlijentu = "Kombinacija naziva i GPS koordinata mora biti jedinstvena.";
                    }
                    else if (poruka.Contains("CHK_DOSTUPNOST_OPREME_NAZIV"))
                    {
                        kljucGreske = "OPREMA_NAZIV";
                        porukaKlijentu = "Naziv dostupne opreme mora biti u formatu slova.";
                    }
                    if (!string.IsNullOrEmpty(kljucGreske))
                    {
                        throw new ValidacijaIzuzetka(kljucGreske, porukaKlijentu);
                    }
                    else
                    {
                        throw new Exception("Greška u bazi podataka: Nepoznata validacija. Detalji: " + poruka);
                    }
                }
                else
                {
                    throw new Exception("Došlo je do greške u komunikaciji sa bazom.");
                }
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
                            ret = true;
                            break;
                    }
                }
                s.Flush();
                s.Close();

                return ret;
            }
            catch (NHibernate.Exceptions.GenericADOException ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message != null)
                {
                    string poruka = ex.InnerException.Message;
                    string kljucGreske = "";
                    string porukaKlijentu = "";
                    if (poruka.Contains("CHK_LOKACIJA_MAX_KAPACITET"))
                    {
                        kljucGreske = "KAPACITET";
                        porukaKlijentu = "Maksimalni kapacitet mora biti veći od 0.";
                    }
                    else if (poruka.Contains("CHK_LOKACIJA_NAZIV"))
                    {
                        kljucGreske = "NAZIV";
                        porukaKlijentu = "Naziv lokacije je u formatu slova.";
                    }
                    else if (poruka.Contains("CHK_LOKACIJA_OPIS"))
                    {
                        kljucGreske = "OPIS";
                        porukaKlijentu = "Opis lokacije je u formatu slova.";
                    }
                    else if (poruka.Contains("CHK_LOKACIJA_TIP"))
                    {
                        kljucGreske = "TIP_LOKACIJE";
                        porukaKlijentu = "Izaberite adekvatan tip lokacije.";
                    }
                    else if (poruka.Contains("CHK_LOKACIJA_GPS_NOV"))
                    {
                        kljucGreske = "GPS_KOORDINATE";
                        porukaKlijentu = "GPS koordinate moraju biti u formatu brojeva.";
                    }
                    else if (poruka.Contains("UQ_LOKACIJA_NAZIV_GPS"))
                    {
                        kljucGreske = "DUPLIKAT";
                        porukaKlijentu = "Kombinacija naziva i GPS koordinata mora biti jedinstvena.";
                    }
                    else if (poruka.Contains("CHK_DOSTUPNOST_OPREME_NAZIV"))
                    {
                        kljucGreske = "OPREMA_NAZIV";
                        porukaKlijentu = "Naziv dostupne opreme mora biti u formatu slova.";
                    }
                    if (!string.IsNullOrEmpty(kljucGreske))
                    {
                        throw new ValidacijaIzuzetka(kljucGreske, porukaKlijentu);
                    }
                    else
                    {
                        throw new Exception("Greška u bazi podataka: Nepoznata validacija. Detalji: " + poruka);
                    }
                }
                else
                {
                    throw new Exception("Došlo je do greške u komunikaciji sa bazom.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return false;
            }
        }

        public static bool ObrisiLokaciju(int idLokacije)
        {
            try
            {
                ISession s = DataLayer.GetSession();
                Lokacija l = s.Get<Lokacija>(idLokacije);

                if (l == null)
                    return false;

                s.Delete(l);
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

        #region DostupnaOprema

        public static IList<DostupnaOpremaView> VratiSvuDostupnuOpremu(int idLokacije)
        {
            IList<DostupnaOpremaView> opremaView = new List<DostupnaOpremaView>();
            try
            {
                ISession s = DataLayer.GetSession();
                IList<DostupnaOprema> oprema = s.Query<DostupnaOprema>()
                    .Where(d => d.Lokacija.ID == idLokacije)
                    .OrderBy(a => a.Lokacija.NAZIV)
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

                    l.DOSTUPNA_OPREMA.Add(oprema);
                    oprema.Lokacija = l;

                    s.SaveOrUpdate(l);
                    transaction.Commit();

                    return new DostupnaOpremaView(oprema.ID, oprema.NAZIV);
                }
            }
            catch (NHibernate.Exceptions.GenericADOException ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message != null)
                {
                    string poruka = ex.InnerException.Message;
                    string kljucGreske = "";
                    string porukaKlijentu = "";

                     if (poruka.Contains("CHK_DOSTUPNOST_OPREME_NAZIV"))
                    {
                        kljucGreske = "OPREMA_NAZIV";
                        porukaKlijentu = "Naziv dostupne opreme mora biti u formatu slova.";
                    }

                    if (!string.IsNullOrEmpty(kljucGreske))
                    {
                        throw new ValidacijaIzuzetka(kljucGreske, porukaKlijentu);
                    }
                    else
                    {
                        throw new Exception("Greška u bazi podataka: Nepoznata validacija. Detalji: " + poruka);
                    }
                }
                else
                {
                    throw new Exception("Došlo je do greške u komunikaciji sa bazom.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public static bool ObrisiDostupnuOpremu(int iddostupne)
        {
            try
            {
                ISession s = DataLayer.GetSession();
                DostupnaOprema oprema = s.Get<DostupnaOprema>(iddostupne);
                if (oprema == null)
                    return false;
                s.Delete(oprema);
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

        #region MenadzerskaAgencija

        public static IList<MenadzerskaAgencijaView> VratiSveMenadzerskeAgencije()
        {
            try
            {
                ISession s = DataLayer.GetSession();
                IList<MenadzerskaAgencija> agencije = s.Query<MenadzerskaAgencija>().OrderBy(a => a.NAZIV).ToList();
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
            catch (NHibernate.Exceptions.GenericADOException ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message != null)
                {
                    string poruka = ex.InnerException.Message;
                    string kljucGreske = "";
                    string porukaKlijentu = "";

                    if (poruka.Contains("CK_MENADZERSKA_AGENCIJA_NAZIV"))
                    {
                        kljucGreske = "AGENCIJA_NAZIV";
                        porukaKlijentu = "Naziv menadžerske agencije sadrži nedozvoljene znakove. Dozvoljena su samo slova i razmaci.";
                    }
                    else if (poruka.Contains("UQ_KONTAKT_OSOBA"))
                    {
                        kljucGreske = "KONTAKT_OSOBA_DUPLIKAT";
                        porukaKlijentu = "Kontakt osoba sa istim imenom već postoji u okviru ove agencije. Unesite jedinstveno ime.";
                    }
                    else if (poruka.Contains("CK_MENADZESKA_AGENCIJA_ADRESA"))
                    {
                        kljucGreske = "AGENCIJA_ADRESA";
                        porukaKlijentu = "Adresa menadžerske agencije sadrži nedozvoljene znakove. Dozvoljena su samo slova, brojevi i razmaci.";
                    }
                    else if (poruka.Contains("CK_M_AGENCIJA_KONTAKT_OSOBA"))
                    {
                        kljucGreske = "KONTAKT_OSOBA_IME";
                        porukaKlijentu = "Ime kontakt osobe menadžerske agencije sadrži nedozvoljene znakove. Dozvoljena su samo slova i razmaci.";
                    }

                    if (!string.IsNullOrEmpty(kljucGreske))
                    {
                        throw new ValidacijaIzuzetka(kljucGreske, porukaKlijentu);
                    }
                    else
                    {
                        throw new Exception("Greška u bazi podataka: Nepoznata validacija. Detalji: " + poruka);
                    }
                }
                else
                {
                    throw new Exception("Došlo je do greške u komunikaciji sa bazom.");
                }
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
            catch (NHibernate.Exceptions.GenericADOException ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message != null)
                {
                    string poruka = ex.InnerException.Message;
                    string kljucGreske = "";
                    string porukaKlijentu = "";

                    if (poruka.Contains("CK_MENADZERSKA_AGENCIJA_NAZIV"))
                    {
                        kljucGreske = "AGENCIJA_NAZIV";
                        porukaKlijentu = "Naziv menadžerske agencije sadrži nedozvoljene znakove. Dozvoljena su samo slova i razmaci.";
                    }
                    else if (poruka.Contains("UQ_KONTAKT_OSOBA"))
                    {
                        kljucGreske = "KONTAKT_OSOBA_DUPLIKAT";
                        porukaKlijentu = "Kontakt osoba sa istim imenom već postoji u okviru ove agencije. Unesite jedinstveno ime.";
                    }
                    else if (poruka.Contains("CK_MENADZESKA_AGENCIJA_ADRESA"))
                    {
                        kljucGreske = "AGENCIJA_ADRESA";
                        porukaKlijentu = "Adresa menadžerske agencije sadrži nedozvoljene znakove. Dozvoljena su samo slova, brojevi i razmaci.";
                    }
                    else if (poruka.Contains("CK_M_AGENCIJA_KONTAKT_OSOBA"))
                    {
                        kljucGreske = "KONTAKT_OSOBA_IME";
                        porukaKlijentu = "Ime kontakt osobe menadžerske agencije sadrži nedozvoljene znakove. Dozvoljena su samo slova i razmaci.";
                    }

                    if (!string.IsNullOrEmpty(kljucGreske))
                    {
                        throw new ValidacijaIzuzetka(kljucGreske, porukaKlijentu);
                    }
                    else
                    {
                        throw new Exception("Greška u bazi podataka: Nepoznata validacija. Detalji: " + poruka);
                    }
                }
                else
                {
                    throw new Exception("Došlo je do greške u komunikaciji sa bazom.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return false;
            }
        }

        public static bool ObrisiMenadzerskuAgenciju(int id)
        {
            try
            {
                ISession s = DataLayer.GetSession();
                MenadzerskaAgencija ma = s.Get<MenadzerskaAgencija>(id);

                if (ma == null) return false;

                s.Delete(ma);
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
                            //case IzvodjacTip.SOLO_UMETNIK:
                            //    Solo_Umetnik u = i as Solo_Umetnik;
                            //    izvodjaciView.Add(new Solo_umetnikView(u.ID, u.IME, i.DRZAVA_POREKLA, u.EMAIL, u.KONTAKT_OSOBA, u.TELEFON, u.Zanr, u.SVIRA_INSTRUMENT, u.TIP_INSTRUMENTA));
                            //    break;
                            //case IzvodjacTip.BEND:
                            //    Bend b = i as Bend;
                            //    izvodjaciView.Add(new BendView(b.ID, b.IME, b.DRZAVA_POREKLA, b.EMAIL, b.KONTAKT_OSOBA, b.TELEFON, b.Zanr, b.BROJ_CLANOVA));
                            //    break;
                            case IzvodjacTip.SOLO_UMETNIK:
                                Solo_Umetnik u = i as Solo_Umetnik;
                                if (u != null)
                                {
                                    izvodjaciView.Add(new Solo_umetnikView(
                                        u.ID,
                                        u.IME ?? "",
                                        u.DRZAVA_POREKLA ?? "",
                                        u.EMAIL ?? "",
                                        u.KONTAKT_OSOBA ?? "",
                                        u.TELEFON ?? "",
                                        u.Zanr ?? "",
                                        u.SVIRA_INSTRUMENT ?? "",
                                        u.TIP_INSTRUMENTA ?? ""
                                    ));
                                }
                                break;

                            case IzvodjacTip.BEND:
                                Bend b = i as Bend;
                                if (b != null)
                                {
                                    izvodjaciView.Add(new BendView(
                                        b.ID,
                                        b.IME ?? "",
                                        b.DRZAVA_POREKLA ?? "",
                                        b.EMAIL ?? "",
                                        b.KONTAKT_OSOBA ?? "",
                                        b.TELEFON ?? "",
                                        b.Zanr ?? "",
                                        b.BROJ_CLANOVA
                                    ));
                                }
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
                    .OrderBy(a => a.MENADZERSKA_AGENCIJA.NAZIV)
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
            catch (NHibernate.Exceptions.GenericADOException ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message != null)
                {
                    string poruka = ex.InnerException.Message;
                    string kljucGreske = "";
                    string porukaKlijentu = "";

                    if (poruka.Contains("CK_TIP_KONTAKTA"))
                    {
                        kljucGreske = "TIP_KONTAKTA";
                        porukaKlijentu = "Tip kontakta (email/telefon) mora biti u ispravnom formatu i imati tacno cifara u broju.";
                    }
                    else if (poruka.Contains("UK_MENADZERSKA_KONTAKT"))
                    {
                        kljucGreske = "DUPLIKAT_KONTAKT";
                        porukaKlijentu = "Kontakt podaci moraju biti jedinstveni, ne smete uneti duplikate.";
                    }
                    else if (poruka.Contains("CHK_TELEFON_EMAIL"))
                    {
                        kljucGreske = "VREDNOST_FORMAT";
                        porukaKlijentu = "Vrednost kontakta mora biti ispravno napisana (npr. validan email ili broj telefona).";
                    }

                    if (!string.IsNullOrEmpty(kljucGreske))
                    {
                        throw new ValidacijaIzuzetka(kljucGreske, porukaKlijentu);
                    }
                    else
                    {
                        throw new Exception("Greška u bazi podataka: Nepoznata validacija. Detalji: " + poruka);
                    }
                }
                else
                {
                    throw new Exception("Došlo je do greške u komunikaciji sa bazom.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Greška prilikom dodavanja kontakta: " + ex.Message);
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
                List<Dogadjaj> dogadjaji = s.Query<Dogadjaj>().OrderBy(a => a.NAZIV).ToList();

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

                return new DogadjajView(id, db.Naziv, db.Tip, db.Opis, db.DatumPocetka, db.DatumKraja, l.NAZIV);
            }
            catch (NHibernate.Exceptions.GenericADOException ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message != null)
                {
                    string poruka = ex.InnerException.Message;
                    string kljucGreske = "";
                    string porukaKlijentu = "";

                    if (poruka.Contains("CHK_DOGADJAJ_DATUMI"))
                    {
                        kljucGreske = "DATUMI";
                        porukaKlijentu = "Datum završetka događaja mora biti veći od datuma početka.";
                    }
                    else if (poruka.Contains("CHK_DOGADJAJ_TIP"))
                    {
                        kljucGreske = "TIP_DOGADJAJA";
                        porukaKlijentu = "Tip događaja mora biti adekvatno izabran.";
                    }
                    else if (poruka.Contains("UQ_DOGADJAJ_NAZIV_LOKACIJA"))
                    {
                        kljucGreske = "DUPLIKAT";
                        porukaKlijentu = "Kombinacija naziva, lokacije i datuma početka događaja mora biti jedinstvena.";
                    }
                    else if (poruka.Contains("CHK_DOGADJAJ_OPIS"))
                    {
                        kljucGreske = "OPIS";
                        porukaKlijentu = "Opis dogadjaja mora da bude u formi slova";
                    }

                    if (!string.IsNullOrEmpty(kljucGreske))
                    {
                        throw new ValidacijaIzuzetka(kljucGreske, porukaKlijentu);
                    }
                    else
                    {
                        throw new Exception("Greška u bazi podataka: Nepoznata validacija. Detalji: " + poruka);
                    }
                }
                else
                {
                    throw new Exception("Došlo je do greške u komunikaciji sa bazom.");
                }
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
                    if (u.KUPAC_ID != null)
                    {
                        var p = new PosetilacView(u.KUPAC_ID.ID, u.KUPAC_ID.IME, u.KUPAC_ID.PREZIME, u.KUPAC_ID.EMAIL, u.KUPAC_ID.Telefon);
                        p.UlaznicaTip = u.TIP_ULAZNICE;
                        posetilacViews.Add(p);
                    }
                    else
                    {
                        Console.WriteLine($"Ulaznica sa ID {u.ID_ULAZNICE} nema dodeljenog kupca!");
                    }
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

                i.Dogadjaji.Add(d);
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

        public static IList<PosetilacView> VratiSvePosetioce()
        {
            try
            {
                ISession s = DataLayer.GetSession();
                IList<Posetilac> lista = s.Query<Posetilac>().OrderBy(a => a.IME).ToList();

                List<PosetilacView> views = new List<PosetilacView>();
                foreach (Posetilac poset in lista)
                {
                    views.Add(new PosetilacView(poset.ID, poset.IME, poset.PREZIME, poset.EMAIL, poset.Telefon));
                }

                return views;
                ;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new List<PosetilacView>();
            }
        }

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
                        var datumi = (pb.Ulaznica as ViseDnevnaBasic).DatumiVazenja;
                        u = new Visednevna
                        {
                            OSNOVNA_CENA = pb.Ulaznica.OsnovnaCena,
                            NACIN_PLACANJA = pb.Ulaznica.NacinPlacanja,
                            DATUM_KUPOVINE = pb.Ulaznica.DatumKupovine,
                            TIP_ULAZNICE = TipUlaznice.VISEDNEVNA,
                            Dogadjaj = d,
                            Dani = datumi,
                            BROJ_DANA = datumi.Count

                        };
                        break;
                    case TipUlaznice.VIP:
                        u = new Vip
                        {
                            OSNOVNA_CENA = pb.Ulaznica.OsnovnaCena,
                            NACIN_PLACANJA = pb.Ulaznica.NacinPlacanja,
                            DATUM_KUPOVINE = pb.Ulaznica.DatumKupovine,
                            TIP_ULAZNICE = TipUlaznice.VIP,
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
                            TIP_ULAZNICE = TipUlaznice.AKREDITACIJA,
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
                p.Ulaznica = u;

                s.SaveOrUpdate(p);
                s.Flush();

                if (pb.Grupa != null)
                {
                    Grupa g = s.Get<Grupa>(pb.Grupa.Id);
                    g.Clanovi.Add(p);
                    s.Update(g);
                    s.Flush();
                }

                s.Close();

                return new PosetilacView(p.ID, pb.Ime, pb.Prezime, pb.Email, pb.Telefon);
            }
            catch (NHibernate.Exceptions.GenericADOException ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message != null)
                {
                    string poruka = ex.InnerException.Message;
                    string kljucGreske = "";
                    string porukaKlijentu = "";

                    if (poruka.Contains("UK_POSETILAC_EMAIL"))
                    {
                        kljucGreske = "EMAIL";
                        porukaKlijentu = "Email mora biti jedinstven.";
                    }
                    else if (poruka.Contains("UK_POSETILAC_TELEFON"))
                    {
                        kljucGreske = "TELEFON";
                        porukaKlijentu = "Telefon mora biti jedinstven.";
                    }
                    else if (poruka.Contains("CHK_POSETILAC_TELEFON"))
                    {
                        kljucGreske = "TELEFON_FORMAT";
                        porukaKlijentu = "Broj telefona mora biti u adekvatnom formatu, da sadrži brojeve i 10 cifara.";
                    }
                    else if (poruka.Contains("CHK_POSETILAC_EMAIL"))
                    {
                        kljucGreske = "EMAIL_FORMAT";
                        porukaKlijentu = "Email mora biti u adekvatnom formatu, da sadrži slova, brojeve, @.";
                    }
                    else if (poruka.Contains("CHK_POSETILAC_PREZIME"))
                    {
                        kljucGreske = "PREZIME";
                        porukaKlijentu = "Prezime mora biti u adekvatnom formatu, da sadrži slova.";
                    }
                    else if (poruka.Contains("CHK_POSETILAC_IME"))
                    {
                        kljucGreske = "IME";
                        porukaKlijentu = "Ime mora biti u adekvatnom formatu, da sadrži slova.";
                    }
                    else if (poruka.Contains("CHK_VIP_POGODNOST_NAZIV"))
                    {
                        kljucGreske = "VIP_POGODNOST_NAZIV";
                        porukaKlijentu = "Naziv VIP pogodnosti je u formatu slova.";
                    }

                    if (!string.IsNullOrEmpty(kljucGreske))
                    {
                        throw new ValidacijaIzuzetka(kljucGreske, porukaKlijentu);
                    }
                    else
                    {
                        throw new Exception("Greška u bazi podataka: Nepoznata validacija. Detalji: " + poruka);
                    }
                }
                else
                {
                    throw new Exception("Došlo je do greške u komunikaciji sa bazom.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public static PosetilacView IzmeniPosetioca(PosetilacBasic pb)
        {
            try
            {
                ISession s = DataLayer.GetSession();
                Posetilac p = s.Get<Posetilac>(pb.Id);

                if (p == null)
                    return null;

                p.IME = pb.Ime;
                p.PREZIME = pb.Prezime;
                p.EMAIL = pb.Email;
                p.Telefon = pb.Telefon;

                s.Update(p);
                s.Flush();
                s.Close();

                return new PosetilacView(p.ID, p.IME, p.PREZIME, p.EMAIL, p.Telefon);
            }
            catch (NHibernate.Exceptions.GenericADOException ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message != null)
                {
                    string poruka = ex.InnerException.Message;
                    string kljucGreske = "";
                    string porukaKlijentu = "";

                    if (poruka.Contains("UK_POSETILAC_EMAIL"))
                    {
                        kljucGreske = "EMAIL";
                        porukaKlijentu = "Email mora biti jedinstven.";
                    }
                    else if (poruka.Contains("UK_POSETILAC_TELEFON"))
                    {
                        kljucGreske = "TELEFON";
                        porukaKlijentu = "Telefon mora biti jedinstven.";
                    }
                    else if (poruka.Contains("CHK_POSETILAC_TELEFON"))
                    {
                        kljucGreske = "TELEFON_FORMAT";
                        porukaKlijentu = "Broj telefona mora biti u adekvatnom formatu, da sadrži brojeve i 10 cifara.";
                    }
                    else if (poruka.Contains("CHK_POSETILAC_EMAIL"))
                    {
                        kljucGreske = "EMAIL_FORMAT";
                        porukaKlijentu = "Email mora biti u adekvatnom formatu, da sadrži slova, brojeve, @.";
                    }
                    else if (poruka.Contains("CHK_POSETILAC_PREZIME"))
                    {
                        kljucGreske = "PREZIME";
                        porukaKlijentu = "Prezime mora biti u adekvatnom formatu, da sadrži slova.";
                    }
                    else if (poruka.Contains("CHK_POSETILAC_IME"))
                    {
                        kljucGreske = "IME";
                        porukaKlijentu = "Ime mora biti u adekvatnom formatu, da sadrži slova.";
                    }
                    else if (poruka.Contains("CHK_VIP_POGODNOST_NAZIV"))
                    {
                        kljucGreske = "VIP_POGODNOST_NAZIV";
                        porukaKlijentu = "Naziv VIP pogodnosti je u formatu slova.";
                    }

                    if (!string.IsNullOrEmpty(kljucGreske))
                    {
                        throw new ValidacijaIzuzetka(kljucGreske, porukaKlijentu);
                    }
                    else
                    {
                        throw new Exception("Greška u bazi podataka: Nepoznata validacija. Detalji: " + poruka);
                    }
                }
                else
                {
                    throw new Exception("Došlo je do greške u komunikaciji sa bazom.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public static bool ObrisiPosetioca(int idPosetioca)
        {
            try
            {
                ISession s = DataLayer.GetSession();
                Posetilac p = s.Get<Posetilac>(idPosetioca);

                if (p == null)
                    return false;

                s.Delete(p);
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

        public static UlaznicaBasic VratiUlaznicuPosetioca(int idPosetioca)
        {
            try
            {
                ISession s = DataLayer.GetSession();
                Ulaznica u = s.Query<Ulaznica>().Where(ul => ul.KUPAC_ID.ID == idPosetioca).FirstOrDefault();

                if (u == null)
                    return null;

                UlaznicaBasic ret = null;
                switch (u.TIP_ULAZNICE)
                {
                    case TipUlaznice.JEDNODNEVNA:
                        ret = new JednodnevnaBasic(u.ID_ULAZNICE, u.OSNOVNA_CENA, u.NACIN_PLACANJA, u.DATUM_KUPOVINE, null, (u as Jednodnevna).DAN_VAZENJA);
                        break;

                    case TipUlaznice.VISEDNEVNA:
                        ret = new ViseDnevnaBasic(u.ID_ULAZNICE, u.OSNOVNA_CENA, u.NACIN_PLACANJA, u.DATUM_KUPOVINE, null, (u as Visednevna).Dani);
                        break;

                    case TipUlaznice.VIP:
                        ret = new VIPBasic(u.ID_ULAZNICE, u.OSNOVNA_CENA, u.NACIN_PLACANJA, u.DATUM_KUPOVINE, null, (u as Vip).Pogodnosti);
                        break;

                    case TipUlaznice.AKREDITACIJA:
                        ret = new AkreditacijaBasic(u.ID_ULAZNICE, u.OSNOVNA_CENA, u.NACIN_PLACANJA, u.DATUM_KUPOVINE, null, (u as Akreditacija).TIP);
                        break;
                }

                return ret;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public static GrupaView VratiGrupuPosetioca(int idPosetioca)
        {
            try
            {
                ISession s = DataLayer.GetSession();
                Posetilac p = s.Get<Posetilac>(idPosetioca);

                if (p == null)
                    return null;

                if (p.GRUPA == null)
                    return null;

                Grupa g = p.GRUPA;

                List<string> imena = new List<string>();
                foreach (var c in g.Clanovi)
                {
                    imena.Add(c.IME);
                }

                GrupaView gb = new GrupaView(g.ID_GRUPE, g.NAZIV, g.AgencijaID.NAZIV, imena);

                return gb;

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public static bool IzbaciIzGrupe(int idPosetioca, int idGrupe)
        {
            try
            {
                ISession s = DataLayer.GetSession();
                Posetilac p = s.Get<Posetilac>(idPosetioca);
                Grupa g = s.Get<Grupa>(idGrupe);

                if (p == null || g == null)
                    return false;

                if (p.GRUPA.ID_GRUPE != g.ID_GRUPE)
                    return false;

                g.Clanovi.Remove(p);
                p.GRUPA = null;

                s.Update(g);
                s.Update(p);

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

        #region AgencijaOrganizator

        public static IList<AgencijaOrganizatorView> VratiSveAgencije()
        {
            try
            {
                ISession s = DataLayer.GetSession();

                IList<AgencijaOrganizator> agencije = s.Query<AgencijaOrganizator>().OrderBy(a => a.NAZIV).ToList();

                List<AgencijaOrganizatorView> views = new List<AgencijaOrganizatorView>();

                foreach (var a in agencije)
                {
                    views.Add(new AgencijaOrganizatorView(a.ID, a.NAZIV, a.ADRESA));
                    ;
                }

                return views;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new List<AgencijaOrganizatorView>();
            }
        }

        public static AgencijaOrganizatorView DodajAgenciju(AgencijaOrganizatorBasic ab)
        {
            try
            {
                ISession s = DataLayer.GetSession();

                AgencijaOrganizator nova = new AgencijaOrganizator
                {
                    NAZIV = ab.Naziv,
                    ADRESA = ab.Adresa
                };

                int Id = (int)s.Save(nova);

                s.Flush();
                s.Close();

                return new AgencijaOrganizatorView(nova.ID, nova.NAZIV, nova.ADRESA);
            }
            //greske za unique 
            catch (NHibernate.Exceptions.GenericADOException ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message != null)
                {
                    string poruka = ex.InnerException.Message;
                    string kljucGreske = "";
                    string porukaKlijentu = "";

                    if (poruka.Contains("CK_AGENCIJA_NAZIV_FORMAT"))
                    {
                        kljucGreske = "NAZIV_FORMAT";
                        porukaKlijentu = "Naziv agencije sadrži nedozvoljene znakove. Dozvoljena su samo slova i razmaci.";
                    }
                    else if (poruka.Contains("CK_AGENCIJA_ADRESA_FORMAT"))
                    {
                        kljucGreske = "ADRESA_FORMAT";
                        porukaKlijentu = "Adresa mora biti u formatu: Ulica Broj[, Grad]";
                    }
                    else if (poruka.Contains("UQ_AGENCIJA_NAZIV"))
                    {
                        kljucGreske = "DUPLIKAT_NAZIV";
                        porukaKlijentu = "Agencija sa ovim nazivom već postoji.";
                    }

                    if (!string.IsNullOrEmpty(kljucGreske))
                    {
                        throw new ValidacijaIzuzetka(kljucGreske, porukaKlijentu);
                    }
                    else
                    {
                        throw new Exception("Greška u bazi podataka: Nepoznata validacija. Detalji: " + poruka);
                    }
                }
                else
                {
                    throw new Exception("Došlo je do greške u komunikaciji sa bazom.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public static bool ObrisiAgenciju(int agencijaId)
        {
            try
            {
                ISession s = DataLayer.GetSession();
                AgencijaOrganizator a = s.Get<AgencijaOrganizator>(agencijaId);

                if (a == null)
                {
                    return false;
                }

                s.Delete(a);
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

        public static bool IzmeniAgencijuOrganizator(AgencijaOrganizatorBasic ab)
        {
            try
            {
                ISession s = DataLayer.GetSession();
                AgencijaOrganizator a = s.Get<AgencijaOrganizator>(ab.Id);

                if (a == null)
                    return false;

                a.NAZIV = ab.Naziv;
                a.ADRESA = ab.Adresa;

                s.Update(a);
                s.Flush();
                s.Close();

                return true;
            }
            //greske za unique 
            catch (NHibernate.Exceptions.GenericADOException ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message != null)
                {
                    string poruka = ex.InnerException.Message;
                    string kljucGreske = "";
                    string porukaKlijentu = "";

                    if (poruka.Contains("CK_AGENCIJA_NAZIV_FORMAT"))
                    {
                        kljucGreske = "NAZIV_FORMAT";
                        porukaKlijentu = "Naziv agencije sadrži nedozvoljene znakove. Dozvoljena su samo slova i razmaci.";
                    }
                    else if (poruka.Contains("CK_AGENCIJA_ADRESA_FORMAT"))
                    {
                        kljucGreske = "ADRESA_FORMAT";
                        porukaKlijentu = "Adresa sadrži nedozvoljene znakove. Dozvoljena su slova, brojevi i . , - znakovi.";
                    }
                    else if (poruka.Contains("UQ_AGENCIJA_NAZIV"))
                    {
                        kljucGreske = "DUPLIKAT_NAZIV";
                        porukaKlijentu = "Agencija sa ovim nazivom već postoji.";
                    }

                    if (!string.IsNullOrEmpty(kljucGreske))
                    {
                        throw new ValidacijaIzuzetka(kljucGreske, porukaKlijentu);
                    }
                    else
                    {
                        throw new Exception("Greška u bazi podataka: Nepoznata validacija. Detalji: " + poruka);
                    }
                }
                else
                {
                    throw new Exception("Došlo je do greške u komunikaciji sa bazom.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        #endregion

        #region Grupa

        public static IList<GrupaView> VratiSveGrupe()
        {
            try
            {
                ISession s = DataLayer.GetSession();
                IList<Grupa> grupe = s.Query<Grupa>().OrderBy(a => a.NAZIV).ToList();
                List<GrupaView> views = new List<GrupaView>();
                foreach (var g in grupe)
                {
                    List<string> imena = new List<string>();
                    foreach (var p in g.Clanovi)
                    {
                        imena.Add(p.IME);
                    }

                    views.Add(new GrupaView(g.ID_GRUPE, g.NAZIV, g.AgencijaID.NAZIV, imena));
                }

                return views;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new List<GrupaView>();
            }
        }

        public static GrupaView DodajGrupu(GrupaBasic gb)
        {
            try
            {
                ISession s = DataLayer.GetSession();
                AgencijaOrganizator ag = s.Get<AgencijaOrganizator>(gb.Agencija.Id);

                if (ag == null)
                {
                    return null;
                }

                Grupa nova = new Grupa
                {
                    NAZIV = gb.Naziv,
                    AgencijaID = ag
                };

                ag.Grupe.Add(nova);

                s.Update(ag);
                s.Flush();
                s.Close();

                return new GrupaView(nova.ID_GRUPE, nova.NAZIV, nova.AgencijaID.NAZIV, new List<string>());
            }
            //greske za unique 
            catch (NHibernate.Exceptions.GenericADOException ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message != null)
                {
                    string poruka = ex.InnerException.Message;
                    string kljucGreske = "";
                    string porukaKlijentu = "";

                    if (poruka.Contains("CHK_GRUPA_NAZIV"))
                    {
                        kljucGreske = "NAZIV_FORMAT";
                        porukaKlijentu = "Naziv grupe sadrži nedozvoljene znakove. Dozvoljena su samo slova, brojevi i razmaci.";
                    }
                    else if (poruka.Contains("UQ_GRUPA_NAZIV_AGENCIJA"))
                    {
                        kljucGreske = "DUPLIKAT_NAZIV";
                        porukaKlijentu = "Grupa sa istim nazivom već postoji u okviru ove agencije. Unesite jedinstven naziv.";
                    }
                    else if (poruka.Contains("FK_GRUPA_AGENCIJA_ORGANIZATOR"))
                    {
                        kljucGreske = "AGENCIJA_NE_POSTOJI";
                        porukaKlijentu = "Izabrana agencija ne postoji ili je obrisana. Proverite unos agencije.";
                    }

                    if (!string.IsNullOrEmpty(kljucGreske))
                    {
                        throw new ValidacijaIzuzetka(kljucGreske, porukaKlijentu);
                    }
                    else
                    {
                        throw new Exception("Greška u bazi podataka: Nepoznata validacija. Detalji: " + poruka);
                    }
                }
                else
                {
                    throw new Exception("Došlo je do greške u komunikaciji sa bazom.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public static bool DodajClanaGrupi(int grupaId, int clanId)
        {
            try
            {
                ISession s = DataLayer.GetSession();
                Grupa g = s.Get<Grupa>(grupaId);
                Posetilac p = s.Get<Posetilac>(clanId);

                if (g == null || p == null)
                {
                    return false;
                }

                if (g.Clanovi.Contains(p))
                    return true;

                g.Clanovi.Add(p);
                s.Update(g);
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

        public static bool IzmeniGrupu(GrupaBasic gb)
        {
            try
            {
                ISession s = DataLayer.GetSession();
                Grupa g = s.Get<Grupa>(gb.Id);

                if (g == null)
                    return false;

                g.NAZIV = gb.Naziv;

                if (gb.Agencija != null && (g.AgencijaID.ID != gb.Agencija.Id))
                {
                    AgencijaOrganizator ag = s.Get<AgencijaOrganizator>(g.AgencijaID.ID);
                    ag.Grupe.Remove(g);
                    s.Update(ag);
                    s.Flush();

                    AgencijaOrganizator nova = s.Get<AgencijaOrganizator>(gb.Agencija.Id);
                    if (nova == null)
                    {
                        s.Close();
                        return false;
                    }

                    nova.Grupe.Add(g);
                    g.AgencijaID = nova;

                    s.Update(nova);
                    s.Flush();
                }

                s.Update(g);
                s.Flush();

                s.Close();
                return true;
            }
            catch (NHibernate.Exceptions.GenericADOException ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message != null)
                {
                    string poruka = ex.InnerException.Message;
                    string kljucGreske = "";
                    string porukaKlijentu = "";

                    if (poruka.Contains("CHK_GRUPA_NAZIV"))
                    {
                        kljucGreske = "NAZIV_FORMAT";
                        porukaKlijentu = "Naziv grupe sadrži nedozvoljene znakove. Dozvoljena su samo slova, brojevi i razmaci.";
                    }
                    else if (poruka.Contains("UQ_GRUPA_NAZIV_AGENCIJA"))
                    {
                        kljucGreske = "DUPLIKAT_NAZIV";
                        porukaKlijentu = "Grupa sa istim nazivom već postoji u okviru ove agencije. Unesite jedinstven naziv.";
                    }
                    else if (poruka.Contains("FK_GRUPA_AGENCIJA_ORGANIZATOR"))
                    {
                        kljucGreske = "AGENCIJA_NE_POSTOJI";
                        porukaKlijentu = "Izabrana agencija ne postoji ili je obrisana. Proverite unos agencije.";
                    }

                    if (!string.IsNullOrEmpty(kljucGreske))
                    {
                        throw new ValidacijaIzuzetka(kljucGreske, porukaKlijentu);
                    }
                    else
                    {
                        throw new Exception("Greška u bazi podataka: Nepoznata validacija. Detalji: " + poruka);
                    }
                }
                else
                {
                    throw new Exception("Došlo je do greške u komunikaciji sa bazom.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public static bool ObrisiGrupu(int grupaId)
        {
            try
            {
                ISession s = DataLayer.GetSession();
                Grupa g = s.Get<Grupa>(grupaId);
                if (g == null)
                    return false;

                s.Delete(g);
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

        #region Ulaznice
        public static IList<UlaznicaBasic> VratiSveUlaznice()
        {
            try
            {
                ISession s = DataLayer.GetSession();
                IList<Ulaznica> ulaznice = s.Query<Ulaznica>().OrderBy(a => a.TIP_ULAZNICE).ToList();

                IList<UlaznicaBasic> basics = new List<UlaznicaBasic>();

                foreach (var u in ulaznice)
                {
                    switch (u.TIP_ULAZNICE)
                    {
                        case TipUlaznice.JEDNODNEVNA:
                            basics.Add(new JednodnevnaBasic(u.ID_ULAZNICE, u.OSNOVNA_CENA, u.NACIN_PLACANJA, u.DATUM_KUPOVINE, null, (u as Jednodnevna).DAN_VAZENJA));
                            break;
                        case TipUlaznice.VISEDNEVNA:
                            basics.Add(new ViseDnevnaBasic(u.ID_ULAZNICE, u.OSNOVNA_CENA, u.NACIN_PLACANJA, u.DATUM_KUPOVINE, null, (u as Visednevna).Dani.ToList()));
                            break;
                        case TipUlaznice.VIP:
                            basics.Add(new VIPBasic(u.ID_ULAZNICE, u.OSNOVNA_CENA, u.NACIN_PLACANJA, u.DATUM_KUPOVINE, null, (u as Vip).Pogodnosti.ToList()));
                            break;
                        case TipUlaznice.AKREDITACIJA:
                            basics.Add(new AkreditacijaBasic(u.ID_ULAZNICE, u.OSNOVNA_CENA, u.NACIN_PLACANJA, u.DATUM_KUPOVINE, null, (u as Akreditacija).TIP));
                            break;
                        default:
                            throw new Exception("Nepravilna ulaznica!");
                    }
                }

                return basics;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new List<UlaznicaBasic>();
            }
        }

        public static bool IzmeniUlaznicu(UlaznicaBasic ub)
        {
            try
            {
                ISession s = DataLayer.GetSession();
                Ulaznica u = s.Get<Ulaznica>(ub.Id);

                if (u == null)
                {
                    return false;
                }

                switch (u.TIP_ULAZNICE)
                {
                    case TipUlaznice.JEDNODNEVNA:
                        Jednodnevna j = u as Jednodnevna;
                        j.OSNOVNA_CENA = ub.OsnovnaCena;
                        j.NACIN_PLACANJA = ub.NacinPlacanja;
                        j.DATUM_KUPOVINE = ub.DatumKupovine;
                        j.DAN_VAZENJA = (ub as JednodnevnaBasic).DatumVazenja;

                        s.Update(j);
                        break;
                    case TipUlaznice.VISEDNEVNA:
                        Visednevna v = u as Visednevna;
                        v.OSNOVNA_CENA = ub.OsnovnaCena;
                        v.NACIN_PLACANJA = ub.NacinPlacanja;
                        v.DATUM_KUPOVINE = ub.DatumKupovine;
                        v.Dani = (ub as ViseDnevnaBasic).DatumiVazenja.ToList();

                        s.Update(v);
                        break;
                    case TipUlaznice.VIP:
                        Vip vi = u as Vip;
                        vi.OSNOVNA_CENA = ub.OsnovnaCena;
                        vi.NACIN_PLACANJA = ub.NacinPlacanja;
                        vi.DATUM_KUPOVINE = ub.DatumKupovine;
                        vi.Pogodnosti = (ub as VIPBasic).Pogodnosti.ToList();

                        s.Update(vi);
                        break;
                    case TipUlaznice.AKREDITACIJA:
                        Akreditacija a = u as Akreditacija;
                        a.OSNOVNA_CENA = ub.OsnovnaCena;
                        a.NACIN_PLACANJA = ub.NacinPlacanja;
                        a.DATUM_KUPOVINE = ub.DatumKupovine;
                        a.TIP = (ub as AkreditacijaBasic).Tip;

                        s.Update(a);
                        break;
                    default:
                        throw new Exception("Nepravilna ulaznica!");
                }

                s.Flush();
                s.Close();

                return true;
            }
            catch (NHibernate.Exceptions.GenericADOException ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message != null)
                {
                    string poruka = ex.InnerException.Message;
                    string kljucGreske = "";
                    string porukaKlijentu = "";

                    if (poruka.Contains("CHK_GRUPA_NAZIV"))
                    {
                        kljucGreske = "NAZIV_FORMAT";
                        porukaKlijentu = "Naziv grupe sadrži nedozvoljene znakove. Dozvoljena su samo slova, brojevi i razmaci.";
                    }
                    else if (poruka.Contains("UQ_GRUPA_NAZIV_AGENCIJA"))
                    {
                        kljucGreske = "DUPLIKAT_NAZIV";
                        porukaKlijentu = "Grupa sa istim nazivom već postoji u okviru ove agencije. Unesite jedinstven naziv.";
                    }
                    else if (poruka.Contains("FK_GRUPA_AGENCIJA_ORGANIZATOR"))
                    {
                        kljucGreske = "AGENCIJA_NE_POSTOJI";
                        porukaKlijentu = "Izabrana agencija ne postoji ili je obrisana. Proverite unos agencije.";
                    }

                    if (!string.IsNullOrEmpty(kljucGreske))
                    {
                        throw new ValidacijaIzuzetka(kljucGreske, porukaKlijentu);
                    }
                    else
                    {
                        throw new Exception("Greška u bazi podataka: Nepoznata validacija. Detalji: " + poruka);
                    }
                }
                else
                {
                    throw new Exception("Došlo je do greške u komunikaciji sa bazom.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public static bool ObrisiUlaznicu(int ulaznicaID)
        {
            try
            {
                ISession s = DataLayer.GetSession();
                Ulaznica u = s.Get<Ulaznica>(ulaznicaID);

                if (u == null)
                {
                    return false;
                }

                s.Delete(u);
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
    }
}