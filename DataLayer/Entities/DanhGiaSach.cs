using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Entities;

[Table("DanhGiaSach")]
[Index("MaSach", "MaDocGia", Name = "UQ_DanhGia_Sach_DocGia", IsUnique = true)]
public partial class DanhGiaSach
{
    [Key]
    public int MaDanhGia { get; set; }

    public int MaSach { get; set; }

    public int MaDocGia { get; set; }

    public int SoSao { get; set; }

    [StringLength(500)]
    public string? NhanXet { get; set; }

    public DateTime NgayDanhGia { get; set; }

    public bool TrangThai { get; set; }

    [ForeignKey("MaDocGia")]
    [InverseProperty("DanhGiaSaches")]
    public virtual DocGium MaDocGiaNavigation { get; set; } = null!;

    [ForeignKey("MaSach")]
    [InverseProperty("DanhGiaSaches")]
    public virtual Sach MaSachNavigation { get; set; } = null!;
}
