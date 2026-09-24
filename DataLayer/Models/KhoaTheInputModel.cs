namespace DataLayer.Models;

/// <summary>
/// Dữ liệu đầu vào khi thực hiện khóa thẻ độc giả.
/// </summary>
public sealed class KhoaTheInputModel
{
    public int MaDocGia { get; set; }
    public string LyDoKhoa { get; set; } = "Vi phạm quy định thư viện";
    public DateOnly NgayKhoa { get; set; }
    public string ThoiHanKhoa { get; set; } = "30 ngày";
    public DateOnly? NgayMoKhoaDuKien { get; set; }
    public string? GhiChu { get; set; }
}
