using System.Globalization;
using System.Net.Http.Headers;
using System.Text.Json;
using API_Project1.Interfaces;
using API_Project1.Models;

//using API_Project1.Responses;k
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;


namespace API_Project1.Services
{
    public class InvoiceListService(IHttpClientFactory httpClientFactory, ITokenService tokenService, IUserInfoService userInfoService) : IInvoiceListService
    {
        private readonly HttpClient _httpClient = httpClientFactory.CreateClient();
        private readonly ITokenService _tokenService = tokenService;
        private readonly IUserInfoService _userInfoService = userInfoService;

        public async Task GetAllDataAsync()
        {
            int currentPage = 0;

            var firstResponse = await GetInvoiceListAsync(currentPage);                              // Lấy lần đầu tiên để biết totalPage
            using var firstDoc = JsonDocument.Parse(firstResponse);
            var root = firstDoc.RootElement;

            int totalPage = root.GetProperty("totalPage").GetInt32();

            var items = root.GetProperty("items");                                                  // Xử lý items trang đầu tiên
            Console.WriteLine($"Trang {currentPage + 1}/{totalPage}");
            foreach (var item in items.EnumerateArray())
            {
                // xử lý từng item
            }
            currentPage++;

            while (currentPage < totalPage)                                                         // Vòng lặp lấy các trang còn lại
            {
                var response = await GetInvoiceListAsync(currentPage);
                using var jsonDoc = JsonDocument.Parse(response);
                var pageRoot = jsonDoc.RootElement;

                var moreItems = pageRoot.GetProperty("items");
                Console.WriteLine($"Trang {currentPage + 1}/{totalPage}");
                foreach (var item in moreItems.EnumerateArray())
                {
                    // xử lý từng item
                }
                currentPage++;
            }
        }

        public async Task<List<string>> GetMaHoaDonListAsync(int currentPage = 0)           //hàm async (bất đồng bộ) trả về list string chứa các mã hóa đơn
        {
            var maHoaDonList = new List<string>();                                          //list rỗng để chứa các mã hóa đơn
            var json = await GetInvoiceListAsync(currentPage);                              //đợi các mã ở trang thứ currentPage, lưu response dạng chuỗi JSON vào biến 'json'
            using var doc = JsonDocument.Parse(json);                                       //phân tích json thành JsonDocument rồi truy cập nội dung chính của JSON
            var root = doc.RootElement;

            var dataArray = root.GetProperty("data");                                       //truy cập vào mảng dữ liệu chính trong JSON, property "data"
            foreach (var item in dataArray.EnumerateArray())                                //duyệt qua từng item trong data
            {
                if (item.TryGetProperty("maHoaDon", out var maHoaDonElement))               //kiểm tra trường maHoaDon, nếu có thì gán vào biến 'maHoaDonElement'
                {
                    string maHoaDon = maHoaDonElement.GetString();                          //lấy giá trị string của maHoaDon, 
                    if (!string.IsNullOrEmpty(maHoaDon))
                    {
                        maHoaDonList.Add(maHoaDon);                                         //giá trị không rỗng thì thêm vào danh sách maHoaDonList
                    }
                }
            }
            return maHoaDonList;
        }

        public async Task<string> GetInvoiceListAsync(int currentPage = 0)
        {

            var accessToken = await _tokenService.GetAccessTokenAsync();
            var (maNguoiDung, maDoanhNghiep) = await _userInfoService.GetUserAndCompanyCodeAsync();
            //lấy response từ hàm GetUserAndCompanyCodeAsync(), lưu vào userMa và doanhNghiepMa


            var request = new HttpRequestMessage(HttpMethod.Get, $"https://dev-billstore.xcyber.vn/api/hddv-hoa-don/get-list?current=1&page={currentPage}&pageSize=10&size=10&trangThaiPheDuyet");

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            request.Headers.Add("doanhnghiepma", maDoanhNghiep);
            request.Headers.Add("userma", maNguoiDung);
            request.Headers.AcceptCharset.Add(
                new StringWithQualityHeaderValue("utf-8")
            );

            var content = await _httpClient.SendAsync(request);
            var response = await content.Content.ReadAsStringAsync();

            

            return response;

        }

