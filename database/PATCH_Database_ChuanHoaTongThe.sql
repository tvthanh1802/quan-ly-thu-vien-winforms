/* PATCH TỔNG THỂ DATABASE QUẢN LÝ THƯ VIỆN EPU: không xóa dữ liệu, chạy lặp lại. */
USE QuanLyThuVienEPU;
GO
SET NOCOUNT ON;
SET XACT_ABORT ON;
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO
DECLARE @TenSachQuaDai INT=(SELECT COUNT(*) FROM dbo.Sach WHERE LEN(LTRIM(RTRIM(TenSach)))>100);
DECLARE @TenSachRong INT=(SELECT COUNT(*) FROM dbo.Sach WHERE LEN(LTRIM(RTRIM(TenSach)))=0);
DECLARE @GiaBiaKhongHopLe INT=(SELECT COUNT(*) FROM dbo.Sach WHERE GiaBia IS NOT NULL AND GiaBia<=0);
IF @TenSachQuaDai>0 THROW 53001,N'Không thể chuẩn hóa: có tên sách dài quá 100 ký tự.',1;
IF @TenSachRong>0 THROW 53002,N'Không thể chuẩn hóa: có tên sách rỗng hoặc chỉ chứa khoảng trắng.',1;
IF @GiaBiaKhongHopLe>0 THROW 53003,N'Không thể chuẩn hóa: có giá bìa nhỏ hơn hoặc bằng 0.',1;
GO
BEGIN TRY
 BEGIN TRANSACTION;
 IF COL_LENGTH(N'dbo.TaiKhoan',N'SoLanDangNhapSai') IS NULL ALTER TABLE dbo.TaiKhoan ADD SoLanDangNhapSai INT NOT NULL CONSTRAINT DF_TaiKhoan_SoLanDangNhapSai DEFAULT(0);
 IF COL_LENGTH(N'dbo.TaiKhoan',N'KhoaDen') IS NULL ALTER TABLE dbo.TaiKhoan ADD KhoaDen DATETIME2 NULL;
 IF COL_LENGTH(N'dbo.DocGia',N'AnhDaiDien') IS NULL ALTER TABLE dbo.DocGia ADD AnhDaiDien NVARCHAR(255) NULL;
 IF COL_LENGTH(N'dbo.NhanVien',N'AnhDaiDien') IS NULL ALTER TABLE dbo.NhanVien ADD AnhDaiDien NVARCHAR(255) NULL;
 IF COL_LENGTH(N'dbo.TheDocGia',N'NgayKhoa') IS NULL ALTER TABLE dbo.TheDocGia ADD NgayKhoa DATE NULL;
 IF COL_LENGTH(N'dbo.TheDocGia',N'NgayMoKhoaDuKien') IS NULL ALTER TABLE dbo.TheDocGia ADD NgayMoKhoaDuKien DATE NULL;
 IF COL_LENGTH(N'dbo.TheDocGia',N'LyDoKhoa') IS NULL ALTER TABLE dbo.TheDocGia ADD LyDoKhoa NVARCHAR(100) NULL;
 IF COL_LENGTH(N'dbo.TheDocGia',N'LoaiKhoa') IS NULL ALTER TABLE dbo.TheDocGia ADD LoaiKhoa NVARCHAR(20) NULL;
 IF COL_LENGTH(N'dbo.DongPhiThuongNien',N'LoaiLePhi') IS NULL ALTER TABLE dbo.DongPhiThuongNien ADD LoaiLePhi NVARCHAR(50) NOT NULL CONSTRAINT DF_DongPhi_LoaiLePhi DEFAULT N'Lệ phí thường niên';
 IF COL_LENGTH(N'dbo.DongPhiThuongNien',N'SoPhieuThu') IS NULL ALTER TABLE dbo.DongPhiThuongNien ADD SoPhieuThu NVARCHAR(30) NULL;
 IF COL_LENGTH(N'dbo.DongPhiThuongNien',N'HinhThucThu') IS NULL ALTER TABLE dbo.DongPhiThuongNien ADD HinhThucThu NVARCHAR(30) NOT NULL CONSTRAINT DF_DongPhi_HinhThucThu DEFAULT N'Tiền mặt';
 IF EXISTS(SELECT 1 FROM sys.key_constraints WHERE parent_object_id=OBJECT_ID(N'dbo.DongPhiThuongNien') AND name=N'UQ_DongPhi_The_Nam') ALTER TABLE dbo.DongPhiThuongNien DROP CONSTRAINT UQ_DongPhi_The_Nam;
 IF EXISTS(SELECT 1 FROM sys.indexes WHERE object_id=OBJECT_ID(N'dbo.DongPhiThuongNien') AND name=N'UQ_DongPhi_The_Nam') DROP INDEX UQ_DongPhi_The_Nam ON dbo.DongPhiThuongNien;
 IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE object_id=OBJECT_ID(N'dbo.DongPhiThuongNien') AND name=N'UX_DongPhi_ThuongNien_The_Nam') CREATE UNIQUE INDEX UX_DongPhi_ThuongNien_The_Nam ON dbo.DongPhiThuongNien(MaThe,Nam) WHERE LoaiLePhi=N'Lệ phí thường niên' AND TrangThai=N'Đã thanh toán';
 IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE object_id=OBJECT_ID(N'dbo.DongPhiThuongNien') AND name=N'UX_DongPhi_SoPhieuThu') CREATE UNIQUE INDEX UX_DongPhi_SoPhieuThu ON dbo.DongPhiThuongNien(SoPhieuThu) WHERE SoPhieuThu IS NOT NULL;
 IF COL_LENGTH(N'dbo.PhieuNhap',N'TrangThai') IS NULL ALTER TABLE dbo.PhieuNhap ADD TrangThai NVARCHAR(20) NULL;
 IF EXISTS(SELECT 1 FROM sys.columns WHERE object_id=OBJECT_ID(N'dbo.PhieuNhap') AND name=N'TrangThai' AND is_nullable=1)
 BEGIN
  EXEC(N'UPDATE dbo.PhieuNhap SET TrangThai=CASE WHEN GhiChu LIKE N''[[]Đã hủy[]]%'' OR GhiChu LIKE N''%hủy%'' THEN N''Đã hủy'' WHEN GhiChu LIKE N''[[]Chờ duyệt[]]%'' THEN N''Chờ duyệt'' WHEN GhiChu LIKE N''[[]Đang nhập[]]%'' THEN N''Đang nhập'' ELSE N''Hoàn thành'' END');
  EXEC(N'UPDATE dbo.PhieuNhap SET GhiChu=NULLIF(LTRIM(STUFF(GhiChu,1,CHARINDEX(N'']'',GhiChu),N'''')),N'''') WHERE GhiChu LIKE N''[[]%[]]%''');
  ALTER TABLE dbo.PhieuNhap ALTER COLUMN TrangThai NVARCHAR(20) NOT NULL;
 END;
 IF NOT EXISTS(SELECT 1 FROM sys.default_constraints WHERE parent_object_id=OBJECT_ID(N'dbo.PhieuNhap') AND name=N'DF_PhieuNhap_TrangThai') ALTER TABLE dbo.PhieuNhap ADD CONSTRAINT DF_PhieuNhap_TrangThai DEFAULT N'Hoàn thành' FOR TrangThai;
 IF NOT EXISTS(SELECT 1 FROM sys.check_constraints WHERE parent_object_id=OBJECT_ID(N'dbo.PhieuNhap') AND name=N'CK_PhieuNhap_TrangThai') ALTER TABLE dbo.PhieuNhap WITH CHECK ADD CONSTRAINT CK_PhieuNhap_TrangThai CHECK(TrangThai IN(N'Đang nhập',N'Chờ duyệt',N'Hoàn thành',N'Đã hủy'));
 IF COL_LENGTH(N'dbo.NhaCungCap',N'TrangThai') IS NULL ALTER TABLE dbo.NhaCungCap ADD TrangThai BIT NOT NULL CONSTRAINT DF_NCC_TrangThai DEFAULT 1;
 IF COL_LENGTH(N'dbo.NhaCungCap',N'NguoiDaiDien') IS NULL ALTER TABLE dbo.NhaCungCap ADD NguoiDaiDien NVARCHAR(100) NULL;
 IF COL_LENGTH(N'dbo.NhaCungCap',N'MaSoThue') IS NULL ALTER TABLE dbo.NhaCungCap ADD MaSoThue NVARCHAR(30) NULL;
 IF COL_LENGTH(N'dbo.NhaCungCap',N'Website') IS NULL ALTER TABLE dbo.NhaCungCap ADD Website NVARCHAR(200) NULL;
 IF COL_LENGTH(N'dbo.NhaCungCap',N'LoaiNcc') IS NULL ALTER TABLE dbo.NhaCungCap ADD LoaiNcc NVARCHAR(50) NULL;
 IF COL_LENGTH(N'dbo.NhaCungCap',N'Logo') IS NULL ALTER TABLE dbo.NhaCungCap ADD Logo NVARCHAR(255) NULL;
 IF COL_LENGTH(N'dbo.NhaCungCap',N'DieuKhoanThanhToan') IS NULL ALTER TABLE dbo.NhaCungCap ADD DieuKhoanThanhToan NVARCHAR(100) NULL;
 IF COL_LENGTH(N'dbo.NhaCungCap',N'NgayBatDauHopTac') IS NULL ALTER TABLE dbo.NhaCungCap ADD NgayBatDauHopTac DATE NULL;
 IF COL_LENGTH(N'dbo.NhaCungCap',N'KhuVucCungCap') IS NULL ALTER TABLE dbo.NhaCungCap ADD KhuVucCungCap NVARCHAR(100) NULL;
 IF COL_LENGTH(N'dbo.NhaCungCap',N'NhomSachCungCap') IS NULL ALTER TABLE dbo.NhaCungCap ADD NhomSachCungCap NVARCHAR(500) NULL;
 IF COL_LENGTH(N'dbo.NhaCungCap',N'GhiChu') IS NULL ALTER TABLE dbo.NhaCungCap ADD GhiChu NVARCHAR(500) NULL;
 IF COL_LENGTH(N'dbo.NhaCungCap',N'HanMucCongNo') IS NULL ALTER TABLE dbo.NhaCungCap ADD HanMucCongNo DECIMAL(18,2) NOT NULL CONSTRAINT DF_NCC_HanMuc DEFAULT 0;
 IF COL_LENGTH(N'dbo.NhaCungCap',N'ChietKhauMacDinh') IS NULL ALTER TABLE dbo.NhaCungCap ADD ChietKhauMacDinh DECIMAL(5,2) NOT NULL CONSTRAINT DF_NCC_ChietKhau DEFAULT 0;
 IF COL_LENGTH(N'dbo.NhaCungCap',N'PhuongThucThanhToan') IS NULL ALTER TABLE dbo.NhaCungCap ADD PhuongThucThanhToan NVARCHAR(50) NULL;
 IF COL_LENGTH(N'dbo.NhaCungCap',N'DanhGiaBanDau') IS NULL ALTER TABLE dbo.NhaCungCap ADD DanhGiaBanDau NVARCHAR(30) NULL;
 IF EXISTS(SELECT 1 FROM sys.columns WHERE object_id=OBJECT_ID(N'dbo.Sach') AND name=N'TenSach' AND max_length<>200)
 BEGIN
  IF EXISTS(SELECT 1 FROM sys.indexes WHERE object_id=OBJECT_ID(N'dbo.Sach') AND name=N'IX_Sach_TenSach') DROP INDEX IX_Sach_TenSach ON dbo.Sach;
  ALTER TABLE dbo.Sach ALTER COLUMN TenSach NVARCHAR(100) NOT NULL;
  CREATE INDEX IX_Sach_TenSach ON dbo.Sach(TenSach);
 END;
 IF EXISTS(SELECT 1 FROM sys.check_constraints WHERE parent_object_id=OBJECT_ID(N'dbo.Sach') AND name=N'CK_Sach_TenSach') ALTER TABLE dbo.Sach DROP CONSTRAINT CK_Sach_TenSach;
 ALTER TABLE dbo.Sach WITH CHECK ADD CONSTRAINT CK_Sach_TenSach CHECK(LEN(LTRIM(RTRIM(TenSach))) BETWEEN 1 AND 100);
 IF EXISTS(SELECT 1 FROM sys.check_constraints WHERE parent_object_id=OBJECT_ID(N'dbo.Sach') AND name=N'CK_Sach_GiaBia') ALTER TABLE dbo.Sach DROP CONSTRAINT CK_Sach_GiaBia;
 ALTER TABLE dbo.Sach WITH CHECK ADD CONSTRAINT CK_Sach_GiaBia CHECK(GiaBia IS NULL OR GiaBia>0);
 IF OBJECT_ID(N'dbo.ChiTietPhat',N'U') IS NOT NULL AND EXISTS(SELECT 1 FROM sys.columns WHERE object_id=OBJECT_ID(N'dbo.ChiTietPhat') AND name=N'MaChiTietMuon' AND is_nullable=0)
 BEGIN
  IF EXISTS(SELECT 1 FROM sys.foreign_keys WHERE parent_object_id=OBJECT_ID(N'dbo.ChiTietPhat') AND name=N'FK_CTP_ChiTietMuon') ALTER TABLE dbo.ChiTietPhat DROP CONSTRAINT FK_CTP_ChiTietMuon;
  ALTER TABLE dbo.ChiTietPhat ALTER COLUMN MaChiTietMuon INT NULL;
  ALTER TABLE dbo.ChiTietPhat WITH CHECK ADD CONSTRAINT FK_CTP_ChiTietMuon FOREIGN KEY(MaChiTietMuon) REFERENCES dbo.ChiTietMuon(MaChiTietMuon);
 END;
 IF OBJECT_ID(N'dbo.ChucNang',N'U') IS NULL
 BEGIN
  CREATE TABLE dbo.ChucNang(MaChucNang NVARCHAR(50) NOT NULL CONSTRAINT PK_ChucNang PRIMARY KEY,MaNhom NVARCHAR(30) NOT NULL,TenNhom NVARCHAR(100) NOT NULL,TenChucNang NVARCHAR(100) NOT NULL,ThuTu INT NOT NULL CONSTRAINT DF_ChucNang_ThuTu DEFAULT(0));
  CREATE INDEX IX_ChucNang_Nhom_ThuTu ON dbo.ChucNang(MaNhom,ThuTu);
 END;
 IF OBJECT_ID(N'dbo.PhanQuyen',N'U') IS NULL
  CREATE TABLE dbo.PhanQuyen(MaVaiTro INT NOT NULL,MaChucNang NVARCHAR(50) NOT NULL,DuocXem BIT NOT NULL CONSTRAINT DF_PhanQuyen_Xem DEFAULT(0),DuocThem BIT NOT NULL CONSTRAINT DF_PhanQuyen_Them DEFAULT(0),DuocSua BIT NOT NULL CONSTRAINT DF_PhanQuyen_Sua DEFAULT(0),DuocXoa BIT NOT NULL CONSTRAINT DF_PhanQuyen_Xoa DEFAULT(0),DuocIn BIT NOT NULL CONSTRAINT DF_PhanQuyen_In DEFAULT(0),DuocXuatExcel BIT NOT NULL CONSTRAINT DF_PhanQuyen_Excel DEFAULT(0),CONSTRAINT PK_PhanQuyen PRIMARY KEY(MaVaiTro,MaChucNang),CONSTRAINT FK_PhanQuyen_VaiTro FOREIGN KEY(MaVaiTro) REFERENCES dbo.VaiTro(MaVaiTro) ON DELETE CASCADE,CONSTRAINT FK_PhanQuyen_ChucNang FOREIGN KEY(MaChucNang) REFERENCES dbo.ChucNang(MaChucNang) ON DELETE CASCADE);
 DECLARE @DanhMuc TABLE(MaChucNang NVARCHAR(50),MaNhom NVARCHAR(30),TenNhom NVARCHAR(100),TenChucNang NVARCHAR(100),ThuTu INT);
 INSERT @DanhMuc VALUES
 (N'SACH.DANHSACH',N'SACH',N'Quản lý sách',N'Danh sách sách',101),(N'SACH.DAUSACH',N'SACH',N'Quản lý sách',N'Danh mục đầu sách',102),(N'SACH.THELOAI',N'SACH',N'Quản lý sách',N'Danh mục thể loại',103),
 (N'DOCGIA.DANHSACH',N'DOCGIA',N'Quản lý độc giả',N'Danh sách độc giả',201),(N'DOCGIA.THEDOCGIA',N'DOCGIA',N'Quản lý độc giả',N'Thẻ độc giả',202),(N'DOCGIA.LEPHI',N'DOCGIA',N'Quản lý độc giả',N'Thu lệ phí',203),
 (N'MUONTRA.MUON',N'MUONTRA',N'Mượn – Trả – Phạt',N'Lập phiếu mượn',301),(N'MUONTRA.TRA',N'MUONTRA',N'Mượn – Trả – Phạt',N'Lập phiếu trả',302),(N'MUONTRA.PHAT',N'MUONTRA',N'Mượn – Trả – Phạt',N'Quản lý tiền phạt',303),
 (N'NHAPSACH.LAPPHIEU',N'NHAPSACH',N'Nhập sách',N'Lập phiếu nhập',401),(N'NHAPSACH.DANHSACH',N'NHAPSACH',N'Nhập sách',N'Danh sách phiếu nhập',402),(N'NHAPSACH.NHACUNGCAP',N'NHAPSACH',N'Nhập sách',N'Nhà cung cấp',403),
 (N'BAOCAO.MUONTRA',N'BAOCAO',N'Thống kê báo cáo',N'Báo cáo mượn trả',501),(N'BAOCAO.SACH',N'BAOCAO',N'Thống kê báo cáo',N'Báo cáo sách',502),(N'BAOCAO.DOCGIA',N'BAOCAO',N'Thống kê báo cáo',N'Báo cáo độc giả',503),
 (N'HETHONG.NHANVIEN',N'HETHONG',N'Hệ thống',N'Quản lý nhân viên',601),(N'HETHONG.TAIKHOAN',N'HETHONG',N'Hệ thống',N'Quản lý tài khoản',602),(N'HETHONG.CAUHINH',N'HETHONG',N'Hệ thống',N'Cấu hình hệ thống',603),(N'HETHONG.NHATKY',N'HETHONG',N'Hệ thống',N'Nhật ký hệ thống',604),(N'HETHONG.THONGBAO',N'HETHONG',N'Hệ thống',N'Quản lý thông báo',605);
 MERGE dbo.ChucNang AS t USING @DanhMuc AS s ON t.MaChucNang=s.MaChucNang WHEN MATCHED THEN UPDATE SET MaNhom=s.MaNhom,TenNhom=s.TenNhom,TenChucNang=s.TenChucNang,ThuTu=s.ThuTu WHEN NOT MATCHED THEN INSERT(MaChucNang,MaNhom,TenNhom,TenChucNang,ThuTu) VALUES(s.MaChucNang,s.MaNhom,s.TenNhom,s.TenChucNang,s.ThuTu);
 INSERT dbo.PhanQuyen(MaVaiTro,MaChucNang,DuocXem,DuocThem,DuocSua,DuocXoa,DuocIn,DuocXuatExcel)
 SELECT v.MaVaiTro,c.MaChucNang,CASE WHEN v.MaVaiTro=1 OR c.MaNhom IN(N'SACH',N'DOCGIA',N'MUONTRA') THEN 1 ELSE 0 END,CASE WHEN v.MaVaiTro=1 OR c.MaNhom IN(N'SACH',N'DOCGIA',N'MUONTRA') THEN 1 ELSE 0 END,CASE WHEN v.MaVaiTro=1 OR c.MaNhom IN(N'SACH',N'DOCGIA',N'MUONTRA') THEN 1 ELSE 0 END,CASE WHEN v.MaVaiTro=1 THEN 1 ELSE 0 END,CASE WHEN v.MaVaiTro=1 OR c.MaNhom IN(N'SACH',N'DOCGIA',N'MUONTRA',N'BAOCAO') THEN 1 ELSE 0 END,CASE WHEN v.MaVaiTro=1 OR c.MaNhom IN(N'SACH',N'DOCGIA',N'NHAPSACH',N'BAOCAO') THEN 1 ELSE 0 END FROM dbo.VaiTro v CROSS JOIN dbo.ChucNang c WHERE NOT EXISTS(SELECT 1 FROM dbo.PhanQuyen p WHERE p.MaVaiTro=v.MaVaiTro AND p.MaChucNang=c.MaChucNang);
 UPDATE p SET DuocXem=1,DuocThem=1,DuocSua=1,DuocXoa=1,DuocIn=1,DuocXuatExcel=1 FROM dbo.PhanQuyen p JOIN dbo.VaiTro v ON v.MaVaiTro=p.MaVaiTro WHERE v.TenVaiTro LIKE N'Quản trị%' OR v.TenVaiTro IN(N'Admin',N'Administrator');
 UPDATE ctm
 SET ctm.TrangThai=CASE WHEN ct.TinhTrangTra=N'Mất' THEN N'Mất' WHEN ct.TinhTrangTra IN(N'Rách nhẹ',N'Hư hỏng') THEN N'Hỏng' ELSE N'Đã trả' END
 FROM dbo.ChiTietMuon ctm
 JOIN dbo.ChiTietTra ct ON ct.MaChiTietMuon=ctm.MaChiTietMuon
 WHERE ctm.TrangThai<>CASE WHEN ct.TinhTrangTra=N'Mất' THEN N'Mất' WHEN ct.TinhTrangTra IN(N'Rách nhẹ',N'Hư hỏng') THEN N'Hỏng' ELSE N'Đã trả' END;
 UPDATE cs
 SET cs.TinhTrang=lanTraCuoi.TinhTrangTra,
     cs.TrangThai=CASE WHEN lanTraCuoi.TinhTrangTra=N'Mất' THEN N'Mất' WHEN lanTraCuoi.TinhTrangTra IN(N'Rách nhẹ',N'Hư hỏng') THEN N'Hỏng' ELSE N'Có sẵn' END
 FROM dbo.CuonSach cs
 CROSS APPLY(
  SELECT TOP(1) ct.TinhTrangTra
  FROM dbo.ChiTietMuon ctm
  JOIN dbo.ChiTietTra ct ON ct.MaChiTietMuon=ctm.MaChiTietMuon
  JOIN dbo.PhieuTra pt ON pt.MaPhieuTra=ct.MaPhieuTra
  WHERE ctm.MaCuonSach=cs.MaCuonSach
  ORDER BY pt.NgayTra DESC,ct.MaChiTietTra DESC
 ) lanTraCuoi
 WHERE NOT EXISTS(SELECT 1 FROM dbo.ChiTietMuon dangMuon WHERE dangMuon.MaCuonSach=cs.MaCuonSach AND dangMuon.TrangThai IN(N'Đang mượn',N'Quá hạn'))
   AND (cs.TinhTrang<>lanTraCuoi.TinhTrangTra
    OR cs.TrangThai<>CASE WHEN lanTraCuoi.TinhTrangTra=N'Mất' THEN N'Mất' WHEN lanTraCuoi.TinhTrangTra IN(N'Rách nhẹ',N'Hư hỏng') THEN N'Hỏng' ELSE N'Có sẵn' END);
 COMMIT TRANSACTION;
