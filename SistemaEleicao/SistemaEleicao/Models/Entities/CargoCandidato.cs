using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaEleicao.Models.Entities
{
    [Table("cargo_x_candidato")]
    public class CargoCandidato
    {
        [ForeignKey("cargo")]
        [Column("cod_cargo", TypeName = "NUMERIC(10)", Order = 1)]
        public decimal CodCargo { get; set; }

        [ForeignKey("candidato")]
        [Column("cod_candidato", TypeName = "NUMERIC(10)", Order = 2)]
        public decimal CodCandidato { get; set; }

        [ForeignKey("eleicao")]
        [Column("cod_eleicao", TypeName = "NUMERIC(10)")]
        public decimal CodEleicao { get; set; }

    }
}
