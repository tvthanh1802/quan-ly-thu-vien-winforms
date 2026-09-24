using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    public class LichSuMuonSachModel
    {
        public int MaPhieuMuon { get; set; }

        public string MaPhieuMuonText { get; set; } =
            string.Empty;

        public DateTime? NgayMuon { get; set; }

        public string MaDocGia { get; set; } =
            string.Empty;

        public string TenDocGia { get; set; } =
            string.Empty;

        public string MaSach { get; set; } =
            string.Empty;

        public string TenSach { get; set; } =
            string.Empty;

        public DateTime? HanTra { get; set; }

        public DateTime? NgayTra { get; set; }

        public string TrangThai { get; set; } =
            string.Empty;

        public int SoNgayMuon { get; set; }
    }
}