using BookStoreApi.Services.Models;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace BookStoreApi.Services
{
    public interface ISmsIrService
    {
        Task<VerifyMessageResponse?> SendVerificationAsync(string mobile);
    }

    public class SmsIrService(HttpClient httpClient, IConfiguration configuration) : ISmsIrService
    {
        private readonly string _apiKey = configuration["SmsIr:SandboxApiKey"]
                      ?? throw new Exception("SmsIr Sandbox API Key is not configured.");
        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        private const string BaseUrl = "https://api.sms.ir/v1/send/verify";

        public async Task<VerifyMessageResponse?> SendVerificationAsync(string mobile)
        {
            var request = new VerifyMessageRequest { Mobile = mobile };
            var json = JsonSerializer.Serialize(request, _jsonOptions);
            using var content = new StringContent(json, Encoding.UTF8, "application/json");

            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Add("x-api-key", _apiKey);

            var response = await httpClient.PostAsync(BaseUrl, content);
            var body = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<VerifyMessageResponse>(body, _jsonOptions);
        }
    }
}
