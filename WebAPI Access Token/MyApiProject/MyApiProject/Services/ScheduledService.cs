﻿using System.Diagnostics.Eventing.Reader;


//Đây là một service chạy nền, chạy ngầm khi ứng dụng khởi động
//không cần request từ người dùng, liên tục chạy task định kỳ như gọi API mỗi vài giây hay đồng bộ dữ liệu định kỳ...
//chạy song song vói web chính, web dừng thì cũng dừng theo

//vì đang không có frontend nên cần service background này, 
//có frontend rồi thì có hay không cũng được

namespace MyApiProject.Services
{
    public class ScheduledService: BackgroundService
    {

        private readonly ILogger<ScheduledService> _logger;
        private readonly LoginService _loginService;
        public ScheduledService(ILogger<ScheduledService> logger, LoginService loginService) 
        {
            _logger = logger;
            _loginService = loginService;
        }
        //Dependency Injection được dùng dể truyền ILogger (ghi log) và LoginService(để lấy token)

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        //định nghĩa logic chạy nền, hàm được gọi ngay khi khởi động app
        {
            //vòng lặp lấy token định kỳ, stoppingToken dừng vòng lặp khi shutdown
            while (!stoppingToken.IsCancellationRequested)
            // nếu không có ! thì chỉ chạy nếu đang yêu cầu hủy, tức là true khi Ctrl+C, nên vòng lặp không chạy
            {
                try
                {
                    //var token = await _loginService.GetAccessTokenAsync();
                    ////LoginService để lấy access_token
                    //_logger.LogInformation($"Token lấy được: {token}");
                    ////ghi lại log
                    ///


                    var tokenData = await _loginService.GetTokenDataAsync();
                    _logger.LogInformation($"Access Token: {tokenData.AccessToken}");
                    _logger.LogInformation($"Refresh Token: {tokenData.RefreshToken}");
                    _logger.LogInformation($"Token Type: {tokenData.TokenType}");
                    _logger.LogInformation($"Expires In: {tokenData.ExpiresIn} giây");
                }

                catch (Exception ex)
                {
                    _logger.LogError(ex, "Lỗi khi gọi API lấy token");
                }
                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
                //delay, 10s rồi lại lặp lại vòng lặp lấy token định kỳ
            }
        }
    }
}
