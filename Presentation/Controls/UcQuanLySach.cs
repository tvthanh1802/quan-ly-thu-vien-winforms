using BusinessLayer.DTOs;
using BusinessLayer.Services;
using DataLayer.Models;
using Presentation.Helpers;
using System.Globalization;
using System.ComponentModel;
using System.Text;
using System.Diagnostics;

namespace Presentation.Controls
{
    public partial class UcQuanLySach : UserControl
    {
        private SachService _sachService = null!;
        private readonly System.Windows.Forms.Timer _searchTimer = new();
        private readonly HashSet<int> _maSachDaChon = new();
        private int _trangHienTai = 1;
        private int _soDongMoiTrang = 6;
        private int _tongBanGhi;
        private int _tongSoTrang = 1;
        private IReadOnlyList<SachListModel> _duLieuHienTai = Array.Empty<SachListModel>();

        private bool _daKhoiTaoDuLieu;
        private bool _dangTaiDuLieu;

        public UcQuanLySach()
        {
            InitializeComponent();

            // Avoid running runtime-only initialization in Visual Studio Designer
            if (IsInDesignMode())
                return;

            _sachService = new SachService();
            CauHinhBangSach();
            GanSuKien();
            ApDungPhanQuyen();

            _searchTimer.Interval = 450;
            _searchTimer.Tick += (_, _) =>
            {
                _searchTimer.Stop();
                _trangHienTai = 1;
                TaiDanhSachSach();
            };
        }

        private bool IsInDesignMode()
        {
            return DesignModeHelper.IsDesignMode(this);
        }

        private void GanSuKien()
        {
            Load += UcQuanLySach_Load;
            Disposed += (_, _) => _searchTimer.Dispose();

            txtTimKiemSach.TextChanged += (_, _) =>
            {
                _searchTimer.Stop();
                _searchTimer.Start();
            };

            cboTheLoai.SelectionChangeCommitted += BoLoc_Changed;
            cboNhaXuatBan.SelectionChangeCommitted += BoLoc_Changed;
            cboTrangThai.SelectionChangeCommitted += BoLoc_Changed;

            btnLamMoi.Click += btnLamMoi_Click;
            btnChonNhieu.Click += btnChonNhieu_Click;
            btnThemSach.Click += btnThemSach_Click;
            btnNhapExcel.Click += btnNhapExcel_Click;
            btnXuatExcel.Click += btnXuatExcel_Click;
            // pagination events (controls are defined in Designer)
            btnTrangDau.Click += btnTrangDau_Click;
            btnTrangTruoc.Click += btnTrangTruoc_Click;
            btnTrangSau.Click += btnTrangSau_Click;
            btnTrangCuoi.Click -= btnNext_Click;
            btnTrangCuoi.Click += btnTrangCuoi_Click;
            btnGo.Click += btnGo_Click;
            txtTrang.KeyDown += txtTrang_KeyDown;
            txtTrang.KeyPress += txtTrang_KeyPress;
            btnPage1.Click += btnPage_Click;
            btnPage2.Click += btnPage_Click;
            btnPage3.Click += btnPage_Click;
            btnPage4.Click += btnPage_Click;
            btnLastPage.Click += btnPage_Click;

            dgvSach.CurrentCellDirtyStateChanged += (_, _) =>
            {
                if (dgvSach.IsCurrentCellDirty)
                    dgvSach.CommitEdit(DataGridViewDataErrorContexts.Commit);
            };
            dgvSach.CellValueChanged += dgvSach_CellValueChanged;
            dgvSach.ColumnHeaderMouseClick += dgvSach_ColumnHeaderMouseClick;
        }

        private void ApDungPhanQuyen()
        {
            btnThemSach.Enabled = PermissionHelper.CanAdd("SACH.DANHSACH");
            btnNhapExcel.Enabled = PermissionHelper.CanAdd("SACH.DANHSACH");
            btnChonNhieu.Enabled = PermissionHelper.CanDelete("SACH.DANHSACH");
            btnXuatExcel.Enabled = PermissionHelper.CanExport("SACH.DANHSACH");
            colSua.Visible = PermissionHelper.CanEdit("SACH.DANHSACH");
            colXoa.Visible = PermissionHelper.CanDelete("SACH.DANHSACH");
        }

