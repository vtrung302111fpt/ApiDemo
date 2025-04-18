using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MyApiProject.Models;
using MyApiProject.Services;

namespace MyApiProject.Services
{
    public class PreviewBase64Service
    {
        private readonly HttpClient _httpClient;

        public PreviewBase64Service(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<PreviewBase64Response> PreviewBase64Async(string accessToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            var response = await _httpClient.GetAsync("https://dev-billstore.xcyber.vn/api/hddv-hoa-don/preview-base64/8de550f6-5e0a-4369-bad6-3a08c8572619");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to preview base64.");
            }
            
            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Raw JSON thông tin bản thể hiện hóa đơn: " + content);

            var result = JsonSerializer.Deserialize<PreviewBase64Response>(content);
            return result;
        }
    }
}
