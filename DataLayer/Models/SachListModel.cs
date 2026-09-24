namespace DataLayer.Models
{
    public class SachListModel
    {
        public int MaSach { get; set; }
        public string MaSachHienThi => $"S{MaSach:D5}";
        public string TenSach { get; set; } = string.Empty;
        public string TacGia { get; set; } = string.Empty;
        public string TenTheLoai { get; set; } = string.Empty;
        public string TenNhaXuatBan { get; set; } = string.Empty;
        public int? NamXuatBan { get; set; }
        public string Isbn { get; set; } = string.Empty;
        public string AnhBia { get; set; } = string.Empty;
        public string ViTri { get; set; } = string.Empty;
        public int SoLuong { get; set; }
        public int DangMuon { get; set; }
        public int ConLai { get; set; }

        public string TrangThai
        {
            get
            {
                if (SoLuong <= 0)
                {
                    return "Chưa có cuốn";
                }

                if (ConLai > 0)
                {
                    return "Còn sẵn";
                }

                if (DangMuon > 0)
                {
                    return "Đang mượn";
                }

                return "Hết sách";
            }
        }
    }
}
