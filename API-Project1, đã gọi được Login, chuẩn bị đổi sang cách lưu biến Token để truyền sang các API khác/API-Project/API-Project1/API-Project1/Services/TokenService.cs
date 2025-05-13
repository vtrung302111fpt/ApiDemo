using System.Text;
using System.Text.Json;
using API_Project1.Interfaces;

namespace API_Project1.Services
{
    public class TokenService : ITokenService
    {
        private readonly HttpClient _httpClient;
        private readonly LoginConfig _loginConfig;

        private string _accessToken;
        private DateTime _expiryTime;

        public TokenService(HttpClient httpClient, LoginConfig loginConfig)
        {
            _httpClient = httpClient;
            _loginConfig = loginConfig;
        }

        public async Task<string> GetAccessTokenAsync()
        {
            if(string.IsNullOrEmpty(_accessToken) || DateTime.UtcNow > _expiryTime)
            {
                await LoginAsync();
            }
            return _accessToken;
        }

        public async Task<string> GetFullLoginResponseAsync()
        {
            var json = await CallLoginApiAsync();
            return json.RootElement.ToString();
        }

        private async Task LoginAsync()
        {
            var json = await CallLoginApiAsync();
            var root = json.RootElement;

            _accessToken = root.GetProperty("access_token").GetString();
            var expiresIn = root.GetProperty("expires_in").GetInt32();

            _expiryTime = DateTime.UtcNow.AddSeconds(expiresIn - 30);

        }
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

            return JsonDocument.Parse(responseBody);
        }
    }
}