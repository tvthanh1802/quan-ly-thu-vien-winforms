/*
    CSDL QUẢN LÝ THƯ VIỆN EPU - 200 SÁCH, ẢNH BÌA VÀ AVATAR ICON
    Phù hợp nghiệp vụ:
      - Quản lý nhân viên, độc giả, thẻ độc giả và đóng phí thường niên.
      - Tách đầu sách (Sach) và cuốn sách vật lý (CuonSach).
      - Mỗi cuốn sách có vị trí riêng.
      - Mượn tối đa 3 cuốn, không trùng đầu sách trong cùng phiếu.
      - Không cho mượn khi thẻ hết hạn/chưa đóng phí/còn sách quá hạn.
      - Trả sách và lập phạt chi tiết cho từng cuốn.
      - Thống kê sách mượn, quá hạn, mất, hỏng.

    CẢNH BÁO: Script này xóa các bảng cũ trong database QuanLyThuVienEPU.
    Hãy sao lưu dữ liệu trước khi chạy.
*/

IF DB_ID(N'QuanLyThuVienEPU') IS NULL
BEGIN
    CREATE DATABASE QuanLyThuVienEPU;
END
GO

USE QuanLyThuVienEPU;
GO

SET QUOTED_IDENTIFIER ON;
SET ANSI_NULLS ON;
GO

/* =========================================================
   0. XÓA CẤU TRÚC CŨ THEO THỨ TỰ PHỤ THUỘC
   ========================================================= */
DROP TABLE IF EXISTS PhanQuyen;
DROP TABLE IF EXISTS ChucNang;
DROP TABLE IF EXISTS ChiTietPhat;
DROP TABLE IF EXISTS PhieuPhat;
DROP TABLE IF EXISTS ChiTietTra;
DROP TABLE IF EXISTS PhieuTra;
DROP TABLE IF EXISTS ChiTietMuon;
DROP TABLE IF EXISTS PhieuMuon;
DROP TABLE IF EXISTS DatTruocSach;
DROP TABLE IF EXISTS DanhGiaSach;
DROP TABLE IF EXISTS ThongBao;
DROP TABLE IF EXISTS LoaiThongBao;
DROP TABLE IF EXISTS ChiTietNhap;
DROP TABLE IF EXISTS PhieuNhap;
DROP TABLE IF EXISTS NhaCungCap;
DROP TABLE IF EXISTS CuonSach;
DROP TABLE IF EXISTS Sach_TacGia;
DROP TABLE IF EXISTS Sach;
DROP TABLE IF EXISTS ViTriSach;
DROP TABLE IF EXISTS TheLoai;
DROP TABLE IF EXISTS TacGia;
DROP TABLE IF EXISTS NhaXuatBan;
DROP TABLE IF EXISTS DongPhiThuongNien;
DROP TABLE IF EXISTS TheDocGia;
DROP TABLE IF EXISTS NhatKyHeThong;
DROP TABLE IF EXISTS TaiKhoan;
DROP TABLE IF EXISTS VaiTro;
DROP TABLE IF EXISTS NhanVien;
DROP TABLE IF EXISTS DocGia;
DROP TABLE IF EXISTS Lop;
DROP TABLE IF EXISTS Khoa;
DROP TABLE IF EXISTS QuyDinh;
GO

/* =========================================================
   1. TỔ CHỨC - NGƯỜI DÙNG
   ========================================================= */
CREATE TABLE Khoa (
    MaKhoa       INT IDENTITY(1,1) PRIMARY KEY,
    TenKhoa      NVARCHAR(100) NOT NULL,
    TrangThai    BIT NOT NULL CONSTRAINT DF_Khoa_TrangThai DEFAULT 1,
    CONSTRAINT UQ_Khoa_TenKhoa UNIQUE (TenKhoa)
);
GO

CREATE TABLE Lop (
    MaLop        INT IDENTITY(1,1) PRIMARY KEY,
    TenLop       NVARCHAR(50) NOT NULL,
    MaKhoa       INT NOT NULL,
    KhoaHoc      NVARCHAR(20) NULL,
    TrangThai    BIT NOT NULL CONSTRAINT DF_Lop_TrangThai DEFAULT 1,
    CONSTRAINT FK_Lop_Khoa FOREIGN KEY (MaKhoa) REFERENCES Khoa(MaKhoa),
    CONSTRAINT UQ_Lop_TenLop_MaKhoa UNIQUE (TenLop, MaKhoa)
);
GO

CREATE TABLE DocGia (
    MaDocGia     INT IDENTITY(1,1) PRIMARY KEY,
    MaSinhVien   NVARCHAR(20) NULL,
    HoTen        NVARCHAR(100) NOT NULL,
    GioiTinh     NVARCHAR(10) NULL,
    NgaySinh     DATE NULL,
    SoDienThoai  NVARCHAR(15) NULL,
    Email        NVARCHAR(100) NULL,
    DiaChi       NVARCHAR(255) NULL,
    AnhDaiDien   NVARCHAR(255) NULL,
    MaLop        INT NULL,
    LoaiDocGia   NVARCHAR(30) NOT NULL,
    NgayDangKy   DATE NOT NULL CONSTRAINT DF_DocGia_NgayDangKy DEFAULT CAST(GETDATE() AS DATE),
    TrangThai    BIT NOT NULL CONSTRAINT DF_DocGia_TrangThai DEFAULT 1,
    CONSTRAINT FK_DocGia_Lop FOREIGN KEY (MaLop) REFERENCES Lop(MaLop),
    CONSTRAINT CK_DocGia_GioiTinh CHECK (GioiTinh IS NULL OR GioiTinh IN (N'Nam', N'Nữ', N'Khác')),
    CONSTRAINT CK_DocGia_Loai CHECK (LoaiDocGia IN (N'Sinh viên', N'Giảng viên', N'Nhân viên', N'Khác'))
);
GO

CREATE UNIQUE INDEX UX_DocGia_MaSinhVien
    ON DocGia(MaSinhVien)
    WHERE MaSinhVien IS NOT NULL;
GO

CREATE UNIQUE INDEX UX_DocGia_Email
    ON DocGia(Email)
    WHERE Email IS NOT NULL;
GO

CREATE TABLE NhanVien (
    MaNhanVien   INT IDENTITY(1,1) PRIMARY KEY,
    MaNhanVienHienThi AS (N'NV' + RIGHT(N'00000' + CONVERT(NVARCHAR(10), MaNhanVien), 5)) PERSISTED,
    HoTen        NVARCHAR(100) NOT NULL,
    GioiTinh     NVARCHAR(10) NULL,
    NgaySinh     DATE NULL,
    ChucVu       NVARCHAR(50) NOT NULL,
    SoDienThoai  NVARCHAR(15) NULL,
    Email        NVARCHAR(100) NULL,
    DiaChi       NVARCHAR(255) NULL,
    AnhDaiDien   NVARCHAR(255) NULL,
    NgayVaoLam   DATE NOT NULL CONSTRAINT DF_NhanVien_NgayVaoLam DEFAULT CAST(GETDATE() AS DATE),
    TrangThai    BIT NOT NULL CONSTRAINT DF_NhanVien_TrangThai DEFAULT 1,
    CONSTRAINT CK_NhanVien_GioiTinh CHECK (GioiTinh IS NULL OR GioiTinh IN (N'Nam', N'Nữ', N'Khác'))
);
GO

CREATE UNIQUE INDEX UX_NhanVien_Email
    ON NhanVien(Email)
    WHERE Email IS NOT NULL;
GO

