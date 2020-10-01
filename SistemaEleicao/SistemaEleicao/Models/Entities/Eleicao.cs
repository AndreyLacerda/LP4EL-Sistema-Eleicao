using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaEleicao.Models.Entities
{
    [Table("eleicao")]
    public class Eleicao
    {

        [Key]
        [Column("cod_eleicao", TypeName = "NUMERIC(10)")]
        public int CodEleicao { get; set; }

        [Required]
        [Column("titulo", TypeName = "VARCHAR(100)")]
        public string Titulo { get; set; }

        [Required]
        [Column("descricao", TypeName = "VARCHAR(500)")]
        public string Descricao { get; set; }

        [Required]
        [Column("chave_acesso", TypeName = "VARCHAR(160)")]
        public string ChaveAcesso { get; set; }

        [Required]
        [Column("voto_multiplo", TypeName = "BOOLEAN")]
        public bool VotoMultiplo { get; set; }

        [Required]
        [Column("status", TypeName = "status_eleicao_t")]
        public string Status { get; set; }

    }
}
