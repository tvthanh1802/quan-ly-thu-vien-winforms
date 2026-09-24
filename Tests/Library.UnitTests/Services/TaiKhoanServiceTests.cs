using BusinessLayer.Models;
using BusinessLayer.Services;

namespace Library.UnitTests.Services;

/// Kiểm thử đơn vị nghiệp vụ quản lý tài khoản và phân quyền (TaiKhoanService).
public sealed class TaiKhoanServiceTests
{
    /// Kiểm tra thêm tài khoản mới với dữ liệu không hợp lệ (tên quá ngắn/chứa khoảng trắng, mật khẩu yếu, vai trò <= 0) sẽ ném ArgumentException trước khi truy vấn CSDL.
    [Theory]
    [InlineData("ab", "Abcdef12", 1)]
    [InlineData("ten user", "Abcdef12", 1)]
    [InlineData("valid.user", "1234567", 1)]
    [InlineData("valid.user", "abcdefgh", 1)]
    [InlineData("valid.user", "Abcdef12", 0)]
    public void ThemTaiKhoan_InputSai_ThrowsBeforeDatabase(string user, string password, int role)
        => Assert.Throws<ArgumentException>(() => new TaiKhoanService().ThemTaiKhoan(user, password, role));

    /// Kiểm tra sửa thông tin vai trò với MaVaiTro hoặc tên/mô tả không hợp lệ sẽ ném ArgumentException trước khi truy vấn CSDL.
    [Theory]
    [InlineData(0, "Vai trò", "")]
    [InlineData(2, "", "")]
    public void SuaVaiTro_InputSai_ThrowsBeforeDatabase(int id, string name, string description)
        => Assert.Throws<ArgumentException>(() => new TaiKhoanService().SuaVaiTro(id, name, description));

    /// Kiểm tra hàm chuẩn hóa phân quyền Normalize: Khi tắt quyền Xem (DuocXem = false) thì tất cả các quyền phụ (Thêm, Sửa, Xóa, In, Xuất Excel) tự động bị tắt theo.
    [Fact]
    public void Normalize_TatXem_TatTatCaQuyenPhu()
    {
        var item = new PhanQuyenGridModel { DuocXem=false, DuocThem=true, DuocSua=true, DuocXoa=true, DuocIn=true, DuocXuatExcel=true };
        TaiKhoanService.Normalize(item);
        Assert.False(item.DuocThem || item.DuocSua || item.DuocXoa || item.DuocIn || item.DuocXuatExcel);
    }
}


