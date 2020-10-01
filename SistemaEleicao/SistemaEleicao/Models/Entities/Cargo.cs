using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaEleicao.Models.Entities
{
    [Table("cargo")]
    public class Cargo
    {
        [Key]
        [Column("cod_cargo", TypeName = "NUMERIC(10)")]
        public int CodCargo { get; set; }

        [ForeignKey("eleicao")]
        [Required]
        [Column("cod_eleicao", TypeName = "NUMERIC(10)")]
        public int CodEleicao { get; set; }

        [Required]
        [Column("nome", TypeName = "VARCHAR(50)")]
        public string Nome { get; set; }

        [Required]
        [Column("descricao", TypeName = "VARCHAR(255)")]
        public string Descricao { get; set; }

        [Required]
        [Column("quant_votos", TypeName = "INT")]
        public bool QuantVotos { get; set; }

    }
}
