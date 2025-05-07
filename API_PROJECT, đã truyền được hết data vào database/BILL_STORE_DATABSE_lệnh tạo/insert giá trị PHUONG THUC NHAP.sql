SELECT TOP (1000) [phuongThucNhap]
      ,[phuongThucNhapName]
  FROM [BILL_STORE].[dbo].[PHUONG_THUC_NHAP]
  INSERT INTO PHUONG_THUC_NHAP (phuongThucNhap, phuongThucNhapName) VALUES
(1, N'Nhập tay'),
(2, N'Import XML'),
(3, N'Lấy hóa đơn từ TCT'),
(4, N'Từ email'),
(5, N'Import PDF');
