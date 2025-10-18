using Microsoft.AspNetCore.Mvc;
using Muzicki_festival;
using Muzicki_festival.DTOs;

namespace MuzickiFestivalWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UlaznicaController : ControllerBase
    {
        [HttpGet]
        [Route("PreuzmiUlaznice")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult VratiSveUlaznice()
        {
            try
            {
                return new JsonResult(DTOManager.VratiSveUlaznice());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }
        [HttpPut]
        [Route("IzmeniUlaznicu")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult IzmeniUlaznicu(UlaznicaBasic ub)
        {
            try
            {
                bool uspeh = DTOManager.IzmeniUlaznicu(ub);
                if (uspeh)
                    return Ok($"Uspešno ste izmenili ulaznicu: {ub.TipUlaznice}.");
                else
                    return NotFound($"Ulaznica sa ID {ub.Id} nije pronađena.");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }
        [HttpDelete]
        [Route("ObrisiUlaznicu")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult ObrisiUlaznicu(int ulaznicaID)
        {
            try
            {
                DTOManager.ObrisiUlaznicu(ulaznicaID);
                return Ok($"Uspešno ste obrisali ulaznicu koji ima id: {ulaznicaID}.");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }
    }
}
