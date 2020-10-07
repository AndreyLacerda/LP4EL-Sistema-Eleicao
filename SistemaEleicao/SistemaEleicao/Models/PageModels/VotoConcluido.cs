using SistemaEleicao.Models.Entities;

namespace SistemaEleicao.Models.PageModels
{
    public class VotoConcluido
    {
        public Candidato Candidato { get; set; }
        public string Cargo { get; set; }
        public long QuantidadeVotos { get; set; }

    }
}