END TRY
BEGIN CATCH
 IF XACT_STATE()<>0 ROLLBACK TRANSACTION;
 THROW;
END CATCH;
GO
CREATE OR ALTER TRIGGER dbo.TRG_ChiTietTra_CapNhatTrangThai ON dbo.ChiTietTra AFTER INSERT AS
BEGIN
 SET NOCOUNT ON;
 UPDATE ctm SET ctm.TrangThai=CASE WHEN i.TinhTrangTra=N'Mất' THEN N'Mất' WHEN i.TinhTrangTra IN(N'Rách nhẹ',N'Hư hỏng') THEN N'Hỏng' ELSE N'Đã trả' END FROM dbo.ChiTietMuon ctm JOIN inserted i ON i.MaChiTietMuon=ctm.MaChiTietMuon;
 UPDATE cs SET cs.TinhTrang=i.TinhTrangTra,cs.TrangThai=CASE WHEN i.TinhTrangTra=N'Mất' THEN N'Mất' WHEN i.TinhTrangTra IN(N'Rách nhẹ',N'Hư hỏng') THEN N'Hỏng' ELSE N'Có sẵn' END FROM dbo.CuonSach cs JOIN dbo.ChiTietMuon ctm ON ctm.MaCuonSach=cs.MaCuonSach JOIN inserted i ON i.MaChiTietMuon=ctm.MaChiTietMuon;
 UPDATE pm SET pm.TrangThai=N'Đã trả' FROM dbo.PhieuMuon pm WHERE pm.MaPhieuMuon IN(SELECT DISTINCT pt.MaPhieuMuon FROM inserted i JOIN dbo.PhieuTra pt ON pt.MaPhieuTra=i.MaPhieuTra) AND NOT EXISTS(SELECT 1 FROM dbo.ChiTietMuon ctm WHERE ctm.MaPhieuMuon=pm.MaPhieuMuon AND ctm.TrangThai IN(N'Đang mượn',N'Quá hạn'));
END;
GO
PRINT N'PATCH_Database_ChuanHoaTongThe hoàn tất.';
GO
