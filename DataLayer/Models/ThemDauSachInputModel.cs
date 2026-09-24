using System;
using System.Collections.Generic;

namespace DataLayer.Models;

/// <summary>
/// Model dữ liệu đầu vào cho chức năng Thêm đầu sách mới.
/// </summary>
public sealed class ThemDauSachInputModel
{
    public int MaSach { get; set; }
    public string MaSachHienThi { get; set; } = string.Empty;
    public string TenSach { get; set; } = string.Empty;
    public string? Isbn { get; set; }
    public int MaTheLoai { get; set; }
    public string TenTheLoai { get; set; } = string.Empty;
    public List<int> DanhSachMaTacGia { get; set; } = new();
    public List<string> DanhSachTenTacGia { get; set; } = new();
    public int MaNxb { get; set; }
    public string TenNxb { get; set; } = string.Empty;
    public int? NamXuatBan { get; set; } = DateTime.Now.Year;
    public string NgonNgu { get; set; } = "Tiếng Việt";
    public int? SoTrang { get; set; }
    public decimal? GiaBia { get; set; }
    public string TrangThai { get; set; } = "Đang hoạt động";

    // Thông tin bổ sung
    public string? AnhBia { get; set; }
    public string? MoTa { get; set; }
    public string? TuKhoa { get; set; }
    public string? ViTriGoiY { get; set; }
    public string? GhiChu { get; set; }

    // Khởi tạo bản sao
    public int SoLuongBanSao { get; set; } = 20;
    public decimal GiaNhapMoiCuon { get; set; } = 150000m;
    public DateTime NgayNhap { get; set; } = DateTime.Now;
    public string TinhTrangBanSao { get; set; } = "Tốt";
    public bool TuTaoMaVach { get; set; } = true;
    public string? GhiChuBanSao { get; set; }

    public decimal TongGiaNhap => SoLuongBanSao * GiaNhapMoiCuon;
}
