namespace BusinessLayer.Models;

public sealed class TaiKhoanGridModel
{
    public int STT { get; set; }
    public int MaTaiKhoan { get; set; }
    public string TenDangNhap { get; set; } = string.Empty;
    public string HoTen { get; set; } = string.Empty;
    public int MaVaiTro { get; set; }
    public string TenVaiTro { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string SoDienThoai { get; set; } = string.Empty;
    public string TrangThai { get; set; } = string.Empty;
    public bool DangHoatDong { get; set; }
}

public sealed class VaiTroGridModel
{
    public int STT { get; set; }
    public int MaVaiTro { get; set; }
    public string TenVaiTro { get; set; } = string.Empty;
    public string MoTaText { get; set; } = string.Empty;
    public int SoNguoiDung { get; set; }
}

public sealed class PhanQuyenGridModel
{
    public string MaChucNang { get; set; } = string.Empty;
    public string MaNhom { get; set; } = string.Empty;
    public string NhomChucNang { get; set; } = string.Empty;
    public string TenChucNang { get; set; } = string.Empty;
    public bool LaDongNhom { get; set; }
    public bool DuocXem { get; set; }
    public bool DuocThem { get; set; }
    public bool DuocSua { get; set; }
    public bool DuocXoa { get; set; }
    public bool DuocIn { get; set; }
    public bool DuocXuatExcel { get; set; }
}
