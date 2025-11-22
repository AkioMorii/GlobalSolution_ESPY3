using GS2_APP.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace GS2_APP.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(UsuarioModel user)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            ViewBag.ProgressoMedio = 90.0;   // double
            ViewBag.CursosConcuidos = 30;    // int
            ViewBag.RotasAndamento = 3;
            ViewBag.Tempo = "2 dias, 3:58min";
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
