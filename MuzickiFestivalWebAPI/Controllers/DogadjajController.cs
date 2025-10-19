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
                var dogadjaji = DTOManager.VratiSveDogadjaje();

                if (dogadjaji == null || !dogadjaji.Any())
                    return Ok("Nema podataka o događajima.");

                return Ok(dogadjaji);
            }
            catch (Exception e)
            {
                return BadRequest($"Greška prilikom preuzimanja događaja: {e.Message}");
            }
        }

        [HttpGet]
        [Route("VratiPosetioceDogadjaja")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult VratiPosetioceDogadjaja(int dogadjajId)
        {
            try
            {
                var posetioci = DTOManager.VratiPosetioceDogadjaja(dogadjajId);

                if (posetioci == null || !posetioci.Any())
                    return Ok($"Nema posetilaca za događaj sa ID: {dogadjajId}.");

                return Ok(posetioci);
            }
            catch (Exception e)
            {
                return BadRequest($"Greška prilikom preuzimanja posetilaca: {e.Message}");
            }
        }

        [HttpGet]
        [Route("PreuzmiSveIzvodjaceZaDogadjaj")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult VratiSveIzvodjaceDogadjaja(int dogadjajId)
        {
            try
            {
                var izvodjaci = DTOManager.VratiSveIzvodjaceDogadjaja(dogadjajId);

                if (izvodjaci == null || !izvodjaci.Any())
                    return Ok($"Nema izvođača za događaj sa ID: {dogadjajId}.");

                return Ok(izvodjaci);
            }
            catch (Exception e)
            {
                return BadRequest($"Greška prilikom preuzimanja izvođača: {e.Message}");
            }
        }

        [HttpPost]
        [Route("DodajDogadjaje")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult DodajDogadjaje([FromBody] DogadjajBasic d)
        {
            try
            {
                DTOManager.DodajDogadjaj(d);
                return Ok($"Uspešno ste dodali događaj: {d.Naziv}.");
            }
            catch (Exception e)
            {
                return BadRequest($"Neuspešno dodavanje događaja: {e.Message}");
            }
        }

        [HttpPost]
        [Route("DodajIzvodjacaNaDogadjaj")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult DodajIzvodjacaNaDogadjaj(int dogadjajId, int izvodjacId)
        {
            try
            {
                bool uspeh = DTOManager.DodajIzvodjacaNaDogadjaj(dogadjajId, izvodjacId);

                if (uspeh)
                    return Ok($"Uspešno ste dodali izvođača sa ID: {izvodjacId} na događaj sa ID: {dogadjajId}.");
                else
                    return NotFound($"Događaj ili izvođač nije pronađen za dati ID.");

            }
            catch (Exception e)
            {
                return BadRequest($"Neuspešno dodavanje izvođača na događaj: {e.Message}");
            }
        }
    }
}
