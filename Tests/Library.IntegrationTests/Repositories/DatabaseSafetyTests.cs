using Library.IntegrationTests.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Library.IntegrationTests.Repositories;

/// Kiểm thử an toàn CSDL tích hợp: Đảm bảo môi trường kiểm thử không tác động đến CSDL sản xuất.
public sealed class DatabaseSafetyTests
{
    /// Kiểm tra Chuỗi kết nối mặc định của môi trường test phải trỏ tới database test (_Test), không được trỏ vào database chính (QuanLyThuVienEPU).
    [Fact]
    public void ConnectionString_MacDinh_ChiDenDatabaseTest()
    {
        var builder = new Microsoft.Data.SqlClient.SqlConnectionStringBuilder(SqlServerTestDatabase.ConnectionString);
        Assert.EndsWith("_Test", builder.InitialCatalog, StringComparison.OrdinalIgnoreCase);
        Assert.NotEqual("QuanLyThuVienEPU", builder.InitialCatalog);
    }

    /// Kiểm tra AppDbContext khi khởi tạo từ SqlServerTestDatabase kết nối đúng vào database test.
    [Fact]
    public void AppDbContext_NhanOptionsTestDatabase()
    {
        using var context = SqlServerTestDatabase.CreateContext();
        Assert.EndsWith("_Test", context.Database.GetDbConnection().Database, StringComparison.OrdinalIgnoreCase);
    }
}


