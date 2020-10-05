using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaEleicao.Models.Entities;

namespace SistemaEleicao.Models.PageModels
{
    public class CandidaturaCadastro
    {

        public decimal CodCargo { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório.")]
        public decimal[] CodCandidato { get; set; }

        public decimal CodEleicao { get; set; }

    }
}
