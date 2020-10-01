using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaEleicao.Models.Entities
{
    [Table("usuario")]
    public class Usuario
    {

        [Key]
        [Column("cod_usuario", TypeName = "NUMERIC(10)")]
        public int CodUsuario { get; set; }

        [Required]
        [Column("email", TypeName = "VARCHAR(100)")]
        public string Email { get; set; }

        [Required]
        [Column("nome", TypeName = "VARCHAR(100)")]
        public string Nome { get; set; }

        [Required]
        [Column("senha", TypeName = "VARCHAR(100)")]
        public string Senha { get; set; }

    }
}
