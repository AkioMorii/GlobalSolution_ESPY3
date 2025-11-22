using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using GS2_APP.Models;
using GS2_APP.Services;
namespace GS2_APP.Controllers
{
    public class LoginController : Controller
    {
        private readonly LoginService _loginService;
        public LoginController(LoginService loginService)
        {
            _loginService = loginService;
        }
        // GET
        [HttpGet]
        public IActionResult Index()
        {
            LoginModel model = new LoginModel();
            return View(model);
        }

        

        [HttpPost]
        public async Task<IActionResult> Verificar(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }
            var (usuario,statusCode, erro, accessToken, refreshToken) = await _loginService.Verificar(model);
            if (usuario != null)
            {
                SaveToken(accessToken, refreshToken);
                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, usuario.Login),
                        new Claim("Name", usuario.Nome),

                        new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                        new Claim("Id", usuario.usuarioId.ToString())
                    };
                claims.Add(new Claim(ClaimTypes.Role, usuario.TipoUsuarioDescricao));
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                return RedirectToAction("Index", "Home", usuario);
            }
            TempData["MensagemErro"] = "Usuário ou senha inválidos.";
            return View("Index", model); // Retorna à view Index para exibir a mensagem
           
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            bool resultLogout= await _loginService.Logout(refreshToken);
            if (resultLogout) RemoveToken();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Login");
        }
        #region private
        private void SaveToken(string accessToken, string refreshToken) {
            Response.Cookies.Append("accessToken", accessToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddMinutes(20)
            });

            // Criar cookie seguro para RefreshToken
            Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(7)
            });
        }
        private void RemoveToken() {
            Response.Cookies.Delete("accessToken");
            Response.Cookies.Delete("refreshToken");
        }
        #endregion private
    }
}
