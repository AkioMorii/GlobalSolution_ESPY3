namespace GS2_Domain.Exceptions.UserAccess
{
    [Serializable]
    public class UsuarioInexistenteException : DomainException
    {
        public UsuarioInexistenteException() : base($"Usuário inexistente") { }
        public UsuarioInexistenteException(string identificador)
            : base($"Usuário inexistente: {identificador}") { }
    }
}
