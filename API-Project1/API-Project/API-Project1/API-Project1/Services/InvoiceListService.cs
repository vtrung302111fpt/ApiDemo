using System.Text.Json;
using API_Project1.Responses;

namespace API_Project1.Services
{
    public class InvoiceListService
    {
        private readonly HttpClient _httpClient;

        public InvoiceListService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<InvoiceListResponse> GetInvoiceListResponse(string accessToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            var response = await _httpClient.GetAsync("https://dev-billstore.xcyber.vn/api/hddv-hoa-don/get-list?current=1&page=0&pageSize=10&size=10&trangThaiPheDuyet");
            //var response = await _httpClient.GetAsync("https://dev-billstore.xcyber.vn//api/hddv-tct/tct/sync-by-doanh-nghiep-login?from=01/03/2025&to=10/03/2025");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Fail to fetch invoice list.");
            }

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<InvoiceListResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true});
            return result;
        }
    }
}
