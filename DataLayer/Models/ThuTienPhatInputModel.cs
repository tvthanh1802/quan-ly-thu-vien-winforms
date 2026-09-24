using System;
using System.Collections.Generic;

namespace DataLayer.Models;

/// <summary>
/// Model chi tiết từng khoản phạt hiển thị trên form thu tiền phạt.
/// </summary>
public sealed class ChiTietPhatGridModel
{
    public int STT { get; set; }
    public string MaSachText { get; set; } = string.Empty;
    public string TenSach { get; set; } = string.Empty;
    public DateOnly NgayTraDuKien { get; set; }
    public DateOnly NgayTraThucTe { get; set; }
    public int SoNgayQuaHan { get; set; }
    public decimal DonGiaPhatNgay { get; set; } = 2000m;
    public decimal ThanhTien { get; set; }
}

/// <summary>
/// Model đầu vào cho chức năng thu tiền phạt.
/// </summary>
public sealed class ThuTienPhatInputModel
{
    public int MaPhieuPhat { get; set; }
    public int MaDocGia { get; set; }
    public DateTime NgayThanhToan { get; set; } = DateTime.Now;
    public int MaNhanVienThu { get; set; }
    public string HinhThucThanhToan { get; set; } = "Tiền mặt";
    public decimal SoTienKhachDua { get; set; }
    public decimal SoTienThua => Math.Max(0, SoTienKhachDua - TongTienPhat);
    public decimal TongTienPhat { get; set; }
    public string? GhiChu { get; set; }
}
