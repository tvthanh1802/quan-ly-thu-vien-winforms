using Presentation.Controls;
using Presentation.Helpers;
using Guna.UI2.WinForms;
using System.Drawing.Drawing2D;

namespace Presentation
{
    public partial class FrmMain : Form
    {
        private UserControl? _currentControl;
        private bool _daKhoiTaoLucChay;

        private readonly Guna2Button[] _menuButtons;

        public bool DaDangXuat { get; private set; }

        public FrmMain()
        {
            InitializeComponent();

            _menuButtons =
            [
                btnTongQuan,
                btnSach,
                btnDocGia,
                btnMuonTra,
                btnNhapSach,
                btnNhanVien,
                btnTaiKhoan,
                btnThongKe,
                btnThongBao
            ];

            // Đồng bộ panelMain với kích thước ban đầu.
            CapNhatVungChuaUc();
            SetActiveSidebarButton(btnTongQuan);

            if (DesignModeHelper.IsDesignMode(this))
                return;

            Shown += FrmMain_Shown;
            Resize += FrmMain_Resize;
            ApplyPermissions();
        }

        private void SetActiveSidebarButton(Guna2Button activeButton)
        {
            foreach (Guna2Button button in _menuButtons)
            {
                button.Checked = ReferenceEquals(button, activeButton);
                button.Invalidate();
            }
        }

        private void FrmMain_Shown(object? sender, EventArgs e)
        {
            if (_daKhoiTaoLucChay)
                return;

            _daKhoiTaoLucChay = true;
            Guna2Button? first = _menuButtons.FirstOrDefault(x => x.Visible && x.Enabled);
            (first ?? btnTongQuan).PerformClick();
        }

        private void ApplyPermissions()
        {
            btnTongQuan.Visible = true;
            btnSach.Visible = PermissionHelper.CanView("SACH.DANHSACH");
            btnDocGia.Visible = PermissionHelper.CanView("DOCGIA.DANHSACH");
            btnMuonTra.Visible = PermissionHelper.CanView("MUONTRA.MUON");
            btnNhapSach.Visible = PermissionHelper.CanView("NHAPSACH.LAPPHIEU");
            btnNhanVien.Visible = PermissionHelper.CanView("HETHONG.NHANVIEN");
            btnTaiKhoan.Visible = PermissionHelper.CanView("HETHONG.TAIKHOAN");
            btnThongKe.Visible = PermissionHelper.CanView("BAOCAO.MUONTRA");
            btnThongBao.Visible = PermissionHelper.CanView("HETHONG.THONGBAO");
        }

