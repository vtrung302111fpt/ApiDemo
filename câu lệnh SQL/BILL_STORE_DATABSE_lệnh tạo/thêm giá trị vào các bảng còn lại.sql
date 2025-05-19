
  insert into [CHECK_DIA_CHI_NGUOI_MUA] ([checkDiaChiNguoiMua]
      ,[checkDiaChiNguoiMuaName])
vALUES 
(0, N'Sai'),
(1, N'Đúng')


  INSERT INTO CHECK_HOA_DON_KY_DIEN_TU (checkHoaDonKyDienTu, checkHoaDonKyDienTuName) VALUES
(0, N'Chữ ký hợp lệ'),
(1, N'Không tìm thấy chữ ký số'),
(2, N'Dữ liệu trong thẻ đã bị thay đổi'),
(3, N'Chữ ký không hợp lệ'),
(4, N'Không ký đúng vị trí'),
(5, N'Không tìm thấy thẻ có ID'),
(6, N'Không tìm thấy thẻ Reference');


insert into [CHECK_MST_NGUOI_MUA] ([checkMstNguoiMua]
      ,[checkMstNguoiMuaName])
	  vALUES (0, N'Sai'),
(1, N'Đúng')


insert into [CHECK_TEN_NGUOI_MUA] ([checkTenNguoiMua]
      ,[checkTenNguoiMuaName])
	  vALUES (0, N'Sai'),
(1, N'Đúng')


INSERT INTO CHECK_TRANG_THAI_HOA_DON (checkTrangThaiHoaDon, checkTrangThaiHoaDonName) VALUES
(0, N'Tổng cục thuế đã nhận'),
(1, N'Đang kiểm tra điều kiện cấp mã'),
(2, N'Cơ quan thuế từ chối hóa đơn theo lần phát sinh'),
(3, N'Hóa đơn đủ điều kiện cấp mã'),
(4, N'Hóa đơn không đủ điều kiện cấp mã'),
(5, N'Đã cấp mã hóa đơn'),
(6, N'TCT đã nhận không mã'),
(7, N'Đã kiểm tra định kỳ hóa đơn điện tử không mã'),
(8, N'TCT đã nhận không mã tính tiền'),
(9, N'Đã kiểm tra định kỳ hóa đơn điện tử không mã máy tính tiền');



  INSERT INTO KI_HIEU_MAU_SO_HOA_DON (kiHieuMauSoHoaDon, kiHieuMauSoHoaDonName) VALUES
(1, N'Hóa đơn giá trị gia tăng'),
(2, N'Hóa đơn bán hàng'),
(3, N'Hóa đơn bán tài sản công'),
(4, N'Hóa đơn bán hàng dự trữ quốc gia'),
(5, N'Hóa đơn khác (tem, vé,...)'),
(6, N'Phiếu xuất kho kiêm vận chuyển nội bộ');



  INSERT INTO LOAI_HOA_DON (loaiHoaDon, loaiHoaDonName) VALUES
(1, N'Hóa đơn gốc'),
(2, N'Hóa đơn thay thế'),
(3, N'Hóa đơn bị thay thế'),
(4, N'Hóa đơn điều chỉnh'),
(5, N'Hóa đơn bị điều chỉnh'),
(6, N'Hóa đơn huỷ bỏ');



  INSERT INTO PHUONG_THUC_NHAP (phuongThucNhap, phuongThucNhapName) VALUES
(1, N'Nhập tay'),
(2, N'Import XML'),
(3, N'Lấy hóa đơn từ TCT'),
(4, N'Từ email'),
(5, N'Import PDF');



  INSERT INTO TINH_CHAT_HOA_DON (tinhChatHoaDon, tinhChatHoaDonName) VALUES
(1, N'Hóa đơn gốc'),
(2, N'Hóa đơn thay thế'),
(3, N'Hóa đơn bị thay thế'),
(4, N'Hóa đơn điều chỉnh'),
(5, N'Hóa đơn bị điều chỉnh'),
(6, N'Hóa đơn hủy bỏ');



  INSERT INTO TRANG_THAI_HOA_DON (trangThaiHoaDon, trangThaiHoaDonName) VALUES
(1, N'Đã tạo hóa đơn'),
(2, N'Cần nhập liệu'),
(3, N'Hóa đơn trùng'),
(5, N'Xóa hóa đơn');




  INSERT INTO TRANG_THAI_PHE_DUYET (trangThaiPheDuyet, trangThaiPheDuyetName) VALUES
(1, N'Chờ duyệt'),
(2, N'Đã duyệt'),
(3, N'Không duyệt'),
(4, N'Đã hủy'),
(5, N'Đã xử lý');

