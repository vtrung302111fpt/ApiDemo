SELECT TOP (1000) [checkTenNguoiMua]
      ,[checkTenNguoiMuaName]
  FROM [BILL_STORE].[dbo].[CHECK_TEN_NGUOI_MUA]



  insert into [CHECK_TEN_NGUOI_MUA] ([checkTenNguoiMua]
      ,[checkTenNguoiMuaName])
	  vALUES (0, N'Sai'),
(1, N'Đúng')