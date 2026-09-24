using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Entities;

[Table("Khoa")]
[Index("TenKhoa", Name = "UQ_Khoa_TenKhoa", IsUnique = true)]
public partial class Khoa
{
    [Key]
    public int MaKhoa { get; set; }

    [StringLength(100)]
    public string TenKhoa { get; set; } = null!;

    public bool TrangThai { get; set; }

    [InverseProperty("MaKhoaNavigation")]
    public virtual ICollection<Lop> Lops { get; set; } = new List<Lop>();
}
