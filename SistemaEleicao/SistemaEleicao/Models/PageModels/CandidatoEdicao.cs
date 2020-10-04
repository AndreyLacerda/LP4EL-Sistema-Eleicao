using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace SistemaEleicao.Models.PageModels
{
    public class CandidatoEdicao
    {
        public decimal CodCandidato { get; set; }

        public decimal CodEleicao { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório.")]
        public string Nome { get; set; }

        public IFormFile Imagem { get; set; }

        public string ImagemAntiga { get; set; }

        public string ImagemPath { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório.")]
        public string Descricao { get; set; }

        public string GrupoPartido { get; set; }
    }
}