CREATE TABLE VaiTro (
    MaVaiTro     INT IDENTITY(1,1) PRIMARY KEY,
    TenVaiTro    NVARCHAR(50) NOT NULL,
    MoTa         NVARCHAR(255) NULL,
    CONSTRAINT UQ_VaiTro_TenVaiTro UNIQUE (TenVaiTro)
);
GO

CREATE TABLE TaiKhoan (
    MaTaiKhoan   INT IDENTITY(1,1) PRIMARY KEY,
    TenDangNhap  NVARCHAR(50) NOT NULL,
    MatKhau      NVARCHAR(255) NOT NULL,
    MaNhanVien   INT NULL,
    MaDocGia     INT NULL,
    MaVaiTro     INT NOT NULL,
    LanDangNhapCuoi DATETIME2 NULL,
    SoLanDangNhapSai INT NOT NULL CONSTRAINT DF_TaiKhoan_SoLanDangNhapSai DEFAULT 0,
    KhoaDen       DATETIME2 NULL,
    TrangThai    BIT NOT NULL CONSTRAINT DF_TaiKhoan_TrangThai DEFAULT 1,
    CONSTRAINT UQ_TaiKhoan_TenDangNhap UNIQUE (TenDangNhap),
    CONSTRAINT FK_TaiKhoan_NhanVien FOREIGN KEY (MaNhanVien) REFERENCES NhanVien(MaNhanVien),
    CONSTRAINT FK_TaiKhoan_DocGia FOREIGN KEY (MaDocGia) REFERENCES DocGia(MaDocGia),
    CONSTRAINT FK_TaiKhoan_VaiTro FOREIGN KEY (MaVaiTro) REFERENCES VaiTro(MaVaiTro),
    CONSTRAINT CK_TaiKhoan_ChuSoHuu CHECK (
        (MaNhanVien IS NOT NULL AND MaDocGia IS NULL)
        OR (MaNhanVien IS NULL AND MaDocGia IS NOT NULL)
    )
);
GO

CREATE UNIQUE INDEX UX_TaiKhoan_NhanVien
    ON TaiKhoan(MaNhanVien)
    WHERE MaNhanVien IS NOT NULL;
GO

CREATE UNIQUE INDEX UX_TaiKhoan_DocGia
    ON TaiKhoan(MaDocGia)
    WHERE MaDocGia IS NOT NULL;
GO

/* =========================================================
   2. THẺ ĐỘC GIẢ VÀ ĐÓNG PHÍ THƯỜNG NIÊN
   ========================================================= */
CREATE TABLE TheDocGia (
    MaThe        INT IDENTITY(1,1) PRIMARY KEY,
    MaTheHienThi AS (N'TDG' + RIGHT(N'000000' + CONVERT(NVARCHAR(10), MaThe), 6)) PERSISTED,
    MaDocGia     INT NOT NULL,
    NgayCap      DATE NOT NULL CONSTRAINT DF_TheDocGia_NgayCap DEFAULT CAST(GETDATE() AS DATE),
    NgayHetHan   DATE NOT NULL,
    TrangThai    NVARCHAR(30) NOT NULL CONSTRAINT DF_TheDocGia_TrangThai DEFAULT N'Đang hiệu lực',
    GhiChu       NVARCHAR(255) NULL,
    NgayKhoa     DATE NULL,
    NgayMoKhoaDuKien DATE NULL,
    LyDoKhoa     NVARCHAR(100) NULL,
    LoaiKhoa     NVARCHAR(20) NULL,
    CONSTRAINT FK_TheDocGia_DocGia FOREIGN KEY (MaDocGia) REFERENCES DocGia(MaDocGia),
    CONSTRAINT CK_TheDocGia_Ngay CHECK (NgayHetHan > NgayCap),
    CONSTRAINT CK_TheDocGia_TrangThai CHECK (TrangThai IN (N'Đang hiệu lực', N'Hết hạn', N'Bị khóa', N'Đã thay thế')),
    CONSTRAINT CK_TheDocGia_LoaiKhoa CHECK (LoaiKhoa IS NULL OR LoaiKhoa IN (N'Tạm thời', N'Vĩnh viễn')),
    CONSTRAINT CK_TheDocGia_NgayMoKhoa CHECK (NgayMoKhoaDuKien IS NULL OR NgayKhoa IS NULL OR NgayMoKhoaDuKien > NgayKhoa)
);
GO

-- Mỗi độc giả chỉ có tối đa một thẻ đang hiệu lực tại một thời điểm.
CREATE UNIQUE INDEX UX_TheDocGia_MotTheHieuLuc
    ON TheDocGia(MaDocGia)
    WHERE TrangThai = N'Đang hiệu lực';
GO

CREATE TABLE DongPhiThuongNien (
    MaDongPhi    INT IDENTITY(1,1) PRIMARY KEY,
    MaThe        INT NOT NULL,
    Nam          INT NOT NULL,
    NgayDong     DATE NOT NULL CONSTRAINT DF_DongPhi_NgayDong DEFAULT CAST(GETDATE() AS DATE),
    SoTien       DECIMAL(18,2) NOT NULL,
    MaNhanVienThu INT NULL,
    TrangThai    NVARCHAR(30) NOT NULL CONSTRAINT DF_DongPhi_TrangThai DEFAULT N'Đã thanh toán',
    LoaiLePhi    NVARCHAR(50) NOT NULL CONSTRAINT DF_DongPhi_LoaiLePhi DEFAULT N'Lệ phí thường niên',
    SoPhieuThu   NVARCHAR(30) NULL,
    HinhThucThu  NVARCHAR(30) NOT NULL CONSTRAINT DF_DongPhi_HinhThucThu DEFAULT N'Tiền mặt',
    GhiChu       NVARCHAR(255) NULL,
    CONSTRAINT FK_DongPhi_The FOREIGN KEY (MaThe) REFERENCES TheDocGia(MaThe),
    CONSTRAINT FK_DongPhi_NhanVien FOREIGN KEY (MaNhanVienThu) REFERENCES NhanVien(MaNhanVien),
    CONSTRAINT CK_DongPhi_Nam CHECK (Nam BETWEEN 2000 AND 2100),
    CONSTRAINT CK_DongPhi_SoTien CHECK (SoTien >= 0),
    CONSTRAINT CK_DongPhi_TrangThai CHECK (TrangThai IN (N'Đã thanh toán', N'Hoàn tiền', N'Đã hủy'))
);
GO
CREATE UNIQUE INDEX UX_DongPhi_ThuongNien_The_Nam ON DongPhiThuongNien(MaThe, Nam)
    WHERE LoaiLePhi = N'Lệ phí thường niên' AND TrangThai = N'Đã thanh toán';
CREATE UNIQUE INDEX UX_DongPhi_SoPhieuThu ON DongPhiThuongNien(SoPhieuThu) WHERE SoPhieuThu IS NOT NULL;
GO

/* =========================================================
   3. QUẢN LÝ ĐẦU SÁCH - CUỐN SÁCH
   ========================================================= */
CREATE TABLE NhaXuatBan (
    MaNXB        INT IDENTITY(1,1) PRIMARY KEY,
    TenNXB       NVARCHAR(150) NOT NULL,
    DiaChi       NVARCHAR(255) NULL,
    SoDienThoai  NVARCHAR(15) NULL,
    Email        NVARCHAR(100) NULL,
    TrangThai    BIT NOT NULL CONSTRAINT DF_NXB_TrangThai DEFAULT 1,
    CONSTRAINT UQ_NhaXuatBan_Ten UNIQUE (TenNXB)
);
GO

