namespace DataLayer.Models
{
    public class DocGiaGridModel
    {
        public int MaDocGia { get; set; }
        public string MaDocGiaText { get; set; } = string.Empty;
        public string HoTen { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string SoDienThoai { get; set; } = string.Empty;
        public string TenLop { get; set; } = string.Empty;
        public string TenKhoa { get; set; } = string.Empty;
        public string AnhDaiDien { get; set; } = string.Empty;
        public DateTime? NgayCap { get; set; }
        public DateTime? NgayHetHan { get; set; }
        public string TrangThaiThe { get; set; } = string.Empty;
        public decimal PhiNam { get; set; }
        public bool DaDongPhiNamNay { get; set; }
        public int DangMuon { get; set; }
        public decimal NoPhat { get; set; }
    }
}
