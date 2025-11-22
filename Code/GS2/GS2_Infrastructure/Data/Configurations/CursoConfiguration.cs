using GS2_Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GS2_Infrastructure.Data.Configurations
{
    public class CursoConfiguration : IEntityTypeConfiguration<Curso>
    {
        public void Configure(EntityTypeBuilder<Curso> builder)
        {
            builder.ToTable("cursos");

            builder.HasKey(c => c.CursoId);

            builder.Property(c => c.CursoId)
                .HasColumnName("curso_id");

            builder.Property(c => c.Titulo)
                .HasColumnName("titulo")
                .IsRequired()
                .HasMaxLength(130);

            builder.Property(c => c.Descricao)
                .HasColumnName("descricao")
                .HasMaxLength(255);

            builder.Property(c => c.CargaHorariaHrs)
                .HasColumnName("carga_horaria_hrs")
                .IsRequired();

            builder.Property(c => c.NivelId)
                .HasColumnName("nivel_id")
                .IsRequired();

            builder.Property(c => c.ProprietarioId)
                .HasColumnName("proprietario_id")
                .IsRequired();

            builder.Property(c => c.TrilhaAprendizagemId)
                .HasColumnName("trilha_aprendizagem_id");

            // FK: Curso → Nivel -ok
            builder.HasOne(c => c.Nivel)
                .WithMany(n => n.Cursos)
                .HasForeignKey(c => c.NivelId)
                .HasConstraintName("fk_cursos_nivel");

            // FK: Curso → Usuario (proprietário) -ok
            builder.HasOne(c => c.Proprietario)
                .WithMany(u => u.CursosCriados)
                .HasForeignKey(c => c.ProprietarioId)
                .HasConstraintName("fk_cursos_proprietario");

            // FK: Curso → Trilha - ok
            builder.HasOne(c => c.TrilhaAprendizagem)
                .WithMany(t => t.Cursos)
                .HasForeignKey(c => c.TrilhaAprendizagemId)
                .HasConstraintName("fk_cursos_trilha");
            // Relacionamentos 1:N
            builder.HasMany(c => c.Conteudos)
                .WithOne(c => c.Curso)
                .HasForeignKey(c => c.CursoId);

            builder.HasMany(c => c.MeusCursos)
                .WithOne(c => c.Curso)
                .HasForeignKey(c => c.CursoId);

            builder.HasMany(c => c.CursosPalavrasChave)
                .WithOne(c => c.Curso)
                .HasForeignKey(c => c.CursoId);
        }
    }
}
