using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Entities;

[Table("NhanVien")]
public partial class NhanVien
{
    [Key]
    public int MaNhanVien { get; set; }

    [StringLength(7)]
    public string? MaNhanVienHienThi { get; set; }

    [StringLength(100)]
    public string HoTen { get; set; } = null!;

    [StringLength(10)]
    public string? GioiTinh { get; set; }

    public DateOnly? NgaySinh { get; set; }

    [StringLength(50)]
    public string ChucVu { get; set; } = null!;

    [StringLength(15)]
    public string? SoDienThoai { get; set; }

    [StringLength(100)]
    public string? Email { get; set; }

    [StringLength(255)]
    public string? DiaChi { get; set; }

    [StringLength(255)]
    public string? AnhDaiDien { get; set; }

    public DateOnly NgayVaoLam { get; set; }

    public bool TrangThai { get; set; }

    [InverseProperty("MaNhanVienThuNavigation")]
    public virtual ICollection<DongPhiThuongNien> DongPhiThuongNiens { get; set; } = new List<DongPhiThuongNien>();

    [InverseProperty("MaNhanVienNavigation")]
    public virtual ICollection<PhieuMuon> PhieuMuons { get; set; } = new List<PhieuMuon>();

    [InverseProperty("MaNhanVienNavigation")]
    public virtual ICollection<PhieuNhap> PhieuNhaps { get; set; } = new List<PhieuNhap>();

    [InverseProperty("MaNhanVienNavigation")]
    public virtual ICollection<PhieuPhat> PhieuPhats { get; set; } = new List<PhieuPhat>();

    [InverseProperty("MaNhanVienNavigation")]
    public virtual ICollection<PhieuTra> PhieuTras { get; set; } = new List<PhieuTra>();

    [InverseProperty("MaNhanVienNavigation")]
    public virtual TaiKhoan? TaiKhoan { get; set; }

    [InverseProperty("MaNhanVienNavigation")]
    public virtual ICollection<ThongBao> ThongBaos { get; set; } = new List<ThongBao>();
}
