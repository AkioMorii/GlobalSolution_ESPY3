using GS2_Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GS2_Infrastructure.Data.Configurations
{
    public class TipoUsuarioConfiguration : IEntityTypeConfiguration<TipoUsuario>
    {
        public void Configure(EntityTypeBuilder<TipoUsuario> builder)
        {
            builder.ToTable("tipo_usuario");

            builder.HasKey(tu => tu.TipoUsuarioId);

            builder.Property(tu => tu.TipoUsuarioId)
                .HasColumnName("tipo_usuario_id");

            builder.Property(tu => tu.Descricao)
                .HasColumnName("cargo")
                .HasMaxLength(255)
                .IsRequired();
            // Seeds
            builder.HasData(
                new { TipoUsuarioId = 1, Descricao = "Administrador" },
                new { TipoUsuarioId = 2, Descricao = "Cliente" },
                new { TipoUsuarioId = 3, Descricao = "Instrutor" }
            );
        }
    }

}
