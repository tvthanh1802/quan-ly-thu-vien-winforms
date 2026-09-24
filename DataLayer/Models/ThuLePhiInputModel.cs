using System;

namespace DataLayer.Models;

/// <summary>
/// Model chứa thông tin đầu vào khi thu lệ phí độc giả.
/// </summary>
public sealed class ThuLePhiInputModel
{
    public int MaDocGia { get; set; }
    public string LoaiLePhi { get; set; } = "Lệ phí thường niên";
    public int NamDongPhi { get; set; } = DateTime.Today.Year;
    public decimal SoTien { get; set; } = 100000m;
    public DateOnly NgayThu { get; set; } = DateOnly.FromDateTime(DateTime.Today);
    public string HinhThucThu { get; set; } = "Tiền mặt";
    public string? GhiChu { get; set; }
    public string SoPhieuThu { get; set; } = string.Empty;
    public int? MaNhanVienThu { get; set; }
    public decimal SoTienNhan { get; set; } = 100000m;
}

/// <summary>
/// Model hiển thị danh sách lịch sử đóng lệ phí của độc giả.
/// </summary>
public sealed class LichSuDongPhiGridModel
{
    public int STT { get; set; }
    public int NamDong { get; set; }
    public decimal SoTien { get; set; }
    public DateOnly NgayDong { get; set; }
    public string LoaiLePhi { get; set; } = "Lệ phí thường niên";
    public string HinhThucThu { get; set; } = "Tiền mặt";
    public string NguoiThu { get; set; } = "-";
    public string SoPhieuThu { get; set; } = "-";
    public string GhiChu { get; set; } = "-";
}
