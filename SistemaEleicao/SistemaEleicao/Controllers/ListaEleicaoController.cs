﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaEleicao.Data.Contexts;

namespace SistemaEleicao.Controllers
{
    public class ListaEleicaoController : Controller
    {

        private AppDbContext _db;

        public ListaEleicaoController(AppDbContext db)
        {
            _db = db;
        }

        [Route("minhas-eleicoes")]
        public IActionResult MinhasEleicoes()
        {
            string idUser = HttpContext.Session.GetString("Id");
            if (idUser != null)
            {
                var eleicoes = _db.Eleicoes
                                .FromSqlRaw("SELECT * from eleicao where cod_eleicao IN " +
                                            "(select cod_eleicao from usuario_x_eleicao where cod_usuario = '" +
                                            idUser + "' and organizador = true) ORDER BY titulo");
                return View(eleicoes);
            }
            return RedirectToAction("Login", "Home");
        }
    }
}
