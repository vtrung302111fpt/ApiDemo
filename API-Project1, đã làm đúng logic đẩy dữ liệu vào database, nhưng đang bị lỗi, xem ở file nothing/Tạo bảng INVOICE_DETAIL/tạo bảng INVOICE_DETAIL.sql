CREATE TABLE INVOICE_DETAIL (
    maHoaDon nvarchar(50) NOT NULL,
    maLichSuFile nvarchar(MAX) NOT NULL,
    tenHoaDon nvarchar(MAX) NOT NULL,
    kiHieuMauSoHoaDon int NOT NULL,
    kiHieuHoaDon nvarchar(MAX) NULL,
    soHoaDon nvarchar(MAX) NOT NULL,
    ngayLap datetime NULL,
    ngayDuyet datetime NULL,
    ngayNhan datetime  NULL,
    ngayThanhToan datetime  NULL,
    ngayKy datetime  NULL,
    hoaDonDanhChoKhuPhiThueQuan nvarchar(100) NULL,
    donViTienTe nvarchar(MAX) NULL,
    tiGia decimal(18, 1) NULL,
    hinhThucThanhToan nvarchar(MAX) NULL,
    mstToChucGiaiPhap nvarchar(MAX) NULL,
    tenNguoiBan nvarchar(MAX) NOT NULL,
    mstNguoiBan nvarchar(MAX) NOT NULL,
    diaChiNguoiBan nvarchar(MAX) NOT NULL,
    tenNguoiMua nvarchar(MAX) NOT NULL,
    mstNguoiMua nvarchar(MAX) NOT NULL,
    diaChiNguoiMua nvarchar(MAX) NOT NULL,
    nhanHoaDon nvarchar(MAX) NULL,
    ghiChu nvarchar(MAX) NULL,
    tongTienChuaThue nvarchar(MAX) NOT NULL,
    tongTienThue nvarchar(MAX) NOT NULL,
    tongTienChietKhauThuongMai nvarchar(MAX) NULL,
    tongTienThanhToanBangSo nvarchar(MAX) NOT NULL,
    tongTienThanhToanBangChu nvarchar(MAX) NOT NULL,
    trangThaiHoaDon int NOT NULL,
    trangThaiPheDuyet int NOT NULL,
    loaiHoaDon int NOT NULL,
    lyDoKhongDuyet nvarchar(MAX) NULL,
    nguoiDuyet nvarchar(MAX) NULL,
    phuongThucNhap int NOT NULL,
    checkTrangThaiXuLy int NOT NULL,
    checkTrangThaiHoaDon int NOT NULL,
    checkMstNguoiMua int NULL,
    checkDiaChiNguoiMua int NULL,
    checkTenNguoiMua int NULL,
    checkHDonKyDienTu int NULL,
    kiemTraChungThu int NOT NULL,
    kiemTraTenNban int NOT NULL,
    kiemTraMstNban int NOT NULL,
    kiemTraHoatDongNmua int NOT NULL,
    kiemTraHoatDongNban int NULL,
    dsFileDinhKem nvarchar(MAX) NULL,
    fileExcel nvarchar(MAX) NULL
);




CREATE TABLE DANH_SACH_THUE_SUAT (
    id int NOT NULL,
    maHoaDon nvarchar(50) NOT NULL,
    thueSuat nvarchar(MAX) NOT NULL,
    tienThue nvarchar(MAX) NOT NULL
);





CREATE TABLE DANH_SACH_HANG_HOA (
    id int NOT NULL,
    maHoaDon nvarchar(50) NOT NULL,
    khuyenMai nvarchar(MAX) NULL,
    stt int NOT NULL,
    tenHangHoa nvarchar(MAX) NOT NULL,
    donGia nvarchar(MAX) NOT NULL,
    loai nvarchar(MAX) NULL,
    donViTinh nvarchar(MAX) NOT NULL,
    soLuong nvarchar(MAX) NOT NULL,
    thanhTien nvarchar(MAX) NOT NULL,
    thueSuat nvarchar(MAX) NOT NULL,
    tienThue nvarchar(MAX) NOT NULL,
    checkSua nvarchar(MAX) NOT NULL
);


