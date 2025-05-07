SELECT TOP (1000) [checkTrangThaiHoaDon]
      ,[checkTrangThaiHoaDonName]
  FROM [BILL_STORE].[dbo].[CHECK_TRANG_THAI_HOA_DON]


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