        public async Task<string> GetDataListAsync(int currentPage = 0)
        {
            var json = await GetInvoiceListAsync(currentPage);                              //đợi các mã ở trang thứ currentPage, lưu response dạng chuỗi JSON vào biến 'json'
            using var doc = JsonDocument.Parse(json);                                       //phân tích json thành JsonDocument rồi truy cập nội dung chính của JSON
            var root = doc.RootElement;

            var dataArray = root.GetProperty("data");
            return dataArray.GetRawText();
        }
        public async Task SaveListToDatabaseAsync(List<InvoiceListDataModel> invoices)
        {
            var connectionString = "Server=localhost\\SQLEXPRESS; Database=BILL_STORE; Trusted_Connection=True; Encrypt=False; TrustServerCertificate=False;";


            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                foreach (var invoice in invoices)
                {
                    var insertHoaDonCmd = new SqlCommand(@"

                    MERGE INTO INVOICE_LIST AS target
                    USING (SELECT 
                        @Id AS Id, @MaHoaDon AS MaHoaDon, @MaLichSuFile AS MaLichSuFile, 
                        @SoHoaDon AS SoHoaDon, @LoaiHoaDon AS LoaiHoaDon, @TenNCC AS TenNCC,
                        @MstNCC AS MstNCC, @TongTien AS TongTien, @TienTruocThue AS TienTruocThue, 
                        @TienThue AS TienThue, @NhanHoaDon AS NhanHoaDon,
                        @TrangThaiPheDuyet AS TrangThaiPheDuyet, @TrangThaiHoaDon AS TrangThaiHoaDon, 
                        @SoDonHang AS SoDonHang, @KiHieuMauSoHoaDon AS KiHieuMauSoHoaDon,
                        @KiHieuHoaDon AS KiHieuHoaDon, @TinhChatHoaDon AS TinhChatHoaDon, 
                        @NgayLap AS NgayLap, @NgayNhan AS NgayNhan, @PhuongThucNhap AS PhuongThucNhap
                    ) AS source
                    ON target.Id = source.Id

                    WHEN MATCHED THEN
                        UPDATE SET 
                            MaHoaDon = source.MaHoaDon,
                            MaLichSuFile = source.MaLichSuFile,
                            SoHoaDon = source.SoHoaDon,
                            LoaiHoaDon = source.LoaiHoaDon,
                            TenNCC = source.TenNCC,
                            MstNCC = source.MstNCC,
                            TongTien = source.TongTien,
                            TienTruocThue = source.TienTruocThue,
                            TienThue = source.TienThue,
                            NhanHoaDon = source.NhanHoaDon,
                            TrangThaiPheDuyet = source.TrangThaiPheDuyet,
                            TrangThaiHoaDon = source.TrangThaiHoaDon,
                            SoDonHang = source.SoDonHang,
                            KiHieuMauSoHoaDon = source.KiHieuMauSoHoaDon,
                            KiHieuHoaDon = source.KiHieuHoaDon,
                            TinhChatHoaDon = source.TinhChatHoaDon,
                            NgayLap = source.NgayLap,
                            NgayNhan = source.NgayNhan,
                            PhuongThucNhap = source.PhuongThucNhap

                    WHEN NOT MATCHED THEN
                        INSERT (
                            Id, MaHoaDon, MaLichSuFile, SoHoaDon, LoaiHoaDon, TenNCC,
                            MstNCC, TongTien, TienTruocThue, TienThue, NhanHoaDon,
                            TrangThaiPheDuyet, TrangThaiHoaDon, SoDonHang, KiHieuMauSoHoaDon,
                            KiHieuHoaDon, TinhChatHoaDon, NgayLap, NgayNhan, PhuongThucNhap
                        )
                        VALUES (
                            source.Id, source.MaHoaDon, source.MaLichSuFile, source.SoHoaDon, source.LoaiHoaDon, source.TenNCC,
                            source.MstNCC, source.TongTien, source.TienTruocThue, source.TienThue, source.NhanHoaDon,
                            source.TrangThaiPheDuyet, source.TrangThaiHoaDon, source.SoDonHang, source.KiHieuMauSoHoaDon,
                            source.KiHieuHoaDon, source.TinhChatHoaDon, source.NgayLap, source.NgayNhan, source.PhuongThucNhap
                        );

                    ", connection);



                    //gán giá trị thực tế của invoice.id@Id vào @Id
                    insertHoaDonCmd.Parameters.AddWithValue("@Id", invoice.id);
                    insertHoaDonCmd.Parameters.AddWithValue("@MaHoaDon", invoice.maHoaDon);
                    //Nếu invoice.maHoaDon là null thì thay bằng khoảng ""
                    insertHoaDonCmd.Parameters.AddWithValue("@MaLichSuFile", invoice.maLichSuFile);
                    insertHoaDonCmd.Parameters.AddWithValue("@SoHoaDon", invoice.soHoaDon);
                    insertHoaDonCmd.Parameters.AddWithValue("@LoaiHoaDon", invoice.loaiHoaDon);
                    insertHoaDonCmd.Parameters.AddWithValue("@TenNCC", invoice.tenNCC);
                    insertHoaDonCmd.Parameters.AddWithValue("@MstNCC", invoice.mstNCC);
                    //insertHoaDonCmd.Parameters.AddWithValue("@TrangThaiTct", invoice.trangThaiTct);
                    insertHoaDonCmd.Parameters.AddWithValue("@TongTien", invoice.tongTien);
                    insertHoaDonCmd.Parameters.AddWithValue("@TienTruocThue", invoice.tienTruocThue);
                    insertHoaDonCmd.Parameters.AddWithValue("@TienThue", invoice.tienThue);
                    insertHoaDonCmd.Parameters.AddWithValue("@NhanHoaDon", (object?)invoice.nhanHoaDon ?? DBNull.Value);
                    insertHoaDonCmd.Parameters.AddWithValue("@TrangThaiPheDuyet", invoice.trangThaiPheDuyet);
                    insertHoaDonCmd.Parameters.AddWithValue("@TrangThaiHoaDon", invoice.trangThaiHoaDon);
                    insertHoaDonCmd.Parameters.AddWithValue("@SoDonHang", (object?)invoice.soDonHang ?? DBNull.Value);
                    insertHoaDonCmd.Parameters.AddWithValue("@KiHieuMauSoHoaDon", invoice.kiHieuMauSoHoaDon);
                    insertHoaDonCmd.Parameters.AddWithValue("@KiHieuHoaDon", invoice.kiHieuHoaDon);
                    insertHoaDonCmd.Parameters.AddWithValue("@TinhChatHoaDon", invoice.tinhChatHoaDon);

                    var format = "dd/MM/yyyy HH:mm:ss";
                    var culture = CultureInfo.InvariantCulture;

                    DateTime parsedNgayLap = DateTime.ParseExact(invoice.ngayLap ?? throw new Exception("ngayLap is required"), format, culture);
                    //DateTime? parsedNgayNhan = DateTime.ParseExact(invoice.ngayNhan ?? throw new Exception("ngayNhan is required"), format, culture);

                    DateTime? parsedNgayNhan = null;
                    if (!string.IsNullOrEmpty(invoice.ngayNhan))
                    {
                        parsedNgayNhan = DateTime.ParseExact(invoice.ngayNhan, format, culture);
                    }

                    insertHoaDonCmd.Parameters.AddWithValue("@NgayLap", parsedNgayLap);
                    //insertHoaDonCmd.Parameters.AddWithValue("@NgayNhan", parsedNgayNhan);
                    insertHoaDonCmd.Parameters.AddWithValue("@NgayNhan", (object?)parsedNgayNhan ?? DBNull.Value);
                    insertHoaDonCmd.Parameters.AddWithValue("@PhuongThucNhap", invoice.phuongThucNhap);


                    foreach (SqlParameter param in insertHoaDonCmd.Parameters)
                    {
                        Console.WriteLine($"{param.ParameterName} = {param.Value} ({param.Value?.GetType()})");
                    }

                    await insertHoaDonCmd.ExecuteNonQueryAsync();




                    // Insert vào bảng ghi chú (GHI_CHU)
                    if (invoice.ghiChu != null)
                    {
                        var insertGhiChuCmd = new SqlCommand(@"
                        MERGE INTO GHI_CHU AS target
                            USING (SELECT 
                                @Id AS ghiChu_id, @TrangThaiXuLy AS checkTrangThaiXuLy, 
                                @TrangThaiHoaDon AS checkTrangThaiHoaDon, @TenNguoiMua AS checkTenNguoiMua,
                                @DiaChiNguoiMua AS checkDiaChiNguoiMua, @MstNguoiMua AS checkMstNguoiMua,
                                @HoaDonKyDienTu AS checkHoaDonKyDienTu
                            ) AS source
                            ON target.ghiChu_id = source.ghiChu_id

                            WHEN MATCHED THEN
                                UPDATE SET 
                                    checkTrangThaiXuLy = source.checkTrangThaiXuLy,
                                    checkTrangThaiHoaDon = source.checkTrangThaiHoaDon,
                                    checkTenNguoiMua = source.checkTenNguoiMua,
                                    checkDiaChiNguoiMua = source.checkDiaChiNguoiMua,
                                    checkMstNguoiMua = source.checkMstNguoiMua,
                                    checkHoaDonKyDienTu = source.checkHoaDonKyDienTu

                            WHEN NOT MATCHED THEN
                                INSERT (
                                    ghiChu_id, checkTrangThaiXuLy, checkTrangThaiHoaDon,
                                    checkTenNguoiMua, checkDiaChiNguoiMua, checkMstNguoiMua,
                                    checkHoaDonKyDienTu
                                )
                                VALUES (
                                    source.ghiChu_id, source.checkTrangThaiXuLy, source.checkTrangThaiHoaDon,
                                    source.checkTenNguoiMua, source.checkDiaChiNguoiMua, source.checkMstNguoiMua,
                                    source.checkHoaDonKyDienTu
                                );
                            ", connection);

                        insertGhiChuCmd.Parameters.AddWithValue("@Id", invoice.id);
                        insertGhiChuCmd.Parameters.AddWithValue("@TrangThaiXuLy", invoice.ghiChu.checkTrangThaiXuLy);
                        insertGhiChuCmd.Parameters.AddWithValue("@TrangThaiHoaDon", (object?)invoice.ghiChu.checkTrangThaiHoaDon ?? DBNull.Value);
                        insertGhiChuCmd.Parameters.AddWithValue("@TenNguoiMua", (object?)invoice.ghiChu.checkTenNguoiMua ?? DBNull.Value);
                        insertGhiChuCmd.Parameters.AddWithValue("@DiaChiNguoiMua", (object?)invoice.ghiChu.checkDiaChiNguoiMua ?? DBNull.Value);
                        insertGhiChuCmd.Parameters.AddWithValue("@MstNguoiMua", (object?)invoice.ghiChu.checkMstNguoiMua ?? DBNull.Value);
                        insertGhiChuCmd.Parameters.AddWithValue("@HoaDonKyDienTu", (object?)invoice.ghiChu.checkHoaDonKyDienTu ?? DBNull.Value);

                        await insertGhiChuCmd.ExecuteNonQueryAsync();
                    }


                    try
                    {
                        await insertHoaDonCmd.ExecuteNonQueryAsync();
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine($"❌ SQL Insert Error: {ex.Message}");
                    }


                }

            }

        }

        public List<InvoiceListDataModel> ConvertJsonToInvoiceList(JsonElement data)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            string jsonString = data.GetRawText();
            return JsonSerializer.Deserialize<List<InvoiceListDataModel>>(jsonString, options);
        }

    }
}
