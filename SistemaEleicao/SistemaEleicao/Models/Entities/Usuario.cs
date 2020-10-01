using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaEleicao.Models.Entities
{
    [Table("usuario")]
    public class Usuario
    {
        public Usuario()
        {
        }

        public Usuario(string email, string nome, string senha)
        {
            Email = email;
            Nome = nome;
            Senha = senha;
        }

        [Key]
        [Column("cod_usuario", TypeName = "NUMERIC(10)")]
        public decimal CodUsuario { get; set; }

        [Column("email", TypeName = "VARCHAR(100)")]
        public string Email { get; set; }

        [Column("nome", TypeName = "VARCHAR(100)")]
        public string Nome { get; set; }

        [Column("senha", TypeName = "VARCHAR(100)")]
        public string Senha { get; set; }

    }
}
