using DataLayer.Context;
using DataLayer.Models;
using DataLayer.Repositories;

namespace BusinessLayer.Services;

public sealed class NhanVienService
{
    private static NhanVienRepository CreateRepository() => new(new AppDbContext());

    public List<NhanVienGridModel> GetDanhSach() => CreateRepository().GetDanhSach();
    public NhanVienStatisticsModel GetStatistics() => CreateRepository().GetStatistics();
    public List<string> GetChucVu() => CreateRepository().GetChucVu();
    public List<VaiTroLookupModel> GetVaiTroLookup() => CreateRepository().GetVaiTroLookup();
    public NhanVienDetailModel? GetChiTiet(int maNhanVien) => maNhanVien > 0 ? CreateRepository().GetChiTiet(maNhanVien) : null;
    public bool ToggleTrangThaiNhanVien(int maNhanVien) => maNhanVien > 0 && CreateRepository().ToggleTrangThaiNhanVien(maNhanVien);
    public bool ToggleTaiKhoan(int maNhanVien) => maNhanVien > 0 && CreateRepository().ToggleTaiKhoan(maNhanVien);
    public bool DatLaiMatKhau(int maNhanVien, string matKhauMoi, bool moKhoaTaiKhoan = false)
    {
        if (maNhanVien <= 0) throw new ArgumentException("Mã nhân viên không hợp lệ.");
        ValidatePassword(matKhauMoi, "Mật khẩu");

        NhanVienDetailModel? nhanVien = GetChiTiet(maNhanVien);
        if (nhanVien == null) throw new InvalidOperationException("Không tìm thấy nhân viên.");
        if (string.Equals(matKhauMoi, nhanVien.TenDangNhap, StringComparison.OrdinalIgnoreCase))
            throw new ArgumentException("Mật khẩu không được trùng với tên đăng nhập.");

        return CreateRepository().DatLaiMatKhau(maNhanVien, matKhauMoi, moKhoaTaiKhoan);
    }
    public bool ChoNghiViec(int maNhanVien) => maNhanVien > 0 && CreateRepository().ChoNghiViec(maNhanVien);
    public bool CapNhatVaiTro(int maNhanVien, string tuKhoaVaiTro) => maNhanVien > 0 && !string.IsNullOrWhiteSpace(tuKhoaVaiTro) && CreateRepository().CapNhatVaiTro(maNhanVien, tuKhoaVaiTro);
    public bool CapNhatVaiTroTheoMa(int maNhanVien, int maVaiTro) => maNhanVien > 0 && maVaiTro > 0 && CreateRepository().CapNhatVaiTroTheoMa(maNhanVien, maVaiTro);
    public bool TonTaiTenDangNhap(string tenDangNhap) => !string.IsNullOrWhiteSpace(tenDangNhap) && CreateRepository().TonTaiTenDangNhap(tenDangNhap.Trim());
    public bool TonTaiEmail(string email) => !string.IsNullOrWhiteSpace(email) && CreateRepository().TonTaiEmail(email.Trim());
    public bool TonTaiSoDienThoai(string soDienThoai) => !string.IsNullOrWhiteSpace(soDienThoai) && CreateRepository().TonTaiSoDienThoai(soDienThoai.Trim());
    public bool TonTaiTenDangNhapKhac(int maNhanVien, string tenDangNhap) => maNhanVien > 0 && !string.IsNullOrWhiteSpace(tenDangNhap) && CreateRepository().TonTaiTenDangNhapKhac(maNhanVien, tenDangNhap.Trim());
    public bool TonTaiEmailKhac(int maNhanVien, string email) => maNhanVien > 0 && !string.IsNullOrWhiteSpace(email) && CreateRepository().TonTaiEmailKhac(maNhanVien, email.Trim());
    public bool TonTaiSoDienThoaiKhac(int maNhanVien, string soDienThoai) => maNhanVien > 0 && !string.IsNullOrWhiteSpace(soDienThoai) && CreateRepository().TonTaiSoDienThoaiKhac(maNhanVien, soDienThoai.Trim());

