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
                var ulaznice = DTOManager.VratiSveUlaznice();
                if (ulaznice == null || !ulaznice.Any())
                    return Ok("Nema dostupnih ulaznica.");

                return Ok(ulaznice);
            }
            catch (Exception e)
            {
                return BadRequest($"Greška prilikom preuzimanja ulaznica: {e.Message}");
            }
        }

        [HttpPut]
        [Route("IzmeniUlaznicu")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult IzmeniUlaznicu([FromBody] UlaznicaBasic ub)
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
                return BadRequest($"Greška prilikom izmene ulaznice: {e.Message}");
            }
        }

        [HttpDelete]
        [Route("ObrisiUlaznicu")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult ObrisiUlaznicu(int ulaznicaID)
        {
            try
            {
                bool uspesno = DTOManager.ObrisiUlaznicu(ulaznicaID);
                if (uspesno)
                    return Ok($"Uspešno ste obrisali ulaznicu koja ima ID: {ulaznicaID}.");
                else
                    return NotFound($"Ulaznica sa ID {ulaznicaID} nije pronađena.");
            }
            catch (Exception e)
            {
                return BadRequest($"Greška prilikom brisanja ulaznice: {e.Message}");
            }
        }
    }
}