CREATE TABLE TacGia (
    MaTacGia     INT IDENTITY(1,1) PRIMARY KEY,
    TenTacGia    NVARCHAR(100) NOT NULL,
    NamSinh      INT NULL,
    QueQuan      NVARCHAR(150) NULL,
    ButDanh      NVARCHAR(100) NULL,
    GhiChu       NVARCHAR(255) NULL,
    CONSTRAINT CK_TacGia_NamSinh CHECK (NamSinh IS NULL OR NamSinh BETWEEN 1000 AND 2100)
);
GO

CREATE TABLE TheLoai (
    MaTheLoai    INT IDENTITY(1,1) PRIMARY KEY,
    TenTheLoai   NVARCHAR(100) NOT NULL,
    MoTa         NVARCHAR(255) NULL,
    TrangThai    BIT NOT NULL CONSTRAINT DF_TheLoai_TrangThai DEFAULT 1,
    CONSTRAINT UQ_TheLoai_Ten UNIQUE (TenTheLoai)
);
GO

CREATE TABLE ViTriSach (
    MaViTri      INT IDENTITY(1,1) PRIMARY KEY,
    TenKe        NVARCHAR(50) NOT NULL,
    Tang         NVARCHAR(50) NULL,
    KhuVuc       NVARCHAR(100) NULL,
    GhiChu       NVARCHAR(255) NULL,
    CONSTRAINT UQ_ViTriSach UNIQUE (TenKe, Tang, KhuVuc)
);
GO

CREATE TABLE Sach (
    MaSach       INT IDENTITY(1,1) PRIMARY KEY,
    MaSachHienThi AS (N'S' + RIGHT(N'00000' + CONVERT(NVARCHAR(10), MaSach), 5)) PERSISTED,
    TenSach      NVARCHAR(100) NOT NULL,
    ISBN         NVARCHAR(30) NULL,
    MaTheLoai    INT NOT NULL,
    MaNXB        INT NULL,
    NamXuatBan   INT NULL,
    NgonNgu      NVARCHAR(50) NULL,
    SoTrang      INT NULL,
    GiaBia       DECIMAL(18,2) NULL,
    MoTa         NVARCHAR(MAX) NULL,
    AnhBia       NVARCHAR(255) NULL,
    TrangThai    BIT NOT NULL CONSTRAINT DF_Sach_TrangThai DEFAULT 1,
    CONSTRAINT FK_Sach_TheLoai FOREIGN KEY (MaTheLoai) REFERENCES TheLoai(MaTheLoai),
    CONSTRAINT FK_Sach_NXB FOREIGN KEY (MaNXB) REFERENCES NhaXuatBan(MaNXB),
    CONSTRAINT CK_Sach_TenSach CHECK (LEN(LTRIM(RTRIM(TenSach))) BETWEEN 1 AND 100),
    CONSTRAINT CK_Sach_NamXuatBan CHECK (NamXuatBan IS NULL OR NamXuatBan BETWEEN 1000 AND 2100),
    CONSTRAINT CK_Sach_SoTrang CHECK (SoTrang IS NULL OR SoTrang > 0),
    CONSTRAINT CK_Sach_GiaBia CHECK (GiaBia IS NULL OR GiaBia > 0)
);
GO

CREATE UNIQUE INDEX UX_Sach_ISBN
    ON Sach(ISBN)
    WHERE ISBN IS NOT NULL;
GO

CREATE TABLE Sach_TacGia (
    MaSach       INT NOT NULL,
    MaTacGia     INT NOT NULL,
    VaiTro       NVARCHAR(50) NULL,
    CONSTRAINT PK_Sach_TacGia PRIMARY KEY (MaSach, MaTacGia),
    CONSTRAINT FK_STG_Sach FOREIGN KEY (MaSach) REFERENCES Sach(MaSach),
    CONSTRAINT FK_STG_TacGia FOREIGN KEY (MaTacGia) REFERENCES TacGia(MaTacGia)
);
GO

CREATE TABLE CuonSach (
    MaCuonSach   INT IDENTITY(1,1) PRIMARY KEY,
    MaCuonSachHienThi AS (N'CS' + RIGHT(N'000000' + CONVERT(NVARCHAR(10), MaCuonSach), 6)) PERSISTED,
    MaSach       INT NOT NULL,
    MaViTri      INT NULL,
    MaVach       NVARCHAR(50) NOT NULL,
    TinhTrang    NVARCHAR(30) NOT NULL CONSTRAINT DF_CuonSach_TinhTrang DEFAULT N'Tốt',
    TrangThai    NVARCHAR(30) NOT NULL CONSTRAINT DF_CuonSach_TrangThai DEFAULT N'Có sẵn',
    NgayNhap     DATE NOT NULL CONSTRAINT DF_CuonSach_NgayNhap DEFAULT CAST(GETDATE() AS DATE),
    GiaNhap      DECIMAL(18,2) NULL,
    GhiChu       NVARCHAR(255) NULL,
    CONSTRAINT UQ_CuonSach_MaVach UNIQUE (MaVach),
    CONSTRAINT FK_CuonSach_Sach FOREIGN KEY (MaSach) REFERENCES Sach(MaSach),
    CONSTRAINT FK_CuonSach_ViTri FOREIGN KEY (MaViTri) REFERENCES ViTriSach(MaViTri),
    CONSTRAINT CK_CuonSach_TinhTrang CHECK (TinhTrang IN (N'Tốt', N'Rách nhẹ', N'Hư hỏng', N'Mất')),
    CONSTRAINT CK_CuonSach_TrangThai CHECK (TrangThai IN (N'Có sẵn', N'Đang mượn', N'Đặt trước', N'Hỏng', N'Mất', N'Thanh lý')),
    CONSTRAINT CK_CuonSach_GiaNhap CHECK (GiaNhap IS NULL OR GiaNhap >= 0)
);
GO

/* =========================================================
   4. QUY ĐỊNH MƯỢN TRẢ
   ========================================================= */
CREATE TABLE QuyDinh (
    MaQuyDinh        INT IDENTITY(1,1) PRIMARY KEY,
    TenQuyDinh       NVARCHAR(100) NOT NULL,
    SoNgayMuonToiDa  INT NOT NULL CONSTRAINT DF_QuyDinh_SoNgay DEFAULT 14,
    SoSachMuonToiDa  INT NOT NULL CONSTRAINT DF_QuyDinh_SoSach DEFAULT 3,
    PhiThuongNien    DECIMAL(18,2) NOT NULL CONSTRAINT DF_QuyDinh_PhiNam DEFAULT 100000,
    TienPhatMoiNgay  DECIMAL(18,2) NOT NULL CONSTRAINT DF_QuyDinh_PhatNgay DEFAULT 5000,
    TyLePhatHong     DECIMAL(5,2) NOT NULL CONSTRAINT DF_QuyDinh_PhatHong DEFAULT 50,
    TyLePhatMat      DECIMAL(5,2) NOT NULL CONSTRAINT DF_QuyDinh_PhatMat DEFAULT 100,
    NgayApDung       DATE NOT NULL CONSTRAINT DF_QuyDinh_Ngay DEFAULT CAST(GETDATE() AS DATE),
    TrangThai        BIT NOT NULL CONSTRAINT DF_QuyDinh_TrangThai DEFAULT 1,
    CONSTRAINT CK_QuyDinh_SoNgay CHECK (SoNgayMuonToiDa > 0),
    CONSTRAINT CK_QuyDinh_SoSach CHECK (SoSachMuonToiDa > 0),
    CONSTRAINT CK_QuyDinh_Tien CHECK (PhiThuongNien >= 0 AND TienPhatMoiNgay >= 0),
    CONSTRAINT CK_QuyDinh_TyLe CHECK (TyLePhatHong BETWEEN 0 AND 100 AND TyLePhatMat BETWEEN 0 AND 300)
);
GO

