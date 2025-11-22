namespace GS2_Domain.Exceptions.UserAccess
{
    [Serializable]
    public class CredenciaisInvalidasException : DomainException
    {
        public CredenciaisInvalidasException()
            : base("Credenciais inválidas.") { }
    }
}
