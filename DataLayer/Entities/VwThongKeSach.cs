using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Entities;

[Keyless]
public partial class VwThongKeSach
{
    public int MaSach { get; set; }

    [StringLength(6)]
    public string? MaSachHienThi { get; set; }

    [StringLength(200)]
    public string TenSach { get; set; } = null!;

    [StringLength(100)]
    public string TenTheLoai { get; set; } = null!;

    public int? TongSoLuong { get; set; }

    public int? ConLai { get; set; }

    public int? DangMuon { get; set; }

    public int? HuHong { get; set; }

    public int? BiMat { get; set; }
}
