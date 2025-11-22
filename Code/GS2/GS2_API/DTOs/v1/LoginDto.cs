namespace GS2_API.DTOs.v1
{
    public class LoginDto
    {
        public string Login { get; set; } = null!;
        public string Senha { get; set; } = null!;
    }
    public class LogoutRequestDto
    {
        public string RefreshToken { get; set; }
    }
}