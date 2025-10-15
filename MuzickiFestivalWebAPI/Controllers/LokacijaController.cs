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
                return new JsonResult(DTOManager.VratiSveLokacije());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }
        [HttpGet]
        [Route("PreuzmiLokacijePoId")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult PreuzmiLokacijePoId(int id)
        {
            try
            {
                return new JsonResult(DTOManager.VratiLokaciju(id));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }
        [HttpPost]
        [Route("DodajLokacije")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult DodajLokaciju([FromBody] LokacijaBasic l)
        {
            try
            {
                DTOManager.DodajLokaciju(l);
                return Ok(l);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }
        [HttpPut]
        [Route("IzmeniLokaciju")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult IzmeniLokaciju(LokacijaBasic nova)
        {
            try
            {
                DTOManager.IzmeniLokaciju(nova);
                return Ok($"Uspešno ste izmenili lokaciju: {nova.Naziv}.");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }
    }
}
