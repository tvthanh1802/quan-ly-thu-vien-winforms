using System;
using System.Collections.Generic;

namespace DataLayer.Models;

/// <summary>
/// Model thông tin một cuốn sách mượn được trả trong phiếu.
/// </summary>
public sealed class SachTraInputItem
{
    public int MaChiTietMuon { get; set; }
    public int MaCuonSach { get; set; }
    public int MaSach { get; set; }
    public string MaSachText { get; set; } = string.Empty;
    public string TenSach { get; set; } = string.Empty;
    public string TacGia { get; set; } = string.Empty;
    public DateOnly NgayMuon { get; set; }
    public DateOnly HanTraDuKien { get; set; }
    public DateOnly NgayTraThucTe { get; set; } = DateOnly.FromDateTime(DateTime.Today);
    public string TinhTrangSach { get; set; } = "Tốt";
    public decimal PhiTreHan { get; set; }
    public decimal PhiHuHong { get; set; }
    public decimal GiaSach { get; set; }
    public string? GhiChu { get; set; }
    /// <summary>
    /// Cuốn sách được chọn để trả trong lần tiếp nhận hiện tại.
    /// Trạng thái đã trả chính thức được xác định bởi ChiTietTra trong CSDL.
    /// </summary>
    public bool DaTra { get; set; }
}

/// <summary>
/// Model đầy đủ khi thực hiện tiếp nhận trả sách.
/// </summary>
public sealed class TiepNhanTraInputModel
{
    public int MaPhieuMuon { get; set; }
    public DateTime NgayTraThucTe { get; set; } = DateTime.Now;
    public int MaNhanVienTiepNhan { get; set; }
    public string? GhiChu { get; set; }
    public string? GhiChuTinhTrang { get; set; }

    public List<SachTraInputItem> DanhSachSachTra { get; set; } = new();
}

/// <summary>
/// Kết quả đã commit của một lần tiếp nhận trả sách.
/// </summary>
public sealed class TiepNhanTraResultModel
{
    public int SoSachDaTra { get; init; }
    public int SoSachConMuon { get; init; }
    public int? MaPhieuPhat { get; init; }
    public decimal TongTienPhat { get; init; }
    public bool CoTienPhat => MaPhieuPhat.HasValue && TongTienPhat > 0;
}
