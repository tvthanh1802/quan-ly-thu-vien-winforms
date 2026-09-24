using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    public class DocGiaStatisticsModel
    {
        public int TongDocGia { get; set; }
        public int TongDocGiaThangTruoc { get; set; }

        public int TheConHieuLuc { get; set; }
        public int TheConHieuLucThangTruoc { get; set; }

        public int SapHetHan { get; set; }
        public int SapHetHanThangTruoc { get; set; }

        public decimal NoPhatChuaThanhToan { get; set; }
        public decimal NoPhatThangTruoc { get; set; }

        public int DocGiaDangMuon { get; set; }
        public int TongGiaoDichMuonTra { get; set; }
        public int SoPhieuPhatChuaThanhToan { get; set; }
    }
}