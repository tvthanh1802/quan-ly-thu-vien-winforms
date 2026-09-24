using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Entities;

[Table("TheLoai")]
[Index("TenTheLoai", Name = "UQ_TheLoai_Ten", IsUnique = true)]
public partial class TheLoai
{
    [Key]
    public int MaTheLoai { get; set; }

    [StringLength(100)]
    public string TenTheLoai { get; set; } = null!;

    [StringLength(255)]
    public string? MoTa { get; set; }

    public bool TrangThai { get; set; }

    [InverseProperty("MaTheLoaiNavigation")]
    public virtual ICollection<Sach> Saches { get; set; } = new List<Sach>();
}
