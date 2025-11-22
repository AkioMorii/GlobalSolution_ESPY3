using GS2_Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GS2_Infrastructure.Data.Configurations
{
    public class NivelConfiguration : IEntityTypeConfiguration<Nivel>
    {
        public void Configure(EntityTypeBuilder<Nivel> builder)
        {
            builder.ToTable("nivel");

            builder.HasKey(n => n.NivelId);

            builder.Property(n => n.NivelId)
                .HasColumnName("nivel_id");

            builder.Property(n => n.Descricao)
                .HasColumnName("nome")
                .IsRequired()
                .HasMaxLength(100);
            builder.HasMany(n => n.Cursos)
               .WithOne(c => c.Nivel)
               .HasForeignKey(c => c.NivelId)
               .HasConstraintName("fk_curso_nivel");
            // Seeds
            builder.HasData(
                new { NivelId=1, Descricao = "Junior" },
                new { NivelId=2, Descricao = "Pleno" },
                new { NivelId=3, Descricao = "Senior" }
            );
        }
    }

}
