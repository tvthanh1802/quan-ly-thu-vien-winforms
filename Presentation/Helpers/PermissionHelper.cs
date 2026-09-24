using BusinessLayer.Models;
using BusinessLayer.Services;

namespace Presentation.Helpers;

public static class PermissionHelper
{
    private static readonly Dictionary<string, PhanQuyenGridModel> Permissions = new(StringComparer.OrdinalIgnoreCase);
    public static void Load(int roleId)
    {
        Permissions.Clear();
        foreach (var item in new TaiKhoanService().LayPhanQuyenTheoVaiTro(roleId).Where(x => !x.LaDongNhom))
            Permissions[item.MaChucNang] = item;
    }
    public static void Clear() => Permissions.Clear();
    public static bool CanView(string code) => Get(code)?.DuocXem == true;
    public static bool CanAdd(string code) => Get(code)?.DuocThem == true;
    public static bool CanEdit(string code) => Get(code)?.DuocSua == true;
    public static bool CanDelete(string code) => Get(code)?.DuocXoa == true;
    public static bool CanPrint(string code) => Get(code)?.DuocIn == true;
    public static bool CanExport(string code) => Get(code)?.DuocXuatExcel == true;
    private static PhanQuyenGridModel? Get(string code) => CurrentUser.IsLogin && Permissions.TryGetValue(code, out var item) ? item : null;
}
