using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using API_Project1.Interfaces;

namespace API_Project1.Services
{
    public class UserInfoService : IUserInfoService
    {
        private readonly HttpClient _httpClient;

        public UserInfoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetUserInfoAsync(string accessToken)
        {


            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            
            
            //var request = new HttpRequestMessage(HttpMethod.Get, "https://dev-billstore.xcyber.vn/api/hddv/nguoidung/get-user-info");
            //request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);




            var response = await client.GetAsync("https://dev-billstore.xcyber.vn/api/hddv/nguoidung/get-user-info");

            if(!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Failed to get user info: {response.StatusCode}\nResponse: {errorContent}");
            }    
            
            return await response.Content.ReadAsStringAsync();
        }
    }

}
