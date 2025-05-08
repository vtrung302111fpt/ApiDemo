using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;

namespace API_Project1.Services
{
    public class LoginService
    {
        private readonly HttpClient _httpClient;
        private readonly LoginConfig _loginConfig;

        public LoginService(HttpClient httpClient, IOptions<LoginConfig> config)
        {
            _httpClient = httpClient;
            _loginConfig = config.Value;
        }

        public async Task<string> GetTokenAsync()
        {
            var loginUrl = "https://stg-accounts-api.xcyber.vn/management/cyberid/login";

            var body = new
            {
                username = _loginConfig.Username,
                password = _loginConfig.Password
            };

            var content = new StringContent(
                JsonSerializer.Serialize(body),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync(loginUrl, content);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Không đăng nhập được.");
            }

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            return doc.RootElement.GetProperty("access_token").GetString();
        }
    }

}
