using GS2_Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GS2_Infrastructure.Data.Configurations
{
    public class PalavraChaveConfiguration : IEntityTypeConfiguration<PalavrasChaves>
    {
        public void Configure(EntityTypeBuilder<PalavrasChaves> builder)
        {
            builder.ToTable("palavras_chaves");

            builder.HasKey(p => p.PalavraChaveId);

            builder.Property(p => p.PalavraChaveId)
                .HasColumnName("palavra_chave_id");

            builder.Property(p => p.Descricao)
                .HasColumnName("descricao")
                .IsRequired()
                .HasMaxLength(255);
            builder.HasMany(p => p.CursosPalavrasChave)
               .WithOne(cp => cp.PalavraChave)
               .HasForeignKey(cp => cp.PalavraChaveId)
               .HasConstraintName("fk_cursopalavra_palavra");
        }
    }

}
