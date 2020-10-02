using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaEleicao.Data.Contexts;
using SistemaEleicao.Models.Entities;

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
            var eleicoes = _db.Eleicoes
                                .FromSqlRaw("SELECT * from eleicao where cod_eleicao IN " + 
                                            "(select cod_eleicao from usuario_x_eleicao where cod_usuario = '" + 
                                            idUser + "' and organizador = true)");
            return View(eleicoes);
        }
    }
}
