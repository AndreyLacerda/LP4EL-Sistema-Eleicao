using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SistemaEleicao.Models;
using SistemaEleicao.Models.PageModels;

namespace SistemaEleicao.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [Route("")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("Id");
            HttpContext.Session.Remove("Nome");
            HttpContext.Session.Remove("Email");
            return View("Index");
        }

        [Route("login")]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("Id") != null)
            {
                return RedirectToAction("Index", "Home");
            }
            UsuarioLogin usuario = new UsuarioLogin();
            return View("../Autenticacao/Login", usuario);
        }

        [Route("cadastro")]
        public IActionResult Cadastro()
        {
            if (HttpContext.Session.GetString("Id") != null)
            {
                return RedirectToAction("Index", "Home");
            }
            UsuarioCadastro usuario = new UsuarioCadastro();
            return View("../Autenticacao/Cadastro", usuario);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
