using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using MyApiProject.Models;

namespace MyApiProject.Services
{
    public class ImportXMLService
    {
        private readonly HttpClient _httpClient;

        public ImportXMLService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ImportXMLResponse> ImportXMLAsync(string accessToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", accessToken);

            // Thêm headers đặc biệt
            if (!_httpClient.DefaultRequestHeaders.Contains("doanhnghiepma"))
                _httpClient.DefaultRequestHeaders.Add("doanhnghiepma", string.Empty);

            if (!_httpClient.DefaultRequestHeaders.Contains("userma"))
                _httpClient.DefaultRequestHeaders.Add("userma", string.Empty);

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Test XML Files", "Invoice_Export_Sample.xml");

            using var fileStream = File.OpenRead(filePath);
            using var streamContent = new StreamContent(fileStream);
            streamContent.Headers.ContentType = new MediaTypeHeaderValue("text/xml");

            var multipartContent = new MultipartFormDataContent();
            multipartContent.Add(streamContent, "file", Path.GetFileName(filePath));

            var response = await _httpClient.PostAsync("https://dev-billstore.xcyber.vn/api/hddv-hoa-don/file/import", multipartContent);

            var content_response = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Raw JSON kết quả import XML là: " + content_response);

            var result = JsonSerializer.Deserialize<ImportXMLResponse>(content_response, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Lỗi API TCT: {result?.Message ?? "Không rõ lỗi"}");
            }

            return result!;
        }
    }
}
