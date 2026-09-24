using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Entities;

[Table("PhieuMuon")]
[Index("MaThe", "NgayMuon", Name = "IX_PhieuMuon_The_NgayMuon", IsDescending = new[] { false, true })]
[Index("TrangThai", "HanTra", Name = "IX_PhieuMuon_TrangThai_HanTra")]
public partial class PhieuMuon
{
    [Key]
    public int MaPhieuMuon { get; set; }

    [StringLength(8)]
    public string? MaPhieuMuonHienThi { get; set; }

    public int MaThe { get; set; }

    public int MaNhanVien { get; set; }

    public DateTime NgayMuon { get; set; }

    public DateOnly HanTra { get; set; }

    [StringLength(30)]
    public string TrangThai { get; set; } = null!;

    [StringLength(255)]
    public string? GhiChu { get; set; }

    [InverseProperty("MaPhieuMuonNavigation")]
    public virtual ICollection<ChiTietMuon> ChiTietMuons { get; set; } = new List<ChiTietMuon>();

    [ForeignKey("MaNhanVien")]
    [InverseProperty("PhieuMuons")]
    public virtual NhanVien MaNhanVienNavigation { get; set; } = null!;

    [ForeignKey("MaThe")]
    [InverseProperty("PhieuMuons")]
    public virtual TheDocGium MaTheNavigation { get; set; } = null!;

    [InverseProperty("MaPhieuMuonNavigation")]
    public virtual ICollection<PhieuPhat> PhieuPhats { get; set; } = new List<PhieuPhat>();

    [InverseProperty("MaPhieuMuonNavigation")]
    public virtual ICollection<PhieuTra> PhieuTras { get; set; } = new List<PhieuTra>();
}
