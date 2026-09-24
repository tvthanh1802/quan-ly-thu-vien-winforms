using Presentation.Models;

namespace Presentation.SmokeTests;

/// Kiểm thử Smoke Test giao diện các Form CRUD: Đảm bảo các form khởi tạo và hủy thành công trong luồng STA mà không bị treo hay lặp vô tận.
public sealed class CrudFormSmokeTests
{
    public static IEnumerable<object[]> SafeForms()
    {
        yield return new object[] { (Func<Form>)(() => new FrmThemNhanVien()) };
        yield return new object[] { (Func<Form>)(() => new FrmThemDocGia()) };
        yield return new object[] { (Func<Form>)(() => new FrmThemSach()) };
        yield return new object[] { (Func<Form>)(() => new FrmThemThongBao()) };
        yield return new object[] { (Func<Form>)(() => new FrmLapPhieuMuon()) };
        yield return new object[] { (Func<Form>)(() => new FrmLapPhieuPhat()) };
        yield return new object[] { (Func<Form>)(() => new FrmTiepNhanTraSach()) };
    }

    /// Smoke Test: Kiểm tra các Form thêm mới/lập phiếu CRUD có thể khởi tạo và Dispose thành công trên luồng STA mà không bị treo hay ném lỗi khởi tạo.
    [Theory]
    [MemberData(nameof(SafeForms))]
    public void FormCrud_CoTheKhoiTaoVaDispose(Func<Form> factory)
    {
        Exception? error = null;
        var thread = new Thread(() =>
        {
            try { using Form form = factory(); Assert.False(form.IsDisposed); }
            catch (Exception ex) { error = ex; }
        });
        thread.SetApartmentState(ApartmentState.STA);
        thread.Start(); thread.Join(TimeSpan.FromSeconds(15));
        Assert.False(thread.IsAlive, "Form constructor bị treo quá 15 giây.");
        Assert.Null(error);
    }

    /// Form trả sách phải có cột checkbox chọn từng cuốn với tên duy nhất.
    [Fact]
    public void FrmTiepNhanTraSach_CoCotCheckboxChonTra()
    {
        Exception? error = null;
        var thread = new Thread(() =>
        {
            try
            {
                using var form = new FrmTiepNhanTraSach();
                var field = typeof(FrmTiepNhanTraSach).GetField("colChonTra", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                var column = Assert.IsType<DataGridViewCheckBoxColumn>(field?.GetValue(form));
                Assert.Equal("colChonTra", column.Name);
            }
            catch (Exception ex) { error = ex; }
        });
        thread.SetApartmentState(ApartmentState.STA);
        thread.Start(); thread.Join(TimeSpan.FromSeconds(15));
        Assert.False(thread.IsAlive, "Form trả sách bị treo quá 15 giây.");
        Assert.Null(error);
    }

    /// Form thu tiền phải hỗ trợ mở trực tiếp đúng phiếu phạt vừa tạo từ luồng trả sách.
    [Fact]
    public void FrmThuTienPhat_CoTheKhoiTaoVoiMaPhieuPhat()
    {
        Exception? error = null;
        var thread = new Thread(() =>
        {
            try
            {
                using var form = new FrmThuTienPhat(1);
                Assert.NotNull(form);
            }
            catch (Exception ex) { error = ex; }
        });
        thread.SetApartmentState(ApartmentState.STA);
        thread.Start(); thread.Join(TimeSpan.FromSeconds(15));
        Assert.False(thread.IsAlive, "Form thu tiền phạt bị treo quá 15 giây.");
        Assert.Null(error);
    }
}


