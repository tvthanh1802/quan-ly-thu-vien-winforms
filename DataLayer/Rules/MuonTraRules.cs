namespace DataLayer.Rules;

public static class MuonTraRules
{
    public const int SoNgayMuonToiDaMacDinh = 14;
    public const int SoSachMuonToiDaMacDinh = 3;
    public const decimal TienPhatMoiNgayMacDinh = 5000m;
    public const decimal TyLePhatHongMacDinh = 50m;
    public const decimal TyLePhatMatMacDinh = 100m;

    public static bool LaDangMuon(string? trangThai) =>
        string.Equals(trangThai, "Đang mượn", StringComparison.CurrentCultureIgnoreCase)
        || string.Equals(trangThai, "Quá hạn", StringComparison.CurrentCultureIgnoreCase);

    public static bool DaKetThuc(string? trangThai) => !LaDangMuon(trangThai);

    public static bool NgayMuonHopLe(DateTime ngayMuon, DateOnly hanTra, int soNgayToiDa) =>
        hanTra >= DateOnly.FromDateTime(ngayMuon)
        && !VuotHanMuon(DateOnly.FromDateTime(ngayMuon), hanTra, soNgayToiDa);

    public static int TinhSoNgayTre(DateOnly hanTra, DateOnly ngayTra) =>
        Math.Max(0, ngayTra.DayNumber - hanTra.DayNumber);

    public static decimal TinhTienPhatQuaHan(DateOnly hanTra, DateOnly ngayTra, decimal tienMoiNgay) =>
        TinhSoNgayTre(hanTra, ngayTra) * Math.Max(0, tienMoiNgay);

    public static decimal TinhTienPhatTheoTyLe(decimal giaTriSach, decimal tyLePhanTram) =>
        Math.Round(Math.Max(0, giaTriSach) * Math.Max(0, tyLePhanTram) / 100m, 2,
            MidpointRounding.AwayFromZero);

    public static bool VuotGioiHanSach(int dangMuon, int soThem, int toiDa) =>
        toiDa <= 0 || Math.Max(0, dangMuon) + Math.Max(0, soThem) > toiDa;

    public static bool VuotHanMuon(DateOnly ngayMuon, DateOnly hanTra, int soNgayToiDa) =>
        soNgayToiDa <= 0 || hanTra.DayNumber - ngayMuon.DayNumber > soNgayToiDa;

    public static string TinhTrangThai(DateOnly hanTra, DateOnly homNay, bool conSachChuaTra,
        bool matSach, bool huHong)
    {
        if (matSach) return "Mất sách";
        if (huHong) return "Hư hỏng";
        if (conSachChuaTra && hanTra < homNay) return "Quá hạn";
        return conSachChuaTra ? "Đang mượn" : "Đã trả";
    }
}

