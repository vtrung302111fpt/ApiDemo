using System.Text.Json;
using API_Project1.Interfaces;
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


                //Phân tích kiểu dữ liệu của từng trường trong từng phần tử
                foreach (var dataElement in dataList)
                {
                    _logger.LogInformation("---- Phân tích một phần tử data ----");
                    foreach (var property in dataElement.EnumerateObject())
                    {
                        var name = property.Name;
                        var value = property.Value;
                        string type;

                        switch (value.ValueKind)
                        {
                            case JsonValueKind.String:
                                type = "string";
                                break;
                            case JsonValueKind.Number:
                                if (value.TryGetInt64(out _))
                                    type = "int/long";
                                else if (value.TryGetDouble(out _))
                                    type = "double";
                                else
                                    type = "number (unknown)";
                                break;
                            case JsonValueKind.True:
                            case JsonValueKind.False:
                                type = "bool";
                                break;
                            case JsonValueKind.Null:
                                type = "null";
                                break;
                            case JsonValueKind.Object:
                                type = "object";
                                break;
                            case JsonValueKind.Array:
                                type = "array";
                                break;
                            default:
                                type = "unknown";
                                break;
                        }

                        _logger.LogInformation("Trường: {Name}, Kiểu dữ liệu: {Type}", name, type);
                    }
                }

                var finalJson = JsonSerializer.Serialize(dataList, new JsonSerializerOptions { WriteIndented = true });
                _logger.LogInformation("Nội dung trả về từ service: {Content}", finalJson);
                return Content(finalJson, "application/json");


            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
