using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Entities;

[Table("ChiTietNhap")]
[Index("MaPhieuNhap", "MaSach", Name = "UQ_CTN_Phieu_Sach", IsUnique = true)]
public partial class ChiTietNhap
{
    [Key]
    public int MaChiTietNhap { get; set; }

    public int MaPhieuNhap { get; set; }

    public int MaSach { get; set; }

    public int SoLuong { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal DonGia { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? ThanhTien { get; set; }

    [ForeignKey("MaPhieuNhap")]
    [InverseProperty("ChiTietNhaps")]
    public virtual PhieuNhap MaPhieuNhapNavigation { get; set; } = null!;

    [ForeignKey("MaSach")]
    [InverseProperty("ChiTietNhaps")]
    public virtual Sach MaSachNavigation { get; set; } = null!;
}
