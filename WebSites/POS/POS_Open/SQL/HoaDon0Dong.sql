CREATE TABLE @HoaDon0Dong@(
	[MaHD] [nvarchar](50) NULL,
	[STT] [bigint] NULL,
	[MaQuay] [nvarchar](20) NULL,
	[MaNV] [nvarchar](20) NULL,
	[TenNVBanHang] [nvarchar](255) NULL,
	[MaThue] [nvarchar](20) NULL,
	[MaPhieuTraHang] [nvarchar](255) NULL,
	[TenKhachHang] [nvarchar](255) NULL,
	[DienThoaiKhachHang] [nvarchar](255) NULL,
	[NgayBatDau] [datetime] NULL,
	[GioBatDau] [int] NULL,
	[NgayKetThuc] [datetime] NULL,
	[GioKetThuc] [int] NULL,
	[CaBan] [int] NULL,
	[DaIn] [bit] NULL,
	[MaHDGoc] [nvarchar](50) NULL,
	[MaHDShop] [nvarchar](50) NULL,
	[MaTheKHTT] [nvarchar](20) NULL,
	[TriGiaBan] [numeric](19, 4) NULL,
	[TienCK] [numeric](19, 2) NULL,
	[ThanhTienBan] [numeric](19, 2) NULL,
	[TienPhuThu] [numeric](19, 2) NULL,
	[LoaiHoaDon] [int] NULL,
	[TienTraKhach] [numeric](19, 2) NULL,
	[DaCapNhatTon] [bit] NULL,
	[DaKetChuyenCongNo] [bit] NULL,
	[DaVanChuyen] [bit] NULL,
	[ID] [nvarchar](50) NULL,
	[TLCK1] [numeric](19, 2) NULL,
	[TLCK2] [numeric](19, 2) NULL,
	[Lan_Ket_Ca] [bigint] NULL,
	[RowID] [uniqueidentifier] NULL
)