        private static bool KiemTraQuyen(bool allowed, string message)
        {
            if (allowed) return true;
            MessageBox.Show(message, "Không đủ quyền", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        private void UcQuanLySach_Load(object? sender, EventArgs e)
        {
            // Dữ liệu được kích hoạt từ btnSach_Click của FrmMain bằng
            // TuDongBamLamMoi(), tránh sự kiện Load chạy không ổn định hoặc tải hai lần.
        }

        /// <summary>
        /// FrmMain gọi phương thức này ngay sau khi người dùng bấm nút Quản lý sách.
        /// Phương thức khởi tạo bộ lọc trước, sau đó gọi PerformClick() đúng một lần
        /// trên nút Làm mới, giống hệt người dùng tự bấm nút.
        /// </summary>
        public void TuDongBamLamMoi()
        {
            if (IsInDesignMode() || IsDisposed || _dangTaiDuLieu)
                return;

            if (InvokeRequired)
            {
                BeginInvoke(new Action(TuDongBamLamMoi));
                return;
            }

            try
            {
                _dangTaiDuLieu = true;

                if (!_daKhoiTaoDuLieu)
                {
                    LoadCurrentUser();
                    KhoiTaoBoLoc();
                    _daKhoiTaoDuLieu = true;
                }
            }
            catch (Exception ex)
            {
                HienLoi("Không thể khởi tạo chức năng quản lý sách.", ex);
                return;
            }
            finally
            {
                _dangTaiDuLieu = false;
            }

            // Thực hiện đúng một lần hành vi của nút Làm mới.
            btnLamMoi.PerformClick();
        }

        /// <summary>
        /// Được FrmMain gọi mỗi khi trang Quản lý sách được mở.
        /// Lần đầu sẽ nạp người dùng, bộ lọc và phân trang; các lần sau chỉ tải lại dữ liệu.
        /// </summary>
        public void RefreshData()
        {
            if (IsInDesignMode() || _dangTaiDuLieu || IsDisposed)
                return;

            try
            {
                _dangTaiDuLieu = true;

                if (!_daKhoiTaoDuLieu)
                {
                    LoadCurrentUser();
                    KhoiTaoBoLoc();
                    _daKhoiTaoDuLieu = true;
                }

                // Luôn về trang đầu khi mở chức năng để chắc chắn có dữ liệu hiển thị.
                _trangHienTai = 1;
                TaiLaiToanBo();
            }
            catch (Exception ex)
            {
                HienLoi("Không thể tải chức năng quản lý sách.", ex);
            }
            finally
            {
                _dangTaiDuLieu = false;
            }
        }

        private void KhoiTaoBoLoc()
        {
            NapCombo(cboTheLoai, _sachService.LayTheLoai(), "-- Tất cả thể loại --");
            NapCombo(cboNhaXuatBan, _sachService.LayNhaXuatBan(), "-- Tất cả NXB --");

            cboTrangThai.DataSource = new[]
            {
                new ComboItem(null, "-- Tất cả trạng thái --"),
                new ComboItem("Còn sẵn", "Còn sẵn"),
                new ComboItem("Đang mượn", "Đang mượn"),
                new ComboItem("Hết sách", "Hết sách")
            };
            cboTrangThai.DisplayMember = nameof(ComboItem.Text);
            cboTrangThai.ValueMember = nameof(ComboItem.Value);

            LamDepComboBox(cboTheLoai);
            LamDepComboBox(cboNhaXuatBan);
            LamDepComboBox(cboTrangThai);

        }


        private static void LamDepComboBox(Guna.UI2.WinForms.Guna2ComboBox combo)
        {
            combo.FocusedColor = Color.FromArgb(210, 218, 235);
            combo.FocusedState.BorderColor = Color.FromArgb(210, 218, 235);

            // Guna thay đổi tên một số thuộc tính ItemsAppearance giữa các phiên bản,
            // dùng reflection để tránh lỗi biên dịch nhưng vẫn bỏ nền xanh khi mở danh sách.
            object? appearance = combo.GetType().GetProperty("ItemsAppearance")?.GetValue(combo);
            if (appearance == null) return;

            void SetColor(string ten, Color mau)
            {
                var property = appearance.GetType().GetProperty(ten);
                if (property?.CanWrite == true) property.SetValue(appearance, mau);
            }

            SetColor("BackColor", Color.White);
            SetColor("ForeColor", Color.FromArgb(35, 48, 90));
            SetColor("SelectedBackColor", Color.FromArgb(242, 246, 255));
            SetColor("SelectedForeColor", Color.FromArgb(35, 48, 90));
        }


        private void btnTrangDau_Click(object? sender, EventArgs e)
        {
            if (_trangHienTai == 1) return;
            _trangHienTai = 1;
            TaiDanhSachSach();
        }

        private void btnTrangTruoc_Click(object? sender, EventArgs e)
        {
            if (_trangHienTai <= 1) return;
            _trangHienTai--;
            TaiDanhSachSach();
        }

        private void btnTrangSau_Click(object? sender, EventArgs e)
        {
            if (_trangHienTai >= _tongSoTrang) return;
            _trangHienTai++;
            TaiDanhSachSach();
        }

        private void btnTrangCuoi_Click(object? sender, EventArgs e)
        {
            if (_trangHienTai == _tongSoTrang) return;
            _trangHienTai = _tongSoTrang;
            TaiDanhSachSach();
        }

        private void btnGo_Click(object? sender, EventArgs e) => ChuyenDenTrangNhap();

        private void txtTrang_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            ChuyenDenTrangNhap();
            e.SuppressKeyPress = true;
            e.Handled = true;
        }

        private static void txtTrang_KeyPress(object? sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        private void ChuyenDenTrangNhap()
        {
            if (!int.TryParse(txtTrang.Text.Trim(), out int trang))
            {
                MessageBox.Show("Vui lòng nhập số trang hợp lệ.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTrang.Focus();
                txtTrang.SelectAll();
                return;
            }

            _trangHienTai = Math.Clamp(trang, 1, _tongSoTrang);
            TaiDanhSachSach();
        }

        private void btnPage_Click(object? sender, EventArgs e)
        {
            if (sender is not Guna.UI2.WinForms.Guna2Button button) return;
            if (button.Tag == null) return;
            int page = Convert.ToInt32(button.Tag);
            if (page < 1 || page > _tongSoTrang || page == _trangHienTai) return;
            _trangHienTai = page;
            TaiDanhSachSach();
        }

        private void CreatePaginationControls()
        {
            // minimal creation: add to pnlBoLoc
            // place controls at right side of pnlBoLoc

            //cboPageSize = new ComboBox
            //{
            //    Location = new Point(1200, 0),
            //    Size = new Size(120, 30)
            //};

            btnTrangDau = new Guna.UI2.WinForms.Guna2Button { Location = new Point(1330, 0), Size = new Size(36, 30), Text = "‹" };
            btnPage1 = new Guna.UI2.WinForms.Guna2Button { Location = new Point(1372, 0), Size = new Size(36, 30) };
            btnPage2 = new Guna.UI2.WinForms.Guna2Button { Location = new Point(1410, 0), Size = new Size(36, 30) };
            btnPage3 = new Guna.UI2.WinForms.Guna2Button { Location = new Point(1448, 0), Size = new Size(36, 30) };
            btnPage4 = new Guna.UI2.WinForms.Guna2Button { Location = new Point(1486, 0), Size = new Size(36, 30) };
            lblDots = new Label { Location = new Point(1564, 6), Text = "...", AutoSize = true };
            btnLastPage = new Guna.UI2.WinForms.Guna2Button { Location = new Point(1588, 0), Size = new Size(56, 30) };
            btnTrangCuoi = new Guna.UI2.WinForms.Guna2Button { Location = new Point(1648, 0), Size = new Size(36, 30), Text = "›" };
            lblPageInfo = new Label { Location = new Point(1150, 36), Size = new Size(540, 30) };

            // add to pnlBoLoc after it's created (we'll add in Load)
        }

        private static void NapCombo(ComboBox combo, IReadOnlyList<LookupItemModel> data, string allText)
        {
            var list = new List<ComboItem> { new(null, allText) };
            list.AddRange(data.Select(x => new ComboItem(x.Id, x.Ten)));
            combo.DataSource = list;
            combo.DisplayMember = nameof(ComboItem.Text);
            combo.ValueMember = nameof(ComboItem.Value);
            combo.SelectedIndex = 0;
        }

        private void TaiLaiToanBo()
        {
            LoadThongKeSach();
            TaiDanhSachSach();
        }

        private void TaiDanhSachSach()
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                var filter = new SachFilterDto
                {
                    TuKhoa = txtTimKiemSach.Text.Trim(),
                    MaTheLoai = LayGiaTriInt(cboTheLoai),
                    MaNhaXuatBan = LayGiaTriInt(cboNhaXuatBan),
                    TrangThai = cboTrangThai.SelectedValue?.ToString(),
                    Trang = _trangHienTai,
                    SoDongMoiTrang = _soDongMoiTrang,
                    SapXep = "MaSachDesc"
                };

                PagedResult<SachListModel> result = _sachService.LayDanhSach(filter);
                _trangHienTai = result.TrangHienTai;
                _soDongMoiTrang = Math.Max(1, result.SoDongMoiTrang);
                _tongBanGhi = result.TongBanGhi;
                _tongSoTrang = Math.Max(1, (int)Math.Ceiling(_tongBanGhi / (double)_soDongMoiTrang));
                _duLieuHienTai = result.DuLieu ?? Array.Empty<SachListModel>();

                dgvSach.Rows.Clear();
                int stt = (_trangHienTai - 1) * _soDongMoiTrang + 1;

                foreach (SachListModel sach in _duLieuHienTai)
                {
                    int rowIndex = dgvSach.Rows.Add();
                    DataGridViewRow row = dgvSach.Rows[rowIndex];
                    row.Tag = sach.MaSach;

                    row.Cells["colChon"].Value = _maSachDaChon.Contains(sach.MaSach);
                    row.Cells["colSTT"].Value = stt++;
                    row.Cells["colAnhBia"].Value = BookCoverImageHelper.LoadForGrid(sach.AnhBia, sach.MaSach, ChieuRongAnhBia, ChieuCaoAnhBia);
                    row.Cells["colMaSach"].Value = sach.MaSachHienThi;
                    row.Cells["colTenSach"].Value = sach.TenSach;
                    row.Cells["colTacGia"].Value = sach.TacGia;
                    row.Cells["colTheLoai"].Value = sach.TenTheLoai;
                    row.Cells["colNhaXuatBan"].Value = sach.TenNhaXuatBan;
                    row.Cells["colNamXuatBan"].Value = sach.NamXuatBan?.ToString() ?? string.Empty;
                    row.Cells["colSoLuong"].Value = sach.SoLuong;
                    row.Cells["colDangMuon"].Value = sach.DangMuon;
                    row.Cells["colConLai"].Value = sach.ConLai;
                    row.Cells["colTrangThai"].Value = sach.TrangThai;
                    row.Cells["colViTri"].Value = sach.ViTri;
                    row.Cells["colXem"].Value = Properties.Resources.eye;
                    row.Cells["colSua"].Value = Properties.Resources.edit;
                    row.Cells["colXoa"].Value = Properties.Resources.delete;
                }

                CapNhatThongTinPhanTrang();
                CapNhatCheDoChonNhieu();
                dgvSach.ClearSelection();
            }
            catch (Exception ex)
            {
                HienLoi("Không tải được danh sách sách. Hãy kiểm tra chuỗi kết nối và dữ liệu trong SQL Server.", ex);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void CapNhatThongTinPhanTrang()
        {
            int from = _tongBanGhi == 0 ? 0 : (_trangHienTai - 1) * _soDongMoiTrang + 1;
            int to = Math.Min(_trangHienTai * _soDongMoiTrang, _tongBanGhi);
            string thongTinTrang = $"Hiển thị {from:N0}–{to:N0} của {_tongBanGhi:N0} đầu sách";
            lblGoiYSapXep.Text = thongTinTrang;

            if (lblPageInfo != null)
            {
                lblPageInfo.Text = thongTinTrang;
            }

            bool coTrangTruoc = _trangHienTai > 1;
            bool coTrangSau = _trangHienTai < _tongSoTrang;
            btnTrangDau.Enabled = coTrangTruoc;
            btnTrangTruoc.Enabled = coTrangTruoc;
            btnTrangSau.Enabled = coTrangSau;
            btnTrangCuoi.Enabled = coTrangSau;

            txtTrang.Text = _trangHienTai.ToString();
            lblTongTrang.Text = $"/ {_tongSoTrang:N0}";
            btnGo.Enabled = _tongBanGhi > 0;

            HienThiCacNutTrang();
        }

        private void HienThiCacNutTrang()
        {
            var buttons = new[] { btnPage1, btnPage2, btnPage3, btnPage4 };

            int startPage;
            if (_tongSoTrang <= 5)
                startPage = 1;
            else if (_trangHienTai <= 3)
                startPage = 1;
            else if (_trangHienTai >= _tongSoTrang - 2)
                startPage = _tongSoTrang - 4;
            else
                startPage = _trangHienTai - 2;

            for (int i = 0; i < buttons.Length; i++)
            {
                int pageNumber = startPage + i;
                var button = buttons[i];
                if (pageNumber <= _tongSoTrang)
                {
                    button.Visible = true;
                    button.Text = pageNumber.ToString();
                    button.Tag = pageNumber;
                    button.Size = new Size(54, 34);
                    button.TextOffset = Point.Empty;
                    DoiMauNutTrang(button, pageNumber == _trangHienTai);
                }
                else
                {
                    button.Visible = false;
                }
            }

            int pageAfterFive = startPage + buttons.Length - 1;
            lblDots.Visible = _tongSoTrang > pageAfterFive + 1;
            btnLastPage.Visible = _tongSoTrang > pageAfterFive;
            btnLastPage.Text = _tongSoTrang.ToString();
            btnLastPage.Tag = _tongSoTrang;
            btnLastPage.Size = new Size(58, 34);
            btnLastPage.TextOffset = Point.Empty;
            DoiMauNutTrang(btnLastPage, _trangHienTai == _tongSoTrang);

            if (_tongSoTrang <= 5)
            {
                lblDots.Visible = false;
                btnLastPage.Visible = false;
            }
        }

        private void DoiMauNutTrang(Guna.UI2.WinForms.Guna2Button button, bool selected)
        {
            if (button == null) return;
            if (selected)
            {
                button.FillColor = Color.FromArgb(38, 61, 214);
                button.ForeColor = Color.White;
                button.BorderThickness = 0;
                button.Font = TaoFontNutTrang(button.Text, FontStyle.Bold);
            }
            else
            {
                button.FillColor = Color.White;
                button.ForeColor = Color.FromArgb(35, 48, 90);
                button.BorderColor = Color.FromArgb(220, 226, 238);
                button.BorderThickness = 1;
                button.Font = TaoFontNutTrang(button.Text, FontStyle.Regular);
            }
        }

        private static Font TaoFontNutTrang(string? text, FontStyle style)
        {
            int length = text?.Length ?? 1;
            float size = length >= 5 ? 6.5F : length == 4 ? 7F : length == 3 ? 8F : 9F;
            return new Font("Segoe UI", size, style);
        }

        private static int? LayGiaTriInt(ComboBox combo)
        {
            if (combo.SelectedValue == null) return null;
            return int.TryParse(combo.SelectedValue.ToString(), out int value) ? value : null;
        }

        private const int ChieuRongAnhBia = 44;
        private const int ChieuCaoAnhBia = 52;

        private Image TaiAnhBiaChuan(string? anhBia, int maSach)
        {
            return BookCoverImageHelper.LoadForGrid(anhBia, maSach, ChieuRongAnhBia, ChieuCaoAnhBia);
        }

        private void LoadThongKeSach()
        {
            SachStatisticsModel tk = _sachService.LayThongKe();
            lblTongSach.Text = tk.TongCuonSach.ToString("N0");
            lblSachCoSan.Text = tk.SachCoSan.ToString("N0");
            lblSachDangMuon.Text = tk.SachDangMuon.ToString("N0");
            lblSachQuaHan.Text = tk.SachQuaHan.ToString("N0");

            SetCardDelta(lblTongSachSub, tk.TongCuonSach, tk.PrevTongCuonSach, "cuốn");
            SetCardDelta(lblSachCoSanSub, tk.SachCoSan, tk.PrevSachCoSan, "cuốn");
            SetCardDelta(lblSachDangMuonSub, tk.SachDangMuon, tk.PrevSachDangMuon, "cuốn");
            SetCardDelta(lblSachQuaHanSub, tk.SachQuaHan, tk.PrevSachQuaHan, "cuốn");

            lblSachQuaHan.ForeColor = tk.SachQuaHan > 0
                ? Color.FromArgb(230, 45, 90)
                : Color.FromArgb(0, 170, 90);
        }

        private static void SetCardDelta(
            Label label,
            int current,
            int previous,
            string unit)
        {
            int difference = current - previous;
            if (difference == 0)
            {
                label.Text = "— Không thay đổi so với tháng trước";
                MetricTrendHelper.ApplyColor(label, difference);
                return;
            }

            MetricTrendHelper.ApplyColor(label, difference);

            string arrow = MetricTrendHelper.Arrow(difference);
            string percentText = previous == 0
                ? string.Empty
                : $" ({Math.Abs(difference * 100.0 / previous):0.#}%)";

            label.Text = $"{arrow} {Math.Abs(difference):N0} {unit}{percentText} so với tháng trước";
        }

        private void BoLoc_Changed(object? sender, EventArgs e)
        {
            _trangHienTai = 1;
            _maSachDaChon.Clear();
            TaiDanhSachSach();
        }

        private void btnLamMoi_Click(object? sender, EventArgs e)
        {
            txtTimKiemSach.Clear();
            cboTheLoai.SelectedIndex = 0;
            cboNhaXuatBan.SelectedIndex = 0;
            cboTrangThai.SelectedIndex = 0;
            _trangHienTai = 1;
            _maSachDaChon.Clear();
            TaiLaiToanBo();
        }

        private void btnThemSach_Click(object? sender, EventArgs e)
        {
            if (!KiemTraQuyen(PermissionHelper.CanAdd("SACH.DANHSACH"), "Bạn không có quyền thêm sách.")) return;
            using var form = new Presentation.Models.FrmThemSach();
            if (form.ShowDialog(FindForm()) == DialogResult.OK)
                TaiLaiToanBo();
        }

        private void dgvSach_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            DataGridViewRow row = dgvSach.Rows[e.RowIndex];
            string columnName = dgvSach.Columns[e.ColumnIndex].Name;

            if (columnName == "colChon")
            {
                bool current = Convert.ToBoolean(row.Cells["colChon"].Value ?? false);
                row.Cells["colChon"].Value = !current;
                dgvSach.CommitEdit(DataGridViewDataErrorContexts.Commit);
                return;
            }

            if (row.Tag is not int maSach) return;

            switch (columnName)
            {
                case "colXem":
                    using (var form = new Presentation.Models.FrmChiTietSach(maSach))
                        form.ShowDialog(FindForm());
                    break;

                case "colSua":
                    if (!KiemTraQuyen(PermissionHelper.CanEdit("SACH.DANHSACH"), "Bạn không có quyền sửa sách.")) break;
                    using (var form = new Presentation.Models.FrmSuaSach(maSach))
                    {
                        if (form.ShowDialog(FindForm()) == DialogResult.OK)
                            TaiLaiToanBo();
                    }
                    break;

                case "colXoa":
                    if (!KiemTraQuyen(PermissionHelper.CanDelete("SACH.DANHSACH"), "Bạn không có quyền ngừng sử dụng sách.")) break;
                    NgungSuDungSach(maSach, row.Cells["colTenSach"].Value?.ToString() ?? string.Empty);
                    break;
            }
        }

        private void NgungSuDungSach(int maSach, string tenSach)
        {
            if (!KiemTraQuyen(PermissionHelper.CanDelete("SACH.DANHSACH"), "Bạn không có quyền ngừng sử dụng sách.")) return;
            DialogResult answer = MessageBox.Show(
                $"Bạn có chắc muốn ngừng sử dụng đầu sách:\n\n{tenSach}?\n\nDữ liệu mượn/trả cũ vẫn được giữ nguyên.",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (answer != DialogResult.Yes) return;

            try
            {
                if (!_sachService.NgungKinhDoanh(maSach))
                    throw new InvalidOperationException("Không tìm thấy sách cần cập nhật.");

                TaiLaiToanBo();
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                HienLoi("Không thể ngừng sử dụng sách.", ex);
            }
        }

        private void btnChonNhieu_Click(object? sender, EventArgs e)
        {
            if (!KiemTraQuyen(PermissionHelper.CanDelete("SACH.DANHSACH"), "Bạn không có quyền ngừng sử dụng nhiều sách.")) return;
            List<int> ids = _maSachDaChon
                .Where(id => id > 0)
                .Distinct()
                .ToList();

            if (ids.Count == 0)
            {
                MessageBox.Show("Vui lòng tích chọn ít nhất một đầu sách cần xóa.", "Chưa chọn sách",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show($"Ngừng sử dụng {ids.Count} đầu sách đã chọn?", "Xác nhận",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            try
            {
                int count = _sachService.NgungKinhDoanhNhieu(ids);
                _maSachDaChon.ExceptWith(ids);
                MessageBox.Show($"Đã cập nhật {count} đầu sách.", "Thành công",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                TaiLaiToanBo();
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                HienLoi("Không thể cập nhật các sách đã chọn.", ex);
            }
        }

        private void CapNhatCheDoChonNhieu()
        {
            colChon.Visible = true;
            colChon.HeaderText = dgvSach.Rows.Count > 0 && dgvSach.Rows.Cast<DataGridViewRow>()
                .All(r => Convert.ToBoolean(r.Cells["colChon"].Value ?? false)) ? "☑" : "☐";
            btnChonNhieu.Text = "Xóa sách đã chọn";
        }

        private void dgvSach_CellBeginEdit(object? sender, DataGridViewCellCancelEventArgs e)
        {
            // Checkbox luôn cho phép tích để phục vụ xuất Excel.
            // Các cột khác đã được đặt ReadOnly trong CauHinhBangSach().
        }

        private void dgvSach_CellValueChanged(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0 ||
                dgvSach.Columns[e.ColumnIndex].Name != "colChon") return;
            DataGridViewRow row = dgvSach.Rows[e.RowIndex];
            if (row.Tag is not int maSach) return;
            bool selected = Convert.ToBoolean(row.Cells["colChon"].Value ?? false);
            if (selected) _maSachDaChon.Add(maSach); else _maSachDaChon.Remove(maSach);
            CapNhatTieuDeCotChonSach();
        }

        private void dgvSach_ColumnHeaderMouseClick(object? sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex < 0 || dgvSach.Columns[e.ColumnIndex].Name != "colChon") return;
            bool selectAll = dgvSach.Rows.Cast<DataGridViewRow>()
                .Any(r => !Convert.ToBoolean(r.Cells["colChon"].Value ?? false));
            foreach (DataGridViewRow row in dgvSach.Rows)
            {
                row.Cells["colChon"].Value = selectAll;
                if (row.Tag is int id)
                {
                    if (selectAll) _maSachDaChon.Add(id); else _maSachDaChon.Remove(id);
                }
            }
            CapNhatTieuDeCotChonSach();
        }

        private void CapNhatTieuDeCotChonSach()
        {
            bool all = dgvSach.Rows.Count > 0 && dgvSach.Rows.Cast<DataGridViewRow>()
                .All(r => Convert.ToBoolean(r.Cells["colChon"].Value ?? false));
            colChon.HeaderText = all ? "☑" : "☐";
        }

        private List<SachListModel> LayToanBoDuLieuDaLoc()
        {
            var filter = new SachFilterDto
            {
                TuKhoa = txtTimKiemSach.Text.Trim(),
                MaTheLoai = LayGiaTriInt(cboTheLoai),
                MaNhaXuatBan = LayGiaTriInt(cboNhaXuatBan),
                TrangThai = cboTrangThai.SelectedValue?.ToString(),
                Trang = 1,
                SoDongMoiTrang = 200,
                SapXep = "MaSachDesc"
            };

            var result = new List<SachListModel>();
            while (true)
            {
                PagedResult<SachListModel> page = _sachService.LayDanhSach(filter);
                IReadOnlyList<SachListModel> items = page.DuLieu ?? Array.Empty<SachListModel>();
                result.AddRange(items);
                if (result.Count >= page.TongBanGhi || items.Count == 0) break;
                filter.Trang++;
            }
            return result;
        }

        private void btnXuatExcel_Click(object? sender, EventArgs e)
        {
            if (!KiemTraQuyen(PermissionHelper.CanExport("SACH.DANHSACH"), "Bạn không có quyền xuất danh sách sách.")) return;
            try
            {
                dgvSach.EndEdit();
                List<SachListModel> duLieuDaLoc = LayToanBoDuLieuDaLoc();
                List<SachListModel> duLieuXuat = _maSachDaChon.Count > 0
                    ? duLieuDaLoc.Where(x => _maSachDaChon.Contains(x.MaSach)).ToList()
                    : duLieuDaLoc;

                if (duLieuXuat.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu phù hợp để xuất.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                using var dialog = new SaveFileDialog
                {
                    Filter = "Excel Workbook (*.xlsx)|*.xlsx",
                    FileName = _maSachDaChon.Count > 0
                        ? $"SachDaChon_{DateTime.Now:yyyyMMdd_HHmm}.xlsx"
                        : $"DanhSachSachDaLoc_{DateTime.Now:yyyyMMdd_HHmm}.xlsx"
                };
                if (dialog.ShowDialog() != DialogResult.OK) return;

                string[] headers =
                {
                    "Mã sách", "Tên sách", "Tác giả", "Thể loại", "NXB", "Năm XB",
                    "ISBN", "Số lượng", "Đang mượn", "Còn lại", "Trạng thái", "Vị trí"
                };
                List<string[]> rows = duLieuXuat.Select(x => new[]
                {
                    x.MaSachHienThi, x.TenSach, x.TacGia, x.TenTheLoai, x.TenNhaXuatBan,
                    x.NamXuatBan?.ToString() ?? "", x.Isbn, x.SoLuong.ToString(),
                    x.DangMuon.ToString(), x.ConLai.ToString(), x.TrangThai, x.ViTri
                }).ToList();

                ExcelHelper.ExportToXlsx(dialog.FileName, "Danh sách sách", headers, rows);
                MessageBox.Show(_maSachDaChon.Count > 0
                    ? $"Đã xuất {duLieuXuat.Count:N0} sách được chọn."
                    : $"Đã xuất {duLieuXuat.Count:N0} sách theo bộ lọc hiện tại.",
                    "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                HienLoi("Xuất Excel thất bại.", ex);
            }
        }

        private void btnNhapExcel_Click(object? sender, EventArgs e)
        {
            if (!KiemTraQuyen(PermissionHelper.CanAdd("SACH.DANHSACH"), "Bạn không có quyền nhập danh sách sách.")) return;
            using var dialog = new OpenFileDialog { Filter = "Excel Workbook (*.xlsx)|*.xlsx" };
            if (dialog.ShowDialog() != DialogResult.OK) return;

            try
            {
                var (headers, rows) = ExcelHelper.ImportFromXlsx(dialog.FileName);
                if (headers.Length == 0)
                    throw new ArgumentException("File Excel không có hàng tiêu đề.");

                var normalizedHeaders = headers
                    .Select((header, index) => new { Name = ChuanHoaTieuDe(header), Index = index })
                    .ToList();
                if (normalizedHeaders.Any(x => string.IsNullOrWhiteSpace(x.Name)))
                    throw new ArgumentException("Tiêu đề cột không được để trống.");

                string? duplicateHeader = normalizedHeaders
                    .GroupBy(x => x.Name)
                    .FirstOrDefault(g => g.Count() > 1)?.Key;
                if (duplicateHeader != null)
                    throw new ArgumentException($"File có tiêu đề cột trùng nhau: '{duplicateHeader}'.");

                Dictionary<string, int> map = normalizedHeaders.ToDictionary(x => x.Name, x => x.Index);
                string[] requiredHeaders = { "tensach", "theloai", "tacgia" };
                if (requiredHeaders.Any(name => !map.ContainsKey(name)) ||
                    (!map.ContainsKey("nxb") && !map.ContainsKey("nhaxuatban")))
                {
                    throw new ArgumentException(
                        "File cần có các cột Tên sách, Thể loại, Nhà xuất bản (hoặc NXB) và Tác giả.");
                }

                IReadOnlyList<LookupItemModel> theLoai = _sachService.LayTheLoai();
                IReadOnlyList<LookupItemModel> nxb = _sachService.LayNhaXuatBan();
                IReadOnlyList<LookupItemModel> tacGia = _sachService.LayTacGia();
                int thanhCong = 0;
                var loi = new List<string>();

                for (int i = 0; i < rows.Count; i++)
                {
                    try
                    {
                        string[] row = rows[i];
                        if (row.All(string.IsNullOrWhiteSpace)) continue;

                        string tenSach = LayCot(row, map, "tensach");
                        string tenTheLoai = LayCot(row, map, "theloai");
                        string tenNxb = LayCot(row, map, "nxb", "nhaxuatban");
                        string authorText = LayCot(row, map, "tacgia");
                        if (string.IsNullOrWhiteSpace(tenSach) ||
                            string.IsNullOrWhiteSpace(tenTheLoai) ||
                            string.IsNullOrWhiteSpace(tenNxb) ||
                            string.IsNullOrWhiteSpace(authorText))
                        {
                            throw new ArgumentException(
                                "Thiếu Tên sách, Thể loại, Nhà xuất bản hoặc Tác giả.");
                        }

                        LookupItemModel? tl = theLoai.FirstOrDefault(x =>
                            string.Equals(x.Ten.Trim(), tenTheLoai.Trim(),
                                StringComparison.CurrentCultureIgnoreCase));
                        if (tl == null)
                            throw new ArgumentException($"Không tìm thấy thể loại '{tenTheLoai}'.");

                        LookupItemModel? publisher = nxb.FirstOrDefault(x =>
                            string.Equals(x.Ten.Trim(), tenNxb.Trim(),
                                StringComparison.CurrentCultureIgnoreCase));
                        if (publisher == null)
                            throw new ArgumentException($"Không tìm thấy nhà xuất bản '{tenNxb}'.");

                        string[] authorNames = authorText.Split(',', ';')
                            .Select(name => name.Trim())
                            .Where(name => name.Length > 0)
                            .Distinct(StringComparer.CurrentCultureIgnoreCase)
                            .ToArray();
                        var authorIds = new List<int>();
                        var missingAuthors = new List<string>();
                        foreach (string name in authorNames)
                        {
                            LookupItemModel? author = tacGia.FirstOrDefault(x =>
                                string.Equals(x.Ten.Trim(), name,
                                    StringComparison.CurrentCultureIgnoreCase));
                            if (author == null) missingAuthors.Add(name);
                            else authorIds.Add(author.Id);
                        }
                        if (missingAuthors.Count > 0)
                            throw new ArgumentException(
                                $"Không tìm thấy tác giả: {string.Join(", ", missingAuthors)}.");

                        var dto = new SachSaveDto
                        {
                            TenSach = tenSach,
                            Isbn = ChuanHoaRong(LayCot(row, map, "isbn")),
                            MaTheLoai = tl.Id,
                            MaNhaXuatBan = publisher.Id,
                            NamXuatBan = ParseNullableIntStrict(
                                LayCot(row, map, "namxb", "namxuatban"), "Năm xuất bản"),
                            NgonNgu = ChuanHoaRong(LayCot(row, map, "ngonngu")),
                            SoTrang = ParseNullableIntStrict(
                                LayCot(row, map, "sotrang"), "Số trang"),
                            GiaBia = ParseNullableDecimalStrict(
                                LayCot(row, map, "giabia"), "Giá bìa"),
                            AnhBia = ChuanHoaRong(LayCot(row, map, "anhbia")),
                            MoTa = ChuanHoaRong(LayCot(row, map, "mota")),
                            MaTacGia = authorIds.Distinct().ToList(),
                            SoLuong = 0
                        };

                        _sachService.Them(dto);
                        thanhCong++;
                    }
                    catch (Exception ex)
                    {
                        loi.Add($"Dòng {i + 2}: {LayThongBaoGoc(ex)}");
                    }
                }

                TaiLaiToanBo();
                string message = $"Đã nhập thành công {thanhCong}/{rows.Count} dòng. " +
                                 "Các đầu sách được tạo chưa có bản sao.";
                if (loi.Count > 0)
                {
                    message += "\n\n" + string.Join("\n", loi.Take(8));
                    if (loi.Count > 8) message += $"\n... và {loi.Count - 8} lỗi khác.";
                }
                MessageBox.Show(message, "Kết quả nhập Excel", MessageBoxButtons.OK,
                    loi.Count == 0 ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                HienLoi("Nhập Excel thất bại.", ex);
            }
        }

        private static string ChuanHoaTieuDe(string value)
        {
            string text = value.Trim().ToLowerInvariant().Normalize(NormalizationForm.FormD);
            return new string(text.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark && char.IsLetterOrDigit(c)).ToArray());
        }

        private static string LayCot(string[] row, Dictionary<string, int> map, params string[] names)
        {
            foreach (string name in names)
                if (map.TryGetValue(name, out int i) && i >= 0 && i < row.Length)
                    return row[i]?.Trim() ?? string.Empty;
            return string.Empty;
        }

        private static string? ChuanHoaRong(string? value)
            => string.IsNullOrWhiteSpace(value) ? null : value.Trim();

        private static int? ParseNullableIntStrict(string value, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(value)) return null;
            if (int.TryParse(value.Trim(), out int number)) return number;
            throw new ArgumentException($"{fieldName} '{value}' không phải số nguyên hợp lệ.");
        }

        private static decimal? ParseNullableDecimalStrict(string value, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(value)) return null;
            if (decimal.TryParse(value.Trim(), NumberStyles.Number, CultureInfo.CurrentCulture, out decimal number) ||
                decimal.TryParse(value.Trim(), NumberStyles.Number, CultureInfo.InvariantCulture, out number))
                return number;
            throw new ArgumentException($"{fieldName} '{value}' không phải số hợp lệ.");
        }

        private static string LayThongBaoGoc(Exception ex)
        {
            Exception root = ex;
            while (root.InnerException != null) root = root.InnerException;
            return root.Message;
        }



        private void dgvSach_CellMouseMove(object? sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 &&
                new[] { "colXem", "colSua", "colXoa" }.Contains(dgvSach.Columns[e.ColumnIndex].Name))
                dgvSach.Cursor = Cursors.Hand;
            else
                dgvSach.Cursor = Cursors.Default;
        }

        private void dgvSach_DataError(object? sender, DataGridViewDataErrorEventArgs e)
        {
            try
            {
                Debug.WriteLine($"DataGridView data error at row {e.RowIndex}, column {e.ColumnIndex}: {e.Exception?.Message}");
            }
            catch { }

            e.ThrowException = false;
        }




        private void dgvSach_RowsAdded(object? sender, DataGridViewRowsAddedEventArgs e)
        {
            // Dữ liệu và biểu tượng được gán trực tiếp trong TaiDanhSachSach().
        }

        private void CauHinhBangSach()
        {
            // Giao diện bảng lấy từ Designer; runtime chỉ mở checkbox chọn sách.
            dgvSach.ReadOnly = false;
            foreach (DataGridViewColumn column in dgvSach.Columns)
                column.ReadOnly = column != colChon;
            colChon.ReadOnly = false;
        }

        private void LoadCurrentUser()
        {
            string displayName = !string.IsNullOrWhiteSpace(CurrentUser.HoTen)
                ? CurrentUser.HoTen
                : CurrentUser.TenDangNhap;
            lblUser.Text = $"Xin chào, {displayName}";
            lblRole.Text = CurrentUser.VaiTro;
        }


        private static void HienLoi(string message, Exception ex)
        {
            Exception root = ex;
            while (root.InnerException != null) root = root.InnerException;
            MessageBox.Show($"{message}\n\nChi tiết: {root.Message}", "Lỗi",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void lblSachQuaHanSub_Click(object sender, EventArgs e)
        {

        }

        private void btnNext_Click(object? sender, EventArgs e)
        {
            // Nút » là tới trang cuối.
            if (_trangHienTai >= _tongSoTrang) return;
            _trangHienTai = _tongSoTrang;
            TaiDanhSachSach();
        }

        private sealed record ComboItem(object? Value, string Text);
    




    }

    internal sealed class FrmSachEditor : Form
    {
        private readonly SachService _service;
        private readonly int? _maSach;
        private readonly bool _readOnly;
        private readonly TextBox txtTen = new();
        private readonly TextBox txtIsbn = new();
        private readonly TextBox txtNam = new();
        private readonly TextBox txtNgonNgu = new();
        private readonly TextBox txtSoTrang = new();
        private readonly TextBox txtGia = new();
        private readonly TextBox txtAnhBia = new();
        private readonly TextBox txtMoTa = new();
        private readonly ComboBox cboTheLoai = new();
        private readonly ComboBox cboNxb = new();
        private readonly ComboBox cboViTri = new();
        private readonly CheckedListBox clbTacGia = new();
        private readonly Button btnSave = new();

        public FrmSachEditor(SachService service, int? maSach, bool readOnly)
        {
            _service = service;
            _maSach = maSach;
            _readOnly = readOnly;
            Text = readOnly ? "Chi tiết sách" : maSach.HasValue ? "Cập nhật sách" : "Thêm sách";
            StartPosition = FormStartPosition.CenterParent;
            Size = new Size(720, 650);
            MinimumSize = new Size(680, 600);
            Font = new Font("Segoe UI", 10F);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            BuildUi();
            LoadData();
        }

        private void BuildUi()
        {
            var table = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20),
                ColumnCount = 4,
                RowCount = 8,
                AutoScroll = true
            };
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));

            Add(table, "Tên sách (*)", txtTen, 0, 0, 3);
            Add(table, "ISBN", txtIsbn, 0, 1);
            Add(table, "Năm XB", txtNam, 2, 1);
            Add(table, "Thể loại (*)", cboTheLoai, 0, 2);
            Add(table, "Nhà xuất bản", cboNxb, 2, 2);
            Add(table, "Ngôn ngữ", txtNgonNgu, 0, 3);
            Add(table, "Số trang", txtSoTrang, 2, 3);
            Add(table, "Giá bìa", txtGia, 0, 4);
            Add(table, "Vị trí", cboViTri, 2, 4);
            Add(table, "Ảnh bìa", txtAnhBia, 0, 5, 3);

            table.Controls.Add(new Label { Text = "Tác giả", AutoSize = true, Anchor = AnchorStyles.Left }, 0, 6);
            clbTacGia.Height = 95;
            clbTacGia.Dock = DockStyle.Fill;
            table.Controls.Add(clbTacGia, 1, 6);
            table.SetColumnSpan(clbTacGia, 3);

            table.Controls.Add(new Label { Text = "Mô tả", AutoSize = true, Anchor = AnchorStyles.Left }, 0, 7);
            txtMoTa.Multiline = true;
            txtMoTa.Height = 90;
            txtMoTa.Dock = DockStyle.Fill;
            table.Controls.Add(txtMoTa, 1, 7);
            table.SetColumnSpan(txtMoTa, 3);

            var buttons = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 62,
                FlowDirection = FlowDirection.RightToLeft,
                Padding = new Padding(10)
            };
            var btnClose = new Button { Text = "Đóng", Width = 110, Height = 36, DialogResult = DialogResult.Cancel };
            btnSave.Text = "Lưu";
            btnSave.Width = 110;
            btnSave.Height = 36;
            btnSave.Click += Save_Click;
            buttons.Controls.Add(btnClose);
            if (!_readOnly) buttons.Controls.Add(btnSave);

            Controls.Add(table);
            Controls.Add(buttons);
            AcceptButton = _readOnly ? null : btnSave;
            CancelButton = btnClose;
        }

        private static void Add(TableLayoutPanel table, string label, Control control, int col, int row, int span = 1)
        {
            table.Controls.Add(new Label { Text = label, AutoSize = true, Anchor = AnchorStyles.Left }, col, row);
            control.Dock = DockStyle.Fill;
            if (control is ComboBox combo) combo.DropDownStyle = ComboBoxStyle.DropDownList;
            table.Controls.Add(control, col + 1, row);
            table.SetColumnSpan(control, span);
        }

        private void LoadData()
        {
            BindCombo(cboTheLoai, _service.LayTheLoai(), false);
            BindCombo(cboNxb, _service.LayNhaXuatBan(), true);
            BindCombo(cboViTri, _service.LayViTri(), true);

            var authors = _service.LayTacGia();
            clbTacGia.DataSource = authors.ToList();
            clbTacGia.DisplayMember = nameof(LookupItemModel.Ten);
            clbTacGia.ValueMember = nameof(LookupItemModel.Id);

            if (_maSach.HasValue)
            {
                SachSaveDto dto = _service.LayChiTiet(_maSach.Value)
                    ?? throw new InvalidOperationException("Không tìm thấy sách.");
                txtTen.Text = dto.TenSach;
                txtIsbn.Text = dto.Isbn;
                txtNam.Text = dto.NamXuatBan?.ToString();
                txtNgonNgu.Text = dto.NgonNgu;
                txtSoTrang.Text = dto.SoTrang?.ToString();
                txtGia.Text = dto.GiaBia?.ToString(CultureInfo.InvariantCulture);
                txtAnhBia.Text = dto.AnhBia;
                txtMoTa.Text = dto.MoTa;
                cboTheLoai.SelectedValue = dto.MaTheLoai;
                cboNxb.SelectedValue = dto.MaNhaXuatBan ?? 0;
                cboViTri.SelectedValue = dto.MaViTri ?? 0;

                for (int i = 0; i < authors.Count; i++)
                    clbTacGia.SetItemChecked(i, dto.MaTacGia.Contains(authors[i].Id));
            }

            if (_readOnly)
            {
                foreach (Control c in GetAllControls(this))
                {
                    if (c is TextBox tb) tb.ReadOnly = true;
                    if (c is ComboBox cb) cb.Enabled = false;
                    if (c is CheckedListBox clb) clb.Enabled = false;
                }
            }
        }

        private static void BindCombo(ComboBox combo, IReadOnlyList<LookupItemModel> data, bool allowEmpty)
        {
            var list = new List<LookupItemModel>();
            if (allowEmpty) list.Add(new LookupItemModel { Id = 0, Ten = "-- Không chọn --" });
            list.AddRange(data);
            combo.DataSource = list;
            combo.DisplayMember = nameof(LookupItemModel.Ten);
            combo.ValueMember = nameof(LookupItemModel.Id);
        }

        private void Save_Click(object? sender, EventArgs e)
        {
            try
            {
                var dto = new SachSaveDto
                {
                    MaSach = _maSach ?? 0,
                    TenSach = txtTen.Text,
                    Isbn = txtIsbn.Text,
                    MaTheLoai = Convert.ToInt32(cboTheLoai.SelectedValue ?? 0),
                    MaNhaXuatBan = NullableId(cboNxb),
                    NamXuatBan = ParseInt(txtNam.Text),
                    NgonNgu = txtNgonNgu.Text,
                    SoTrang = ParseInt(txtSoTrang.Text),
                    GiaBia = ParseDecimal(txtGia.Text),
                    MaViTri = NullableId(cboViTri),
                    AnhBia = txtAnhBia.Text,
                    MoTa = txtMoTa.Text,
                    MaTacGia = clbTacGia.CheckedItems.Cast<LookupItemModel>().Select(x => x.Id).ToList()
                };

                if (_maSach.HasValue) _service.CapNhat(dto);
                else _service.Them(dto);

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                Exception root = ex;
                while (root.InnerException != null) root = root.InnerException;
                MessageBox.Show(root.Message, "Dữ liệu không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private static int? NullableId(ComboBox combo)
        {
            int id = Convert.ToInt32(combo.SelectedValue ?? 0);
            return id > 0 ? id : null;
        }

        private static int? ParseInt(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return null;
            if (!int.TryParse(value, out int result)) throw new ArgumentException($"'{value}' không phải số nguyên hợp lệ.");
            return result;
        }

        private static decimal? ParseDecimal(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return null;
            if (decimal.TryParse(value, NumberStyles.Number, CultureInfo.CurrentCulture, out decimal result) ||
                decimal.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture, out result)) return result;
            throw new ArgumentException($"'{value}' không phải số tiền hợp lệ.");
        }

        private static IEnumerable<Control> GetAllControls(Control root)
        {
            foreach (Control child in root.Controls)
            {
                yield return child;
                foreach (Control nested in GetAllControls(child)) yield return nested;
            }
        }


    }
}

