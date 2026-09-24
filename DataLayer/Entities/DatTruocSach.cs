using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Entities;

[Table("DatTruocSach")]
public partial class DatTruocSach
{
    [Key]
    public int MaDatTruoc { get; set; }

    public int MaDocGia { get; set; }

    public int MaSach { get; set; }

    public DateTime NgayDat { get; set; }

    public DateTime? HanGiuDen { get; set; }

    [StringLength(30)]
    public string TrangThai { get; set; } = null!;

    [StringLength(255)]
    public string? GhiChu { get; set; }

    [ForeignKey("MaDocGia")]
    [InverseProperty("DatTruocSaches")]
    public virtual DocGium MaDocGiaNavigation { get; set; } = null!;

    [ForeignKey("MaSach")]
    [InverseProperty("DatTruocSaches")]
    public virtual Sach MaSachNavigation { get; set; } = null!;
}
