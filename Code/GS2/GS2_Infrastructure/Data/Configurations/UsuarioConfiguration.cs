using GS2_Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GS2_Infrastructure.Data.Configurations
{
    public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("usuarios");

            builder.HasKey(u => u.UsuarioId);

            builder.Property(u => u.UsuarioId)
                .HasColumnName("usuario_id");

            builder.Property(u => u.Nome)
                .HasColumnName("nome")
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(u => u.Email)
                .HasColumnName("email")
                .HasMaxLength(345);

            builder.Property(u => u.SenhaHash)
                .HasColumnName("senha")
                .HasMaxLength(350)
                .IsRequired();

            builder.Property(u => u.Telefone)
                .HasColumnName("telefone")
                .HasMaxLength(20)
                .IsRequired();
            builder.Property(u => u.Cpf)
               .HasColumnName("cpf")
               .HasMaxLength(20)
               .IsRequired();

            builder.Property(u => u.Login)
                .HasColumnName("login")
                .HasMaxLength(30)
                .IsRequired();

            builder.Property(u => u.DataCadastro)
                .HasColumnName("data_cadastro")
                .IsRequired();

            builder.Property(u => u.DataRedefinirSenha)
                .HasColumnName("data_redefinir_senha");

            builder.Property(u => u.Ativo)
                .HasColumnName("ativo")
                .IsRequired();

            builder.Property(u => u.TipoUsuarioId)
                .HasColumnName("tipo_usuario_id")
                .IsRequired();

            // FK: Usuario → TipoUsuario
            builder.HasOne(u => u.TipoUsuario)
                .WithMany(tu => tu.Usuarios)
                .HasForeignKey(u => u.TipoUsuarioId)
                .HasConstraintName("fk_usuarios_tipo_usuario");
            builder.HasMany<RefreshToken>()
            .WithOne()
            .HasForeignKey(rt => rt.UsuarioId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("fk_usuarios_refresh_tokens");

            // UNIQUE Indexes
            builder.HasIndex(u => u.Email)
                .IsUnique()
                .HasDatabaseName("uq_usuarios_email");

            builder.HasIndex(u => u.Login)
                .IsUnique()
                .HasDatabaseName("uq_usuarios_login");
            /*seed para usuário administrador.
             * SENHA : Adm123456!
             * USUARIO: "administrador"
            */
            builder.HasData(
                new {
                    UsuarioId=1,
                    Nome="ADM",
                    Email= "adm@adm.com",
                    Login= "administrador",
                    SenhaHash= "$2a$11$kfA7gVkje5eIJ5X0QlmUnOn9p24J2H3H16LLZGOx.t4vMyUStJmIu",
                    Telefone= "(11) 11111-1111",
                    Cpf= "123.453.456-78",
                    TipoUsuarioId=1,
                    Ativo=true,
                    DataCadastro = new DateTime(2025, 11, 21, 0, 0, 0)
                }
            );
        }
    }

}
