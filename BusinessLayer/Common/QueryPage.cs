namespace BusinessLayer.Common;

public readonly record struct PageWindow(int Page, int PageSize, int TotalItems, int TotalPages,
    int Skip, int From, int To)
{
    public static PageWindow Create(int page, int pageSize, int totalItems)
    {
        totalItems = Math.Max(0, totalItems);
        pageSize = pageSize > 0 ? pageSize : 10;
        int pages = Math.Max(1, (int)Math.Ceiling(totalItems / (double)pageSize));
        page = Math.Clamp(page, 1, pages);
        int skip = (page - 1) * pageSize;
        int from = totalItems == 0 ? 0 : skip + 1;
        int to = Math.Min(skip + pageSize, totalItems);
        return new(page, pageSize, totalItems, pages, skip, from, to);
    }
}

public static class QueryFilter
{
    public static bool Contains(string? value, string? keyword) =>
        string.IsNullOrWhiteSpace(keyword)
        || (!string.IsNullOrEmpty(value)
            && value.Contains(keyword.Trim(), StringComparison.CurrentCultureIgnoreCase));
}
