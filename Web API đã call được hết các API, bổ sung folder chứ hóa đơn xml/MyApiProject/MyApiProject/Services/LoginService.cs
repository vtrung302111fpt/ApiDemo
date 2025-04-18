//using System.Net.Http;
//using System.Text;
//using System.Text.Json;
//using System.Threading.Tasks;
//using System.Text;
//using System.Text.Json;
//using static System.Net.Mime.MediaTypeNames;


//namespace MyApiProject.Services
//{
//    public class LoginService
//    {
//        private readonly HttpClient _httpClient;
//        private readonly IConfiguration _configuration;
//        //nhận HttpCLient và Iconfigugration để thực hiện thao tác HTTP và truy xuất config như username và pass
//        public LoginService(HttpClient httpClient, IConfiguration configuration)
//        {
//            _httpClient = httpClient;
//            _configuration = configuration;
//            //dữ liệu đăng nhập lấy từ appsettings.json, 
//        }
//        public async Task<string> GetAccessTokenAsync()
//        {
//            var loginData = new
//            {
//                username = _configuration["Login:Username"],
//                password = _configuration["Login:Password"]
//            };

//            var content = new StringContent(
//                //dùng để gửi yêu cầu
//                JsonSerializer.Serialize(loginData),
//                Encoding.UTF8,
//                "application/json"
//                //chuyển đổi dữ liệu đăng nhập thành JSON, Content - Type: application/json cho yêu cầu HTTP
//                );

//            var response = await _httpClient.PostAsync("https://stg-accounts-api.xcyber.vn/management/cyberid/login", content);
//            //HTTP POST được gửi đến API đăng nhập với loginData làm nội dung yêu cầu

//            var responseContent = await response.Content.ReadAsStringAsync();
//            //nếu trạng thái HTTP không phải là 200 OK thì sẽ trả về exception với thông tin chi tiết lỗi
//            if (!response.IsSuccessStatusCode)
//                throw new Exception($"Lỗi gọi API: {(int)response.StatusCode} - {responseContent}");

//            var json = JsonDocument.Parse(responseContent); 
//            var token = json.RootElement.GetProperty("access_token").GetString();
//            //phản hồi được phân tích từ JSON để lấy giá trị của access_token

//            return token ?? throw new Exception("Không tìm thấy access_token trong response");
//            //Nếu không có access_token trong phản hồi, sẽ ném ra một ngoại lệ với thông báo như trên
//        }

//    }
//}



using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MyApiProject.Models;

namespace MyApiProject.Services
{
    public class LoginService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public LoginService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<TokenData> GetTokenDataAsync()
        {
            var loginData = new
            {
                username = _configuration["Login:Username"],
                password = _configuration["Login:Password"]
            };

            var content = new StringContent(
                JsonSerializer.Serialize(loginData),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.PostAsync("https://stg-accounts-api.xcyber.vn/management/cyberid/login", content);
            var responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Response lấy access token là: " + responseContent);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Lỗi gọi API: {(int)response.StatusCode} - {responseContent}");

            var result = JsonSerializer.Deserialize<TokenData>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true // Cho phép không phân biệt hoa thường
            });

            if (result == null)
                throw new Exception("Không thể phân tích TokenData từ phản hồi");

            return result;
        }
    }
}
