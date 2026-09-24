namespace DataLayer.Models;

public sealed class ThongKeCardModel
{
    public int TongDauSach { get; set; }
    public int DauSachMoiKyNay { get; set; }
    public int DauSachMoiKyTruoc { get; set; }
    public int TongBanSao { get; set; }
    public int BanSaoMoiKyNay { get; set; }
    public int BanSaoMoiKyTruoc { get; set; }
    public int TongDocGia { get; set; }
    public int DocGiaMoiKyNay { get; set; }
    public int DocGiaMoiKyTruoc { get; set; }
    public int DangMuon { get; set; }
    public int LuotMuonKyNay { get; set; }
    public int LuotMuonKyTruoc { get; set; }
    public int QuaHanKyNay { get; set; }
    public int QuaHanKyTruoc { get; set; }
    public decimal TienPhatKyNay { get; set; }
    public decimal TienPhatKyTruoc { get; set; }
}

public sealed class TopSachMuonModel
{
    public int MaSach { get; set; }
    public string TenSach { get; set; } = string.Empty;
    public string? AnhBia { get; set; }
    public int SoLuotMuon { get; set; }
}

public sealed class TopDocGiaMuonModel
{
    public int MaDocGia { get; set; }
    public string HoTen { get; set; } = string.Empty;
    public string? AnhDaiDien { get; set; }
    public int SoLuotMuon { get; set; }
}

public sealed class BaoCaoQuaHanNgayModel
{
    public DateTime Ngay { get; set; }
    public int SoSachQuaHan { get; set; }
    public decimal TienPhat { get; set; }
}

public sealed class ThongKePhieuNhapThangModel
{
    public DateTime Thang { get; set; }
    public int SoPhieuNhap { get; set; }
    public int TongSoBanSaoNhap { get; set; }
    public decimal TongTienNhap { get; set; }
}
