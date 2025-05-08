using System.Net.Http.Headers;
using System.Text.Json;
using API_Project1.Responses;

namespace API_Project1.Services
{
    public class UserInfoService
    {
        private readonly HttpClient _httpClient;

        public UserInfoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<UserInfoResponse> GetUserInfoAsync(string accessToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await _httpClient.GetAsync("https://dev-billstore.xcyber.vn/api/hddv/nguoidung/get-user-info");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to fetch user info.");
            }

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<UserInfoResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return result;
        }
    }

}
