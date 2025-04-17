using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MyApiProject.Models
{
    public class BillListResponse
    {
        [JsonPropertyName("status")]
        public int Status { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("data")]
        public List<BillData> Data { get; set; }

        [JsonPropertyName("totalData")]
        public int TotalData { get; set; }

        [JsonPropertyName("totalPage")]
        public int TotalPage { get; set; }

        [JsonPropertyName("currentPage")]
        public int CurrentPage { get; set; }

        [JsonPropertyName("tongTienThanhToan")]
        public string TongTienThanhToan { get; set; }

        [JsonPropertyName("tongTienTruocThue")]
        public string TongTienTruocThue { get; set; }

        [JsonPropertyName("tongTienThue")]
        public string TongTienThue { get; set; }
    }

    public class BillData
    {
        [JsonPropertyName("maHoaDon")]
        public string MaHoaDon { get; set; }

        [JsonPropertyName("maLichSuFile")]
        public string MaLichSuFile { get; set; }

        [JsonPropertyName("soHoaDon")]
        public int SoHoaDon { get; set; }

        [JsonPropertyName("loaiHoaDon")]
        public int LoaiHoaDon { get; set; }

        [JsonPropertyName("tenNCC")]
        public string TenNCC { get; set; }

        [JsonPropertyName("mstNCC")]
        public string MstNCC { get; set; }

        [JsonPropertyName("tongTien")]
        public string TongTien { get; set; }

        [JsonPropertyName("tienTruocThue")]
        public string TienTruocThue { get; set; }

        [JsonPropertyName("tienThue")]
        public string TienThue { get; set; }

        [JsonPropertyName("nhanHoaDon")]
        public string NhanHoaDon { get; set; } = string.Empty;

        [JsonPropertyName("trangThaiPheDuyet")]
        public int TrangThaiPheDuyet { get; set; }

        [JsonPropertyName("trangThaiHoaDon")]
        public int TrangThaiHoaDon { get; set; }

        [JsonPropertyName("soDonHang")]
        public string? SoDonHang { get; set; } = string.Empty;

        [JsonPropertyName("kiHieuMauSoHoaDon")]
        public int KiHieuMauSoHoaDon { get; set; }

        [JsonPropertyName("kiHieuHoaDon")]
        public string KiHieuHoaDon { get; set; }

        [JsonPropertyName("tinhChatHoaDon")]
        public int TinhChatHoaDon { get; set; }

        [JsonPropertyName("ngayLap")]
        public string NgayLap { get; set; }

        [JsonPropertyName("ngayNhan")]
        public string NgayNhan { get; set; }

        [JsonPropertyName("phuongThucNhap")]
        public int PhuongThucNhap { get; set; }

        [JsonPropertyName("ghiChu")]
        public GhiChu GhiChu { get; set; }
    }

    public class GhiChu
    {
        [JsonPropertyName("checkTrangThaiXuLy")]
        public int CheckTrangThaiXuLy { get; set; }

        [JsonPropertyName("checkTrangThaiHoaDon")]
        public int CheckTrangThaiHoaDon { get; set; }

        [JsonPropertyName("checkTenNguoiMua")]
        public string? CheckTenNguoiMua { get; set; }

        [JsonPropertyName("checkDiaChiNguoiMua")]
        public string? CheckDiaChiNguoiMua { get; set; }

        [JsonPropertyName("checkMstNguoiMua")]
        public string? CheckMstNguoiMua { get; set; }

        [JsonPropertyName("checkHoaDonKyDienTu")]
        public int CheckHoaDonKyDienTu { get; set; }
    }
}
