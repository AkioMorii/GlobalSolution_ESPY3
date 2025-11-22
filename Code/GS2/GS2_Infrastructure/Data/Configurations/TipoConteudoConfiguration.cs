using GS2_Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GS2_Infrastructure.Data.Configurations
{
    public class TipoConteudoConfiguration : IEntityTypeConfiguration<TipoConteudo>
    {
        public void Configure(EntityTypeBuilder<TipoConteudo> builder)
        {
            builder.ToTable("tipo_conteudo");

            builder.HasKey(t => t.TipoConteudoId);

            builder.Property(t => t.TipoConteudoId)
                .HasColumnName("tipo_conteudo_id");

            builder.Property(t => t.Descricao)
                .HasColumnName("descricao")
                .IsRequired()
                .HasMaxLength(255);
            // Seeds
            builder.HasData(
                new { TipoConteudoId=1,Descricao = "Textos e Documentos" },
                new { TipoConteudoId=2,Descricao = "Videoaulas" },
                new { TipoConteudoId=3,Descricao = "Áudios e Podcasts" }
            );
        }
    }

}
