using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Entities;

[Table("NhatKyHeThong")]
[Index("ThoiGian", Name = "IX_NhatKy_ThoiGian", AllDescending = true)]
public partial class NhatKyHeThong
{
    [Key]
    public long MaNhatKy { get; set; }

    public int? MaTaiKhoan { get; set; }

    [StringLength(100)]
    public string HanhDong { get; set; } = null!;

    [StringLength(100)]
    public string? BangTacDong { get; set; }

    [StringLength(100)]
    public string? KhoaChinh { get; set; }

    public string? NoiDung { get; set; }

    [Column("DiaChiIP")]
    [StringLength(50)]
    public string? DiaChiIp { get; set; }

    public DateTime ThoiGian { get; set; }

    [ForeignKey("MaTaiKhoan")]
    [InverseProperty("NhatKyHeThongs")]
    public virtual TaiKhoan? MaTaiKhoanNavigation { get; set; }
}
