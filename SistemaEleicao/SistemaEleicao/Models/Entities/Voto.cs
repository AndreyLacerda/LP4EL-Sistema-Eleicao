using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaEleicao.Models.Entities
{
    [Table("voto")]
    public class Voto
    {
        [Key]
        [Column("cod_voto", TypeName = "NUMERIC(10)")]
        public decimal CodVoto { get; set; }

        [ForeignKey("usuario")]
        [Required]
        [Column("cod_usuario", TypeName = "NUMERIC(10)")]
        public decimal CodUsuario { get; set; }

        [ForeignKey("cargo")]
        [Column("cod_cargo", TypeName = "NUMERIC(10)")]
        public decimal CodCargo { get; set; }

        [ForeignKey("candidato")]
        [Column("cod_candidato", TypeName = "NUMERIC(10)")]
        public decimal CodCandidato { get; set; }

        [ForeignKey("eleicao")]
        [Column("cod_eleicao", TypeName = "NUMERIC(10)")]
        public decimal CodEleicao { get; set; }

    }
}
