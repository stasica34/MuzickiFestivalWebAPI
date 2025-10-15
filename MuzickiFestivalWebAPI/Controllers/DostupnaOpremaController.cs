using Microsoft.AspNetCore.Mvc;
using Muzicki_festival;
using Muzicki_festival.DTOs;

namespace MuzickiFestivalWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DostupnaOpremaController : ControllerBase
    {
        [HttpGet]
        [Route("PreuzmiDostupnuOpremu")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult PreuzmiDostupnuOpremu(int idLokacije)
        {
            try
            {
                return new JsonResult(DTOManager.VratiSvuDostupnuOpremu(idLokacije));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }
        [HttpPost]
        [Route("DodajDostupnuOpremu")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult DodajDostupnuOpremu([FromBody] DostupnaOpremaBasic d)
        {
            try
            {
                DTOManager.DodajDostupnuOpremu(d);
                return Ok(d);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }
        [HttpDelete]
        [Route("ObrisiDostupnuOpremu")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult ObrisiDostupnuOpremu(DostupnaOpremaBasic o)
        {
            try
            {
                DTOManager.ObrisiDostupnuOpremu(o);
                return Ok($"Uspešno ste obrisali dostupnu opremu koji ima id: {o.Id}.");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }

    }
}
