namespace DataLayer.Models;

public sealed class ThongBaoEditModel
{
    public int MaThongBao { get; set; }
    public int MaLoaiThongBao { get; set; }
    public string TieuDe { get; set; } = string.Empty;
    public string NoiDung { get; set; } = string.Empty;
    public DateTime NgayGui { get; set; }
    public bool DaDoc { get; set; }
    public int? MaNhanVien { get; set; }
    public int? MaDocGia { get; set; }
    public string DoiTuongNhan { get; set; } = "Toàn hệ thống";
}

public sealed class LoaiThongBaoLookupModel
{
    public int MaLoaiThongBao { get; set; }
    public string TenLoai { get; set; } = string.Empty;
    public string Icon { get; set; } = "Bell";
    public string Mau { get; set; } = "#6432EB";
}

public sealed class ThemThongBaoInputModel
{
    public int MaLoaiThongBao { get; set; }
    public string TieuDe { get; set; } = string.Empty;
    public string NoiDung { get; set; } = string.Empty;
    public DateTime NgayGui { get; set; }
    public bool GuiNhanVien { get; set; }
    public bool GuiDocGia { get; set; }
    public bool GuiToanHeThong { get; set; }
}

public sealed class ThemThongBaoResultModel
{
    public int SoBanGhiDaTao { get; set; }
    public int SoNhanVienNhan { get; set; }
    public int SoDocGiaNhan { get; set; }
}

public sealed class ThongKeNguoiNhanModel
{
    public int SoNhanVien { get; set; }
    public int SoDocGia { get; set; }
    public int TongCong => SoNhanVien + SoDocGia;
}
