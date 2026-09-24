using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Entities;

[Table("ChucNang")]
[Index(nameof(MaNhom), nameof(ThuTu), Name = "IX_ChucNang_Nhom_ThuTu")]
public sealed class ChucNang
{
    [Key, StringLength(50)] public string MaChucNang { get; set; } = string.Empty;
    [StringLength(30)] public string MaNhom { get; set; } = string.Empty;
    [StringLength(100)] public string TenNhom { get; set; } = string.Empty;
    [StringLength(100)] public string TenChucNang { get; set; } = string.Empty;
    public int ThuTu { get; set; }
    [InverseProperty(nameof(PhanQuyen.MaChucNangNavigation))]
    public ICollection<PhanQuyen> PhanQuyens { get; set; } = new List<PhanQuyen>();
}
