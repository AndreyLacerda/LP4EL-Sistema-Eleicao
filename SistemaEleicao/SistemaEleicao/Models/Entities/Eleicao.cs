using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaEleicao.Models.Entities
{
    [Table("eleicao")]
    public class Eleicao
    {
        public Eleicao()
        {
        }

        public Eleicao(string titulo, string descricao, string chave_acesso, bool voto_multiplo,string status)
        {
            Titulo = titulo;
            Descricao = descricao;
            ChaveAcesso = chave_acesso;
            VotoMultiplo = voto_multiplo;
            Status = status;
        }


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

        [Column("status", TypeName = "CHAR(1)")]
        public string Status { get; set; }
    }
}
