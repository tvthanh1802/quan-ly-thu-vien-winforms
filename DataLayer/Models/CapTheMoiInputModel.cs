namespace DataLayer.Models;

/// <summary>
/// Dữ liệu đầu vào khi cấp thẻ thư viện mới cho độc giả và thu lệ phí.
/// </summary>
public sealed class CapTheMoiInputModel
{
    public int MaDocGia { get; set; }
    public string MaTheHienThi { get; set; } = string.Empty;
    public DateOnly NgayCap { get; set; }
    public DateOnly NgayHetHan { get; set; }
    public string TrangThaiThe { get; set; } = "Đang hiệu lực";
    public string? GhiChuThe { get; set; }

    // Lệ phí
    public bool ThuLePhiNgay { get; set; }
    public int NamDongPhi { get; set; }
    public decimal SoTienLePhi { get; set; }
    public DateOnly NgayDongPhi { get; set; }
    public string HinhThucThu { get; set; } = "Tiền mặt";
    public string? GhiChuLePhi { get; set; }
}
