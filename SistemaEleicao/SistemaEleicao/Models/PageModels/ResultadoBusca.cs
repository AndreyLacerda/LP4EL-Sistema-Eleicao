
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SistemaEleicao.Models.Entities;

namespace SistemaEleicao.Models.PageModels
{
    public class ResultadoBusca
    {
        public string ValorPesquisa { get; set; }

        public List<Eleicao> Eleicoes { get; set; }
    }
}
