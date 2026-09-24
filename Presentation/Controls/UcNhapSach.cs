using BusinessLayer.Services;
using DataLayer.Models;
using Presentation.Helpers;
using Presentation.Models;
using System.Drawing.Printing;

namespace Presentation.Controls
{
    public partial class UcNhapSach : UserControl
    {
        private readonly NhapSachService? _service;
        private readonly HashSet<int> _phieuDaChon = new();
        private List<NhapSachGridModel> _duLieuGoc = new();
        private List<NhapSachGridModel> _duLieuLoc = new();
        private int _trangHienTai = 1;
        private int _soDongMoiTrang = 5;
        private int _tongTrang = 1;
        private int? _maPhieuDangXem;
        private bool _dangKhoiTaoBoLoc;
        private bool _daGanKhoangNgayBanDau;

        public UcNhapSach()
        {
            InitializeComponent();

            if (DesignModeHelper.IsDesignMode(this))
            {
                return;
            }

            _service = new NhapSachService();
            CauHinhGiaoDien();
            GanSuKien();
            ApDungPhanQuyen();
        }

        private void ApDungPhanQuyen()
        {
            btnLapPhieuNhap.Enabled = PermissionHelper.CanAdd("NHAPSACH.LAPPHIEU");
            btnThemNhaCungCap.Enabled = PermissionHelper.CanAdd("NHAPSACH.NHACUNGCAP");
            btnXuatExcel.Enabled = PermissionHelper.CanExport("NHAPSACH.DANHSACH");
            colIn.Visible = PermissionHelper.CanPrint("NHAPSACH.DANHSACH");
        }

