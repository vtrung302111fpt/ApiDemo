//using API_Project.Models;
//using System.Globalization;
//using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;



//namespace API_Project.Helper
//{
//    public static class InvoiceDatabaseHelper
//    {
//        public static async Task SaveToDatabaseAsync(List<BillListDataModel> bills)
//        {
//            //Kết nối đến SQL Server (đang là local)
//            //var connectionString = "Server=localhost\\SQLEXPRESS;Database=BILL_STORE_SAMPLE;Trusted_Connection=True;";

//            var connectionString = "Server=localhost\\SQLEXPRESS;Database=BILL_STORE_SAMPLE;Trusted_Connection=True;";
//            using (var connection = new SqlConnection(connectionString))
//            {
//                await connection.OpenAsync();

//                foreach (var bill in bills)
//                {
//                    // Insert vào bảng chính (INVOICE_LIST)
//                    var insertHoaDonCmd = new SqlCommand(@"
//                    INSERT INTO INVOICE_LIST (
//                        Id, MaHoaDon, MaLichSuFile, SoHoaDon, LoaiHoaDon, TenNCC,
//                        MstNCC, TongTien, TienTruocThue, TienThue, NhanHoaDon,
//                        TrangThaiPheDuyet, TrangThaiHoaDon, SoDonHang, KiHieuMauSoHoaDon,
//                        KiHieuHoaDon, TinhChatHoaDon, NgayLap, NgayNhan, PhuongThucNhap
//                    ) VALUES (
//                        @Id, @MaHoaDon, @MaLichSuFile, @SoHoaDon, @LoaiHoaDon, @TenNCC,
//                        @MstNCC, @TongTien, @TienTruocThue, @TienThue, @NhanHoaDon,
//                        @TrangThaiPheDuyet, @TrangThaiHoaDon, @SoDonHang, @KiHieuMauSoHoaDon,
//                        @KiHieuHoaDon, @TinhChatHoaDon, @NgayLap, @NgayNhan, @PhuongThucNhap
//                    )", connection);


//                    //gán giá trị thực tế của bill.id@Id vào @Id
//                    insertHoaDonCmd.Parameters.AddWithValue("@Id", bill.id);
//                    insertHoaDonCmd.Parameters.AddWithValue("@MaHoaDon", bill.maHoaDon);
//                    //Nếu bill.maHoaDon là null thì thay bằng khoảng ""
//                    insertHoaDonCmd.Parameters.AddWithValue("@MaLichSuFile", bill.maLichSuFile);
//                    insertHoaDonCmd.Parameters.AddWithValue("@SoHoaDon", bill.soHoaDon);
//                    insertHoaDonCmd.Parameters.AddWithValue("@LoaiHoaDon", bill.loaiHoaDon);
//                    insertHoaDonCmd.Parameters.AddWithValue("@TenNCC", bill.tenNCC);
//                    insertHoaDonCmd.Parameters.AddWithValue("@MstNCC", bill.mstNCC);
//                    //insertHoaDonCmd.Parameters.AddWithValue("@TrangThaiTct", bill.trangThaiTct);
//                    insertHoaDonCmd.Parameters.AddWithValue("@TongTien", bill.tongTien);
//                    insertHoaDonCmd.Parameters.AddWithValue("@TienTruocThue", bill.tienTruocThue);
//                    insertHoaDonCmd.Parameters.AddWithValue("@TienThue", bill.tienThue);
//                    insertHoaDonCmd.Parameters.AddWithValue("@NhanHoaDon", (object?)bill.nhanHoaDon ?? DBNull.Value);
//                    insertHoaDonCmd.Parameters.AddWithValue("@TrangThaiPheDuyet", bill.trangThaiPheDuyet);
//                    insertHoaDonCmd.Parameters.AddWithValue("@TrangThaiHoaDon", bill.trangThaiHoaDon);
//                    insertHoaDonCmd.Parameters.AddWithValue("@SoDonHang", (object?)bill.soDonHang ?? DBNull.Value);
//                    insertHoaDonCmd.Parameters.AddWithValue("@KiHieuMauSoHoaDon", bill.kiHieuMauSoHoaDon);
//                    insertHoaDonCmd.Parameters.AddWithValue("@KiHieuHoaDon", bill.kiHieuHoaDon);
//                    insertHoaDonCmd.Parameters.AddWithValue("@TinhChatHoaDon", bill.tinhChatHoaDon);

