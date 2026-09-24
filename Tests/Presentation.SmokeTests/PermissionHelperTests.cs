using Presentation.Helpers;

namespace Presentation.SmokeTests;

/// Kiểm thử đơn vị bộ hỗ trợ kiểm tra phân quyền người dùng trên giao diện (PermissionHelper).
public sealed class PermissionHelperTests
{
    /// Kiểm tra khi người dùng chưa đăng nhập (CurrentUser rỗng), tất cả các hàm kiểm tra quyền (Xem, Thêm, Sửa, Xóa, In, Xuất) mặc định trả về false.
    [Fact]
    public void ChuaDangNhap_MacDinhTuChoiMoiQuyen()
    {
        CurrentUser.Clear();
        Assert.False(PermissionHelper.CanView("SACH.DANHSACH"));
        Assert.False(PermissionHelper.CanAdd("SACH.DANHSACH"));
        Assert.False(PermissionHelper.CanEdit("SACH.DANHSACH"));
        Assert.False(PermissionHelper.CanDelete("SACH.DANHSACH"));
        Assert.False(PermissionHelper.CanPrint("SACH.DANHSACH"));
        Assert.False(PermissionHelper.CanExport("SACH.DANHSACH"));
    }
}


