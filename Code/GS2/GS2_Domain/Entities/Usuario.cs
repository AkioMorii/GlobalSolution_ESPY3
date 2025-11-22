namespace GS2_Domain.Entities
{

    public class Usuario
    {
        public int UsuarioId { get; private set; }
        public string Nome { get; private set; }
        public string Email { get; private set; }
        public string Login { get; private set; }
        public string SenhaHash { get; private set; }
        public string Telefone { get; private set; }
        public string Cpf { get; private set; }
        public DateTime DataCadastro { get; private set; }
        public DateTime? DataRedefinirSenha { get; private set; }
        public bool Ativo { get; private set; }

        public int TipoUsuarioId { get; private set; }
        public TipoUsuario TipoUsuario { get; private set; }
        public ICollection<MeusCursos> MeusCursos { get; set; }
        public ICollection<Curso> CursosCriados { get; private set; }
        protected Usuario()
        {
            MeusCursos = new List<MeusCursos>();
            CursosCriados = new List<Curso>();
        }

        public Usuario(string nome, string email, string login, string senhaHash, string telefone,string cpf, int tipoUsuarioId)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("Nome é obrigatório.");

            if (string.IsNullOrWhiteSpace(login))
                throw new ArgumentException("Login é obrigatório.");

            if (string.IsNullOrWhiteSpace(senhaHash))
                throw new ArgumentException("Senha é obrigatória.");

            if (string.IsNullOrWhiteSpace(telefone))
                throw new ArgumentException("Telefone é obrigatório.");
            Nome = nome;
            Email = email.Trim().ToLowerInvariant();
            Login = login;
            SenhaHash = senhaHash;
            Telefone = telefone.Trim();
            Cpf = cpf.Trim();
            DataCadastro = DateTime.UtcNow;
            TipoUsuarioId = tipoUsuarioId;
            Ativo = true;
            MeusCursos = new List<MeusCursos>();
        }

        public void DefinirNovaSenha(string novaSenhaHash)
        {
            SenhaHash = novaSenhaHash;
            DataRedefinirSenha = DateTime.UtcNow;
        }
        public void AtualizarDados(string nome, string email, string telefone, int tipoUsuarioId, bool ativo)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("Nome é obrigatório.");

            if (string.IsNullOrWhiteSpace(telefone))
                throw new ArgumentException("Telefone é obrigatório.");

            Nome = nome;
            Email = email?.Trim().ToLowerInvariant();
            Telefone = telefone.Trim();
            TipoUsuarioId = tipoUsuarioId;
            Ativo = ativo;
        }

        public void Desativar() => Ativo = false;
        public void Ativar() => Ativo = true;
    }
}
