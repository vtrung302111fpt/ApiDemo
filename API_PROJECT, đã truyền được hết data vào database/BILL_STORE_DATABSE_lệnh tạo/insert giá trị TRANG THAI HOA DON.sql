SELECT TOP (1000) [trangThaiHoaDon]
      ,[trangThaiHoaDonName]
  FROM [BILL_STORE].[dbo].[TRANG_THAI_HOA_DON]
  INSERT INTO TRANG_THAI_HOA_DON (trangThaiHoaDon, trangThaiHoaDonName) VALUES
(1, N'Đã tạo hóa đơn'),
(2, N'Cần nhập liệu'),
(3, N'Hóa đơn trùng'),
(5, N'Xóa hóa đơn');
