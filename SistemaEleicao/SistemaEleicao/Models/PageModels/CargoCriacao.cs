using System.ComponentModel.DataAnnotations;

namespace SistemaEleicao.Models.PageModels
{
    public class CargoCriacao
    {

        public decimal CodEleicao { get; set; }

        public decimal CodCargo { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório.")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório.")]
        [Range(1, 5, ErrorMessage = "Por valor insira um valor válido entre 1 e 5.")]
        public int QuantVotos { get; set; }
    }
}
