using GS2_Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GS2_Infrastructure.Data.Configurations
{
    public class MeusCursosConfiguration : IEntityTypeConfiguration<MeusCursos>
    {
        public void Configure(EntityTypeBuilder<MeusCursos> builder)
        {
            builder.ToTable("meus_cursos");

            // PK composta
            builder.HasKey(mc => new { mc.UsuarioId, mc.CursoId });

            // Colunas
            builder.Property(mc => mc.UsuarioId)
                .HasColumnName("usuario_id")
                .IsRequired();

            builder.Property(mc => mc.CursoId)
                .HasColumnName("curso_id")
                .IsRequired();

            builder.Property(mc => mc.DataInicio)
                .HasColumnName("data_inicio")
                .IsRequired();

            builder.Property(mc => mc.DataFim)
                .HasColumnName("data_fim");

            builder.Property(mc => mc.Ativo)
                .HasColumnName("ativo")
                .IsRequired();

            // Relacionamentos
            builder.HasOne(mc => mc.Usuario)
                .WithMany(u => u.MeusCursos)
                .HasForeignKey(mc => mc.UsuarioId)
                .HasConstraintName("fk_meuscursos_usuario");

            builder.HasOne(mc => mc.Curso)
                .WithMany(c => c.MeusCursos)
                .HasForeignKey(mc => mc.CursoId)
                .HasConstraintName("fk_meuscursos_curso");
        }
    }
}
