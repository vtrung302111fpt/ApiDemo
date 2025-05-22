
CREATE TABLE CHECK_TRANG_THAI_XU_LY (
    checkTrangThaiXuLy decimal(18, 0) NOT NULL,
    checkTrangThaiXuLyName nvarchar(MAX) NOT NULL
);

-- Bảng KIEM_TRA_CHUNG_THU
CREATE TABLE KIEM_TRA_CHUNG_THU (
    kiemTraChungThu decimal(18, 0) NOT NULL,
    kiemTraChungThuName nvarchar(MAX) NOT NULL
);

-- Bảng KIEM_TRA_TEN_NGUOI_BAN
CREATE TABLE KIEM_TRA_TEN_NGUOI_BAN (
    kiemTraTenNban decimal(18, 0) NOT NULL,
    kiemTraTenNbanName nvarchar(MAX) NOT NULL
);

-- Bảng KIEM_TRA_MST_NGUOI_BAN
CREATE TABLE KIEM_TRA_MST_NGUOI_BAN (
    kiemTraMstNban decimal(18, 0) NOT NULL,
    kiemTraMstNbanName nvarchar(MAX) NOT NULL
);

-- Bảng KIEM_TRA_HOAT_DONG_NGUOI_BAN
CREATE TABLE KIEM_TRA_HOAT_DONG_NGUOI_BAN (
    kiemTraHoatDongNban decimal(18, 0) NOT NULL,
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

