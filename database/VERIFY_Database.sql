/* Chạy sau schema, seed và các patch. Script THROW nếu phát hiện lỗi. */
USE QuanLyThuVienEPU;
GO
SET NOCOUNT ON;

DECLARE @Loi TABLE (Nhom NVARCHAR(100), SoLoi INT, ChiTiet NVARCHAR(500));

INSERT @Loi
SELECT N'Schema',1,N'Thiếu cột TaiKhoan.SoLanDangNhapSai' WHERE COL_LENGTH(N'dbo.TaiKhoan',N'SoLanDangNhapSai') IS NULL
UNION ALL SELECT N'Schema',1,N'Thiếu cột TaiKhoan.KhoaDen' WHERE COL_LENGTH(N'dbo.TaiKhoan',N'KhoaDen') IS NULL
UNION ALL SELECT N'Schema',1,N'Thiếu cột DocGia.AnhDaiDien' WHERE COL_LENGTH(N'dbo.DocGia',N'AnhDaiDien') IS NULL
UNION ALL SELECT N'Schema',1,N'Thiếu cột NhanVien.AnhDaiDien' WHERE COL_LENGTH(N'dbo.NhanVien',N'AnhDaiDien') IS NULL
UNION ALL SELECT N'Schema',1,N'Thiếu các cột khóa TheDocGia' WHERE COL_LENGTH(N'dbo.TheDocGia',N'NgayKhoa') IS NULL OR COL_LENGTH(N'dbo.TheDocGia',N'NgayMoKhoaDuKien') IS NULL OR COL_LENGTH(N'dbo.TheDocGia',N'LyDoKhoa') IS NULL OR COL_LENGTH(N'dbo.TheDocGia',N'LoaiKhoa') IS NULL
UNION ALL SELECT N'Schema',1,N'Thiếu các cột mở rộng DongPhiThuongNien' WHERE COL_LENGTH(N'dbo.DongPhiThuongNien',N'LoaiLePhi') IS NULL OR COL_LENGTH(N'dbo.DongPhiThuongNien',N'SoPhieuThu') IS NULL OR COL_LENGTH(N'dbo.DongPhiThuongNien',N'HinhThucThu') IS NULL
UNION ALL SELECT N'Schema',1,N'Thiếu filtered unique index lệ phí' WHERE NOT EXISTS(SELECT 1 FROM sys.indexes WHERE object_id=OBJECT_ID(N'dbo.DongPhiThuongNien') AND name=N'UX_DongPhi_ThuongNien_The_Nam' AND is_unique=1) OR NOT EXISTS(SELECT 1 FROM sys.indexes WHERE object_id=OBJECT_ID(N'dbo.DongPhiThuongNien') AND name=N'UX_DongPhi_SoPhieuThu' AND is_unique=1)
UNION ALL SELECT N'Schema',1,N'PhieuNhap.TrangThai thiếu hoặc nullable' WHERE COL_LENGTH(N'dbo.PhieuNhap',N'TrangThai') IS NULL OR EXISTS(SELECT 1 FROM sys.columns WHERE object_id=OBJECT_ID(N'dbo.PhieuNhap') AND name=N'TrangThai' AND is_nullable=1)
UNION ALL SELECT N'Schema',1,N'Thiếu constraint PhieuNhap.TrangThai' WHERE NOT EXISTS(SELECT 1 FROM sys.check_constraints WHERE parent_object_id=OBJECT_ID(N'dbo.PhieuNhap') AND name=N'CK_PhieuNhap_TrangThai' AND is_disabled=0 AND is_not_trusted=0)
UNION ALL SELECT N'Schema',1,N'Thiếu các cột mở rộng NhaCungCap' WHERE COL_LENGTH(N'dbo.NhaCungCap',N'TrangThai') IS NULL OR COL_LENGTH(N'dbo.NhaCungCap',N'NguoiDaiDien') IS NULL OR COL_LENGTH(N'dbo.NhaCungCap',N'MaSoThue') IS NULL OR COL_LENGTH(N'dbo.NhaCungCap',N'Website') IS NULL OR COL_LENGTH(N'dbo.NhaCungCap',N'LoaiNcc') IS NULL OR COL_LENGTH(N'dbo.NhaCungCap',N'Logo') IS NULL OR COL_LENGTH(N'dbo.NhaCungCap',N'DieuKhoanThanhToan') IS NULL OR COL_LENGTH(N'dbo.NhaCungCap',N'NgayBatDauHopTac') IS NULL OR COL_LENGTH(N'dbo.NhaCungCap',N'KhuVucCungCap') IS NULL OR COL_LENGTH(N'dbo.NhaCungCap',N'NhomSachCungCap') IS NULL OR COL_LENGTH(N'dbo.NhaCungCap',N'GhiChu') IS NULL OR COL_LENGTH(N'dbo.NhaCungCap',N'HanMucCongNo') IS NULL OR COL_LENGTH(N'dbo.NhaCungCap',N'ChietKhauMacDinh') IS NULL OR COL_LENGTH(N'dbo.NhaCungCap',N'PhuongThucThanhToan') IS NULL OR COL_LENGTH(N'dbo.NhaCungCap',N'DanhGiaBanDau') IS NULL
UNION ALL SELECT N'Schema',1,N'Thiếu bảng ChucNang hoặc PhanQuyen' WHERE OBJECT_ID(N'dbo.ChucNang',N'U') IS NULL OR OBJECT_ID(N'dbo.PhanQuyen',N'U') IS NULL
UNION ALL SELECT N'Schema',1,N'Thiếu hoặc tắt trigger TRG_ChiTietTra_CapNhatTrangThai' WHERE NOT EXISTS(SELECT 1 FROM sys.triggers WHERE parent_id=OBJECT_ID(N'dbo.ChiTietTra') AND name=N'TRG_ChiTietTra_CapNhatTrangThai' AND is_disabled=0)
UNION ALL SELECT N'Schema',1,N'Thiếu hoặc chưa trust CK_Sach_TenSach' WHERE NOT EXISTS(SELECT 1 FROM sys.check_constraints WHERE parent_object_id=OBJECT_ID(N'dbo.Sach') AND name=N'CK_Sach_TenSach' AND is_disabled=0 AND is_not_trusted=0)
UNION ALL SELECT N'Schema',1,N'Thiếu hoặc chưa trust CK_Sach_GiaBia' WHERE NOT EXISTS(SELECT 1 FROM sys.check_constraints WHERE parent_object_id=OBJECT_ID(N'dbo.Sach') AND name=N'CK_Sach_GiaBia' AND is_disabled=0 AND is_not_trusted=0);