CREATE TABLE CHECK_TRANG_THAI_XU_LY (
    checkTrangThaiXuLy int NOT NULL,
    checkTrangThaiXuLyName nvarchar(MAX) NOT NULL
);

-- Bảng KIEM_TRA_CHUNG_THU
CREATE TABLE KIEM_TRA_CHUNG_THU (
    kiemTraChungThu int  NOT NULL,
    kiemTraChungThuName nvarchar(MAX) NOT NULL
);

-- Bảng KIEM_TRA_TEN_NGUOI_BAN
CREATE TABLE KIEM_TRA_TEN_NGUOI_BAN (
    kiemTraTenNban int  NOT NULL,
    kiemTraTenNbanName nvarchar(MAX) NOT NULL
);

-- Bảng KIEM_TRA_MST_NGUOI_BAN
CREATE TABLE KIEM_TRA_MST_NGUOI_BAN (
    kiemTraMstNban int  NOT NULL,
    kiemTraMstNbanName nvarchar(MAX) NOT NULL
);

-- Bảng KIEM_TRA_HOAT_DONG_NGUOI_BAN
CREATE TABLE KIEM_TRA_HOAT_DONG_NGUOI_BAN (
    kiemTraHoatDongNban int  NOT NULL,
    kiemTraHoatDongNbanName nvarchar(MAX) NOT NULL
);



  insert into KIEM_TRA_HOAT_DONG_NGUOI_BAN(kiemTraHoatDongNban,kiemTraHoatDongNbanName)
  values 
  ('0', N'Chưa kiểm tra'),
  ('1', N'Đúng'),
  ('2', N'Sai'),
  ('3', N'Lỗi khi kiểm tra')

  

  insert into KIEM_TRA_MST_NGUOI_BAN(kiemTraMstNban,kiemTraMstNbanName)
  values 
  ('0', N'Chưa kiểm tra'),
  ('1', N'Đúng'),
  ('2', N'Sai'),
  ('3', N'Lỗi khi kiểm tra')


  insert into KIEM_TRA_TEN_NGUOI_BAN(kiemTraTenNban, kiemTraTenNbanName)
  values 
  ('0', N'Chưa kiểm tra'),
  ('1', N'Đúng'),
  ('2', N'Sai'),
  ('3', N'Lỗi khi kiểm tra')


  insert into KIEM_TRA_CHUNG_THU(kiemTraChungThu,kiemTraChungThuName)
  values 
  ('0', N'Chưa kiểm tra'),
  ('1', N'Chứng thư số của NNT tại thời điểm ký hóa đơn hợp lệ'),
  ('2', N'Chứng thư số của NNT tại thời điểm ký hóa đơn không hợp lệ'),
  ('3', N'Lỗi khi kiểm tra')







    insert into CHECK_TRANG_THAI_XU_LY(checkTrangThaiXuLy, checkTrangThaiXuLyName)
  values 
  ('0', N'Tổng cục thuế đã nhận'),
  ('1', N'Đang kiểm tra điều kiện cấp mã'),
  ('2', N'Cơ quan thuế từ chối hóa đơn theo lần phát sinh'),
  ('3', N'Hóa đơn đủ điều kiện cấp mã'),
  ('4', N'Hóa đơn không đủ điều kiện cấp mã'),
  ('5', N'Đã cấp mã hóa đơn'),
  ('6', N'TCT đã nhận không mã'),
  ('7', N'Đã kiểm tra định ký hóa đơn điện tử không mã'),
  ('8', N'TCT đã nhận không mã máy tính tiền'),
  ('9', N'Đã kiểm tra định kỳ hóa đơn điện tử không có mã máy tính tiền')




Select * from INVOICE_DETAIL
Select * from DANH_SACH_HANG_HOA
Select * from DANH_SACH_THUE_SUAT
	delete froM INVOICE_DETAIL
	
	delete froM DANH_SACH_HANG_HOA

	delete froM DANH_SACH_THUE_SUAT