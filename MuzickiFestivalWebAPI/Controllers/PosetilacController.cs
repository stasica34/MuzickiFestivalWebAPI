using Microsoft.AspNetCore.Mvc;
using Muzicki_festival;
using Muzicki_festival.DTOs;

namespace MuzickiFestivalWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PosetilacController : ControllerBase
    {
        [HttpGet]
        [Route("PreuzmiPosetioce")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult PPreuzmiPosetiocereu()
        {
            try
            {
                return new JsonResult(DTOManager.VratiSvePosetioce());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }
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
        [HttpGet]
        [Route("VratiUlaznicuPosetioca")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult VratiUlaznicuPosetioca(int idPosetioca)
        {
            try
            {
                return new JsonResult(DTOManager.VratiUlaznicuPosetioca(idPosetioca));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }
        [HttpGet]
        [Route("VratiGrupuPosetioca")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult VratiGrupuPosetioca(int idPosetioca)
        {
            try
            {
                return new JsonResult(DTOManager.VratiGrupuPosetioca(idPosetioca));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }
        [HttpPut]
        [Route("IzmeniPosetioca")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult IzmeniPosetioca(PosetilacBasic pb)
        {
            try
            { 
                PosetilacView uspeh = DTOManager.IzmeniPosetioca(pb);
                if (uspeh!=null)
                    return Ok($"Uspešno ste izmenili posetioca: {pb.Ime}.");
                else
                    return NotFound($"Posetioca sa ID {pb.Id} nije pronađena.");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }
        [HttpDelete]
        [Route("ObrisiPosetioca")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult ObrisiPosetioca(int idPosetioca)
        {
            try
            {
                DTOManager.ObrisiPosetioca(idPosetioca);
                return Ok($"Uspešno ste obrisali posetioca koji ima id: {idPosetioca}.");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }
        [HttpDelete]
        [Route("IzbaciIzGrupe")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult IzbaciIzGrupe(int idPosetioca, int idGrupe)
        {
            try
            {
                DTOManager.IzbaciIzGrupe(idPosetioca, idGrupe);
                return Ok($"Uspešno ste obrisali posetioca (ID: {idPosetioca}) grupe (ID: {idGrupe}).");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }
    }
}
