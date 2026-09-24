using DataLayer.Rules;

namespace Library.UnitTests.Rules;

/// Kiểm thử đơn vị các quy tắc nghiệp vụ Mượn - Trả sách (MuonTraRules).
public sealed class MuonTraRulesTests
{
    private static readonly DateOnly Today = new(2026, 7, 25);

    /// Kiểm tra nhận diện chính xác các trạng thái được tính là đang mượn sách ('Đang mượn', 'Quá hạn').
    [Theory]
    [InlineData("Đang mượn", true)]
    [InlineData("Quá hạn", true)]
    [InlineData("Đã trả", false)]
    [InlineData(null, false)]
    public void LaDangMuon_NhanDienDung(string? status, bool expected) =>
        Assert.Equal(expected, MuonTraRules.LaDangMuon(status));

    /// Kiểm tra tính số ngày trả trễ mượn sách, đảm bảo số ngày không âm khi trả đúng hạn hoặc trả trước hạn.
    [Theory]
    [InlineData(25, 25, 0)]
    [InlineData(25, 26, 1)]
    [InlineData(25, 30, 5)]
    [InlineData(25, 20, 0)]
    public void TinhSoNgayTre_KhongAm(int dueDay, int returnDay, int expected) =>
        Assert.Equal(expected, MuonTraRules.TinhSoNgayTre(new DateOnly(2026, 7, dueDay), new DateOnly(2026, 7, returnDay)));

    /// Kiểm tra tính tiền phạt quá hạn theo công thức: Số ngày trễ * Đơn giá phạt mỗi ngày.
    [Fact]
    public void TinhTienPhatQuaHan_NhanSoNgayVaDonGia() =>
        Assert.Equal(25_000m, MuonTraRules.TinhTienPhatQuaHan(new(2026, 7, 20), Today, 5_000m));

    /// Kiểm tra tính tiền phạt theo tỷ lệ phần trăm giá trị sách (dùng cho trường hợp làm mất hoặc làm hỏng sách).
    [Theory]
    [InlineData(100000, 50, 50000)]
    [InlineData(100000, 100, 100000)]
    [InlineData(-1, 100, 0)]
    public void TinhTienPhatTheoTyLe_HopLe(decimal price, decimal rate, decimal expected) =>
        Assert.Equal(expected, MuonTraRules.TinhTienPhatTheoTyLe(price, rate));

    /// Kiểm tra thứ tự ưu tiên khi xác định trạng thái phiếu mượn (Mất sách > Hư hỏng > Quá hạn > Đã trả).
    [Theory]
    [InlineData(false, true, true, "Mất sách")]
    [InlineData(true, false, true, "Hư hỏng")]
    [InlineData(false, false, true, "Quá hạn")]
    [InlineData(false, false, false, "Đã trả")]
    public void TinhTrangThai_UuTienDung(bool damaged, bool lost, bool borrowing, string expected)
    {
        DateOnly due = borrowing ? Today.AddDays(-1) : Today.AddDays(1);
        Assert.Equal(expected, MuonTraRules.TinhTrangThai(due, Today, borrowing, lost, damaged));
    }

    /// Kiểm tra phiếu mượn đến hạn trả đúng vào ngày hôm nay vẫn ở trạng thái 'Đang mượn', chưa bị coi là quá hạn.
    [Fact]
    public void TinhTrangThai_DenHanHomNay_VanDangMuon() =>
        Assert.Equal("Đang mượn", MuonTraRules.TinhTrangThai(Today, Today, true, false, false));

    /// Kiểm tra điều kiện số lượng sách mượn có vượt quá giới hạn tối đa cho phép của độc giả hay không.
    [Theory]
    [InlineData(2, 1, 3, false)]
    [InlineData(3, 1, 3, true)]
    public void VuotGioiHanSach_DungQuyDinh(int current, int added, int max, bool expected) =>
        Assert.Equal(expected, MuonTraRules.VuotGioiHanSach(current, added, max));

    /// Kiểm tra phân loại chính xác các trạng thái phiếu mượn đã kết thúc ('Đã trả', 'Mất', 'Hỏng').
    [Theory]
    [InlineData("Đã trả", true)]
    [InlineData("Mất", true)]
    [InlineData("Hỏng", true)]
    [InlineData("Đang mượn", false)]
    [InlineData("Quá hạn", false)]
    public void DaKetThuc_PhanLoaiDung(string status, bool expected) =>
        Assert.Equal(expected, MuonTraRules.DaKetThuc(status));

    /// Kiểm tra ngày mượn và ngày hẹn trả có hợp lệ theo số ngày mượn tối đa cho phép hay không.
    [Fact]
    public void NgayMuonHopLe_ChoPhepDungNgayToiDa()
    {
        DateTime ngayMuon = new(2026, 7, 1, 10, 0, 0);
        Assert.True(MuonTraRules.NgayMuonHopLe(ngayMuon, new DateOnly(2026, 7, 15), 14));
        Assert.False(MuonTraRules.NgayMuonHopLe(ngayMuon, new DateOnly(2026, 7, 16), 14));
        Assert.False(MuonTraRules.NgayMuonHopLe(ngayMuon, new DateOnly(2026, 6, 30), 14));
    }
}


