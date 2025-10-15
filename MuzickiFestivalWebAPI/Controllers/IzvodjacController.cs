using Microsoft.AspNetCore.Mvc;
using Muzicki_festival;
using Muzicki_festival.DTOs;

namespace MuzickiFestivalWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IzvodjacController : ControllerBase
    {
        [HttpGet]
        [Route("PreuzmiIzvodjace")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult PreuzmiIzvodjace()
        {
            try
            {
                return new JsonResult(DTOManager.VratiSveIzvodjace());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }
        [HttpGet]
        [Route("PreuzmiIzvodjacaPoId")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult PreuzmiIzvodjacaPoId(int id)
        {
            try
            {
                return new JsonResult(DTOManager.VratiIzvodjaca(id));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }
        [HttpGet]
        [Route("PreuzmiClanoveBenda")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult PreuzmiClanoveBenda(int bendId)
        {
            try
            {
                return new JsonResult(DTOManager.VratiClanoveBenda(bendId));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }
        [HttpGet]
        [Route("PreuzmiTehnickeZahteve")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult PreuzmiTehnickeZahteve(int Id)
        {
            try
            {
                return new JsonResult(DTOManager.VratiTehnickeZahteve(Id));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }
        [HttpGet]
        [Route("PreuzmiVokalneSposobnosti")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult PreuzmiVokalneSposobnosti(int Id)
        {
            try
            {
                return new JsonResult(DTOManager.VratiVokalneSposobnosti(Id));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }
        [HttpPost("DodajSoloUmetnika")]
        public IActionResult DodajSoloUmetnika([FromBody] Solo_UmetnikBasic umetnik)
        {
            try
            {
                DTOManager.DodajIzvodjaca(umetnik);
                return Ok($"Uspešno ste dodali soloumetnika: {umetnik.Ime}.");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }

        [HttpPost("DodajBend")]
        public IActionResult DodajBend([FromBody] BendBasic bend)
        {
            try
            {
                DTOManager.DodajIzvodjaca(bend);
                return Ok($"Uspešno ste dodali bend: {bend.Ime}.");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }
        [HttpPost("DodajClanoveBenda")]
        public IActionResult DodajClanaBendu([FromBody] ClanBendaBasic bend)
        {
            try
            {
                DTOManager.DodajClanaBendu(bend);
                return Ok($"Uspešno ste dodali clana bendu: {bend.Ime}.");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }
        [HttpPost("DodajTehnickiZahtev")]
        public IActionResult DodajTehnickiZahtev(int Id, string zahtev)
        {
            try
            {
                DTOManager.DodajTehnickiZahtev(Id, zahtev);
                return Ok($"Uspešno ste dodali tehnicki zahtev: {zahtev} izvodjacu sa id: {Id}");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }
        [HttpPost("DodajVokalnuSposobnost")]
        public IActionResult DodajVokalnuSposobnost(int Id, string sposobnost)
        {
            try
            {
                DTOManager.DodajVokalnuSposobnost(Id, sposobnost);
                return Ok($"Uspešno ste dodali vokalnu sposobnost: {sposobnost} izvodjacu sa id: {Id}");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }
        [HttpPut]
        [Route("IzmeniIzvodjaca")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult IzmeniIzvodjaca(IzvodjacBasic i)
        {
            try
            {
                DTOManager.IzmeniIzvodjaca(i);
                return Ok($"Uspešno ste izmenili izvodajca: {i.Ime}");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }
        [HttpDelete]
        [Route("ObrisiIzvodjaca")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult ObrisiIzvodjaca(int id)
        {
            try
            {
                DTOManager.ObrisiIzvodjaca(id);
                return Ok($"Uspešno ste obrisali izvodjaca koji ima id: {id}.");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }
        [HttpDelete]
        [Route("ObrisiClana")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult ObrisiClana(ClanBendaBasic cb)
        {
            try
            {
                DTOManager.ObrisiClana(cb);
                return Ok($"Uspešno ste obrisali clana benda koji ima id: {cb.Id}.");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }
        [HttpDelete]
        [Route("ObrisiTehnickiZahtev")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult ObrisiTehnickiZahtev(int Id, string zahtev)
        {
            try
            {
                DTOManager.ObrisiTehnickiZahtev(Id, zahtev);
                return Ok($"Uspešno ste obrisali tehnicki zahtev: {zahtev} izvodjacu sa id: {Id}");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }
        [HttpDelete]
        [Route("ObrisiVokalnuSposobnost")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult ObrisiVokalnuSposobnost(int Id, string sposobnost)
        {
            try
            {
                DTOManager.ObrisiTehnickiZahtev(Id, sposobnost);
                return Ok($"Uspešno ste obrisali vokalnu sposobnost: {sposobnost} izvodjacu sa id: {Id}");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }

    }
}
