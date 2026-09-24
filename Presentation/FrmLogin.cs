using BusinessLayer.Services;
using DataLayer.Entities;
using DataLayer.Models;
using Presentation.Helpers;

namespace Presentation
{
    public partial class FrmLogin : Form
    {
        private const string UsernamePlaceholder = "Tên đăng nhập";
        private const string PasswordPlaceholder = "Mật khẩu";
        private TaiKhoanService? _taiKhoanService;

        public FrmLogin()
        {
            InitializeComponent();

            if (DesignModeHelper.IsDesignMode(this))
                return;

            _taiKhoanService = new TaiKhoanService();
            CauHinhOThongTinDangNhap();
            btnLogin.Click += btnLogin_Click;
            btnReset.Click += btnReset_Click;
            btnExit.Click += btnExit_Click;
            txtPassword.IconRightClick += txtPassword_IconRightClick;
            linkLabel2.Click += linkLabel2_Click;
        }

        private void linkLabel2_Click(object? sender, EventArgs e)
        {
            using FrmDangKy frm = new FrmDangKy();
            Hide();
            frm.ShowDialog();
            Show();
            txtUsername.Focus();
        }


        private void CauHinhOThongTinDangNhap()
        {
            // Tắt IME để tránh Guna2TextBox giữ lại lớp placeholder khi nhập.
            txtUsername.ImeMode = ImeMode.Disable;
            txtPassword.ImeMode = ImeMode.Disable;

            txtUsername.DefaultText = string.Empty;
            txtPassword.DefaultText = string.Empty;
            txtUsername.PlaceholderText = UsernamePlaceholder;
            txtPassword.PlaceholderText = PasswordPlaceholder;

            txtUsername.Enter += (_, _) => txtUsername.PlaceholderText = string.Empty;
            txtUsername.Leave += (_, _) =>
                txtUsername.PlaceholderText = string.IsNullOrWhiteSpace(txtUsername.Text)
                    ? UsernamePlaceholder
                    : string.Empty;
            txtUsername.TextChanged += (_, _) =>
            {
                txtUsername.PlaceholderText = string.IsNullOrEmpty(txtUsername.Text) && !txtUsername.Focused
                    ? UsernamePlaceholder
                    : string.Empty;
                txtUsername.Invalidate();
            };

            txtPassword.Enter += (_, _) => txtPassword.PlaceholderText = string.Empty;
            txtPassword.Leave += (_, _) =>
                txtPassword.PlaceholderText = string.IsNullOrWhiteSpace(txtPassword.Text)
                    ? PasswordPlaceholder
                    : string.Empty;
            txtPassword.TextChanged += (_, _) =>
            {
                txtPassword.PlaceholderText = string.IsNullOrEmpty(txtPassword.Text) && !txtPassword.Focused
                    ? PasswordPlaceholder
                    : string.Empty;
                txtPassword.Invalidate();
            };
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string usernameRaw = txtUsername.Text;
            string passwordRaw = txtPassword.Text;

            // TC04: Bỏ trống cả Tên DN và MK
            if (string.IsNullOrEmpty(usernameRaw) && string.IsNullOrEmpty(passwordRaw))
            {
                msgDialog.Caption = "Thiếu thông tin";
                msgDialog.Text = "Tên đăng nhập và mật khẩu không được để trống.";
                msgDialog.Icon = Guna.UI2.WinForms.MessageDialogIcon.Warning;
                msgDialog.Show();
                txtUsername.Focus();
                return;
            }

            // TC02: Bỏ trống Tên đăng nhập
            if (string.IsNullOrEmpty(usernameRaw))
            {
                msgDialog.Caption = "Thiếu thông tin";
                msgDialog.Text = "Tên đăng nhập không được để trống.";
                msgDialog.Icon = Guna.UI2.WinForms.MessageDialogIcon.Warning;
                msgDialog.Show();
                txtUsername.Focus();
                return;
            }

            // TC03: Bỏ trống Mật khẩu
            if (string.IsNullOrEmpty(passwordRaw))
            {
                msgDialog.Caption = "Thiếu thông tin";
                msgDialog.Text = "Mật khẩu không được để trống.";
                msgDialog.Icon = Guna.UI2.WinForms.MessageDialogIcon.Warning;
                msgDialog.Show();
                txtPassword.Focus();
                return;
            }

            // TC05: Tên DN có khoảng trắng
            if (usernameRaw.Contains(" "))
            {
                msgDialog.Caption = "Định dạng không hợp lệ";
                msgDialog.Text = "Tên đăng nhập không được chứa khoảng trắng.";
                msgDialog.Icon = Guna.UI2.WinForms.MessageDialogIcon.Warning;
                msgDialog.Show();
                txtUsername.Focus();
                return;
            }

            // TC06: Mật khẩu quá ngắn (<6)
            if (passwordRaw.Length < 6)
            {
                msgDialog.Caption = "Định dạng không hợp lệ";
                msgDialog.Text = "Mật khẩu phải từ 6 ký tự trở lên.";
                msgDialog.Icon = Guna.UI2.WinForms.MessageDialogIcon.Warning;
                msgDialog.Show();
                txtPassword.Focus();
                return;
            }

            KetQuaDangNhap ketQua;
            try
            {
                ketQua = _taiKhoanService!.DangNhapChiTiet(usernameRaw, passwordRaw);
            }
            catch (Exception ex)
            {
                msgDialog.Caption = "Không thể đăng nhập";
                msgDialog.Text = "Không thể kết nối hoặc xác thực dữ liệu.\n" + ex.Message;
                msgDialog.Icon = Guna.UI2.WinForms.MessageDialogIcon.Error;
                msgDialog.Show();
                return;
            }

            if (ketQua.TrangThai == TrangThaiDangNhap.TaiKhoanKhongTonTai)
            {
                msgDialog.Caption = "Đăng nhập thất bại";
                msgDialog.Text = "Tài khoản không tồn tại.";
                msgDialog.Icon = Guna.UI2.WinForms.MessageDialogIcon.Error;
                msgDialog.Show();
                txtUsername.Focus();
                return;
            }

            if (ketQua.TrangThai == TrangThaiDangNhap.TaiKhoanBiVoHieuHoa)
            {
                msgDialog.Caption = "Tài khoản bị khóa";
                msgDialog.Text = "Tài khoản của bạn đã bị khóa bởi quản trị viên.";
                msgDialog.Icon = Guna.UI2.WinForms.MessageDialogIcon.Error;
                msgDialog.Show();
                return;
            }

            if (ketQua.TrangThai == TrangThaiDangNhap.TamThoiBiKhoa)
            {
                int minutes = 15;
                if (ketQua.KhoaDen.HasValue)
                {
                    var diff = ketQua.KhoaDen.Value - DateTime.Now;
                    minutes = (int)Math.Ceiling(diff.TotalMinutes);
                    if (minutes < 1) minutes = 1;
                }
                msgDialog.Caption = "Tài khoản bị khóa";
                msgDialog.Text = $"Tài khoản bị khóa tạm thời. Vui lòng thử lại sau {minutes} phút.";
                msgDialog.Icon = Guna.UI2.WinForms.MessageDialogIcon.Error;
                msgDialog.Show();
                return;
            }

            if (ketQua.TrangThai == TrangThaiDangNhap.MatKhauKhongDung)
            {
                msgDialog.Caption = "Đăng nhập thất bại";
                msgDialog.Text = "Tên đăng nhập hoặc mật khẩu không đúng.";
                msgDialog.Icon = Guna.UI2.WinForms.MessageDialogIcon.Error;
                msgDialog.Show();

                txtPassword.Clear();
                txtPassword.Focus();
                return;
            }

            // Đăng nhập thành công
            TaiKhoan taiKhoan = ketQua.TaiKhoan!;
            msgDialog.Caption = "Đăng nhập thành công";
            msgDialog.Text = "Chào mừng bạn quay lại hệ thống thư viện EPU.";
            msgDialog.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
            msgDialog.Show();

            CurrentUser.MaTaiKhoan = taiKhoan.MaTaiKhoan;
            CurrentUser.TenDangNhap = taiKhoan.TenDangNhap;
            CurrentUser.MaVaiTro = taiKhoan.MaVaiTro;
            CurrentUser.VaiTro = taiKhoan.MaVaiTroNavigation?.TenVaiTro ?? "";
            CurrentUser.MaNhanVien = taiKhoan.MaNhanVien;
            CurrentUser.MaDocGia = taiKhoan.MaDocGia;

            CurrentUser.HoTen =
                taiKhoan.MaNhanVienNavigation?.HoTen
                ?? taiKhoan.MaDocGiaNavigation?.HoTen
                ?? taiKhoan.TenDangNhap;

            CurrentUser.AnhDaiDien =
                taiKhoan.MaDocGiaNavigation?.AnhDaiDien
                ?? taiKhoan.MaNhanVienNavigation?.AnhDaiDien;

            try
            {
                PermissionHelper.Load(CurrentUser.MaVaiTro);
            }
            catch (Exception ex)
            {
                CurrentUser.Clear();
                msgDialog.Caption = "Không thể tải phân quyền";
                msgDialog.Text = "Vui lòng liên hệ quản trị viên.\n" + ex.Message;
                msgDialog.Icon = Guna.UI2.WinForms.MessageDialogIcon.Error;
                msgDialog.Show();
                return;
            }

            using FrmMain frm = new FrmMain();
            Hide();
            frm.ShowDialog();

            if (!frm.DaDangXuat)
            {
                Close();
                return;
            }

            txtPassword.Clear();
            txtUsername.Clear();
            hienMatKhau = false;
            txtPassword.PasswordChar = '●';
            Show();
            Activate();
            txtUsername.Focus();
        }

        private void btnReset_Click(object? sender, EventArgs e)
        {
            txtUsername.Clear();
            txtPassword.Clear();
            hienMatKhau = false;
            txtPassword.PasswordChar = '●';
            txtUsername.Focus();
        }

        private void btnExit_Click(object? sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Bạn có chắc chắn muốn thoát ứng dụng?",
                "Xác nhận thoát",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private bool hienMatKhau = false;

        private void txtPassword_IconRightClick(object? sender, EventArgs e)
        {
            hienMatKhau = !hienMatKhau;

            txtPassword.PasswordChar = hienMatKhau ? '\0' : '●';
        }
    }
}
