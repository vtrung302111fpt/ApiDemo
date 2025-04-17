using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MyApiProject.Models;

namespace MyApiProject.Services
{
    public class BillDetailService
    {
        private readonly HttpClient _httpClient;
    

        public BillDetailService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

    public async Task<BillDetailResponse> GetBillDetailAsync(string accessToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            var response = await _httpClient.GetAsync("https://dev-billstore.xcyber.vn/api/hddv-hoa-don/detail/8de550f6-5e0a-4369-bad6-3a08c8572619");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to fetch user info.");
            }

            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Raw JSON chi tiết hóa đơn là: " + content);

            var result = JsonSerializer.Deserialize<BillDetailResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return result;
        }
    }
}
