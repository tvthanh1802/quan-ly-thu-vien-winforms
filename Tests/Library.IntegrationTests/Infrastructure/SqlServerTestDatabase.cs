using DataLayer.Context;
using Microsoft.EntityFrameworkCore;

namespace Library.IntegrationTests.Infrastructure;

public static class SqlServerTestDatabase
{
    public static string ConnectionString => Environment.GetEnvironmentVariable("LIBRARY_TEST_CONNECTION_STRING")
        ?? "Server=.;Database=QuanLyThuVienEPU_Test;Trusted_Connection=True;TrustServerCertificate=True";

    public static AppDbContext CreateContext()
    {
        ValidateTestDatabase();
        var options = new DbContextOptionsBuilder<AppDbContext>().UseSqlServer(ConnectionString).Options;
        return new AppDbContext(options);
    }

    public static void EnsureSchema()
    {
        ValidateTestDatabase();
        using var context = CreateContext();
        context.Database.ExecuteSqlRaw("""
            IF COL_LENGTH(N'dbo.TaiKhoan', N'SoLanDangNhapSai') IS NULL
            BEGIN
                ALTER TABLE dbo.TaiKhoan ADD SoLanDangNhapSai INT NOT NULL
                    CONSTRAINT DF_TaiKhoan_SoLanDangNhapSai DEFAULT (0);
            END;

            IF COL_LENGTH(N'dbo.TaiKhoan', N'KhoaDen') IS NULL
            BEGIN
                ALTER TABLE dbo.TaiKhoan ADD KhoaDen DATETIME2 NULL;
            END;

            IF OBJECT_ID(N'dbo.CK_CuonSach_TinhTrang', N'C') IS NOT NULL
                ALTER TABLE dbo.CuonSach DROP CONSTRAINT CK_CuonSach_TinhTrang;
            UPDATE dbo.CuonSach
                SET TinhTrang = N'Tốt'
                WHERE TinhTrang NOT IN (N'Tốt', N'Rách nhẹ', N'Hư hỏng', N'Mất');
            ALTER TABLE dbo.CuonSach WITH CHECK ADD CONSTRAINT CK_CuonSach_TinhTrang
                CHECK (TinhTrang IN (N'Tốt', N'Rách nhẹ', N'Hư hỏng', N'Mất'));

            IF OBJECT_ID(N'dbo.CK_CuonSach_TrangThai', N'C') IS NOT NULL
                ALTER TABLE dbo.CuonSach DROP CONSTRAINT CK_CuonSach_TrangThai;
            UPDATE dbo.CuonSach
                SET TrangThai = N'Có sẵn'
                WHERE TrangThai NOT IN (N'Có sẵn', N'Đang mượn', N'Đặt trước', N'Hỏng', N'Mất', N'Thanh lý');
            ALTER TABLE dbo.CuonSach WITH CHECK ADD CONSTRAINT CK_CuonSach_TrangThai
                CHECK (TrangThai IN (N'Có sẵn', N'Đang mượn', N'Đặt trước', N'Hỏng', N'Mất', N'Thanh lý'));
            """);
    }

    private static void ValidateTestDatabase()
    {
        var builder = new Microsoft.Data.SqlClient.SqlConnectionStringBuilder(ConnectionString);
        if (!builder.InitialCatalog.EndsWith("_Test", StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException("Integration test chỉ được chạy trên database có hậu tố _Test.");
    }
}
