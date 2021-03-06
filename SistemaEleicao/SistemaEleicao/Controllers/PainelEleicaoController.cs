﻿using System.Linq;
using System.Transactions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaEleicao.Data.Contexts;
using SistemaEleicao.Models.Entities;
using SistemaEleicao.Models.PageModels;

namespace SistemaEleicao.Controllers
{
    public class PainelEleicaoController : Controller
    {
        private AppDbContext _db;

        public PainelEleicaoController(AppDbContext db)
        {
            _db = db;
        }

        [Route("eleicao")]
        public IActionResult PainelEleicao(string id)
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
                    EleicaoPainel eleicaoPainel = new EleicaoPainel(eleicao.First());
                    var cargos = _db.Cargos.Where(c => c.CodEleicao.Equals(eleicaoPainel.CodEleicao)).ToList();
                    var candidatos = _db.Candidatos.Where(c => c.CodEleicao.Equals(eleicaoPainel.CodEleicao)).ToList();
                    eleicaoPainel.Cargos = cargos;
                    eleicaoPainel.Candidatos = candidatos;
                    ViewBag.MensagemErro = TempData["MensagemErro"] != null ? TempData["MensagemErro"].ToString() : null;
                    return View(eleicaoPainel);
                }
            }
            return RedirectToAction("Login", "Home");
        }

        public IActionResult ExcluirEleicao(string id)
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
                    using (var scope = new TransactionScope(TransactionScopeOption.Required,
                                new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
                    {
                        Eleicao registroEleicao = eleicao.First();
                        _db.Votos.RemoveRange(_db.Votos.Where(v => v.CodEleicao == eleicao.First().CodEleicao));
                        _db.CargoCandidatos.RemoveRange(_db.CargoCandidatos.Where(c => c.CodEleicao == eleicao.First().CodEleicao));
                        _db.SaveChanges();
                        _db.Candidatos.RemoveRange(_db.Candidatos.Where(c => c.CodEleicao == eleicao.First().CodEleicao));
                        _db.Cargos.RemoveRange(_db.Cargos.Where(c => c.CodEleicao == eleicao.First().CodEleicao));
                        _db.UsuarioEleicoes.RemoveRange(_db.UsuarioEleicoes.Where(u => u.CodEleicao == eleicao.First().CodEleicao));
                        _db.SaveChanges();
                        _db.Eleicoes.Remove(registroEleicao);
                        _db.SaveChanges();

                        scope.Complete();
                    }
                    return RedirectToAction("MinhasEleicoes", "ListaEleicao");
                }
            }

            return RedirectToAction("Index", "Home");
        }


        public IActionResult AlterarStatus(string id, string status)
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
                    var candidaturas = _db.CargoCandidatos
                                            .Where(cc => cc.CodEleicao == eleicao.First().CodEleicao)
                                            .ToList();

                    if (status.Equals("I") && candidaturas.Count() < 2)
                    {
                        TempData["MensagemErro"] = "Não é possível iniciar eleições com menos de duas candidaturas.";
                        return RedirectToAction("PainelEleicao", new { id = eleicao.First().CodEleicao });
                    };

                    eleicao.First().Status = status;
                    _db.Eleicoes.Update(eleicao.First());
                    _db.SaveChanges();
                    return RedirectToAction("PainelEleicao", new { id = eleicao.First().CodEleicao });
                }
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
