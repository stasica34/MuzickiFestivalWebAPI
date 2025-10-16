using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Muzicki_festival.DTOs
{
    public class AgencijaOrganizatorView
    {
        public int Id { get; set; }
        public string Naziv { get; set; }
        public string Adresa { get; set; }
        public AgencijaOrganizatorView(int id, string naziv, string adresa)
        {
            Id = id;
            Naziv = naziv;
            Adresa = adresa;
        }

        public override string ToString()
        {
            return Naziv;
        }
    }

    public class AgencijaOrganizatorBasic
    {
        public int Id { get; set; }
        public string Naziv { get; set; }
        public string Adresa { get; set; }
        public List<PosetilacBasic>? Posetioci { get; set; }
        public AgencijaOrganizatorBasic() { }
        public AgencijaOrganizatorBasic(int id, string naziv, string adresa, List<PosetilacBasic> posetioci)
        {
            Id = id;
            Naziv = naziv;
            Adresa = adresa;
            Posetioci = posetioci;
        }
    }
}
