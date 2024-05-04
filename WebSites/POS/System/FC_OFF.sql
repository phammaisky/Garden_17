USE TOPOS_DB 
IF OBJECT_ID('DBO.TTT_DanhMuc', 'U') IS NOT NULL 
EXEC sp_rename '[dbo].[TTT_DanhMuc]', 'TTT_DanhMuc_Save'