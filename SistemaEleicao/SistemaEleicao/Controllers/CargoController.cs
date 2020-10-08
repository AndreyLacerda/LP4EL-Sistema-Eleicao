using System;
using System.Linq;
using System.Transactions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaEleicao.Data.Contexts;
using SistemaEleicao.Models.Entities;
using SistemaEleicao.Models.PageModels;

namespace SistemaEleicao.Controllers
{
    public class CargoController : Controller
    {

        private AppDbContext _db;

        public CargoController(AppDbContext db)
        {
            _db = db;
        }

        [Route("eleicao/criar-cargo")]
        public IActionResult CriarCargo(string id)
        {
            string idUser = HttpContext.Session.GetString("Id");
            if (idUser != null)
            {
                var eleicao = _db.Eleicoes
                                .FromSqlRaw("SELECT * from eleicao where cod_eleicao IN " +
                                            "(select cod_eleicao from usuario_x_eleicao where cod_usuario = '" +
                                            idUser + "' and cod_eleicao = " + id + " and organizador = true)");
                if (eleicao.Count() > 0 && eleicao.First().Status.Equals("P"))
                {
                    CargoCriacao cargo = new CargoCriacao();
                    cargo.CodEleicao = Decimal.Parse(id);
                    ViewBag.EleicaoId = id;
                    ViewBag.MensagemSucesso = TempData["MensagemSucesso"] != null ? TempData["MensagemSucesso"].ToString() : null;
                    return View(cargo);
                }
                return RedirectToAction("MinhasEleicoes", "ListaEleicao");
            }

            return RedirectToAction("Login", "Home");
        }

        [Route("eleicao/editar-cargo")]
        public IActionResult EditarCargo(string id, string cargoId)
        {
            string idUser = HttpContext.Session.GetString("Id");
            if (idUser != null)
            {
                var eleicao = _db.Eleicoes
                                .FromSqlRaw("SELECT * from eleicao where cod_eleicao IN " +
                                            "(select cod_eleicao from usuario_x_eleicao where cod_usuario = '" +
                                            idUser + "' and cod_eleicao = " + id + " and organizador = true)");
                if (eleicao.Count() > 0 && eleicao.First().Status.Equals("P"))
                {
                    var cargo = _db.Cargos.SingleOrDefault(c => c.CodCargo.ToString().Equals(cargoId)); ;
                    if (cargo != null)
                    {
                        CargoCriacao cargoModel = new CargoCriacao();
                        cargoModel.CodEleicao = cargo.CodEleicao;
                        cargoModel.CodCargo = cargo.CodCargo;
                        cargoModel.Nome = cargo.Nome;
                        cargoModel.Descricao = cargo.Descricao;
                        ViewBag.MensagemSucesso = TempData["MensagemSucesso"] != null ? TempData["MensagemSucesso"].ToString() : null;
                        return View(cargoModel);
                    }

                    return RedirectToAction("PainelEleicao", "PainelEleicao", new { id });
                }
                return RedirectToAction("MinhasEleicoes", "ListaEleicao");
            }

            return RedirectToAction("Login", "Home");
        }

        [Route("eleicao/criar-cargo/Post")]
        [HttpPost]
        public IActionResult CriarCargoPost(CargoCriacao cargoCriacao)
        {
            string idUser = HttpContext.Session.GetString("Id");
            if (idUser != null)
            {
                var eleicao = _db.Eleicoes
                                .FromSqlRaw("SELECT * from eleicao where cod_eleicao IN " +
                                            "(select cod_eleicao from usuario_x_eleicao where cod_usuario = '" +
                                            idUser + "' and cod_eleicao = " + cargoCriacao.CodEleicao + " and organizador = true)");
                if (eleicao.Count() > 0 && eleicao.First().Status.Equals("P"))
                {
                    var cargoExistente = _db.Cargos.Where(c => c.Nome.Equals(cargoCriacao.Nome) && c.CodEleicao.Equals(cargoCriacao.CodEleicao));
                    if (cargoExistente.Count() == 0)
                    {
                        Cargo cargo = new Cargo();
                        cargo.CodEleicao = cargoCriacao.CodEleicao;
                        cargo.Descricao = cargoCriacao.Descricao;
                        cargo.Nome = cargoCriacao.Nome;
                        cargo.CodCargo = _db.GetMySequence("seq_cargo");

                        _db.Cargos.Add(cargo);
                        _db.SaveChanges();

                        TempData["MensagemSucesso"] = "Cargo criado com sucesso!";
                        return RedirectToAction("CriarCargo", new { id = cargoCriacao.CodEleicao });
                    }
                    ViewBag.MensagemErro = "Este cargo já existe.";
                    ViewBag.EleicaoId = cargoCriacao.CodEleicao;
                    return View("CriarCargo");
                }
                return RedirectToAction("MinhasEleicoes", "ListaEleicao");
            }

            return RedirectToAction("Login", "Home");
        }

