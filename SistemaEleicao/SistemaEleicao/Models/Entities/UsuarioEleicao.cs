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
        public decimal CodUsuario { get; set; }

        [Key]
        [ForeignKey("eleicao")]
        [Column("cod_eleicao", TypeName = "NUMERIC(10)", Order = 2)]
        public decimal CodEleicao { get; set; }

        [Column("organizador", TypeName = "BOOLEAN")]
        public bool Organizador { get; set; }

        [Column("voto_concluido", TypeName = "BOOLEAN")]
        public bool VotoConcluido { get; set; }

    }
}
