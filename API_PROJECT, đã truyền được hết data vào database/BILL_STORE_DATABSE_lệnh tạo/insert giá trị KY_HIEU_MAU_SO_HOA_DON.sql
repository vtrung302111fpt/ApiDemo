SELECT TOP (1000) [kiHieuMauSoHoaDon]
      ,[kiHieuMauSoHoaDonName]
  FROM [BILL_STORE].[dbo].[KI_HIEU_MAU_SO_HOA_DON]



  INSERT INTO KI_HIEU_MAU_SO_HOA_DON (kiHieuMauSoHoaDon, kiHieuMauSoHoaDonName) VALUES
(1, N'Hóa đơn giá trị gia tăng'),
(2, N'Hóa đơn bán hàng'),
(3, N'Hóa đơn bán tài sản công'),
(4, N'Hóa đơn bán hàng dự trữ quốc gia'),
(5, N'Hóa đơn khác (tem, vé,...)'),
(6, N'Phiếu xuất kho kiêm vận chuyển nội bộ');