        private static bool KiemTraQuyen(bool allowed, string message)
        {
            if (allowed) return true;
            MessageBox.Show(message, "Không đủ quyền", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        private void GanSuKien()
        {
            Load += UcNhapSach_Load;
            txtTimKiem.TextChanged += BoLoc_Changed;
            cboTrangThai.SelectedIndexChanged += BoLoc_Changed;
            cboNhaCungCap.SelectedIndexChanged += BoLoc_Changed;
            dtpTuNgay.ValueChanged += BoLoc_Changed;
            dtpDenNgay.ValueChanged += BoLoc_Changed;

            btnLamMoi.Click += BtnLamMoi_Click;
            btnXuatExcel.Click += BtnXuatExcel_Click;
            // btnNhapExcel.Click += BtnNhapExcel_Click;
            btnLapPhieuNhap.Click += BtnLapPhieuNhap_Click;
            btnThemNhaCungCap.Click += BtnThemNhaCungCap_Click;

            dgvPhieuNhap.CellClick += DgvPhieuNhap_CellClick;
            dgvPhieuNhap.CellContentClick += DgvPhieuNhap_CellContentClick;
            dgvPhieuNhap.CellValueChanged += DgvPhieuNhap_CellValueChanged;
            dgvPhieuNhap.CurrentCellDirtyStateChanged += DgvPhieuNhap_CurrentCellDirtyStateChanged;
            dgvPhieuNhap.ColumnHeaderMouseClick += DgvPhieuNhap_ColumnHeaderMouseClick;
            btnTrangThaiPhieu.Click += BtnTrangThaiPhieu_Click;

            btnTrangDau.Click += (_, _) => ChuyenTrang(1);
            btnTrangTruoc.Click += (_, _) => ChuyenTrang(_trangHienTai - 1);
            btnTrangSau.Click += (_, _) => ChuyenTrang(_trangHienTai + 1);
            btnTrangCuoi.Click += (_, _) => ChuyenTrang(_tongTrang);
            btnPage1.Click += NutTrang_Click;
            btnPage2.Click += NutTrang_Click;
            btnPage3.Click += NutTrang_Click;
            btnPage4.Click += NutTrang_Click;
            btnLastPage.Click += NutTrang_Click;
            btnGo.Click += BtnGo_Click;
            txtTrang.KeyPress += TxtTrang_KeyPress;
            txtTrang.KeyDown += TxtTrang_KeyDown;

            // lnkXemTatCaNhap.LinkClicked += (_, _) =>
            // {
            //     txtTimKiem.Clear();
            //     cboTrangThai.SelectedIndex = 0;
            //     cboNhaCungCap.SelectedIndex = 0;
            //     dtpTuNgay.Value = LayNgayNhoNhat();
            //     dtpDenNgay.Value = DateTime.Today;
            //     LocDuLieu();
            // };
        }

        private void UcNhapSach_Load(object? sender, EventArgs e)
        {
            LoadCurrentUser();
            KhoiTaoBoLoc();
            TaiDuLieu();
        }

        private void LoadCurrentUser()
        {
            string displayName = string.IsNullOrWhiteSpace(CurrentUser.HoTen)
                ? (string.IsNullOrWhiteSpace(CurrentUser.TenDangNhap) ? "Người dùng" : CurrentUser.TenDangNhap)
                : CurrentUser.HoTen;

            lblUser.Text = $"Xin chào, {displayName}";
            lblRole.Text = string.IsNullOrWhiteSpace(CurrentUser.VaiTro)
                ? "Người dùng"
                : CurrentUser.VaiTro;

            if (Controls.Find("picAvatar", true).FirstOrDefault() is PictureBox avatar)
            {
                CurrentUserAvatarHelper.Apply(avatar);
            }
        }

        private void KhoiTaoBoLoc()
        {
            if (_service == null) return;

            _dangKhoiTaoBoLoc = true;
            try
            {
                cboTrangThai.Items.Clear();
                cboTrangThai.Items.AddRange(new object[]
                {
                    "-- Tất cả trạng thái --",
                    "Hoàn thành",
                    "Đang nhập",
                    "Chờ duyệt",
                    "Đã hủy"
                });
                cboTrangThai.SelectedIndex = 0;

                List<NhaCungCapLookupModel> nhaCungCaps = _service.GetNhaCungCaps();
                nhaCungCaps.Insert(0, new NhaCungCapLookupModel
                {
                    MaNhaCungCap = 0,
                    TenNhaCungCap = "-- Tất cả nhà cung cấp --"
                });

                cboNhaCungCap.DataSource = nhaCungCaps;
                cboNhaCungCap.DisplayMember = nameof(NhaCungCapLookupModel.TenNhaCungCap);
                cboNhaCungCap.ValueMember = nameof(NhaCungCapLookupModel.MaNhaCungCap);

                dtpTuNgay.Value = DateTime.Today.AddMonths(-1);
                dtpDenNgay.Value = DateTime.Today;
            }
            finally
            {
                _dangKhoiTaoBoLoc = false;
            }
        }

        private void TaiDuLieu()
        {
            if (_service == null) return;

            try
            {
                Cursor = Cursors.WaitCursor;
                _duLieuGoc = _service.GetDanhSach();
                _phieuDaChon.RemoveWhere(id => _duLieuGoc.All(x => x.MaPhieuNhap != id));

                if (!_daGanKhoangNgayBanDau)
                {
                    _dangKhoiTaoBoLoc = true;
                    try
                    {
                        dtpTuNgay.Value = LayNgayNhoNhat();
                        dtpDenNgay.Value = DateTime.Today;
                    }
                    finally
                    {
                        _dangKhoiTaoBoLoc = false;
                    }
                    _daGanKhoangNgayBanDau = true;
                }

                CapNhatCards();
                CapNhatNhapGanDay();
                CapNhatNhaCungCapThuongXuyen();
                LocDuLieu();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể tải dữ liệu nhập sách.\n" + ex.Message,
                    "Lỗi dữ liệu",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void CapNhatCards()
        {
            if (_service == null) return;
            NhapSachStatisticsModel model = _service.GetStatistics();

            lblPhieuNhapHomNay.Text = model.PhieuNhapHomNay.ToString("N0");
            HienThiBienDong(lblPhieuNhapHomNaySub, model.PhieuNhapHomNay, model.PhieuNhapHomQua, "so với hôm qua");

            lblSachNhapThangNay.Text = model.SoCuonNhapThangNay.ToString("N0");
            HienThiBienDong(lblSachNhapThangNaySub, model.SoCuonNhapThangNay, model.SoCuonNhapThangTruoc, "so với tháng trước");

            lblNhaCungCap.Text = model.SoNhaCungCapHoatDong.ToString("N0");
            lblNhaCungCapSub.Text = "Đang hoạt động";
            lblNhaCungCapSub.ForeColor = Color.FromArgb(90, 100, 125);

            lblTongTienNhap.Text = model.TongTienNhapThangNay.ToString("N0") + " đ";
            HienThiBienDong(lblTongTienNhapSub, model.TongTienNhapThangNay, model.TongTienNhapThangTruoc, "so với tháng trước");
        }

        private static void HienThiBienDong(Label label, decimal hienTai, decimal truoc, string moTa)
        {
            decimal phanTram;
            if (truoc == 0)
            {
                phanTram = hienTai > 0 ? 100 : 0;
            }
            else
            {
                phanTram = (hienTai - truoc) / truoc * 100;
            }

            if (phanTram > 0)
            {
                label.Text = $"↑ {phanTram:N0}% {moTa}";
                MetricTrendHelper.ApplyColor(label, phanTram);
            }
            else if (phanTram < 0)
            {
                label.Text = $"↓ {Math.Abs(phanTram):N0}% {moTa}";
                MetricTrendHelper.ApplyColor(label, phanTram);
            }
            else
            {
                label.Text = $"Không đổi {moTa}";
                MetricTrendHelper.ApplyColor(label, phanTram);
            }
        }

        private void BoLoc_Changed(object? sender, EventArgs e)
        {
            if (_dangKhoiTaoBoLoc) return;
            LocDuLieu();
        }

        private void LocDuLieu()
        {
            IEnumerable<NhapSachGridModel> query = _duLieuGoc;
            string keyword = txtTimKiem.Text.Trim();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(x =>
                    x.MaPhieuText.Contains(keyword, StringComparison.CurrentCultureIgnoreCase) ||
                    x.TenNhaCungCap.Contains(keyword, StringComparison.CurrentCultureIgnoreCase) ||
                    x.NguoiLap.Contains(keyword, StringComparison.CurrentCultureIgnoreCase));
            }

            string trangThai = cboTrangThai.SelectedItem?.ToString() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(trangThai) && !trangThai.StartsWith("--"))
            {
                query = query.Where(x => x.TrangThai == trangThai);
            }

            if (cboNhaCungCap.SelectedValue != null &&
                int.TryParse(cboNhaCungCap.SelectedValue.ToString(), out int maNcc) && maNcc > 0)
            {
                query = query.Where(x => x.MaNhaCungCap == maNcc);
            }

            DateTime tuNgay = dtpTuNgay.Value.Date;
            DateTime denNgayGoc = dtpDenNgay.Value.Date;
            if (tuNgay > denNgayGoc)
            {
                _duLieuLoc.Clear();
                _trangHienTai = 1;
                HienThiTrang();
                MessageBox.Show("Từ ngày không được lớn hơn đến ngày.", "Khoảng ngày không hợp lệ",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DateTime denNgay = denNgayGoc.AddDays(1);
            query = query.Where(x => x.NgayNhap >= tuNgay && x.NgayNhap < denNgay);
            _duLieuLoc = query.OrderByDescending(x => x.NgayNhap).ThenByDescending(x => x.MaPhieuNhap).ToList();
            _trangHienTai = 1;
            HienThiTrang();
        }

        private void HienThiTrang()
        {
            int tongBanGhi = _duLieuLoc.Count;
            _tongTrang = Math.Max(1, (int)Math.Ceiling(tongBanGhi / (double)_soDongMoiTrang));
            _trangHienTai = Math.Clamp(_trangHienTai, 1, _tongTrang);

            List<object> dataTrang = _duLieuLoc
                .Skip((_trangHienTai - 1) * _soDongMoiTrang)
                .Take(_soDongMoiTrang)
                .Select((x, index) => (object)new
                {
                    Chon = _phieuDaChon.Contains(x.MaPhieuNhap),
                    STT = (_trangHienTai - 1) * _soDongMoiTrang + index + 1,
                    x.MaPhieuNhap,
                    x.MaPhieuText,
                    x.TenNhaCungCap,
                    NgayNhapText = x.NgayNhap.ToString("dd/MM/yyyy"),
                    x.SoDauSach,
                    x.TongSoCuon,
                    TongTienText = x.TongTien.ToString("N0") + " đ",
                    x.TrangThai,
                    x.NguoiLap
                })
                .ToList();

            dgvPhieuNhap.DataSource = null;
            dgvPhieuNhap.DataSource = dataTrang;
            dgvPhieuNhap.ClearSelection();

            int batDau = tongBanGhi == 0 ? 0 : (_trangHienTai - 1) * _soDongMoiTrang + 1;
            int ketThuc = Math.Min(_trangHienTai * _soDongMoiTrang, tongBanGhi);
            lblPageInfo.Text = $"Hiển thị {batDau:N0}–{ketThuc:N0} của {tongBanGhi:N0} phiếu";
            lblTongTrang.Text = $"/ {_tongTrang:N0}";
            txtTrang.Text = _trangHienTai.ToString();

            TaoNutPhanTrang();
            CapNhatTrangThaiNutDieuHuong();

            if (tongBanGhi == 0)
            {
                HienThiChiTietRong();
            }
            else
            {
                int firstIndex = (_trangHienTai - 1) * _soDongMoiTrang;
                List<NhapSachGridModel> dataTrangModel = _duLieuLoc.Skip(firstIndex).Take(_soDongMoiTrang).ToList();
                if (dataTrangModel.Count > 0 &&
                    (!_maPhieuDangXem.HasValue || dataTrangModel.All(x => x.MaPhieuNhap != _maPhieuDangXem.Value)))
                {
                    TaiChiTiet(dataTrangModel[0].MaPhieuNhap);
                }
            }
        }

        private void TaoNutPhanTrang()
        {
            var buttons = new[] { btnPage1, btnPage2, btnPage3, btnPage4 };
            int start = Math.Max(1, Math.Min(_trangHienTai - 1, _tongTrang - buttons.Length + 1));

            for (int i = 0; i < buttons.Length; i++)
            {
                int page = start + i;
                buttons[i].Visible = page <= _tongTrang;
                buttons[i].Text = page.ToString();
                buttons[i].Tag = page;
                DinhDangNutTrang(buttons[i], page == _trangHienTai);
            }

            bool canHienTrangCuoi = _tongTrang > start + buttons.Length - 1;
            lblDots.Visible = canHienTrangCuoi;
            btnLastPage.Visible = canHienTrangCuoi;
            btnLastPage.Text = _tongTrang.ToString();
            btnLastPage.Tag = _tongTrang;
            btnLastPage.AutoSize = true;
            DinhDangNutTrang(btnLastPage, _trangHienTai == _tongTrang);
        }

        private static void DinhDangNutTrang(Guna.UI2.WinForms.Guna2Button button, bool active)
        {
            button.AutoSize = true;
            button.Padding = new Padding(5, 0, 5, 0);
            button.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            button.FillColor = active ? Color.FromArgb(35, 85, 220) : Color.White;
            button.ForeColor = active ? Color.White : Color.FromArgb(25, 50, 105);
            button.BorderThickness = active ? 0 : 1;
            button.BorderColor = Color.FromArgb(210, 220, 238);
        }

        private void CapNhatTrangThaiNutDieuHuong()
        {
            bool coTrangTruoc = _trangHienTai > 1;
            bool coTrangSau = _trangHienTai < _tongTrang;
            btnTrangDau.Enabled = coTrangTruoc;
            btnTrangTruoc.Enabled = coTrangTruoc;
            btnTrangSau.Enabled = coTrangSau;
            btnTrangCuoi.Enabled = coTrangSau;
        }

        private void ChuyenTrang(int page)
        {
            int pageHopLe = Math.Clamp(page, 1, _tongTrang);
            if (pageHopLe == _trangHienTai) return;
            _trangHienTai = pageHopLe;
            HienThiTrang();
        }

        private void NutTrang_Click(object? sender, EventArgs e)
        {
            if (sender is Guna.UI2.WinForms.Guna2Button button &&
                int.TryParse(button.Tag?.ToString() ?? button.Text, out int page))
            {
                ChuyenTrang(page);
            }
        }

        private void BtnGo_Click(object? sender, EventArgs e)
        {
            if (!int.TryParse(txtTrang.Text.Trim(), out int page) || page < 1 || page > _tongTrang)
            {
                MessageBox.Show($"Vui lòng nhập số trang từ 1 đến {_tongTrang:N0}.", "Trang không hợp lệ",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtTrang.SelectAll();
                txtTrang.Focus();
                return;
            }

            ChuyenTrang(page);
        }

        private void TxtTrang_KeyPress(object? sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) e.Handled = true;
        }

        private void TxtTrang_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnGo.PerformClick();
                e.SuppressKeyPress = true;
            }
        }

        private void DgvPhieuNhap_CurrentCellDirtyStateChanged(object? sender, EventArgs e)
        {
            if (dgvPhieuNhap.IsCurrentCellDirty && dgvPhieuNhap.CurrentCell?.OwningColumn == colChon)
            {
                dgvPhieuNhap.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void DgvPhieuNhap_CellValueChanged(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex != colChon.Index) return;
            int? maPhieu = LayMaPhieuTuDong(e.RowIndex);
            if (!maPhieu.HasValue) return;

            bool chon = Convert.ToBoolean(dgvPhieuNhap.Rows[e.RowIndex].Cells[colChon.Name].Value ?? false);
            if (chon) _phieuDaChon.Add(maPhieu.Value);
            else _phieuDaChon.Remove(maPhieu.Value);
        }

        private void DgvPhieuNhap_ColumnHeaderMouseClick(object? sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex != colChon.Index) return;

            List<NhapSachGridModel> dataTrang = _duLieuLoc
                .Skip((_trangHienTai - 1) * _soDongMoiTrang)
                .Take(_soDongMoiTrang)
                .ToList();

            bool tatCaDaChon = dataTrang.Count > 0 && dataTrang.All(x => _phieuDaChon.Contains(x.MaPhieuNhap));
            foreach (NhapSachGridModel item in dataTrang)
            {
                if (tatCaDaChon) _phieuDaChon.Remove(item.MaPhieuNhap);
                else _phieuDaChon.Add(item.MaPhieuNhap);
            }

            HienThiTrang();
        }

        private void DgvPhieuNhap_CellContentClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            string columnName = dgvPhieuNhap.Columns[e.ColumnIndex].Name;
            int? maPhieu = LayMaPhieuTuDong(e.RowIndex);
            if (!maPhieu.HasValue) return;

            if (columnName == colXem.Name)
            {
                using FrmChiTietPhieuNhap frm = new(maPhieu.Value);
                frm.ShowDialog(FindForm());
            }
            else if (columnName == colIn.Name)
            {
                if (!KiemTraQuyen(PermissionHelper.CanPrint("NHAPSACH.DANHSACH"), "Bạn không có quyền in phiếu nhập.")) return;
                using FrmInPhieuNhap frm = new(maPhieu.Value);
                frm.ShowDialog(FindForm());
            }
        }

        private void DgvPhieuNhap_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            int? maPhieu = LayMaPhieuTuDong(e.RowIndex);
            if (maPhieu.HasValue) TaiChiTiet(maPhieu.Value);
        }

