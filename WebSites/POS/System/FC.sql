USE TOPOS_DB 
IF OBJECT_ID('DBO.TTT_DanhMuc_Save', 'U') IS NOT NULL 
EXEC sp_rename '[dbo].[TTT_DanhMuc_Save]', 'TTT_DanhMuc'

----------DELETE

DELETE TOPOS_DB.DBO.TTT_DanhMuc
DELETE TOPOS_DB.DBO.TTT_CTKichHoat
DELETE TOPOS_DB.DBO.TTT_CTKichHoat_NhomThanhToan
DELETE TOPOS_DB.DBO.TTT_CTNapTien
DELETE TOPOS_DB.DBO.TTT_CTNapTien_HinhThucThanhToan

----------SELECT

--TTT_DanhMuc
USE TOPOS_DB_181
IF OBJECT_ID('DBO.TTT_DanhMuc', 'U') IS NOT NULL 
DROP TABLE DBO.TTT_DanhMuc
GO

SELECT *
INTO TOPOS_DB_181.DBO.TTT_DanhMuc
FROM [Server-001-1].TOPOS_DB.DBO.TTT_DanhMuc

--TTT_CTKichHoat
USE TOPOS_DB_181
IF OBJECT_ID('DBO.TTT_CTKichHoat', 'U') IS NOT NULL 
DROP TABLE DBO.TTT_CTKichHoat
GO

SELECT *
INTO TOPOS_DB_181.DBO.TTT_CTKichHoat
FROM [Server-001-1].TOPOS_DB.DBO.TTT_CTKichHoat

--TTT_CTKichHoat_NhomThanhToan
USE TOPOS_DB_181
IF OBJECT_ID('DBO.TTT_CTKichHoat_NhomThanhToan', 'U') IS NOT NULL 
DROP TABLE DBO.TTT_CTKichHoat_NhomThanhToan
GO

SELECT *
INTO TOPOS_DB_181.DBO.TTT_CTKichHoat_NhomThanhToan
FROM [Server-001-1].TOPOS_DB.DBO.TTT_CTKichHoat_NhomThanhToan

--TTT_CTNapTien
USE TOPOS_DB_181
IF OBJECT_ID('DBO.TTT_CTNapTien', 'U') IS NOT NULL 
DROP TABLE DBO.TTT_CTNapTien
GO

SELECT *
INTO TOPOS_DB_181.DBO.TTT_CTNapTien
FROM [Server-001-1].TOPOS_DB.DBO.TTT_CTNapTien

--TTT_CTNapTien_HinhThucThanhToan
USE TOPOS_DB_181
IF OBJECT_ID('DBO.TTT_CTNapTien_HinhThucThanhToan', 'U') IS NOT NULL 
DROP TABLE DBO.TTT_CTNapTien_HinhThucThanhToan
GO

SELECT *
INTO TOPOS_DB_181.DBO.TTT_CTNapTien_HinhThucThanhToan
FROM [Server-001-1].TOPOS_DB.DBO.TTT_CTNapTien_HinhThucThanhToan

----------INSERT 

--TTT_DanhMuc
INSERT INTO TOPOS_DB.DBO.TTT_DanhMuc
SELECT * FROM TOPOS_DB_181.DBO.TTT_DanhMuc

--TTT_CTKichHoat
INSERT INTO TOPOS_DB.DBO.TTT_CTKichHoat
SELECT * FROM TOPOS_DB_181.DBO.TTT_CTKichHoat

--TTT_CTKichHoat_NhomThanhToan
SET IDENTITY_INSERT TOPOS_DB.DBO.TTT_CTKichHoat_NhomThanhToan ON

INSERT INTO TOPOS_DB.DBO.TTT_CTKichHoat_NhomThanhToan 
	  (CTKichHoatID, MaNhomThanhToan, TongTien, CTKichHoat_Chi_Tiet_ID)
SELECT CTKichHoatID, MaNhomThanhToan, TongTien, CTKichHoat_Chi_Tiet_ID
FROM TOPOS_DB_181.DBO.TTT_CTKichHoat_NhomThanhToan

SET IDENTITY_INSERT TOPOS_DB.DBO.TTT_CTKichHoat_NhomThanhToan OFF

--TTT_CTNapTien
SET IDENTITY_INSERT TOPOS_DB.DBO.TTT_CTNapTien ON

INSERT INTO TOPOS_DB.DBO.TTT_CTNapTien 
	  (CTKichHoatID, MaFoodCourt, STT, NgayNap, SoTien, TienTruocKhiNap, TienSauKhiNap, MaNVTao, HinhThuc, CTNapTien_ID)
SELECT CTKichHoatID, MaFoodCourt, STT, NgayNap, SoTien, TienTruocKhiNap, TienSauKhiNap, MaNVTao, HinhThuc, CTNapTien_ID
FROM TOPOS_DB_181.DBO.TTT_CTNapTien

SET IDENTITY_INSERT TOPOS_DB.DBO.TTT_CTNapTien OFF

--TTT_CTNapTien_HinhThucThanhToan
SET IDENTITY_INSERT TOPOS_DB.DBO.TTT_CTNapTien_HinhThucThanhToan ON

INSERT INTO TOPOS_DB.DBO.TTT_CTNapTien_HinhThucThanhToan 
	  (CTKichHoatID, STT, MaHinhThuc, MaNhomThanhToan, MaThe, ChuThe, ThanhTien, TyGiaNgoaiTe, ThanhTienQuyDoi, TLFee, HoaDonID, MaHD, TienTruocThanhToan, TienSauThanhToan, CTNapTien_Chi_Tiet_ID)
SELECT CTKichHoatID, STT, MaHinhThuc, MaNhomThanhToan, MaThe, ChuThe, ThanhTien, TyGiaNgoaiTe, ThanhTienQuyDoi, TLFee, HoaDonID, MaHD, TienTruocThanhToan, TienSauThanhToan, CTNapTien_Chi_Tiet_ID
FROM TOPOS_DB_181.DBO.TTT_CTNapTien_HinhThucThanhToan

SET IDENTITY_INSERT TOPOS_DB.DBO.TTT_CTNapTien_HinhThucThanhToan OFF