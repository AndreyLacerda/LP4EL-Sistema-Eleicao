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
    public class CandidatoController : Controller
    {

        private AppDbContext _db;
        private IWebHostEnvironment _hostingEnvironment;

        public CandidatoController(AppDbContext db,
                                   IWebHostEnvironment hostingEnvironment)
        {
            _db = db;
            _hostingEnvironment = hostingEnvironment;
        }

        [Route("eleicao/cadastrar-candidato")]
        public IActionResult CriarCandidato(string id)
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
                    CandidatoCadastro candidato = new CandidatoCadastro();
                    candidato.CodEleicao = Decimal.Parse(id);
                    ViewBag.EleicaoId = id;
                    ViewBag.MensagemSucesso = TempData["MensagemSucesso"] != null ? TempData["MensagemSucesso"].ToString() : null;
                    return View(candidato);
                }
                return RedirectToAction("MinhasEleicoes", "ListaEleicao");
            }

            return RedirectToAction("Login", "Home");
        }

        [Route("eleicao/editar-candidato")]
        public IActionResult EditarCandidato(string id, string candidatoId)
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
                    var candidato = _db.Candidatos.SingleOrDefault(c => c.CodCandidato.ToString().Equals(candidatoId)); ;
                    if (candidato != null)
                    {
                        CandidatoEdicao candidatoModel = new CandidatoEdicao();
                        candidatoModel.CodEleicao = candidato.CodEleicao;
                        candidatoModel.CodCandidato = candidato.CodCandidato;
                        candidatoModel.Nome = candidato.Nome;
                        candidatoModel.GrupoPartido = candidato.GrupoPartido;
                        candidatoModel.Descricao = candidato.Descricao;
                        candidatoModel.ImagemPath = candidato.Imagem;
                        candidatoModel.ImagemAntiga = candidato.Imagem.Substring(candidato.Imagem.IndexOf("_") + 1);
                        ViewBag.MensagemSucesso = TempData["MensagemSucesso"] != null ? TempData["MensagemSucesso"].ToString() : null;
                        ViewBag.EleicaoId = id;
                        return View(candidatoModel);
                    }

                    return RedirectToAction("PainelEleicao", "PainelEleicao", new { id });
                }
                return RedirectToAction("MinhasEleicoes", "ListaEleicao");
            }

            return RedirectToAction("Login", "Home");
        }

        [Route("eleicao/cadastrar-candidato/Post")]
        [HttpPost]
        public IActionResult CriarCandidatoPost(CandidatoCadastro candidatoCadastro)
        {
            string idUser = HttpContext.Session.GetString("Id");
            if (idUser != null)
            {
                var eleicao = _db.Eleicoes
                                .FromSqlRaw("SELECT * from eleicao where cod_eleicao IN " +
                                            "(select cod_eleicao from usuario_x_eleicao where cod_usuario = '" +
                                            idUser + "' and cod_eleicao = " + candidatoCadastro.CodEleicao + " and organizador = true)");
                if (eleicao.Count() > 0 && eleicao.First().Status.Equals("P"))
                {
                    var candidatoExistente = _db.Candidatos.Where(c => c.Nome.Equals(candidatoCadastro.Nome) && c.CodEleicao.Equals(candidatoCadastro.CodEleicao));
                    if (candidatoExistente.Count() == 0)
                    {
                        string pastaUpload = Path.Combine(_hostingEnvironment.WebRootPath + "/media");
                        string nomeArquivo = Guid.NewGuid().ToString() + "_" + candidatoCadastro.Imagem.FileName;
                        string pathArquivo = Path.Combine(pastaUpload, nomeArquivo);
                        FileStream fs = new FileStream(pathArquivo, FileMode.Create);
                        candidatoCadastro.Imagem.CopyTo(fs);
                        fs.Close();

                        Candidato candidato = new Candidato();
                        candidato.CodEleicao = candidatoCadastro.CodEleicao;
                        candidato.CodCandidato = _db.GetMySequence("seq_candidato");
                        candidato.Descricao = candidatoCadastro.Descricao;
                        candidato.Nome = candidatoCadastro.Nome;
                        candidato.GrupoPartido = candidatoCadastro.GrupoPartido;
                        candidato.Imagem = nomeArquivo;

                        _db.Candidatos.Add(candidato);
                        _db.SaveChanges();

                        TempData["MensagemSucesso"] = "Candidato criado com sucesso!";
                        return RedirectToAction("CriarCandidato", new { id = candidatoCadastro.CodEleicao });
                    }
                    ViewBag.MensagemErro = "Este candidato já foi cadastrado.";
                    ViewBag.EleicaoId = candidatoCadastro.CodEleicao;
                    return View("CriarCandidato");
                }
                return RedirectToAction("MinhasEleicoes", "ListaEleicao");
            }

            return RedirectToAction("Login", "Home");
        }

        [Route("eleicao/editar-candidato/Post")]
        [HttpPost]
        public IActionResult EditarCandidatoPost(CandidatoEdicao candidatoEdicao)
        {
            string idUser = HttpContext.Session.GetString("Id");
            if (idUser != null)
            {
                var eleicao = _db.Eleicoes
                                .FromSqlRaw("SELECT * from eleicao where cod_eleicao IN " +
                                            "(select cod_eleicao from usuario_x_eleicao where cod_usuario = '" +
                                            idUser + "' and cod_eleicao = " + candidatoEdicao.CodEleicao + " and organizador = true)");
                if (eleicao.Count() > 0 && eleicao.First().Status.Equals("P"))
                {
                    var candidatoExistente = _db.Candidatos.Where(c => c.CodCandidato.Equals(candidatoEdicao.CodCandidato));
                    if (candidatoExistente.Count() > 0)
                    {
                        var nomeAtual = _db.Candidatos
                                            .Where(c => c.CodCandidato.Equals(candidatoEdicao.CodCandidato))
                                            .Select(c => c.Nome)
                                            .ToList().First();
                        if (!nomeAtual.Equals(candidatoEdicao.Nome))
                        {
                            var nomeExistente = _db.Candidatos.Where(c => c.Nome.Equals(candidatoEdicao.Nome) && c.CodEleicao.Equals(candidatoEdicao.CodEleicao));
                            if (nomeExistente.Count() > 0)
                            {
                                candidatoEdicao.Nome = nomeAtual;
                                ViewBag.MensagemErro = "Já existe um candidato com este nome.";
                                ViewBag.EleicaoId = candidatoEdicao.CodEleicao;
                                return View("EditarCandidato", candidatoEdicao);
                            }
                        }
                        
                        Candidato candidato = new Candidato();
                        candidato.CodEleicao = candidatoEdicao.CodEleicao;
                        candidato.CodCandidato = candidatoEdicao.CodCandidato;
                        candidato.Descricao = candidatoEdicao.Descricao;
                        candidato.Nome = candidatoEdicao.Nome;
                        candidato.GrupoPartido = candidatoEdicao.GrupoPartido;
                        candidato.Imagem = candidatoEdicao.ImagemPath;

                        if (candidatoEdicao.Imagem != null)
                        {
                            string pastaUpload = Path.Combine(_hostingEnvironment.WebRootPath + "/media");
                            string pathArquivo = Path.Combine(pastaUpload, candidatoEdicao.ImagemPath);
                            System.IO.File.Delete(pathArquivo);
                            string nomeArquivo = Guid.NewGuid().ToString() + "_" + candidatoEdicao.Imagem.FileName;
                            pathArquivo = Path.Combine(pastaUpload, nomeArquivo);
                            FileStream fs = new FileStream(pathArquivo, FileMode.Create);
                            candidatoEdicao.Imagem.CopyTo(fs);
                            fs.Close();
                            candidato.Imagem = nomeArquivo;
                        }
                        _db.Candidatos.Update(candidato);
                        _db.SaveChanges();

                        TempData["MensagemSucesso"] = "Alterações salvas com sucesso!";
                        return RedirectToAction("EditarCandidato", new { id = candidatoEdicao.CodEleicao, candidatoId = candidatoEdicao.CodCandidato });
                    }
                    ViewBag.MensagemErro = "Este cargo não existe.";
                    ViewBag.EleicaoId = candidatoEdicao.CodEleicao;
                    return View("EditarCandidato", candidatoEdicao);
                }
                return RedirectToAction("MinhasEleicoes", "ListaEleicao");
            }

            return RedirectToAction("Login", "Home");
        }

        public IActionResult ExcluirCandidato(string id, string candidato)
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
                    var candidatoExistente = _db.Candidatos.SingleOrDefault(u => u.CodCandidato.ToString().Equals(candidato));
                    if (candidatoExistente != null)
                    {
                        using (var scope = new TransactionScope(TransactionScopeOption.Required,
                                new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
                        {
                            string pastaUpload = Path.Combine(_hostingEnvironment.WebRootPath + "/media");
                            string pathArquivo = Path.Combine(pastaUpload, candidatoExistente.Imagem);
                            System.IO.File.Delete(pathArquivo);
                            _db.Votos.RemoveRange(_db.Votos.Where(v => v.CodCargo == candidatoExistente.CodCandidato));
                            _db.CargoCandidatos.RemoveRange(_db.CargoCandidatos.Where(c => c.CodCargo == candidatoExistente.CodCandidato));
                            _db.SaveChanges();
                            _db.Candidatos.Remove(candidatoExistente);
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