    public ThemNhanVienResultModel ThemNhanVien(ThemNhanVienInputModel input)
    {
        ArgumentNullException.ThrowIfNull(input);
        input.HoTen = input.HoTen.Trim();
        input.GioiTinh = input.GioiTinh.Trim();
        input.ChucVu = input.ChucVu.Trim();
        input.SoDienThoai = ChuanHoaNullable(input.SoDienThoai);
        input.Email = ChuanHoaNullable(input.Email);
        input.DiaChi = ChuanHoaNullable(input.DiaChi);
        input.AnhDaiDien = ChuanHoaNullable(input.AnhDaiDien);
        input.TenDangNhap = input.TenDangNhap.Trim();

        if (input.HoTen.Length == 0) throw new ArgumentException("Họ và tên không được để trống.");
        if (input.GioiTinh.Length == 0) throw new ArgumentException("Giới tính không được để trống.");
        if (input.ChucVu.Length == 0) throw new ArgumentException("Chức vụ không được để trống.");
        if (input.NgaySinh.HasValue && input.NgaySinh.Value >= DateOnly.FromDateTime(DateTime.Today))
            throw new ArgumentException("Ngày sinh phải nhỏ hơn ngày hiện tại.");
        if (input.NgayVaoLam > DateOnly.FromDateTime(DateTime.Today))
            throw new ArgumentException("Ngày vào làm không được lớn hơn ngày hiện tại.");
        if (input.TenDangNhap.Length < 4 || input.TenDangNhap.Any(char.IsWhiteSpace))
            throw new ArgumentException("Tên đăng nhập phải có ít nhất 4 ký tự và không chứa khoảng trắng.");
        ValidatePassword(input.MatKhau, "Mật khẩu");
        if (input.MaVaiTro <= 0) throw new ArgumentException("Vai trò tài khoản không hợp lệ.");

        return CreateRepository().ThemNhanVien(input);
    }

    public bool CapNhatNhanVien(CapNhatNhanVienInputModel input)
    {
        ArgumentNullException.ThrowIfNull(input);
        input.HoTen = input.HoTen.Trim();
        input.GioiTinh = input.GioiTinh.Trim();
        input.ChucVu = input.ChucVu.Trim();
        input.SoDienThoai = ChuanHoaNullable(input.SoDienThoai);
        input.Email = ChuanHoaNullable(input.Email);
        input.DiaChi = ChuanHoaNullable(input.DiaChi);
        input.AnhDaiDien = ChuanHoaNullable(input.AnhDaiDien);
        input.TenDangNhap = input.TenDangNhap.Trim();
        input.MatKhauMoi = ChuanHoaNullable(input.MatKhauMoi);

        if (input.MaNhanVien <= 0) throw new ArgumentException("Mã nhân viên không hợp lệ.");
        if (input.HoTen.Length == 0) throw new ArgumentException("Họ và tên không được để trống.");
        if (input.GioiTinh.Length == 0) throw new ArgumentException("Giới tính không được để trống.");
        if (input.ChucVu.Length == 0) throw new ArgumentException("Chức vụ không được để trống.");
        if (input.NgaySinh.HasValue && input.NgaySinh.Value >= DateOnly.FromDateTime(DateTime.Today))
            throw new ArgumentException("Ngày sinh phải nhỏ hơn ngày hiện tại.");
        if (input.NgayVaoLam > DateOnly.FromDateTime(DateTime.Today))
            throw new ArgumentException("Ngày vào làm không được lớn hơn ngày hiện tại.");
        if (input.TenDangNhap.Length < 4 || input.TenDangNhap.Any(char.IsWhiteSpace))
            throw new ArgumentException("Tên đăng nhập phải có ít nhất 4 ký tự và không chứa khoảng trắng.");
        if (input.MaVaiTro <= 0) throw new ArgumentException("Vai trò tài khoản không hợp lệ.");
        if (input.MatKhauMoi != null) ValidatePassword(input.MatKhauMoi, "Mật khẩu mới");

        return CreateRepository().CapNhatNhanVien(input);
    }

    public void CapNhatAnhDaiDien(int maNhanVien, string? anhDaiDien)
    {
        if (maNhanVien <= 0) throw new ArgumentException("Mã nhân viên không hợp lệ.");
        NhanVienRepository repository = CreateRepository();
        repository.CapNhatAnhDaiDien(maNhanVien, ChuanHoaNullable(anhDaiDien));
    }

    public static void ValidatePassword(string? password, string fieldName = "Mật khẩu")
    {
        password ??= string.Empty;
        if (password.Length < 8) throw new ArgumentException($"{fieldName} phải có ít nhất 8 ký tự.");
        if (!password.Any(char.IsUpper)) throw new ArgumentException($"{fieldName} phải có ít nhất một chữ cái viết hoa.");
        if (!password.Any(char.IsLower)) throw new ArgumentException($"{fieldName} phải có ít nhất một chữ cái viết thường.");
        if (!password.Any(char.IsDigit)) throw new ArgumentException($"{fieldName} phải có ít nhất một chữ số.");
        if (!password.Any(ch => !char.IsLetterOrDigit(ch) && !char.IsWhiteSpace(ch)))
            throw new ArgumentException($"{fieldName} phải có ít nhất một ký tự đặc biệt.");
        if (password.Any(char.IsWhiteSpace)) throw new ArgumentException($"{fieldName} không được chứa khoảng trắng.");
    }

    private static string? ChuanHoaNullable(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}
