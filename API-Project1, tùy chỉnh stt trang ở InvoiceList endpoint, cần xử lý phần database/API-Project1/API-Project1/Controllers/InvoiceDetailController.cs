using API_Project1.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_Project1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceDetailController : ControllerBase
    {
        private readonly InterfaceInvoiceDetail _invoiceDetailService;
        
        public InvoiceDetailController(InterfaceInvoiceDetail invoiceDetailService)
        {
            _invoiceDetailService = invoiceDetailService;
        }

        [HttpGet("get-invoice-detail")]
        public async Task<IActionResult> GetInvoiceDetailAsync()
        {
            try
            {
                var content = await _invoiceDetailService.GetInvoiceDetailAsync();
                return Content(content, "application/json");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
