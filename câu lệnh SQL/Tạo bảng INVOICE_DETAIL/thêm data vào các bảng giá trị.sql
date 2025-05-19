
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


    insert into CHECK_HOA_DON_KY_DIEN_TU(checkHDonKyDienTu, checkHDonKyDienTuName)
  values 
  ('0', N'Chữ ký hợp lệ'),
  ('1', N'Không tìm thấy chữ ký số'),
  ('2', N'Dữ liệu trong thẻ đã bị thay đổi'),
  ('3', N'Chữ ký không hợp lệ'),
  ('4', N'Không ký đúng vị trí'),
  ('5', N'Không tìm thấy thẻ có ID'),
  ('6', N'Không tìm thấy thẻ Reference')



  insert into CHECK_TEN_NGUOI_MUA(checkTenNguoiMua, checkTenNguoiMuaName)
  values 
  ('0', N'Sai'),
  ('1', N'Đúng')


   insert into CHECK_DIA_CHI_NGUOI_MUA(checkDiaChiNguoiMua, checkDiaChiNguoiMuaName)
  values 
  ('0', N'Sai'),
  ('1', N'Đúng')


  insert into CHECK_MST_NGUOI_MUA(checkMstNguoiMua, checkMstNguoiMuaName)
  values 
  ('0', N'Sai'),
  ('1', N'Đúng')



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



  
    insert into PHUONG_THUC_NHAP(phuongThucNhap, phuongThucNhapName)
  values 
  ('1', N'Nhập tay'),
  ('2', N'Import XML'),
  ('3', N'Lấy hóa đơn từ TCT'),
  ('4', N'Từ Email'),
  ('5', N'Import PDF')


  insert into LOAI_HOA_DON(loaiHoaDon, loaiHoaDonName)
  values 
  ('1', N'Gốc'),
  ('2', N'Thay thế'),
  ('3', N'Điều chỉnh'),
  ('4', N'Bị thay thế'),
  ('5', N'Bị điều chỉnh'),
  ('6', N'Bị xóa hoặc hủy bỏ')


  insert into TRANG_THAI_PHE_DUYET(trangThaiPheDuyet, trangThaiPheDuyetName)
  values 
  ('1', N'Chờ duyệt'),
  ('2', N'Đã duyệt'),
  ('3', N'Không duyệt'),
  ('4', N'Đã hủy'),
  ('5', N'Đã xử lý')


  insert into TRANG_THAI_HOA_DON(trangThaiHoaDon, trangThaiHoaDonName)
  values 
  ('1', N'Đã tạo hóa đơn'),
  ('2', N'Cần nhập liệu'),
  ('3', N'Hóa đơn trùng'),
  ('5', N'Xóa hóa đơn')


   insert into KI_HIEU_MAU_SO_HOA_DON(kiHieuMauSoHoaDon, kiHieuMauSoHoaDonName)
  values 
  ('1', N'Hóa đơn giá trị gia tăng'),
  ('2', N'Hóa đơn bán hàng'),
  ('3', N'Hóa đơn bán tài sản công'),
  ('4', N'Hóa đơn bán hàng dự trữ quốc gia'),
  ('5', N'Hóa đơn khác (tem, vé,...)'),
  ('6', N'Phiếu xuất kho kiêm vận chuyển nội bộ')