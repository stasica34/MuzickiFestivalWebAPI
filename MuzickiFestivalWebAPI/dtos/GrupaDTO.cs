using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Muzicki_festival.DTOs
{
    public class GrupaView
    {
        public int Id { get; set; }
        public string Naziv { get; set; }
        public string NazivAgencije { get; set; }
        public List<string>? Clanovi { get; set; }

        public GrupaView(int id, string naziv, string nazivAgencije, List<string> clanovi)
        {
            Id = id;
            Naziv = naziv;
            NazivAgencije = nazivAgencije;
            Clanovi = clanovi;
        }
    }

    public class GrupaBasic
    {
        public int Id { get; set; }
        public string Naziv { get; set; }
        public AgencijaOrganizatorBasic? Agencija { get; set; }
        public List<PosetilacBasic>? Clanovi { get; set; }
        public GrupaBasic() { }
        public GrupaBasic(int id, string naziv, AgencijaOrganizatorBasic agencija, List<PosetilacBasic> clanovi)
        {
            Id = id;
            Naziv = naziv;
            Agencija = agencija;
            Clanovi = clanovi;
        }
    }
}
