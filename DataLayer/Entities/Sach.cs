using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Entities;

[Table("Sach")]
[Index("MaNxb", Name = "IX_Sach_NXB")]
[Index("TenSach", Name = "IX_Sach_TenSach")]
[Index("MaTheLoai", Name = "IX_Sach_TheLoai")]
public partial class Sach
{
    [Key]
    public int MaSach { get; set; }

    [StringLength(6)]
    public string? MaSachHienThi { get; set; }

    [StringLength(200)]
    public string TenSach { get; set; } = null!;

    [Column("ISBN")]
    [StringLength(30)]
    public string? Isbn { get; set; }

    public int MaTheLoai { get; set; }

    [Column("MaNXB")]
    public int? MaNxb { get; set; }

    public int? NamXuatBan { get; set; }

    [StringLength(50)]
    public string? NgonNgu { get; set; }

    public int? SoTrang { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? GiaBia { get; set; }

    public string? MoTa { get; set; }

    [StringLength(255)]
    public string? AnhBia { get; set; }

    public bool TrangThai { get; set; }

    [InverseProperty("MaSachNavigation")]
    public virtual ICollection<ChiTietNhap> ChiTietNhaps { get; set; } = new List<ChiTietNhap>();

    [InverseProperty("MaSachNavigation")]
    public virtual ICollection<CuonSach> CuonSaches { get; set; } = new List<CuonSach>();

    [InverseProperty("MaSachNavigation")]
    public virtual ICollection<DanhGiaSach> DanhGiaSaches { get; set; } = new List<DanhGiaSach>();

    [InverseProperty("MaSachNavigation")]
    public virtual ICollection<DatTruocSach> DatTruocSaches { get; set; } = new List<DatTruocSach>();

    [ForeignKey("MaNxb")]
    [InverseProperty("Saches")]
    public virtual NhaXuatBan? MaNxbNavigation { get; set; }

    [ForeignKey("MaTheLoai")]
    [InverseProperty("Saches")]
    public virtual TheLoai MaTheLoaiNavigation { get; set; } = null!;

    [InverseProperty("MaSachNavigation")]
    public virtual ICollection<SachTacGium> SachTacGia { get; set; } = new List<SachTacGium>();
}
