//using Microsoft.AspNetCore.Mvc;
//using API_Project.Services;
//using API_Project.Models;
//using System;
//using System.Threading.Tasks;

//namespace API_Project.Controllers
//{
//    [Route("api/connect-billstore")]
//    [ApiController]
//    public class BillStoreController : ControllerBase
//    {
//        private readonly BillStoreService _billStoreService;

//        public BillStoreController(BillStoreService billStoreService)
//        {
//            _billStoreService = billStoreService;
//        }

//        [HttpGet("token")]
//        public async Task<IActionResult> GetAccessToken()
//        {
//            try
//            {
//                var token = await _billStoreService.GetAccessTokenAsync();
//                return Ok(token);
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, ex.Message);
//            }
//        }
//    }
//}
