using System.Globalization;
using System.Net.Http.Headers;
using System.Text.Json;
using API_Project1.Interfaces;
using API_Project1.Models;
using Microsoft.Data.SqlClient;

namespace API_Project1.Services
{
    public class InvoiceDetailService: IInvoiceDetailService
    {
        private readonly ITokenService _tokenService;
        private readonly HttpClient _httpClient;
        private readonly IInvoiceListService _interfaceInvoiceList;

        public InvoiceDetailService(ITokenService tokenService, HttpClient httpClient, IInvoiceListService invoiceList)
        {
            _tokenService = tokenService;
            _httpClient = httpClient;
            _interfaceInvoiceList = invoiceList;
        }


        public async Task<string> GetInvoiceDetailAsync(int currentPage = 0)
        {
            var accessToken = await _tokenService.GetAccessTokenAsync();

            var maHoaDonList = await _interfaceInvoiceList.GetMaHoaDonListAsync(currentPage);           //lây danh sách mã hóa đơn, truyền currentPage vào như tham số vào hàm, trả về List<string> gồm các maHoaDon, user có thể truyền vào số trang để điều khiền dữ liệu cần lấy

            if (maHoaDonList == null || !maHoaDonList.Any()) 
            {
                throw new Exception("Không tìm thấy mã hóa đơn!");                                      //kiểm tra nếu list rỗng                                           
            }


            var detailList = new List<JsonElement>();                                                   //tạo danh sách rỗng để chứa các hóa đơn chi tiết dạng JSON

            foreach (var maHoaDon in maHoaDonList)                                                      //duyệt qua từng mã hóa đơn
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"https://dev-billstore.xcyber.vn/api/hddv-hoa-don/detail/{maHoaDon}");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);   //tạo request, gắn maHoaDon vào URL và access token vào header

                var response = await _httpClient.SendAsync(request);
                var content = await response.Content.ReadAsStringAsync();                               //gửi request, đọc response dạng string

