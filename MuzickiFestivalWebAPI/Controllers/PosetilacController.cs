using Microsoft.AspNetCore.Mvc;
using Muzicki_festival;
using Muzicki_festival.DTOs;

namespace MuzickiFestivalWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PosetilacController : ControllerBase
    {
        [HttpPost]
        [Route("DodajPosetioca")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult DodajPosetioca([FromBody] PosetilacBasic pb)
        {
            try
            {
                DTOManager.DodajPosetioca(pb);
                return Ok($"Uspešno ste dodali posetioca: {pb.Ime}.");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }
    }
}
