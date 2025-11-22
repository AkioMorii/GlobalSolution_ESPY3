using GS2_APP.Models;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GS2_APP.Services
{
    public class LoginService
    {
        private readonly ApiClientService _apiClientService;
        private const string ENDPOINT = "login";
        public LoginService(ApiClientService apiClientService)
        {
            _apiClientService= apiClientService;
        }
        public async Task<(UsuarioModel?,int statusCode,string? errorMessage,string? accessToken, string? refreshToken)> Verificar(LoginModel model) {
            var json = JsonSerializer.Serialize(model);
            // await _apiClientService.PostAsync2(ENDPOINT, json);
            var response = await _apiClientService.PostAsync<LoginResponseDto>(ENDPOINT, model);
            if (response.Success && response.Data != null)
            {
                UsuarioModel usuario = response.Data.Usuario;
                return (usuario,200, null, response.Data.AccessToken, response.Data.RefreshToken);
            }
            else {
                return (null,response.StatusCode, response.Error ?? "Erro desconhecido no login",null,null);
            }
        }
        public async Task<bool> Logout(string refreshToken) {
            var json = new { refreshToken };
            var response = await _apiClientService.PostAsync(ENDPOINT + "/logout", json);
            return  response.Success;
        }
    }
}
