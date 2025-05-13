using System.Text;
using System.Text.Json;
using API_Project1.Interfaces;

namespace API_Project1.Services
{
    public class TokenService(HttpClient httpClient, LoginConfig loginConfig) : ITokenService
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly LoginConfig _loginConfig = loginConfig;

        private async Task<JsonDocument> CallLoginApiAsync()
        {
            var loginUrl = "https://stg-accounts-api.xcyber.vn/management/cyberid/login";

            var body = new
            {
                username = _loginConfig.Username,
                password = _loginConfig.Password,
            };

            var json = JsonSerializer.Serialize(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(loginUrl, content);
            var responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to login: {response.StatusCode}\nResponse: {responseBody}");
            }

            var tokenJson = await response.Content.ReadAsStringAsync();
            return JsonDocument.Parse(tokenJson);
        }


        public async Task<string> GetFullLoginResponseAsync()
        {
            var json = await CallLoginApiAsync();
            return json.RootElement.ToString();
        }
    }
}