using DataLayer.Entities;
using DataLayer.Models;
using DataLayer.Repositories;
using DataLayer.Security;
using Library.IntegrationTests.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Library.IntegrationTests.Repositories;

/// Kiểm thử tích hợp Repository Tài khoản: Kiểm tra thêm tài khoản, đăng nhập, mã hóa mật khẩu, đặt lại mật khẩu và cơ chế khóa tạm thời.
[Collection("SqlServer repository tests")]
public sealed class TaiKhoanRepositoryTests : IDisposable
{
    private readonly string _prefix = "it_" + Guid.NewGuid().ToString("N")[..10];
    private int _testMaNhanVien;

    public TaiKhoanRepositoryTests()
    {
        SqlServerTestDatabase.EnsureSchema();
        using var context = SqlServerTestDatabase.CreateContext();
        context.Database.EnsureCreated();
        if (!context.VaiTros.Any(x => x.TenVaiTro == "Integration Test"))
        {
            context.VaiTros.Add(new VaiTro { TenVaiTro = "Integration Test", MoTa = "Dữ liệu kiểm thử tự động" });
            context.SaveChanges();
        }

        var employee = new NhanVien
        {
            HoTen = _prefix + " Employee",
            ChucVu = "Kiểm thử",
            NgayVaoLam = DateOnly.FromDateTime(DateTime.Today),
            TrangThai = true
        };
        context.NhanViens.Add(employee);
        context.SaveChanges();
        _testMaNhanVien = employee.MaNhanVien;
    }

    /// Kiểm tra khi thêm tài khoản mới, mật khẩu phải được lưu dưới dạng chuỗi mã hóa (Hash) trong CSDL chứ không lưu văn bản thô (Plaintext).
    [Fact]
    public void ThemTaiKhoan_LuuHashKhongLuuPlaintext()
    {
        var repository = CreateRepository();
        int id = repository.ThemTaiKhoan(NewAccount("create", "MatKhau@123"));
        using var verify = SqlServerTestDatabase.CreateContext();
        string stored = verify.TaiKhoans.Single(x => x.MaTaiKhoan == id).MatKhau;
        Assert.NotEqual("MatKhau@123", stored);
        Assert.True(new PasswordHasher().Verify("MatKhau@123", stored));
    }

    /// Kiểm tra Đăng nhập thành công khi mật khẩu đúng, thất bại khi mật khẩu sai, và từ chối truy cập khi tài khoản bị khóa (vô hiệu hóa).
    [Fact]
    public void DangNhap_DungThanhCong_SaiThatBai_VaTaiKhoanKhoaBiTuChoi()
    {
        string username = _prefix + "_login";
        var repository = CreateRepository();
        int id = repository.ThemTaiKhoan(NewAccount("login", "MatKhau@123"));
        Assert.NotNull(repository.DangNhap(username, "MatKhau@123"));
        Assert.Null(repository.DangNhap(username, "SaiMatKhau"));
        repository.DoiTrangThaiTaiKhoan(id, false);
        Assert.Null(repository.DangNhap(username, "MatKhau@123"));
    }

    /// Kiểm tra tài khoản cũ (Legacy plaintext) khi đăng nhập đúng sẽ tự động nâng cấp mật khẩu sang dạng mã hóa PBKDF2.
    [Fact]
    public void DangNhapLegacy_DungTuDongNangCapSangHash()
    {
        string username = _prefix + "_legacy";
        using (var seed = SqlServerTestDatabase.CreateContext())
        {
            seed.TaiKhoans.Add(NewAccount("legacy", "legacy123"));
            seed.SaveChanges();
        }
        Assert.NotNull(CreateRepository().DangNhap(username, "legacy123"));
        using var verify = SqlServerTestDatabase.CreateContext();
        string stored = verify.TaiKhoans.Single(x => x.TenDangNhap == username).MatKhau;
        Assert.True(new PasswordHasher().IsHash(stored));
        Assert.True(new PasswordHasher().Verify("legacy123", stored));
    }

    /// Kiểm tra Đặt lại mật khẩu sẽ hủy hiệu lực của mật khẩu cũ và chấp nhận mật khẩu mới khi đăng nhập.
    [Fact]
    public void DatLaiMatKhau_HuyMatKhauCu_VaChapNhanMatKhauMoi()
    {
        var repository = CreateRepository();
        TaiKhoan account = NewAccount("reset", "OldPass@123");
        int id = repository.ThemTaiKhoan(account);
        repository.DatLaiMatKhau(id, "NewPass@123");
        Assert.Null(repository.DangNhap(account.TenDangNhap, "OldPass@123"));
        Assert.NotNull(repository.DangNhap(account.TenDangNhap, "NewPass@123"));
    }