        private int? LayMaPhieuTuDong(int rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= dgvPhieuNhap.Rows.Count) return null;
            object? dataBound = dgvPhieuNhap.Rows[rowIndex].DataBoundItem;
            if (dataBound == null) return null;
            var property = dataBound.GetType().GetProperty("MaPhieuNhap");
            object? value = property?.GetValue(dataBound);
            return value == null ? null : Convert.ToInt32(value);
        }


        private void TaiChiTiet(int maPhieuNhap)
        {
            if (_service == null) return;
            NhapSachDetailModel? model = _service.GetChiTiet(maPhieuNhap);
            if (model == null)
            {
                HienThiChiTietRong();
                return;
            }

            _maPhieuDangXem = maPhieuNhap;
            label2.Text = "Chi tiết phiếu nhập";
            lblTenNhaCungCapChiTiet.Text = model.TenNhaCungCap;
            lblSoDienThoaiNcc.Text = model.SoDienThoaiNcc;
            lblDiaChiNcc.Text = model.DiaChiNcc;
            lblMaPhieuChiTiet.Text = model.MaPhieuText;
            lblNgayNhapChiTiet.Text = model.NgayNhap.ToString("dd/MM/yyyy");
            lblNguoiLapChiTiet.Text = model.NguoiLap;
            lblDanhSachDauSachTitle.Text = $"Danh sách đầu sách ({model.DauSaches.Count:N0})";
            lblTamTinh.Text = model.TamTinh.ToString("N0") + " đ";
            lblChietKhau.Text = model.ChietKhau.ToString("N0") + " đ";
            lblTongTienChiTiet.Text = model.TongTien.ToString("N0") + " đ";
            HienThiTrangThaiChiTiet(model.TrangThai);

            flpDauSach.SuspendLayout();
            try
            {
                foreach (Control control in flpDauSach.Controls.Cast<Control>().ToList()) control.Dispose();
                flpDauSach.Controls.Clear();

                foreach (DauSachNhapItemModel item in model.DauSaches)
                {
                    UcDauSachNhapItem uc = new();
                    uc.SetData(item);
                    uc.Width = Math.Max(220, flpDauSach.ClientSize.Width - 25);
                    uc.Margin = new Padding(0, 0, 0, 5);
                    flpDauSach.Controls.Add(uc);
                }
            }
            finally
            {
                flpDauSach.ResumeLayout();
            }
        }

