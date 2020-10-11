using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaEleicao.Data.Contexts;
using SistemaEleicao.Models.Entities;
using SistemaEleicao.Models.PageModels;
using Microsoft.EntityFrameworkCore;

namespace SistemaEleicao.Controllers
{
    public class MinhaContaController : Controller
    {

        private AppDbContext _db;

        public MinhaContaController(AppDbContext db)
        {
            _db = db;
        }


        [HttpPost]
        public ActionResult Atualizar(UsuarioAtualizacao usuarioAtualizacao)
        {
            string idUser = HttpContext.Session.GetString("Id");
            if (idUser != null)
            {
                var usuario = _db.Usuarios.SingleOrDefault(u => u.Email.Equals(usuarioAtualizacao.Email));
                if (usuario != null)
                {
                    if (BCrypt.Net.BCrypt.Verify(usuarioAtualizacao.SenhaAtual, usuario.Senha))
                    {
                        usuario.Email = usuarioAtualizacao.Email;
                        usuario.Nome = usuarioAtualizacao.Nome;
                        if (!string.IsNullOrEmpty(usuarioAtualizacao.Senha)) usuario.Senha = BCrypt.Net.BCrypt.HashPassword(usuarioAtualizacao.Senha);

                        _db.SaveChanges();

                        HttpContext.Session.SetString("Nome", usuario.Nome);
                        HttpContext.Session.SetString("Email", usuario.Email);

                        return RedirectToAction("Index", "Home");
                    }
                }
                ViewBag.MensagemErro = "Algo deu errado.";
                //return View("MinhaConta");
                return View("MinhaConta", usuarioAtualizacao);
            }
            return RedirectToAction("Login", "Home");
        }

        [Route("atualizar-conta")]
        public ActionResult AtualizarConta()
        {
            UsuarioAtualizacao usuarioAtualizacao = new UsuarioAtualizacao();
            usuarioAtualizacao.Nome = HttpContext.Session.GetString("Nome");
            usuarioAtualizacao.Email = HttpContext.Session.GetString("Email");
            return View("MinhaConta", usuarioAtualizacao);
        }

    }
}
