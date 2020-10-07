using System.Collections.Generic;
using SistemaEleicao.Models.Entities;

namespace SistemaEleicao.Models.PageModels
{
    public class Votacao
    {
        public Cargo Cargo { get; set; }

        public List<Candidato> Candidatos { get; set; }

    }
}
