namespace DataLayer.Models;

public sealed class MuonTraStatisticsModel
{
    public int SoCuonDangMuon { get; set; }
    public int SoPhieuDangMuon { get; set; }
    public int SoPhieuTraHomNay { get; set; }
    public int SoPhieuQuaHan { get; set; }
    public decimal TienPhatChuaThu { get; set; }
    public int SoPhieuPhatChuaThu { get; set; }
}

public sealed class MuonTraGridModel
{
    public int MaPhieuMuon { get; set; }
    public string MaPhieuText { get; set; } = string.Empty;
    public int MaDocGia { get; set; }
    public string MaDocGiaText { get; set; } = string.Empty;
    public string HoTen { get; set; } = string.Empty;
    public string AnhDaiDien { get; set; } = string.Empty;
    public int SoSach { get; set; }
    public DateTime NgayMuon { get; set; }
    public DateTime HanTra { get; set; }
    public DateTime? NgayTra { get; set; }
    public string TrangThai { get; set; } = string.Empty;
    public decimal TienPhat { get; set; }
    public string TenSachTimKiem { get; set; } = string.Empty;

    public string NgayMuonText => NgayMuon.ToString("dd/MM/yyyy");
    public string HanTraText => HanTra.ToString("dd/MM/yyyy");
    public string NgayTraText => NgayTra?.ToString("dd/MM/yyyy") ?? "-";
    public string TienPhatText => TienPhat <= 0 ? "0 đ" : $"{TienPhat:N0} đ";
}

public sealed class MuonTraDetailModel
{
    public int MaPhieuMuon { get; set; }
    public string MaPhieuText { get; set; } = string.Empty;
    public int MaDocGia { get; set; }
    public string MaDocGiaText { get; set; } = string.Empty;
    public string HoTen { get; set; } = string.Empty;
    public string AnhDaiDien { get; set; } = string.Empty;
    public string SoDienThoai { get; set; } = string.Empty;
    public string LopDonVi { get; set; } = string.Empty;
    public string TrangThaiThe { get; set; } = string.Empty;
    public DateTime NgayMuon { get; set; }
    public DateTime HanTra { get; set; }
    public int SoSach { get; set; }
    public decimal TongTienPhat { get; set; }
    public int? MaPhieuPhatChuaThanhToan { get; set; }
    public List<SachMuonItemModel> Saches { get; set; } = new();
    public List<LichSuMuonTraItemModel> LichSu { get; set; } = new();
    public List<PhieuPhatDetailModel> PhieuPhats { get; set; } = new();
}

public sealed class SachMuonItemModel
{
    public int MaSach { get; set; }
    public string MaSachText { get; set; } = string.Empty;
    public string TenSach { get; set; } = string.Empty;
    public string AnhBia { get; set; } = string.Empty;
    public string MaCuonText { get; set; } = string.Empty;
    public string TrangThai { get; set; } = string.Empty;
    public DateTime HanTra { get; set; }
}

public sealed class LichSuMuonTraItemModel
{
    public DateTime Ngay { get; set; }
    public string Loai { get; set; } = string.Empty;
    public string NguoiThucHien { get; set; } = string.Empty;
    public string NgayText => Ngay.ToString("dd/MM/yyyy HH:mm");
}

public sealed class PhieuPhatDetailModel
{
    public int MaPhieuPhat { get; set; }
    public string LoaiPhat { get; set; } = string.Empty;
    public string NoiDung { get; set; } = string.Empty;
    public int SoNgayTre { get; set; }
    public decimal SoTien { get; set; }
    public string TrangThai { get; set; } = string.Empty;
    public string GhiChu { get; set; } = string.Empty;
    public DateTime NgayLap { get; set; }
    public DateTime? NgayThanhToan { get; set; }
    public string MaPhieuPhatText => $"PP{MaPhieuPhat:D6}";
}
