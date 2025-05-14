using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;
using API_Project1.Interfaces; // Import interface của LoginService

[ApiController]
[Route("api/[controller]")]
public class InvoiceListController : ControllerBase
{
    private readonly IInvoiceListService _invoiceListService;

    public InvoiceListController(IInvoiceListService invoiceListService)
    {
        _invoiceListService = invoiceListService;
    }

    [HttpGet(Name = "get-invoice-list")]
    public async Task<IActionResult> GetInvoiceListAsync()
    {
        try
        {
            var raw = await _invoiceListService.GetInvoiceListAsync();
            //return Content(raw, "application/json");
            var root = JsonDocument.Parse(raw).RootElement;
            var data = root.GetProperty("data");
            return Ok(data);
            
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error: {ex.Message}");
        } 
        
    }
}
