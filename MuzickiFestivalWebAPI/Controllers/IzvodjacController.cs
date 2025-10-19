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
                var izvodjaci = DTOManager.VratiSveIzvodjace();
                if (izvodjaci == null || !izvodjaci.Any())
                    return Ok("Nema dostupnih izvođača.");

                return Ok(izvodjaci);
            }
            catch (Exception e)
            {
                return BadRequest($"Greška prilikom preuzimanja izvođača: {e.Message}");
            }
        }

        [HttpGet]
        [Route("PreuzmiIzvodjacaPoId")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult PreuzmiIzvodjacaPoId(int id)
        {
            try
            {
                var izvodjac = DTOManager.VratiIzvodjaca(id);
                if (izvodjac == null)
                    return NotFound($"Izvođač sa ID {id} nije pronađen.");

                return Ok(izvodjac);
            }
            catch (Exception e)
            {
                return BadRequest($"Greška prilikom preuzimanja izvođača: {e.Message}");
            }
        }

        [HttpGet]
        [Route("PreuzmiClanoveBenda")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult PreuzmiClanoveBenda(int bendId)
        {
            try
            {
                var clanovi = DTOManager.VratiClanoveBenda(bendId);
                if (clanovi == null || !clanovi.Any())
                    return Ok($"Nema članova za bend sa ID {bendId}.");

                return Ok(clanovi);
            }
            catch (Exception e)
            {
                return BadRequest($"Greška prilikom preuzimanja članova benda: {e.Message}");
            }
        }

        [HttpGet]
        [Route("PreuzmiTehnickeZahteve")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult PreuzmiTehnickeZahteve(int id)
        {
            try
            {
                var zahtevi = DTOManager.VratiTehnickeZahteve(id);
                if (zahtevi == null || !zahtevi.Any())
                    return Ok($"Nema tehničkih zahteva za izvođača sa ID {id}.");

                return Ok(zahtevi);
            }
            catch (Exception e)
            {
                return BadRequest($"Greška prilikom preuzimanja tehničkih zahteva: {e.Message}");
            }
        }

        [HttpGet]
        [Route("PreuzmiVokalneSposobnosti")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult PreuzmiVokalneSposobnosti(int id)
        {
            try
            {
                var sposobnosti = DTOManager.VratiVokalneSposobnosti(id);
                if (sposobnosti == null || !sposobnosti.Any())
                    return Ok($"Nema vokalnih sposobnosti za izvođača sa ID {id}.");

                return Ok(sposobnosti);
            }
            catch (Exception e)
            {
                return BadRequest($"Greška prilikom preuzimanja vokalnih sposobnosti: {e.Message}");
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
                return BadRequest($"Neuspešno dodavanje soloumetnika: {e.Message}");
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
                return BadRequest($"Neuspešno dodavanje benda: {e.Message}");
            }
        }

        [HttpPost("DodajClanoveBenda")]
        public IActionResult DodajClanaBendu([FromBody] ClanBendaBasic clan)
        {
            try
            {
                DTOManager.DodajClanaBendu(clan);
                return Ok($"Uspešno ste dodali člana bendu: {clan.Ime}.");
            }
            catch (Exception e)
            {
                return BadRequest($"Neuspešno dodavanje člana benda: {e.Message}");
            }
        }

        [HttpPost("DodajTehnickiZahtev")]
        public IActionResult DodajTehnickiZahtev(int id, string zahtev)
        {
            try
            {
                DTOManager.DodajTehnickiZahtev(id, zahtev);
                return Ok($"Uspešno ste dodali tehnički zahtev: {zahtev} izvođaču sa ID: {id}.");
            }
            catch (Exception e)
            {
                return BadRequest($"Neuspešno dodavanje tehničkog zahteva: {e.Message}");
            }
        }

        [HttpPost("DodajVokalnuSposobnost")]
        public IActionResult DodajVokalnuSposobnost(int id, string sposobnost)
        {
            try
            {
                DTOManager.DodajVokalnuSposobnost(id, sposobnost);
                return Ok($"Uspešno ste dodali vokalnu sposobnost: {sposobnost} izvođaču sa ID: {id}.");
            }
            catch (Exception e)
            {
                return BadRequest($"Neuspešno dodavanje vokalne sposobnosti: {e.Message}");
            }
        }

        [HttpPut]
        [Route("IzmeniIzvodjaca")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult IzmeniIzvodjaca([FromBody] IzvodjacBasic i)
        {
            try
            {
                bool uspeh = DTOManager.IzmeniIzvodjaca(i);
                if (uspeh)
                    return Ok($"Uspešno ste izmenili izvođača: {i.Ime}.");
                else
                    return NotFound($"Izvođač sa ID {i.Id} nije pronađen.");
            }
            catch (Exception e)
            {
                return BadRequest($"Neuspešna izmena izvođača: {e.Message}");
            }
        }

        [HttpDelete]
        [Route("ObrisiIzvodjaca")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult ObrisiIzvodjaca(int id)
        {
            try
            {
                bool uspeh = DTOManager.ObrisiIzvodjaca(id);
                if (uspeh)
                    return Ok($"Uspešno ste obrisali izvođača sa ID: {id}.");
                else
                    return NotFound($"Izvođač sa ID {id} nije pronađen.");
            }
            catch (Exception e)
            {
                return BadRequest($"Neuspešno brisanje izvođača: {e.Message}");
            }
        }

        [HttpDelete]
        [Route("ObrisiClana")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult ObrisiClana([FromBody] ClanBendaBasic cb)
        {
            try
            {
                bool uspeh = DTOManager.ObrisiClana(cb);
                if (uspeh)
                    return Ok($"Uspešno ste obrisali člana benda sa ID: {cb.Id}.");
                else
                    return NotFound($"Član benda sa ID {cb.Id} nije pronađen.");
            }
            catch (Exception e)
            {
                return BadRequest($"Neuspešno brisanje člana benda: {e.Message}");
            }
        }

        [HttpDelete]
        [Route("ObrisiTehnickiZahtev")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult ObrisiTehnickiZahtev(int id, string zahtev)
        {
            try
            {
                bool uspeh = DTOManager.ObrisiTehnickiZahtev(id, zahtev);
                if (uspeh)
                    return Ok($"Uspešno ste obrisali tehnički zahtev: {zahtev} izvođaču sa ID: {id}.");
                else
                    return NotFound($"Tehnički zahtev '{zahtev}' za izvođača sa ID {id} nije pronađen.");
            }
            catch (Exception e)
            {
                return BadRequest($"Neuspešno brisanje tehničkog zahteva: {e.Message}");
            }
        }

        [HttpDelete]
        [Route("ObrisiVokalnuSposobnost")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult ObrisiVokalnuSposobnost(int id, string sposobnost)
        {
            try
            {
                bool uspeh = DTOManager.ObrisiVokalnuSposobnost(id, sposobnost);
                if (uspeh)
                    return Ok($"Uspešno ste obrisali vokalnu sposobnost: {sposobnost} izvođaču sa ID: {id}.");
                else
                    return NotFound($"Vokalna sposobnost '{sposobnost}' za izvođača sa ID {id} nije pronađena.");
            }
            catch (Exception e)
            {
                return BadRequest($"Neuspešno brisanje vokalne sposobnosti: {e.Message}");
            }
        }
    }
}