INSERT @Loi
SELECT N'Dữ liệu sách',COUNT(*),N'Tên sách rỗng hoặc dài quá 100 ký tự' FROM dbo.Sach WHERE LEN(LTRIM(RTRIM(TenSach))) NOT BETWEEN 1 AND 100 HAVING COUNT(*)>0
UNION ALL SELECT N'Dữ liệu sách',COUNT(*),N'Giá bìa nhỏ hơn hoặc bằng 0' FROM dbo.Sach WHERE GiaBia IS NOT NULL AND GiaBia<=0 HAVING COUNT(*)>0
UNION ALL SELECT N'Quan hệ',COUNT(*),N'ChiTietMuon mồ côi CuonSach' FROM dbo.ChiTietMuon m LEFT JOIN dbo.CuonSach c ON c.MaCuonSach=m.MaCuonSach WHERE c.MaCuonSach IS NULL HAVING COUNT(*)>0
UNION ALL SELECT N'Quan hệ',COUNT(*),N'ChiTietTra mồ côi ChiTietMuon' FROM dbo.ChiTietTra t LEFT JOIN dbo.ChiTietMuon m ON m.MaChiTietMuon=t.MaChiTietMuon WHERE m.MaChiTietMuon IS NULL HAVING COUNT(*)>0
UNION ALL SELECT N'Mượn trả',COUNT(*),N'ChiTietMuon không đồng bộ với tình trạng trả' FROM dbo.ChiTietMuon m JOIN dbo.ChiTietTra t ON t.MaChiTietMuon=m.MaChiTietMuon WHERE m.TrangThai<>CASE WHEN t.TinhTrangTra=N'Mất' THEN N'Mất' WHEN t.TinhTrangTra IN(N'Rách nhẹ',N'Hư hỏng') THEN N'Hỏng' ELSE N'Đã trả' END HAVING COUNT(*)>0
UNION ALL SELECT N'Mượn trả',COUNT(*),N'CuonSach không đồng bộ với lần trả mới nhất' FROM dbo.CuonSach c CROSS APPLY(SELECT TOP(1) t.TinhTrangTra FROM dbo.ChiTietMuon m JOIN dbo.ChiTietTra t ON t.MaChiTietMuon=m.MaChiTietMuon JOIN dbo.PhieuTra pt ON pt.MaPhieuTra=t.MaPhieuTra WHERE m.MaCuonSach=c.MaCuonSach ORDER BY pt.NgayTra DESC,t.MaChiTietTra DESC) lanTraCuoi WHERE NOT EXISTS(SELECT 1 FROM dbo.ChiTietMuon dangMuon WHERE dangMuon.MaCuonSach=c.MaCuonSach AND dangMuon.TrangThai IN(N'Đang mượn',N'Quá hạn')) AND (c.TinhTrang<>lanTraCuoi.TinhTrangTra OR c.TrangThai<>CASE WHEN lanTraCuoi.TinhTrangTra=N'Mất' THEN N'Mất' WHEN lanTraCuoi.TinhTrangTra IN(N'Rách nhẹ',N'Hư hỏng') THEN N'Hỏng' ELSE N'Có sẵn' END) HAVING COUNT(*)>0
UNION ALL SELECT N'Thông báo',COUNT(*),N'Vẫn còn tiêu đề và nội dung mặc định cũ' FROM dbo.ThongBao WHERE TieuDe=N'Thông báo thư viện' AND NoiDung=N'Nội dung thông báo từ Thư viện EPU.' HAVING COUNT(*)>0
UNION ALL SELECT N'Thông báo',5-COUNT(DISTINCT MaLoaiThongBao),N'Dữ liệu mẫu chưa phủ đủ 5 loại thông báo' FROM dbo.ThongBao HAVING COUNT(DISTINCT MaLoaiThongBao)<5
UNION ALL SELECT N'Thông báo',20-COUNT(DISTINCT TieuDe),N'Có ít hơn 20 tiêu đề khác nhau' FROM dbo.ThongBao HAVING COUNT(DISTINCT TieuDe)<20
UNION ALL SELECT N'Thông báo',100-COUNT(DISTINCT CONVERT(NVARCHAR(4000),NoiDung)),N'Có ít hơn 100 nội dung khác nhau' FROM dbo.ThongBao HAVING COUNT(DISTINCT CONVERT(NVARCHAR(4000),NoiDung))<100
UNION ALL SELECT N'Thông báo',COUNT(*),N'Tiêu đề rỗng hoặc vượt 200 ký tự' FROM dbo.ThongBao WHERE NULLIF(LTRIM(RTRIM(TieuDe)),N'') IS NULL OR LEN(TieuDe)>200 HAVING COUNT(*)>0
UNION ALL SELECT N'Thông báo',COUNT(*),N'Một thông báo đồng thời gán độc giả và nhân viên' FROM dbo.ThongBao WHERE MaDocGia IS NOT NULL AND MaNhanVien IS NOT NULL HAVING COUNT(*)>0
UNION ALL SELECT N'Mượn trả 7 ngày',17-COUNT(*),N'Thiếu lượt mượn seed 7 ngày' FROM dbo.ChiTietMuon ctm JOIN dbo.PhieuMuon pm ON pm.MaPhieuMuon=ctm.MaPhieuMuon WHERE pm.GhiChu LIKE N'\[SEED\_7\_NGAY\]%' ESCAPE N'\' HAVING COUNT(*)<>17
UNION ALL SELECT N'Mượn trả 7 ngày',10-COUNT(*),N'Thiếu lượt trả seed 7 ngày' FROM dbo.ChiTietTra ctt JOIN dbo.PhieuTra pt ON pt.MaPhieuTra=ctt.MaPhieuTra WHERE pt.GhiChu LIKE N'\[SEED\_7\_NGAY\]%' ESCAPE N'\' HAVING COUNT(*)<>10
UNION ALL SELECT N'Mượn trả 7 ngày',COUNT(*),N'Lượt mượn seed nằm ngoài cửa sổ 7 ngày' FROM dbo.PhieuMuon WHERE GhiChu LIKE N'\[SEED\_7\_NGAY\]%' ESCAPE N'\' AND (CAST(NgayMuon AS DATE)<DATEADD(DAY,-6,CAST(GETDATE() AS DATE)) OR CAST(NgayMuon AS DATE)>CAST(GETDATE() AS DATE)) HAVING COUNT(*)>0
UNION ALL SELECT N'Mượn trả 7 ngày',COUNT(*),N'Lượt trả seed nằm ngoài cửa sổ 7 ngày' FROM dbo.PhieuTra WHERE GhiChu LIKE N'\[SEED\_7\_NGAY\]%' ESCAPE N'\' AND (CAST(NgayTra AS DATE)<DATEADD(DAY,-6,CAST(GETDATE() AS DATE)) OR CAST(NgayTra AS DATE)>CAST(GETDATE() AS DATE)) HAVING COUNT(*)>0
UNION ALL SELECT N'Mượn trả 7 ngày',COUNT(*),N'Có lượt trả trước ngày mượn' FROM dbo.PhieuTra pt JOIN dbo.PhieuMuon pm ON pm.MaPhieuMuon=pt.MaPhieuMuon WHERE pt.GhiChu LIKE N'\[SEED\_7\_NGAY\]%' ESCAPE N'\' AND pt.NgayTra<pm.NgayMuon HAVING COUNT(*)>0
UNION ALL SELECT N'Mượn trả 7 ngày',COUNT(*),N'Phiếu mượn seed không có đúng một chi tiết' FROM dbo.PhieuMuon pm OUTER APPLY(SELECT COUNT(*) SoLuong FROM dbo.ChiTietMuon ctm WHERE ctm.MaPhieuMuon=pm.MaPhieuMuon) x WHERE pm.GhiChu LIKE N'\[SEED\_7\_NGAY\]%' ESCAPE N'\' AND x.SoLuong<>1 HAVING COUNT(*)>0
UNION ALL SELECT N'Phân quyền',20-COUNT(*),N'Thiếu mã chức năng chuẩn' FROM dbo.ChucNang WHERE MaChucNang IN(N'SACH.DANHSACH',N'SACH.DAUSACH',N'SACH.THELOAI',N'DOCGIA.DANHSACH',N'DOCGIA.THEDOCGIA',N'DOCGIA.LEPHI',N'MUONTRA.MUON',N'MUONTRA.TRA',N'MUONTRA.PHAT',N'NHAPSACH.LAPPHIEU',N'NHAPSACH.DANHSACH',N'NHAPSACH.NHACUNGCAP',N'BAOCAO.MUONTRA',N'BAOCAO.SACH',N'BAOCAO.DOCGIA',N'HETHONG.NHANVIEN',N'HETHONG.TAIKHOAN',N'HETHONG.CAUHINH',N'HETHONG.NHATKY',N'HETHONG.THONGBAO') HAVING COUNT(*)<20
UNION ALL SELECT N'Phân quyền',COUNT(*),N'Thiếu dòng ma trận vai trò–chức năng' FROM dbo.VaiTro v CROSS JOIN dbo.ChucNang c LEFT JOIN dbo.PhanQuyen p ON p.MaVaiTro=v.MaVaiTro AND p.MaChucNang=c.MaChucNang WHERE p.MaVaiTro IS NULL HAVING COUNT(*)>0
UNION ALL SELECT N'Phân quyền',COUNT(*),N'Quản trị viên thiếu ít nhất một quyền' FROM dbo.PhanQuyen p JOIN dbo.VaiTro v ON v.MaVaiTro=p.MaVaiTro WHERE (v.TenVaiTro LIKE N'Quản trị%' OR v.TenVaiTro IN(N'Admin',N'Administrator')) AND (DuocXem=0 OR DuocThem=0 OR DuocSua=0 OR DuocXoa=0 OR DuocIn=0 OR DuocXuatExcel=0) HAVING COUNT(*)>0;

SELECT N'Sach' Bang,COUNT_BIG(*) SoDong FROM dbo.Sach
UNION ALL SELECT N'DocGia',COUNT_BIG(*) FROM dbo.DocGia
UNION ALL SELECT N'NhanVien',COUNT_BIG(*) FROM dbo.NhanVien
UNION ALL SELECT N'CuonSach',COUNT_BIG(*) FROM dbo.CuonSach
UNION ALL SELECT N'ChiTietMuon',COUNT_BIG(*) FROM dbo.ChiTietMuon
UNION ALL SELECT N'ChiTietTra',COUNT_BIG(*) FROM dbo.ChiTietTra;

IF EXISTS(SELECT 1 FROM @Loi)
BEGIN
 SELECT * FROM @Loi ORDER BY Nhom,ChiTiet;
 THROW 53100,N'VERIFY_Database phát hiện schema hoặc dữ liệu không hợp lệ.',1;
END;

SELECT N'PASS' KetQua,N'Schema, constraint, trigger, quan hệ và bất biến nghiệp vụ đều hợp lệ.' ChiTiet;
GO
