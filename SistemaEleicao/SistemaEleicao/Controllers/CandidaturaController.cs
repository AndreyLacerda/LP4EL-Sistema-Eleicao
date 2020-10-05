using System.Linq;
using System.Transactions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaEleicao.Data.Contexts;
using SistemaEleicao.Models.Entities;
using SistemaEleicao.Models.PageModels;

namespace SistemaEleicao.Controllers
{
    public class CandidaturaController : Controller
    {

        private AppDbContext _db;

        public CandidaturaController(AppDbContext db)
        {
            _db = db;
        }

        [Route("eleicao/candidaturas")]
        public IActionResult Candidaturas(string id, string cargoId)
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
                    var cargo = _db.Cargos.Where(c => c.CodCargo.ToString().Equals(cargoId)).ToList();
                    if (cargo.Count() > 0)
                    {
                        var candidaturas = _db.CargoCandidatos.Where(c => c.CodCargo.ToString().Equals(cargoId)).Select(c=> c.CodCandidato).ToList();
                        var candidatos = _db.Candidatos.Where(c => candidaturas.Contains(c.CodCandidato)).ToList();
                        Candidaturas candidaturasModel = new Candidaturas();
                        candidaturasModel.Eleicao = eleicao.First();
                        candidaturasModel.Cargo = cargo.First();
                        candidaturasModel.Candidatos = candidatos;
                        return View(candidaturasModel);
                    }

                    return RedirectToAction("PainelEleicao", "PainelEleicao", new { id });
                }
                return RedirectToAction("MinhasEleicoes", "ListaEleicao");
            }

            return RedirectToAction("Login", "Home");
        }

        [Route("eleicao/cadastro-candidatura")]
        public IActionResult CadastrarCandidatura(string id, string cargoId)
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
                    var cargo = _db.Cargos.Where(c => c.CodCargo.ToString().Equals(cargoId)).ToList();
                    if (cargo.Count() > 0)
                    {
                        CandidaturaCadastro candidatura = new CandidaturaCadastro();
                        candidatura.CodEleicao = decimal.Parse(id);
                        candidatura.CodCargo = decimal.Parse(cargoId);

                        var candidaturasRegistradas = _db.CargoCandidatos
                                                        .Where(cc => cc.CodCargo.ToString().Equals(cargoId))
                                                        .Select(cc => cc.CodCandidato).ToList();

                        var candidatos = _db.Candidatos
                                            .Where(c => !candidaturasRegistradas.Contains(c.CodCandidato))
                                            .Select(c => new {
                                                        CodCandidato = c.CodCandidato,
                                                        Nome = c.Nome
                                                    }).ToList();
                        ViewBag.Candidatos = new MultiSelectList(candidatos, "CodCandidato", "Nome"); ;

                        ViewBag.EleicaoTitulo = eleicao.First().Titulo;
                        ViewBag.CargoNome = cargo.First().Nome;
                        return View(candidatura);
                    }

                    return RedirectToAction("PainelEleicao", "PainelEleicao", new { id });
                }
                return RedirectToAction("MinhasEleicoes", "ListaEleicao");
            }

            return RedirectToAction("Login", "Home");
        }

        [Route("eleicao/cadastro-candidatura/Post")]
        [HttpPost]
        public IActionResult CadastrarCandidaturaPost(CandidaturaCadastro candidaturaCadastro)
        {
            string idUser = HttpContext.Session.GetString("Id");
            if (idUser != null)
            {
                var eleicao = _db.Eleicoes
                                .FromSqlRaw("SELECT * from eleicao where cod_eleicao IN " +
                                            "(select cod_eleicao from usuario_x_eleicao where cod_usuario = '" +
                                            idUser + "' and cod_eleicao = " + candidaturaCadastro.CodEleicao + " and organizador = true)");
                if (eleicao.Count() > 0)
                {
                    foreach (decimal codCandidato in candidaturaCadastro.CodCandidato)
                    {
                        var candidaturaExistente = _db.CargoCandidatos.Where(c => c.CodCandidato.Equals(codCandidato) && c.CodCargo.Equals(candidaturaCadastro.CodCargo));
                        if (candidaturaExistente.Count() == 0)
                        {
                            CargoCandidato cargo_x_candidato = new CargoCandidato();
                            cargo_x_candidato.CodEleicao = candidaturaCadastro.CodEleicao;
                            cargo_x_candidato.CodCandidato = codCandidato;
                            cargo_x_candidato.CodCargo = candidaturaCadastro.CodCargo;

                            _db.CargoCandidatos.Add(cargo_x_candidato);
                        }
                    }
                    _db.SaveChanges();
                    return RedirectToAction("Candidaturas", new { id = candidaturaCadastro.CodEleicao, cargoId = candidaturaCadastro.CodCargo });
                }
                return RedirectToAction("MinhasEleicoes", "ListaEleicao");
            }

            return RedirectToAction("Login", "Home");
        }

        public IActionResult ExcluirCandidatura(string id, string candidato, string cargo)
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
                    var cargoExistente = _db.Cargos.SingleOrDefault(u => u.CodCargo.ToString().Equals(cargo));
                    var candidatoExistente = _db.Candidatos.SingleOrDefault(u => u.CodCandidato.ToString().Equals(candidato));
                    if (cargoExistente != null && candidatoExistente != null)
                    {
                        using (var scope = new TransactionScope(TransactionScopeOption.Required,
                                new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
                        {
                            _db.Votos.RemoveRange(_db.Votos.Where(v => v.CodCargo == cargoExistente.CodCargo && v.CodCandidato == candidatoExistente.CodCandidato));
                            _db.CargoCandidatos.RemoveRange(_db.CargoCandidatos.Where(c => c.CodCargo == cargoExistente.CodCargo && c.CodCandidato == candidatoExistente.CodCandidato));
                            _db.SaveChanges();

                            scope.Complete();
                        }
                    }

                    return RedirectToAction("Candidaturas", new { id, cargoId = cargo });
                }
            }

            return RedirectToAction("Login", "Home");
        }
    }
}
