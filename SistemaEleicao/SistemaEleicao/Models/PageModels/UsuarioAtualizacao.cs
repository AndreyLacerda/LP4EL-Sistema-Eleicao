using System.ComponentModel.DataAnnotations;

namespace SistemaEleicao.Models.PageModels
{
    public class UsuarioAtualizacao
    {

        [Required(ErrorMessage = "Este campo é obrigatório.")]
        [EmailAddress(ErrorMessage = "E-mail inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório.")]
        public string Nome { get; set; }

        [MinLength(0, ErrorMessage = "A senha deve conter pelo menos {1} caracteres.")]
        [RegularExpression(@"^((?=.*[a-z])(?=.*[A-Z])(?=.*\d)).+$", ErrorMessage = "A senha deve conter números, caracteres maiúsculos e mínusculos.")]
        public string Senha { get; set; }

        [Compare("Senha", ErrorMessage = "As senhas não estão iguais.")]
        public string ConfirmacaoSenha { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório para validar as alterações.")]
        public string SenhaAtual { get; set; }

    }
}
