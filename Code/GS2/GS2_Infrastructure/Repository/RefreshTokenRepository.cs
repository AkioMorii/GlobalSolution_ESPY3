using GS2_Domain.Entities;
using GS2_Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GS2_Infrastructure.Repository
{
    public class RefreshTokenRepository
    {
        private readonly AppDbContext _context;

        public RefreshTokenRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(RefreshToken token)
        {
            await _context.RefreshTokens.AddAsync(token);
            await _context.SaveChangesAsync();
        }

        public async Task<RefreshToken?> GetByHashAsync(string tokenHash)
        {
            return await _context.RefreshTokens
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.TokenHash == tokenHash);
        }

        public async Task<List<RefreshToken>> GetByUserIdAsync(int usuarioId)
        {
            return await _context.RefreshTokens
                .Where(t => t.UsuarioId == usuarioId)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task RevokeAsync(RefreshToken token, string? replacedBy = null)
        {
            token.Revoke(replacedBy);
            _context.RefreshTokens.Update(token);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(RefreshToken token)
        {
            _context.RefreshTokens.Update(token);
            await _context.SaveChangesAsync();
        }

        public async Task RevokeAllForUserAsync(int userId)
        {
            var tokens = await _context.RefreshTokens
                .Where(t => t.UsuarioId == userId && t.RevokedAt == null)
                .ToListAsync();

            foreach (var t in tokens)
                t.Revoke();

            _context.RefreshTokens.UpdateRange(tokens);
            await _context.SaveChangesAsync();
        }
    }
}
