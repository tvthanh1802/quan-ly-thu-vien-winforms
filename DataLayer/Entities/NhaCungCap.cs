using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Entities;

[Table("NhaCungCap")]
[Index("TenNcc", Name = "UQ_NCC_Ten", IsUnique = true)]
public partial class NhaCungCap
{
    [Key]
    [Column("MaNCC")]
    public int MaNcc { get; set; }

    [Column("TenNCC")]
    [StringLength(150)]
    public string TenNcc { get; set; } = null!;

    [StringLength(255)]
    public string? DiaChi { get; set; }

    [StringLength(15)]
    public string? SoDienThoai { get; set; }

    [StringLength(100)]
    public string? Email { get; set; }

    [StringLength(100)]
    public string? NguoiDaiDien { get; set; }

    [StringLength(30)]
    public string? MaSoThue { get; set; }

    [StringLength(200)]
    public string? Website { get; set; }

    [StringLength(50)]
    public string? LoaiNcc { get; set; }

    [StringLength(255)]
    public string? Logo { get; set; }

    [StringLength(100)]
    public string? DieuKhoanThanhToan { get; set; }

    public DateOnly? NgayBatDauHopTac { get; set; }

    [StringLength(100)]
    public string? KhuVucCungCap { get; set; }

    [StringLength(500)]
    public string? NhomSachCungCap { get; set; }

    [StringLength(500)]
    public string? GhiChu { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal HanMucCongNo { get; set; }

    [Column(TypeName = "decimal(5, 2)")]
    public decimal ChietKhauMacDinh { get; set; }

    [StringLength(50)]
    public string? PhuongThucThanhToan { get; set; }

    [StringLength(30)]
    public string? DanhGiaBanDau { get; set; }

    public bool TrangThai { get; set; }

    [InverseProperty("MaNccNavigation")]
    public virtual ICollection<PhieuNhap> PhieuNhaps { get; set; } = new List<PhieuNhap>();
}
