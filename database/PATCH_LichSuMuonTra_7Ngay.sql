/*
    DỮ LIỆU MẪU DASHBOARD: LỊCH SỬ MƯỢN - TRẢ 7 NGÀY GẦN NHẤT
    Chạy lặp lại an toàn bằng marker [SEED_7_NGAY].
*/
USE QuanLyThuVienEPU;
GO
SET QUOTED_IDENTIFIER ON;
SET ANSI_NULLS ON;
SET ANSI_PADDING ON;
SET ANSI_WARNINGS ON;
SET CONCAT_NULL_YIELDS_NULL ON;
SET ARITHABORT ON;
SET NUMERIC_ROUNDABORT OFF;
SET NOCOUNT ON;
SET XACT_ABORT ON;
GO
BEGIN TRY
 BEGIN TRANSACTION;
 DECLARE @Marker NVARCHAR(30)=N'[SEED_7_NGAY]',@MarkerPattern NVARCHAR(40)=N'\[SEED\_7\_NGAY\]%',@HomNay DATE=CAST(GETDATE() AS DATE);
 DECLARE @MaNhanVien INT=(SELECT TOP(1) MaNhanVien FROM dbo.NhanVien WHERE TrangThai=1 ORDER BY MaNhanVien);
 IF @MaNhanVien IS NULL THROW 53201,N'Không có nhân viên hoạt động để tạo lịch sử 7 ngày.',1;

 UPDATE cs SET TrangThai=N'Có sẵn',TinhTrang=N'Tốt'
 FROM dbo.CuonSach cs JOIN dbo.ChiTietMuon ctm ON ctm.MaCuonSach=cs.MaCuonSach
 JOIN dbo.PhieuMuon pm ON pm.MaPhieuMuon=ctm.MaPhieuMuon WHERE pm.GhiChu LIKE @MarkerPattern ESCAPE N'\';
 DELETE ctt FROM dbo.ChiTietTra ctt JOIN dbo.PhieuTra pt ON pt.MaPhieuTra=ctt.MaPhieuTra WHERE pt.GhiChu LIKE @MarkerPattern ESCAPE N'\';
 DELETE FROM dbo.PhieuTra WHERE GhiChu LIKE @MarkerPattern ESCAPE N'\';
 DELETE ctm FROM dbo.ChiTietMuon ctm JOIN dbo.PhieuMuon pm ON pm.MaPhieuMuon=ctm.MaPhieuMuon WHERE pm.GhiChu LIKE @MarkerPattern ESCAPE N'\';
 DELETE FROM dbo.PhieuMuon WHERE GhiChu LIKE @MarkerPattern ESCAPE N'\';

 DECLARE @KeHoach TABLE(ThuTu INT PRIMARY KEY,NgayMuonOffset INT NOT NULL,NgayTraOffset INT NULL,MaThe INT NULL,MaCuonSach INT NULL);
 INSERT @KeHoach VALUES
 (1,-6,-5,NULL,NULL),(2,-6,-4,NULL,NULL),(3,-5,-4,NULL,NULL),(4,-5,-3,NULL,NULL),(5,-5,-2,NULL,NULL),
 (6,-4,-2,NULL,NULL),(7,-3,-2,NULL,NULL),(8,-3,-1,NULL,NULL),(9,-3,-1,NULL,NULL),(10,-3,0,NULL,NULL),
 (11,-2,NULL,NULL,NULL),(12,-2,NULL,NULL,NULL),(13,-1,NULL,NULL,NULL),(14,-1,NULL,NULL,NULL),(15,-1,NULL,NULL,NULL),
 (16,0,NULL,NULL,NULL),(17,0,NULL,NULL,NULL);

 ;WITH t AS(SELECT td.MaThe,ROW_NUMBER() OVER(ORDER BY td.MaThe) rn FROM dbo.TheDocGia td
 WHERE td.TrangThai=N'Đang hiệu lực' AND td.NgayHetHan>=@HomNay
 AND EXISTS(SELECT 1 FROM dbo.DongPhiThuongNien dp WHERE dp.MaThe=td.MaThe AND dp.Nam=YEAR(@HomNay) AND dp.TrangThai=N'Đã thanh toán')
 AND NOT EXISTS(SELECT 1 FROM dbo.PhieuMuon pm JOIN dbo.ChiTietMuon ctm ON ctm.MaPhieuMuon=pm.MaPhieuMuon WHERE pm.MaThe=td.MaThe AND ctm.TrangThai IN(N'Đang mượn',N'Quá hạn') AND pm.HanTra<@HomNay))
 UPDATE k SET MaThe=t.MaThe FROM @KeHoach k JOIN t ON t.rn=k.ThuTu;
 ;WITH c AS(SELECT MaCuonSach,ROW_NUMBER() OVER(ORDER BY MaCuonSach) rn FROM dbo.CuonSach WHERE TrangThai=N'Có sẵn')
 UPDATE k SET MaCuonSach=c.MaCuonSach FROM @KeHoach k JOIN c ON c.rn=k.ThuTu;
 IF EXISTS(SELECT 1 FROM @KeHoach WHERE MaThe IS NULL) THROW 53202,N'Cần ít nhất 17 thẻ hợp lệ.',1;
 IF EXISTS(SELECT 1 FROM @KeHoach WHERE MaCuonSach IS NULL) THROW 53203,N'Cần ít nhất 17 cuốn có sẵn.',1;

 DECLARE @i INT=1,@the INT,@cuon INT,@mo INT,@tr INT,@pm INT,@ctm INT,@pt INT;
 WHILE @i<=17 BEGIN
  SELECT @the=MaThe,@cuon=MaCuonSach,@mo=NgayMuonOffset,@tr=NgayTraOffset FROM @KeHoach WHERE ThuTu=@i;
  INSERT dbo.PhieuMuon(MaThe,MaNhanVien,NgayMuon,HanTra,TrangThai,GhiChu)
  VALUES(@the,@MaNhanVien,DATEADD(HOUR,8,CAST(DATEADD(DAY,@mo,@HomNay) AS DATETIME2)),DATEADD(DAY,14,DATEADD(DAY,@mo,@HomNay)),N'Đang mượn',@Marker+N' Lượt '+CONVERT(NVARCHAR(10),@i));
  SET @pm=CONVERT(INT,SCOPE_IDENTITY());
  INSERT dbo.ChiTietMuon(MaPhieuMuon,MaCuonSach,TrangThai,GhiChu) VALUES(@pm,@cuon,N'Đang mượn',@Marker+N' Chi tiết mượn');
  SELECT @ctm=MaChiTietMuon FROM dbo.ChiTietMuon WHERE MaPhieuMuon=@pm AND MaCuonSach=@cuon;
  IF @tr IS NOT NULL BEGIN
   INSERT dbo.PhieuTra(MaPhieuMuon,MaNhanVien,NgayTra,GhiChu) VALUES(@pm,@MaNhanVien,DATEADD(HOUR,16,CAST(DATEADD(DAY,@tr,@HomNay) AS DATETIME2)),@Marker+N' Trả lượt '+CONVERT(NVARCHAR(10),@i));
   SET @pt=CONVERT(INT,SCOPE_IDENTITY());
   INSERT dbo.ChiTietTra(MaPhieuTra,MaChiTietMuon,TinhTrangTra,SoNgayTre,GhiChu) VALUES(@pt,@ctm,N'Tốt',0,@Marker+N' Chi tiết trả');
  END;
  SET @i+=1;
 END;
 IF (SELECT COUNT(*) FROM dbo.ChiTietMuon c JOIN dbo.PhieuMuon p ON p.MaPhieuMuon=c.MaPhieuMuon WHERE p.GhiChu LIKE @MarkerPattern ESCAPE N'\')<>17 THROW 53204,N'Số lượt mượn seed không bằng 17.',1;
 IF (SELECT COUNT(*) FROM dbo.ChiTietTra c JOIN dbo.PhieuTra p ON p.MaPhieuTra=c.MaPhieuTra WHERE p.GhiChu LIKE @MarkerPattern ESCAPE N'\')<>10 THROW 53205,N'Số lượt trả seed không bằng 10.',1;
 COMMIT;
END TRY
BEGIN CATCH
 IF XACT_STATE()<>0 ROLLBACK;
 THROW;
END CATCH;
GO
SELECT CAST(pm.NgayMuon AS DATE) Ngay,COUNT(*) SoMuon FROM dbo.ChiTietMuon c JOIN dbo.PhieuMuon pm ON pm.MaPhieuMuon=c.MaPhieuMuon WHERE pm.GhiChu LIKE N'\[SEED\_7\_NGAY\]%' ESCAPE N'\' GROUP BY CAST(pm.NgayMuon AS DATE) ORDER BY Ngay;
SELECT CAST(pt.NgayTra AS DATE) Ngay,COUNT(*) SoTra FROM dbo.ChiTietTra c JOIN dbo.PhieuTra pt ON pt.MaPhieuTra=c.MaPhieuTra WHERE pt.GhiChu LIKE N'\[SEED\_7\_NGAY\]%' ESCAPE N'\' GROUP BY CAST(pt.NgayTra AS DATE) ORDER BY Ngay;
GO
PRINT N'PATCH_LichSuMuonTra_7Ngay hoàn tất.';
GO
