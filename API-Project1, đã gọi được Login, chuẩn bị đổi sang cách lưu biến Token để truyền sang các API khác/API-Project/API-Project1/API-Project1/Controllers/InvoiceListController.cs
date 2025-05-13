using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;
using API_Project1.Interfaces; // Import interface của LoginService

[ApiController]
[Route("api/[controller]")]
public class InvoiceListController : ControllerBase
{
    private readonly HttpClient _httpClient;
    private readonly ITokenService _tokenService; // Thêm biến này để gọi đến LoginService

    public InvoiceListController(HttpClient httpClient, ITokenService tokenService)
    {
        _httpClient = httpClient;
        _tokenService = tokenService; // Inject ILoginService
    }

    [HttpGet(Name = "get-invoice-list")]
    public async Task<IActionResult> GetInvoiceList()
    {
        // Lấy access token từ LoginService
        //var accessToken = await _tokenService.GetAccessTokenAsync();

        // Bước 2: Gọi API /user-info
        var userInfoUrl = "https://dev-billstore.xcyber.vn/api/hddv/nguoidung/get-user-info";

        var userRequest = new HttpRequestMessage(HttpMethod.Get, userInfoUrl);
        //userRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var userInfoResponse = await _httpClient.SendAsync(userRequest);

        if (!userInfoResponse.IsSuccessStatusCode)
        {
            return StatusCode((int)userInfoResponse.StatusCode, "Failed to get user info.");
        }

        var userInfoContent = await userInfoResponse.Content.ReadAsStringAsync();
        var userInfoJson = JsonDocument.Parse(userInfoContent).RootElement;

        var maNguoiDung = userInfoJson
            .GetProperty("data")
            .GetProperty("hddvNguoiDungReponse")
            .GetProperty("maNguoiDung")
            .GetString();

        var maDoanhNghiep = userInfoJson
            .GetProperty("data")
            .GetProperty("hddvDoanhNghiepReponse")
            .GetProperty("maDoanhNghiep")
            .GetString();

        // Bước 3: Gọi API /invoice-list (endpoint cuối)
        var invoiceListUrl = "https://dev-billstore.xcyber.vn/api/hddv-hoa-don/get-list?current=1&page=0&pageSize=10&size=10&trangThaiPheDuyet";
        var invoiceListRequest = new HttpRequestMessage(HttpMethod.Get, invoiceListUrl);
        //invoiceListRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        invoiceListRequest.Headers.Add("doanhnghiepma", maDoanhNghiep);
        invoiceListRequest.Headers.Add("userma", maNguoiDung);

        invoiceListRequest.Headers.AcceptCharset.Add(
            new System.Net.Http.Headers.StringWithQualityHeaderValue("utf-8")
        );

        var invoiceListResponse = await _httpClient.SendAsync(invoiceListRequest);

        if (!invoiceListResponse.IsSuccessStatusCode)
        {
            return StatusCode((int)invoiceListResponse.StatusCode, "Failed to get invoice list.");
        }

        var invoiceListJson = await invoiceListResponse.Content.ReadAsStringAsync();
        var invoiceListDoc = JsonDocument.Parse(invoiceListJson);

        // Lọc phần "data" trong response
        var data = invoiceListDoc.RootElement.GetProperty("data");
        return Ok(data);  // Trả về phần data của invoice list
    }
}
