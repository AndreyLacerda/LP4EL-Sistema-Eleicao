using System.Collections.Generic;
using SistemaEleicao.Models.Entities;

namespace SistemaEleicao.Models.PageModels
{
    public class Candidaturas
    {
        public Eleicao Eleicao { get; set; }
        public Cargo Cargo { get; set; }
        public List<Candidato> Candidatos { get; set; }
    }
}
