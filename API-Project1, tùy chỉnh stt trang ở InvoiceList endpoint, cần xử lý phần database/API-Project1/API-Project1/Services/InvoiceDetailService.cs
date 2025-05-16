using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using API_Project1.Interfaces;


namespace API_Project1.Services
{
    public class InvoiceDetailService: InterfaceInvoiceDetail
    {
        private readonly InterfaceToken _tokenService;
        private readonly HttpClient _httpClient;

        public InvoiceDetailService(InterfaceToken tokenService, HttpClient httpClient)
        {
            _tokenService = tokenService;
            _httpClient = httpClient;
        }


        public async Task<string> GetInvoiceDetailAsync()
        {
            var accessToken = await _tokenService.GetAccessTokenAsync();

            var request = new HttpRequestMessage(HttpMethod.Get, "https://dev-billstore.xcyber.vn/api/hddv-hoa-don/detail/7d1fe5f5-9592-4f01-8ac5-3b099f3d9fa4");
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
