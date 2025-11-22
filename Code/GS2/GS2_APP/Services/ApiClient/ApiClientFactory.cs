using GS2_APP.Models;
using Microsoft.Extensions.Options;

public class ApiClientFactory
{
    private readonly IHttpClientFactory _factory;
    private readonly IHttpContextAccessor _accessor;
    private readonly ApiClientSettings _settings;

    public ApiClientFactory(
        IHttpClientFactory factory,
        IHttpContextAccessor accessor,
        IOptions<ApiClientSettings> settings)
    {
        _factory = factory;
        _accessor = accessor;
        _settings = settings.Value;
    }

    public HttpClient Create()
    {
        var client = _factory.CreateClient();

        client.BaseAddress = new Uri(_settings.BaseUrl);
        client.Timeout = TimeSpan.FromSeconds(20);

        // Header fixo de versionamento via header (ACORDO DESDE O INÍCIO)
        client.DefaultRequestHeaders.Remove("X-Api-Version");
        client.DefaultRequestHeaders.Add("X-Api-Version", _settings.ApiVersion);

        // Token da sessão
        var token = _accessor.HttpContext?.Session.GetString("AccessToken");
        if (!string.IsNullOrWhiteSpace(token))
        {
            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }

        return client;
    }
}
