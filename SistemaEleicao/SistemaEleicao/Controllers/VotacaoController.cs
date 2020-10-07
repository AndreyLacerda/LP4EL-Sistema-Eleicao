using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaEleicao.Data.Contexts;
using SistemaEleicao.Models.Entities;
using SistemaEleicao.Models.PageModels;

namespace SistemaEleicao.Controllers
{
    public class VotacaoController : Controller
    {

        private AppDbContext _db;

        public VotacaoController(AppDbContext db)
        {
            _db = db;
        }

        [Route("eleicao/inicio")]
        public IActionResult VotacaoInicio(string id)
        {
            string idUser = HttpContext.Session.GetString("Id");
            if (idUser != null)
            {
                var eleicao = _db.Eleicoes
                                .FromSqlRaw("SELECT * from eleicao where cod_eleicao IN " +
                                            "(select cod_eleicao from usuario_x_eleicao where cod_usuario = '" +
                                            idUser + "' and cod_eleicao = " + id + ")");
                if (eleicao.Count() > 0)
                {
                    if (!eleicao.First().Status.Equals("F"))
                    {
                        var usuario_x_eleicao = _db.UsuarioEleicoes
                                                    .SingleOrDefault(ue => ue.CodEleicao.ToString().Equals(id) &&
                                                                     ue.CodUsuario.ToString().Equals(idUser));
                        if (!usuario_x_eleicao.VotoConcluido)
                        {
                            return View(eleicao.First());
                        }

                        var cargos = _db.Cargos
                                    .Where(c => c.CodEleicao == eleicao.First().CodEleicao)
                                    .ToList();

                        List<VotoConcluido> votosConcluidos = new List<VotoConcluido>();
                        foreach (Cargo cargo in cargos)
                        {
                            var cargo_x_candidato = _db.CargoCandidatos
                                                        .Where(cc => cc.CodCargo == cargo.CodCargo)
                                                        .Select(cc => cc.CodCandidato)
                                                        .ToList();
                            var candidatos = _db.Candidatos
                                                .Where(c => cargo_x_candidato.Contains(c.CodCandidato))
                                                .ToList();

                            foreach (Candidato candidato in candidatos)
                            {
                                var votos = _db.Votos
                                        .Where(v => v.CodCargo == cargo.CodCargo &&
                                                v.CodCandidato == candidato.CodCandidato &&
                                                v.CodUsuario.ToString().Equals(idUser))
                                        .ToList();
                                if (votos.Count() > 0)
                                {
                                    votosConcluidos.Add(new VotoConcluido()
                                    {
                                        Candidato = candidato,
                                        Cargo = cargo.Nome,
                                        QuantidadeVotos = votos.Count()
                                    });
                                }
                            }
                        }

                        ViewBag.EleicaoTitulo = eleicao.First().Titulo;
                        return View("VotacaoConcluida", votosConcluidos);
                    }

                    ViewBag.EleicaoTitulo = eleicao.First().Titulo;
                    return RedirectToAction("VotacaoApuracao", "Votacao", new { id });
                }

                return RedirectToAction("MeusVotos", "ListaVotos");
            }
            return RedirectToAction("Login", "Home");
        }

