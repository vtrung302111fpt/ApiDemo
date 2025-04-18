using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MyApiProject.Models;
using MyApiProject.Services;

namespace MyApiProject.Models
{
    public class BillListService
    {
        private readonly HttpClient _httpClient;

        public BillListService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<BillDetailResponse> GetBillListAsync(string accessToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            
            _httpClient.DefaultRequestHeaders.Add("doanhnghiepma", String.Empty);
            _httpClient.DefaultRequestHeaders.Add("userma", String.Empty);


            var response = await _httpClient.GetAsync("https://dev-billstore.xcyber.vn/api/hddv-hoa-don/get-list?current=1&page=0&pageSize=10&size=10&trangThaiPheDuyet");
            

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to fetch invoice list.");
            }

            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Raw JSON danh sách hóa đơn là: " + content);

            var result = JsonSerializer.Deserialize<BillDetailResponse>(content);
            return result;
        }

    }
}
