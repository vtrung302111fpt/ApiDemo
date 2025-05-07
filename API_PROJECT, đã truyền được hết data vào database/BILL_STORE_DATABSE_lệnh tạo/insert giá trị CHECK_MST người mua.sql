SELECT TOP (1000) [checkMstNguoiMua]
      ,[checkMstNguoiMuaName]
  FROM [BILL_STORE].[dbo].[CHECK_MST_NGUOI_MUA]
   insert into [CHECK_MST_NGUOI_MUA] ([checkMstNguoiMua]
      ,[checkMstNguoiMuaName])
	  vALUES (0, N'Sai'),
(1, N'Đúng')