namespace BusinessLayer.DTOs
{
    public class SachSaveDto
    {
        public int MaSach { get; set; }
        public string? MaSachHienThi { get; set; }
        public string TenSach { get; set; } = string.Empty;
        public string? Isbn { get; set; }
        public int MaTheLoai { get; set; }
        public int? MaNhaXuatBan { get; set; }
        public int? NamXuatBan { get; set; }
        public string? NgonNgu { get; set; }
        public int? SoTrang { get; set; }
        public decimal? GiaBia { get; set; }
        public int? MaViTri { get; set; }
        public string? MoTa { get; set; }
        public string? AnhBia { get; set; }
        public bool TrangThai { get; set; } = true;

        // Giữ tương thích với các form/import cũ.
        public int SoLuong { get; set; } = 1;
        public List<int> MaTacGia { get; set; } = new();

        // Dữ liệu đầy đủ của FrmThemSach.
        public List<SachTacGiaSaveDto> TacGiaChiTiet { get; set; } = new();
        public DateOnly NgayNhap { get; set; } = DateOnly.FromDateTime(DateTime.Today);
        public decimal? GiaNhap { get; set; }
        public string TinhTrangCuon { get; set; } = "Tốt";
        public string TrangThaiCuon { get; set; } = "Có sẵn";
        public string? TienToMaVach { get; set; }
        public string? GhiChuCuon { get; set; }
    }

    public class SachTacGiaSaveDto
    {
        public int MaTacGia { get; set; }
        public string VaiTro { get; set; } = "Tác giả";
    }
}
