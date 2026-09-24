using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Entities;

[Table("ChiTietPhat")]
public partial class ChiTietPhat
{
    [Key]
    public int MaChiTietPhat { get; set; }

    public int MaPhieuPhat { get; set; }

    public int? MaChiTietMuon { get; set; }

    [StringLength(30)]
    public string LoaiPhat { get; set; } = null!;

    [StringLength(255)]
    public string NoiDung { get; set; } = null!;

    public int SoNgayTre { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal SoTien { get; set; }

    public virtual ChiTietMuon? MaChiTietMuonNavigation { get; set; }

    [ForeignKey("MaPhieuPhat")]
    [InverseProperty("ChiTietPhats")]
    public virtual PhieuPhat MaPhieuPhatNavigation { get; set; } = null!;
}
