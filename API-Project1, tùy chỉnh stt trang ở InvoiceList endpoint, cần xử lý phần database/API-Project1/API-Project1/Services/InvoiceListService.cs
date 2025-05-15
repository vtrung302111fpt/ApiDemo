using System.Net.Http.Headers;
using System.Text.Json;
using API_Project1.Interfaces;
//using API_Project1.Responses;k
using Microsoft.AspNetCore.Http.HttpResults;


namespace API_Project1.Services
{
    public class InvoiceListService : IInvoiceListService
    {
        private readonly HttpClient _httpClient;
        private readonly ITokenService _tokenService;
        private readonly IUserInfoService _userInfoService;


        public InvoiceListService(IHttpClientFactory httpClientFactory, ITokenService tokenService, IUserInfoService userInfoService)

        {
            _httpClient = httpClientFactory.CreateClient();
            _tokenService = tokenService;
            _userInfoService = userInfoService;
        }

        public async Task GetAllDataAsync()
        {
            int currentPage = 0;

            // Lấy lần đầu tiên để biết totalPage
            var firstResponse = await GetInvoiceListAsync(currentPage);
            using var firstDoc = JsonDocument.Parse(firstResponse);
            var root = firstDoc.RootElement;

            int totalPage = root.GetProperty("totalPage").GetInt32();

            // Xử lý items trang đầu tiên
            var items = root.GetProperty("items");
            Console.WriteLine($"Trang {currentPage + 1}/{totalPage}");
            foreach (var item in items.EnumerateArray())
            {
                // xử lý từng item
            }

            currentPage++;

            // Vòng lặp lấy các trang còn lại
            while (currentPage < totalPage)
            {
                var response = await GetInvoiceListAsync(currentPage);
                using var jsonDoc = JsonDocument.Parse(response);
                var pageRoot = jsonDoc.RootElement;

                var moreItems = pageRoot.GetProperty("items");
                Console.WriteLine($"Trang {currentPage + 1}/{totalPage}");
                foreach (var item in moreItems.EnumerateArray())
                {
                    // xử lý từng item
                }

                currentPage++;
            }
        }

        public async Task<string> GetInvoiceListAsync(int currentPage)
        {

            var accessToken = await _tokenService.GetAccessTokenAsync();
            var (maNguoiDung, maDoanhNghiep) = await _userInfoService.GetUserAndCompanyCodeAsync();
            //lấy response từ hàm GetUserAndCompanyCodeAsync(), lưu vào userMa và doanhNghiepMa


            //var request = new HttpRequestMessage(HttpMethod.Get, "https://dev-billstore.xcyber.vn/api/hddv-hoa-don/get-list?current=1&page=0&pageSize=10&size=10&trangThaiPheDuyet");
            var request = new HttpRequestMessage(HttpMethod.Get,$"https://dev-billstore.xcyber.vn/api/hddv-hoa-don/get-list?current=1&page={currentPage}&pageSize=10&size=10&trangThaiPheDuyet");

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            request.Headers.Add("doanhnghiepma", maDoanhNghiep);
            request.Headers.Add("userma", maNguoiDung);
            request.Headers.AcceptCharset.Add(
                new StringWithQualityHeaderValue("utf-8")
            );

            var content = await _httpClient.SendAsync(request);
            var response = await content.Content.ReadAsStringAsync();


            //Console.WriteLine($"Header doanhnghiepma: {maDoanhNghiep}, userma: {maNguoiDung}");


            if (!content.IsSuccessStatusCode)
            {
                throw new Exception($"Request failed: {content.StatusCode}\n{content}");
            }

            return response;
        }

    }
}
