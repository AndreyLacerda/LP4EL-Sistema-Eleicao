using System.ComponentModel.DataAnnotations;

namespace SistemaEleicao.Models.PageModels
{
    public class FormularioInscricao
    {

        [Required(ErrorMessage = "Este campo é obrigatório.")]
        public string ChaveAcesso { get; set; }

        public decimal CodEleicao { get; set; }

    }
}
