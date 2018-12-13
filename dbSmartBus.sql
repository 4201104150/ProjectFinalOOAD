create database SmartBus
go
use SmartBus
go
create table ThongTinKhachHang
(
	ID varchar(10) primary key,
	HoTen nvarchar(50) not null,
	NgaySinh date not null,
	GioiTinh nvarchar(10) not null,
	SDT varchar(12) not null,
	SoTienHienTai int not null,
	CheckHanhDong int
)

