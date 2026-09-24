using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    public class TopDanhMucModel
    {
        public int MaTheLoai { get; set; }

        public string TenTheLoai { get; set; } = string.Empty;

        public int SoLuongSach { get; set; }

        public double TyLePhanTram { get; set; }
    }
}
