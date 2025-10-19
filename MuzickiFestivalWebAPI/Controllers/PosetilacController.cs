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
        public IActionResult PreuzmiPosetioce()
        {
            try
            {
                var posetioci = DTOManager.VratiSvePosetioce();
                if (posetioci == null || !posetioci.Any())
                    return Ok("Nema dostupnih posetilaca.");

                return Ok(posetioci);
            }
            catch (Exception e)
            {
                return BadRequest($"Greška prilikom preuzimanja posetilaca: {e.Message}");
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
                return BadRequest($"Greška prilikom dodavanja posetioca: {e.Message}");
            }
        }

        [HttpGet]
        [Route("VratiUlaznicuPosetioca")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult VratiUlaznicuPosetioca(int idPosetioca)
        {
            try
            {
                var ulaznica = DTOManager.VratiUlaznicuPosetioca(idPosetioca);
                if (ulaznica == null)
                    return NotFound($"Ulaznica za posetioca sa ID {idPosetioca} nije pronađena.");

                return Ok(ulaznica);
            }
            catch (Exception e)
            {
                return BadRequest($"Greška prilikom preuzimanja ulaznice: {e.Message}");
            }
        }

        [HttpGet]
        [Route("VratiGrupuPosetioca")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult VratiGrupuPosetioca(int idPosetioca)
        {
            try
            {
                var grupa = DTOManager.VratiGrupuPosetioca(idPosetioca);
                if (grupa == null)
                    return NotFound($"Grupa za posetioca sa ID {idPosetioca} nije pronađena.");

                return Ok(grupa);
            }
            catch (Exception e)
            {
                return BadRequest($"Greška prilikom preuzimanja grupe: {e.Message}");
            }
        }

        [HttpPut]
        [Route("IzmeniPosetioca")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult IzmeniPosetioca([FromBody] PosetilacBasic pb)
        {
            try
            {
                PosetilacView uspeh = DTOManager.IzmeniPosetioca(pb);
                if (uspeh != null)
                    return Ok($"Uspešno ste izmenili posetioca: {pb.Ime}.");
                else
                    return NotFound($"Posetilac sa ID {pb.Id} nije pronađen.");
            }
            catch (Exception e)
            {
                return BadRequest($"Greška prilikom izmene posetioca: {e.Message}");
            }
        }

        [HttpDelete]
        [Route("ObrisiPosetioca")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult ObrisiPosetioca(int idPosetioca)
        {
            try
            {
                bool uspesno = DTOManager.ObrisiPosetioca(idPosetioca);
                if (uspesno)
                    return Ok($"Uspešno ste obrisali posetioca koji ima ID: {idPosetioca}.");
                else
                    return NotFound($"Posetilac sa ID {idPosetioca} nije pronađen.");
            }
            catch (Exception e)
            {
                return BadRequest($"Greška prilikom brisanja posetioca: {e.Message}");
            }
        }

        [HttpDelete]
        [Route("IzbaciIzGrupe")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult IzbaciIzGrupe(int idPosetioca, int idGrupe)
        {
            try
            {
                bool uspesno = DTOManager.IzbaciIzGrupe(idPosetioca, idGrupe);
                if (uspesno)
                    return Ok($"Uspešno ste izbacili posetioca (ID: {idPosetioca}) iz grupe (ID: {idGrupe}).");
                else
                    return NotFound($"Posetilac ili grupa nisu pronađeni.");
            }
            catch (Exception e)
            {
                return BadRequest($"Greška prilikom izbacivanja iz grupe: {e.Message}");
            }
        }
    }
}
