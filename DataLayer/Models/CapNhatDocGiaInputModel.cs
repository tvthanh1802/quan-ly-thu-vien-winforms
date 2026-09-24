namespace DataLayer.Models;

/// <summary>
/// Dữ liệu đầu vào dùng khi cập nhật hồ sơ độc giả.
/// Thông tin thẻ được quản lý qua các nghiệp vụ thẻ chuyên biệt.
/// </summary>
public sealed class CapNhatDocGiaInputModel
{
    public int MaDocGia { get; set; }
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
    public DateOnly NgayDangKy { get; set; }
    public bool TrangThaiDocGia { get; set; } = true;
}
