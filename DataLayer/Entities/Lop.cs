using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Entities;

[Table("Lop")]
[Index("TenLop", "MaKhoa", Name = "UQ_Lop_TenLop_MaKhoa", IsUnique = true)]
public partial class Lop
{
    [Key]
    public int MaLop { get; set; }

    [StringLength(50)]
    public string TenLop { get; set; } = null!;

    public int MaKhoa { get; set; }

    [StringLength(20)]
    public string? KhoaHoc { get; set; }

    public bool TrangThai { get; set; }

    [InverseProperty("MaLopNavigation")]
    public virtual ICollection<DocGium> DocGia { get; set; } = new List<DocGium>();

    [ForeignKey("MaKhoa")]
    [InverseProperty("Lops")]
    public virtual Khoa MaKhoaNavigation { get; set; } = null!;
}
