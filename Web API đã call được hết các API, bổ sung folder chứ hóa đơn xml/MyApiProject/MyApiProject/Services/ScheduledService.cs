using MyApiProject.Models;
using System.Diagnostics.Eventing.Reader;


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
        private readonly UserInfoService _userInfoService;
        private readonly ConnectTctService _connectTctService;
        private readonly DisconnectTctService _disconnectTctService;
        private readonly BillDetailService _billDetailService;
        private readonly BillListService _billListService;
        private readonly SyncInvoiceTctService _syncInvoiceTctService;
        private readonly ImportXMLService _importXMLService;
        private readonly PreviewBase64Service _previewBase64Service;
        
        public ScheduledService(
            ILogger<ScheduledService> logger, 
            LoginService loginService, 
            UserInfoService userInfoService, 
            ConnectTctService connectTctService,
            DisconnectTctService disconnectTctService,
            BillDetailService billDetailService, 
            BillListService billListService,
            SyncInvoiceTctService syncInvoiceTctService,
            ImportXMLService importXMLService,
            PreviewBase64Service previewBase64Service
            ) 
        {
            _logger = logger;
            _loginService = loginService;
            _userInfoService = userInfoService;
            _connectTctService = connectTctService;
            _disconnectTctService = disconnectTctService;
            _billDetailService = billDetailService;
            _billListService = billListService;
            _syncInvoiceTctService = syncInvoiceTctService;
            _importXMLService = importXMLService;
            _previewBase64Service = previewBase64Service; 
        }
        //Dependency Injection được dùng dể truyền ILogger (ghi log) và LoginService(để lấy token)

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        //định nghĩa logic service nền, hàm được gọi ngay khi khởi động 
        {
            //vòng lặp lấy token định kỳ, stoppingToken dừng vòng lặp khi shutdown
            while (!stoppingToken.IsCancellationRequested)
            // nếu không có ! thì chỉ chạy nếu đang yêu cầu hủy, tức là true khi Ctrl+C, nên vòng lặp không chạy
            {
                try
                {
                    var tokenData = await _loginService.GetTokenDataAsync();
                    //_logger.LogInformation($"Access Token: {tokenData.AccessToken}");
                    //_logger.LogInformation($"Refresh Token: {tokenData.RefreshToken}");
                    //_logger.LogInformation($"Token Type: {tokenData.TokenType}");
                    //_logger.LogInformation($"Expires In: {tokenData.ExpiresIn} giây");
                    //_logger.LogInformation($"Refresh expires In: {tokenData.RefreshExpiresIn} giây");


                    var userInfo = await _userInfoService.GetUserInfoAsync(tokenData.AccessToken);
                    var connectTct = await _connectTctService.ConnectTctAsync(tokenData.AccessToken);
                    var disconnectTct = await _disconnectTctService.DisconnectTctAsync(tokenData.AccessToken);
                    var billDetail = await _billDetailService.GetBillDetailAsync(tokenData.AccessToken);
                    var billList = await _billListService.GetBillListAsync(tokenData.AccessToken);
                    var syncInvoiceTct = await _syncInvoiceTctService.SyncInvoiceTctAsync(tokenData.AccessToken);
                    var importXML = await _importXMLService.ImportXMLAsync(tokenData.AccessToken);
                    var previewBase64 = await _previewBase64Service.PreviewBase64Async(tokenData.AccessToken);
                }

                catch (Exception ex)
                {
                    _logger.LogError(ex, "Lỗi khi gọi API lấy token");
                }
                await Task.Delay(TimeSpan.FromMinutes(10), stoppingToken);
                //delay, 10s rồi lại lặp lại vòng lặp lấy token định kỳ
            }
        }
    }
}
