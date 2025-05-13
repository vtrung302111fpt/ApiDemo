//using System.Text;
//using System.Text.Json;
//using API_Project1.Interfaces;
//using Microsoft.AspNetCore.Mvc;

//namespace API_Project1.Services
//{
//    public class LoginService : ILoginService
//    {
//        private readonly HttpClient _httpClient;
//        private readonly LoginConfig _loginConfig;
//        private readonly ITokenService _tokenService;

//        public LoginService(HttpClient httpClient, LoginConfig loginConfig, ITokenService tokenService)
//        {
//            _httpClient = httpClient;
//            _loginConfig = loginConfig;
//            _tokenService = tokenService;
//        }

//        // Hàm gọi login chung
//        private async Task<JsonDocument> CallLoginApiAsync()
//        {
//            var loginUrl = "https://stg-accounts-api.xcyber.vn/management/cyberid/login";

//            var loginRequest = new
//            {
//                username = _loginConfig.Username,
//                password = _loginConfig.Password,
//            };

//            var json = JsonSerializer.Serialize(loginRequest);
//            var content = new StringContent(json, Encoding.UTF8, "application/json");

//            var response = await _httpClient.PostAsync(loginUrl, content);
//            var responseBody = await response.Content.ReadAsStringAsync();

//            if (!response.IsSuccessStatusCode)
//            {
//                throw new Exception($"Failed to login: {response.StatusCode}\nResponse: {responseBody}");
//            }

//            var tokenJson = await response.Content.ReadAsStringAsync();
//            return JsonDocument.Parse(tokenJson);
//        }

//        // Lấy full response
//        public async Task<string> GetFullLoginResponse()
//        {
//            var json = await CallLoginApiAsync();
//            return json.RootElement.ToString();  // Trả về full response
//        }

//        // Lấy access token
//        public async Task<string> GetAccessTokenAsync()
//        {
//            if (_tokenService.IsTokenExpired())
//            {
//                await CallLoginApiAsync();
//            }
//            return _tokenService.AccessToken;
//        }
//    }
//}
