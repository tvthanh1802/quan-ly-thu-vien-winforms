using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Entities;

[Table("ThongBao")]
[Index("MaDocGia", "DaDoc", "NgayGui", Name = "IX_ThongBao_DocGia_DaDoc", IsDescending = new[] { false, false, true })]
public partial class ThongBao
{
    [Key]
    public int MaThongBao { get; set; }

    public int MaLoaiThongBao { get; set; }

    public int? MaDocGia { get; set; }

    public int? MaNhanVien { get; set; }

    [StringLength(200)]
    public string TieuDe { get; set; } = null!;

    public string? NoiDung { get; set; }

    public DateTime NgayGui { get; set; }

    public bool DaDoc { get; set; }

    [ForeignKey("MaDocGia")]
    [InverseProperty("ThongBaos")]
    public virtual DocGium? MaDocGiaNavigation { get; set; }

    [ForeignKey("MaLoaiThongBao")]
    [InverseProperty("ThongBaos")]
    public virtual LoaiThongBao MaLoaiThongBaoNavigation { get; set; } = null!;

    [ForeignKey("MaNhanVien")]
    [InverseProperty("ThongBaos")]
    public virtual NhanVien? MaNhanVienNavigation { get; set; }
}
