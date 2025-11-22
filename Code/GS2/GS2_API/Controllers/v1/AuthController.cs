using AutoMapper;
using GS2_API.Auth;
using GS2_API.DTOs.v1;
using GS2_API.Services.v1;
using GS2_Domain.Entities;
using GS2_Domain.Exceptions.UserAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GS2_API.Controllers.v1
{
    [AllowAnonymous]
    [Route("api/login")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly LoginService _loginService;
        private readonly TokenService _tokenService;
        private readonly RefreshTokenService _refreshService;
        private readonly IMapper _mapper;

        public AuthController(
            LoginService loginService,
            TokenService tokenService,
            RefreshTokenService refreshService,
            IMapper mapper)
        {
            _loginService = loginService;
            _tokenService = tokenService;
            _refreshService = refreshService;
            _mapper = mapper;
        }

        // LOGIN
        
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginDto request)
        {
            if (request == null)
                return BadRequest("Payload inválido.");

            var usuario = await _loginService.AutenticarAsync(request);
            if (usuario == null)
                throw new UsuarioInexistenteException();

            var roleEnum = usuario.TipoUsuario?.TipoUsuarioId switch
            {
                1 => TipoUsuarioEnum.Administrador,
                2 => TipoUsuarioEnum.Cliente,
                3 => TipoUsuarioEnum.Instrutor,
                _ => TipoUsuarioEnum.Cliente
            };

            // gera access token
            var accessToken = _tokenService.GenerateAccessToken(usuario, roleEnum);

            // gera refresh token (raw + entidade)
            var (refreshRaw, refreshEntity) = await _refreshService.GenerateRefreshTokenAsync(usuario.UsuarioId);

            // seta cookie HttpOnly (web)
            Response.Cookies.Append("refresh_token", refreshRaw, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = refreshEntity.ExpiresAt
            });

            // Retorna access token e refresh raw (mobile usa; web ignora)
            var response = new AuthTokenResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshRaw,
                AccessTokenExpiresAt = DateTime.UtcNow.AddMinutes(40),
                RefreshTokenExpiresAt = refreshEntity.ExpiresAt
            };

            return Ok(new
            {
                accessToken = response.AccessToken,
                refreshToken = response.RefreshToken, // mobile pega; web ignora
                usuario = _mapper.Map<UsuarioDto>(usuario)
            });
        }

        // REFRESH: lê cookie ou body
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequestDto dto)
        {
            // 1) tenta cookie
            Request.Cookies.TryGetValue("refresh_token", out var refreshRaw);

            // 2) se não veio cookie, tenta body (mobile)
            if (string.IsNullOrWhiteSpace(refreshRaw))
            {
                if (dto == null || string.IsNullOrWhiteSpace(dto.RefreshToken))
                    return BadRequest("RefreshToken é obrigatório.");

                refreshRaw = dto.RefreshToken;
            }

            var tokenEntity = await _refreshService.ValidateRefreshTokenAsync(refreshRaw);
            if (tokenEntity == null)
                return Unauthorized("RefreshToken inválido ou expirado.");

            var usuario = await _loginService.ObterUsuarioPorId(tokenEntity.UsuarioId);
            if (usuario == null)
                throw new UsuarioInexistenteException();

            var roleEnum = usuario.TipoUsuario?.TipoUsuarioId switch
            {
                1 => TipoUsuarioEnum.Administrador,
                2 => TipoUsuarioEnum.Cliente,
                3 => TipoUsuarioEnum.Instrutor,
                _ => TipoUsuarioEnum.Cliente
            };

            // Gera novo access token
            var novoAccess = _tokenService.GenerateAccessToken(usuario, roleEnum);

            // Rotaciona refresh token: cria novo raw + entidade e revoga o antigo
            var (novoRefreshRaw, novoRefreshEntity) = await _refreshService.GenerateRefreshTokenAsync(usuario.UsuarioId);

            await _refreshService.RevogarAsync(tokenEntity, novoRefreshEntity.TokenHash);

            // Se a requisição veio via cookie (web) → atualiza cookie e retorna só accessToken
            if (Request.Cookies.ContainsKey("refresh_token"))
            {
                Response.Cookies.Append("refresh_token", novoRefreshRaw, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = novoRefreshEntity.ExpiresAt
                });

                return Ok(new { accessToken = novoAccess });
            }

            // Se veio via body/header (mobile) → retornar novo refresh raw para o cliente (ele armazena em Secure Storage)
            return Ok(new
            {
                accessToken = novoAccess,
                refreshToken = novoRefreshRaw
            });
        }

        // LOGOUT: revoga refresh e limpa cookie
        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] RefreshRequestDto dto)
        {
            // tenta cookie
            Request.Cookies.TryGetValue("refresh_token", out var refreshRaw);

            // se não veio cookie, usa body
            if (string.IsNullOrWhiteSpace(refreshRaw))
            {
                if (dto == null || string.IsNullOrWhiteSpace(dto.RefreshToken))
                {
                    // sem token → apenas deletar cookie e OK
                    Response.Cookies.Delete("refresh_token");
                    return Ok();
                }

                refreshRaw = dto.RefreshToken;
            }

            var tokenEntity = await _refreshService.ValidateRefreshTokenAsync(refreshRaw);
            if (tokenEntity == null)
            {
                Response.Cookies.Delete("refresh_token");
                return Ok(); // já inválido
            }

            await _refreshService.RevogarAsync(tokenEntity);

            Response.Cookies.Delete("refresh_token");
            return Ok();
        }
    }
}
