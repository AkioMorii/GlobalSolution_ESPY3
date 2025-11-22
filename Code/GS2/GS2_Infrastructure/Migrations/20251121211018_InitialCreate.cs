using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GS2_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "nivel",
                columns: table => new
                {
                    nivel_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_nivel", x => x.nivel_id);
                });

            migrationBuilder.CreateTable(
                name: "palavras_chaves",
                columns: table => new
                {
                    palavra_chave_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    descricao = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_palavras_chaves", x => x.palavra_chave_id);
                });

            migrationBuilder.CreateTable(
                name: "tipo_conteudo",
                columns: table => new
                {
                    tipo_conteudo_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    descricao = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tipo_conteudo", x => x.tipo_conteudo_id);
                });

            migrationBuilder.CreateTable(
                name: "tipo_usuario",
                columns: table => new
                {
                    tipo_usuario_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    cargo = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tipo_usuario", x => x.tipo_usuario_id);
                });

            migrationBuilder.CreateTable(
                name: "trilha_aprendizagem",
                columns: table => new
                {
                    trilha_aprendizagem_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nome = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    descricao = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    carga_horaria_hrs = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_trilha_aprendizagem", x => x.trilha_aprendizagem_id);
                });

            migrationBuilder.CreateTable(
                name: "usuarios",
                columns: table => new
                {
                    usuario_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nome = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    email = table.Column<string>(type: "character varying(345)", maxLength: 345, nullable: false),
                    login = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    senha = table.Column<string>(type: "character varying(350)", maxLength: 350, nullable: false),
                    telefone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    cpf = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    data_cadastro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    data_redefinir_senha = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ativo = table.Column<bool>(type: "boolean", nullable: false),
                    tipo_usuario_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usuarios", x => x.usuario_id);
                    table.ForeignKey(
                        name: "fk_usuarios_tipo_usuario",
                        column: x => x.tipo_usuario_id,
                        principalTable: "tipo_usuario",
                        principalColumn: "tipo_usuario_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "cursos",
                columns: table => new
                {
                    curso_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    titulo = table.Column<string>(type: "character varying(130)", maxLength: 130, nullable: false),
                    descricao = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    carga_horaria_hrs = table.Column<int>(type: "integer", nullable: false),
                    nivel_id = table.Column<int>(type: "integer", nullable: false),
                    proprietario_id = table.Column<int>(type: "integer", nullable: false),
                    trilha_aprendizagem_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cursos", x => x.curso_id);
                    table.ForeignKey(
                        name: "fk_curso_nivel",
                        column: x => x.nivel_id,
                        principalTable: "nivel",
                        principalColumn: "nivel_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_cursos_proprietario",
                        column: x => x.proprietario_id,
                        principalTable: "usuarios",
                        principalColumn: "usuario_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_cursos_trilha",
                        column: x => x.trilha_aprendizagem_id,
                        principalTable: "trilha_aprendizagem",
                        principalColumn: "trilha_aprendizagem_id");
                });

            migrationBuilder.CreateTable(
                name: "refresh_tokens",
                columns: table => new
                {
                    refresh_token_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    usuario_id = table.Column<int>(type: "integer", nullable: false),
                    token_hash = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    expires_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    revoked_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    replaced_by_hash = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_refresh_tokens", x => x.refresh_token_id);
                    table.ForeignKey(
                        name: "fk_usuarios_refresh_tokens",
                        column: x => x.usuario_id,
                        principalTable: "usuarios",
                        principalColumn: "usuario_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "conteudo",
                columns: table => new
                {
                    conteudo_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    curso_id = table.Column<int>(type: "integer", nullable: false),
                    url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    data_cadastro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ordem = table.Column<int>(type: "integer", nullable: false),
                    tipo_conteudo_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_conteudo", x => x.conteudo_id);
                    table.ForeignKey(
                        name: "fk_conteudo_curso",
                        column: x => x.curso_id,
                        principalTable: "cursos",
                        principalColumn: "curso_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_conteudo_tipo",
                        column: x => x.tipo_conteudo_id,
                        principalTable: "tipo_conteudo",
                        principalColumn: "tipo_conteudo_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "curso_palavra_chave",
                columns: table => new
                {
                    curso_id = table.Column<int>(type: "integer", nullable: false),
                    palavra_chave_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_curso_palavra_chave", x => new { x.curso_id, x.palavra_chave_id });
                    table.ForeignKey(
                        name: "fk_cursopalavra_curso",
                        column: x => x.curso_id,
                        principalTable: "cursos",
                        principalColumn: "curso_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_cursopalavra_palavra",
                        column: x => x.palavra_chave_id,
                        principalTable: "palavras_chaves",
                        principalColumn: "palavra_chave_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "meus_cursos",
                columns: table => new
                {
                    usuario_id = table.Column<int>(type: "integer", nullable: false),
                    curso_id = table.Column<int>(type: "integer", nullable: false),
                    data_inicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    data_fim = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ativo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_meus_cursos", x => new { x.usuario_id, x.curso_id });
                    table.ForeignKey(
                        name: "fk_meuscursos_curso",
                        column: x => x.curso_id,
                        principalTable: "cursos",
                        principalColumn: "curso_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_meuscursos_usuario",
                        column: x => x.usuario_id,
                        principalTable: "usuarios",
                        principalColumn: "usuario_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "nivel",
                columns: new[] { "nivel_id", "nome" },
                values: new object[,]
                {
                    { 1, "Junior" },
                    { 2, "Pleno" },
                    { 3, "Senior" }
                });

            migrationBuilder.InsertData(
                table: "tipo_conteudo",
                columns: new[] { "tipo_conteudo_id", "descricao" },
                values: new object[,]
                {
                    { 1, "Textos e Documentos" },
                    { 2, "Videoaulas" },
                    { 3, "Áudios e Podcasts" }
                });

            migrationBuilder.InsertData(
                table: "tipo_usuario",
                columns: new[] { "tipo_usuario_id", "cargo" },
                values: new object[,]
                {
                    { 1, "Administrador" },
                    { 2, "Cliente" },
                    { 3, "Instrutor" }
                });

            migrationBuilder.InsertData(
                table: "trilha_aprendizagem",
                columns: new[] { "trilha_aprendizagem_id", "carga_horaria_hrs", "descricao", "nome" },
                values: new object[,]
                {
                    { 1, 40, "Trilha voltada para iniciantes aprenderem lógica, algoritmos e fundamentos da programação.", "Introdução à Programação" },
                    { 2, 60, "Aprenda HTML, CSS, JavaScript e frameworks modernos para criação de interfaces.", "Desenvolvimento Web Front-end" },
                    { 3, 80, "Fundamentos de análise de dados, estatística básica e uso de Python em data science.", "Data Science para Iniciantes" },
                    { 4, 30, "Conceitos de Scrum, Kanban, ciclos ágeis e práticas de gestão moderna.", "Gestão de Projetos com Metodologias Ágeis" },
                    { 5, 50, "Especialização em redes sociais, tráfego pago, SEO e estratégias de conteúdo.", "Marketing Digital" },
                    { 6, 45, "Princípios de segurança digital, prevenção de ataques e boas práticas de proteção.", "Cibersegurança Essencial" },
                    { 7, 25, "Desenvolvimento de habilidades comportamentais como comunicação, trabalho em equipe e liderança.", "Soft Skills para o Mercado de Trabalho" },
                    { 8, 60, "Foco em vocabulário corporativo, reuniões, apresentações e comunicação profissional.", "Inglês para Negócios" }
                });

            migrationBuilder.InsertData(
                table: "usuarios",
                columns: new[] { "usuario_id", "ativo", "cpf", "data_cadastro", "data_redefinir_senha", "email", "login", "nome", "senha", "telefone", "tipo_usuario_id" },
                values: new object[] { 1, true, "123.453.456-78", new DateTime(2025, 11, 21, 3, 0, 0, 0, DateTimeKind.Utc), null, "adm@adm.com", "administrador", "ADM", "$2a$11$kfA7gVkje5eIJ5X0QlmUnOn9p24J2H3H16LLZGOx.t4vMyUStJmIu", "(11) 11111-1111", 1 });

            migrationBuilder.CreateIndex(
                name: "IX_conteudo_curso_id",
                table: "conteudo",
                column: "curso_id");

            migrationBuilder.CreateIndex(
                name: "IX_conteudo_tipo_conteudo_id",
                table: "conteudo",
                column: "tipo_conteudo_id");

            migrationBuilder.CreateIndex(
                name: "IX_curso_palavra_chave_palavra_chave_id",
                table: "curso_palavra_chave",
                column: "palavra_chave_id");

            migrationBuilder.CreateIndex(
                name: "IX_cursos_nivel_id",
                table: "cursos",
                column: "nivel_id");

            migrationBuilder.CreateIndex(
                name: "IX_cursos_proprietario_id",
                table: "cursos",
                column: "proprietario_id");

            migrationBuilder.CreateIndex(
                name: "IX_cursos_trilha_aprendizagem_id",
                table: "cursos",
                column: "trilha_aprendizagem_id");

            migrationBuilder.CreateIndex(
                name: "IX_meus_cursos_curso_id",
                table: "meus_cursos",
                column: "curso_id");

            migrationBuilder.CreateIndex(
                name: "IX_refresh_tokens_usuario_id",
                table: "refresh_tokens",
                column: "usuario_id");

            migrationBuilder.CreateIndex(
                name: "IX_usuarios_tipo_usuario_id",
                table: "usuarios",
                column: "tipo_usuario_id");

            migrationBuilder.CreateIndex(
                name: "uq_usuarios_email",
                table: "usuarios",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "uq_usuarios_login",
                table: "usuarios",
                column: "login",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "conteudo");

            migrationBuilder.DropTable(
                name: "curso_palavra_chave");

            migrationBuilder.DropTable(
                name: "meus_cursos");

            migrationBuilder.DropTable(
                name: "refresh_tokens");

            migrationBuilder.DropTable(
                name: "tipo_conteudo");

            migrationBuilder.DropTable(
                name: "palavras_chaves");

            migrationBuilder.DropTable(
                name: "cursos");

            migrationBuilder.DropTable(
                name: "nivel");

            migrationBuilder.DropTable(
                name: "usuarios");

            migrationBuilder.DropTable(
                name: "trilha_aprendizagem");

            migrationBuilder.DropTable(
                name: "tipo_usuario");
        }
    }
}
