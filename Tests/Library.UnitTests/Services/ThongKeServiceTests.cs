using BusinessLayer.Services;

namespace Library.UnitTests.Services;

/// Kiểm thử đơn vị nghiệp vụ thống kê và báo cáo (ThongKeService).
public sealed class ThongKeServiceTests
{
    /// Kiểm tra thống kê Top sách mượn nhiều nhất với khoảng ngày ngược (từ ngày > đến ngày) sẽ ném ArgumentException trước khi truy vấn CSDL.
    [Fact]
    public void TopSach_KhoangNgayNguoc_ThrowsBeforeDatabase()
    {
        var service = new ThongKeService();
        Assert.Throws<ArgumentException>(() =>
            service.GetTopSachMuon(new DateTime(2026, 7, 2), new DateTime(2026, 7, 1)));
    }

    /// Kiểm tra thống kê Top độc giả mượn nhiều nhất với số lượng lấy (take) <= 0 sẽ ném ArgumentOutOfRangeException trước khi truy vấn CSDL.
    [Fact]
    public void TopDocGia_TakeKhongDuong_ThrowsBeforeDatabase()
    {
        var service = new ThongKeService();
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            service.GetTopDocGiaMuon(new DateTime(2026, 7, 1), new DateTime(2026, 7, 2), 0));
    }

    /// Kiểm tra báo cáo mượn quá hạn theo ngày với khoảng ngày ngược (từ ngày > đến ngày) sẽ ném ArgumentException trước khi truy vấn CSDL.
    [Fact]
    public void BaoCaoQuaHan_KhoangNgayNguoc_ThrowsBeforeDatabase()
    {
        var service = new ThongKeService();
        Assert.Throws<ArgumentException>(() =>
            service.GetBaoCaoQuaHanTheoNgay(new DateTime(2026, 7, 2), new DateTime(2026, 7, 1)));
    }

    /// Kiểm tra báo cáo mượn quá hạn với số lượng lấy (take) <= 0 sẽ ném ArgumentOutOfRangeException trước khi truy vấn CSDL.
    [Fact]
    public void BaoCaoQuaHan_TakeKhongDuong_ThrowsBeforeDatabase()
    {
        var service = new ThongKeService();
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            service.GetBaoCaoQuaHanTheoNgay(new DateTime(2026, 7, 1), new DateTime(2026, 7, 2), -1));
    }

    /// Kiểm tra thống kê phiếu nhập sách theo tháng với số tháng mốc <= 0 sẽ ném ArgumentOutOfRangeException trước khi truy vấn CSDL.
    [Fact]
    public void PhieuNhap_SoThangKhongDuong_ThrowsBeforeDatabase()
    {
        var service = new ThongKeService();
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            service.GetThongKePhieuNhapTheoThang(new DateTime(2026, 7, 15), 0));
    }
}


