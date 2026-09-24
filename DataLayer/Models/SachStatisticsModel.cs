namespace DataLayer.Models
{
    public class SachStatisticsModel
    {
        public int TongDauSach { get; set; }
        public int TongCuonSach { get; set; }
        public int SachCoSan { get; set; }
        public int SachDangMuon { get; set; }
        public int SachQuaHan { get; set; }
        public int SachMoiTrongThang { get; set; }
        // Previous month values for delta display
        public int PrevTongCuonSach { get; set; }
        public int PrevSachCoSan { get; set; }
        public int PrevSachDangMuon { get; set; }
        public int PrevSachQuaHan { get; set; }
        public int PrevSachMoiTrongThang { get; set; }
    }
}
