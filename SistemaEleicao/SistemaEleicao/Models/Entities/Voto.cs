using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaEleicao.Models.Entities
{
    [Table("voto")]
    public class Voto
    {
        [Key]
        [Column("cod_voto", TypeName = "NUMERIC(10)")]
        public int CodVoto { get; set; }

        [ForeignKey("usuario")]
        [Required]
        [Column("cod_usuario", TypeName = "NUMERIC(10)")]
        public int CodUsuario { get; set; }

        [ForeignKey("cargo")]
        [Required]
        [Column("cod_cargo", TypeName = "NUMERIC(10)")]
        public string CodCargo { get; set; }

        [ForeignKey("candidato")]
        [Required]
        [Column("cod_candidato", TypeName = "NUMERIC(10)")]
        public string CodCandidato { get; set; }

        [ForeignKey("eleicao")]
        [Required]
        [Column("cod_eleicao", TypeName = "NUMERIC(10)")]
        public string CodEleicao { get; set; }

    }
}
