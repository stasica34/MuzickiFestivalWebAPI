using Microsoft.AspNetCore.Mvc;
using Muzicki_festival;
using Muzicki_festival.DTOs;

namespace MuzickiFestivalWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AgencijaOrganizatorController : ControllerBase
    {
        [HttpGet]
        [Route("PreuzmiAgencije")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult VratiSveAgencije()
        {
            try
            {
                var agencije = DTOManager.VratiSveAgencije();

                if (agencije == null || !agencije.Any())
                    return Ok("Nema podataka o agencijama.");

                return Ok(agencije);
            }
            catch (Exception e)
            {
                return BadRequest($"Greška prilikom preuzimanja agencija: {e.Message}");
            }
        }

        [HttpPost]
        [Route("DodajAgenciju")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult DodajAgenciju([FromBody] AgencijaOrganizatorBasic ab)
        {
            try
            {
                DTOManager.DodajAgenciju(ab);
                return Ok($"Uspešno ste dodali agenciju: {ab.Naziv}.");
            }
            catch (Exception e)
            {
                return BadRequest($"Neuspešno dodavanje agencije: {e.Message}");
            }
        }

        [HttpPut]
        [Route("IzmeniAgencijuOrganizator")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult IzmeniAgencijuOrganizator([FromBody] AgencijaOrganizatorBasic ab)
        {
            try
            {
                bool uspeh = DTOManager.IzmeniAgencijuOrganizator(ab);

                if (uspeh)
                    return Ok($"Uspešno ste izmenili agenciju: {ab.Naziv}.");
                else
                    return NotFound($"Agencija sa ID {ab.Id} nije pronađena.");
            }
            catch (Exception e)
            {
                return BadRequest($"Neuspešna izmena agencije: {e.Message}");
            }
        }

        [HttpDelete]
        [Route("ObrisiAgenciju")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult ObrisiAgenciju([FromQuery] int agencijaId)
        {
            try
            {
                bool uspeh = DTOManager.ObrisiAgenciju(agencijaId);

                if (uspeh)
                    return Ok($"Uspešno ste obrisali agenciju sa ID: {agencijaId}.");
                else
                    return NotFound($"Agencija sa ID {agencijaId} nije pronađena.");

            }
            catch (Exception e)
            {
                return BadRequest($"Neuspešno brisanje agencije: {e.Message}");
            }
        }
    }
}
