using Microsoft.EntityFrameworkCore;
using SistemaEleicao.Models.Entities;

namespace SistemaEleicao.Data.Contexts
{
    public class AppDbContext : DbContext
    {
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Eleicao> Eleicoes { get; set; }
        public DbSet<Cargo> Cargos { get; set; }
        public DbSet<Candidato> Candidatos { get; set; }
        public DbSet<CargoCandidato> CargoCandidatos { get; set; }
        public DbSet<UsuarioEleicao> UsuarioEleicoes { get; set; }
        public DbSet<Voto> Votos { get; set; }


        public AppDbContext(DbContextOptions<AppDbContext> options) :
            base(options)
        {
        }
    }
}