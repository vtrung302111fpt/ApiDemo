using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MyApiProject.Models;


namespace MyApiProject.Services
{
    public class ConnectTctService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        public ConnectTctService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }


        public async Task<ConnectTctResponse> ConnectTctAsync(string accessToken)
        {

            var loginData = new
            {
                username = _configuration["LoginTCT:Username"],
                password = _configuration["LoginTCT:Password"]
            };

            var content = new StringContent(
               JsonSerializer.Serialize(loginData),
               Encoding.UTF8,
               "application/json"
           );

            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            var response = await _httpClient.PostAsync("https://dev-billstore.xcyber.vn/api/hddv-tct/connect/sign-in", content);

           

            var content_response = await response.Content.ReadAsStringAsync();
            Console.WriteLine("RAW JSON là: " + content_response);
            if (!response.IsSuccessStatusCode)
                {
                var errorResult = JsonSerializer.Deserialize<ConnectTctResponse>(content_response, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                throw new Exception($"Lỗi API TCT: {errorResult?.Message ?? "Không rõ lỗi"}");
            }
            var result = JsonSerializer.Deserialize<ConnectTctResponse>(content_response, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return result;
        }
    } 
}