        [Route("eleicao/votacao")]
        public IActionResult Votacao(string id)
        {
            string idUser = HttpContext.Session.GetString("Id");
            if (idUser != null)
            {
                var eleicao = _db.Eleicoes
                                .FromSqlRaw("SELECT * from eleicao where cod_eleicao IN " +
                                            "(select cod_eleicao from usuario_x_eleicao where cod_usuario = '" +
                                            idUser + "' and cod_eleicao = " + id + ")");
                if (eleicao.Count() > 0)
                {
                    var usuario_x_eleicao = _db.UsuarioEleicoes
                                                    .SingleOrDefault(ue => ue.CodEleicao.ToString().Equals(id) &&
                                                                     ue.CodUsuario.ToString().Equals(idUser));

                    if (eleicao.First().Status.Equals("I") && !usuario_x_eleicao.VotoConcluido)
                    {
                        List<Cargo> cargos = new List<Cargo>();
                        if (eleicao.First().VotoMultiplo)
                        {
                            cargos = _db.Cargos
                                        .Where(c => c.CodEleicao == eleicao.First().CodEleicao)
                                        .ToList();
                        }
                        else
                        {
                            var votos = _db.Votos
                                        .Where(v => v.CodEleicao == eleicao.First().CodEleicao &&
                                                v.CodUsuario.ToString().Equals(idUser))
                                        .Select(v => v.CodCargo)
                                        .ToList();

                            cargos = _db.Cargos
                                        .Where(c => c.CodEleicao == eleicao.First().CodEleicao &&
                                                !votos.Contains(c.CodCargo))
                                        .ToList();
                        }
                        
                        List<Votacao> votacao = new List<Votacao>();
                        foreach(Cargo cargo in cargos)
                        {
                            var cargo_x_candidato = _db.CargoCandidatos
                                                        .Where(cc => cc.CodCargo == cargo.CodCargo)
                                                        .Select(cc => cc.CodCandidato)
                                                        .ToList();
                            var candidatos = _db.Candidatos
                                                .Where(c => cargo_x_candidato.Contains(c.CodCandidato))
                                                .ToList();

                            votacao.Add(new Votacao()
                                        {
                                            Cargo = cargo,
                                            Candidatos = candidatos
                                        }
                            );
                        }
                        ViewBag.EleicaoId = eleicao.First().CodEleicao;
                        ViewBag.EleicaoTitulo = eleicao.First().Titulo;
                        ViewBag.MensagemSucesso = TempData["MensagemSucesso"] != null ? TempData["MensagemSucesso"].ToString() : null;
                        return View(votacao);
                    }
                    else
                    {
                        if (eleicao.First().Status.Equals("F"))
                        {
                            ViewBag.EleicaoTitulo = eleicao.First().Titulo;
                            return RedirectToAction("VotacaoApuracao", "Votacao", new { id });
                        }
                    }

                    ViewBag.EleicaoTitulo = eleicao.First().Titulo;
                    return RedirectToAction("VotacaoInicio", "Votacao", new { id });
                }

                return RedirectToAction("MeusVotos", "ListaVotos");
            }
            return RedirectToAction("Login", "Home");
        }

        [Route("eleicao/apuracao")]
        public IActionResult VotacaoApuracao(string id)
        {
            string idUser = HttpContext.Session.GetString("Id");
            if (idUser != null)
            {
                var eleicao = _db.Eleicoes
                                .FromSqlRaw("SELECT * from eleicao where cod_eleicao IN " +
                                            "(select cod_eleicao from usuario_x_eleicao where cod_usuario = '" +
                                            idUser + "' and cod_eleicao = " + id + ")");
                if (eleicao.Count() > 0)
                {
                    if (eleicao.First().Status.Equals("F"))
                    {
                        var cargos = _db.Cargos
                                    .Where(c => c.CodEleicao == eleicao.First().CodEleicao)
                                    .ToList();

                        List<VotoConcluido> votosUsuario = new List<VotoConcluido>();
                        List<ListaApuracao> apuracaoCargos = new List<ListaApuracao>();
                        foreach (Cargo cargo in cargos)
                        {
                            var cargo_x_candidato = _db.CargoCandidatos
                                                        .Where(cc => cc.CodCargo == cargo.CodCargo)
                                                        .Select(cc => cc.CodCandidato)
                                                        .ToList();
                            var candidatos = _db.Candidatos
                                                .Where(c => cargo_x_candidato.Contains(c.CodCandidato))
                                                .ToList();

                            List<VotoConcluido> resultados = new List<VotoConcluido>();
                            foreach (Candidato candidato in candidatos)
                            {

                                var votos = _db.Votos
                                        .Where(v => v.CodCargo == cargo.CodCargo &&
                                                v.CodCandidato == candidato.CodCandidato)
                                        .ToList();

                                List<decimal> candidatosUsuario = votos
                                                                    .Where(v => v.CodUsuario.ToString().Equals(idUser))
                                                                    .Select(v => v.CodCandidato)
                                                                    .ToList();
                                if (candidatosUsuario.Contains(candidato.CodCandidato))
                                {
                                    votosUsuario.Add(new VotoConcluido()
                                    {
                                        Candidato = candidato,
                                        Cargo = cargo.Nome,
                                        QuantidadeVotos = votos.Where(v => v.CodUsuario.ToString().Equals(idUser)).Count()
                                    });
                                }

                                resultados.Add(new VotoConcluido()
                                {
                                    Candidato = candidato,
                                    Cargo = cargo.Nome,
                                    QuantidadeVotos = votos.Count()
                                });
                            }

                            apuracaoCargos.Add(new ListaApuracao()
                            {
                                Cargo = cargo,
                                Candidatos = resultados.OrderByDescending(r => r.QuantidadeVotos).ThenBy(r => r.Candidato.Nome).ToList()
                            });
                        }
                        ApuracaoVotacao apuracao = new ApuracaoVotacao() 
                                                    {
                                                        VotosUsuario = votosUsuario
                                                                        .OrderByDescending(v => v.QuantidadeVotos)
                                                                        .ToList(),
                                                        Resultados = apuracaoCargos
                                                                        .OrderByDescending(a => a.Cargo.Nome)
                                                                        .ToList()
                        };

                        ViewBag.EleicaoTitulo = eleicao.First().Titulo;
                        return View(apuracao);
                    }

                    ViewBag.EleicaoTitulo = eleicao.First().Titulo;
                    return RedirectToAction("VotacaoInicio", "Votacao", new { id });
                }

                return RedirectToAction("MeusVotos", "ListaVotos");
            }
            return RedirectToAction("Login", "Home");
        }

