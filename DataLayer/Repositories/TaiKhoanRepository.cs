using DataLayer.Context;
using DataLayer.Entities;
using DataLayer.Models;
using DataLayer.Security;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Repositories;

public class TaiKhoanRepository
{
    private readonly Func<AppDbContext> _contextFactory;
    private readonly PasswordHasher _hasher;

    public TaiKhoanRepository() : this(() => new AppDbContext(), new PasswordHasher()) { }

    public TaiKhoanRepository(Func<AppDbContext> contextFactory, PasswordHasher? hasher = null)
    {
        _contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
        _hasher = hasher ?? new PasswordHasher();
    }

    public TaiKhoan? DangNhap(string tenDangNhap, string matKhau)
        => DangNhapChiTiet(tenDangNhap, matKhau).TaiKhoan;

    public KetQuaDangNhap DangNhapChiTiet(string tenDangNhap, string matKhau)
    {
        if (string.IsNullOrWhiteSpace(tenDangNhap) || string.IsNullOrEmpty(matKhau))
            return KetQuaDangNhap.ThatBai(TrangThaiDangNhap.TaiKhoanKhongTonTai);

        using var context = _contextFactory();
        TaiKhoan? account = context.TaiKhoans
            .Include(x => x.MaVaiTroNavigation)
            .Include(x => x.MaNhanVienNavigation)
            .Include(x => x.MaDocGiaNavigation)
            .FirstOrDefault(x => x.TenDangNhap == tenDangNhap.Trim());

        if (account == null)
            return KetQuaDangNhap.ThatBai(TrangThaiDangNhap.TaiKhoanKhongTonTai);

        if (!account.TrangThai)
            return KetQuaDangNhap.ThatBai(TrangThaiDangNhap.TaiKhoanBiVoHieuHoa);

        DateTime now = DateTime.Now;
        if (account.KhoaDen is DateTime khoaDen && khoaDen > now)
            return KetQuaDangNhap.ThatBai(TrangThaiDangNhap.TamThoiBiKhoa, khoaDen);

        bool legacy = !_hasher.IsHash(account.MatKhau);
        bool valid = legacy
            ? string.Equals(account.MatKhau, matKhau, StringComparison.Ordinal)
            : _hasher.Verify(matKhau, account.MatKhau);

        if (!valid)
        {
            account.SoLanDangNhapSai++;
            if (account.SoLanDangNhapSai >= 3)
            {
                account.KhoaDen = now.AddMinutes(15);
                account.SoLanDangNhapSai = 0;
                context.SaveChanges();
                return KetQuaDangNhap.ThatBai(TrangThaiDangNhap.TamThoiBiKhoa, account.KhoaDen);
            }

            context.SaveChanges();
            return KetQuaDangNhap.ThatBai(TrangThaiDangNhap.MatKhauKhongDung);
        }

        if (legacy || _hasher.NeedsRehash(account.MatKhau))
            account.MatKhau = _hasher.Hash(matKhau);
        account.SoLanDangNhapSai = 0;
        account.KhoaDen = null;
        account.LanDangNhapCuoi = now;
        context.SaveChanges();
        return KetQuaDangNhap.ThanhCong(account);
    }

    public List<TaiKhoan> LayTatCaTaiKhoan()
    {
        using var context = _contextFactory();
        return context.TaiKhoans.AsNoTracking()
            .Include(x => x.MaVaiTroNavigation).Include(x => x.MaNhanVienNavigation)
            .Include(x => x.MaDocGiaNavigation).OrderBy(x => x.MaTaiKhoan).ToList();
    }

    public List<VaiTro> LayTatCaVaiTro()
    {
        using var context = _contextFactory();
        return context.VaiTros.AsNoTracking().Include(x => x.TaiKhoans)
            .OrderBy(x => x.MaVaiTro).ToList();
    }

    public int ThemTaiKhoan(TaiKhoan taiKhoan)
    {
        ArgumentNullException.ThrowIfNull(taiKhoan);
        using var context = _contextFactory();
        string username = taiKhoan.TenDangNhap.Trim();
        if (context.TaiKhoans.Any(x => x.TenDangNhap.ToLower() == username.ToLower()))
            throw new InvalidOperationException("Tên đăng nhập đã tồn tại.");
        if (!context.VaiTros.Any(x => x.MaVaiTro == taiKhoan.MaVaiTro))
            throw new InvalidOperationException("Vai trò không tồn tại.");
        taiKhoan.TenDangNhap = username;
        taiKhoan.MatKhau = _hasher.IsHash(taiKhoan.MatKhau) ? taiKhoan.MatKhau : _hasher.Hash(taiKhoan.MatKhau);
        context.TaiKhoans.Add(taiKhoan); context.SaveChanges(); return taiKhoan.MaTaiKhoan;
    }

