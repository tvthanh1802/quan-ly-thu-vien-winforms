using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Entities;

[Table("QuyDinh")]
public partial class QuyDinh
{
    [Key]
    public int MaQuyDinh { get; set; }

    [StringLength(100)]
    public string TenQuyDinh { get; set; } = null!;

    public int SoNgayMuonToiDa { get; set; }

    public int SoSachMuonToiDa { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal PhiThuongNien { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal TienPhatMoiNgay { get; set; }

    [Column(TypeName = "decimal(5, 2)")]
    public decimal TyLePhatHong { get; set; }

    [Column(TypeName = "decimal(5, 2)")]
    public decimal TyLePhatMat { get; set; }

    public DateOnly NgayApDung { get; set; }

    public bool TrangThai { get; set; }
}
