using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SistemaEleicao.Models.Entities;

namespace SistemaEleicao.Models.PageModels
{
    public class ListaApuracao
    {
        public Cargo Cargo { get; set; }
        public List<VotoConcluido> Candidatos { get; set; }

    }
}
