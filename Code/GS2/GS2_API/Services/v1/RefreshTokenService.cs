using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using GS2_Domain.Entities;
using GS2_Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace GS2_API.Auth
{
    public class RefreshTokenService
    {
        private readonly AppDbContext _context;
        private readonly JwtSettings _jwtSettings;

        public RefreshTokenService(AppDbContext context, IOptions<JwtSettings> options)
        {
            _context = context;
            _jwtSettings = options.Value;
        }

        // Gera token raw e salva hash no banco; retorna o token raw
        public async Task<(string tokenRaw, RefreshToken refreshToken)> GenerateRefreshTokenAsync(int usuarioId)
        {
            if (usuarioId <= 0) throw new ArgumentException("ID de usuário inválido.", nameof(usuarioId));

            var tokenRaw = GenerateRandomToken();
            var tokenHash = ComputeHash(tokenRaw);

            var expiresAt = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays);
            var refreshToken = new RefreshToken(usuarioId, tokenHash, expiresAt);

            _context.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync();

            return (tokenRaw, refreshToken);
        }

        public async Task<RefreshToken?> ValidateRefreshTokenAsync(string token)
        {
            if (string.IsNullOrWhiteSpace(token)) return null;

            var hash = ComputeHash(token);
            var refreshToken = await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.TokenHash == hash);

            if (refreshToken == null || !refreshToken.IsActive())
                return null;

            return refreshToken;
        }

        public async Task<RefreshToken> RotateRefreshTokenAsync(RefreshToken oldToken)
        {
            if (oldToken == null) throw new ArgumentNullException(nameof(oldToken));

            oldToken.Revoke();

            var newTokenRaw = GenerateRandomToken();
            var newTokenHash = ComputeHash(newTokenRaw);
            var newExpiresAt = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays);

            var newToken = new RefreshToken(oldToken.UsuarioId, newTokenHash, newExpiresAt);

            _context.RefreshTokens.Update(oldToken);
            _context.RefreshTokens.Add(newToken);
            await _context.SaveChangesAsync();
            return newToken;
        }

        private string GenerateRandomToken()
        {
            var bytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }

        private string ComputeHash(string token)
        {
            using var sha = System.Security.Cryptography.SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(token);
            var hashBytes = sha.ComputeHash(bytes);
            return Convert.ToBase64String(hashBytes);
        }

        public async Task<(string tokenRaw, RefreshToken refreshToken)> GerarAsync(int usuarioId, TimeSpan? expiration = null)
        {
            return await GenerateRefreshTokenAsync(usuarioId);
        }

        public async Task<RefreshToken?> ValidarAsync(string tokenRaw)
        {
            return await ValidateRefreshTokenAsync(tokenRaw);
        }

        public async Task RevogarAsync(RefreshToken token, string? replacedByHash = null)
        {
            token.Revoke(replacedByHash);
            _context.RefreshTokens.Update(token);
            await _context.SaveChangesAsync();
        }
    }
}