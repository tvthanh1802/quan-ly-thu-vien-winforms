using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Entities;

[Table("DongPhiThuongNien")]
[Index("MaThe", "Nam", Name = "UQ_DongPhi_The_Nam", IsUnique = true)]
public partial class DongPhiThuongNien
{
    [Key]
    public int MaDongPhi { get; set; }

    public int MaThe { get; set; }

    public int Nam { get; set; }

    public DateOnly NgayDong { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal SoTien { get; set; }

    public int? MaNhanVienThu { get; set; }

    [StringLength(30)]
    public string TrangThai { get; set; } = null!;

    [StringLength(255)]
    public string? GhiChu { get; set; }

    [StringLength(50)]
    public string LoaiLePhi { get; set; } = "Lệ phí thường niên";

    [StringLength(30)]
    public string? SoPhieuThu { get; set; }

    [StringLength(30)]
    public string HinhThucThu { get; set; } = "Tiền mặt";

    [ForeignKey("MaNhanVienThu")]
    [InverseProperty("DongPhiThuongNiens")]
    public virtual NhanVien? MaNhanVienThuNavigation { get; set; }

    [ForeignKey("MaThe")]
    [InverseProperty("DongPhiThuongNiens")]
    public virtual TheDocGium MaTheNavigation { get; set; } = null!;
}
