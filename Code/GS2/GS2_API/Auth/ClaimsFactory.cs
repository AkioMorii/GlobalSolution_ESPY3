using System.Security.Claims;
using GS2_Domain.Entities;

namespace GS2_API.Auth
{
    public static class ClaimsFactory
    {
        public static List<Claim> Create(Usuario user, TipoUsuarioEnum role)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user), "Usuário não pode ser nulo.");

            // role é enum mas está coerente com os ids da base
            return new List<Claim>
            {
                new Claim("sub", user.UsuarioId.ToString()),
                new Claim("name", user.Nome ?? string.Empty),
                new Claim("email", user.Email ?? string.Empty),
                new Claim(ClaimTypes.Role, role.ToString()),
            };
        }
    }
}
