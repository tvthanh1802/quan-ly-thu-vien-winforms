using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    public class TopDanhMucThongKeModel
    {
        public int MaDanhMuc { get; set; }

        public string TenDanhMuc { get; set; } = string.Empty;

        public int TongSach { get; set; }

        public int CoSan { get; set; }

        public int DangMuon { get; set; }

        public int QuaHan { get; set; }

        public double TyLe { get; set; }

        public string TyLeText =>
            $"{TyLe:0.00}%";
    }
}
