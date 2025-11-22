namespace GS2_Domain.Exceptions.UserAccess
{
    [Serializable]
    public class UsuarioNaoElegivelParaTrilhaException : DomainException
    {
        public UsuarioNaoElegivelParaTrilhaException(string identificador)
            : base($"Usuário inexistente: {identificador}") { }
    }
}
