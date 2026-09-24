using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Entities;

public partial class TacGium
{
    [Key]
    public int MaTacGia { get; set; }

    [StringLength(100)]
    public string TenTacGia { get; set; } = null!;

    public int? NamSinh { get; set; }

    [StringLength(150)]
    public string? QueQuan { get; set; }

    [StringLength(100)]
    public string? ButDanh { get; set; }

    [StringLength(255)]
    public string? GhiChu { get; set; }

    [InverseProperty("MaTacGiaNavigation")]
    public virtual ICollection<SachTacGium> SachTacGia { get; set; } = new List<SachTacGium>();
}
