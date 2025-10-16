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
                return new JsonResult(DTOManager.VratiSveGrupe());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
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
                return BadRequest(e.Message.ToString());
            }
        }
        [HttpPost]
        [Route("DodajClanaGrupi")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult DodajClanaGrupi(int grupaId, int posetilacid)
        {
            try
            {
                DTOManager.DodajClanaGrupi(grupaId, posetilacid);
                return Ok($"Uspešno ste dodali grupu: {grupaId} i njegovog clana {posetilacid}.");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }
        [HttpPut]
        [Route("IzmeniGrupu")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult IzmeniGrupu(GrupaBasic gb)
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
                return BadRequest(e.Message.ToString());
            }
        }
        [HttpDelete]
        [Route("ObrisiGrupu")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult ObrisiGrupu(int grupaId)
        {
            try
            {
                DTOManager.ObrisiGrupu(grupaId);
                return Ok($"Uspešno ste obrisali grupu koji ima id: {grupaId}.");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }
    }
}
