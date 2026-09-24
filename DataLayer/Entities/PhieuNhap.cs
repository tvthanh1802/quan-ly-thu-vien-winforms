using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Entities;

[Table("PhieuNhap")]
public partial class PhieuNhap
{
    [Key]
    public int MaPhieuNhap { get; set; }

    [StringLength(8)]
    public string? MaPhieuNhapHienThi { get; set; }

    [Column("MaNCC")]
    public int MaNcc { get; set; }

    public int MaNhanVien { get; set; }

    public DateTime NgayNhap { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal TongTien { get; set; }

    [StringLength(20)]
    public string TrangThai { get; set; } = "Hoàn thành";

    [StringLength(255)]
    public string? GhiChu { get; set; }

    [InverseProperty("MaPhieuNhapNavigation")]
    public virtual ICollection<ChiTietNhap> ChiTietNhaps { get; set; } = new List<ChiTietNhap>();

    [ForeignKey("MaNcc")]
    [InverseProperty("PhieuNhaps")]
    public virtual NhaCungCap MaNccNavigation { get; set; } = null!;

    [ForeignKey("MaNhanVien")]
    [InverseProperty("PhieuNhaps")]
    public virtual NhanVien MaNhanVienNavigation { get; set; } = null!;
}