        public IActionResult RegistrarVoto(string id, string cargo, string candidato)
        {
            string idUser = HttpContext.Session.GetString("Id");
            if (idUser != null)
            {
                var eleicao = _db.Eleicoes
                                .FromSqlRaw("SELECT * from eleicao where cod_eleicao IN " +
                                            "(select cod_eleicao from usuario_x_eleicao where cod_usuario = '" +
                                            idUser + "' and cod_eleicao = " + id + ")");

                var cargo_x_candidato = _db.CargoCandidatos
                                            .SingleOrDefault(cc => cc.CodCandidato.ToString().Equals(candidato) &&
                                                    cc.CodCargo.ToString().Equals(cargo));
                if (eleicao.Count() > 0 && cargo_x_candidato != null && eleicao.First().Status.Equals("I"))
                {
                    if (!eleicao.First().VotoMultiplo)
                    {
                        var votoExistente = _db.Votos
                                                .SingleOrDefault(v => v.CodCargo.ToString().Equals(cargo) &&
                                                        v.CodUsuario.ToString().Equals(idUser) &&
                                                        v.CodCandidato.ToString().Equals(candidato));
                        if (votoExistente != null)
                        {
                            ViewBag.EleicaoTitulo = eleicao.First().Titulo;
                            return RedirectToAction("VotacaoInicio", "Votacao", new { id });
                        }

                        var todosVotos = _db.Votos
                                                .Where(v => v.CodEleicao.ToString().Equals(id) &&
                                                        v.CodUsuario.ToString().Equals(idUser))
                                                .Select(v => v.CodCargo)
                                                .ToList();
                        var cargos = _db.Cargos
                                        .Where(c => c.CodEleicao.ToString().Equals(id) &&
                                                !todosVotos.Contains(c.CodCargo))
                                        .ToList();

                        if (cargos.Count() == 1)
                        {
                            var usuario_x_eleicao = _db.UsuarioEleicoes
                                                        .SingleOrDefault(ue => ue.CodEleicao.ToString().Equals(id) &&
                                                                         ue.CodUsuario.ToString().Equals(idUser));
                            usuario_x_eleicao.VotoConcluido = true;
                            _db.UsuarioEleicoes.Update(usuario_x_eleicao);
                            _db.SaveChanges();
                        }
                            
                    }

                    Voto voto = new Voto();
                    voto.CodCandidato = Decimal.Parse(candidato);
                    voto.CodCargo = Decimal.Parse(cargo);
                    voto.CodEleicao = Decimal.Parse(id);
                    voto.CodUsuario = Decimal.Parse(idUser);
                    voto.CodVoto = _db.GetMySequence("seq_voto");

                    _db.Votos.Add(voto);
                    _db.SaveChanges();

                    TempData["MensagemSucesso"] = "Seu voto anterior foi salvo com sucesso! Continue Votando!";
                    ViewBag.EleicaoTitulo = eleicao.First().Titulo;
                    return RedirectToAction("Votacao", "Votacao", new { id });
                }

                return RedirectToAction("MeusVotos", "ListaVotos");
            }

            return RedirectToAction("Login", "Home");
        }

    }
}
