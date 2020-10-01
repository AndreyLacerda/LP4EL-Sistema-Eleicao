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
        public decimal CodEleicao { get; set; }

        [Column("titulo", TypeName = "VARCHAR(100)")]
        public string Titulo { get; set; }

        [Column("descricao", TypeName = "VARCHAR(500)")]
        public string Descricao { get; set; }

        [Column("chave_acesso", TypeName = "VARCHAR(160)")]
        public string ChaveAcesso { get; set; }

        [Column("voto_multiplo", TypeName = "BOOLEAN")]
        public bool VotoMultiplo { get; set; }

        [Column("status", TypeName = "status_eleicao_t")]
        public string Status { get; set; }

    }
}
