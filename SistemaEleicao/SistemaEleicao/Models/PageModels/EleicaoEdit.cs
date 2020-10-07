using System.ComponentModel.DataAnnotations;

namespace SistemaEleicao.Models.PageModels
{
    public class EleicoesEdit
    {

        public decimal CodEleicao { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório.")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório.")]
        public string Descricao { get; set; }

        public string ChaveAcesso { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório.")]
        public bool VotoMultiplo { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório.")]
        public string Status { get; set; }

        [Compare("ChaveAcesso", ErrorMessage = "As chaves não estão iguais.")]
        public string ConfirmacaoChave { get; set; }
    }
}
