SELECT TOP (1000) [tinhChatHoaDon]
      ,[tinhChatHoaDonName]
  FROM [BILL_STORE].[dbo].[TINH_CHAT_HOA_DON]
  INSERT INTO TINH_CHAT_HOA_DON (tinhChatHoaDon, tinhChatHoaDonName) VALUES
(1, N'Hóa đơn gốc'),
(2, N'Hóa đơn thay thế'),
(3, N'Hóa đơn bị thay thế'),
(4, N'Hóa đơn điều chỉnh'),
(5, N'Hóa đơn bị điều chỉnh'),
(6, N'Hóa đơn hủy bỏ');
