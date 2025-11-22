using GS2_Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Reflection;

namespace GS2_Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<TipoUsuario> TipoUsuarios { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        public DbSet<TrilhaAprendizagem> TrilhasAprendizagem { get; set; }
        public DbSet<Nivel> Niveis { get; set; }

        public DbSet<PalavrasChaves> PalavrasChaves { get; set; }

        public DbSet<TipoConteudo> TiposConteudo { get; set; }

        public DbSet<Curso> Cursos { get; set; }
        public DbSet<Conteudo> Conteudos { get; set; }

        public DbSet<MeusCursos> MeusCursos { get; set; }

        public DbSet<CursoPalavraChave> CursoPalavraChave { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //PostGreSql não se dá bem com DATETIME : faz conversão para timestapm.
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(DateTime) || property.ClrType == typeof(DateTime?))
                    {
                        property.SetValueConverter(new ValueConverter<DateTime, DateTime>(
                            v => v.ToUniversalTime(),
                            v => DateTime.SpecifyKind(v, DateTimeKind.Utc)));
                    }
                }
            }

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}