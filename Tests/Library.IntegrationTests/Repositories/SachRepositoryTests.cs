using System;
using System.Linq;
using DataLayer.Entities;
using DataLayer.Repositories;
using BusinessLayer.Services;
using Library.IntegrationTests.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Library.IntegrationTests.Repositories;

/// Kiểm thử tích hợp Repository Sách: Kiểm tra các thao tác dữ liệu thực tế với CSDL SQL Server.
[Collection("SqlServer repository tests")]
public sealed class SachRepositoryTests : IDisposable
{
    private readonly string _prefix = "it_sach_" + Guid.NewGuid().ToString("N")[..10];
    private int _testMaSach;
    private int _testMaTheLoai;
    private int _testMaNxb;

    public SachRepositoryTests()
    {
        Environment.SetEnvironmentVariable("LIBRARY_TEST_CONNECTION_STRING", SqlServerTestDatabase.ConnectionString);
        SqlServerTestDatabase.EnsureSchema();
        using var context = SqlServerTestDatabase.CreateContext();
        context.Database.EnsureCreated();

        // Ensure we have a Category
        var activeCategory = context.TheLoais.FirstOrDefault(t => t.TrangThai);
        if (activeCategory == null)
        {
            var category = new TheLoai { TenTheLoai = _prefix + "_TL", TrangThai = true };
            context.TheLoais.Add(category);
            context.SaveChanges();
            _testMaTheLoai = category.MaTheLoai;
        }
        else
        {
            _testMaTheLoai = activeCategory.MaTheLoai;
        }

        // Ensure we have a Publisher
        var activeNxb = context.NhaXuatBans.FirstOrDefault(n => n.TrangThai);
        if (activeNxb == null)
        {
            var nxb = new NhaXuatBan { TenNxb = _prefix + "_NXB", TrangThai = true };
            context.NhaXuatBans.Add(nxb);
            context.SaveChanges();
            _testMaNxb = nxb.MaNxb;
        }
        else
        {
            _testMaNxb = activeNxb.MaNxb;
        }

        // Create a test Book
        var sach = new Sach
        {
            TenSach = _prefix + "_Book",
            MaTheLoai = _testMaTheLoai,
            MaNxb = _testMaNxb,
            TrangThai = true,
            Isbn = "TEST-" + Guid.NewGuid().ToString("N")[..8].ToUpper()
        };
        context.Saches.Add(sach);
        context.SaveChanges();
        _testMaSach = sach.MaSach;
    }

    /// Kiểm tra HasBorrowedCopies trả về true khi đầu sách có ít nhất 1 cuốn sách đang ở trạng thái 'Đang mượn'.
    [Fact]
    public void HasBorrowedCopies_KhiCoCuonSachDangMuon_TraVeTrue()
    {
        using (var context = SqlServerTestDatabase.CreateContext())
        {
            var cuon = new CuonSach
            {
                MaSach = _testMaSach,
                MaVach = _prefix + "_MV1",
                TinhTrang = "Tốt",
                TrangThai = "Đang mượn",
                NgayNhap = DateOnly.FromDateTime(DateTime.Today)
            };
            context.CuonSaches.Add(cuon);
            context.SaveChanges();
        }

        var repository = new SachRepository();
        Assert.True(repository.HasBorrowedCopies(_testMaSach));
    }

    /// Kiểm tra HasBorrowedCopies trả về false khi tất cả các cuốn sách của đầu sách đều ở trạng thái 'Có sẵn'.
    [Fact]
    public void HasBorrowedCopies_KhiChiCoCuonSachCoSan_TraVeFalse()
    {
        using (var context = SqlServerTestDatabase.CreateContext())
        {
            var cuon = new CuonSach
            {
                MaSach = _testMaSach,
                MaVach = _prefix + "_MV2",
                TinhTrang = "Tốt",
                TrangThai = "Có sẵn",
                NgayNhap = DateOnly.FromDateTime(DateTime.Today)
            };
            context.CuonSaches.Add(cuon);
            context.SaveChanges();
        }

        var repository = new SachRepository();
        Assert.False(repository.HasBorrowedCopies(_testMaSach));
    }

    /// Kiểm tra SachService.NgungKinhDoanh ném ngoại lệ InvalidOperationException khi cố ngừng kinh doanh đầu sách đang có cuốn mượn.
    [Fact]
    public void NgungKinhDoanh_KhiCoCuonSachDangMuon_NemNgoaiLe()
    {
        using (var context = SqlServerTestDatabase.CreateContext())
        {
            var cuon = new CuonSach
            {
                MaSach = _testMaSach,
                MaVach = _prefix + "_MV3",
                TinhTrang = "Tốt",
                TrangThai = "Đang mượn",
                NgayNhap = DateOnly.FromDateTime(DateTime.Today)
            };
            context.CuonSaches.Add(cuon);
            context.SaveChanges();
        }

        var service = new SachService();
        var exception = Assert.Throws<InvalidOperationException>(() => service.NgungKinhDoanh(_testMaSach));
        Assert.Contains("Không thể xóa đầu sách đang có cuốn mượn", exception.Message);
    }

    public void Dispose()
    {
        Environment.SetEnvironmentVariable("LIBRARY_TEST_CONNECTION_STRING", null);
        using var context = SqlServerTestDatabase.CreateContext();
        context.CuonSaches.Where(c => c.MaSach == _testMaSach).ExecuteDelete();
        context.Saches.Where(s => s.MaSach == _testMaSach).ExecuteDelete();
        
        // Clean up categories and publishers if created dynamically
        context.TheLoais.Where(t => t.TenTheLoai.StartsWith(_prefix)).ExecuteDelete();
        context.NhaXuatBans.Where(n => n.TenNxb.StartsWith(_prefix)).ExecuteDelete();
    }
}