/* =========================================================
   5. MƯỢN - TRẢ - PHẠT
   ========================================================= */
CREATE TABLE PhieuMuon (
    MaPhieuMuon  INT IDENTITY(1,1) PRIMARY KEY,
    MaPhieuMuonHienThi AS (N'PM' + RIGHT(N'000000' + CONVERT(NVARCHAR(10), MaPhieuMuon), 6)) PERSISTED,
    MaThe        INT NOT NULL,
    MaNhanVien   INT NOT NULL,
    NgayMuon     DATETIME2 NOT NULL CONSTRAINT DF_PhieuMuon_NgayMuon DEFAULT SYSDATETIME(),
    HanTra       DATE NOT NULL,
    TrangThai    NVARCHAR(30) NOT NULL CONSTRAINT DF_PhieuMuon_TrangThai DEFAULT N'Đang mượn',
    GhiChu       NVARCHAR(255) NULL,
    CONSTRAINT FK_PM_TheDocGia FOREIGN KEY (MaThe) REFERENCES TheDocGia(MaThe),
    CONSTRAINT FK_PM_NhanVien FOREIGN KEY (MaNhanVien) REFERENCES NhanVien(MaNhanVien),
    CONSTRAINT CK_PM_HanTra CHECK (HanTra >= CAST(NgayMuon AS DATE)),
    CONSTRAINT CK_PM_TrangThai CHECK (TrangThai IN (N'Đang mượn', N'Đã trả', N'Quá hạn', N'Đã hủy'))
);
GO

CREATE TABLE ChiTietMuon (
    MaChiTietMuon INT IDENTITY(1,1) PRIMARY KEY,
    MaPhieuMuon   INT NOT NULL,
    MaCuonSach    INT NOT NULL,
    TrangThai     NVARCHAR(30) NOT NULL CONSTRAINT DF_CTM_TrangThai DEFAULT N'Đang mượn',
    GhiChu        NVARCHAR(255) NULL,
    CONSTRAINT FK_CTM_PhieuMuon FOREIGN KEY (MaPhieuMuon) REFERENCES PhieuMuon(MaPhieuMuon),
    CONSTRAINT FK_CTM_CuonSach FOREIGN KEY (MaCuonSach) REFERENCES CuonSach(MaCuonSach),
    CONSTRAINT UQ_CTM_Phieu_Cuon UNIQUE (MaPhieuMuon, MaCuonSach),
    CONSTRAINT CK_CTM_TrangThai CHECK (TrangThai IN (N'Đang mượn', N'Đã trả', N'Quá hạn', N'Mất', N'Hỏng'))
);
GO

CREATE TABLE PhieuTra (
    MaPhieuTra   INT IDENTITY(1,1) PRIMARY KEY,
    MaPhieuTraHienThi AS (N'PT' + RIGHT(N'000000' + CONVERT(NVARCHAR(10), MaPhieuTra), 6)) PERSISTED,
    MaPhieuMuon  INT NOT NULL,
    MaNhanVien   INT NOT NULL,
    NgayTra      DATETIME2 NOT NULL CONSTRAINT DF_PhieuTra_NgayTra DEFAULT SYSDATETIME(),
    GhiChu       NVARCHAR(255) NULL,
    CONSTRAINT FK_PT_PhieuMuon FOREIGN KEY (MaPhieuMuon) REFERENCES PhieuMuon(MaPhieuMuon),
    CONSTRAINT FK_PT_NhanVien FOREIGN KEY (MaNhanVien) REFERENCES NhanVien(MaNhanVien)
);
GO

CREATE TABLE ChiTietTra (
    MaChiTietTra  INT IDENTITY(1,1) PRIMARY KEY,
    MaPhieuTra    INT NOT NULL,
    MaChiTietMuon INT NOT NULL,
    TinhTrangTra  NVARCHAR(30) NOT NULL CONSTRAINT DF_CTT_TinhTrang DEFAULT N'Tốt',
    SoNgayTre      INT NOT NULL CONSTRAINT DF_CTT_SoNgayTre DEFAULT 0,
    GhiChu         NVARCHAR(255) NULL,
    CONSTRAINT FK_CTT_PhieuTra FOREIGN KEY (MaPhieuTra) REFERENCES PhieuTra(MaPhieuTra),
    CONSTRAINT FK_CTT_ChiTietMuon FOREIGN KEY (MaChiTietMuon) REFERENCES ChiTietMuon(MaChiTietMuon),
    CONSTRAINT UQ_CTT_ChiTietMuon UNIQUE (MaChiTietMuon),
    CONSTRAINT CK_CTT_TinhTrang CHECK (TinhTrangTra IN (N'Tốt', N'Rách nhẹ', N'Hư hỏng', N'Mất')),
    CONSTRAINT CK_CTT_SoNgayTre CHECK (SoNgayTre >= 0)
);
GO

CREATE TABLE PhieuPhat (
    MaPhieuPhat  INT IDENTITY(1,1) PRIMARY KEY,
    MaPhieuPhatHienThi AS (N'PP' + RIGHT(N'000000' + CONVERT(NVARCHAR(10), MaPhieuPhat), 6)) PERSISTED,
    MaDocGia     INT NOT NULL,
    MaPhieuMuon  INT NULL,
    MaNhanVien   INT NOT NULL,
    NgayLap      DATETIME2 NOT NULL CONSTRAINT DF_PhieuPhat_NgayLap DEFAULT SYSDATETIME(),
    TongTien     DECIMAL(18,2) NOT NULL CONSTRAINT DF_PhieuPhat_TongTien DEFAULT 0,
    TrangThai    NVARCHAR(30) NOT NULL CONSTRAINT DF_PhieuPhat_TrangThai DEFAULT N'Chưa thanh toán',
    NgayThanhToan DATETIME2 NULL,
    GhiChu       NVARCHAR(255) NULL,
    CONSTRAINT FK_PP_DocGia FOREIGN KEY (MaDocGia) REFERENCES DocGia(MaDocGia),
    CONSTRAINT FK_PP_PhieuMuon FOREIGN KEY (MaPhieuMuon) REFERENCES PhieuMuon(MaPhieuMuon),
    CONSTRAINT FK_PP_NhanVien FOREIGN KEY (MaNhanVien) REFERENCES NhanVien(MaNhanVien),
    CONSTRAINT CK_PP_TongTien CHECK (TongTien >= 0),
    CONSTRAINT CK_PP_TrangThai CHECK (TrangThai IN (N'Chưa thanh toán', N'Đã thanh toán', N'Đã hủy'))
);
GO

CREATE TABLE ChiTietPhat (
    MaChiTietPhat INT IDENTITY(1,1) PRIMARY KEY,
    MaPhieuPhat   INT NOT NULL,
    MaChiTietMuon INT NULL,
    LoaiPhat      NVARCHAR(30) NOT NULL,
    NoiDung       NVARCHAR(255) NOT NULL,
    SoNgayTre     INT NOT NULL CONSTRAINT DF_CTP_SoNgayTre DEFAULT 0,
    SoTien        DECIMAL(18,2) NOT NULL,
    CONSTRAINT FK_CTP_PhieuPhat FOREIGN KEY (MaPhieuPhat) REFERENCES PhieuPhat(MaPhieuPhat),
    CONSTRAINT FK_CTP_ChiTietMuon FOREIGN KEY (MaChiTietMuon) REFERENCES ChiTietMuon(MaChiTietMuon),
    CONSTRAINT CK_CTP_LoaiPhat CHECK (LoaiPhat IN (N'Trễ hạn', N'Hư hỏng', N'Mất sách', N'Khác')),
    CONSTRAINT CK_CTP_SoNgayTre CHECK (SoNgayTre >= 0),
    CONSTRAINT CK_CTP_SoTien CHECK (SoTien >= 0)
);
GO

