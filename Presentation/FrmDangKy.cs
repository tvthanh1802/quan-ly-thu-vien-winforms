using BusinessLayer.Services;
using Presentation.Helpers;
using System.Text.RegularExpressions;

namespace Presentation
{
    public partial class FrmDangKy : Form
    {
        private readonly TaiKhoanService _taiKhoanService = new();
        private bool _hienMatKhau = false;
        private bool _hienXacNhanMatKhau = false;

        public FrmDangKy()
        {
            InitializeComponent();

            if (DesignModeHelper.IsDesignMode(this))
                return;

            CauHinhOThongTinDangKy();
            cboLoaiTaiKhoan.Items.Add("Độc giả");
            cboLoaiTaiKhoan.SelectedIndex = 0;

            btnDangKy.Click += btnDangKy_Click;
            lnkDangNhap.Click += lnkDangNhap_Click;
            txtMatKhau.IconRightClick += txtMatKhau_IconRightClick;
            txtXacNhanMatKhau.IconRightClick += txtXacNhanMatKhau_IconRightClick;
        }

        private void CauHinhOThongTinDangKy()
        {
            txtHoTen.ImeMode = ImeMode.Disable;
            txtTenDangNhap.ImeMode = ImeMode.Disable;
            txtEmail.ImeMode = ImeMode.Disable;
            txtSoDienThoai.ImeMode = ImeMode.Disable;
            txtMatKhau.ImeMode = ImeMode.Disable;
            txtXacNhanMatKhau.ImeMode = ImeMode.Disable;

            txtHoTen.DefaultText = string.Empty;
            txtTenDangNhap.DefaultText = string.Empty;
            txtEmail.DefaultText = string.Empty;
            txtSoDienThoai.DefaultText = string.Empty;
            txtMatKhau.DefaultText = string.Empty;
            txtXacNhanMatKhau.DefaultText = string.Empty;

            txtHoTen.PlaceholderText = "Họ và tên";
            txtTenDangNhap.PlaceholderText = "Tên đăng nhập";
            txtEmail.PlaceholderText = "Email";
            txtSoDienThoai.PlaceholderText = "Số điện thoại";
            txtMatKhau.PlaceholderText = "Mật khẩu";
            txtXacNhanMatKhau.PlaceholderText = "Xác nhận mật khẩu";
        }

        private void txtMatKhau_IconRightClick(object? sender, EventArgs e)
        {
            _hienMatKhau = !_hienMatKhau;
            txtMatKhau.PasswordChar = _hienMatKhau ? '\0' : '●';
        }

        private void txtXacNhanMatKhau_IconRightClick(object? sender, EventArgs e)
        {
            _hienXacNhanMatKhau = !_hienXacNhanMatKhau;
            txtXacNhanMatKhau.PasswordChar = _hienXacNhanMatKhau ? '\0' : '●';
        }

        private void lnkDangNhap_Click(object? sender, EventArgs e)
        {
            Close();
        }

        private void btnDangKy_Click(object? sender, EventArgs e)
        {
            string hoTen = txtHoTen.Text.Trim();
            string tenDangNhap = txtTenDangNhap.Text.Trim();
            string email = txtEmail.Text.Trim();
            string soDienThoai = txtSoDienThoai.Text.Trim();
            string matKhau = txtMatKhau.Text.Trim();
            string xacNhanMatKhau = txtXacNhanMatKhau.Text.Trim();

            if (string.IsNullOrEmpty(hoTen))
            {
                HienThiThongBao("Thiếu thông tin", "Vui lòng nhập họ và tên.", Guna.UI2.WinForms.MessageDialogIcon.Warning);
                txtHoTen.Focus();
                return;
            }

            if (string.IsNullOrEmpty(tenDangNhap))
            {
                HienThiThongBao("Thiếu thông tin", "Vui lòng nhập tên đăng nhập.", Guna.UI2.WinForms.MessageDialogIcon.Warning);
                txtTenDangNhap.Focus();
                return;
            }

            if (string.IsNullOrEmpty(email))
            {
                HienThiThongBao("Thiếu thông tin", "Vui lòng nhập email.", Guna.UI2.WinForms.MessageDialogIcon.Warning);
                txtEmail.Focus();
                return;
            }

            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                HienThiThongBao("Lỗi định dạng", "Email không đúng định dạng.", Guna.UI2.WinForms.MessageDialogIcon.Warning);
                txtEmail.Focus();
                return;
            }

            if (string.IsNullOrEmpty(soDienThoai))
            {
                HienThiThongBao("Thiếu thông tin", "Vui lòng nhập số điện thoại.", Guna.UI2.WinForms.MessageDialogIcon.Warning);
                txtSoDienThoai.Focus();
                return;
            }

            if (!Regex.IsMatch(soDienThoai, @"^[0-9]{10,11}$"))
            {
                HienThiThongBao("Lỗi định dạng", "Số điện thoại phải gồm 10 hoặc 11 chữ số.", Guna.UI2.WinForms.MessageDialogIcon.Warning);
                txtSoDienThoai.Focus();
                return;
            }

            if (string.IsNullOrEmpty(matKhau))
            {
                HienThiThongBao("Thiếu thông tin", "Vui lòng nhập mật khẩu.", Guna.UI2.WinForms.MessageDialogIcon.Warning);
                txtMatKhau.Focus();
                return;
            }

            if (matKhau != xacNhanMatKhau)
            {
                HienThiThongBao("Mật khẩu không khớp", "Mật khẩu xác nhận không trùng khớp.", Guna.UI2.WinForms.MessageDialogIcon.Warning);
                txtXacNhanMatKhau.Focus();
                return;
            }

            if (!chkDongY.Checked)
            {
                HienThiThongBao("Chưa đồng ý điều khoản", "Vui lòng đồng ý với điều khoản sử dụng.", Guna.UI2.WinForms.MessageDialogIcon.Warning);
                return;
            }

            try
            {
                _taiKhoanService.DangKyTaiKhoan(hoTen, tenDangNhap, email, soDienThoai, matKhau);
                HienThiThongBao("Đăng ký thành công", "Tài khoản của bạn đã được tạo thành công.", Guna.UI2.WinForms.MessageDialogIcon.Information);
                Close();
            }
            catch (Exception ex)
            {
                HienThiThongBao("Lỗi đăng ký", ex.Message, Guna.UI2.WinForms.MessageDialogIcon.Error);
            }
        }

        private void HienThiThongBao(string caption, string text, Guna.UI2.WinForms.MessageDialogIcon icon)
        {
            msgDialog.Caption = caption;
            msgDialog.Text = text;
            msgDialog.Icon = icon;
            msgDialog.Show();
        }
    }
}
