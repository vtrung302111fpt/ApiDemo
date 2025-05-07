SELECT TOP (1000) [checkHoaDonKyDienTu]
      ,[checkHoaDonKyDienTuName]
  FROM [BILL_STORE].[dbo].[CHECK_HOA_DON_KY_DIEN_TU]
  INSERT INTO CHECK_HOA_DON_KY_DIEN_TU (checkHoaDonKyDienTu, checkHoaDonKyDienTuName) VALUES
(0, N'Chữ ký hợp lệ'),
(1, N'Không tìm thấy chữ ký số'),
(2, N'Dữ liệu trong thẻ đã bị thay đổi'),
(3, N'Chữ ký không hợp lệ'),
(4, N'Không ký đúng vị trí'),
(5, N'Không tìm thấy thẻ có ID'),
(6, N'Không tìm thấy thẻ Reference');
