CREATE TABLE @ThanhToanHoaDon@(
	[MaHD] [nvarchar](50) NOT NULL,
	[MaHinhThuc] [nvarchar](20) NOT NULL,
	[MaNhomThanhToan] [nvarchar](20) NULL,
	[MaThe] [nvarchar](30) NULL,
	[ThanhTien] [numeric](19, 2) NULL,
	[TyGiaNgoaiTe] [numeric](19, 3) NULL,
	[ThanhTienQuiDoi] [numeric](19, 2) NULL,
	[TLFee] [numeric](19, 3) NULL,
	[DaVanChuyen] [bit] NULL,
	[ChuThe] [nvarchar](255) NULL,
	[RowID] [uniqueidentifier] NULL DEFAULT (newid()),
	
	PRIMARY KEY ([MaHD], [MaHinhThuc])
)	