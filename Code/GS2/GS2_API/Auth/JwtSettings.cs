namespace GS2_API.Auth
{
    public class JwtSettings
    {
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public string Secret { get; set; } = string.Empty;
        public int AccessTokenExpirationMinutes { get; set; } = 40;
        public int RefreshTokenExpirationDays { get; set; } = 5;
    }
}
