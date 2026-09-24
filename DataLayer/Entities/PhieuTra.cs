using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Entities;

[Table("PhieuTra")]
[Index("MaPhieuMuon", "NgayTra", Name = "IX_PhieuTra_PhieuMuon_NgayTra", IsDescending = new[] { false, true })]
public partial class PhieuTra
{
    [Key]
    public int MaPhieuTra { get; set; }

    [StringLength(8)]
    public string? MaPhieuTraHienThi { get; set; }

    public int MaPhieuMuon { get; set; }

    public int MaNhanVien { get; set; }

    public DateTime NgayTra { get; set; }

    [StringLength(255)]
    public string? GhiChu { get; set; }

    [InverseProperty("MaPhieuTraNavigation")]
    public virtual ICollection<ChiTietTra> ChiTietTras { get; set; } = new List<ChiTietTra>();

    [ForeignKey("MaNhanVien")]
    [InverseProperty("PhieuTras")]
    public virtual NhanVien MaNhanVienNavigation { get; set; } = null!;

    [ForeignKey("MaPhieuMuon")]
    [InverseProperty("PhieuTras")]
    public virtual PhieuMuon MaPhieuMuonNavigation { get; set; } = null!;
}
