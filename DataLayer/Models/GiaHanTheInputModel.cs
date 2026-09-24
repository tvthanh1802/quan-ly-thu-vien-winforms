namespace DataLayer.Models;

/// <summary>
/// Dữ liệu gia hạn thẻ và khoản thu phát sinh trong cùng một giao dịch.
/// </summary>
public sealed class GiaHanTheInputModel
{
    public int MaDocGia { get; set; }
    public DateOnly NgayGiaHan { get; set; } = DateOnly.FromDateTime(DateTime.Today);
    public DateOnly NgayHetHanMoi { get; set; }
    public int SoThangGiaHan { get; set; } = 12;
    public decimal LePhiGiaHan { get; set; }
    public string HinhThucThanhToan { get; set; } = "Tiền mặt";
    public string LyDoGiaHan { get; set; } = "Gia hạn định kỳ";
    public string? GhiChu { get; set; }
    public int? MaNhanVienThu { get; set; }
}
