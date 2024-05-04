USE TOPOS_DB 
IF OBJECT_ID('DBO.TTT_DanhMuc_Save', 'U') IS NOT NULL 
EXEC sp_rename '[dbo].[TTT_DanhMuc_Save]', 'TTT_DanhMuc'