        private void HienThiTrangThaiChiTiet(string trangThai)
        {
            btnTrangThaiPhieu.Text = trangThai;
            (Color fill, Color fore) = trangThai switch
            {
                "Hoàn thành" => (Color.FromArgb(228, 248, 235), Color.FromArgb(25, 145, 70)),
                "Đang nhập" => (Color.FromArgb(232, 241, 255), Color.FromArgb(30, 95, 220)),
                "Chờ duyệt" => (Color.FromArgb(255, 244, 228), Color.FromArgb(230, 120, 25)),
                "Đã hủy" => (Color.FromArgb(255, 235, 237), Color.FromArgb(225, 55, 65)),
                _ => (Color.FromArgb(240, 242, 246), Color.Gray)
            };
            btnTrangThaiPhieu.FillColor = fill;
            btnTrangThaiPhieu.ForeColor = fore;
            btnTrangThaiPhieu.Enabled = trangThai == "Đang nhập";
            btnTrangThaiPhieu.Cursor = btnTrangThaiPhieu.Enabled ? Cursors.Hand : Cursors.Default;
        }

        private void BtnTrangThaiPhieu_Click(object? sender, EventArgs e)
        {
            if (!KiemTraQuyen(PermissionHelper.CanEdit("NHAPSACH.LAPPHIEU"), "Bạn không có quyền hoàn tất phiếu nhập.")) return;
            if (_service == null || !_maPhieuDangXem.HasValue || btnTrangThaiPhieu.Text != "Đang nhập") return;
            DialogResult confirm = MessageBox.Show(
                "Hoàn tất phiếu sẽ tạo toàn bộ cuốn sách vào kho và không thể thực hiện lần hai. Tiếp tục?",
                "Hoàn tất phiếu nhập", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            try
            {
                _service.HoanTatPhieuNhap(_maPhieuDangXem.Value);
                MessageBox.Show("Đã hoàn tất phiếu nhập và cập nhật kho.", "Thành công",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                TaiDuLieu();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể hoàn tất phiếu nhập.\n" + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void HienThiChiTietRong()
        {
            _maPhieuDangXem = null;
            lblTenNhaCungCapChiTiet.Text = "Chưa chọn phiếu nhập";
            lblSoDienThoaiNcc.Text = "-";
            lblDiaChiNcc.Text = "-";
            lblMaPhieuChiTiet.Text = "-";
            lblNgayNhapChiTiet.Text = "-";
            lblNguoiLapChiTiet.Text = "-";
            lblDanhSachDauSachTitle.Text = "Danh sách đầu sách (0)";
            lblTamTinh.Text = "0 đ";
            lblChietKhau.Text = "0 đ";
            lblTongTienChiTiet.Text = "0 đ";
            btnTrangThaiPhieu.Text = "Chưa chọn";
            btnTrangThaiPhieu.FillColor = Color.FromArgb(240, 242, 246);
            btnTrangThaiPhieu.ForeColor = Color.Gray;
            btnTrangThaiPhieu.Enabled = false;
            btnTrangThaiPhieu.Cursor = Cursors.Default;
            foreach (Control control in flpDauSach.Controls.Cast<Control>().ToList()) control.Dispose();
            flpDauSach.Controls.Clear();
        }

        private void CapNhatNhapGanDay()
        {
            if (_service == null) return;
            List<NhapSachGridModel> data = _service.GetNhapGanDay(3);
            var labels = new[] { lblNhapGanDay1, lblNhapGanDay2, lblNhapGanDay3 };
            for (int i = 0; i < labels.Length; i++)
            {
                if (i < data.Count)
                {
                    NhapSachGridModel item = data[i];
                    labels[i].Text = $"{item.MaPhieuText}   {item.TenNhaCungCap}   {item.NgayNhap:dd/MM/yyyy}   {item.TongTien:N0} đ";
                    labels[i].Visible = true;
                }
                else
                {
                    labels[i].Text = "Chưa có dữ liệu";
                    labels[i].Visible = i == 0;
                }
            }
        }

        private void CapNhatNhaCungCapThuongXuyen()
        {
            if (_service == null) return;
            List<NhaCungCapThuongXuyenModel> data = _service.GetNhaCungCapThuongXuyen(3);
            var labels = new[] { lblNhaCungCapThuongXuyen1, lblNhaCungCapThuongXuyen2, d };
            for (int i = 0; i < labels.Length; i++)
            {
                labels[i].Text = i < data.Count
                    ? $"{data[i].TenNhaCungCap}   {data[i].SoLanNhap:N0} lần"
                    : "Chưa có dữ liệu";
            }
        }

        private void BtnLamMoi_Click(object? sender, EventArgs e)
        {
            _dangKhoiTaoBoLoc = true;
            try
            {
                txtTimKiem.Clear();
                cboTrangThai.SelectedIndex = 0;
                cboNhaCungCap.SelectedIndex = 0;
                dtpTuNgay.Value = DateTime.Today.AddMonths(-1);
                dtpDenNgay.Value = DateTime.Today;
                _phieuDaChon.Clear();
            }
            finally
            {
                _dangKhoiTaoBoLoc = false;
            }
            TaiDuLieu();
        }

        private void BtnXuatExcel_Click(object? sender, EventArgs e)
        {
            if (!KiemTraQuyen(PermissionHelper.CanExport("NHAPSACH.DANHSACH"), "Bạn không có quyền xuất danh sách phiếu nhập.")) return;
            List<NhapSachGridModel> data = _phieuDaChon.Count > 0
                ? _duLieuLoc.Where(x => _phieuDaChon.Contains(x.MaPhieuNhap)).ToList()
                : _duLieuLoc.ToList();

            if (data.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất.", "Xuất Excel", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using SaveFileDialog dialog = new()
            {
                Filter = "Excel Workbook (*.xlsx)|*.xlsx",
                FileName = $"PhieuNhap_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx",
                Title = "Chọn nơi lưu danh sách phiếu nhập"
            };

            if (dialog.ShowDialog(FindForm()) != DialogResult.OK) return;

            string[] headers = { "STT", "Mã phiếu", "Nhà cung cấp", "Ngày nhập", "Số đầu sách", "Tổng số cuốn", "Tổng tiền", "Trạng thái", "Người lập" };
            List<string[]> rows = data.Select((x, index) => new[]
            {
                (index + 1).ToString(), x.MaPhieuText, x.TenNhaCungCap,
                x.NgayNhap.ToString("dd/MM/yyyy"), x.SoDauSach.ToString("N0"),
                x.TongSoCuon.ToString("N0"), x.TongTien.ToString("N0"), x.TrangThai, x.NguoiLap
            }).ToList();

            try
            {
                ExcelHelper.ExportToXlsx(dialog.FileName, "Phiếu nhập", headers, rows);
                MessageBox.Show($"Đã xuất {rows.Count:N0} phiếu nhập.", "Xuất Excel thành công",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể xuất Excel.\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnNhapExcel_Click(object? sender, EventArgs e)
        {
            MessageBox.Show(
                "Chức năng nhập Excel cần được thực hiện qua form riêng để kiểm tra mã sách, nhà cung cấp và dữ liệu trùng trước khi lưu.",
                "Nhập Excel", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnThemNhaCungCap_Click(object? sender, EventArgs e)
        {
            if (!KiemTraQuyen(PermissionHelper.CanAdd("NHAPSACH.NHACUNGCAP"), "Bạn không có quyền thêm nhà cung cấp.")) return;
            using FrmThemNhaCungCap frm = new();
            if (frm.ShowDialog(FindForm()) == DialogResult.OK)
            {
                KhoiTaoBoLoc();
                TaiDuLieu();
            }
        }

        private void BtnChuaHoanThien_Click(object? sender, EventArgs e)
        {
            string tenChucNang = sender == btnLapPhieuNhap ? "Lập phiếu nhập" : "Thêm nhà cung cấp";
            MessageBox.Show($"Giao diện {tenChucNang} sẽ được mở bằng form riêng ở bước tiếp theo.",
                tenChucNang, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void InPhieuNhap(int maPhieuNhap)
        {
            if (_service == null) return;
            NhapSachDetailModel? model = _service.GetChiTiet(maPhieuNhap);
            if (model == null) return;

            using PrintDocument document = new();
            document.DocumentName = model.MaPhieuText;
            document.PrintPage += (_, e) =>
            {
                Graphics graphics = e.Graphics;
                float y = 50;
                using Font titleFont = new("Segoe UI", 18, FontStyle.Bold);
                using Font normalFont = new("Segoe UI", 10);
                using Font boldFont = new("Segoe UI", 10, FontStyle.Bold);

                graphics.DrawString("PHIẾU NHẬP SÁCH", titleFont, Brushes.Navy, 60, y); y += 48;
                graphics.DrawString($"Mã phiếu: {model.MaPhieuText}", boldFont, Brushes.Black, 60, y); y += 25;
                graphics.DrawString($"Ngày nhập: {model.NgayNhap:dd/MM/yyyy}", normalFont, Brushes.Black, 60, y); y += 25;
                graphics.DrawString($"Nhà cung cấp: {model.TenNhaCungCap}", normalFont, Brushes.Black, 60, y); y += 25;
                graphics.DrawString($"Người lập: {model.NguoiLap}", normalFont, Brushes.Black, 60, y); y += 40;

                foreach (DauSachNhapItemModel item in model.DauSaches)
                {
                    graphics.DrawString($"• {item.TenSach} - SL: {item.SoLuong:N0} - {item.ThanhTien:N0} đ",
                        normalFont, Brushes.Black, 70, y);
                    y += 24;
                }

                y += 20;
                graphics.DrawString($"TỔNG TIỀN: {model.TongTien:N0} đ", boldFont, Brushes.DarkGreen, 60, y);
            };

            using PrintPreviewDialog preview = new() { Document = document, Width = 1000, Height = 700 };
            preview.ShowDialog(FindForm());
        }

        private void BtnLapPhieuNhap_Click(object? sender, EventArgs e)
        {
            if (!KiemTraQuyen(PermissionHelper.CanAdd("NHAPSACH.LAPPHIEU"), "Bạn không có quyền lập phiếu nhập.")) return;
            using FrmLapPhieuNhap frm = new();
            if (frm.ShowDialog(FindForm()) == DialogResult.OK)
            {
                TaiDuLieu();
            }
        }

        private DateTime LayNgayNhoNhat()
        {
            return _duLieuGoc.Count == 0 ? DateTime.Today.AddMonths(-1) : _duLieuGoc.Min(x => x.NgayNhap).Date;
        }

        private void CauHinhGiaoDien()
        {
            SuspendLayout();
            BackColor = Color.FromArgb(248, 250, 255);

            lblTitle.Font = new Font("Segoe UI", 17F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(24, 38, 92);
            lblSubTitle.Font = new Font("Segoe UI", 9.5F);
            lblSubTitle.ForeColor = Color.FromArgb(105, 116, 150);
            picAvatar.IconSize = 60;
            picAvatar.BorderStyle = BorderStyle.None;
            lblUser.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblRole.Font = new Font("Segoe UI", 9F);
            lblRole.ForeColor = Color.FromArgb(98, 108, 140);

            foreach (Guna.UI2.WinForms.Guna2Panel card in new[]
                     { cardPhieuNhapHomNay, cardSachNhapThangNay, cardNhaCungCap, cardTongTienNhap })
                TrangTriTheThongKe(card);
            foreach (Label value in new[] { lblPhieuNhapHomNay, lblSachNhapThangNay, lblNhaCungCap, lblTongTienNhap })
                value.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            foreach (Label caption in new[] { lblTongSachTitle, lblSachCoSanTitle, lblSachDangMuonTitle, f })
                caption.Font = new Font("Segoe UI", 9F, FontStyle.Bold);

            TrangTriNut(btnLapPhieuNhap, Color.FromArgb(38, 100, 235), Color.White);
            TrangTriNut(btnThemNhaCungCap, Color.FromArgb(126, 69, 210), Color.White);
            // TrangTriNut(btnNhapExcel, Color.FromArgb(18, 151, 85), Color.White);
            TrangTriNut(btnXuatExcel, Color.FromArgb(232, 252, 241), Color.FromArgb(16, 145, 82));
            TrangTriNut(btnLamMoi, Color.White, Color.FromArgb(45, 58, 105));
            btnLamMoi.BorderColor = Color.FromArgb(218, 225, 239);
            btnLamMoi.BorderThickness = 1;

            pnlChart.BorderThickness = 0;
            pnlChart.FillColor = Color.Transparent;
            guna2Panel1.BorderColor = Color.FromArgb(226, 232, 244);
            guna2Panel1.BorderRadius = 10;

            txtTimKiem.BorderColor = Color.FromArgb(218, 225, 239);
            txtTimKiem.FocusedState.BorderColor = Color.FromArgb(71, 96, 235);
            txtTimKiem.HoverState.BorderColor = Color.FromArgb(154, 169, 225);
            foreach (Guna.UI2.WinForms.Guna2ComboBox combo in new[] { cboNhaCungCap, cboTrangThai })
            {
                combo.BorderColor = Color.FromArgb(218, 225, 239);
                combo.FocusedState.BorderColor = Color.FromArgb(71, 96, 235);
                combo.HoverState.BorderColor = Color.FromArgb(154, 169, 225);
            }
            foreach (Guna.UI2.WinForms.Guna2DateTimePicker picker in new[] { dtpTuNgay, dtpDenNgay })
            {
                picker.BorderColor = Color.FromArgb(218, 225, 239);
                picker.FillColor = Color.White;
                picker.BorderRadius = 8;
            }

            pnlDanhSach.BorderColor = Color.FromArgb(226, 232, 244);
            pnlDanhSach.BorderRadius = 12;
            pnlDanhSach.ShadowDecoration.Enabled = true;
            pnlDanhSach.ShadowDecoration.Depth = 6;
            pnlDanhSach.ShadowDecoration.Color = Color.FromArgb(145, 125, 220);
            dgvPhieuNhap.AutoGenerateColumns = false;
            // DataPropertyName là binding dữ liệu, còn giao diện bảng được quản lý trong Designer.
            colChon.DataPropertyName = "Chon";
            colSTT.DataPropertyName = "STT";
            colMaPhieu.DataPropertyName = "MaPhieuText";
            colNhaCungCap.DataPropertyName = "TenNhaCungCap";
            colNgayNhap.DataPropertyName = "NgayNhapText";
            colSoDauSach.DataPropertyName = "SoDauSach";
            colTongSoCuon.DataPropertyName = "TongSoCuon";
            colTongTien.DataPropertyName = "TongTienText";
            colTrangThai.DataPropertyName = "TrangThai";
            colNguoiLap.DataPropertyName = "NguoiLap";
            colHoTen.Visible = false;

            pnlChiTiet.BorderColor = Color.FromArgb(226, 232, 244);
            pnlChiTiet.BorderRadius = 12;
            pnlChiTiet.ShadowDecoration.Enabled = true;
            pnlChiTiet.ShadowDecoration.Depth = 6;
            pnlChiTiet.ShadowDecoration.Color = Color.FromArgb(145, 125, 220);
            guna2Panel2.FillColor = Color.FromArgb(224, 250, 231);
            guna2Panel2.BorderColor = Color.FromArgb(184, 235, 198);
            guna2Panel2.BorderRadius = 9;

            foreach (Guna.UI2.WinForms.Guna2Panel panel in new[] { pnlChuThich, pnlNhapGanDay, pnlNhaCungCapThuongXuyen })
            {
                panel.BorderColor = Color.FromArgb(226, 232, 244);
                panel.BorderThickness = 1;
                panel.BorderRadius = 10;
                panel.FillColor = Color.White;
            }

            dtpTuNgay.BackColor = Color.White;
            dtpDenNgay.BackColor = Color.White;
            dtpTuNgay.Format = dtpDenNgay.Format = DateTimePickerFormat.Custom;
            dtpTuNgay.CustomFormat = dtpDenNgay.CustomFormat = "dd/MM/yyyy";
            txtTrang.TextAlign = HorizontalAlignment.Center;
            btnGo.FillColor = Color.FromArgb(38, 100, 235);
            btnGo.ForeColor = Color.White;
            btnGo.BorderColor = Color.Transparent;

            btnTrangThaiPhieu.TabStop = false;
            btnTrangThaiPhieu.Cursor = Cursors.Default;
            guna2CircleButton2.Enabled = true;
            guna2CircleButton2.FillColor = Color.FromArgb(25, 160, 75);
            guna2CircleButton3.Enabled = true;
            guna2CircleButton3.FillColor = Color.FromArgb(30, 95, 220);
            guna2CircleButton4.Enabled = true;
            guna2CircleButton4.FillColor = Color.FromArgb(230, 120, 25);
            guna2CircleButton5.Enabled = true;
            guna2CircleButton5.FillColor = Color.FromArgb(225, 55, 65);

            HienThiChiTietRong();
            ResumeLayout(false);
        }

        private static void TrangTriTheThongKe(Guna.UI2.WinForms.Guna2Panel card)
        {
            card.BorderRadius = 14;
            card.BorderColor = Color.FromArgb(229, 233, 243);
            card.BorderThickness = 1;
            card.FillColor = Color.White;
            card.ShadowDecoration.Enabled = true;
            card.ShadowDecoration.Depth = 6;
            card.ShadowDecoration.Color = Color.FromArgb(145, 125, 220);
        }

        private static void TrangTriNut(Guna.UI2.WinForms.Guna2Button button, Color fill, Color foreground)
        {
            button.Animated = true;
            button.Cursor = Cursors.Hand;
            button.BorderRadius = 9;
            button.FillColor = fill;
            button.ForeColor = foreground;
            button.HoverState.FillColor = ControlPaint.Light(fill, 0.08F);
            button.HoverState.ForeColor = foreground;
        }


    }
}

