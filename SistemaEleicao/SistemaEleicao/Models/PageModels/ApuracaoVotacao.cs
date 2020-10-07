using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaEleicao.Models.PageModels
{
    public class ApuracaoVotacao
    {
        public List<ListaApuracao> Resultados { get; set; }

        public List<VotoConcluido> VotosUsuario { get; set; }
    }
}
