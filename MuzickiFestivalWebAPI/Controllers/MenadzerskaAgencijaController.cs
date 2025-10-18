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
                return new JsonResult(DTOManager.VratiSveMenadzerskeAgencije());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }
        [HttpGet]
        [Route("PreuzmiMenadzerskePoID")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult PreuzmiMenadzerskePoID(int id)
        {
            try
            {
                return new JsonResult(DTOManager.VratiMenadzerskuAgenciju(id));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }
        [HttpGet]
        [Route("PreuzmiMenadzerskePoIzvodjacu")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult PreuzmiMenadzerskePoIzvodjacu(int idIzvodjaca)
        {
            try
            {
                return new JsonResult(DTOManager.VratiMenadzerskuIzvodjaca(idIzvodjaca));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }
        [HttpGet]
        [Route("PreuzmiIzvodjaceMenadzerskeAgencije")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult VratiIzvodjaceMenadzerskeAgencije(int idMenadzerskeAgencije)
        {
            try
            {
                return new JsonResult(DTOManager.VratiIzvodjaceMenadzerskeAgencije(idMenadzerskeAgencije));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }
        [HttpGet]
        [Route("PreuzmiKonktatPodatke")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult VratiSveKontaktPodatke(int idMenadzerskeAgencije)
        {
            try
            {
                return new JsonResult(DTOManager.VratiSveKontaktPodatke(idMenadzerskeAgencije));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }
        //ovde ne mogu da se dodaju kontakt podaci, ne dodaje se u bazi
        [HttpPost]
        [Route("DodajMenadzerskuAgenciju")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult DodajMenadzerskuAgenciju([FromBody] MenadzerskaAgencijaBasic ma)
        {
            try
            {
                DTOManager.DodajMenadzerskuAgenciju(ma);
                return Ok($"Uspešno ste dodali menadžersku agenciji: {ma.Naziv}.");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }
        //ovde se dodaju u bazi
        [HttpPost]
        [Route("DodajKontaktMenadzerskeAgencije")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult DodajKontaktMenadzerskeAgencije([FromBody] MenadzerskaAgencijaKontaktBasic kontakt)
        {
            try
            {
                DTOManager.DodajKontaktMenadzerskeAgencije(kontakt);
                return Ok($"Uspešno ste dodali kontkat menadžerskoj agenciji: {kontakt.MenadzerkaAgencija.Naziv}.");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }
        [HttpPut]
        [Route("IzmeniMenadzerskuAgenciju")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult IzmeniMenadzerskuAgenciju(MenadzerskaAgencijaBasic mb)
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
                return BadRequest(e.Message.ToString());
            }
        }
        [HttpDelete]
        [Route("ObrisiMenadzerskuAgenciju")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult ObrisiMenadzerskuAgenciju(int id)
        {
            try
            {
                DTOManager.ObrisiMenadzerskuAgenciju(id);
                return Ok($"Uspešno ste obrisali menadžersku agenciju koja ima id: {id}.");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }
        [HttpDelete]
        [Route("ObrisiKontaktMenadzerskeAgencije")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult ObrisiKontaktMenadzerskeAgencije([FromBody]MenadzerskaAgencijaKontaktBasic k)
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
                return BadRequest(e.Message);
            }
        }
    }
}
