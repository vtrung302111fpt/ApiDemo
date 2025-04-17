using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MyApiProject.Models;

namespace MyApiProject.Services
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
            _httpClient.DefaultRequestHeaders.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            var response = await _httpClient.GetAsync("https://dev-billstore.xcyber.vn/api/hddv/nguoidung/get-user-info");

            if (!response.IsSuccessStatusCode)
            {
                //return await response.Content.ReadFromJsonAsync<UserInfoResponse>();
                //var error = await response.Content.ReadAsStringAsync();
                //Console.WriteLine("Error là: " + error);
                throw new Exception("Failed to fetch user info.");
            }
            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Raw JSON chi tiết người dùng là: " + content);


            var result = JsonSerializer.Deserialize<UserInfoResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            //Console.WriteLine("Nội dung response là: " + JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true }));

            return result;
        }
    }
}