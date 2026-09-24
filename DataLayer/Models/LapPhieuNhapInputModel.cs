using System;
using System.Collections.Generic;

namespace DataLayer.Models;

/// <summary>
/// Model từng dòng đầu sách nhập trong phiếu nhập.
/// </summary>
public sealed class SachNhapInputItem
{
    public int MaChiTietNhap { get; set; }
    public int MaSach { get; set; }
    public string MaSachText { get; set; } = string.Empty;
    public string TenSach { get; set; } = string.Empty;
    public string TheLoai { get; set; } = string.Empty;
    public int SoLuongNhap { get; set; } = 1;
    public decimal DonGiaNhap { get; set; }
    public decimal ThanhTien => SoLuongNhap * DonGiaNhap;
    public string GhiChu { get; set; } = "-";
}

/// <summary>
/// Dữ liệu đầu sách thực tế dùng trong hộp thoại chọn sách nhập.
/// </summary>
public sealed class SachNhapLookupModel
{
    public int MaSach { get; set; }
    public string MaSachText { get; set; } = string.Empty;
    public string TenSach { get; set; } = string.Empty;
    public string TacGia { get; set; } = string.Empty;
    public string TheLoai { get; set; } = string.Empty;
    public string Isbn { get; set; } = string.Empty;
    public decimal GiaThamKhao { get; set; }
}

/// <summary>
/// Model dữ liệu đầu vào cho chức năng lập phiếu nhập.
/// </summary>
public sealed class LapPhieuNhapInputModel
{
    public int MaPhieuNhap { get; set; }
    public string MaPhieuNhapHienThi { get; set; } = string.Empty;
    public DateTime NgayNhap { get; set; } = DateTime.Now;
    public int MaNhanVienLap { get; set; }
    public int MaNcc { get; set; }
    public string TrangThai { get; set; } = "Hoàn thành";
    public string? GhiChuChung { get; set; }

    public List<SachNhapInputItem> DanhSachSachNhap { get; set; } = new();

    public decimal TamTinh => DanhSachSachNhap.Sum(x => x.ThanhTien);
    public decimal ChietKhau { get; set; }
    public decimal ThueVat { get; set; }
    public decimal TongTien => TamTinh - ChietKhau + ThueVat;
}
