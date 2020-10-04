using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace SistemaEleicao.Models.PageModels
{
    public class CandidatoCadastro
    {

        public decimal CodCandidato { get; set; }

        public decimal CodEleicao { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório.")]
        public IFormFile Imagem { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório.")]
        public string Descricao { get; set; }

        public string GrupoPartido { get; set; }
    }
}
