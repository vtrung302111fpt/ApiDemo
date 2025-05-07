SELECT TOP (1000) [trangThaiPheDuyet]
      ,[trangThaiPheDuyetName]
  FROM [BILL_STORE].[dbo].[TRANG_THAI_PHE_DUYET]
  INSERT INTO TRANG_THAI_PHE_DUYET (trangThaiPheDuyet, trangThaiPheDuyetName) VALUES
(1, N'Chờ duyệt'),
(2, N'Đã duyệt'),
(3, N'Không duyệt'),
(4, N'Đã hủy'),
(5, N'Đã xử lý');
