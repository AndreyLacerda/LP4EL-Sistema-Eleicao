using System.ComponentModel.DataAnnotations;

namespace SistemaEleicao.Models.PageModels
{
    public class UsuarioLogin
    {

        [Required(ErrorMessage = "Este campo é obrigatório.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório.")]
        public string Senha { get; set; }

    }
}
