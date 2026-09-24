using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Entities;

[Table("PhanQuyen")]
public sealed class PhanQuyen
{
    public int MaVaiTro { get; set; }
    [StringLength(50)] public string MaChucNang { get; set; } = string.Empty;
    public bool DuocXem { get; set; }
    public bool DuocThem { get; set; }
    public bool DuocSua { get; set; }
    public bool DuocXoa { get; set; }
    public bool DuocIn { get; set; }
    public bool DuocXuatExcel { get; set; }
    [ForeignKey(nameof(MaVaiTro)), InverseProperty(nameof(VaiTro.PhanQuyens))]
    public VaiTro MaVaiTroNavigation { get; set; } = null!;
    [ForeignKey(nameof(MaChucNang)), InverseProperty(nameof(ChucNang.PhanQuyens))]
    public ChucNang MaChucNangNavigation { get; set; } = null!;
}
