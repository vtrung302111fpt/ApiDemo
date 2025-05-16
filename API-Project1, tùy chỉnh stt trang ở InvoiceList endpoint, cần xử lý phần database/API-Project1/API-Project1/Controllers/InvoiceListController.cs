using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;
using API_Project1.Interfaces; // Import interface của LoginService

[ApiController]
[Route("api/[controller]")]
public class InvoiceListController : ControllerBase
{
    private readonly InterfaceInvoiceList _invoiceListService;

    public InvoiceListController(InterfaceInvoiceList invoiceListService)
    {
        _invoiceListService = invoiceListService;
    }

    [HttpGet(Name = "get-invoice-list")]
    public async Task<IActionResult> GetInvoiceListAsync([FromQuery] int currentPage = 0)
    {
        try
        {
            var raw = await _invoiceListService.GetInvoiceListAsync(currentPage);


            var root = JsonDocument.Parse(raw).RootElement;
            var data = root.GetProperty("data");
            return Ok(data);




            //return Content(raw, "application/json");                            //gọi hết full response
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error: {ex.Message}");
        }

    }




    //public async Task<IActionResult> GetInvoiceListAsync([FromQuery] int currentPage = 0)
    //{
    //    try
    //    {
    //        // Lấy danh sách mã hóa đơn từ trang đầu tiên
    //        var maHoaDonList = await _invoiceListService.GetMaHoaDonListAsync(currentPage);

    //        return Ok(maHoaDonList);

    //        // Nếu muốn trả lại toàn bộ response JSON gốc, dùng dòng sau thay thế:
    //        // var raw = await _invoiceListService.GetInvoiceListAsync(currentPage);
    //        // return Content(raw, "application/json");
    //    }
    //    catch (Exception ex)
    //    {
    //        return StatusCode(500, $"Error: {ex.Message}");
    //    }
    //}
}
