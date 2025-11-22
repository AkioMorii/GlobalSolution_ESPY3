using GS2_Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GS2_Infrastructure.Data.Configurations
{
    public class TrilhaAprendizagemConfiguration : IEntityTypeConfiguration<TrilhaAprendizagem>
    {
        public void Configure(EntityTypeBuilder<TrilhaAprendizagem> builder)
        {
            builder.ToTable("trilha_aprendizagem");

            builder.HasKey(t => t.TrilhaAprendizagemId);

            builder.Property(t => t.TrilhaAprendizagemId)
                .HasColumnName("trilha_aprendizagem_id");

            builder.Property(t => t.Titulo)
                .HasColumnName("nome")
                .IsRequired()
                .HasMaxLength(255);
            builder.Property(t => t.Descricao)
                   .HasColumnName("descricao")
                   .HasMaxLength(255);

            builder.Property(t => t.CargaHoraria)
                   .HasColumnName("carga_horaria_hrs")
                   .IsRequired();

            builder.HasMany(t => t.Cursos)
                   .WithOne(c => c.TrilhaAprendizagem)
                   .HasForeignKey(c => c.TrilhaAprendizagemId)
                   .HasConstraintName("fk_cursos_trilha");
            //seeds:
            builder.HasData(

                new
                {
                    TrilhaAprendizagemId = 1,
                    Titulo = "Introdução à Programação",
                    Descricao = "Trilha voltada para iniciantes aprenderem lógica, algoritmos e fundamentos da programação.",
                    CargaHoraria = 40
                },
                new
                {
                    TrilhaAprendizagemId = 2,
                    Titulo = "Desenvolvimento Web Front-end",
                    Descricao = "Aprenda HTML, CSS, JavaScript e frameworks modernos para criação de interfaces.",
                    CargaHoraria = 60
                },
                new
                {
                    TrilhaAprendizagemId = 3,
                    Titulo = "Data Science para Iniciantes",
                    Descricao = "Fundamentos de análise de dados, estatística básica e uso de Python em data science.",
                    CargaHoraria = 80
                },
                new
                {
                    TrilhaAprendizagemId = 4,
                    Titulo = "Gestão de Projetos com Metodologias Ágeis",
                    Descricao = "Conceitos de Scrum, Kanban, ciclos ágeis e práticas de gestão moderna.",
                    CargaHoraria = 30
                },
                new
                {
                    TrilhaAprendizagemId = 5,
                    Titulo = "Marketing Digital",
                    Descricao = "Especialização em redes sociais, tráfego pago, SEO e estratégias de conteúdo.",
                    CargaHoraria = 50
                },
                new
                {
                    TrilhaAprendizagemId = 6,
                    Titulo = "Cibersegurança Essencial",
                    Descricao = "Princípios de segurança digital, prevenção de ataques e boas práticas de proteção.",
                    CargaHoraria = 45
                },
                new
                {
                    TrilhaAprendizagemId = 7,
                    Titulo = "Soft Skills para o Mercado de Trabalho",
                    Descricao = "Desenvolvimento de habilidades comportamentais como comunicação, trabalho em equipe e liderança.",
                    CargaHoraria = 25
                },
                new
                {
                    TrilhaAprendizagemId = 8,
                    Titulo = "Inglês para Negócios",
                    Descricao = "Foco em vocabulário corporativo, reuniões, apresentações e comunicação profissional.",
                    CargaHoraria = 60
                }
            );
        }
    }
}
