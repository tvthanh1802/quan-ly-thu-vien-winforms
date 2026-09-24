namespace DataLayer.Models
{
    public class LichSuMuonDashboardModel
    {
        public int MaPhieuMuon { get; set; }

        public string DocGia { get; set; } = "";

        public string TenSach { get; set; } = "";

        public DateOnly NgayMuon { get; set; }

        public DateOnly HanTra { get; set; }

        public string TrangThai { get; set; } = "";
    }
}
