using System;
using System.Collections.Generic;

namespace DataLayer.Models;

/// <summary>
/// Mẫu thông tin một đầu sách chọn mượn trong phiếu.
/// </summary>
public sealed class SachMuonInputItem
{
    public int MaSach { get; set; }
    public string MaSachText { get; set; } = string.Empty;
    public string TenSach { get; set; } = string.Empty;
    public string TacGia { get; set; } = string.Empty;
    public string TheLoai { get; set; } = string.Empty;
    public int SoLuong { get; set; } = 1;
    public DateOnly NgayTraDuKien { get; set; }
    public string AnhBia { get; set; } = string.Empty;
    public int SoLuongCon { get; set; }
    public int TongSoLuong { get; set; }
}

/// <summary>
/// Model thông tin đầu vào khi lập phiếu mượn sách mới.
/// </summary>
public sealed class LapPhieuMuonInputModel
{
    public int MaDocGia { get; set; }
    public int MaNhanVien { get; set; }
    public DateTime NgayLap { get; set; } = DateTime.Now;
    public DateOnly HanTraDuKien { get; set; } = DateOnly.FromDateTime(DateTime.Today.AddDays(14));
    public string? GhiChu { get; set; }
    public string MaPhieuHienThi { get; set; } = string.Empty;

    public List<SachMuonInputItem> DanhSachSach { get; set; } = new();
}
