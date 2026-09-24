namespace DataLayer.Models;

/// <summary>
/// Dữ liệu đầu vào dùng khi tạo hồ sơ độc giả. Việc cấp thẻ và thu phí
/// được thực hiện bằng các nghiệp vụ độc lập sau khi hồ sơ được tạo.
/// </summary>
public sealed class ThemDocGiaInputModel
{
    public string HoTen { get; set; } = string.Empty;
    public DateOnly NgaySinh { get; set; }
    public string GioiTinh { get; set; } = "Nam";
    public string SoDienThoai { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? DiaChi { get; set; }
    public string? AnhDaiDien { get; set; }
    public string LoaiDocGia { get; set; } = "Sinh viên";
    public string? MaSinhVien { get; set; }
    public int? MaLop { get; set; }
}

public sealed class QuyDinhDocGiaModel
{
    public int SoSachMuonToiDa { get; set; } = 3;
    public int SoNgayMuonToiDa { get; set; } = 14;
    public decimal PhiThuongNien { get; set; } = 100_000m;
}
