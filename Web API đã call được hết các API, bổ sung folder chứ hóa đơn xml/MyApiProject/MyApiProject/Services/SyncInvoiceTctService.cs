using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MyApiProject.Models;
using MyApiProject.Services;

namespace MyApiProject.Services
{
    public class SyncInvoiceTctService
    {
        private readonly HttpClient _httpClient;

        public SyncInvoiceTctService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<SyncInvoiceTctResponse> SyncInvoiceTctAsync(string accessToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            _httpClient.DefaultRequestHeaders.Add("doanhnghiepma", String.Empty);
            _httpClient.DefaultRequestHeaders.Add("userma", String.Empty);

            var response = await _httpClient.GetAsync("https://dev-billstore.xcyber.vn/api/hddv-tct/tct/sync-by-doanh-nghiep-login?from=01/03/2025&to=10/03/2025");
        
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to sync invoice from TCT");
            }

            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Raw JSON kết quả tiếp nhận yêu cầu đồng bộ hóa đơn từ TCT: " + content);

            var result = JsonSerializer.Deserialize<SyncInvoiceTctResponse>(content);
            return result;
        }
    }


}
