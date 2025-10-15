using Muzicki_festival.Entiteti;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Muzicki_festival.DTOs
{
    public class DostupnaOpremaView
    {
        public int Id { get; set; }
        public string Naziv { get; set; }
        public DostupnaOpremaView(int id, string naziv)
        {
            Id = id;
            Naziv = naziv;
        }
    }

    public class DostupnaOpremaBasic
    {
        public int Id { get; set; }
        public string Naziv { get; set; }
        
        public LokacijaBasic Lokacija { get; set; }
        public DostupnaOpremaBasic(int id, string naziv, LokacijaBasic lokacija)
        {
            Id = id;
            Naziv = naziv;
            Lokacija = lokacija;
        }
    }
}
