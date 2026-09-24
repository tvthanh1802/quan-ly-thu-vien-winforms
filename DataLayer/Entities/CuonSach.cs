using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Entities;

[Table("CuonSach")]
[Index("MaSach", "TrangThai", Name = "IX_CuonSach_Sach_TrangThai")]
[Index("MaViTri", Name = "IX_CuonSach_ViTri")]
[Index("MaVach", Name = "UQ_CuonSach_MaVach", IsUnique = true)]
public partial class CuonSach
{
    [Key]
    public int MaCuonSach { get; set; }

    [StringLength(8)]
    public string? MaCuonSachHienThi { get; set; }

    public int MaSach { get; set; }

    public int? MaViTri { get; set; }

    [StringLength(50)]
    public string MaVach { get; set; } = null!;

    [StringLength(30)]
    public string TinhTrang { get; set; } = null!;

    [StringLength(30)]
    public string TrangThai { get; set; } = null!;

    public DateOnly NgayNhap { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? GiaNhap { get; set; }

    [StringLength(255)]
    public string? GhiChu { get; set; }

    [InverseProperty("MaCuonSachNavigation")]
    public virtual ICollection<ChiTietMuon> ChiTietMuons { get; set; } = new List<ChiTietMuon>();

    [ForeignKey("MaSach")]
    [InverseProperty("CuonSaches")]
    public virtual Sach MaSachNavigation { get; set; } = null!;

    [ForeignKey("MaViTri")]
    [InverseProperty("CuonSaches")]
    public virtual ViTriSach? MaViTriNavigation { get; set; }
}
