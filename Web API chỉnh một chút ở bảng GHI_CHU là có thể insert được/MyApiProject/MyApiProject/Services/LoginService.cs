using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MyApiProject.Models;

namespace MyApiProject.Services
{
    //Thực hiện thao tác đăng nhập
    public class LoginService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        //nhận HttpCLient: thực hiện thao tác HTTP     Iconfigugration: truy xuất config như username và pass
        public LoginService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration; //dữ liệu đăng nhập lấy từ appsettings.json, 
        }

        //Gửi yêu cầu đăng nhập và nhận token từ API
        public async Task<TokenData> GetTokenDataAsync()
        {
            var loginData = new
            {
                username = _configuration["Login:Username"],
                password = _configuration["Login:Password"]
            };

            var content = new StringContent(
                //dùng để gửi yêu cầu
                JsonSerializer.Serialize(loginData),
                Encoding.UTF8,
                "application/json"
                //chuyển đổi dữ liệu đăng nhập thành JSON, Content - Type: application/json cho yêu cầu HTTP
            );


            //HTTP POST được gửi đến API đăng nhập với dữ liệu loginData 
            var response = await _httpClient.PostAsync("https://stg-accounts-api.xcyber.vn/management/cyberid/login", content);
            

            //Đọc nội dung response từ API
            var responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Response lấy access token là: " + responseContent);

            if (!response.IsSuccessStatusCode)
                //nếu trạng thái HTTP không phải là 200 OK thì sẽ trả về exception với thông tin chi tiết lỗi
                throw new Exception($"Lỗi gọi API: {(int)response.StatusCode} - {responseContent}");


            // Deserialize dữ liệu JSON từ phản hồi thành đối tượng TokenData
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
