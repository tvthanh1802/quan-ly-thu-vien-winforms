using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Entities;

[Table("PhieuPhat")]
[Index("MaDocGia", "TrangThai", Name = "IX_PhieuPhat_DocGia_TrangThai")]
public partial class PhieuPhat
{
    [Key]
    public int MaPhieuPhat { get; set; }

    [StringLength(8)]
    public string? MaPhieuPhatHienThi { get; set; }

    public int MaDocGia { get; set; }

    public int? MaPhieuMuon { get; set; }

    public int MaNhanVien { get; set; }

    public DateTime NgayLap { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal TongTien { get; set; }

    [StringLength(30)]
    public string TrangThai { get; set; } = null!;

    public DateTime? NgayThanhToan { get; set; }

    [StringLength(255)]
    public string? GhiChu { get; set; }

    [InverseProperty("MaPhieuPhatNavigation")]
    public virtual ICollection<ChiTietPhat> ChiTietPhats { get; set; } = new List<ChiTietPhat>();

    [ForeignKey("MaDocGia")]
    [InverseProperty("PhieuPhats")]
    public virtual DocGium MaDocGiaNavigation { get; set; } = null!;

    [ForeignKey("MaNhanVien")]
    [InverseProperty("PhieuPhats")]
    public virtual NhanVien MaNhanVienNavigation { get; set; } = null!;

    [ForeignKey("MaPhieuMuon")]
    [InverseProperty("PhieuPhats")]
    public virtual PhieuMuon? MaPhieuMuonNavigation { get; set; }
}