    /// Kiểm tra cơ chế đếm số lần đăng nhập sai: sau 3 lần sai liên tiếp thì tài khoản bị khóa tạm thời, sau khi hết thời gian khóa sẽ tự động mở lại.
    [Fact]
    public void DangNhapChiTiet_XacThucKhoaTamThoi3Lan()
    {
        string username = _prefix + "_lockout";
        var repository = CreateRepository();
        int id = repository.ThemTaiKhoan(NewAccount("lockout", "MatKhau@123"));

        // Lần 1: Sai mật khẩu
        var result1 = repository.DangNhapChiTiet(username, "SaiMatKhau");
        Assert.Equal(TrangThaiDangNhap.MatKhauKhongDung, result1.TrangThai);
        using (var verify = SqlServerTestDatabase.CreateContext())
        {
            var acc = verify.TaiKhoans.Single(x => x.MaTaiKhoan == id);
            Assert.Equal(1, acc.SoLanDangNhapSai);
            Assert.Null(acc.KhoaDen);
        }

        // Lần 2: Sai mật khẩu
        var result2 = repository.DangNhapChiTiet(username, "SaiMatKhau");
        Assert.Equal(TrangThaiDangNhap.MatKhauKhongDung, result2.TrangThai);
        using (var verify = SqlServerTestDatabase.CreateContext())
        {
            var acc = verify.TaiKhoans.Single(x => x.MaTaiKhoan == id);
            Assert.Equal(2, acc.SoLanDangNhapSai);
            Assert.Null(acc.KhoaDen);
        }

        // Lần 3: Sai mật khẩu -> Bị khóa tạm thời
        var result3 = repository.DangNhapChiTiet(username, "SaiMatKhau");
        Assert.Equal(TrangThaiDangNhap.TamThoiBiKhoa, result3.TrangThai);
        Assert.NotNull(result3.KhoaDen);
        using (var verify = SqlServerTestDatabase.CreateContext())
        {
            var acc = verify.TaiKhoans.Single(x => x.MaTaiKhoan == id);
            Assert.Equal(0, acc.SoLanDangNhapSai); // Reset count
            Assert.NotNull(acc.KhoaDen);
        }

        // Đang khóa: Dùng đúng mật khẩu vẫn bị từ chối
        var result4 = repository.DangNhapChiTiet(username, "MatKhau@123");
        Assert.Equal(TrangThaiDangNhap.TamThoiBiKhoa, result4.TrangThai);

        // Giả lập khóa hết hạn (đặt KhoaDen về quá khứ)
        using (var update = SqlServerTestDatabase.CreateContext())
        {
            var acc = update.TaiKhoans.Single(x => x.MaTaiKhoan == id);
            acc.KhoaDen = DateTime.Now.AddMinutes(-5);
            update.SaveChanges();
        }

        // Hết khóa: Đăng nhập đúng mật khẩu thành công và reset KhoaDen
        var result5 = repository.DangNhapChiTiet(username, "MatKhau@123");
        Assert.Equal(TrangThaiDangNhap.ThanhCong, result5.TrangThai);
        Assert.NotNull(result5.TaiKhoan);
        using (var verify = SqlServerTestDatabase.CreateContext())
        {
            var acc = verify.TaiKhoans.Single(x => x.MaTaiKhoan == id);
            Assert.Null(acc.KhoaDen);
            Assert.Equal(0, acc.SoLanDangNhapSai);
        }
    }

    /// Kiểm tra nhận diện quản trị dựa trên tên vai trò, không phụ thuộc identity phải bằng 1.
    [Fact]
    public void LaVaiTroQuanTri_VaiTroIdBatKy_VanDuocNhanDienTheoTen()
    {
        int roleId;
        using (var context = SqlServerTestDatabase.CreateContext())
        {
            var role = new VaiTro { TenVaiTro = "Quản trị " + _prefix, MoTa = "Vai trò quản trị có ID động" };
            context.VaiTros.Add(role);
            context.SaveChanges();
            roleId = role.MaVaiTro;
        }

        Assert.True(CreateRepository().LaVaiTroQuanTri(roleId));
    }

    private TaiKhoanRepository CreateRepository() => new(SqlServerTestDatabase.CreateContext);

    private TaiKhoan NewAccount(string suffix, string password)
    {
        using var context = SqlServerTestDatabase.CreateContext();
        int roleId = context.VaiTros.Single(x => x.TenVaiTro == "Integration Test").MaVaiTro;
        return new TaiKhoan { TenDangNhap = _prefix + "_" + suffix, MatKhau = password,
            MaNhanVien = _testMaNhanVien, MaVaiTro = roleId, TrangThai = true };
    }

    public void Dispose()
    {
        using var context = SqlServerTestDatabase.CreateContext();
        context.TaiKhoans.Where(x => x.TenDangNhap.StartsWith(_prefix)).ExecuteDelete();
        context.NhanViens.Where(x => x.MaNhanVien == _testMaNhanVien).ExecuteDelete();
        context.VaiTros.Where(x => x.TenVaiTro.EndsWith(_prefix)).ExecuteDelete();
    }
}
