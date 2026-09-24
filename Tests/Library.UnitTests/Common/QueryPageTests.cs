using BusinessLayer.Common;

namespace Library.UnitTests.Common;

/// Kiểm thử đơn vị các hàm tiện ích phân trang (PageWindow) và lọc chuỗi (QueryFilter).
public sealed class QueryPageTests
{
    /// Kiểm tra hàm PageWindow.Create tính toán chính xác số trang, số phần tử bỏ qua (Skip), và khoảng hiển thị (From - To).
    [Theory]
    [InlineData(0, 10, 25, 1, 3, 0, 1, 10)]
    [InlineData(9, 10, 25, 3, 3, 20, 21, 25)]
    [InlineData(2, 10, 25, 2, 3, 10, 11, 20)]
    [InlineData(1, 10, 0, 1, 1, 0, 0, 0)]
    public void Create_ChuanHoaTrangVaKhoang(int page, int size, int total, int expectedPage,
        int pages, int skip, int from, int to)
    {
        PageWindow x = PageWindow.Create(page, size, total);
        Assert.Equal((expectedPage, pages, skip, from, to), (x.Page, x.TotalPages, x.Skip, x.From, x.To));
    }

    /// Kiểm tra hàm QueryFilter.Contains tìm kiếm chuỗi không phân biệt hoa thường, hỗ trợ tiếng Việt có dấu và xử lý an toàn giá trị null.
    [Theory]
    [InlineData("Thư viện Điện lực", "điện", true)]
    [InlineData("Thông báo", "SÁCH", false)]
    [InlineData(null, "abc", false)]
    [InlineData(null, "", true)]
    public void Contains_KhongPhanBietHoaThuongVaNull(string? value, string? keyword, bool expected) =>
        Assert.Equal(expected, QueryFilter.Contains(value, keyword));
}


