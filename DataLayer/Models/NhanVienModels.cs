namespace DataLayer.Models;

public sealed class NhanVienGridModel
{
    public int MaNhanVien { get; set; }
    public string MaNhanVienText { get; set; } = string.Empty;
    public string HoTen { get; set; } = string.Empty;
    public string GioiTinh { get; set; } = string.Empty;
    public DateOnly? NgaySinh { get; set; }
    public string ChucVu { get; set; } = string.Empty;
    public string SoDienThoai { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string DiaChi { get; set; } = string.Empty;
    public string AnhDaiDien { get; set; } = string.Empty;
    public DateOnly NgayVaoLam { get; set; }
    public bool DangLamViec { get; set; }
    public int? MaTaiKhoan { get; set; }
    public string TenDangNhap { get; set; } = string.Empty;
    public bool TaiKhoanHoatDong { get; set; }
    public int? MaVaiTro { get; set; }
    public string TenVaiTro { get; set; } = string.Empty;

    public string NgaySinhText => NgaySinh?.ToString("dd/MM/yyyy") ?? "-";
    public string NgayVaoLamText => NgayVaoLam.ToString("dd/MM/yyyy");
    public string TrangThai => !TaiKhoanHoatDong && MaTaiKhoan.HasValue
        ? "Đã khóa"
        : DangLamViec ? "Đang làm việc" : "Tạm nghỉ";
}

public sealed class NhanVienStatisticsModel
{
    public int TongNhanVien { get; set; }
    public int DangLamViec { get; set; }
    public int TamNghi { get; set; }
    public int TaiKhoanBiKhoa { get; set; }
}

public sealed class NhanVienDetailModel
{
    public int MaNhanVien { get; set; }
    public string MaNhanVienText { get; set; } = string.Empty;
    public string HoTen { get; set; } = string.Empty;
    public string GioiTinh { get; set; } = string.Empty;
    public DateOnly? NgaySinh { get; set; }
    public string ChucVu { get; set; } = string.Empty;
    public string SoDienThoai { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string DiaChi { get; set; } = string.Empty;
    public string AnhDaiDien { get; set; } = string.Empty;
    public DateOnly NgayVaoLam { get; set; }
    public bool DangLamViec { get; set; }
    public int? MaTaiKhoan { get; set; }
    public string TenDangNhap { get; set; } = string.Empty;
    public bool TaiKhoanHoatDong { get; set; }
    public DateTime? LanDangNhapCuoi { get; set; }
    public int? MaVaiTro { get; set; }
    public string TenVaiTro { get; set; } = string.Empty;
    public int TongPhieuXuLy { get; set; }
    public int SoPhieuXuLyThangNay { get; set; }

    public string TrangThai => !TaiKhoanHoatDong && MaTaiKhoan.HasValue
        ? "Đã khóa"
        : DangLamViec ? "Đang làm việc" : "Tạm nghỉ";
}

public sealed class ThemNhanVienInputModel
{
    public string HoTen { get; set; } = string.Empty;
    public string GioiTinh { get; set; } = string.Empty;
    public DateOnly? NgaySinh { get; set; }
    public string ChucVu { get; set; } = string.Empty;
    public string? SoDienThoai { get; set; }
    public string? Email { get; set; }
    public string? DiaChi { get; set; }
    public string? AnhDaiDien { get; set; }
    public DateOnly NgayVaoLam { get; set; }
    public bool TrangThai { get; set; }
    public string TenDangNhap { get; set; } = string.Empty;
    public string MatKhau { get; set; } = string.Empty;
    public int MaVaiTro { get; set; }
}

public sealed class CapNhatNhanVienInputModel
{
    public int MaNhanVien { get; set; }
    public string HoTen { get; set; } = string.Empty;
    public string GioiTinh { get; set; } = string.Empty;
    public DateOnly? NgaySinh { get; set; }
    public string ChucVu { get; set; } = string.Empty;
    public string? SoDienThoai { get; set; }
    public string? Email { get; set; }
    public string? DiaChi { get; set; }
    public string? AnhDaiDien { get; set; }
    public DateOnly NgayVaoLam { get; set; }
    public bool TrangThai { get; set; }
    public string TenDangNhap { get; set; } = string.Empty;
    public int MaVaiTro { get; set; }
    public string? MatKhauMoi { get; set; }
}

public sealed class VaiTroLookupModel
{
    public int MaVaiTro { get; set; }
    public string TenVaiTro { get; set; } = string.Empty;
    public override string ToString() => TenVaiTro;
}

public sealed class ThemNhanVienResultModel
{
    public int MaNhanVien { get; set; }
    public string MaNhanVienText { get; set; } = string.Empty;
}