/* =========================================================
   6. NHẬP SÁCH - NHÀ CUNG CẤP
   ========================================================= */
CREATE TABLE NhaCungCap (
    MaNCC        INT IDENTITY(1,1) PRIMARY KEY,
    TenNCC       NVARCHAR(150) NOT NULL,
    DiaChi       NVARCHAR(255) NULL,
    SoDienThoai  NVARCHAR(15) NULL,
    Email        NVARCHAR(100) NULL,
    NguoiDaiDien NVARCHAR(100) NULL,
    MaSoThue     NVARCHAR(30) NULL,
    Website      NVARCHAR(200) NULL,
    LoaiNcc      NVARCHAR(50) NULL,
    Logo         NVARCHAR(255) NULL,
    DieuKhoanThanhToan NVARCHAR(100) NULL,
    NgayBatDauHopTac DATE NULL,
    KhuVucCungCap NVARCHAR(100) NULL,
    NhomSachCungCap NVARCHAR(500) NULL,
    GhiChu       NVARCHAR(500) NULL,
    HanMucCongNo DECIMAL(18,2) NOT NULL CONSTRAINT DF_NCC_HanMuc DEFAULT 0,
    ChietKhauMacDinh DECIMAL(5,2) NOT NULL CONSTRAINT DF_NCC_ChietKhau DEFAULT 0,
    PhuongThucThanhToan NVARCHAR(50) NULL,
    DanhGiaBanDau NVARCHAR(30) NULL,
    TrangThai    BIT NOT NULL CONSTRAINT DF_NCC_TrangThai DEFAULT 1,
    CONSTRAINT UQ_NCC_Ten UNIQUE (TenNCC)
);
GO

CREATE TABLE PhieuNhap (
    MaPhieuNhap  INT IDENTITY(1,1) PRIMARY KEY,
    MaPhieuNhapHienThi AS (N'PN' + RIGHT(N'000000' + CONVERT(NVARCHAR(10), MaPhieuNhap), 6)) PERSISTED,
    MaNCC        INT NOT NULL,
    MaNhanVien   INT NOT NULL,
    NgayNhap     DATETIME2 NOT NULL CONSTRAINT DF_PN_NgayNhap DEFAULT SYSDATETIME(),
    TongTien     DECIMAL(18,2) NOT NULL CONSTRAINT DF_PN_TongTien DEFAULT 0,
    TrangThai    NVARCHAR(20) NOT NULL CONSTRAINT DF_PhieuNhap_TrangThai DEFAULT N'Hoàn thành',
    GhiChu       NVARCHAR(255) NULL,
    CONSTRAINT FK_PN_NCC FOREIGN KEY (MaNCC) REFERENCES NhaCungCap(MaNCC),
    CONSTRAINT FK_PN_NhanVien FOREIGN KEY (MaNhanVien) REFERENCES NhanVien(MaNhanVien),
    CONSTRAINT CK_PN_TongTien CHECK (TongTien >= 0),
    CONSTRAINT CK_PhieuNhap_TrangThai CHECK (TrangThai IN (N'Đang nhập', N'Chờ duyệt', N'Hoàn thành', N'Đã hủy'))
);
GO

CREATE TABLE ChiTietNhap (
    MaChiTietNhap INT IDENTITY(1,1) PRIMARY KEY,
    MaPhieuNhap   INT NOT NULL,
    MaSach        INT NOT NULL,
    SoLuong       INT NOT NULL,
    DonGia        DECIMAL(18,2) NOT NULL,
    ThanhTien AS (CONVERT(DECIMAL(18,2), SoLuong * DonGia)) PERSISTED,
    CONSTRAINT FK_CTN_PhieuNhap FOREIGN KEY (MaPhieuNhap) REFERENCES PhieuNhap(MaPhieuNhap),
    CONSTRAINT FK_CTN_Sach FOREIGN KEY (MaSach) REFERENCES Sach(MaSach),
    CONSTRAINT UQ_CTN_Phieu_Sach UNIQUE (MaPhieuNhap, MaSach),
    CONSTRAINT CK_CTN_SoLuong CHECK (SoLuong > 0),
    CONSTRAINT CK_CTN_DonGia CHECK (DonGia >= 0)
);
GO

/* =========================================================
   7. CHỨC NĂNG BỔ SUNG
   ========================================================= */
CREATE TABLE DatTruocSach (
    MaDatTruoc   INT IDENTITY(1,1) PRIMARY KEY,
    MaDocGia     INT NOT NULL,
    MaSach       INT NOT NULL,
    NgayDat      DATETIME2 NOT NULL CONSTRAINT DF_DatTruoc_NgayDat DEFAULT SYSDATETIME(),
    HanGiuDen    DATETIME2 NULL,
    TrangThai    NVARCHAR(30) NOT NULL CONSTRAINT DF_DatTruoc_TrangThai DEFAULT N'Đang chờ',
    GhiChu       NVARCHAR(255) NULL,
    CONSTRAINT FK_DatTruoc_DocGia FOREIGN KEY (MaDocGia) REFERENCES DocGia(MaDocGia),
    CONSTRAINT FK_DatTruoc_Sach FOREIGN KEY (MaSach) REFERENCES Sach(MaSach),
    CONSTRAINT CK_DatTruoc_TrangThai CHECK (TrangThai IN (N'Đang chờ', N'Đã thông báo', N'Đã nhận', N'Đã hủy', N'Hết hạn'))
);
GO

CREATE UNIQUE INDEX UX_DatTruoc_DangCho
    ON DatTruocSach(MaDocGia, MaSach)
    WHERE TrangThai IN (N'Đang chờ', N'Đã thông báo');
GO

CREATE TABLE DanhGiaSach (
    MaDanhGia    INT IDENTITY(1,1) PRIMARY KEY,
    MaSach       INT NOT NULL,
    MaDocGia     INT NOT NULL,
    SoSao        INT NOT NULL,
    NhanXet      NVARCHAR(500) NULL,
    NgayDanhGia  DATETIME2 NOT NULL CONSTRAINT DF_DanhGia_Ngay DEFAULT SYSDATETIME(),
    TrangThai    BIT NOT NULL CONSTRAINT DF_DanhGia_TrangThai DEFAULT 1,
    CONSTRAINT FK_DanhGia_Sach FOREIGN KEY (MaSach) REFERENCES Sach(MaSach),
    CONSTRAINT FK_DanhGia_DocGia FOREIGN KEY (MaDocGia) REFERENCES DocGia(MaDocGia),
    CONSTRAINT UQ_DanhGia_Sach_DocGia UNIQUE (MaSach, MaDocGia),
    CONSTRAINT CK_DanhGia_SoSao CHECK (SoSao BETWEEN 1 AND 5)
);
GO

CREATE TABLE LoaiThongBao (
    MaLoaiThongBao INT IDENTITY(1,1) PRIMARY KEY,
    TenLoai        NVARCHAR(100) NOT NULL,
    Icon           NVARCHAR(50) NOT NULL,
    Mau            NVARCHAR(30) NOT NULL,
    MoTa           NVARCHAR(255) NULL,
    CONSTRAINT UQ_LoaiThongBao_Ten UNIQUE (TenLoai)
);
GO

