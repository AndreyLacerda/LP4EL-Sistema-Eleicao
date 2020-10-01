using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaEleicao.Models.Entities
{
    [Table("cargo_x_candidato")]
    public class CargoCandidato
    {
        [Key]
        [ForeignKey("cargo")]
        [Column("cod_cargo", TypeName = "NUMERIC(10)", Order = 1)]
        public int CodCargo { get; set; }

        [Key]
        [ForeignKey("candidato")]
        [Column("cod_candidato", TypeName = "NUMERIC(10)", Order = 2)]
        public int CodCandidato { get; set; }

        [ForeignKey("eleicao")]
        [Required]
        [Column("cod_eleicao", TypeName = "NUMERIC(10)")]
        public string CodEleicao { get; set; }

    }
}
