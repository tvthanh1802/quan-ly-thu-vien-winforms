namespace DataLayer.Models
{
    public sealed class ChiTietSachModel
    {
        public int MaSach { get; set; }
        public string MaSachText { get; set; } = string.Empty;
        public string MaSachHienThi { get; set; } = string.Empty;
        public string TenSach { get; set; } = string.Empty;
        public string Isbn { get; set; } = string.Empty;
        public int MaTheLoai { get; set; }
        public int? MaNhaXuatBan { get; set; }
        public string TheLoai { get; set; } = string.Empty;
        public string NhaXuatBan { get; set; } = string.Empty;
        public int? NamXuatBan { get; set; }
        public string NgonNgu { get; set; } = string.Empty;
        public int? SoTrang { get; set; }
        public decimal? GiaBia { get; set; }
        public string MoTa { get; set; } = string.Empty;
        public string AnhBia { get; set; } = string.Empty;
        public bool TrangThai { get; set; }
        public DateTime? NgayThem { get; set; }
        public DateTime? NgayCapNhat { get; set; }
        public string NguoiCapNhat { get; set; } = string.Empty;
        public List<TacGiaChiTietModel> TacGias { get; set; } = new();
        public List<BanSaoSachModel> BanSaos { get; set; } = new();
        public List<LichSuMuonSachChiTietModel> LichSuMuons { get; set; } = new();
        public List<DanhGiaSachChiTietModel> DanhGias { get; set; } = new();
    }

    public sealed class TacGiaChiTietModel
    {
        public int MaTacGia { get; set; }
        public string TenTacGia { get; set; } = string.Empty;
        public string VaiTro { get; set; } = string.Empty;
    }

    public sealed class BanSaoSachModel
    {
        public int MaCuonSach { get; set; }
        public string MaCuonText { get; set; } = string.Empty;
        public string MaVach { get; set; } = string.Empty;
        public string ViTri { get; set; } = string.Empty;
        public string TinhTrang { get; set; } = string.Empty;
        public string TrangThai { get; set; } = string.Empty;
        public DateTime? NgayNhap { get; set; }
    }

    public sealed class LichSuMuonSachChiTietModel
    {
        public int MaPhieuMuon { get; set; }
        public string MaPhieuText { get; set; } = string.Empty;
        public string TenDocGia { get; set; } = string.Empty;
        public DateTime? NgayMuon { get; set; }
        public DateTime? HanTra { get; set; }
        public DateTime? NgayTra { get; set; }
        public string TrangThai { get; set; } = string.Empty;
    }

    public sealed class DanhGiaSachChiTietModel
    {
        public string TenDocGia { get; set; } = string.Empty;
        public int SoSao { get; set; }
        public string NoiDung { get; set; } = string.Empty;
        public DateTime? NgayDanhGia { get; set; }
    }
}
