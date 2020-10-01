using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaEleicao.Models.Entities
{
    [Table("candidato")]
    public class Candidato
    {

        [Key]
        [Column("cod_candidato", TypeName = "NUMERIC(10)")]
        public int CodCandidato { get; set; }

        [ForeignKey("eleicao")]
        [Required]
        [Column("cod_eleicao", TypeName = "NUMERIC(10)")]
        public int CodEleicao { get; set; }

        [Required]
        [Column("nome", TypeName = "VARCHAR(50)")]
        public string Nome { get; set; }

        [Required]
        [Column("imagem", TypeName = "VARCHAR(255)")]
        public string Imagem { get; set; }

        [Column("descricao", TypeName = "VARCHAR(255)")]
        public bool Descricao { get; set; }

        [Column("grupo_partido", TypeName = "VARCHAR(50)")]
        public bool GrupoPartido { get; set; }

    }
}
