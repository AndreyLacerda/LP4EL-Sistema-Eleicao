using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaEleicao.Models.Entities
{
    [Table("cargo")]
    public class Cargo
    {
        [Key]
        [Column("cod_cargo", TypeName = "NUMERIC(10)")]
        public decimal CodCargo { get; set; }

        [ForeignKey("eleicao")]
        [Column("cod_eleicao", TypeName = "NUMERIC(10)")]
        public decimal CodEleicao { get; set; }

        [Column("nome", TypeName = "VARCHAR(50)")]
        public string Nome { get; set; }

        [Column("descricao", TypeName = "VARCHAR(255)")]
        public string Descricao { get; set; }

        [Column("quant_votos", TypeName = "INT")]
        public bool QuantVotos { get; set; }

    }
}
