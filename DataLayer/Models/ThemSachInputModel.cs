namespace DataLayer.Models
{
    public sealed class SachTacGiaInputModel
    {
        public int MaTacGia { get; set; }
        public string VaiTro { get; set; } = "Tác giả";
    }

    public sealed class BanSaoTaoMoiModel
    {
        public int SoLuong { get; set; }
        public int? MaViTri { get; set; }
        public DateOnly NgayNhap { get; set; } = DateOnly.FromDateTime(DateTime.Today);
        public decimal? GiaNhap { get; set; }
        public string TinhTrang { get; set; } = "Tốt";
        public string TrangThai { get; set; } = "Có sẵn";
        public string? TienToMaVach { get; set; }
        public string? GhiChu { get; set; }
    }
}
