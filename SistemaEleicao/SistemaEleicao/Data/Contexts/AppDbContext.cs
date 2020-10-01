using System;
using Microsoft.EntityFrameworkCore;
using Npgsql;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasSequence<decimal>("seq_candidato")
                .StartsAt(0)
                .IncrementsBy(1);
            modelBuilder.HasSequence<decimal>("seq_cargo")
                .StartsAt(0)
                .IncrementsBy(1);
            modelBuilder.HasSequence<decimal>("seq_eleicao")
                .StartsAt(0)
                .IncrementsBy(1);
            modelBuilder.HasSequence<decimal>("seq_usuario")
                .StartsAt(0)
                .IncrementsBy(1);
            modelBuilder.HasSequence<decimal>("seq_voto")
                .StartsAt(0)
                .IncrementsBy(1);

            modelBuilder.Entity<CargoCandidato>()
                .HasKey(c => new { c.CodCandidato, c.CodCargo });
            modelBuilder.Entity<UsuarioEleicao>()
                .HasKey(c => new { c.CodUsuario, c.CodEleicao });

            modelBuilder.Entity<Usuario>()
                .Property(u => u.CodUsuario)
                .HasDefaultValueSql("nextval('seq_usuario')");
            modelBuilder.Entity<Cargo>()
                .Property(c => c.CodCargo)
                .HasDefaultValueSql("nextval('seq_cargo')");
            modelBuilder.Entity<Candidato>()
                .Property(c => c.CodCandidato)
                .HasDefaultValueSql("nextval('seq_candidato')");
            modelBuilder.Entity<Eleicao>()
                .Property(e => e.CodEleicao)
                .HasDefaultValueSql("nextval('seq_eleicao')");
            modelBuilder.Entity<Voto>()
                .Property(v => v.CodVoto)
                .HasDefaultValueSql("nextval('seq_voto')");

        }

        public decimal GetMySequence(string sequence)
        {
            NpgsqlParameter result = new NpgsqlParameter("@result", System.Data.SqlDbType.Decimal)
            {
                Direction = System.Data.ParameterDirection.Output
            };

            Database.ExecuteSqlRaw("SELECT nextval('" + sequence + "')", result);

            return Convert.ToDecimal(result.Value);
        }

    }
}