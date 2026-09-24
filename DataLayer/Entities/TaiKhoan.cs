using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Entities;

[Table("TaiKhoan")]
[Index("TenDangNhap", Name = "UQ_TaiKhoan_TenDangNhap", IsUnique = true)]
public partial class TaiKhoan
{
    [Key]
    public int MaTaiKhoan { get; set; }

    [StringLength(50)]
    public string TenDangNhap { get; set; } = null!;

    [StringLength(255)]
    public string MatKhau { get; set; } = null!;

    public int? MaNhanVien { get; set; }

    public int? MaDocGia { get; set; }

    public int MaVaiTro { get; set; }

    public DateTime? LanDangNhapCuoi { get; set; }

    public int SoLanDangNhapSai { get; set; }

    public DateTime? KhoaDen { get; set; }

    public bool TrangThai { get; set; }

    [ForeignKey("MaDocGia")]
    [InverseProperty("TaiKhoan")]
    public virtual DocGium? MaDocGiaNavigation { get; set; }

    [ForeignKey("MaNhanVien")]
    [InverseProperty("TaiKhoan")]
    public virtual NhanVien? MaNhanVienNavigation { get; set; }

    [ForeignKey("MaVaiTro")]
    [InverseProperty("TaiKhoans")]
    public virtual VaiTro MaVaiTroNavigation { get; set; } = null!;

    [InverseProperty("MaTaiKhoanNavigation")]
    public virtual ICollection<NhatKyHeThong> NhatKyHeThongs { get; set; } = new List<NhatKyHeThong>();
}
