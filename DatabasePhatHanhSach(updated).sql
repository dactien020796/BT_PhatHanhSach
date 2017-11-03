create database PhatHanhSach
use PhatHanhSach

drop database PhatHanhSach
create table SACH
(
	MaSach int identity(1,1),
	TenSach nvarchar(100),
	TacGia nvarchar(100),
	LinhVuc nvarchar(30),
	DonGiaNhap int,
	DonGiaXuat int,
	GhiChu nvarchar(50),
	HinhAnh varchar(50),
	TrangThai bit,
	primary key (MaSach)
)
create table NHAXUATBAN
(
	MaNXB int identity(1,1),
	Ten nvarchar(30),
	DiaChi nvarchar(100),
	SoDT varchar(30),
	SoTK varchar(30),
	TrangThai bit,
	primary key (MaNXB)
)
create table DAILY
(
	MaDL int identity(1,1),
	Ten nvarchar(30),
	DiaChi nvarchar(50),
	SoDT varchar(30),
	TrangThai bit,
	primary key (MaDL)
)
create table PHIEUNHAP
(
	MaPN int identity(1,1),
	NguoiGiao int,
	NgayNhap datetime,
	MaNXB int,
	TongTien int,
	TrangThai bit,
	primary key (MaPN),
	foreign key (MaNXB) references NHAXUATBAN(MaNXB)
)
create table CT_PHIEUNHAP
(
	IdPN int not null,
	MaPN int,
	MaSach int,
	SLNhap int,
	DonGia int,
	ThanhTien int,
	primary key (IdPN),
	foreign key (MaPN) references PHIEUNHAP(MaPN),
	foreign key (MaSach) references SACH(MaSach)
)
create table PHIEUXUAT
(
	MaPX int identity(1,1),
	NguoiNhan int ,
	NgayXuat datetime,
	MaDL int,
	TongTien int,
	TrangThai bit,
	primary key (MaPX),
	foreign key (MaDL) references DAILY(MaDL)
)
 create table CT_PHIEUXUAT
(
MaPX int,
MaSach int,
SLXuat int,
DonGia int,
ThanhTien int,
primary key (MaPX,MaSach),
foreign key (MaPX) references PHIEUXUAT(MaPX),
foreign key (MaSach) references SACH(MaSach)
)
drop table BAOCAODL
create table BAOCAODL
(
	MaBCDL int identity(1,1),
	MaDL int,
	NgayLap datetime,
	NgayBD datetime,
	NgayKT datetime,
	ThanhToan int, 
	TinhTrang bit,
	primary key (MaBCDL),
	foreign key (MaDL) references DAILY(MaDL)
)
create table CT_BAOCAODL
(
MaBCDL int,
MaSach int,
SoLuongBan int,
DonGiaBan int,
ThanhTien int,
primary key(MaBCDL,MaSach),
foreign key (MaBCDL) references BAOCAODL(MaBCDL),
foreign key (MaSach) references SACH(MaSach)
)
create table DOANHSO
(
	MaDS int identity(1,1),
	MaNXB int,
	ThoiGian datetime,
	ThanhToan int, 
	TrangThai bit,
	primary key (MaDS),
	foreign key (MaNXB) references NHAXUATBAN(MaNXB)
)
create table CT_DOANHSO
(
MaDS int,
MaSach int,
SoLuongBan int,
DonGiaNhap int,
ThanhTien int,
primary key (MaDS,MaSach),
foreign key (MaDS) references DOANHSO(MaDS),
foreign key (MaSach) references SACH(MaSach)
)
create table TONKHO
(
	MaSach int identity(1,1),
	ThoiGian datetime,
	SLTon int,
	primary key (MaSach, ThoiGian),
	foreign key (MaSach) references SACH(MaSach)
)
create table CONGNO_NXB
(
	MaNXB int not null,
	ThoiGian datetime,
	TienNo int,
	TienDaTra int,
	primary key (MaNXB, ThoiGian),
	foreign key (MaNXB) references NHAXUATBAN(MaNXB)
)
create table CONGNO_DL
(
	MaDL int not null,
	ThoiGian datetime,
	TienNo int,
	TienDaTra int,
	primary key (MaDL, ThoiGian),
	foreign key (MaDL) references DAILY(MaDL)
)
-- ===================================================================

create table CHUCVU
(
	MaCV int identity(1,1),
	Ten nvarchar(30),
	GhiChu nvarchar(100),
	TrangThai bit,
	primary key (MaCV)
)

create table NGUOIDUNG
(
	MaND int identity(1,1),
	HoTen nvarchar(100),
	Pass varchar(20),
	Email varchar(30),
	SoDT varchar(30),
	MaCV int,
	TrangThai bit,
	primary key (MaND),
	foreign key (MaCV) references CHUCVU(MaCV)
)
-- ========================================================================

SELECT * FROM SACH
INSERT INTO SACH VALUES (N'Cơ sở dữ liệu mờ và xác suất', N'Nguyễn Tuệ', N'Cơ sở dữ liệu', 35000, 45000, N'','1.jpg',1 )
INSERT INTO SACH VALUES (N'Thủ thuật Windows XP', N'Lê Xuân Đồng', N'Hệ điều hành', 10000, 20000, N'', '2.jpg',1 )
INSERT INTO SACH VALUES (N'Nâng cấp và sửa chữa phần cứng', N'Nguyễn Dương Hà', N'Phần cứng', 45000, 55000, N'', '3.jpg', 1 )
INSERT INTO SACH VALUES (N'Photoshop toàn tập', N'Phạm Quang Huy', N'Đồ họa', 80000, 95000, N'', '4.png', 1  )
INSERT INTO SACH VALUES (N'Thiết kế kiến trúc 3D', N'Trần Quang Minh', N'Đồ họa', 65000, 75000, N'', '5.jpg', 1 )