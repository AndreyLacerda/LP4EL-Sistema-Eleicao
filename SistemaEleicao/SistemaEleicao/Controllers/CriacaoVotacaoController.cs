using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaEleicao.Data.Contexts;
using SistemaEleicao.Models.Entities;
using SistemaEleicao.Models.PageModels;

namespace SistemaEleicao.Controllers
{
    public class CriacaoVotacaoController : Controller
    {

        private AppDbContext _db;

        public CriacaoVotacaoController(AppDbContext db)
        {
            _db = db;
        }

        [Route("criar-votacao")]
        public IActionResult CriacaoVotacao()
        {
            if (HttpContext.Session.GetString("Id") != null)
            {
                return View();
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult CadastrarEleicao(Eleicoes criarVotacao)
        {
            string idUser = HttpContext.Session.GetString("Id");
            var usuario = _db.Usuarios.SingleOrDefault(u => u.CodUsuario.ToString().Equals(idUser));
            if (usuario != null)
            {
                var nomeVotacaoExistente = _db.Eleicoes.SingleOrDefault(u => u.Titulo.Equals(criarVotacao.Titulo));
                if (nomeVotacaoExistente == null)
                {
                    criarVotacao.ChaveAcesso = BCrypt.Net.BCrypt.HashPassword(criarVotacao.ChaveAcesso);
                    Eleicao eleicao = new Eleicao(criarVotacao.Titulo, criarVotacao.Descricao, criarVotacao.ChaveAcesso, criarVotacao.VotoMultiplo, "P");
                    eleicao.CodEleicao = _db.GetMySequence("seq_eleicao");
                    UsuarioEleicao usuario_x_eleicao = new UsuarioEleicao();
                    usuario_x_eleicao.CodEleicao = eleicao.CodEleicao;
                    usuario_x_eleicao.CodUsuario = usuario.CodUsuario;
                    usuario_x_eleicao.Organizador = true;
                    usuario_x_eleicao.VotoConcluido = false;
                    _db.Eleicoes.Add(eleicao);
                    _db.UsuarioEleicoes.Add(usuario_x_eleicao);
                    _db.SaveChanges();
                    return RedirectToAction("Index", "Home");
                }
                ViewBag.MensagemErro = "Este título já está em uso.";
                return View("CriacaoVotacao");
            }
            return RedirectToAction("Login", "Home");
        }
    }
}
