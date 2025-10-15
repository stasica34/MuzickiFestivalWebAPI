using Microsoft.AspNetCore.Mvc;

namespace MuzickiFestivalWebAPI.Controllers
{
    public class UlaznicaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
