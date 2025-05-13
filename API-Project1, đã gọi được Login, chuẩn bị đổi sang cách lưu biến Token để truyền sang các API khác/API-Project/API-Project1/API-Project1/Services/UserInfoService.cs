using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using API_Project1.Interfaces;

namespace API_Project1.Services
{
    public class UserInfoService : IUserInfoService
    {
        private readonly ITokenService _tokenService;
        private readonly HttpClient _httpClient;

        public UserInfoService(ITokenService tokenService, IHttpClientFactory httpClientFactory)
        {
            _tokenService = tokenService;
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<string> GetUserInfoAsync()
        {
            var accessToken = await _tokenService.GetAccessTokenAsync();

            var request = new HttpRequestMessage(HttpMethod.Get, "https://dev-billstore.xcyber.vn/api/hddv/nguoidung/get-user-info");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await _httpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Request failed: {response.StatusCode}\n{content}");
            }

            return content;
        }
    }


}
