USE QuanLyThuVienEPU;
GO
SET NOCOUNT ON; SET XACT_ABORT ON;
SET QUOTED_IDENTIFIER ON; SET ANSI_NULLS ON; SET ANSI_WARNINGS ON; SET ANSI_PADDING ON; SET ARITHABORT ON; SET CONCAT_NULL_YIELDS_NULL ON; SET NUMERIC_ROUNDABORT OFF;
GO
BEGIN TRY
 BEGIN TRANSACTION;
 ;WITH Nguon AS(
  SELECT t.MaThongBao,t.MaLoaiThongBao,t.MaDocGia,t.MaNhanVien,
   ROW_NUMBER() OVER(ORDER BY t.MaThongBao) rn,
   COALESCE(d.HoTen,nv.HoTen,N'bạn') TenNguoi,
   s.TenSach
  FROM dbo.ThongBao t
  LEFT JOIN dbo.DocGia d ON d.MaDocGia=t.MaDocGia
  LEFT JOIN dbo.NhanVien nv ON nv.MaNhanVien=t.MaNhanVien
  CROSS APPLY(SELECT TOP(1) TenSach FROM dbo.Sach WHERE MaSach=1+(t.MaThongBao*37)%310) s
  WHERE t.TieuDe=N'Thông báo thư viện' AND t.NoiDung=N'Nội dung thông báo từ Thư viện EPU.'
 )
 UPDATE t SET
  TieuDe=CASE n.MaLoaiThongBao
   WHEN 1 THEN CHOOSE(1+n.rn%4,N'Nhắc trả sách đúng hạn',N'Cảnh báo sách đang quá hạn',N'Phí phạt quá hạn dự kiến',N'Xác nhận tiếp nhận sách trả muộn')
   WHEN 2 THEN CHOOSE(1+n.rn%4,N'Thẻ thư viện sắp hết hạn',N'Xác nhận phí thường niên',N'Hồ sơ độc giả đã được cập nhật',N'Chào mừng bạn đến Thư viện EPU')
   WHEN 3 THEN CHOOSE(1+n.rn%4,N'Sách mới đã có tại thư viện',N'Giáo trình mới dành cho bạn',N'Gợi ý tài liệu học tập',N'Khám phá sách mới trong tuần')
   WHEN 4 THEN CHOOSE(1+n.rn%4,N'Thông báo lịch phục vụ',N'Bảo trì hệ thống thư viện',N'Nhắc nhở nội quy phòng đọc',N'Sự kiện đọc sách tại EPU')
   ELSE CHOOSE(1+n.rn%4,N'Đặt trước sách thành công',N'Sách đặt trước đã sẵn sàng',N'Sắp hết hạn giữ sách',N'Cập nhật yêu cầu đặt trước') END,
  NoiDung=CASE n.MaLoaiThongBao
   WHEN 1 THEN CONCAT(N'Kính gửi ',n.TenNguoi,N', vui lòng kiểm tra thời hạn trả cuốn “',n.TenSach,N'”. Mã nhắc #',FORMAT(n.MaThongBao,N'0000'),N'; liên hệ quầy thủ thư nếu cần gia hạn.')
   WHEN 2 THEN CONCAT(N'Kính gửi ',n.TenNguoi,N', thông tin thẻ và hồ sơ thư viện của bạn vừa được hệ thống rà soát. Mã giao dịch #',FORMAT(n.MaThongBao,N'0000'),N'.')
   WHEN 3 THEN CONCAT(N'Thư viện EPU giới thiệu “',n.TenSach,N'”. ',n.TenNguoi,N' có thể tra cứu vị trí và tình trạng cuốn sách trên hệ thống. Mã giới thiệu #',FORMAT(n.MaThongBao,N'0000'),N'.')
   WHEN 4 THEN CONCAT(N'Kính gửi ',n.TenNguoi,N', Thư viện EPU gửi thông tin hoạt động và dịch vụ kỳ này. Vui lòng theo dõi lịch tại quầy hoặc trên hệ thống. Mã thông báo #',FORMAT(n.MaThongBao,N'0000'),N'.')
   ELSE CONCAT(N'Kính gửi ',n.TenNguoi,N', yêu cầu đặt trước cuốn “',n.TenSach,N'” đã được cập nhật. Vui lòng nhận sách đúng thời hạn. Mã đặt trước #',FORMAT(n.MaThongBao,N'0000'),N'.') END
 FROM dbo.ThongBao t JOIN Nguon n ON n.MaThongBao=t.MaThongBao;
 DECLARE @DaSua INT=@@ROWCOUNT;
 IF EXISTS(SELECT 1 FROM dbo.ThongBao WHERE TieuDe=N'Thông báo thư viện' AND NoiDung=N'Nội dung thông báo từ Thư viện EPU.') THROW 53301,N'Vẫn còn thông báo mặc định cũ.',1;
 IF (SELECT COUNT(DISTINCT TieuDe) FROM dbo.ThongBao)<20 THROW 53302,N'Chưa đủ 20 tiêu đề khác nhau.',1;
 IF (SELECT COUNT(DISTINCT CONVERT(NVARCHAR(4000),NoiDung)) FROM dbo.ThongBao)<100 THROW 53303,N'Chưa đủ 100 nội dung khác nhau.',1;
 COMMIT;
 SELECT @DaSua SoDongDaSua,(SELECT COUNT(DISTINCT TieuDe) FROM dbo.ThongBao) TieuDeKhac,(SELECT COUNT(DISTINCT CONVERT(NVARCHAR(4000),NoiDung)) FROM dbo.ThongBao) NoiDungKhac;
END TRY
BEGIN CATCH IF XACT_STATE()<>0 ROLLBACK; THROW; END CATCH;
GO
