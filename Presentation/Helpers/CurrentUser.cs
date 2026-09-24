using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Presentation.Helpers
{
    public static class CurrentUser
    {
        public static int MaTaiKhoan { get; set; }

        public static string TenDangNhap { get; set; } = "";

        public static string HoTen { get; set; } = "";

        public static string VaiTro { get; set; } = "";

        public static int MaVaiTro { get; set; }

        public static int? MaNhanVien { get; set; }

        public static int? MaDocGia { get; set; }

        public static string? AnhDaiDien { get; set; }

        public static bool LaTaiKhoanDocGia => MaDocGia.HasValue;

        public static bool IsLogin => MaTaiKhoan > 0;

        public static void Clear()
        {
            MaTaiKhoan = 0;
            TenDangNhap = "";
            HoTen = "";
            VaiTro = "";
            MaVaiTro = 0;
            MaNhanVien = null;
            MaDocGia = null;
            AnhDaiDien = null;
            PermissionHelper.Clear();
        }
    }
}
