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
        private readonly InterfaceInvoiceList _interfaceInvoiceList;

        public InvoiceDetailService(InterfaceToken tokenService, HttpClient httpClient, InterfaceInvoiceList invoiceList)
        {
            _tokenService = tokenService;
            _httpClient = httpClient;
            _interfaceInvoiceList = invoiceList;
        }


        public async Task<string> GetInvoiceDetailAsync()
        {
            var accessToken = await _tokenService.GetAccessTokenAsync();

            var maHoaDonList = await _interfaceInvoiceList.GetMaHoaDonListAsync(0);                      //lây danh sách mã hóa đơn ở trang 0, trả về List<string> gồm các maHoaDon

            if (maHoaDonList == null || !maHoaDonList.Any()) 
            {
                throw new Exception("Không tìm thấy mã hóa đơn!");                                      //kiểm tra nếu list rỗng                                           
            }


            var detailList = new List<JsonElement>();                                                   //tạo danh sách rỗng để chứa các hóa đơn chi tiết dạng JSON

            foreach (var maHoaDon in maHoaDonList)                                                      //duyệt qua từng mã hóa đơn
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"https://dev-billstore.xcyber.vn/api/hddv-hoa-don/detail/{maHoaDon}");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);   //tạo request, gắn maHoaDon vào URL và access token vào header

                var response = await _httpClient.SendAsync(request);
                var content = await response.Content.ReadAsStringAsync();                               //gửi request, đọc response dạng string

                if (response.IsSuccessStatusCode)
                {
                    using var jsonDoc = JsonDocument.Parse(content);                                    //Parse JSON thành JsonDocument, lấy RootElement, clone lại vì JsonDocument bị dispose sau đó??? 
                    detailList.Add(jsonDoc.RootElement.Clone());                                        //add jsonDoc vào detailList
                }
                else
                {
                    Console.WriteLine($"Lỗi khi lấy chi tiết mã hóa đơn {maHoaDon}: {response.StatusCode}");
                }    
            }

            var finalSon = JsonSerializer.Serialize(detailList, new JsonSerializerOptions { WriteIndented = true });
            return finalSon;

        }
    }
}
