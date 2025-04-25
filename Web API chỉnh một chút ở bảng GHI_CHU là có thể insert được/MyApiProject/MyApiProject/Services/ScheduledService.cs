using MyApiProject.Models;
using System.Diagnostics.Eventing.Reader;


//Đây là một  background service, chạy ngầm khi ASP.NET Core khởi động
//không cần request từ người dùng, dùng để chạy task định kỳ như gọi API mỗi vài giây hay đồng bộ dữ liệu định kỳ...
//

//vì đang không có frontend nên cần service background này, 
//có frontend rồi thì có hay không cũng được

namespace MyApiProject.Services
{
    public class ScheduledService: BackgroundService
    {

        private readonly ILogger<ScheduledService> _logger;
        private readonly LoginService _loginService;
        private readonly BillListService _billListService;
        //biến trong class, để lưu service
        
        
        public ScheduledService(
            ILogger<ScheduledService> logger, 
            LoginService loginService,
            BillListService billListService     
            ) 
        {
            _logger = logger;
            _loginService = loginService;
            _billListService = billListService;
            //gán giá trị cho các biến để sử dụng mọi nơi trong ScheduledService
        }
        //Dependency Injection được dùng dể truyền ILogger (ghi log) và LoginService(để lấy token)


        //hàm chính được gọi khi Service chạy
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //vòng lặp lấy token định kỳ
            //stoppingToken dừng vòng lặp khi shutdown
            while (!stoppingToken.IsCancellationRequested)
            // nếu không có ! thì chỉ chạy nếu đang yêu cầu hủy, tức là true khi Ctrl+C, nên vòng lặp không chạy
            {
                try
                {
                    var tokenData = await _loginService.GetTokenDataAsync(); 
                    var billList = await _billListService.GetBillListAsync(tokenData.AccessToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Lỗi khi gọi API lấy token");
                }
                await Task.Delay(TimeSpan.FromMinutes(10), stoppingToken);
            }
        }
    }
}






//private readonly UserInfoService _userInfoService;
//private readonly ConnectTctService _connectTctService;
//private readonly DisconnectTctService _disconnectTctService;
//private readonly BillDetailService _billDetailService;
//private readonly SyncInvoiceTctService _syncInvoiceTctService;
//private readonly ImportXMLService _importXMLService;
//private readonly PreviewBase64Service _previewBase64Service;


//_userInfoService = userInfoService;
//_connectTctService = connectTctService;
//_disconnectTctService = disconnectTctService;
//_billDetailService = billDetailService;
//_syncInvoiceTctService = syncInvoiceTctService;
//_importXMLService = importXMLService;
//_previewBase64Service = previewBase64Service; 






//_logger.LogInformation($"Access Token: {tokenData.AccessToken}");
//_logger.LogInformation($"Refresh Token: {tokenData.RefreshToken}");
//_logger.LogInformation($"Token Type: {tokenData.TokenType}");
//_logger.LogInformation($"Expires In: {tokenData.ExpiresIn} giây");
//_logger.LogInformation($"Refresh expires In: {tokenData.RefreshExpiresIn} giây"); 




//var syncInvoiceTct = await _syncInvoiceTctService.SyncInvoiceTctAsync(tokenData.AccessToken);
//var importXML = await _importXMLService.ImportXMLAsync(tokenData.AccessToken);
//var previewBase64 = await _previewBase64Service.PreviewBase64Async(tokenData.AccessToken);
//var userInfo = await _userInfoService.GetUserInfoAsync(tokenData.AccessToken);
//var connectTct = await _connectTctService.ConnectTctAsync(tokenData.AccessToken);
//var disconnectTct = await _disconnectTctService.DisconnectTctAsync(tokenData.AccessToken);
//var billDetail = await _billDetailService.GetBillDetailAsync(tokenData.AccessToken);