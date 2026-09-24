using System.Collections.Generic;

namespace BusinessLayer.DTOs
{
    public class PagedResult<T>
    {
        public IReadOnlyList<T>? DuLieu { get; set; }
        public int TongBanGhi { get; set; }
        public int TrangHienTai { get; set; }
        public int SoDongMoiTrang { get; set; }
    }
}
