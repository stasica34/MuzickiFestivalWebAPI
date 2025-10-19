using Microsoft.AspNetCore.Mvc;
using Muzicki_festival;
using Muzicki_festival.DTOs;

namespace MuzickiFestivalWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MenadzerskaAgencijaController : ControllerBase
    {
        [HttpGet]
        [Route("PreuzmiSveMenadzerske")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult PreuzmiSveMenadzerske()
        {
            try
            {
                var agencije = DTOManager.VratiSveMenadzerskeAgencije();
                if (agencije == null || !agencije.Any())
                    return Ok("Nema dostupnih menadžerskih agencija.");

                return Ok(agencije);
            }
            catch (Exception e)
            {
                return BadRequest($"Greška prilikom preuzimanja menadžerskih agencija: {e.Message}");
            }
        }

        [HttpGet]
        [Route("PreuzmiMenadzerskePoID")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult PreuzmiMenadzerskePoID(int id)
        {
            try
            {
                var agencija = DTOManager.VratiMenadzerskuAgenciju(id);
                if (agencija == null)
                    return NotFound($"Menadžerska agencija sa ID {id} nije pronađena.");

                return Ok(agencija);
            }
            catch (Exception e)
            {
                return BadRequest($"Greška prilikom preuzimanja menadžerske agencije: {e.Message}");
            }
        }

        [HttpGet]
        [Route("PreuzmiMenadzerskePoIzvodjacu")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult PreuzmiMenadzerskePoIzvodjacu(int idIzvodjaca)
        {
            try
            {
                var agencija = DTOManager.VratiMenadzerskuIzvodjaca(idIzvodjaca);
                if (agencija == null)
                    return NotFound($"Menadžerska agencija za izvođača sa ID {idIzvodjaca} nije pronađena.");

                return Ok(agencija);
            }
            catch (Exception e)
            {
                return BadRequest($"Greška prilikom preuzimanja menadžerske agencije: {e.Message}");
            }
        }

        [HttpGet]
        [Route("PreuzmiIzvodjaceMenadzerskeAgencije")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult VratiIzvodjaceMenadzerskeAgencije(int idMenadzerskeAgencije)
        {
            try
            {
                var izvodjaci = DTOManager.VratiIzvodjaceMenadzerskeAgencije(idMenadzerskeAgencije);
                if (izvodjaci == null || !izvodjaci.Any())
                    return Ok("Nema izvođača za ovu menadžersku agenciju.");

                return Ok(izvodjaci);
            }
            catch (Exception e)
            {
                return BadRequest($"Greška prilikom preuzimanja izvođača: {e.Message}");
            }
        }

        [HttpGet]
        [Route("PreuzmiKonktatPodatke")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult VratiSveKontaktPodatke(int idMenadzerskeAgencije)
        {
            try
            {
                var kontakti = DTOManager.VratiSveKontaktPodatke(idMenadzerskeAgencije);
                if (kontakti == null || !kontakti.Any())
                    return Ok("Nema kontakt podataka za ovu menadžersku agenciju.");

                return Ok(kontakti);
            }
            catch (Exception e)
            {
                return BadRequest($"Greška prilikom preuzimanja kontakt podataka: {e.Message}");
            }
        }

        [HttpPost]
        [Route("DodajMenadzerskuAgenciju")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult DodajMenadzerskuAgenciju([FromBody] MenadzerskaAgencijaBasic ma)
        {
            try
            {
                DTOManager.DodajMenadzerskuAgenciju(ma);
                return Ok($"Uspešno ste dodali menadžersku agenciju: {ma.Naziv}.");
            }
            catch (Exception e)
            {
                return BadRequest($"Greška prilikom dodavanja menadžerske agencije: {e.Message}");
            }
        }

        [HttpPost]
        [Route("DodajKontaktMenadzerskeAgencije")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult DodajKontaktMenadzerskeAgencije([FromBody] MenadzerskaAgencijaKontaktBasic kontakt)
        {
            try
            {
                DTOManager.DodajKontaktMenadzerskeAgencije(kontakt);
                return Ok($"Uspešno ste dodali kontakt menadžerskoj agenciji: {kontakt.MenadzerkaAgencija?.Naziv}.");
            }
            catch (Exception e)
            {
                return BadRequest($"Greška prilikom dodavanja kontakta: {e.Message}");
            }
        }

        [HttpPut]
        [Route("IzmeniMenadzerskuAgenciju")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult IzmeniMenadzerskuAgenciju([FromBody] MenadzerskaAgencijaBasic mb)
        {
            try
            {
                bool uspeh = DTOManager.IzmeniMenadzerskuAgenciju(mb);
                if (uspeh)
                    return Ok($"Uspešno ste izmenili menadžersku agenciju: {mb.Naziv}.");
                else
                    return NotFound($"Menadžerska agencija sa ID {mb.ID} nije pronađena.");
            }
            catch (Exception e)
            {
                return BadRequest($"Greška prilikom izmene menadžerske agencije: {e.Message}");
            }
        }

        [HttpDelete]
        [Route("ObrisiMenadzerskuAgenciju")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult ObrisiMenadzerskuAgenciju(int id)
        {
            try
            {
                bool uspeh = DTOManager.ObrisiMenadzerskuAgenciju(id);
                if (uspeh)
                    return Ok($"Uspešno ste obrisali menadžersku agenciju sa ID: {id}.");
                else
                    return NotFound($"Menadžerska agencija sa ID {id} nije pronađena.");
            }
            catch (Exception e)
            {
                return BadRequest($"Greška prilikom brisanja menadžerske agencije: {e.Message}");
            }
        }

        [HttpDelete]
        [Route("ObrisiKontaktMenadzerskeAgencije")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult ObrisiKontaktMenadzerskeAgencije([FromBody] MenadzerskaAgencijaKontaktBasic k)
        {
            try
            {
                bool uspesno = DTOManager.ObrisiKontaktMenadzerskeAgencije(k);
                if (!uspesno)
                {
                    return BadRequest("Brisanje kontakta nije uspelo. Proverite da li kontakt i agencija postoje.");
                }
                return Ok($"Uspešno ste obrisali kontakt (ID: {k.ID}) menadžerske agencije (ID: {k.MenadzerkaAgencija?.ID}).");
            }
            catch (Exception e)
            {
                return BadRequest($"Greška prilikom brisanja kontakta: {e.Message}");
            }
        }
    }
}
