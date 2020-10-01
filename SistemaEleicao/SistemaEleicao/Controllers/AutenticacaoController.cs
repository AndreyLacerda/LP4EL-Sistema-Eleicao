using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaEleicao.Data.Contexts;
using SistemaEleicao.Models.Entities;
using SistemaEleicao.Models.PageModels;

namespace SistemaEleicao.Controllers
{
    public class AutenticacaoController : Controller
    {

        private AppDbContext _db;

        public AutenticacaoController(AppDbContext db)
        {
            _db = db;
        }

        [HttpPost]
        public ActionResult Cadastrar(UsuarioCadastro usuarioCadastro)
        {
            var usuarioExistente = _db.Usuarios.SingleOrDefault(u => u.Email.Equals(usuarioCadastro.Email));
            if (usuarioExistente == null)
            {
                Usuario usuario = new Usuario(usuarioCadastro.Email, usuarioCadastro.Nome, usuarioCadastro.Senha);
                usuario.CodUsuario = _db.GetMySequence("seq_usuario");
                usuario.Senha = BCrypt.Net.BCrypt.HashPassword(usuario.Senha);
                _db.Usuarios.Add(usuario);
                _db.SaveChanges();
                HttpContext.Session.SetString("Id", usuario.CodUsuario.ToString());
                HttpContext.Session.SetString("Nome", usuario.Nome);
                HttpContext.Session.SetString("Email", usuario.Email);
                return RedirectToAction("Index", "Home");
            }
            ViewBag.MensagemErro = "Este e-mail já está em uso.";
            return View("Cadastro");
        }

        [HttpPost]
        public ActionResult Logar(UsuarioLogin usuarioLogin)
        {
            var usuarioExistente = _db.Usuarios.SingleOrDefault(u => u.Email.Equals(usuarioLogin.Email));
            if (usuarioExistente != null)
            {
                if (BCrypt.Net.BCrypt.Verify(usuarioLogin.Senha, usuarioExistente.Senha))
                {
                    HttpContext.Session.SetString("Id", usuarioExistente.CodUsuario.ToString());
                    HttpContext.Session.SetString("Nome", usuarioExistente.Nome);
                    HttpContext.Session.SetString("Email", usuarioExistente.Email);
                    return RedirectToAction("Index", "Home");
                }
            }
            ViewBag.MensagemErro = "E-mail e/ou senha inválidos.";
            return View("Login");
        }
    }
}
