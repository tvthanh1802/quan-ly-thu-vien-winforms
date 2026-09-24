using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Entities;

[Table("ChiTietTra")]
[Index("MaChiTietMuon", Name = "UQ_CTT_ChiTietMuon", IsUnique = true)]
public partial class ChiTietTra
{
    [Key]
    public int MaChiTietTra { get; set; }

    public int MaPhieuTra { get; set; }

    public int MaChiTietMuon { get; set; }

    [StringLength(30)]
    public string TinhTrangTra { get; set; } = null!;

    public int SoNgayTre { get; set; }

    [StringLength(255)]
    public string? GhiChu { get; set; }

    [ForeignKey("MaChiTietMuon")]
    [InverseProperty("ChiTietTra")]
    public virtual ChiTietMuon MaChiTietMuonNavigation { get; set; } = null!;

    [ForeignKey("MaPhieuTra")]
    [InverseProperty("ChiTietTras")]
    public virtual PhieuTra MaPhieuTraNavigation { get; set; } = null!;
}
