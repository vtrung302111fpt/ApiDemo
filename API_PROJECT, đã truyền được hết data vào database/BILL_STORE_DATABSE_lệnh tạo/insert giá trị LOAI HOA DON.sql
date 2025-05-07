SELECT TOP (1000) [loaiHoaDon]
      ,[loaiHoaDonName]
  FROM [BILL_STORE].[dbo].[LOAI_HOA_DON]
  INSERT INTO LOAI_HOA_DON (loaiHoaDon, loaiHoaDonName) VALUES
(1, N'Hóa đơn gốc'),
(2, N'Hóa đơn thay thế'),
(3, N'Hóa đơn bị thay thế'),
(4, N'Hóa đơn điều chỉnh'),
(5, N'Hóa đơn bị điều chỉnh'),
(6, N'Hóa đơn huỷ bỏ');
