using GS2_Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GS2_Infrastructure.Data.Configurations
{
    public class ConteudoConfiguration : IEntityTypeConfiguration<Conteudo>
    {
        public void Configure(EntityTypeBuilder<Conteudo> builder)
        {
            builder.ToTable("conteudo");

            builder.HasKey(c => c.ConteudoId);

            builder.Property(c => c.ConteudoId)
                .HasColumnName("conteudo_id");

            builder.Property(c => c.CursoId)
                .HasColumnName("curso_id")
                .IsRequired();

            builder.Property(c => c.Url)
                .HasColumnName("url")
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(c => c.DataCadastro)
                .HasColumnName("data_cadastro")
                .IsRequired();

            builder.Property(c => c.Ordem)
                .HasColumnName("ordem")
                .IsRequired();

            builder.Property(c => c.TipoConteudoId)
                .HasColumnName("tipo_conteudo_id")
                .IsRequired();

            // FK Conteudo → Curso
            builder.HasOne(c => c.Curso)
                .WithMany(cu => cu.Conteudos)
                .HasForeignKey(c => c.CursoId)
                .HasConstraintName("fk_conteudo_curso");

            // FK Conteudo → TipoConteudo
            builder.HasOne(c => c.TipoConteudo)
                .WithMany(tc => tc.Conteudos)
                .HasForeignKey(c => c.TipoConteudoId)
                .HasConstraintName("fk_conteudo_tipo");
        }
    }

}