        [Route("eleicao/editar-cargo/Post")]
        [HttpPost]
        public IActionResult EditarCargoPost(CargoCriacao cargoCriacao)
        {
            string idUser = HttpContext.Session.GetString("Id");
            if (idUser != null)
            {
                var eleicao = _db.Eleicoes
                                .FromSqlRaw("SELECT * from eleicao where cod_eleicao IN " +
                                            "(select cod_eleicao from usuario_x_eleicao where cod_usuario = '" +
                                            idUser + "' and cod_eleicao = " + cargoCriacao.CodEleicao + " and organizador = true)");
                if (eleicao.Count() > 0 && eleicao.First().Status.Equals("P"))
                {
                    var cargoExistente = _db.Cargos.Where(c => c.CodCargo.Equals(cargoCriacao.CodCargo));
                    if (cargoExistente.Count() > 0)
                    {
                        var nomeAtual = _db.Cargos
                                            .Where(c => c.CodCargo.Equals(cargoCriacao.CodCargo))
                                            .Select(c => c.Nome)
                                            .ToList().First();

                        if (!nomeAtual.Equals(cargoCriacao.Nome))
                        {
                            var nomeExistente = _db.Cargos.Where(c => c.Nome.Equals(cargoCriacao.Nome) && c.CodEleicao.Equals(cargoCriacao.CodEleicao));
                            if (nomeExistente.Count() > 0)
                            {
                                cargoCriacao.Nome = nomeAtual;
                                ViewBag.MensagemErro = "Já existe um cargo com este nome.";
                                ViewBag.EleicaoId = cargoCriacao.CodEleicao;
                                return View("EditarCargo", cargoCriacao);
                            }
                         }

                        Cargo cargo = new Cargo();
                        cargo.CodEleicao = cargoCriacao.CodEleicao;
                        cargo.Descricao = cargoCriacao.Descricao;
                        cargo.Nome = cargoCriacao.Nome;
                        cargo.CodCargo = cargoCriacao.CodCargo;

                        _db.Cargos.Update(cargo);
                        _db.SaveChanges();

                        TempData["MensagemSucesso"] = "Alterações salvas com sucesso!";
                        return RedirectToAction("EditarCargo", new { id = cargoCriacao.CodEleicao, cargoId = cargoCriacao.CodCargo });
                    }
                    ViewBag.MensagemErro = "Este cargo não existe.";
                    ViewBag.EleicaoId = cargoCriacao.CodEleicao;
                    return View("EditarCargo", cargoCriacao);
                }
                return RedirectToAction("MinhasEleicoes", "ListaEleicao");
            }

            return RedirectToAction("Login", "Home");
        }

        public IActionResult ExcluirCargo(string id, string cargo)
        {
            string idUser = HttpContext.Session.GetString("Id");
            if (idUser != null)
            {
                var eleicao = _db.Eleicoes
                                .FromSqlRaw("SELECT * from eleicao where cod_eleicao IN " +
                                            "(select cod_eleicao from usuario_x_eleicao where cod_usuario = '" +
                                            idUser + "' and cod_eleicao = " + id + " and organizador = true)");
                if (eleicao.Count() > 0 && eleicao.First().Status.Equals("P"))
                {
                    var cargoExistente = _db.Cargos.SingleOrDefault(u => u.CodCargo.ToString().Equals(cargo));
                    if (cargoExistente != null)
                    {
                        using (var scope = new TransactionScope(TransactionScopeOption.Required,
                                new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
                        {
                            _db.Votos.RemoveRange(_db.Votos.Where(v => v.CodCargo == cargoExistente.CodCargo));
                            _db.CargoCandidatos.RemoveRange(_db.CargoCandidatos.Where(c => c.CodCargo == cargoExistente.CodCargo));
                            _db.SaveChanges();
                            _db.Cargos.Remove(cargoExistente);
                            _db.SaveChanges();

                            scope.Complete();
                        }
                        
                    }

                    return RedirectToAction("PainelEleicao", "PainelEleicao", new { id });
                }
            }

            return RedirectToAction("Login", "Home");
        }
    }
}
