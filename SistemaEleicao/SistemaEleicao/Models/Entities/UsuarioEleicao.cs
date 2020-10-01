using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaEleicao.Models.Entities
{
    [Table("usuario_x_eleicao")]
    public class UsuarioEleicao
    {
        [Key]
        [ForeignKey("usuario")]
        [Column("cod_usuario", Order = 1, TypeName = "NUMERIC(10)")]
        public int CodUsuario { get; set; }

        [Key]
        [ForeignKey("eleicao")]
        [Column("cod_eleicao", TypeName = "NUMERIC(10)", Order = 2)]
        public string CodEleicao { get; set; }

        [Required]
        [Column("organizador", TypeName = "BOOLEAN")]
        public string Organizador { get; set; }

        [Required]
        [Column("voto_concluido", TypeName = "BOOLEAN")]
        public string VotoConcluido { get; set; }

    }
}
