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

        public async Task<string> GetInvoiceListAsync()
        {

            var accessToken = await _tokenService.GetAccessTokenAsync();
            var (userMa, doanhNghiepMa) = await _userInfoService.GetUserAndCompanyCodeAsync();


            var request = new HttpRequestMessage(HttpMethod.Get, "https://dev-billstore.xcyber.vn/api/hddv-hoa-don/get-list?current=1&page=0&pageSize=10&size=10&trangThaiPheDuyet");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            request.Headers.Add("doanhnghiepma", doanhNghiepMa);
            request.Headers.Add("userma", userMa);
            request.Headers.AcceptCharset.Add(
                new StringWithQualityHeaderValue("utf-8")
            );

            var content = await _httpClient.SendAsync(request);
            var response = await content.Content.ReadAsStringAsync();
            Console.WriteLine($"Header doanhnghiepma: {doanhNghiepMa}, userma: {userMa}");


            if (!content.IsSuccessStatusCode)
            {
                throw new Exception($"Request failed: {content.StatusCode}\n{content}");
            }

            return response;

            //var invoiceListDoc = JsonDocument.Parse(content);
            //var data = invoiceListDoc.RootElement.GetProperty("data");
            //return data;

            //var invoiceListJson = await invoiceListResponse.Content.ReadAsStringAsync();
            //var invoiceListDoc = JsonDocument.Parse(invoiceListJson);

            //// Lọc phần "data" trong response
            //var data = invoiceListDoc.RootElement.GetProperty("data");
            //return Ok(data);  // Trả về phần data của invoice list
        }

    }
}
