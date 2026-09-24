using System;
using System.Collections.Generic;

namespace DataLayer.Models;

/// <summary>
/// Model dữ liệu đầu vào cho chức năng Thêm nhà cung cấp mới.
/// </summary>
public sealed class ThemNhaCungCapInputModel
{
    public int MaNcc { get; set; }
    public string MaNccHienThi { get; set; } = string.Empty;
    public string TenNcc { get; set; } = string.Empty;
    public string? NguoiDaiDien { get; set; }
    public string? SoDienThoai { get; set; }
    public string? Email { get; set; }
    public string? DiaChi { get; set; }
    public string? MaSoThue { get; set; }
    public string? Website { get; set; }
    public string LoaiNcc { get; set; } = "Nhà sách / phát hành";
    public bool TrangThai { get; set; } = true;

    // Bổ sung
    public string? Logo { get; set; }
    public string DieuKhoanThanhToan { get; set; } = "Trả chậm 30 ngày";
    public DateTime NgayBatDauHopTac { get; set; } = DateTime.Now;
    public string KhuVucCungCap { get; set; } = "Hà Nội";
    public List<string> NhomSachCungCap { get; set; } = new() { "Tin học", "Kinh tế", "Giáo trình" };
    public string? GhiChu { get; set; }

    // Hợp tác
    public decimal HanMucCongNo { get; set; }
    public double ChietKhauMacDinh { get; set; }
    public string PhuongThucThanhToan { get; set; } = "Chuyển khoản";
    public string DanhGiaBanDau { get; set; } = "Tốt";
    public decimal CongNoHienTai { get; set; }
}
