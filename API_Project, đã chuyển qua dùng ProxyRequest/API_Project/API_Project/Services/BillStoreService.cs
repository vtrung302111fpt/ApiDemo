//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Http;
//using System.Text.Json;
//using System.Threading.Tasks;
//using System.Web;
//using API_Project.Models;
////using System.Web.Http.Controllers;

//namespace API_Project.Services
//{
//    public class BillStoreService
//    {
//        private readonly HttpClient _httpClient;
//        private readonly IConfiguration _configuration;

//        public BillStoreService(HttpClient httpClient, IConfiguration configuration)
//        {
//            _httpClient = httpClient;
//            _configuration = configuration;
//        }
//        public async Task<TokenData> GetAccessTokenAsync()
//        {
//            var loginData = new
//            {
//                username = _configuration["Login:Username"],
//                password = _configuration["Login:Password"],
//            };

//            var content = new StringContent("{}", System.Text.Encoding.UTF8, "application/json");

//            var response = await _httpClient.PostAsync("https://stg-accounts-api.xcyber.vn/management/cyberid/login", content);
//            //Đọc nội dung response từ API
//            var responseContent = await response.Content.ReadAsStringAsync();
//            if (!response.IsSuccessStatusCode)
//                //nếu trạng thái HTTP không phải là 200 OK thì sẽ trả về exception với thông tin chi tiết lỗi
//                throw new Exception($"Lỗi gọi API: {(int)response.StatusCode}");


//            var result = JsonSerializer.Deserialize<TokenData>(responseContent, new JsonSerializerOptions
//            {
//                PropertyNameCaseInsensitive = true // Cho phép không phân biệt hoa thường
//            });

//            if (result == null)
//                throw new Exception("Không thể phân tích TokenData từ phản hồi");

//            return result;
//        }
//    }
//}