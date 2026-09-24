using DataLayer.Context;
using DataLayer.Entities;
using DataLayer.Models;
using DataLayer.Security;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Repositories;

public sealed class NhanVienRepository
{
    private readonly AppDbContext _context;
    private readonly PasswordHasher _hasher;

    public NhanVienRepository(AppDbContext context, PasswordHasher? hasher = null)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _hasher = hasher ?? new PasswordHasher();
    }

    public List<NhanVienGridModel> GetDanhSach()
    {
        var entities = _context.NhanViens
            .AsNoTracking()
            .Include(nv => nv.TaiKhoan)
                .ThenInclude(tk => tk!.MaVaiTroNavigation)
            .OrderBy(nv => nv.MaNhanVien)
            .ToList();

        return entities.Select(nv => new NhanVienGridModel
        {
            MaNhanVien = nv.MaNhanVien,
            MaNhanVienText = nv.MaNhanVienHienThi ?? $"NV{nv.MaNhanVien:D5}",
            HoTen = nv.HoTen,
            GioiTinh = nv.GioiTinh ?? string.Empty,
            NgaySinh = nv.NgaySinh,
            ChucVu = nv.ChucVu,
            SoDienThoai = nv.SoDienThoai ?? string.Empty,
            Email = nv.Email ?? string.Empty,
            DiaChi = nv.DiaChi ?? string.Empty,
            AnhDaiDien = nv.AnhDaiDien ?? string.Empty,
            NgayVaoLam = nv.NgayVaoLam,
            DangLamViec = nv.TrangThai,
            MaTaiKhoan = nv.TaiKhoan?.MaTaiKhoan,
            TenDangNhap = nv.TaiKhoan?.TenDangNhap ?? string.Empty,
            TaiKhoanHoatDong = nv.TaiKhoan?.TrangThai ?? true,
            MaVaiTro = nv.TaiKhoan?.MaVaiTro,
            TenVaiTro = nv.TaiKhoan?.MaVaiTroNavigation?.TenVaiTro ?? string.Empty
        }).ToList();
    }

    public NhanVienStatisticsModel GetStatistics()
    {
        return new NhanVienStatisticsModel
        {
            TongNhanVien = _context.NhanViens.Count(),
            DangLamViec = _context.NhanViens.Count(x => x.TrangThai),
            TamNghi = _context.NhanViens.Count(x => !x.TrangThai),
            TaiKhoanBiKhoa = _context.TaiKhoans.Count(x => x.MaNhanVien != null && !x.TrangThai)
        };
    }

    public List<string> GetChucVu()
    {
        return _context.NhanViens.AsNoTracking()
            .Select(x => x.ChucVu)
            .Where(x => x != null && x != string.Empty)
            .Distinct()
            .OrderBy(x => x)
            .ToList();
    }

    public NhanVienDetailModel? GetChiTiet(int maNhanVien)
    {
        var nv = _context.NhanViens.AsNoTracking()
            .Include(x => x.TaiKhoan)
                .ThenInclude(tk => tk!.MaVaiTroNavigation)
            .FirstOrDefault(x => x.MaNhanVien == maNhanVien);
        if (nv == null) return null;

        DateTime dauThang = new(DateTime.Today.Year, DateTime.Today.Month, 1);
        DateTime dauThangSau = dauThang.AddMonths(1);
        int tongPhieu = _context.PhieuMuons.Count(x => x.MaNhanVien == maNhanVien)
                        + _context.PhieuTras.Count(x => x.MaNhanVien == maNhanVien)
                        + _context.PhieuNhaps.Count(x => x.MaNhanVien == maNhanVien)
                        + _context.PhieuPhats.Count(x => x.MaNhanVien == maNhanVien);
        int phieuThangNay = _context.PhieuMuons.Count(x => x.MaNhanVien == maNhanVien && x.NgayMuon >= dauThang && x.NgayMuon < dauThangSau)
                            + _context.PhieuTras.Count(x => x.MaNhanVien == maNhanVien && x.NgayTra >= dauThang && x.NgayTra < dauThangSau)
                            + _context.PhieuNhaps.Count(x => x.MaNhanVien == maNhanVien && x.NgayNhap >= dauThang && x.NgayNhap < dauThangSau)
                            + _context.PhieuPhats.Count(x => x.MaNhanVien == maNhanVien && x.NgayLap >= dauThang && x.NgayLap < dauThangSau);

        return new NhanVienDetailModel
        {
            MaNhanVien = nv.MaNhanVien,
            MaNhanVienText = nv.MaNhanVienHienThi ?? $"NV{nv.MaNhanVien:D5}",
            HoTen = nv.HoTen,
            GioiTinh = nv.GioiTinh ?? string.Empty,
            NgaySinh = nv.NgaySinh,
            ChucVu = nv.ChucVu,
            SoDienThoai = nv.SoDienThoai ?? string.Empty,
            Email = nv.Email ?? string.Empty,
            DiaChi = nv.DiaChi ?? string.Empty,
            AnhDaiDien = nv.AnhDaiDien ?? string.Empty,
            NgayVaoLam = nv.NgayVaoLam,
            DangLamViec = nv.TrangThai,
            MaTaiKhoan = nv.TaiKhoan?.MaTaiKhoan,
            TenDangNhap = nv.TaiKhoan?.TenDangNhap ?? string.Empty,
            TaiKhoanHoatDong = nv.TaiKhoan?.TrangThai ?? true,
            LanDangNhapCuoi = nv.TaiKhoan?.LanDangNhapCuoi,
            MaVaiTro = nv.TaiKhoan?.MaVaiTro,
            TenVaiTro = nv.TaiKhoan?.MaVaiTroNavigation?.TenVaiTro ?? string.Empty,
            TongPhieuXuLy = tongPhieu,
            SoPhieuXuLyThangNay = phieuThangNay
        };
    }

    public bool ToggleTrangThaiNhanVien(int maNhanVien)
    {
        var nhanVien = _context.NhanViens.FirstOrDefault(x => x.MaNhanVien == maNhanVien);
        if (nhanVien == null) return false;
        nhanVien.TrangThai = !nhanVien.TrangThai;
        _context.SaveChanges();
        return true;
    }

    public bool ToggleTaiKhoan(int maNhanVien)
    {
        var taiKhoan = _context.TaiKhoans.FirstOrDefault(x => x.MaNhanVien == maNhanVien);
        if (taiKhoan == null) return false;
        taiKhoan.TrangThai = !taiKhoan.TrangThai;
        _context.SaveChanges();
        return true;
    }

    public bool DatLaiMatKhau(int maNhanVien, string matKhauMoi, bool moKhoaTaiKhoan = false)
    {
        var taiKhoan = _context.TaiKhoans.FirstOrDefault(x => x.MaNhanVien == maNhanVien);
        if (taiKhoan == null) return false;
        if (_hasher.IsHash(taiKhoan.MatKhau) && _hasher.Verify(matKhauMoi, taiKhoan.MatKhau))
            throw new InvalidOperationException("Mật khẩu mới không được trùng mật khẩu hiện tại.");
        if (!_hasher.IsHash(taiKhoan.MatKhau) && string.Equals(taiKhoan.MatKhau, matKhauMoi, StringComparison.Ordinal))
            throw new InvalidOperationException("Mật khẩu mới không được trùng mật khẩu hiện tại.");
        taiKhoan.MatKhau = _hasher.Hash(matKhauMoi);
        if (moKhoaTaiKhoan) taiKhoan.TrangThai = true;
        _context.SaveChanges();
        return true;
    }

    public bool ChoNghiViec(int maNhanVien)
    {
        using var transaction = _context.Database.BeginTransaction();
        try
        {
            var nhanVien = _context.NhanViens.Include(x => x.TaiKhoan)
                .FirstOrDefault(x => x.MaNhanVien == maNhanVien);
            if (nhanVien == null) return false;

            nhanVien.TrangThai = false;
            if (nhanVien.TaiKhoan != null) nhanVien.TaiKhoan.TrangThai = false;
            _context.SaveChanges();
            transaction.Commit();
            return true;
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    public bool CapNhatVaiTro(int maNhanVien, string tuKhoaVaiTro)
    {
        var taiKhoan = _context.TaiKhoans.FirstOrDefault(x => x.MaNhanVien == maNhanVien);
        if (taiKhoan == null) return false;

        string keyword = tuKhoaVaiTro.Trim().ToLower();
        var vaiTro = _context.VaiTros.AsEnumerable()
            .FirstOrDefault(x => x.TenVaiTro.ToLower().Contains(keyword));
        if (vaiTro == null) return false;

        taiKhoan.MaVaiTro = vaiTro.MaVaiTro;
        _context.SaveChanges();
        return true;
    }

    public bool CapNhatVaiTroTheoMa(int maNhanVien, int maVaiTro)
    {
        var taiKhoan = _context.TaiKhoans.FirstOrDefault(x => x.MaNhanVien == maNhanVien);
        if (taiKhoan == null) return false;
        if (!_context.VaiTros.Any(x => x.MaVaiTro == maVaiTro))
            throw new InvalidOperationException("Vai trò được chọn không tồn tại.");
        taiKhoan.MaVaiTro = maVaiTro;
        _context.SaveChanges();
        return true;
    }

    public List<VaiTroLookupModel> GetVaiTroLookup()
    {
        return _context.VaiTros.AsNoTracking()
            .OrderBy(x => x.TenVaiTro)
            .Select(x => new VaiTroLookupModel
            {
                MaVaiTro = x.MaVaiTro,
                TenVaiTro = x.TenVaiTro
            }).ToList();
    }

    public bool TonTaiTenDangNhap(string tenDangNhap) =>
        _context.TaiKhoans.AsNoTracking().Any(x => x.TenDangNhap == tenDangNhap);

    public bool TonTaiEmail(string email) =>
        _context.NhanViens.AsNoTracking().Any(x => x.Email == email);

    public bool TonTaiSoDienThoai(string soDienThoai) =>
        _context.NhanViens.AsNoTracking().Any(x => x.SoDienThoai == soDienThoai);

    public ThemNhanVienResultModel ThemNhanVien(ThemNhanVienInputModel input)
    {
        using var transaction = _context.Database.BeginTransaction();
        try
        {
            if (_context.TaiKhoans.Any(x => x.TenDangNhap == input.TenDangNhap))
                throw new InvalidOperationException("Tên đăng nhập đã tồn tại.");
            if (!string.IsNullOrWhiteSpace(input.Email) && _context.NhanViens.Any(x => x.Email == input.Email))
                throw new InvalidOperationException("Email đã được sử dụng bởi nhân viên khác.");
            if (!string.IsNullOrWhiteSpace(input.SoDienThoai) && _context.NhanViens.Any(x => x.SoDienThoai == input.SoDienThoai))
                throw new InvalidOperationException("Số điện thoại đã được sử dụng bởi nhân viên khác.");
            if (!_context.VaiTros.Any(x => x.MaVaiTro == input.MaVaiTro))
                throw new InvalidOperationException("Vai trò tài khoản không tồn tại.");

            var nhanVien = new NhanVien
            {
                HoTen = input.HoTen,
                GioiTinh = input.GioiTinh,
                NgaySinh = input.NgaySinh,
                ChucVu = input.ChucVu,
                SoDienThoai = input.SoDienThoai,
                Email = input.Email,
                DiaChi = input.DiaChi,
                AnhDaiDien = input.AnhDaiDien,
                NgayVaoLam = input.NgayVaoLam,
                TrangThai = input.TrangThai
            };
            _context.NhanViens.Add(nhanVien);
            _context.SaveChanges();

            _context.TaiKhoans.Add(new TaiKhoan
            {
                TenDangNhap = input.TenDangNhap,
                MatKhau = _hasher.Hash(input.MatKhau),
                MaNhanVien = nhanVien.MaNhanVien,
                MaVaiTro = input.MaVaiTro,
                TrangThai = input.TrangThai
            });
            _context.SaveChanges();
            transaction.Commit();

            return new ThemNhanVienResultModel
            {
                MaNhanVien = nhanVien.MaNhanVien,
                MaNhanVienText = nhanVien.MaNhanVienHienThi ?? $"NV{nhanVien.MaNhanVien:D5}"
            };
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    public bool TonTaiTenDangNhapKhac(int maNhanVien, string tenDangNhap) =>
        _context.TaiKhoans.AsNoTracking()
            .Any(x => x.TenDangNhap == tenDangNhap && x.MaNhanVien != maNhanVien);

    public bool TonTaiEmailKhac(int maNhanVien, string email) =>
        _context.NhanViens.AsNoTracking()
            .Any(x => x.Email == email && x.MaNhanVien != maNhanVien);

    public bool TonTaiSoDienThoaiKhac(int maNhanVien, string soDienThoai) =>
        _context.NhanViens.AsNoTracking()
            .Any(x => x.SoDienThoai == soDienThoai && x.MaNhanVien != maNhanVien);

    public bool CapNhatNhanVien(CapNhatNhanVienInputModel input)
    {
        using var transaction = _context.Database.BeginTransaction();
        try
        {
            var nhanVien = _context.NhanViens.Include(x => x.TaiKhoan)
                .FirstOrDefault(x => x.MaNhanVien == input.MaNhanVien);
            if (nhanVien == null)
                throw new InvalidOperationException("Không tìm thấy nhân viên cần cập nhật.");
            if (nhanVien.TaiKhoan == null)
                throw new InvalidOperationException("Nhân viên chưa có tài khoản để cập nhật.");
            if (!_context.VaiTros.Any(x => x.MaVaiTro == input.MaVaiTro))
                throw new InvalidOperationException("Vai trò tài khoản không tồn tại.");
            if (_context.TaiKhoans.Any(x => x.TenDangNhap == input.TenDangNhap && x.MaNhanVien != input.MaNhanVien))
                throw new InvalidOperationException("Tên đăng nhập đã được sử dụng bởi nhân viên khác.");
            if (!string.IsNullOrWhiteSpace(input.Email) &&
                _context.NhanViens.Any(x => x.Email == input.Email && x.MaNhanVien != input.MaNhanVien))
                throw new InvalidOperationException("Email đã được sử dụng bởi nhân viên khác.");
            if (!string.IsNullOrWhiteSpace(input.SoDienThoai) &&
                _context.NhanViens.Any(x => x.SoDienThoai == input.SoDienThoai && x.MaNhanVien != input.MaNhanVien))
                throw new InvalidOperationException("Số điện thoại đã được sử dụng bởi nhân viên khác.");

            nhanVien.HoTen = input.HoTen;
            nhanVien.GioiTinh = input.GioiTinh;
            nhanVien.NgaySinh = input.NgaySinh;
            nhanVien.ChucVu = input.ChucVu;
            nhanVien.SoDienThoai = input.SoDienThoai;
            nhanVien.Email = input.Email;
            nhanVien.DiaChi = input.DiaChi;
            nhanVien.AnhDaiDien = input.AnhDaiDien;
            nhanVien.NgayVaoLam = input.NgayVaoLam;
            nhanVien.TrangThai = input.TrangThai;

            nhanVien.TaiKhoan.TenDangNhap = input.TenDangNhap;
            nhanVien.TaiKhoan.MaVaiTro = input.MaVaiTro;
            if (!string.IsNullOrEmpty(input.MatKhauMoi))
                nhanVien.TaiKhoan.MatKhau = _hasher.Hash(input.MatKhauMoi);

            _context.SaveChanges();
            transaction.Commit();
            return true;
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    public void CapNhatAnhDaiDien(int maNhanVien, string? anhDaiDien)
    {
        NhanVien nhanVien = _context.NhanViens.Find(maNhanVien)
            ?? throw new InvalidOperationException("Không tìm thấy nhân viên.");
        nhanVien.AnhDaiDien = string.IsNullOrWhiteSpace(anhDaiDien) ? null : anhDaiDien.Trim();
        _context.SaveChanges();
    }
}
