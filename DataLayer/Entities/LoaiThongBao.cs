using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Entities;

[Table("LoaiThongBao")]
[Index("TenLoai", Name = "UQ_LoaiThongBao_Ten", IsUnique = true)]
public partial class LoaiThongBao
{
    [Key]
    public int MaLoaiThongBao { get; set; }

    [StringLength(100)]
    public string TenLoai { get; set; } = null!;

    [StringLength(50)]
    public string Icon { get; set; } = null!;

    [StringLength(30)]
    public string Mau { get; set; } = null!;

    [StringLength(255)]
    public string? MoTa { get; set; }

    [InverseProperty("MaLoaiThongBaoNavigation")]
    public virtual ICollection<ThongBao> ThongBaos { get; set; } = new List<ThongBao>();
}
