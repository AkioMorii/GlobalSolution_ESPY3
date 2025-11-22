namespace GS2_APP.Models
{
    public class ApiClientSettings
    {
        public string BaseUrl { get; set; } = string.Empty;
        public string ApiVersion { get; set; } = "1";
        public string accessToken { get; set; }
    }
}
