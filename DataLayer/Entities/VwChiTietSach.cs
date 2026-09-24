using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Entities;

[Keyless]
public partial class VwChiTietSach
{
    public int MaSach { get; set; }

    [StringLength(6)]
    public string? MaSachHienThi { get; set; }

    [StringLength(200)]
    public string TenSach { get; set; } = null!;

    [Column("ISBN")]
    [StringLength(30)]
    public string? Isbn { get; set; }

    public int? NamXuatBan { get; set; }

    [StringLength(50)]
    public string? NgonNgu { get; set; }

    public int? SoTrang { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? GiaBia { get; set; }

    public string? MoTa { get; set; }

    [StringLength(255)]
    public string? AnhBia { get; set; }

    [StringLength(100)]
    public string TenTheLoai { get; set; } = null!;

    [Column("TenNXB")]
    [StringLength(150)]
    public string? TenNxb { get; set; }

    [StringLength(4000)]
    public string? TacGia { get; set; }
}
