using Muzicki_festival.Entiteti;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Muzicki_festival.DTOs
{
    public abstract class UlaznicaBasic
    {
        public int Id { get; set; }
        public float OsnovnaCena { get; set; }
        public string NacinPlacanja { get; set; }
        public DateTime DatumKupovine { get; set; }
        public TipUlaznice TipUlaznice { get; set; }
        public DogadjajBasic Dogadjaj { get; set; }
        public DateTime? DatumVazenja { get; set; }
        public IList<DateTime>? DatumiVazenja { get; set; }
        public IList<string>? Pogodnosti { get; set; }
        public TipAkreditacije Tip { get; set; }


        public UlaznicaBasic() { }

        public UlaznicaBasic(int id, float osnovnaCena, string nacinPlacanja, DateTime datumKupovine, TipUlaznice tipUlaznice, DogadjajBasic dogadjaj)
        {
            Id = id;
            OsnovnaCena = osnovnaCena;
            NacinPlacanja = nacinPlacanja;
            DatumKupovine = datumKupovine;
            TipUlaznice = tipUlaznice;
            Dogadjaj = dogadjaj;
        }
    }

    public class JednodnevnaBasic : UlaznicaBasic
    {
        public JednodnevnaBasic() { }

        public JednodnevnaBasic(int id, float osnovnaCena, string nacinPlacanja, DateTime datumKupovine, DogadjajBasic dogadjaj, DateTime datumVazenja)
            : base(id, osnovnaCena, nacinPlacanja, datumKupovine, TipUlaznice.JEDNODNEVNA, dogadjaj)
        {
            DatumVazenja = datumVazenja;
        }
    }

    public class ViseDnevnaBasic : UlaznicaBasic
    {
        public ViseDnevnaBasic() { }
        public ViseDnevnaBasic(int id, float osnovnaCena, string nacinPlacanja, DateTime datumKupovine, DogadjajBasic dogadjaj, IList<DateTime> datumiVazenja)
            : base(id, osnovnaCena, nacinPlacanja, datumKupovine, TipUlaznice.VISEDNEVNA, dogadjaj)
        {
            DatumiVazenja = datumiVazenja;
        }
    }

    public class VIPBasic : UlaznicaBasic
    {
        public VIPBasic() { } 
        public VIPBasic(int id, float osnovnaCena, string nacinPlacanja, DateTime datumKupovine, DogadjajBasic dogadjaj, IList<string> pogodnosti)
            : base(id, osnovnaCena, nacinPlacanja, datumKupovine, TipUlaznice.VIP, dogadjaj)
        {
            Pogodnosti = pogodnosti;
        }
    }

    public class AkreditacijaBasic : UlaznicaBasic
    {
        public AkreditacijaBasic() { }
        public AkreditacijaBasic(int id, float osnovnaCena, string nacinPlacanja, DateTime datumKupovine, DogadjajBasic dogadjaj, TipAkreditacije tip)
             :base(id, osnovnaCena, nacinPlacanja, datumKupovine, TipUlaznice.AKREDITACIJA, dogadjaj)
        {
            Tip = tip;
        }
    }

}
