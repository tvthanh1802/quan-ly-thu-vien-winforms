using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Entities;

[Table("ChiTietMuon")]
[Index("MaPhieuMuon", "TrangThai", Name = "IX_ChiTietMuon_Phieu_TrangThai")]
[Index("MaPhieuMuon", "MaCuonSach", Name = "UQ_CTM_Phieu_Cuon", IsUnique = true)]
public partial class ChiTietMuon
{
    [Key]
    public int MaChiTietMuon { get; set; }

    public int MaPhieuMuon { get; set; }

    public int MaCuonSach { get; set; }

    [StringLength(30)]
    public string TrangThai { get; set; } = null!;

    [StringLength(255)]
    public string? GhiChu { get; set; }

    [InverseProperty("MaChiTietMuonNavigation")]
    public virtual ICollection<ChiTietPhat> ChiTietPhats { get; set; } = new List<ChiTietPhat>();

    [InverseProperty("MaChiTietMuonNavigation")]
    public virtual ChiTietTra? ChiTietTra { get; set; }

    [ForeignKey("MaCuonSach")]
    [InverseProperty("ChiTietMuons")]
    public virtual CuonSach MaCuonSachNavigation { get; set; } = null!;

    [ForeignKey("MaPhieuMuon")]
    [InverseProperty("ChiTietMuons")]
    public virtual PhieuMuon MaPhieuMuonNavigation { get; set; } = null!;
}
