using System;
using System.Collections.Generic;

namespace DataLayer.Models;

/// <summary>
/// Model cho từng dòng sách được xử lý trong phiếu mượn.
/// </summary>
public sealed class SachXuLyInputItem
{
    public int MaChiTietMuon { get; set; }
    public int MaCuonSach { get; set; }
    public int MaSach { get; set; }
    public string MaSachText { get; set; } = string.Empty;
    public string TenSach { get; set; } = string.Empty;
    public string TacGia { get; set; } = string.Empty;
    public string MaCuonSachText { get; set; } = string.Empty;
    public DateOnly NgayMuon { get; set; }
    public DateOnly HanTraDuKien { get; set; }
    public DateOnly NgayTraThucTe { get; set; } = DateOnly.FromDateTime(DateTime.Today);
    public string TinhTrangKhiTra { get; set; } = "Bình thường";
    public int SoNgayQuaHan => Math.Max(0, NgayTraThucTe.DayNumber - HanTraDuKien.DayNumber);
    public bool DaXuLy { get; set; }
}

/// <summary>
/// Model dữ liệu đầu vào cho chức năng xử lý phiếu mượn.
/// </summary>
public sealed class XuLyPhieuMuonInputModel
{
    public int MaPhieuMuon { get; set; }
    public int MaDocGia { get; set; }
    public string LoaiXuLy { get; set; } = "Trả sách";
    public DateTime NgayTraThucTe { get; set; } = DateTime.Now;
    public string TinhTrangChung { get; set; } = "Bình thường";
    public int MaNhanVienXuLy { get; set; }
    public string? GhiChu { get; set; }

    public List<SachXuLyInputItem> DanhSachSach { get; set; } = new();
}
