using GS2_Domain.Entities;
using GS2_Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GS2_Infrastructure.Repository
{
    public class UsuarioRepository
    {
        private readonly AppDbContext _context;

        public UsuarioRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Usuario?> ValidarLoginAsync(string login, string senhaHash)
        {
            return await _context.Usuarios
                .Include(u => u.TipoUsuario)
                .FirstOrDefaultAsync(u => u.Login == login && u.SenhaHash == senhaHash);
        }
        public async Task<Usuario?> ObterPorIdAsync(int usuarioId)
        {
            return await _context.Usuarios
                .Include(u => u.TipoUsuario)
                .FirstOrDefaultAsync(u => u.UsuarioId == usuarioId);
        }
        public async Task<Usuario?> ObterPorLoginAsync(string login)
        {
            return await _context.Usuarios
                .Include(x => x.TipoUsuario)
                .Include(x => x.MeusCursos)
                    .ThenInclude(mc => mc.Curso)
                .FirstOrDefaultAsync(x => x.Login == login);
        }
    }
}
