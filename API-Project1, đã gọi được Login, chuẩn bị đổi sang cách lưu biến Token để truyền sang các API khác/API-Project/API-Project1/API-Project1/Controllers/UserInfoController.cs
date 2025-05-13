using System.Net.Http.Headers;
using System.Threading.Tasks;
using API_Project1.Interfaces;
using API_Project1.Services;
using Microsoft.AspNetCore.Mvc;

namespace API_Project1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserInfoController : ControllerBase
    {
        private readonly ILoginService _loginService;
        private readonly IUserInfoService _userInfoService;


        public UserInfoController(IUserInfoService userInfoService, ILoginService loginService)
        {
            _userInfoService = userInfoService;
            _loginService = loginService;
        }

        [HttpGet("get-user-info")]
        public async Task<IActionResult> GetUserInfoAsync()
        {
            try
            {
                var accessToken = await _loginService.GetAccessTokenAsync();
                // GetAccessTokenAsync sẽ check nếu token đã có sẵn, chưa hết hạn => dùng lại.
                


                Console.WriteLine("Access token: " + accessToken);

                var userInfo = await _userInfoService.GetUserInfoAsync(accessToken);

                return Content(userInfo, "application/json");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
    }
}
