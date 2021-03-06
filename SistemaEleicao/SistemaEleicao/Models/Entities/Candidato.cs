﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaEleicao.Models.Entities
{
    [Table("candidato")]
    public class Candidato
    {

        [Key]
        [Column("cod_candidato", TypeName = "NUMERIC(10)")]
        public decimal CodCandidato { get; set; }

        [ForeignKey("eleicao")]
        [Column("cod_eleicao", TypeName = "NUMERIC(10)")]
        public decimal CodEleicao { get; set; }

        [Column("nome", TypeName = "VARCHAR(50)")]
        public string Nome { get; set; }

        [Column("imagem", TypeName = "VARCHAR(255)")]
        public string Imagem { get; set; }

        [Column("descricao", TypeName = "VARCHAR(255)")]
        public string Descricao { get; set; }

        [Column("grupo_partido", TypeName = "VARCHAR(50)")]
        public string GrupoPartido { get; set; }

    }
}
