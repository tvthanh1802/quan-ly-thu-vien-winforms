using System;
using System.Collections.Generic;

namespace DataLayer.Models;

/// <summary>
/// Model từng dòng sách trong form chỉnh sửa phiếu mượn.
/// </summary>
public sealed class SachSuaInputItem
{
    public int MaChiTietMuon { get; set; }
    public int MaCuonSach { get; set; }
    public int MaSach { get; set; }
    public string MaSachText { get; set; } = string.Empty;
    public string TenSach { get; set; } = string.Empty;
    public string TacGia { get; set; } = string.Empty;
    public string TheLoai { get; set; } = string.Empty;
    public string MaCuonSachText { get; set; } = string.Empty;
    public DateOnly NgayMuon { get; set; }
    public DateOnly HanTraDuKien { get; set; }
    public string TrangThai { get; set; } = "Đang mượn";
}

/// <summary>
/// Model dữ liệu đầu vào cho chức năng sửa phiếu mượn.
/// </summary>
public sealed class SuaPhieuMuonInputModel
{
    public int MaPhieuMuon { get; set; }
    public int MaDocGia { get; set; }
    public DateTime NgayMuon { get; set; }
    public DateOnly HanTra { get; set; }
    public int MaNhanVienLap { get; set; }
    public string? GhiChuPhieu { get; set; }
    public string? GhiChuSua { get; set; }

    public List<SachSuaInputItem> DanhSachSach { get; set; } = new();
}
