namespace GS2_API.DTOs.v1
{
    public class AuthTokenResponseDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime AccessTokenExpiresAt { get; set; }
        public DateTime RefreshTokenExpiresAt { get; set; }
    }
    public class RefreshRequestDto
    {
        public string RefreshToken { get; set; }
    }
}
