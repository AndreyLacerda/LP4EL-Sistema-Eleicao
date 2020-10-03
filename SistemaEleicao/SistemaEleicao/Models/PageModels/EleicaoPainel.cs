using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SistemaEleicao.Models.Entities;

namespace SistemaEleicao.Models.PageModels
{
    public class EleicaoPainel
    {

        public EleicaoPainel (Eleicao eleicao)
        {
            CodEleicao = eleicao.CodEleicao;
            Titulo = eleicao.Titulo;
            Descricao = eleicao.Descricao;
            ChaveAcesso = eleicao.ChaveAcesso;
            VotoMultiplo = eleicao.VotoMultiplo;
            Status = eleicao.Status;
        }

        public decimal CodEleicao { get; set; }

        public string Titulo { get; set; }

        public string Descricao { get; set; }

        public string ChaveAcesso { get; set; }

        public bool VotoMultiplo { get; set; }

        public string Status { get; set; }

        public List<Cargo> Cargos { get; set; }

        public List<Candidato> Candidatos { get; set; }
    }
}
