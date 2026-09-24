using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Entities;

[Table("ViTriSach")]
[Index("TenKe", "Tang", "KhuVuc", Name = "UQ_ViTriSach", IsUnique = true)]
public partial class ViTriSach
{
    [Key]
    public int MaViTri { get; set; }

    [StringLength(50)]
    public string TenKe { get; set; } = null!;

    [StringLength(50)]
    public string? Tang { get; set; }

    [StringLength(100)]
    public string? KhuVuc { get; set; }

    [StringLength(255)]
    public string? GhiChu { get; set; }

    [InverseProperty("MaViTriNavigation")]
    public virtual ICollection<CuonSach> CuonSaches { get; set; } = new List<CuonSach>();
}
