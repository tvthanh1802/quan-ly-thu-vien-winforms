namespace BusinessLayer.DTOs
{
    public class SachFilterDto
    {
        public string? TuKhoa { get; set; }
        public int? MaTheLoai { get; set; }
        public int? MaNhaXuatBan { get; set; }
        public string? TrangThai { get; set; }
        public string SapXep { get; set; } = "MaSachDesc";
        public int Trang { get; set; } = 1;
        public int SoDongMoiTrang { get; set; } = 10;
    }
}
