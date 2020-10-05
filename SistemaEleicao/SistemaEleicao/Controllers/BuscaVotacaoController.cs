using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaEleicao.Data.Contexts;
using SistemaEleicao.Models.PageModels;

namespace SistemaEleicao.Controllers
{
    public class BuscaVotacaoController : Controller
    {

        private AppDbContext _db;

        public BuscaVotacaoController(AppDbContext db)
        {
            _db = db;
        }

        [Route("busca")]
        public IActionResult Busca(string input)
        {
            string idUser = HttpContext.Session.GetString("Id");
            if (idUser != null)
            {
                ResultadoBusca resultado = new ResultadoBusca();
                resultado.ValorPesquisa = input;

                var eleicoesInscrito = _db.UsuarioEleicoes
                                            .Where(ue => ue.CodUsuario.ToString().Equals(idUser))
                                            .Select(ue => ue.CodEleicao)
                                            .ToList();
                var eleicoesDisponiveis = _db.Eleicoes
                                            .Where(e => !eleicoesInscrito.Contains(e.CodEleicao) &&
                                            e.Titulo.ToUpper().Contains(input.ToUpper()) &&
                                            !e.Status.Equals("F"))
                                            .ToList();
                resultado.Eleicoes = eleicoesDisponiveis;

                return View(resultado);
            }

            return RedirectToAction("Login", "Home");
        }

    }
}
