using Microsoft.AspNetCore.Mvc;
using Muzicki_festival;
using Muzicki_festival.DTOs;

namespace MuzickiFestivalWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GrupaController : ControllerBase
    {
        [HttpGet]
        [Route("PreuzmiGrupe")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult VratiSveGrupe()
        {
            try
            {
                var grupe = DTOManager.VratiSveGrupe();
                if (grupe == null || !grupe.Any())
                    return Ok("Nema dostupnih grupa.");

                return Ok(grupe);
            }
            catch (Exception e)
            {
                return BadRequest($"Greška prilikom preuzimanja grupa: {e.Message}");
            }
        }

        [HttpPost]
        [Route("DodajGrupu")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult DodajGrupu([FromBody] GrupaBasic gb)
        {
            try
            {
                DTOManager.DodajGrupu(gb);
                return Ok($"Uspešno ste dodali grupu: {gb.Naziv}.");
            }
            catch (Exception e)
            {
                return BadRequest($"Neuspešno dodavanje grupe: {e.Message}");
            }
        }

        [HttpPost]
        [Route("DodajClanaGrupi")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult DodajClanaGrupi(int grupaId, int posetilacId)
        {
            try
            {
                DTOManager.DodajClanaGrupi(grupaId, posetilacId);
                return Ok($"Uspešno ste dodali člana sa ID: {posetilacId} grupi sa ID: {grupaId}.");
            }
            catch (Exception e)
            {
                return BadRequest($"Neuspešno dodavanje člana grupi: {e.Message}");
            }
        }

        [HttpPut]
        [Route("IzmeniGrupu")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult IzmeniGrupu([FromBody] GrupaBasic gb)
        {
            try
            {
                bool uspeh = DTOManager.IzmeniGrupu(gb);
                if (uspeh)
                    return Ok($"Uspešno ste izmenili grupu: {gb.Naziv}.");
                else
                    return NotFound($"Grupa sa ID {gb.Id} nije pronađena.");
            }
            catch (Exception e)
            {
                return BadRequest($"Neuspešna izmena grupe: {e.Message}");
            }
        }

        [HttpDelete]
        [Route("ObrisiGrupu")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult ObrisiGrupu(int grupaId)
        {
            try
            {
                bool uspeh = DTOManager.ObrisiGrupu(grupaId);
                if (uspeh)
                    return Ok($"Uspešno ste obrisali grupu sa ID: {grupaId}.");
                else
                    return NotFound($"Grupa sa ID {grupaId} nije pronađena.");
            }
            catch (Exception e)
            {
                return BadRequest($"Neuspešno brisanje grupe: {e.Message}");
            }
        }
    }
}
