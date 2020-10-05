using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaEleicao.Data.Contexts;

namespace SistemaEleicao.Controllers
{
    public class ListaVotos : Controller
    {

        private AppDbContext _db;

        public ListaVotos(AppDbContext db)
        {
            _db = db;
        }

        [Route("meus-votos")]
        public IActionResult MeusVotos()
        {
            string idUser = HttpContext.Session.GetString("Id");
            if (idUser != null)
            {
                var usuario_x_eleicao = _db.UsuarioEleicoes
                                            .Where(ue => ue.CodUsuario.ToString().Equals(idUser))
                                            .Select(e=> e.CodEleicao)
                                            .ToList();
                var eleicoes = _db.Eleicoes
                                .Where(e => usuario_x_eleicao.Contains(e.CodEleicao))
                                .ToList();
                return View(eleicoes);
            }
            return RedirectToAction("Login", "Home");
        }
    }
}