CREATE TABLE ThongBao (
    MaThongBao     INT IDENTITY(1,1) PRIMARY KEY,
    MaLoaiThongBao INT NOT NULL,
    MaDocGia       INT NULL,
    MaNhanVien     INT NULL,
    TieuDe         NVARCHAR(200) NOT NULL,
    NoiDung        NVARCHAR(MAX) NULL,
    NgayGui        DATETIME2 NOT NULL CONSTRAINT DF_ThongBao_NgayGui DEFAULT SYSDATETIME(),
    DaDoc          BIT NOT NULL CONSTRAINT DF_ThongBao_DaDoc DEFAULT 0,
    CONSTRAINT FK_ThongBao_Loai FOREIGN KEY (MaLoaiThongBao) REFERENCES LoaiThongBao(MaLoaiThongBao),
    CONSTRAINT FK_ThongBao_DocGia FOREIGN KEY (MaDocGia) REFERENCES DocGia(MaDocGia),
    CONSTRAINT FK_ThongBao_NhanVien FOREIGN KEY (MaNhanVien) REFERENCES NhanVien(MaNhanVien),
    CONSTRAINT CK_ThongBao_NguoiNhan CHECK (NOT (MaDocGia IS NOT NULL AND MaNhanVien IS NOT NULL))
);
GO

CREATE TABLE NhatKyHeThong (
    MaNhatKy     BIGINT IDENTITY(1,1) PRIMARY KEY,
    MaTaiKhoan   INT NULL,
    HanhDong     NVARCHAR(100) NOT NULL,
    BangTacDong  NVARCHAR(100) NULL,
    KhoaChinh    NVARCHAR(100) NULL,
    NoiDung      NVARCHAR(MAX) NULL,
    DiaChiIP     NVARCHAR(50) NULL,
    ThoiGian     DATETIME2 NOT NULL CONSTRAINT DF_NhatKy_ThoiGian DEFAULT SYSDATETIME(),
    CONSTRAINT FK_NhatKy_TaiKhoan FOREIGN KEY (MaTaiKhoan) REFERENCES TaiKhoan(MaTaiKhoan)
);
GO

/* =========================================================
   8. INDEX PHỤC VỤ TÌM KIẾM - THỐNG KÊ
   ========================================================= */
CREATE INDEX IX_Sach_TenSach ON Sach(TenSach);
CREATE INDEX IX_Sach_TheLoai ON Sach(MaTheLoai);
CREATE INDEX IX_Sach_NXB ON Sach(MaNXB);
CREATE INDEX IX_CuonSach_Sach_TrangThai ON CuonSach(MaSach, TrangThai);
CREATE INDEX IX_CuonSach_ViTri ON CuonSach(MaViTri);
CREATE INDEX IX_PhieuMuon_The_NgayMuon ON PhieuMuon(MaThe, NgayMuon DESC);
CREATE INDEX IX_PhieuMuon_TrangThai_HanTra ON PhieuMuon(TrangThai, HanTra);
CREATE INDEX IX_ChiTietMuon_Phieu_TrangThai ON ChiTietMuon(MaPhieuMuon, TrangThai);
CREATE INDEX IX_PhieuTra_PhieuMuon_NgayTra ON PhieuTra(MaPhieuMuon, NgayTra DESC);
CREATE INDEX IX_PhieuPhat_DocGia_TrangThai ON PhieuPhat(MaDocGia, TrangThai);
CREATE INDEX IX_ThongBao_DocGia_DaDoc ON ThongBao(MaDocGia, DaDoc, NgayGui DESC);
CREATE INDEX IX_NhatKy_ThoiGian ON NhatKyHeThong(ThoiGian DESC);
GO

/* =========================================================
   9. TRIGGER KIỂM SOÁT NGHIỆP VỤ MƯỢN
   ========================================================= */
CREATE OR ALTER TRIGGER TRG_ChiTietMuon_KiemTraNghiepVu
ON ChiTietMuon
INSTEAD OF INSERT
AS
BEGIN
    SET NOCOUNT ON;

    -- Không cho một phiếu mượn hai cuốn cùng một đầu sách.
    IF EXISTS (
        SELECT 1
        FROM inserted i
        JOIN CuonSach cs ON cs.MaCuonSach = i.MaCuonSach
        JOIN ChiTietMuon ctm ON ctm.MaPhieuMuon = i.MaPhieuMuon
        JOIN CuonSach csCu ON csCu.MaCuonSach = ctm.MaCuonSach
        WHERE cs.MaSach = csCu.MaSach
    )
    OR EXISTS (
        SELECT 1
        FROM inserted i
        JOIN CuonSach cs ON cs.MaCuonSach = i.MaCuonSach
        GROUP BY i.MaPhieuMuon, cs.MaSach
        HAVING COUNT(*) > 1
    )
    BEGIN
        THROW 51001, N'Một phiếu mượn không được mượn hai cuốn cùng một đầu sách.', 1;
    END;

    -- Cuốn sách phải đang có sẵn.
    IF EXISTS (
        SELECT 1
        FROM inserted i
        JOIN CuonSach cs ON cs.MaCuonSach = i.MaCuonSach
        WHERE cs.TrangThai <> N'Có sẵn'
    )
    BEGIN
        THROW 51002, N'Có cuốn sách không ở trạng thái Có sẵn.', 1;
    END;

    -- Thẻ phải hiệu lực, chưa hết hạn và đã đóng phí năm hiện tại.
    IF EXISTS (
        SELECT 1
        FROM inserted i
        JOIN PhieuMuon pm ON pm.MaPhieuMuon = i.MaPhieuMuon
        JOIN TheDocGia tdg ON tdg.MaThe = pm.MaThe
        WHERE tdg.TrangThai <> N'Đang hiệu lực'
           OR tdg.NgayHetHan < CAST(GETDATE() AS DATE)
           OR NOT EXISTS (
               SELECT 1
               FROM DongPhiThuongNien dp
               WHERE dp.MaThe = tdg.MaThe
                 AND dp.Nam = YEAR(GETDATE())
                 AND dp.TrangThai = N'Đã thanh toán'
           )
    )
    BEGIN
        THROW 51003, N'Thẻ độc giả không hợp lệ, đã hết hạn hoặc chưa đóng phí thường niên.', 1;
    END;

    -- Không được mượn nếu còn sách quá hạn.
    IF EXISTS (
        SELECT 1
        FROM inserted i
        JOIN PhieuMuon pmMoi ON pmMoi.MaPhieuMuon = i.MaPhieuMuon
        JOIN TheDocGia tdg ON tdg.MaThe = pmMoi.MaThe
        JOIN TheDocGia tdgCu ON tdgCu.MaDocGia = tdg.MaDocGia
        JOIN PhieuMuon pmCu ON pmCu.MaThe = tdgCu.MaThe
        JOIN ChiTietMuon ctmCu ON ctmCu.MaPhieuMuon = pmCu.MaPhieuMuon
        WHERE ctmCu.TrangThai IN (N'Đang mượn', N'Quá hạn')
          AND pmCu.HanTra < CAST(GETDATE() AS DATE)
    )
    BEGIN
        THROW 51004, N'Độc giả còn sách quá hạn nên không được mượn thêm.', 1;
    END;

    -- Tổng số sách đang mượn của độc giả không vượt quá quy định đang áp dụng.
    DECLARE @SoSachToiDa INT = ISNULL((
        SELECT TOP 1 SoSachMuonToiDa
        FROM QuyDinh
        WHERE TrangThai = 1 AND NgayApDung <= CAST(GETDATE() AS DATE)
        ORDER BY NgayApDung DESC, MaQuyDinh DESC
    ), 3);

    IF EXISTS (
        SELECT 1
        FROM inserted i
        JOIN PhieuMuon pmMoi ON pmMoi.MaPhieuMuon = i.MaPhieuMuon
        JOIN TheDocGia tdgMoi ON tdgMoi.MaThe = pmMoi.MaThe
        CROSS APPLY (
            SELECT COUNT(*) AS DangMuonHienTai
            FROM ChiTietMuon ctm
            JOIN PhieuMuon pm ON pm.MaPhieuMuon = ctm.MaPhieuMuon
            JOIN TheDocGia tdg ON tdg.MaThe = pm.MaThe
            WHERE tdg.MaDocGia = tdgMoi.MaDocGia
              AND ctm.TrangThai IN (N'Đang mượn', N'Quá hạn')
        ) x
        CROSS APPLY (
            SELECT COUNT(*) AS SoThem
            FROM inserted i2
            JOIN PhieuMuon pm2 ON pm2.MaPhieuMuon = i2.MaPhieuMuon
            JOIN TheDocGia tdg2 ON tdg2.MaThe = pm2.MaThe
            WHERE tdg2.MaDocGia = tdgMoi.MaDocGia
        ) y
        WHERE x.DangMuonHienTai + y.SoThem > @SoSachToiDa
    )
    BEGIN
        THROW 51005, N'Số sách đang mượn vượt quá giới hạn cho phép.', 1;
    END;

    INSERT INTO ChiTietMuon (MaPhieuMuon, MaCuonSach, TrangThai, GhiChu)
    SELECT MaPhieuMuon, MaCuonSach, ISNULL(TrangThai, N'Đang mượn'), GhiChu
    FROM inserted;

    UPDATE cs
    SET cs.TrangThai = N'Đang mượn'
    FROM CuonSach cs
    JOIN inserted i ON i.MaCuonSach = cs.MaCuonSach;
