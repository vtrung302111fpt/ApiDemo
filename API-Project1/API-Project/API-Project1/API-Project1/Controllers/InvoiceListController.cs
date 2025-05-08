using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

[ApiController]
[Route("api/[controller]")]
public class InvoiceListController : ControllerBase
{
    private readonly HttpClient _httpClient;
    private readonly LoginConfig _loginConfig;

    public InvoiceListController(HttpClient httpClient, IOptions<LoginConfig> loginOptions)
    {
        _httpClient = httpClient;
        _loginConfig = loginOptions.Value;
    }

    [HttpGet(Name ="Get access token")]
    public async Task<IActionResult> GetInvoiceList()
    {
        var loginUrl = "https://stg-accounts-api.xcyber.vn/management/cyberid/login";

        var loginRequest = new
        {
            username = _loginConfig.Username,
            password = _loginConfig.Password
        };

        var json = JsonSerializer.Serialize(loginRequest);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var tokenResponse = await _httpClient.PostAsync(loginUrl, content);

        if (!tokenResponse.IsSuccessStatusCode)
        {
            return StatusCode((int)tokenResponse.StatusCode, "Failed to login.");
        }

        //var responseContent = await response.Content.ReadAsStringAsync();

        //return Ok(JsonDocument.Parse(responseContent).RootElement);
        //return null;

        var tokenJson = await tokenResponse.Content.ReadAsStringAsync();
        var tokenDoc = JsonDocument.Parse(tokenJson);
        var accessToken = tokenDoc.RootElement.GetProperty("access_token").GetString();


        if (string.IsNullOrEmpty(accessToken))
        {
            return BadRequest("Access token not found in response.");
        }

        // Bước 2: Gọi API /user-info
        var userInfoUrl = "https://dev-billstore.xcyber.vn/api/hddv/nguoidung/get-user-info";

        var userRequest = new HttpRequestMessage(HttpMethod.Get, userInfoUrl);
        userRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

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


        //return Ok(userInfoJson);

        // Bước 3: Gọi API /invoice-list (endpoint cuối)
        //var invoiceListUrl = "https://dev-billstore.xcyber.vn//api/hddv-tct/tct/sync-by-doanh-nghiep-login?from=01/03/2025&to=10/03/2025t";
        var invoiceListUrl = "https://dev-billstore.xcyber.vn/api/hddv-hoa-don/get-list?current=1&page=0&pageSize=10&size=10&trangThaiPheDuyet";
        var invoiceListRequest = new HttpRequestMessage(HttpMethod.Get, invoiceListUrl);
        invoiceListRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        invoiceListRequest.Headers.Add("doanhnghiepma", maDoanhNghiep);
        invoiceListRequest.Headers.Add("userma", maNguoiDung);



        invoiceListRequest.Headers.AcceptCharset.Add(
            new System.Net.Http.Headers.StringWithQualityHeaderValue("utf-8")
        );


        var invoiceListResponse = await _httpClient.SendAsync(invoiceListRequest);

        var invoiceListJson = await invoiceListResponse.Content.ReadAsStringAsync();
        var invoiceListDoc = JsonDocument.Parse(invoiceListJson);
        return Ok(invoiceListDoc);
    }
}