        private bool EnsurePermission(string code)
        {
            if (PermissionHelper.CanView(code)) return true;
            MessageBox.Show("Bạn không có quyền truy cập chức năng này.", "Từ chối truy cập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        private void FrmMain_Resize(object? sender, EventArgs e)
        {
            CapNhatVungChuaUc();
        }

        private void CapNhatVungChuaUc()
        {
            if (panelMain == null || pnlSidebar == null)
                return;

            panelMain.Location = new Point(pnlSidebar.Width, 0);
            panelMain.Size = new Size(
                Math.Max(0, ClientSize.Width - pnlSidebar.Width),
                ClientSize.Height);

            if (_currentControl != null &&
                !_currentControl.IsDisposed &&
                _currentControl is not UcTongQuan)
            {
                _currentControl.Dock = DockStyle.Fill;
                _currentControl.Bounds = panelMain.ClientRectangle;
            }
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            if (DesignModeHelper.IsDesignMode(this))
                return;
        }

        /// <summary>
        /// Vẽ đường phân cách và dấu mũi tên ở mép phải giống ảnh mẫu.
        /// </summary>
        private void MenuButton_Paint(object? sender, PaintEventArgs e)
        {
            if (sender is not Guna2Button button)
                return;

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            if (!button.Checked)
            {
                using Pen linePen = new(
                    Color.FromArgb(45, 126, 137, 224),
                    1F);

                e.Graphics.DrawLine(
                    linePen,
                    57,
                    button.Height - 1,
                    button.Width - 15,
                    button.Height - 1);
            }
            else
            {
                // Vệt sáng nhỏ trên nút active.
                using Pen activePen = new(
                    Color.FromArgb(120, 211, 195, 255),
                    1.2F);

                e.Graphics.DrawLine(activePen, 58, 2, button.Width - 40, 2);
            }

            int centerY = button.Height / 2;
            int arrowX = button.Width - 25;

            using Pen arrowPen = new(
                Color.FromArgb(225, 220, 228, 255),
                2.1F)
            {
                StartCap = LineCap.Round,
                EndCap = LineCap.Round
            };

            e.Graphics.DrawLine(arrowPen, arrowX - 4, centerY - 6, arrowX + 1, centerY);
            e.Graphics.DrawLine(arrowPen, arrowX + 1, centerY, arrowX - 4, centerY + 6);
        }

        /// <summary>
        /// Tạo lại UserControl mỗi lần chuyển trang để sự kiện Load chạy lại và dữ liệu được làm mới.
        /// </summary>
        private void OpenPage(Func<UserControl> pageFactory, Control activeButton)
        {
            SuspendLayout();
            panelMain.SuspendLayout();

            try
            {
                if (_currentControl != null)
                {
                    panelMain.Controls.Remove(_currentControl);
                    _currentControl.Dispose();
                    _currentControl = null;
                }

                CapNhatVungChuaUc();

                UserControl page = pageFactory();

                // UcTongQuan sử dụng nguyên vẹn AutoScaleMode, Size và vị trí
                // được khai báo trong UcTongQuan.Designer.cs. Không ép Dock/Bounds
                // tại runtime vì sẽ làm các thẻ bị dời khỏi bố cục Designer.
                if (page is not UcTongQuan)
                {
                    page.AutoScaleMode = AutoScaleMode.None;
                    page.Dock = DockStyle.Fill;
                    page.Margin = Padding.Empty;
                    page.Padding = Padding.Empty;
                    page.MinimumSize = Size.Empty;
                    page.MaximumSize = Size.Empty;
                    page.Bounds = panelMain.ClientRectangle;
                }

                if (page is not UcTongQuan && page.Controls.Find("picAvatar", true).FirstOrDefault() is PictureBox avatar)
                {
                    CurrentUserAvatarHelper.Apply(avatar);
                }

                panelMain.Controls.Clear();
                panelMain.Controls.Add(page);

                page.BringToFront();
                page.CreateControl();
                _currentControl = page;

                if (activeButton is Guna2Button button)
                    SetActiveSidebarButton(button);
            }
            finally
            {
                panelMain.ResumeLayout(true);
                ResumeLayout(true);
            }
        }

        public void OpenControl(UserControl control)
        {
            OpenPage(() => control, control);
        }

        private void btnTongQuan_Click(object sender, EventArgs e)
        {
            OpenPage(() => new Controls.UcTongQuan(), btnTongQuan);
        }

        private void btnSach_Click(object sender, EventArgs e)
        {
            if (!EnsurePermission("SACH.DANHSACH")) return;
            MoTrangQuanLySach();
        }

        private void MoTrangQuanLySach()
        {
            var uc = new Controls.UcQuanLySach();
            OpenPage(() => uc, btnSach);

            BeginInvoke(new Action(() =>
            {
                if (!uc.IsDisposed)
                    uc.TuDongBamLamMoi();
            }));
        }

        private void FrmMain_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt && e.KeyCode == Keys.S && EnsurePermission("SACH.DANHSACH"))
            {
                MoTrangQuanLySach();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void btnDocGia_Click(object? sender, EventArgs e)
        {
            if (!EnsurePermission("DOCGIA.DANHSACH")) return;
            var uc = new Controls.UcQuanLyDocGia();
            OpenPage(() => uc, btnDocGia);

            BeginInvoke(new Action(() =>
            {
                try
                {
                    if (!uc.IsDisposed)
                        uc.ReloadData();
                }
                catch
                {
                    // Giữ giao diện hoạt động nếu thao tác làm mới dữ liệu thất bại.
                }
            }));
        }

        private void btnMuonTra_Click(object sender, EventArgs e)
        {
            if (!EnsurePermission("MUONTRA.MUON")) return;
            OpenPage(() => new Controls.UcMuonTraPhat(), btnMuonTra);
        }

        private void btnNhapSach_Click(object? sender, EventArgs e)
        {
            if (!EnsurePermission("NHAPSACH.LAPPHIEU")) return;
            OpenPage(() => new Controls.UcNhapSach(), btnNhapSach);
        }

        private void btnThongKe_Click(object sender, EventArgs e)
        {
            if (!EnsurePermission("BAOCAO.MUONTRA")) return;
            OpenPage(() => new Controls.UcThongKeBaoCao(), btnThongKe);
        }

        private void btnNhanVien_Click(object sender, EventArgs e)
        {
            if (!EnsurePermission("HETHONG.NHANVIEN")) return;
            OpenPage(() => new Controls.UcNhanVien(), btnNhanVien);
        }

        private void btnTaiKhoan_Click(object sender, EventArgs e)
        {
            if (!EnsurePermission("HETHONG.TAIKHOAN")) return;
            OpenPage(() => new Controls.UcTaiKhoanPhanQuyen(), btnTaiKhoan);
        }

        private void btnThongBao_Click(object? sender, EventArgs e)
        {
            if (!EnsurePermission("HETHONG.THONGBAO")) return;
            OpenPage(() => new Controls.UcThongBao(), btnThongBao);
        }

        private void btnDangXuat_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Bạn có chắc chắn muốn đăng xuất khỏi hệ thống?",
                "Xác nhận đăng xuất",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
                return;

            CurrentUser.Clear();
            panelMain.Controls.Clear();
            _currentControl?.Dispose();
            _currentControl = null;
            DaDangXuat = true;
            Close();
        }

        private static void OpenPlaceholder(string name)
        {
            MessageBox.Show(
                $"Chức năng '{name}' chưa được triển khai trong bản này.",
                "Thông báo",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
    }
}
