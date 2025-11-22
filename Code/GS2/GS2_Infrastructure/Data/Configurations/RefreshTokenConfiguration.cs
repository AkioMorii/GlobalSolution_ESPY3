using GS2_Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GS2_Infrastructure.Data.Configurations
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable("refresh_tokens");

            builder.HasKey(x => x.RefreshTokenId);

            builder.Property(x => x.RefreshTokenId)
                .HasColumnName("refresh_token_id");

            builder.Property(x => x.UsuarioId)
                .HasColumnName("usuario_id")
                .IsRequired();

            builder.Property(x => x.TokenHash)
                .HasColumnName("token_hash")
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(x => x.ExpiresAt)
                .HasColumnName("expires_at")
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            builder.Property(x => x.RevokedAt)
                .HasColumnName("revoked_at");

            builder.Property(x => x.ReplacedByHash)
                .HasColumnName("replaced_by_hash")
                .HasMaxLength(256);

            builder.HasOne<Usuario>()
                .WithMany()
                .HasForeignKey(x => x.UsuarioId)
                .HasConstraintName("fk_refresh_token_usuario");
        }
    }
}
