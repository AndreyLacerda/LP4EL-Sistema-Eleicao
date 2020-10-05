using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaEleicao.Data.Contexts;
using SistemaEleicao.Models.Entities;
using SistemaEleicao.Models.PageModels;

namespace SistemaEleicao.Controllers
{
    public class InscricaoController : Controller
    {

        private AppDbContext _db;

        public InscricaoController(AppDbContext db)
        {
            _db = db;
        }

        [Route("inscricao")]
        public IActionResult FormInscricao(string id)
        {
            string idUser = HttpContext.Session.GetString("Id");
            if (idUser != null)
            {
                var eleicao = _db.Eleicoes.SingleOrDefault(e => e.CodEleicao.ToString().Equals(id));
                if (eleicao != null)
                {
                    var usuarioInscrito = _db.UsuarioEleicoes
                                            .SingleOrDefault(e => e.CodEleicao.ToString().Equals(id) &&
                                            e.CodUsuario.ToString().Equals(idUser));
                    if (usuarioInscrito == null)
                    {
                        FormularioInscricao model = new FormularioInscricao();
                        model.CodEleicao = Decimal.Parse(id);
                        ViewBag.EleicaoTitulo = eleicao.Titulo;
                        return View(model);
                    }

                }

                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Login", "Home");
        }

        [Route("inscricaoPost")]
        [HttpPost]
        public IActionResult InscreverSeEleicao(FormularioInscricao inscricao)
        {
            string idUser = HttpContext.Session.GetString("Id");
            if (idUser != null)
            {
                var eleicao = _db.Eleicoes.SingleOrDefault(e => e.CodEleicao == inscricao.CodEleicao);
                if (eleicao != null)
                {
                    var usuarioInscrito = _db.UsuarioEleicoes
                                            .SingleOrDefault(e => e.CodEleicao == inscricao.CodEleicao &&
                                            e.CodUsuario.ToString().Equals(idUser));
                    if (usuarioInscrito == null)
                    {
                        if (BCrypt.Net.BCrypt.Verify(inscricao.ChaveAcesso, eleicao.ChaveAcesso))
                        {
                            UsuarioEleicao usuario_x_eleicao = new UsuarioEleicao();
                            usuario_x_eleicao.CodEleicao = inscricao.CodEleicao;
                            usuario_x_eleicao.CodUsuario = Decimal.Parse(idUser);
                            usuario_x_eleicao.Organizador = false;
                            usuario_x_eleicao.VotoConcluido = false;

                            _db.UsuarioEleicoes.Add(usuario_x_eleicao);
                            _db.SaveChanges();

                            return RedirectToAction("VotacaoInicio", "Votacao", new { id = inscricao.CodEleicao });
                        }
                        ViewBag.EleicaoTitulo = eleicao.Titulo;
                        ViewData.Add("MensagemErro", "Chave de acesso inválida.");
                        return View("FormInscricao", inscricao);
                    }

                }

                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Login", "Home");
        }
    }
}