    public int DangKyTaiKhoan(string hoTen, string tenDangNhap, string email, string soDienThoai, string matKhau)
    {
        using var context = _contextFactory();
        if (context.TaiKhoans.Any(x => x.TenDangNhap == tenDangNhap.Trim()))
            throw new InvalidOperationException("Tên đăng nhập đã tồn tại.");
        if (context.DocGia.Any(x => x.Email == email.Trim()))
            throw new InvalidOperationException("Email đã được sử dụng.");
        if (context.DocGia.Any(x => x.SoDienThoai == soDienThoai.Trim()))
            throw new InvalidOperationException("Số điện thoại đã được sử dụng.");

        using var transaction = context.Database.BeginTransaction();
        try
        {
            var docGia = new DocGium
            {
                HoTen = hoTen.Trim(),
                Email = email.Trim(),
                SoDienThoai = soDienThoai.Trim(),
                LoaiDocGia = "Độc giả",
                NgayDangKy = DateOnly.FromDateTime(DateTime.Today),
                TrangThai = true
            };
            context.DocGia.Add(docGia);
            context.SaveChanges();

            var defaultCard = new TheDocGium
            {
                MaDocGia = docGia.MaDocGia,
                NgayCap = DateOnly.FromDateTime(DateTime.Today),
                NgayHetHan = DateOnly.FromDateTime(DateTime.Today).AddYears(1),
                TrangThai = "Đang hiệu lực",
                MaTheHienThi = $"DG{docGia.MaDocGia:D4}"
            };
            context.TheDocGia.Add(defaultCard);
            context.SaveChanges();

            var taiKhoan = new TaiKhoan
            {
                TenDangNhap = tenDangNhap.Trim(),
                MatKhau = _hasher.IsHash(matKhau) ? matKhau : _hasher.Hash(matKhau),
                MaDocGia = docGia.MaDocGia,
                MaVaiTro = 4, // Độc giả
                TrangThai = true
            };
            context.TaiKhoans.Add(taiKhoan);
            context.SaveChanges();

            transaction.Commit();
            return taiKhoan.MaTaiKhoan;
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    public void CapNhatVaiTroTaiKhoan(int maTaiKhoan, int maVaiTro)
    {
        using var context = _contextFactory();
        var item = context.TaiKhoans.Find(maTaiKhoan) ?? throw new InvalidOperationException("Không tìm thấy tài khoản.");
        if (!context.VaiTros.Any(x => x.MaVaiTro == maVaiTro)) throw new InvalidOperationException("Vai trò không tồn tại.");
        bool currentIsAdmin = LaVaiTroQuanTri(context, item.MaVaiTro);
        if (currentIsAdmin && !LaVaiTroQuanTri(context, maVaiTro) && item.TrangThai &&
            DemQuanTriHoatDong(context) <= 1)
            throw new InvalidOperationException("Hệ thống phải luôn còn ít nhất một quản trị viên đang hoạt động.");
        item.MaVaiTro = maVaiTro; context.SaveChanges();
    }

    public void DoiTrangThaiTaiKhoan(int maTaiKhoan, bool trangThai)
    {
        using var context = _contextFactory();
        var item = context.TaiKhoans.Find(maTaiKhoan) ?? throw new InvalidOperationException("Không tìm thấy tài khoản.");
        if (!trangThai && LaVaiTroQuanTri(context, item.MaVaiTro) && item.TrangThai && DemQuanTriHoatDong(context) <= 1)
            throw new InvalidOperationException("Không thể khóa quản trị viên đang hoạt động cuối cùng.");
        item.TrangThai = trangThai; context.SaveChanges();
    }

    public void DatLaiMatKhau(int maTaiKhoan, string matKhauMoi)
    {
        using var context = _contextFactory();
        var item = context.TaiKhoans.Find(maTaiKhoan) ?? throw new InvalidOperationException("Không tìm thấy tài khoản.");
        item.MatKhau = _hasher.Hash(matKhauMoi); context.SaveChanges();
    }

    public int ThemVaiTro(string tenVaiTro, string? moTa)
    {
        using var context = _contextFactory();
        if (context.VaiTros.Any(x => x.TenVaiTro == tenVaiTro)) throw new InvalidOperationException("Tên vai trò đã tồn tại.");
        var item = new VaiTro { TenVaiTro = tenVaiTro, MoTa = moTa }; context.VaiTros.Add(item); context.SaveChanges(); return item.MaVaiTro;
    }

    public void SuaVaiTro(int maVaiTro, string tenVaiTro, string? moTa)
    {
        using var context = _contextFactory();
        if (LaVaiTroQuanTri(context, maVaiTro)) throw new InvalidOperationException("Không thể sửa vai trò quản trị hệ thống.");
        var item = context.VaiTros.Find(maVaiTro) ?? throw new InvalidOperationException("Không tìm thấy vai trò.");
        string normalized = tenVaiTro.Trim();
        if (context.VaiTros.Any(x => x.MaVaiTro != maVaiTro && x.TenVaiTro.ToLower() == normalized.ToLower())) throw new InvalidOperationException("Tên vai trò đã tồn tại.");
        item.TenVaiTro = normalized; item.MoTa = moTa; context.SaveChanges();
    }

    public void XoaVaiTro(int maVaiTro)
    {
        using var context = _contextFactory();
        if (LaVaiTroQuanTri(context, maVaiTro)) throw new InvalidOperationException("Không thể xóa vai trò quản trị hệ thống.");
        var item = context.VaiTros.Include(x => x.TaiKhoans).FirstOrDefault(x => x.MaVaiTro == maVaiTro)
            ?? throw new InvalidOperationException("Không tìm thấy vai trò.");
        if (item.TaiKhoans.Count > 0) throw new InvalidOperationException("Vai trò đang được tài khoản sử dụng nên không thể xóa.");
        context.VaiTros.Remove(item); context.SaveChanges();
    }

    public List<PhanQuyen> LayPhanQuyen(int maVaiTro)
    {
        try
        {
            using var context = _contextFactory();
            return context.PhanQuyens.AsNoTracking().Include(x => x.MaChucNangNavigation)
                .Where(x => x.MaVaiTro == maVaiTro)
                .OrderBy(x => x.MaChucNangNavigation.ThuTu).ToList();
        }
        catch
        {
            return new List<PhanQuyen>();
        }
    }

    public List<ChucNang> LayDanhMucChucNang()
    {
        try
        {
            using var context = _contextFactory();
            return context.ChucNangs.AsNoTracking().OrderBy(x => x.ThuTu).ToList();
        }
        catch
        {
            return new List<ChucNang>();
        }
    }

    public void LuuPhanQuyen(int maVaiTro, IEnumerable<PhanQuyen> permissions)
    {
        using var context = _contextFactory();
        if (!context.VaiTros.Any(x => x.MaVaiTro == maVaiTro)) throw new InvalidOperationException("Vai trò không tồn tại.");
        try
        {
            using var transaction = context.Database.BeginTransaction();
            context.PhanQuyens.RemoveRange(context.PhanQuyens.Where(x => x.MaVaiTro == maVaiTro));
            context.SaveChanges();
            context.PhanQuyens.AddRange(permissions);
            context.SaveChanges();
            transaction.Commit();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Không thể lưu phân quyền. Vui lòng đảm bảo bảng PhanQuyen và ChucNang đã được tạo trong CSDL.", ex);
        }
    }

    public bool LaVaiTroQuanTri(int maVaiTro)
    {
        using var context = _contextFactory();
        return LaVaiTroQuanTri(context, maVaiTro);
    }

    private static bool LaVaiTroQuanTri(AppDbContext context, int roleId)
    {
        string? name = context.VaiTros.AsNoTracking().Where(x => x.MaVaiTro == roleId).Select(x => x.TenVaiTro).FirstOrDefault();
        return name != null && (name.Equals("Admin", StringComparison.OrdinalIgnoreCase)
            || name.Equals("Administrator", StringComparison.OrdinalIgnoreCase)
            || name.StartsWith("Quản trị", StringComparison.CurrentCultureIgnoreCase));
    }

    private static int DemQuanTriHoatDong(AppDbContext context)
    {
        int[] adminIds = context.VaiTros.AsNoTracking().AsEnumerable().Where(x =>
            x.TenVaiTro.Equals("Admin", StringComparison.OrdinalIgnoreCase)
            || x.TenVaiTro.Equals("Administrator", StringComparison.OrdinalIgnoreCase)
            || x.TenVaiTro.StartsWith("Quản trị", StringComparison.CurrentCultureIgnoreCase))
            .Select(x => x.MaVaiTro).ToArray();
        return context.TaiKhoans.Count(x => x.TrangThai && adminIds.Contains(x.MaVaiTro));
    }
}
