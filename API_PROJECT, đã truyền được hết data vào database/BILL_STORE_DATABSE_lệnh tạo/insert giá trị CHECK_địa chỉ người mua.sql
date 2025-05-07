SELECT TOP (1000) [checkDiaChiNguoiMua]
      ,[checkDiaChiNguoiMuaName]
  FROM [BILL_STORE].[dbo].[CHECK_DIA_CHI_NGUOI_MUA]
  insert into [CHECK_DIA_CHI_NGUOI_MUA] ([checkDiaChiNguoiMua]
      ,[checkDiaChiNguoiMuaName])
	  vALUES (0, N'Sai'),
(1, N'Đúng')