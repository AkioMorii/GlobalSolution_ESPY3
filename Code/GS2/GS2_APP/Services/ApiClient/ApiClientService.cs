using GS2_APP.Models;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GS2_APP.Services
{
    public class ApiClientService
    {
        private readonly string _baseUrl;
        private readonly string _apiVersion;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpClient _httpClient;

        public ApiClientService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            _baseUrl = configuration["ApiSettings:BaseUrl"]
                ?? throw new ArgumentNullException("ApiSettings:BaseUrl não configurado");

            var version = configuration["ApiSettings:ApiVersion"];
            _apiVersion = string.IsNullOrWhiteSpace(version) ? "1.0" : version;

            _httpClient = CreateHttpClient();
            _httpClient.BaseAddress = new Uri(_baseUrl);
        }

        private HttpClient CreateHttpClient()
        {
            var client = new HttpClient(new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (msg, cert, chain, errors) => true
            });

            client.DefaultRequestHeaders.Remove("api-version");
            client.DefaultRequestHeaders.Add("api-version", _apiVersion);

            return client;
        }

        /// <summary>
        /// Insere o AccessToken automaticamente antes de cada requisição
        /// </summary>
        private void AddJwtHeaderIfExists()
        {
            var token = _httpContextAccessor.HttpContext?.Request.Cookies["AccessToken"];

            if (!string.IsNullOrWhiteSpace(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<ApiResponse<object>> PostAsync(string endpoint, object payload)
        {
            return await PostAsync<object>(endpoint, payload);
        }

        public async Task<ApiResponse<T>> PostAsync<T>(string endpoint, object payload)
        {
            AddJwtHeaderIfExists(); //token automático

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync(endpoint, content);

                var responseContent = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrWhiteSpace(responseContent))
                    responseContent = response.ReasonPhrase ?? "";

                if (response.IsSuccessStatusCode)
                {
                    T? data = default;

                    try
                    {
                        var trimmed = responseContent.TrimStart();
                        if (trimmed.StartsWith("{") || trimmed.StartsWith("["))
                            data = JsonSerializer.Deserialize<T>(responseContent,
                                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    }
                    catch { }

                    return new ApiResponse<T>
                    {
                        Success = true,
                        StatusCode = (int)response.StatusCode,
                        Data = data
                    };
                }
                else
                {
                    string? errorMessage = null;
                    try
                    {
                        using var doc = JsonDocument.Parse(responseContent);
                        if (doc.RootElement.TryGetProperty("error", out var errorProp))
                            errorMessage = errorProp.GetString();
                    }
                    catch
                    {
                        errorMessage = responseContent;
                    }

                    return new ApiResponse<T>
                    {
                        Success = false,
                        StatusCode = (int)response.StatusCode,
                        Error = errorMessage
                    };
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<T>
                {
                    Success = false,
                    StatusCode = 0,
                    Error = ex.Message
                };
            }
        }

        public async Task<ApiResponse<TResponse>> GetAllAsync<TResponse>(string endpoint)
        {
            AddJwtHeaderIfExists();
            ApiResponse<TResponse> apiResponse =new ApiResponse<TResponse>();
            var response = await _httpClient.GetAsync(endpoint);
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                apiResponse.Success = true;
                apiResponse.StatusCode = (int)response.StatusCode;
                apiResponse.status = "success";
                var datas = JsonSerializer.Deserialize<TResponse>(responseContent,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                apiResponse.Data = datas;
            }
            else {
                apiResponse.Success = false;
                apiResponse.StatusCode = (int)response.StatusCode;
                apiResponse.status = "error";
            }
            return apiResponse;
        }
        public async Task<ApiResponse<TResponse>> GetOneAsync<TResponse>(string endpoint,int id)
        {
            AddJwtHeaderIfExists();
            ApiResponse<TResponse> apiResponse = new ApiResponse<TResponse>();
            var response = await _httpClient.GetAsync(endpoint+"/"+id.ToString());
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                apiResponse.Success = true;
                apiResponse.StatusCode = (int)response.StatusCode;
                apiResponse.status = "success";
                var data = JsonSerializer.Deserialize<TResponse>(responseContent,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                apiResponse.Data = data;
            }
            else
            {
                apiResponse.Success = false;
                apiResponse.StatusCode = (int)response.StatusCode;
                apiResponse.status = "error";
            }
            return apiResponse;
        }
        public async Task<bool> DeleteAsync(string endpoint, int id)
        {
            AddJwtHeaderIfExists();
            var response = await _httpClient.DeleteAsync(endpoint+"/"+id.ToString());
            return response.IsSuccessStatusCode;
        }
        public async Task<ApiResponse<T>> PutAsync<T>(int id,string endpoint, object payload)
        {
            AddJwtHeaderIfExists();
            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                endpoint = endpoint + "/" + id.ToString();
                var response = await _httpClient.PutAsync(endpoint, content);
                var responseContent = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(responseContent))
                    responseContent = response.ReasonPhrase ?? "";

                if (response.IsSuccessStatusCode)
                {
                    T? data = default;

                    try
                    {
                        var trimmed = responseContent.TrimStart();
                        if (trimmed.StartsWith("{") || trimmed.StartsWith("["))
                            data = JsonSerializer.Deserialize<T>(responseContent,
                                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    }
                    catch { }

                    return new ApiResponse<T>
                    {
                        Success = true,
                        StatusCode = (int)response.StatusCode,
                        Data = data
                    };
                }
                else
                {
                    string? errorMessage = null;
                    try
                    {
                        using var doc = JsonDocument.Parse(responseContent);
                        if (doc.RootElement.TryGetProperty("error", out var errorProp))
                            errorMessage = errorProp.GetString();
                    }
                    catch
                    {
                        errorMessage = responseContent;
                    }

                    return new ApiResponse<T>
                    {
                        Success = false,
                        StatusCode = (int)response.StatusCode,
                        Error = errorMessage
                    };
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<T>
                {
                    Success = false,
                    StatusCode = 0,
                    Error = ex.Message
                };
            }
        }
        public async Task<ApiResponse<T>> PostFilesAsync<T>(string endpoint, List<ConteudoModel> conteudos)
        {
            AddJwtHeaderIfExists(); // token automático

            using var content = new MultipartFormDataContent();

            foreach (var c in conteudos.Where(x => x.Arquivo != null))
            {
                var arquivo = c.Arquivo;

                using var ms = new MemoryStream();
                await arquivo.CopyToAsync(ms);
                ms.Position = 0;

                var fileContent = new ByteArrayContent(ms.ToArray());
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(arquivo.ContentType);

                // Adiciona arquivo
                content.Add(fileContent, "arquivos", arquivo.FileName);

                // Adiciona metadados correspondentes
                content.Add(new StringContent(c.TipoConteudoId.ToString()), "TipoConteudoId");
                content.Add(new StringContent(c.Ordem.ToString()), "Ordem");
            }

            try
            {
                var response = await _httpClient.PostAsync(endpoint, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrWhiteSpace(responseContent))
                    responseContent = response.ReasonPhrase ?? "";

                if (response.IsSuccessStatusCode)
                {
                    T? data = default;
                    try
                    {
                        var trimmed = responseContent.TrimStart();
                        if (trimmed.StartsWith("{") || trimmed.StartsWith("["))
                            data = JsonSerializer.Deserialize<T>(responseContent,
                                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    }
                    catch { }

                    return new ApiResponse<T>
                    {
                        Success = true,
                        StatusCode = (int)response.StatusCode,
                        Data = data
                    };
                }
                else
                {
                    string? errorMessage = null;
                    try
                    {
                        using var doc = JsonDocument.Parse(responseContent);
                        if (doc.RootElement.TryGetProperty("error", out var errorProp))
                            errorMessage = errorProp.GetString();
                    }
                    catch
                    {
                        errorMessage = responseContent;
                    }

                    return new ApiResponse<T>
                    {
                        Success = false,
                        StatusCode = (int)response.StatusCode,
                        Error = errorMessage
                    };
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<T>
                {
                    Success = false,
                    StatusCode = 0,
                    Error = ex.Message
                };
            }
        }


    }
}
