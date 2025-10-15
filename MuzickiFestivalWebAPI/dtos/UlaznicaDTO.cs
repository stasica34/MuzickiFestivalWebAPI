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
        public int Id;
        public float OsnovnaCena;
        public string NacinPlacanja;
        public DateTime DatumKupovine;
        public TipUlaznice TipUlaznice;
        public DogadjajBasic Dogadjaj;

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
        public DateTime DatumVazenja;

        public JednodnevnaBasic(int id, float osnovnaCena, string nacinPlacanja, DateTime datumKupovine, DogadjajBasic dogadjaj, DateTime datumVazenja)
            : base(id, osnovnaCena, nacinPlacanja, datumKupovine, TipUlaznice.JEDNODNEVNA, dogadjaj)
        {
            DatumVazenja = datumVazenja;
        }
    }

    public class ViseDnevnaBasic : UlaznicaBasic
    {
        public List<DateTime> DatumiVazenja;

        public ViseDnevnaBasic(int id, float osnovnaCena, string nacinPlacanja, DateTime datumKupovine, DogadjajBasic dogadjaj, List<DateTime> datumiVazenja)
            : base(id, osnovnaCena, nacinPlacanja, datumKupovine, TipUlaznice.JEDNODNEVNA, dogadjaj)
        {
            DatumiVazenja = datumiVazenja;
        }
    }

    public class VIPBasic : UlaznicaBasic
    {
        public List<string> Pogodnosti;

        public VIPBasic(int id, float osnovnaCena, string nacinPlacanja, DateTime datumKupovine, DogadjajBasic dogadjaj, List<string> pogodnosti)
            : base(id, osnovnaCena, nacinPlacanja, datumKupovine, TipUlaznice.JEDNODNEVNA, dogadjaj)
        {
            Pogodnosti = pogodnosti;
        }
    }

    public class AkreditacijaBasic : UlaznicaBasic
    {
        public TipAkreditacije Tip;

        public AkreditacijaBasic(int id, float osnovnaCena, string nacinPlacanja, DateTime datumKupovine, DogadjajBasic dogadjaj, TipAkreditacije tip)
             :base(id, osnovnaCena, nacinPlacanja, datumKupovine, TipUlaznice.JEDNODNEVNA, dogadjaj)
        {
            Tip = tip;
        }
    }

}
