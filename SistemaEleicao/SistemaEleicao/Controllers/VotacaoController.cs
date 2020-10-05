using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaEleicao.Data.Contexts;

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
                        return View(eleicao.First());
                    }

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
                    if (!eleicao.First().Status.Equals("F"))
                    {
                        return View(eleicao.First());
                    }

                    return RedirectToAction("VotacaoApuracao", "Votacao", new { id });
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
                        return View(eleicao.First());
                    }

                    return RedirectToAction("VotacaoInicio", "Votacao", new { id });
                }

                return RedirectToAction("MeusVotos", "ListaVotos");
            }
            return RedirectToAction("Login", "Home");
        }

    }
}
