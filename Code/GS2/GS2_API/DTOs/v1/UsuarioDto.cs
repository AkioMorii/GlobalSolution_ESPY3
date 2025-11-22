namespace GS2_API.DTOs.v1
{
    public class UsuarioDto
    {
        public int UsuarioId { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public string Telefone { get; set; }
        public string Cpf { get; private set; }
        public bool Ativo { get; set; }
        public DateTime DataCadastro { get; set; }

        public int TipoUsuarioId { get; set; }
        public string TipoUsuarioDescricao { get; set; }

        // Cursos criados pelo usuário (somente para admins)
        public List<CursoResumoDto> CursosCriados { get; set; }

        // Cursos que o usuário possui (para acesso ao conteúdo)
        public List<MeusCursosDto> MeusCursos { get; set; }
    }


    public class UsuarioCreateDto
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }
        public string Telefone { get; set; }
        public string Cpf { get; set; }
        public int TipoUsuarioId { get; set; }
    }
    public class UsuarioUpdateDto
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string Cpf { get; set; }
        public int TipoUsuarioId { get; set; }
        public bool Ativo { get; set; }
    }

    // DTO para login e retorno de payload completo
    public class UsuarioResponseDto
    {
        public int UsuarioId { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public bool Ativo { get; set; }

        public int TipoUsuarioId { get; set; }
        public string TipoUsuarioDescricao { get; set; }

        public List<MeusCursosDto> MeusCursos { get; set; }
    }
    public class UsuarioAlterarSenhaDto
    {
        public string SenhaAtual { get; set; }
        public string NovaSenha { get; set; }
    }

    public class CursoResumoDto
    {
        public int CursoId { get; set; }
        public string Nome { get; set; }
    }
}
