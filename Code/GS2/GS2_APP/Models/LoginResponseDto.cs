namespace GS2_APP.Models
{
    public class LoginResponseDto
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public UsuarioModel Usuario { get; set; } = new();
    }

}
