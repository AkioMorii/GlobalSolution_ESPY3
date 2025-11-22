using GS2_API.DTOs.v1;
using GS2_Domain.Entities;
using GS2_Domain.Exceptions.UserAccess;
using GS2_Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;

namespace GS2_API.Services.v1
{
    public class LoginService
    {
        private readonly UsuarioRepository _repo;

        public LoginService(UsuarioRepository repo)
        {
            _repo = repo;
        }

        // ============================================================
        // LOGIN NORMAL
        // ============================================================
        public async Task<Usuario> AutenticarAsync(LoginDto request)
        {
            //var usuario = await _repo.ValidarLoginAsync(request.Login, senhaHash);
            var usuario=await _repo.ObterPorLoginAsync(request.Login);

            if (usuario == null)
                throw new UsuarioInexistenteException(request.Login);
            if (!BCrypt.Net.BCrypt.Verify(request.Senha, usuario.SenhaHash))
                throw new CredenciaisInvalidasException();
            if (!usuario.Ativo)
                throw new UsuarioBloqueadoException();
            return usuario;
        }

        // ============================================================
        // USADO PELO REFRESH TOKEN
        // ============================================================
        public async Task<Usuario?> ObterUsuarioPorId(int usuarioId)
        {
            return await _repo.ObterPorIdAsync(usuarioId);
        }
        private string HashPasswordBCrypt(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
    }
}
