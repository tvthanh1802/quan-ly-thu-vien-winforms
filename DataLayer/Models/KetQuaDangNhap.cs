using DataLayer.Entities;

namespace DataLayer.Models;

public enum TrangThaiDangNhap
{
    ThanhCong,
    TaiKhoanKhongTonTai,
    MatKhauKhongDung,
    TamThoiBiKhoa,
    TaiKhoanBiVoHieuHoa
}

public sealed class KetQuaDangNhap
{
    public TrangThaiDangNhap TrangThai { get; init; }
    public TaiKhoan? TaiKhoan { get; init; }
    public DateTime? KhoaDen { get; init; }

    public static KetQuaDangNhap ThanhCong(TaiKhoan taiKhoan) => new()
    {
        TrangThai = TrangThaiDangNhap.ThanhCong,
        TaiKhoan = taiKhoan
    };

    public static KetQuaDangNhap ThatBai(TrangThaiDangNhap trangThai, DateTime? khoaDen = null) => new()
    {
        TrangThai = trangThai,
        KhoaDen = khoaDen
    };
}

