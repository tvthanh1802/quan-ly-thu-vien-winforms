using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Entities;

[PrimaryKey("MaSach", "MaTacGia")]
[Table("Sach_TacGia")]
public partial class SachTacGium
{
    [Key]
    public int MaSach { get; set; }

    [Key]
    public int MaTacGia { get; set; }

    [StringLength(50)]
    public string? VaiTro { get; set; }

    [ForeignKey("MaSach")]
    [InverseProperty("SachTacGia")]
    public virtual Sach MaSachNavigation { get; set; } = null!;

    [ForeignKey("MaTacGia")]
    [InverseProperty("SachTacGia")]
    public virtual TacGium MaTacGiaNavigation { get; set; } = null!;
}
