using API_Project.Models;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Globalization;
using System.Net.Http.Headers;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace API_Project.Endpoints
{
    public static class InvoiceListEndpoint
    {
        public static void MapInvoiceListEndpoint(this WebApplication app)
        {
            app.MapGet("/api/invoices", async (HttpContext context) =>
            {
                var client = new HttpClient();

                var authorizationHeader = context.Request.Headers["Authorization"].FirstOrDefault();
                var token = authorizationHeader?.Replace("Bearer ", "");

                var doanhnghiepma = context.Request.Headers["doanhnghiepma"].FirstOrDefault() ?? string.Empty;
                var userma = context.Request.Headers["userma"].FirstOrDefault() ?? string.Empty;

                var requestMessage = new HttpRequestMessage(HttpMethod.Get,
                    "https://dev-billstore.xcyber.vn/api/hddv-hoa-don/get-list?current=1&page=0&pageSize=10&size=10&trangThaiPheDuyet");

                if (!string.IsNullOrEmpty(token))
                {
                    requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
                if (!string.IsNullOrEmpty(doanhnghiepma))
                {
                    requestMessage.Headers.Add("doanhnghiepma", doanhnghiepma);
                }
                if (!string.IsNullOrEmpty(userma))
                {
                    requestMessage.Headers.Add("userma", userma);
                }

                HttpResponseMessage response;
                try
                {
                    response = await client.SendAsync(requestMessage);
                }
                catch (Exception ex)
                {
                    context.Response.StatusCode = 500;
                    await context.Response.WriteAsync($"Lỗi kết nối đến server: {ex.Message}");
                    return;                
                }

                //var responseContent = await response.Content.ReadAsStringAsync();
                var content = await response.Content.ReadAsStringAsync();
                context.Response.StatusCode = (int)response.StatusCode;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(content);

                try
                {
                    using var conn = new SqlConnection("Server=localhost\\SQLEXPRESS; Database=BILL_STORE; User Id=ssa; Password=123456; Trusted_Connection=True; TrustServerCertificate=True; Encrypt=True");
                    conn.Open();
                    Console.WriteLine("✅ Kết nối SQL thành công.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("❌ Lỗi kết nối SQL: " + ex.Message);
                }

                
                var result = JsonSerializer.Deserialize<BillListResponse>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                });

                if (result?.data != null)
                {
                    foreach (var item in result.data)
                    {
                        string json = JsonSerializer.Serialize(item, new JsonSerializerOptions { WriteIndented = true });
                        //Console.WriteLine(json);
                    }
                    await SaveToDatabaseSync(result.data);
                }
                //return result;

            });

        }




        public static async Task SaveToDatabaseSync(List<BillListDataModel> bills)
        {
            var connectionString = "Server = localhost\\SQLEXPRESS; Database = BILL_STORE; User Id = ssa; Password = 123456; Trusted_Connection = True; TrustServerCertificate = True; Encrypt = True";




            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                foreach (var bill in bills)
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


                    //gán giá trị thực tế của bill.id@Id vào @Id
                    insertHoaDonCmd.Parameters.AddWithValue("@Id", bill.id);
                    insertHoaDonCmd.Parameters.AddWithValue("@MaHoaDon", bill.maHoaDon);
                    //Nếu bill.maHoaDon là null thì thay bằng khoảng ""
                    insertHoaDonCmd.Parameters.AddWithValue("@MaLichSuFile", bill.maLichSuFile);
                    insertHoaDonCmd.Parameters.AddWithValue("@SoHoaDon", bill.soHoaDon);
                    insertHoaDonCmd.Parameters.AddWithValue("@LoaiHoaDon", bill.loaiHoaDon);
                    insertHoaDonCmd.Parameters.AddWithValue("@TenNCC", bill.tenNCC);
                    insertHoaDonCmd.Parameters.AddWithValue("@MstNCC", bill.mstNCC);
                    //insertHoaDonCmd.Parameters.AddWithValue("@TrangThaiTct", bill.trangThaiTct);
                    insertHoaDonCmd.Parameters.AddWithValue("@TongTien", bill.tongTien);
                    insertHoaDonCmd.Parameters.AddWithValue("@TienTruocThue", bill.tienTruocThue);
                    insertHoaDonCmd.Parameters.AddWithValue("@TienThue", bill.tienThue);
                    insertHoaDonCmd.Parameters.AddWithValue("@NhanHoaDon", (object?)bill.nhanHoaDon ?? DBNull.Value);
                    insertHoaDonCmd.Parameters.AddWithValue("@TrangThaiPheDuyet", bill.trangThaiPheDuyet);
                    insertHoaDonCmd.Parameters.AddWithValue("@TrangThaiHoaDon", bill.trangThaiHoaDon);
                    insertHoaDonCmd.Parameters.AddWithValue("@SoDonHang", (object?)bill.soDonHang ?? DBNull.Value);
                    insertHoaDonCmd.Parameters.AddWithValue("@KiHieuMauSoHoaDon", bill.kiHieuMauSoHoaDon);
                    insertHoaDonCmd.Parameters.AddWithValue("@KiHieuHoaDon", bill.kiHieuHoaDon);
                    insertHoaDonCmd.Parameters.AddWithValue("@TinhChatHoaDon", bill.tinhChatHoaDon);


                    var format = "dd/MM/yyyy HH:mm:ss";
                    var culture = CultureInfo.InvariantCulture;

                    DateTime parsedNgayLap = DateTime.ParseExact(bill.ngayLap ?? throw new Exception("ngayLap is required"), format, culture);
                    DateTime parsedNgayNhan = DateTime.ParseExact(bill.ngayNhan ?? throw new Exception("ngayLap is required"), format, culture);


                    insertHoaDonCmd.Parameters.AddWithValue("@NgayLap", parsedNgayLap);
                    insertHoaDonCmd.Parameters.AddWithValue("@NgayNhan", parsedNgayNhan);

                    insertHoaDonCmd.Parameters.AddWithValue("@PhuongThucNhap", bill.phuongThucNhap);


                    foreach (SqlParameter param in insertHoaDonCmd.Parameters)
                    {
                        Console.WriteLine($"{param.ParameterName} = {param.Value} ({param.Value?.GetType()})");
                    }

                    await insertHoaDonCmd.ExecuteNonQueryAsync();

                    // Insert vào bảng ghi chú (GHI_CHU)
                    if (bill.ghiChu != null)
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

                        insertGhiChuCmd.Parameters.AddWithValue("@Id", bill.id);
                        insertGhiChuCmd.Parameters.AddWithValue("@TrangThaiXuLy", bill.ghiChu.checkTrangThaiXuLy);
                        insertGhiChuCmd.Parameters.AddWithValue("@TrangThaiHoaDon", bill.ghiChu.checkTrangThaiHoaDon);
                        insertGhiChuCmd.Parameters.AddWithValue("@TenNguoiMua", (object?)bill.ghiChu.checkTenNguoiMua ?? DBNull.Value);
                        insertGhiChuCmd.Parameters.AddWithValue("@DiaChiNguoiMua", (object?)bill.ghiChu.checkDiaChiNguoiMua ?? DBNull.Value);
                        insertGhiChuCmd.Parameters.AddWithValue("@MstNguoiMua", (object?)bill.ghiChu.checkMstNguoiMua ?? DBNull.Value);
                        insertGhiChuCmd.Parameters.AddWithValue("@HoaDonKyDienTu", bill.ghiChu.checkHoaDonKyDienTu);

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
    }
}
    
















//Console.WriteLine($"Token: {token}");
//Console.WriteLine($"DoanhNghiepMa: {doanhnghiepma}");
//Console.WriteLine($"UserMa: {userma}");