//                    var format = "dd/MM/yyyy HH:mm:ss";
//                    var culture = CultureInfo.InvariantCulture;

//                    //parse chuỗi ngày dạng như format thành type Datetype
//                    DateTime parsedNgayLap = DateTime.ParseExact(bill.ngayLap ?? throw new Exception("ngayLap is required"), format, culture);
//                    DateTime parsedNgayNhan = DateTime.ParseExact(bill.ngayNhan ?? throw new Exception("ngayNhan is required"), format, culture);

//                    insertHoaDonCmd.Parameters.AddWithValue("@NgayLap", parsedNgayLap);
//                    insertHoaDonCmd.Parameters.AddWithValue("@NgayNhan", parsedNgayNhan);


//                    insertHoaDonCmd.Parameters.AddWithValue("@PhuongThucNhap", bill.phuongThucNhap);

//                    //if (bill.ngayLap == null)
//                    //    throw new Exception($"Bill ID {bill.id} bị thiếu NgayLap, mà cột này không cho phép null.");
//                    //if (bill.ngayNhan == null)
//                    //    throw new Exception($"Bill ID {bill.id} bị thiếu NgayNhan, mà cột này không cho phép null.");


//                    foreach (SqlParameter param in insertHoaDonCmd.Parameters)
//                    {
//                        Console.WriteLine($"{param.ParameterName} = {param.Value} ({param.Value?.GetType()})");
//                    }

//                    await insertHoaDonCmd.ExecuteNonQueryAsync();

//                    // Insert vào bảng ghi chú (GHI_CHU)
//                    if (bill.ghiChu != null)
//                    {
//                        var insertGhiChuCmd = new SqlCommand(@"
//                        INSERT INTO GHI_CHU (
//                            ghiChu_id, checkTrangThaiXuLy, checkTrangThaiHoaDon,
//                            checkTenNguoiMua, checkDiaChiNguoiMua, checkMstNguoiMua,
//                            checkHoaDonKyDienTu
//                        ) VALUES (
//                            @Id, @TrangThaiXuLy, @TrangThaiHoaDon,
//                            @TenNguoiMua, @DiaChiNguoiMua, @MstNguoiMua,
//                            @HoaDonKyDienTu
//                        )", connection);

//                        insertGhiChuCmd.Parameters.AddWithValue("@Id", bill.id);
//                        insertGhiChuCmd.Parameters.AddWithValue("@TrangThaiXuLy", bill.ghiChu.checkTrangThaiXuLy);
//                        insertGhiChuCmd.Parameters.AddWithValue("@TrangThaiHoaDon", bill.ghiChu.checkTrangThaiHoaDon);
//                        insertGhiChuCmd.Parameters.AddWithValue("@TenNguoiMua", (object?)bill.ghiChu.checkTenNguoiMua ?? DBNull.Value);
//                        insertGhiChuCmd.Parameters.AddWithValue("@DiaChiNguoiMua", (object?)bill.ghiChu.checkDiaChiNguoiMua ?? DBNull.Value);
//                        insertGhiChuCmd.Parameters.AddWithValue("@MstNguoiMua", (object?)bill.ghiChu.checkMstNguoiMua ?? DBNull.Value);
//                        insertGhiChuCmd.Parameters.AddWithValue("@HoaDonKyDienTu", bill.ghiChu.checkHoaDonKyDienTu);

//                        await insertGhiChuCmd.ExecuteNonQueryAsync();
//                    }
//                }
//            }
//        }
//    }
//}

