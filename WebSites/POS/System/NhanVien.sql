UPDATE TOPOS_DB.dbo.NhanVien
SET TenNV = TenDangNhap

--NhanVien
INSERT INTO [Server-001-1].TOPOS_DB.dbo.NhanVien
SELECT * FROM TOPOS_DB.dbo.NhanVien
WHERE MaNV NOT IN (SELECT MaNV FROM [Server-001-1].TOPOS_DB.dbo.NhanVien)

--QuyenNhanVien
INSERT INTO [Server-001-1].TOPOS_DB.dbo.QuyenNhanVien
SELECT * FROM TOPOS_DB.dbo.QuyenNhanVien
WHERE (
(MaNV NOT IN (SELECT MaNV FROM [Server-001-1].TOPOS_DB.dbo.QuyenNhanVien))
--AND (MaNhomQuyen NOT IN (SELECT MaNhomQuyen FROM [Server-001-1].TOPOS_DB.dbo.QuyenNhanVien))
--AND (MaQuyen NOT IN (SELECT MaQuyen FROM [Server-001-1].TOPOS_DB.dbo.QuyenNhanVien))
)