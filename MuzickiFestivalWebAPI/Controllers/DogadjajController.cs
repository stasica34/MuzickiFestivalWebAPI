using Microsoft.AspNetCore.Mvc;
using Muzicki_festival;
using Muzicki_festival.DTOs;
using Muzicki_festival.Entiteti;

namespace MuzickiFestivalWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DogadjajController : ControllerBase
    {
        [HttpGet]
        [Route("PreuzmiDogadjaje")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult PreuzmiDogadjaje()
        {
            try
            {
                return new JsonResult(DTOManager.VratiSveDogadjaje());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }
        [HttpGet]
        [Route("PreuzmiDogadjajePoID")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult PreuzmiDogadjajePoID(int dogadjajId)
        {
            try
            {
                return new JsonResult(DTOManager.VratiPosetioceDogadjaja(dogadjajId));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }
        [HttpGet]
        [Route("PreuzmiSveIzvodjaceZaDogadjaj")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult  VratiSveIzvodjaceDogadjaja(int dogadjajId)
        {
            try
            {
                return new JsonResult(DTOManager.VratiSveIzvodjaceDogadjaja(dogadjajId));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }
        [HttpPost]
        [Route("DodajDogadjaje")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult DodajDogadjaje([FromBody]DogadjajBasic d)
        {
            try
            {
                DTOManager.DodajDogadjaj(d);
                return Ok($"Uspešno ste dodali dogadjaj: {d.Naziv}.");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }
        [HttpPost]
        [Route("DodajIzvodjacaNaDogadjaj")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult DodajIzvodjacaNaDogadjaj(int dogadjajId, int izvodjacId)
        {
            try
            {
                DTOManager.DodajIzvodjacaNaDogadjaj(dogadjajId, izvodjacId);
                return Ok($"Uspešno ste dodali izvodajce na dogadjaj: {izvodjacId} {dogadjajId}.");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }

    }
}
