using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Entities;

public partial class TheDocGium
{
    [Key]
    public int MaThe { get; set; }

    [StringLength(9)]
    public string? MaTheHienThi { get; set; }

    public int MaDocGia { get; set; }

    public DateOnly NgayCap { get; set; }

    public DateOnly NgayHetHan { get; set; }

    [StringLength(30)]
    public string TrangThai { get; set; } = null!;

    [StringLength(255)]
    public string? GhiChu { get; set; }

    public DateOnly? NgayKhoa { get; set; }

    public DateOnly? NgayMoKhoaDuKien { get; set; }

    [StringLength(100)]
    public string? LyDoKhoa { get; set; }

    [StringLength(20)]
    public string? LoaiKhoa { get; set; }

    [InverseProperty("MaTheNavigation")]
    public virtual ICollection<DongPhiThuongNien> DongPhiThuongNiens { get; set; } = new List<DongPhiThuongNien>();

    [ForeignKey("MaDocGia")]
    [InverseProperty("TheDocGium")]
    public virtual DocGium MaDocGiaNavigation { get; set; } = null!;

    [InverseProperty("MaTheNavigation")]
    public virtual ICollection<PhieuMuon> PhieuMuons { get; set; } = new List<PhieuMuon>();
}
