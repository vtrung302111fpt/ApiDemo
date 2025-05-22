using System.Text.Json;
using API_Project1.Interfaces;
using API_Project1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API_Project1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceDetailController : ControllerBase
    {
        private readonly IInvoiceDetailService _invoiceDetailService;
        private readonly ILogger<InvoiceDetailController> _logger;
        
        public InvoiceDetailController(IInvoiceDetailService invoiceDetailService, ILogger<InvoiceDetailController> logger)
        {
            _invoiceDetailService = invoiceDetailService;
            _logger = logger;
        }

        [HttpGet("get-invoice-detail")]
        public async Task<IActionResult> GetInvoiceDetailAsync([FromQuery]int currentPage = 0)
        {
            try
            {
                var content = await _invoiceDetailService.GetInvoiceDetailAsync(currentPage);
                //return Content(content, "application/json");



                using var doc = JsonDocument.Parse(content);
                var root = doc.RootElement;

                if (root.ValueKind != JsonValueKind.Array)                   //kiểm tra dạng của
                {
                    return BadRequest("Dữ liệu không hợp lệ, không phải mảng JSON");
                }

                var dataList = new List<JsonElement>();

                foreach (var element in root.EnumerateArray())
                {
                    if (element.TryGetProperty("data", out var dataElement))
                    {
                        dataList.Add(dataElement.Clone());
                    }    
                }

                var finalJson = JsonSerializer.Serialize(dataList, new JsonSerializerOptions { WriteIndented = true });


                List<InvoiceDetailDataModel> invoicesDetails = _invoiceDetailService.ConvertJsonToInvoiceDetail(dataList);

                await _invoiceDetailService.SaveDetailToDatabaseAsync(invoicesDetails);
                return Content(finalJson, "application/json");

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