END;
GO

/* =========================================================
   10. TRIGGER CẬP NHẬT KHI TRẢ SÁCH
   ========================================================= */
CREATE OR ALTER TRIGGER TRG_ChiTietTra_CapNhatTrangThai
ON ChiTietTra
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE ctm
    SET ctm.TrangThai = CASE
        WHEN i.TinhTrangTra = N'Mất' THEN N'Mất'
        WHEN i.TinhTrangTra IN (N'Rách nhẹ', N'Hư hỏng') THEN N'Hỏng'
        ELSE N'Đã trả'
    END
    FROM ChiTietMuon ctm
    JOIN inserted i ON i.MaChiTietMuon = ctm.MaChiTietMuon;

    UPDATE cs
    SET cs.TinhTrang = i.TinhTrangTra,
        cs.TrangThai = CASE
            WHEN i.TinhTrangTra = N'Mất' THEN N'Mất'
            WHEN i.TinhTrangTra IN (N'Rách nhẹ', N'Hư hỏng') THEN N'Hỏng'
            ELSE N'Có sẵn'
        END
    FROM CuonSach cs
    JOIN ChiTietMuon ctm ON ctm.MaCuonSach = cs.MaCuonSach
    JOIN inserted i ON i.MaChiTietMuon = ctm.MaChiTietMuon;

    -- Đánh dấu phiếu mượn đã trả khi không còn chi tiết đang mượn/quá hạn.
    UPDATE pm
    SET pm.TrangThai = N'Đã trả'
    FROM PhieuMuon pm
    WHERE pm.MaPhieuMuon IN (
        SELECT DISTINCT pt.MaPhieuMuon
        FROM inserted i
        JOIN PhieuTra pt ON pt.MaPhieuTra = i.MaPhieuTra
    )
    AND NOT EXISTS (
        SELECT 1
        FROM ChiTietMuon ctm
        WHERE ctm.MaPhieuMuon = pm.MaPhieuMuon
          AND ctm.TrangThai IN (N'Đang mượn', N'Quá hạn')
    );
END;
GO

/* =========================================================
   11. VIEW PHỤC VỤ DASHBOARD / CHI TIẾT SÁCH
   ========================================================= */
CREATE OR ALTER VIEW vw_ThongKeSach
AS
SELECT
    s.MaSach,
    s.MaSachHienThi,
    s.TenSach,
    tl.TenTheLoai,
    COUNT(cs.MaCuonSach) AS TongSoLuong,
    SUM(CASE WHEN cs.TrangThai = N'Có sẵn' THEN 1 ELSE 0 END) AS ConLai,
    SUM(CASE WHEN cs.TrangThai = N'Đang mượn' THEN 1 ELSE 0 END) AS DangMuon,
    SUM(CASE WHEN cs.TrangThai = N'Hỏng' THEN 1 ELSE 0 END) AS HuHong,
    SUM(CASE WHEN cs.TrangThai = N'Mất' THEN 1 ELSE 0 END) AS BiMat
FROM Sach s
JOIN TheLoai tl ON tl.MaTheLoai = s.MaTheLoai
LEFT JOIN CuonSach cs ON cs.MaSach = s.MaSach
GROUP BY s.MaSach, s.MaSachHienThi, s.TenSach, tl.TenTheLoai;
GO

CREATE OR ALTER VIEW vw_ChiTietSach
AS
SELECT
    s.MaSach,
    s.MaSachHienThi,
    s.TenSach,
    s.ISBN,
    s.NamXuatBan,
    s.NgonNgu,
    s.SoTrang,
    s.GiaBia,
    s.MoTa,
    s.AnhBia,
    tl.TenTheLoai,
    nxb.TenNXB,
    STRING_AGG(tg.TenTacGia, N', ') AS TacGia
FROM Sach s
JOIN TheLoai tl ON tl.MaTheLoai = s.MaTheLoai
LEFT JOIN NhaXuatBan nxb ON nxb.MaNXB = s.MaNXB
LEFT JOIN Sach_TacGia stg ON stg.MaSach = s.MaSach
LEFT JOIN TacGia tg ON tg.MaTacGia = stg.MaTacGia
GROUP BY s.MaSach, s.MaSachHienThi, s.TenSach, s.ISBN, s.NamXuatBan,
         s.NgonNgu, s.SoTrang, s.GiaBia, s.MoTa, s.AnhBia,
         tl.TenTheLoai, nxb.TenNXB;
GO

/* =========================================================
   12. DỮ LIỆU CẤU HÌNH BAN ĐẦU
   ========================================================= */
INSERT INTO VaiTro (TenVaiTro, MoTa)
VALUES
    (N'Quản trị viên', N'Toàn quyền hệ thống'),
    (N'Thủ thư', N'Quản lý mượn trả và độc giả'),
    (N'Quản lý sách', N'Quản lý đầu sách, cuốn sách và nhập sách'),
    (N'Độc giả', N'Tra cứu, đặt trước và đánh giá sách');
GO

INSERT INTO QuyDinh (
    TenQuyDinh, SoNgayMuonToiDa, SoSachMuonToiDa,
    PhiThuongNien, TienPhatMoiNgay, TyLePhatHong, TyLePhatMat
)
VALUES (
    N'Quy định mặc định', 14, 3,
    100000, 5000, 50, 100
);
GO

INSERT INTO LoaiThongBao (TenLoai, Icon, Mau, MoTa)
VALUES
    (N'Sách quá hạn', N'TriangleExclamation', N'#FF7A00', N'Cảnh báo sách quá hạn'),
    (N'Độc giả', N'User', N'#1677FF', N'Thông báo liên quan độc giả'),
    (N'Sách mới', N'BookOpen', N'#19A866', N'Thông báo sách mới nhập'),
    (N'Thông báo chung', N'Bullhorn', N'#6536D9', N'Thông báo chung hệ thống'),
    (N'Đặt trước', N'Bookmark', N'#8A4FFF', N'Thông báo đặt trước sách');
GO

/* =========================================================
   13. PHÂN QUYỀN VÀ CHỨC NĂNG HỆ THỐNG
   ========================================================= */
