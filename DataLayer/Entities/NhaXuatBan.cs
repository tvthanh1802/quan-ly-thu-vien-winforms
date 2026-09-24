using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Entities;

[Table("NhaXuatBan")]
[Index("TenNxb", Name = "UQ_NhaXuatBan_Ten", IsUnique = true)]
public partial class NhaXuatBan
{
    [Key]
    [Column("MaNXB")]
    public int MaNxb { get; set; }

    [Column("TenNXB")]
    [StringLength(150)]
    public string TenNxb { get; set; } = null!;

    [StringLength(255)]
    public string? DiaChi { get; set; }

    [StringLength(15)]
    public string? SoDienThoai { get; set; }

    [StringLength(100)]
    public string? Email { get; set; }

    public bool TrangThai { get; set; }

    [InverseProperty("MaNxbNavigation")]
    public virtual ICollection<Sach> Saches { get; set; } = new List<Sach>();
}
