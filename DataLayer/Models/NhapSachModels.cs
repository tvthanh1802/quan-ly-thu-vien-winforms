namespace DataLayer.Models;

public sealed class NhapSachGridModel
{
    public int MaPhieuNhap { get; set; }
    public string MaPhieuText { get; set; } = string.Empty;
    public int MaNhaCungCap { get; set; }
    public string TenNhaCungCap { get; set; } = string.Empty;
    public DateTime NgayNhap { get; set; }
    public int SoDauSach { get; set; }
    public int TongSoCuon { get; set; }
    public decimal TongTien { get; set; }
    public string TrangThai { get; set; } = string.Empty;
    public string NguoiLap { get; set; } = string.Empty;
}

public sealed class NhapSachStatisticsModel
{
    public int PhieuNhapHomNay { get; set; }
    public int PhieuNhapHomQua { get; set; }
    public int SoCuonNhapThangNay { get; set; }
    public int SoCuonNhapThangTruoc { get; set; }
    public int SoNhaCungCapHoatDong { get; set; }
    public decimal TongTienNhapThangNay { get; set; }
    public decimal TongTienNhapThangTruoc { get; set; }
}

public sealed class NhapSachDetailModel
{
    public int MaPhieuNhap { get; set; }
    public string MaPhieuText { get; set; } = string.Empty;
    public string TrangThai { get; set; } = string.Empty;
    public DateTime NgayNhap { get; set; }
    public string NguoiLap { get; set; } = string.Empty;
    public string TenNhaCungCap { get; set; } = string.Empty;
    public string SoDienThoaiNcc { get; set; } = string.Empty;
    public string DiaChiNcc { get; set; } = string.Empty;
    public string EmailNcc { get; set; } = string.Empty;
    public string NguoiDaiDienNcc { get; set; } = string.Empty;
    public string MaSoThueNcc { get; set; } = string.Empty;
    public bool NhaCungCapDangHoatDong { get; set; }
    public string GhiChu { get; set; } = string.Empty;
    public decimal TamTinh { get; set; }
    public decimal ChietKhau { get; set; }
    public decimal TongTien { get; set; }
    public List<DauSachNhapItemModel> DauSaches { get; set; } = new();
}

public sealed class DauSachNhapItemModel
{
    public int MaSach { get; set; }
    public string TenSach { get; set; } = string.Empty;
    public string TacGia { get; set; } = string.Empty;
    public string MaSachText { get; set; } = string.Empty;
    public string TheLoai { get; set; } = string.Empty;
    public string ViTri { get; set; } = string.Empty;
    public string? AnhBia { get; set; }
    public int SoLuong { get; set; }
    public decimal DonGia { get; set; }
    public decimal ThanhTien { get; set; }
}

public sealed class NhaCungCapLookupModel
{
    public int MaNhaCungCap { get; set; }
    public string TenNhaCungCap { get; set; } = string.Empty;
    public string SoDienThoai { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string DiaChi { get; set; } = string.Empty;
}

public sealed class NhaCungCapThuongXuyenModel
{
    public string TenNhaCungCap { get; set; } = string.Empty;
    public int SoLanNhap { get; set; }
}
