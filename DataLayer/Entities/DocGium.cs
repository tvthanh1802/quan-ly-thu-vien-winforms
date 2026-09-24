using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Entities;

public partial class DocGium
{
    [Key]
    public int MaDocGia { get; set; }

    [StringLength(20)]
    public string? MaSinhVien { get; set; }

    [StringLength(100)]
    public string HoTen { get; set; } = null!;

    [StringLength(10)]
    public string? GioiTinh { get; set; }

    public DateOnly? NgaySinh { get; set; }

    [StringLength(15)]
    public string? SoDienThoai { get; set; }

    [StringLength(100)]
    public string? Email { get; set; }

    [StringLength(255)]
    public string? DiaChi { get; set; }

    [StringLength(255)]
    public string? AnhDaiDien { get; set; }

    public int? MaLop { get; set; }

    [StringLength(30)]
    public string LoaiDocGia { get; set; } = null!;

    public DateOnly NgayDangKy { get; set; }

    public bool TrangThai { get; set; }

    [InverseProperty("MaDocGiaNavigation")]
    public virtual ICollection<DanhGiaSach> DanhGiaSaches { get; set; } = new List<DanhGiaSach>();

    [InverseProperty("MaDocGiaNavigation")]
    public virtual ICollection<DatTruocSach> DatTruocSaches { get; set; } = new List<DatTruocSach>();

    [ForeignKey("MaLop")]
    [InverseProperty("DocGia")]
    public virtual Lop? MaLopNavigation { get; set; }

    [InverseProperty("MaDocGiaNavigation")]
    public virtual ICollection<PhieuPhat> PhieuPhats { get; set; } = new List<PhieuPhat>();

    [InverseProperty("MaDocGiaNavigation")]
    public virtual TaiKhoan? TaiKhoan { get; set; }

    [InverseProperty("MaDocGiaNavigation")]
    public virtual TheDocGium? TheDocGium { get; set; }

    [InverseProperty("MaDocGiaNavigation")]
    public virtual ICollection<ThongBao> ThongBaos { get; set; } = new List<ThongBao>();
}
