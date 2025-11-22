using GS2_API.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace GS2_API.Middleware
{
    public class JwtRefreshMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly JwtSettings _jwtSettings;
        private readonly ILogger<JwtRefreshMiddleware> _logger;

        public JwtRefreshMiddleware(
            RequestDelegate next,
            IOptions<JwtSettings> options,
            ILogger<JwtRefreshMiddleware> logger)
        {
            _next = next;
            _jwtSettings = options.Value;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var refreshTokenService =
                context.RequestServices.GetRequiredService<RefreshTokenService>();

            var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();

            if (string.IsNullOrWhiteSpace(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                await _next(context);
                return;
            }

            var accessToken = authHeader.Replace("Bearer ", "").Trim();
            var tokenHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtToken;

            try
            {
                tokenHandler.ValidateToken(accessToken, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = _jwtSettings.Issuer,
                    ValidateAudience = true,
                    ValidAudience = _jwtSettings.Audience,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(_jwtSettings.Secret)),
                    ValidateLifetime = false
                }, out var validatedToken);

                jwtToken = validatedToken as JwtSecurityToken;
                if (jwtToken == null)
                {
                    _logger.LogWarning("Token JWT não pôde ser lido.");
                    await _next(context);
                    return;
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Token JWT inválido: {Message}", ex.Message);
                await _next(context);
                return;
            }

            var expClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp)?.Value;
            if (expClaim == null)
            {
                await _next(context);
                return;
            }

            var expirationUnix = long.Parse(expClaim);
            var expirationDate = DateTimeOffset.FromUnixTimeSeconds(expirationUnix).UtcDateTime;

            if (expirationDate > DateTime.UtcNow)
            {
                await _next(context);
                return;
            }

            // ===============================
            // Token expirado → tentar refresh
            // ===============================

            context.Request.Cookies.TryGetValue("refresh_token", out var refreshTokenRaw);

            if (string.IsNullOrWhiteSpace(refreshTokenRaw))
            {
                refreshTokenRaw = context.Request.Headers["X-Refresh-Token"].FirstOrDefault();
            }

            if (string.IsNullOrWhiteSpace(refreshTokenRaw))
            {
                await _next(context);
                return;
            }

            var oldToken = await refreshTokenService.ValidateRefreshTokenAsync(refreshTokenRaw);

            if (oldToken == null)
            {
                await _next(context);
                return;
            }

            var newToken = await refreshTokenService.RotateRefreshTokenAsync(oldToken);

            context.Response.Cookies.Append("refresh_token", newToken.TokenHash, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = newToken.ExpiresAt
            });

            await _next(context);
        }
    }
}
