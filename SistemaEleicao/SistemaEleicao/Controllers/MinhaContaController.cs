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
                var usuario = _db.Usuarios.SingleOrDefault(u => u.CodUsuario.ToString().Equals(idUser));
                if (usuario != null)
                {
                    if (BCrypt.Net.BCrypt.Verify(usuarioAtualizacao.SenhaAtual, usuario.Senha))
                    {
                        if (!usuario.Email.Equals(usuarioAtualizacao.Email))
                        {
                            var emailsExistentes = _db.Usuarios
                                                        .Where(u => u.Email.Equals(usuarioAtualizacao.Email))
                                                        .Select(u => u.Email)
                                                        .ToList();
                            if (emailsExistentes.Count() > 0)
                            {
                                ViewBag.MensagemErro = "Este e-mail já está sendo utilizado por outra conta.";
                                return View("MinhaConta", usuarioAtualizacao);
                            }
                        }
                        usuario.Email = usuarioAtualizacao.Email;
                        usuario.Nome = usuarioAtualizacao.Nome;
                        if (!string.IsNullOrEmpty(usuarioAtualizacao.Senha)) usuario.Senha = BCrypt.Net.BCrypt.HashPassword(usuarioAtualizacao.Senha);

                        _db.SaveChanges();

                        HttpContext.Session.SetString("Nome", usuario.Nome);
                        HttpContext.Session.SetString("Email", usuario.Email);

                        TempData["MensagemSucesso"] = "Alterações salvas com sucesso.";
                        return RedirectToAction("AtualizarConta");
                    }

                    ViewBag.MensagemErro = "Senha atual incorreta.";
                    return View("MinhaConta", usuarioAtualizacao);
                }
            }
            return RedirectToAction("Login", "Home");
        }

        [Route("minha-conta")]
        public ActionResult AtualizarConta()
        {
            UsuarioAtualizacao usuarioAtualizacao = new UsuarioAtualizacao();
            usuarioAtualizacao.Nome = HttpContext.Session.GetString("Nome");
            usuarioAtualizacao.Email = HttpContext.Session.GetString("Email");
            ViewBag.MensagemSucesso = TempData["MensagemSucesso"] != null ? TempData["MensagemSucesso"].ToString() : null;
            return View("MinhaConta", usuarioAtualizacao);
        }

    }
}
