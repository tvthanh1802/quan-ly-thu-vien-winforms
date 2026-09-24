using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DataLayer.Models
{
    public class ThongBaoDashboardModel
    {
        public int MaThongBao { get; set; }

        public int MaLoaiThongBao { get; set; }

        public string TenLoai { get; set; } = string.Empty;

        public string TieuDe { get; set; } = string.Empty;

        public string NoiDung { get; set; } = string.Empty;

        public DateTime? NgayGui { get; set; }

        public bool DaDoc { get; set; }

        public int? MaNhanVien { get; set; }

        public int? MaDocGia { get; set; }

        public string DoiTuongNhan { get; set; } = "Toàn hệ thống";

        // true khi thông báo không chỉ định độc giả/nhân viên cụ thể.
        public bool LaThongBaoChung { get; set; }

        public string Icon { get; set; } = "Bell";

        public string Mau { get; set; } = "#6432EB";
    }
}