IF OBJECT_ID(N'dbo.ChucNang', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.ChucNang (
        MaChucNang nvarchar(50) NOT NULL CONSTRAINT PK_ChucNang PRIMARY KEY,
        MaNhom nvarchar(30) NOT NULL,
        TenNhom nvarchar(100) NOT NULL,
        TenChucNang nvarchar(100) NOT NULL,
        ThuTu int NOT NULL CONSTRAINT DF_ChucNang_ThuTu DEFAULT (0)
    );
    CREATE INDEX IX_ChucNang_Nhom_ThuTu ON dbo.ChucNang(MaNhom, ThuTu);
END;
GO

IF OBJECT_ID(N'dbo.PhanQuyen', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.PhanQuyen (
        MaVaiTro int NOT NULL,
        MaChucNang nvarchar(50) NOT NULL,
        DuocXem bit NOT NULL CONSTRAINT DF_PhanQuyen_Xem DEFAULT (0),
        DuocThem bit NOT NULL CONSTRAINT DF_PhanQuyen_Them DEFAULT (0),
        DuocSua bit NOT NULL CONSTRAINT DF_PhanQuyen_Sua DEFAULT (0),
        DuocXoa bit NOT NULL CONSTRAINT DF_PhanQuyen_Xoa DEFAULT (0),
        DuocIn bit NOT NULL CONSTRAINT DF_PhanQuyen_In DEFAULT (0),
        DuocXuatExcel bit NOT NULL CONSTRAINT DF_PhanQuyen_Excel DEFAULT (0),
        CONSTRAINT PK_PhanQuyen PRIMARY KEY (MaVaiTro, MaChucNang),
        CONSTRAINT FK_PhanQuyen_VaiTro FOREIGN KEY (MaVaiTro) REFERENCES dbo.VaiTro(MaVaiTro) ON DELETE CASCADE,
        CONSTRAINT FK_PhanQuyen_ChucNang FOREIGN KEY (MaChucNang) REFERENCES dbo.ChucNang(MaChucNang) ON DELETE CASCADE
    );
END;
GO

DECLARE @DanhMuc TABLE(MaChucNang nvarchar(50), MaNhom nvarchar(30), TenNhom nvarchar(100), TenChucNang nvarchar(100), ThuTu int);
INSERT @DanhMuc VALUES
(N'SACH.DANHSACH',N'SACH',N'Quản lý sách',N'Danh sách sách',101),(N'SACH.DAUSACH',N'SACH',N'Quản lý sách',N'Danh mục đầu sách',102),(N'SACH.THELOAI',N'SACH',N'Quản lý sách',N'Danh mục thể loại',103),
(N'DOCGIA.DANHSACH',N'DOCGIA',N'Quản lý độc giả',N'Danh sách độc giả',201),(N'DOCGIA.THEDOCGIA',N'DOCGIA',N'Quản lý độc giả',N'Thẻ độc giả',202),(N'DOCGIA.LEPHI',N'DOCGIA',N'Quản lý độc giả',N'Thu lệ phí',203),
(N'MUONTRA.MUON',N'MUONTRA',N'Mượn – Trả – Phạt',N'Lập phiếu mượn',301),(N'MUONTRA.TRA',N'MUONTRA',N'Mượn – Trả – Phạt',N'Lập phiếu trả',302),(N'MUONTRA.PHAT',N'MUONTRA',N'Mượn – Trả – Phạt',N'Quản lý tiền phạt',303),
(N'NHAPSACH.LAPPHIEU',N'NHAPSACH',N'Nhập sách',N'Lập phiếu nhập',401),(N'NHAPSACH.DANHSACH',N'NHAPSACH',N'Nhập sách',N'Danh sách phiếu nhập',402),(N'NHAPSACH.NHACUNGCAP',N'NHAPSACH',N'Nhập sách',N'Nhà cung cấp',403),
(N'BAOCAO.MUONTRA',N'BAOCAO',N'Thống kê báo cáo',N'Báo cáo mượn trả',501),(N'BAOCAO.SACH',N'BAOCAO',N'Thống kê báo cáo',N'Báo cáo sách',502),(N'BAOCAO.DOCGIA',N'BAOCAO',N'Thống kê báo cáo',N'Báo cáo độc giả',503),
(N'HETHONG.NHANVIEN',N'HETHONG',N'Hệ thống',N'Quản lý nhân viên',601),(N'HETHONG.TAIKHOAN',N'HETHONG',N'Hệ thống',N'Quản lý tài khoản',602),(N'HETHONG.CAUHINH',N'HETHONG',N'Hệ thống',N'Cấu hình hệ thống',603),(N'HETHONG.NHATKY',N'HETHONG',N'Hệ thống',N'Nhật ký hệ thống',604),
(N'HETHONG.THONGBAO',N'HETHONG',N'Hệ thống',N'Quản lý thông báo',605);

MERGE dbo.ChucNang AS t USING @DanhMuc AS s ON t.MaChucNang=s.MaChucNang
WHEN MATCHED THEN UPDATE SET MaNhom=s.MaNhom,TenNhom=s.TenNhom,TenChucNang=s.TenChucNang,ThuTu=s.ThuTu
WHEN NOT MATCHED THEN INSERT(MaChucNang,MaNhom,TenNhom,TenChucNang,ThuTu) VALUES(s.MaChucNang,s.MaNhom,s.TenNhom,s.TenChucNang,s.ThuTu);

INSERT dbo.PhanQuyen(MaVaiTro,MaChucNang,DuocXem,DuocThem,DuocSua,DuocXoa,DuocIn,DuocXuatExcel)
SELECT v.MaVaiTro,c.MaChucNang,
 CASE WHEN v.TenVaiTro LIKE N'Quản trị%' OR v.TenVaiTro IN (N'Admin', N'Administrator') OR c.MaNhom IN(N'SACH',N'DOCGIA',N'MUONTRA') THEN 1 ELSE 0 END,
 CASE WHEN v.TenVaiTro LIKE N'Quản trị%' OR v.TenVaiTro IN (N'Admin', N'Administrator') OR c.MaNhom IN(N'SACH',N'DOCGIA',N'MUONTRA') THEN 1 ELSE 0 END,
 CASE WHEN v.TenVaiTro LIKE N'Quản trị%' OR v.TenVaiTro IN (N'Admin', N'Administrator') OR c.MaNhom IN(N'SACH',N'DOCGIA',N'MUONTRA') THEN 1 ELSE 0 END,
 CASE WHEN v.TenVaiTro LIKE N'Quản trị%' OR v.TenVaiTro IN (N'Admin', N'Administrator') THEN 1 ELSE 0 END,
 CASE WHEN v.TenVaiTro LIKE N'Quản trị%' OR v.TenVaiTro IN (N'Admin', N'Administrator') OR c.MaNhom IN(N'SACH',N'DOCGIA',N'MUONTRA',N'BAOCAO') THEN 1 ELSE 0 END,
 CASE WHEN v.TenVaiTro LIKE N'Quản trị%' OR v.TenVaiTro IN (N'Admin', N'Administrator') OR c.MaNhom IN(N'SACH',N'DOCGIA',N'NHAPSACH',N'BAOCAO') THEN 1 ELSE 0 END
FROM dbo.VaiTro v CROSS JOIN dbo.ChucNang c
WHERE NOT EXISTS(SELECT 1 FROM dbo.PhanQuyen p WHERE p.MaVaiTro=v.MaVaiTro AND p.MaChucNang=c.MaChucNang);
GO

PRINT N'Đã tạo xong CSDL QuanLyThuVienEPU phiên bản phù hợp nghiệp vụ.';
GO
