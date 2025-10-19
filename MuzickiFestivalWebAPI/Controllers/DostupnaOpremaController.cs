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
                var oprema = DTOManager.VratiSvuDostupnuOpremu(idLokacije);

                if (oprema == null || !oprema.Any())
                    return Ok($"Nema dostupne opreme za lokaciju sa ID: {idLokacije}.");

                return Ok(oprema);
            }
            catch (Exception e)
            {
                return BadRequest($"Greška prilikom preuzimanja opreme: {e.Message}");
            }
        }

        [HttpPost]
        [Route("DodajDostupnuOpremu")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult DodajDostupnuOpremu(DostupnaOpremaBasic d)
        {
            try
            {
                if (d.Lokacija == null || d.Lokacija.Id == 0)
                {
                    return BadRequest("Lokacija za datu opremu ne postoji, morate prvo dodati lokaciju.");
                }

                DTOManager.DodajDostupnuOpremu(d);
                return Ok($"Uspešno ste dodali dostupnu opremu: {d.Naziv}.");
            }
            catch (Exception e)
            {
                return BadRequest($"Neuspešno dodavanje opreme: {e.Message}");
            }
        }

        [HttpDelete]
        [Route("ObrisiDostupnuOpremu")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult ObrisiDostupnuOpremu(int idDostupne)
        {
            try
            {
                bool uspeh = DTOManager.ObrisiDostupnuOpremu(idDostupne);

                if (uspeh)
                    return Ok($"Uspešno ste obrisali dostupnu opremu sa ID: {idDostupne}.");
                else
                    return NotFound($"Nije pronađena dostupna oprema sa ID: {idDostupne}.");
            }
            catch (Exception e)
            {
                return BadRequest($"Neuspešno brisanje opreme: {e.Message}");
            }
        }
    }
}
