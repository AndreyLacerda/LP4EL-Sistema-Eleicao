using System;
using System.IO;
using System.Linq;
using System.Transactions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
                    return RedirectToAction("PainelEleicao", "PainelEleicao", new { id = eleicao.CodEleicao });
                }
                ViewBag.MensagemErro = "Este título já está em uso.";
                return View("CriacaoVotacao");
            }
            return RedirectToAction("Login", "Home");
        }

        [Route("eleicao/editar-eleicao")]
        public IActionResult EditarVotacao(string id)
        {
            string idUser = HttpContext.Session.GetString("Id");
            if (idUser != null)
            {
                var eleicao = _db.Eleicoes
                                .FromSqlRaw("SELECT * from eleicao where cod_eleicao IN " +
                                            "(select cod_eleicao from usuario_x_eleicao where cod_usuario = '" +
                                            idUser + "' and cod_eleicao = " + id + " and organizador = true)");
                if (eleicao.Count() > 0)
                {
                    var eleicaoEdit = _db.Eleicoes.SingleOrDefault(e => e.CodEleicao.ToString().Equals(id));
                    if (eleicao != null)
                    {
                        EleicoesEdit eleicaoModel = new EleicoesEdit();
                        eleicaoModel.CodEleicao = eleicaoEdit.CodEleicao;
                        eleicaoModel.Titulo = eleicaoEdit.Titulo;
                        eleicaoModel.Descricao = eleicaoEdit.Descricao;
                        eleicaoModel.Status = eleicaoEdit.Status;
                        eleicaoModel.VotoMultiplo = eleicaoEdit.VotoMultiplo;
                        ViewBag.MensagemSucesso = TempData["MensagemSucesso"] != null ? TempData["MensagemSucesso"].ToString() : null;
                        return View(eleicaoModel);
                    }

                    return RedirectToAction("PainelEleicao", "PainelEleicao", new { id });
                }
                return RedirectToAction("MinhasEleicoes", "ListaEleicao");
            }

            return RedirectToAction("Login", "Home");
        }
        [Route("eleicao/editar-eleicao/Post")]
        [HttpPost]
        public IActionResult EditarVotacaoPost(EleicoesEdit cadastrarEleicao)
        {
            string idUser = HttpContext.Session.GetString("Id");
            if (idUser != null)
            {
                var eleicao = _db.Eleicoes
                                .FromSqlRaw("SELECT * from eleicao where cod_eleicao IN " +
                                            "(select cod_eleicao from usuario_x_eleicao where cod_usuario = '" +
                                            idUser + "' and cod_eleicao = " + cadastrarEleicao.CodEleicao + " and organizador = true)");
                if (eleicao.Count() > 0)
                {
                    var nomeAtual = _db.Eleicoes
                                        .Where(e => e.CodEleicao == cadastrarEleicao.CodEleicao)
                                        .Select(e => e.Titulo)
                                        .ToList()
                                        .First();
                    if (!nomeAtual.Equals(cadastrarEleicao.Titulo))
                    {
                        var nomeExistente = _db.Eleicoes.Where(e => e.Titulo.Equals(cadastrarEleicao.Titulo)).ToList();
                        if (nomeExistente.Count() > 0)
                        {
                            ViewBag.MensagemErro = "Este título já está sendo utilizado.";
                            return View("EditarVotacao", cadastrarEleicao);
                        }
                    }
                    if (cadastrarEleicao.ChaveAcesso == null)
                    {
                        var chaveAcessoAtual = _db.Eleicoes
                                                  .Where(e => e.CodEleicao == cadastrarEleicao.CodEleicao)
                                                  .Select(e => e.ChaveAcesso)
                                                  .ToList()
                                                  .First();
                        cadastrarEleicao.ChaveAcesso = chaveAcessoAtual;
                    }
                    else
                    {
                        cadastrarEleicao.ChaveAcesso = BCrypt.Net.BCrypt.HashPassword(cadastrarEleicao.ChaveAcesso);
                    }
                    Eleicao eleicaoModel = new Eleicao();
                    eleicaoModel.ChaveAcesso = cadastrarEleicao.ChaveAcesso;
                    eleicaoModel.CodEleicao = cadastrarEleicao.CodEleicao;
                    eleicaoModel.Titulo = cadastrarEleicao.Titulo;
                    eleicaoModel.Descricao = cadastrarEleicao.Descricao;
                    eleicaoModel.Status = cadastrarEleicao.Status;
                    eleicaoModel.VotoMultiplo = cadastrarEleicao.VotoMultiplo;
                    _db.Eleicoes.Update(eleicaoModel);
                    _db.SaveChanges();
                    TempData["MensagemSucesso"] = "Eleição alterada com sucesso.";
                    return RedirectToAction("EditarVotacao", "CriacaoVotacao", new { id = cadastrarEleicao.CodEleicao });
                }
                return RedirectToAction("MinhasEleicoes", "ListaEleicao");
            }

            return RedirectToAction("Login", "Home");
        }
    }
}
