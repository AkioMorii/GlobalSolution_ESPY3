namespace GS2_Domain.Exceptions.UserAccess
{
    [Serializable]
    public class UsuarioBloqueadoException : DomainException
    {
        public UsuarioBloqueadoException()
            : base("Usuário encontra-se bloqueado.") { }
    }
}
