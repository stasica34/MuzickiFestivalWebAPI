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
                return new JsonResult(DTOManager.VratiSveAgencije());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
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
                return BadRequest(e.Message.ToString());
            }
        }
        [HttpPut]
        [Route("IzmeniAgencijuOrganizator")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult IzmeniAgencijuOrganizator(AgencijaOrganizatorBasic ab)
        {
            try
            {
                bool uspeh = DTOManager.IzmeniAgencijuOrganizator(ab);
                if (uspeh)
                    return Ok($"Uspešno ste izmenili agenciju: {ab.Naziv}.");
                else
                    return NotFound($"Menadžerska agencija sa ID {ab.Id} nije pronađena.");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }
        [HttpDelete]
        [Route("ObrisiAgenciju")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult ObrisiAgenciju(int agencijaId)
        {
            try
            {
                DTOManager.ObrisiAgenciju(agencijaId);
                return Ok($"Uspešno ste obrisali agenciju koji ima id: {agencijaId}.");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }
    }
}