                if (response.IsSuccessStatusCode)
                {
                    using var jsonDoc = JsonDocument.Parse(content);                                    //Parse JSON thành JsonDocument, lấy RootElement, clone lại vì JsonDocument bị dispose sau đó??? 
                    detailList.Add(jsonDoc.RootElement.Clone());                                        //add jsonDoc vào detailList
                }
                else
                {
                    Console.WriteLine($"Lỗi khi lấy chi tiết mã hóa đơn {maHoaDon}: {response.StatusCode}");
                }    
            }

            var finalSon = JsonSerializer.Serialize(detailList, new JsonSerializerOptions { WriteIndented = true });
            return finalSon;

        }

        public List<InvoiceDetailDataModel> ConvertJsonToInvoiceDetail(JsonElement data)
        {
            var options = new JsonSerializerOptions();
            {
                PropertyNameCaseInsensitive = true,
            };
            string jsonString = data.GetRawText();
            return JsonSerializer.Deserialize<List<InvoiceDetailDataModel>>(jsonString, options);
        }


        public async Task SaveDetailToDatabaseAsync(List<InvoiceDetailDataModel> invoiceDetails)
        {
            var connectionString = "Server=localhost\\SQLEXPRESS; Database=BILL_STORE; Trusted_Connection=True";


            using (var connection = new SqlConnection(connectionString)) 
            {
                await connection.OpenAsync();

                foreach (var invoiceDetail in invoiceDetails) 
                {
                    var insertHoaDonDetail = new SqlCommand(@"
                    
                    MERGE INTO INVOICE_DETAIL AS target
                    USING (SELECT 
                        @MaHoaDon as MaHoaDon, @MaLichSuFile as MaLichSuFile,
                        @TenHoaDon as TenHoaDon, @KiHieuMauSoHoaDon as KiHieuMauSoHoaDon, @SoHoaDon as SoHoaDOn,
                        @NgayLap as NgayLap, @NgayDuyet as NgayDuyet, @NgayNhan as NgayNhan, @NgayThanhToan as NgayThanhToan,
                        @NgayKy as NgayKy, @HoaDonDanhChoKhuPhiThueQuan as HoaDonDanhChoKhuPhiThueQuan, 
                        @DonViTienTe as DonViTienTe, @TiGia as TiGia, @HinhThucThanhToan as HinhThucThanhToan,
                        @MstToChucGiaiPhap as MstToChucGiaiPhap, @TenNguoiBan as TenNguoiBan, @MstNguoiBan as MstNguoiBan,
                        @DiaChiNguoiBan as DiaChiNguoiBan, @TenNguoiMua as TenNguoiMua, @MstNguoiMua as MstNguoiMua,
                        @DiaChiNguoiMua as DiaChiNguoiMua, @NhanHoaDon as NhanHoaDon, @GhiChu as GhiChu,
                        @TongTienChuaThue as TongTienChuaThue, @TongTienThue as TongTienThue,
                        @TongTienChietKhauThuongMai as TongTienChietKhauThuongMai, @TongTienThanhToanBangSo as TongTienThanhToanBangSo,
                        @TongTienThanhToanBangChu as TongTienThanhToanBangChu, @TrangThaiHoaDon as TrangThaiHoaDon,
                        @TrangThaiPheDuyet as TrangThaiPheDuyet, @LoaiHoaDon as LoaiHoaDon, @LyDoKhongDuyet as LyDoKhongDuyet,
                        @NguoiDuyet as NguoiDuyet, @PhuongThucNhap as PhuongThucNhap, @CheckTrangThaiXuLy as CheckTrangThaiXuLy,
                        @CheckTrangThaiHoaDon as CheckTrangThaiHoaDon, @CheckMstNguoiMua as CheckMstNguoiMua,
                        @CheckDiaChiNguoiMua as CheckDiaChiNguoiMua, @CheckTenNguoiMua as CheckTenNguoiMua,
                        @CheckHDonKyDienTu as CheckHDonKyDienTu, @KiemTraChungThu as KiemTraChungThu,
                        @KiemTraTenNban as KiemTraTenNban, @KiemTraMstNban as KiemTraMstNban, @KiemTraHoatDongNmua as KiemTraHoatDongNmua,
                        @KiemTraHoatDongNban as KiemTraHoatDongNban, @DsFileDinhKem as DsFileDinhKem, @FileExcel as FileExcel
                    ) AS source 
                    ON target.MaHoaDon = source.MaHoaDon

                    WHEN MATCHED THEN 
                        UPDATE SET 
                            MaLichSuFIle = source.MaLichSuFIle,
                            TenHoaDon = source.TenHoaDon,
                            KiHieuMauSoHoaDon = source.KiHieuMauSoHoaDon,
                            SoHoaDon = source.SoHoaDon,
                            NgayLap = source.NgayLap,
                            NgayDuyet = source.NgayDuyet,
                            NgayNhan = source.NgayNhan,
                            NgayThanhToan = source.NgayThanhToan,
                            NgayKy = source.NgayKy,
                            HoaDonDanhChoKhuPhiThueQuan = source.HoaDonDanhChoKhuPhiThueQuan,
                            DonViTienTe = source.DonViTienTe,
                            TiGia = source.TiGia,
                            HinhThucThanhToan = source.HinhThucThanhToan,
                            MstToChucGiaiPhap = source.MstToChucGiaiPhap,
                            TenNguoiBan = source.TenNguoiBan,
                            MstNguoiBan = source.MstNguoiBan,
                            DiaChiNguoiBan = source.DiaChiNguoiBan,
                            TenNguoiMua = source.TenNguoiMua,
                            MstNguoiMua = source.MstNguoiMua,
                            DiaChiNguoiMua = source.DiaChiNguoiMua,
                            NhanHoaDon = source.NhanHoaDon,
                            GhiChu = source.GhiChu,
                            TongTienChuaThue = source.TongTienChuaThue,
                            TongTienThue = source.TongTienThue,
                            TongTienChietKhauThuongMai = source.TongTienChietKhauThuongMai,
                            TongTienThanhToanBangSo = source.TongTienThanhToanBangSo,
                            TongTienThanhToanBangChu = source.TongTienThanhToanBangChu,
                            TrangThaiHoaDon = source.TrangThaiHoaDon,
                            TrangThaiPheDuyet = source.TrangThaiPheDuyet,
                            LoaiHoaDon = source.LoaiHoaDon,
                            LyDoKhongDuyet = source.LyDoKhongDuyet,
                            NguoiDuyet = source.NguoiDuyet,
                            PhuongThucNhap = source.PhuongThucNhap,
                            CheckTrangThaiXuLy = source.CheckTrangThaiXuLy,
                            CheckTrangThaiHoaDon = source.CheckTrangThaiHoaDon,
                            CheckMstNguoiMua = source.CheckMstNguoiMua,
                            CheckDiaChiNguoiMua = source.CheckDiaChiNguoiMua,
                            CheckTenNguoiMua = source.CheckTenNguoiMua,
                            CheckHDonKyDienTu = source.CheckHDonKyDienTu,
                            KiemTraChungThu = source.KiemTraChungThu,
                            KiemTraTenNban = source.KiemTraTenNban,
                            KiemTraMstNban = source.KiemTraMstNban,
                            KiemTraHoatDongNmua = source.KiemTraHoatDongNmua,
                            KiemTraHoatDongNban = source.KiemTraHoatDongNban,
                            DsFileDinhKem = source.DsFileDinhKem,
                            FileExcel = source.FileExcel

                    WHEN NOT MATCHED THEN 
                        INSERT (
                    MaHoaDon, MaLichSuFIle, TenHoaDon, KiHieuMauSoHoaDon, SoHoaDon, NgayLap, NgayDuyet, NgayNhan, NgayNhan, NgayThanhToan, NgayKy, HoaDonDanhChoKhuPhiThueQuan,
                    DonViTienTe, TiGia, HinhThucThanhToan, MstToChucGiaiPhap, 

"
                }
            }
        }
    }
}
