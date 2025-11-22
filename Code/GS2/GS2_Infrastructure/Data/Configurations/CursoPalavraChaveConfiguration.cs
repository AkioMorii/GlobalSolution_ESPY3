using GS2_Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GS2_Infrastructure.Data.Configurations
{
    public class CursoPalavraChaveConfiguration : IEntityTypeConfiguration<CursoPalavraChave>
    {
        public void Configure(EntityTypeBuilder<CursoPalavraChave> builder)
        {
            builder.ToTable("curso_palavra_chave");

            // Chave primária composta
            builder.HasKey(cpk => new { cpk.CursoId, cpk.PalavraChaveId });

            builder.Property(cpk => cpk.CursoId)
                .HasColumnName("curso_id")
                .IsRequired();

            builder.Property(cpk => cpk.PalavraChaveId)
                .HasColumnName("palavra_chave_id")
                .IsRequired();

            // Relacionamentos
            builder.HasOne(cpk => cpk.Curso)
                .WithMany(c => c.CursosPalavrasChave)
                .HasForeignKey(cpk => cpk.CursoId)
                .HasConstraintName("fk_cursopalavra_curso");

            builder.HasOne(cpk => cpk.PalavraChave)
                .WithMany(pc => pc.CursosPalavrasChave)
                .HasForeignKey(cpk => cpk.PalavraChaveId)
                .HasConstraintName("fk_cursopalavra_palavra");
        }
    }
}
