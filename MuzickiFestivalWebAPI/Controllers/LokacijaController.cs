using Microsoft.AspNetCore.Mvc;
using Muzicki_festival;
using Muzicki_festival.DTOs;

namespace MuzickiFestivalWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LokacijaController : ControllerBase
    {
        [HttpGet]
        [Route("PreuzmiLokacije")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult PreuzmiLokacije()
        {
            try
            {
                var lokacije = DTOManager.VratiSveLokacije();
                if (lokacije == null || !lokacije.Any())
                    return Ok("Nema dostupnih lokacija.");

                return Ok(lokacije);
            }
            catch (Exception e)
            {
                return BadRequest($"Greška prilikom preuzimanja lokacija: {e.Message}");
            }
        }

        [HttpGet]
        [Route("PreuzmiLokacijePoId")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult PreuzmiLokacijePoId(int id)
        {
            try
            {
                var lokacija = DTOManager.VratiLokaciju(id);
                if (lokacija == null)
                    return NotFound($"Lokacija sa ID {id} nije pronađena.");

                return Ok(lokacija);
            }
            catch (Exception e)
            {
                return BadRequest($"Greška prilikom preuzimanja lokacije: {e.Message}");
            }
        }
        [HttpPost]
        [Route("DodajLokaciju")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult DodajLokaciju([FromBody] LokacijaBasic l)
        {
            try
            {
                DTOManager.DodajLokaciju(l);
                return Ok($"Uspešno ste dodali lokaciju: {l.Naziv}.");
            }
            catch (Exception e)
            {
                return BadRequest($"Neuspešno dodavanje lokacije: {e.Message}");
            }
        }

        [HttpPut]
        [Route("IzmeniLokaciju")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult IzmeniLokaciju([FromBody] LokacijaBasic nova)
        {
            try
            {
                bool uspeh = DTOManager.IzmeniLokaciju(nova);
                if (uspeh)
                    return Ok($"Uspešno ste izmenili lokaciju: {nova.Naziv}.");
                else
                    return NotFound($"Lokacija sa ID {nova.Id} nije pronađena.");
            }
            catch (Exception e)
            {
                return BadRequest($"Greška prilikom izmene lokacije: {e.Message}");
            }
        }

        [HttpDelete]
        [Route("ObrisiLokaciju")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult ObrisiLokaciju(int idLokacije)
        {
            try
            {
                bool uspeh = DTOManager.ObrisiLokaciju(idLokacije);
                if (uspeh)
                    return Ok($"Uspešno ste obrisali lokaciju sa ID: {idLokacije}.");
                else
                    return NotFound($"Lokacija sa ID {idLokacije} nije pronađena.");
            }
            catch (Exception e)
            {
                return BadRequest($"Greška prilikom brisanja lokacije: {e.Message}");
            }
        }
    }
}
