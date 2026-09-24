using BusinessLayer.Services;
using DataLayer.Models;
using Guna.UI2.WinForms;
using System.Text;
using Presentation.Helpers;

namespace Presentation.Models
{
    public partial class FrmLichSuMuonSachGanDay : Form
    {
        private readonly ThongKeService _thongKeService = null!;
        private List<LichSuMuonSachModel> _duLieuGoc = new();
        private List<LichSuMuonSachModel> _duLieuLoc = new();

        private int _trangHienTai = 1;
        private int _soDongMoiTrang = 8;
        private int _tongTrang = 1;
        private bool _dangKhoiTao;
        private readonly Label _lblDenTrang = new();
        private readonly TextBox _txtTrang = new();
        private readonly Label _lblTongTrang = new();
        private readonly Guna2Button _btnGoTrang = new();

        public FrmLichSuMuonSachGanDay()
        {
            InitializeComponent();

            if (DesignModeHelper.IsDesignMode(this))
                return;

            _thongKeService = new ThongKeService();

            CauHinhForm();
            CauHinhDataGridView();
            GanSuKien();
        }

        private void CauHinhForm()
        {
            KeyPreview = true;
            guna2DragControl1.TargetControl = pnlTitleBar;
            guna2ShadowForm1.SetShadowForm(this);

            TaoDieuKhienChuyenTrang();

            // Bố trí lại cụm phân trang cho đúng ảnh mẫu.
            flowSoTrang.BorderStyle = BorderStyle.None;
            flowSoTrang.FlowDirection = FlowDirection.LeftToRight;
            flowSoTrang.WrapContents = false;
            flowSoTrang.AutoSize = false;
            flowSoTrang.Size = new Size(42, 38);

            pnlPhanTrang.SizeChanged += (_, _) => CanChinhPhanTrang();
            CanChinhPhanTrang();
        }

        private void TaoDieuKhienChuyenTrang()
        {
            _lblDenTrang.Text = "Đến trang:";
            _lblDenTrang.AutoSize = true;
            _lblDenTrang.ForeColor = Color.FromArgb(35, 48, 90);
            _lblDenTrang.Font = new Font("Segoe UI", 8.5F);

            _txtTrang.Text = "1";
            _txtTrang.TextAlign = HorizontalAlignment.Center;
            _txtTrang.ForeColor = Color.FromArgb(35, 48, 90);
            _txtTrang.Font = new Font("Segoe UI", 8.5F);
            _txtTrang.Size = new Size(48, 34);
            _txtTrang.KeyDown += (_, e) =>
            {
                if (e.KeyCode != Keys.Enter) return;
                ChuyenDenTrangNhap();
                e.SuppressKeyPress = true;
            };
            _txtTrang.KeyPress += (_, e) =>
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                    e.Handled = true;
            };

            _lblTongTrang.Text = "/ 1";
            _lblTongTrang.AutoSize = true;
            _lblTongTrang.ForeColor = Color.FromArgb(35, 48, 90);
            _lblTongTrang.Font = new Font("Segoe UI", 8.5F);

            _btnGoTrang.Text = "Go";
            _btnGoTrang.Size = new Size(45, 34);
            _btnGoTrang.BorderRadius = 6;
            _btnGoTrang.BorderThickness = 1;
            _btnGoTrang.BorderColor = Color.FromArgb(170, 190, 235);
            _btnGoTrang.FillColor = Color.White;
            _btnGoTrang.ForeColor = Color.FromArgb(35, 85, 220);
            _btnGoTrang.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            _btnGoTrang.Click += (_, _) => ChuyenDenTrangNhap();

            pnlPhanTrang.Controls.Add(_lblDenTrang);
            pnlPhanTrang.Controls.Add(_txtTrang);
            pnlPhanTrang.Controls.Add(_lblTongTrang);
            pnlPhanTrang.Controls.Add(_btnGoTrang);
            _lblDenTrang.BringToFront();
            _txtTrang.BringToFront();
            _lblTongTrang.BringToFront();
            _btnGoTrang.BringToFront();
        }

        private void ChuyenDenTrangNhap()
        {
            if (!int.TryParse(_txtTrang.Text.Trim(), out int trang))
            {
                MessageBox.Show("Vui lòng nhập số trang hợp lệ.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtTrang.Focus();
                _txtTrang.SelectAll();
                return;
            }

            _trangHienTai = Math.Clamp(trang, 1, _tongTrang);
            HienThiTrang();
        }

        private void CanChinhPhanTrang()
        {
            const int y = 12;
            const int buttonWidth = 42;
            const int gap = 3;
            int right = pnlPhanTrang.ClientSize.Width - 22;

            btnTrangCuoi.Location = new Point(right - buttonWidth, y);
            btnTrangSau.Location = new Point(btnTrangCuoi.Left - gap - buttonWidth, y);

            int flowWidth = Math.Max(58, Math.Min(190, flowSoTrang.Controls.Count * 62));
            flowSoTrang.Size = new Size(flowWidth, 38);
            flowSoTrang.Location = new Point(btnTrangSau.Left - gap - flowWidth, y);

            btnTrangTruoc.Location = new Point(flowSoTrang.Left - gap - buttonWidth, y);
            btnTrangDau.Location = new Point(btnTrangTruoc.Left - gap - buttonWidth, y);

            _btnGoTrang.Location = new Point(btnTrangDau.Left - 7 - _btnGoTrang.Width, y + 1);
            _lblTongTrang.Location = new Point(_btnGoTrang.Left - 55, y + 7);
            _txtTrang.Location = new Point(_lblTongTrang.Left - 52, y + 1);
            _lblDenTrang.Location = new Point(_txtTrang.Left - 77, y + 7);
            cboSoDong.Location = new Point(_lblDenTrang.Left - 8 - cboSoDong.Width, y + 1);
        }

        private void GanSuKien()
        {
            Load -= FrmLichSuMuonSachGanDay_Load;
            Load += FrmLichSuMuonSachGanDay_Load;

            Shown -= FrmLichSuMuonSachGanDay_Shown;
            Shown += FrmLichSuMuonSachGanDay_Shown;

            btnTimKiem.Click -= btnTimKiem_Click;
            btnTimKiem.Click += btnTimKiem_Click;

            btnLamMoi.Click -= btnLamMoi_Click;
            btnLamMoi.Click += btnLamMoi_Click;

            btnXuatExcel.Click -= btnXuatExcel_Click;
            btnXuatExcel.Click += btnXuatExcel_Click;

            btnDong.Click -= btnDong_Click;
            btnDong.Click += btnDong_Click;

            btnTrangDau.Click -= btnTrangDau_Click;
            btnTrangDau.Click += btnTrangDau_Click;

            btnTrangTruoc.Click -= btnTrangTruoc_Click;
            btnTrangTruoc.Click += btnTrangTruoc_Click;

            btnTrangSau.Click -= btnTrangSau_Click;
            btnTrangSau.Click += btnTrangSau_Click;

            // Designer cũ đã gán btnNext_Click; bỏ và gán lại đúng chức năng.
            btnTrangCuoi.Click -= btnNext_Click;
            btnTrangCuoi.Click -= btnTrangCuoi_Click;
            btnTrangCuoi.Click += btnTrangCuoi_Click;

            cboSoDong.SelectedIndexChanged -= cboSoDong_SelectedIndexChanged;
            cboSoDong.SelectedIndexChanged += cboSoDong_SelectedIndexChanged;

            cboTrangThai.SelectedIndexChanged -= cboTrangThai_SelectedIndexChanged;
            cboTrangThai.SelectedIndexChanged += cboTrangThai_SelectedIndexChanged;

            txtTuKhoa.KeyDown -= txtTuKhoa_KeyDown;
            txtTuKhoa.KeyDown += txtTuKhoa_KeyDown;

            dgvLichSuMuon.CellPainting -= dgvLichSuMuon_CellPainting;
            dgvLichSuMuon.CellPainting += dgvLichSuMuon_CellPainting;

            dgvLichSuMuon.CellFormatting -= dgvLichSuMuon_CellFormatting;
            dgvLichSuMuon.CellFormatting += dgvLichSuMuon_CellFormatting;

            dgvLichSuMuon.CellDoubleClick -= dgvLichSuMuon_CellDoubleClick;
            dgvLichSuMuon.CellDoubleClick += dgvLichSuMuon_CellDoubleClick;

            KeyDown -= FrmLichSuMuonSachGanDay_KeyDown;
            KeyDown += FrmLichSuMuonSachGanDay_KeyDown;
        }

        private void FrmLichSuMuonSachGanDay_Load(object? sender, EventArgs e)
        {
            _dangKhoiTao = true;

            var khoangNgay = _thongKeService.GetBorrowHistoryDateRange();
            // Hiển thị toàn bộ lịch sử từ ngày giao dịch đầu tiên đến đúng ngày hôm nay.
            // Không dùng ngày giao dịch cuối cùng làm "Đến ngày", vì dữ liệu có thể cũ hơn ngày máy.
            dtpTuNgay.Value = khoangNgay?.TuNgay ?? DateTime.Today.AddDays(-7);
            dtpDenNgay.Value = DateTime.Today;

            cboTrangThai.Items.Clear();
            cboTrangThai.Items.AddRange(new object[]
            {
                "-- Tất cả --",
                "Đang mượn",
                "Đã trả",
                "Quá hạn"
            });
            cboTrangThai.SelectedIndex = 0;

            cboSoDong.Items.Clear();
            cboSoDong.Items.AddRange(new object[]
            {
                "5 / trang",
                "8 / trang",
                "10 / trang",
                "15 / trang"
            });
            cboSoDong.SelectedIndex = 1;

            _dangKhoiTao = false;
        }

        private void FrmLichSuMuonSachGanDay_Shown(object? sender, EventArgs e)
        {
            // Luôn truy vấn lại dữ liệu mới nhất khi form đã mở và sẵn sàng hiển thị.
            TaiDuLieu();
        }

        private void CauHinhDataGridView()
        {
            dgvLichSuMuon.AutoGenerateColumns = false;
            dgvLichSuMuon.AllowUserToAddRows = false;
            dgvLichSuMuon.AllowUserToDeleteRows = false;
            dgvLichSuMuon.AllowUserToResizeRows = false;
            dgvLichSuMuon.AllowUserToOrderColumns = false;
            dgvLichSuMuon.ReadOnly = true;
            dgvLichSuMuon.RowHeadersVisible = false;
            dgvLichSuMuon.MultiSelect = false;
            dgvLichSuMuon.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvLichSuMuon.EnableHeadersVisualStyles = false;
            dgvLichSuMuon.ColumnHeadersHeight = 42;
            dgvLichSuMuon.RowTemplate.Height = 71;
            dgvLichSuMuon.BackgroundColor = Color.White;
            dgvLichSuMuon.GridColor = Color.FromArgb(225, 230, 240);

            dgvLichSuMuon.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(245, 247, 252);
            dgvLichSuMuon.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(60, 70, 90);
            dgvLichSuMuon.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgvLichSuMuon.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgvLichSuMuon.DefaultCellStyle.BackColor = Color.FromArgb(245, 247, 252);
            dgvLichSuMuon.DefaultCellStyle.ForeColor = Color.FromArgb(60, 70, 90);
            dgvLichSuMuon.DefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgvLichSuMuon.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvLichSuMuon.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvLichSuMuon.DefaultCellStyle.SelectionBackColor = Color.FromArgb(245, 247, 252);
            dgvLichSuMuon.DefaultCellStyle.SelectionForeColor = Color.FromArgb(60, 70, 90);
            dgvLichSuMuon.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(251, 252, 255);

            colSTT.DataPropertyName = nameof(LichSuMuonGridRow.STT);
            colMaPhieuMuon.DataPropertyName = nameof(LichSuMuonGridRow.MaPhieuMuonText);
            colNgayMuon.DataPropertyName = nameof(LichSuMuonGridRow.NgayMuonText);
            colDocGia.DataPropertyName = nameof(LichSuMuonGridRow.DocGiaText);
            colSach.DataPropertyName = nameof(LichSuMuonGridRow.SachText);
            colNgayHenTra.DataPropertyName = nameof(LichSuMuonGridRow.NgayHenTraText);
            colNgayTra.DataPropertyName = nameof(LichSuMuonGridRow.NgayTraText);
            colTrangThai.DataPropertyName = nameof(LichSuMuonGridRow.TrangThai);
            colSoNgayMuon.DataPropertyName = nameof(LichSuMuonGridRow.SoNgayMuonText);

            colDocGia.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            colSach.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            colNgayMuon.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            colNgayTra.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
        }

        private void TaiDuLieu()
        {
            if (dtpTuNgay.Value.Date > dtpDenNgay.Value.Date)
            {
                MessageBox.Show(
                    "Từ ngày không được lớn hơn đến ngày.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            try
            {
                BatTrangThaiTai(true);

                _duLieuGoc = _thongKeService.GetLichSuMuonSach(
                    dtpTuNgay.Value.Date,
                    dtpDenNgay.Value.Date);

                LocDuLieu();
            }
            catch (Exception ex)
            {
                _duLieuGoc.Clear();
                _duLieuLoc.Clear();
                HienThiTrang();

                MessageBox.Show(
                    "Không thể tải lịch sử mượn sách."
                    + Environment.NewLine
                    + Environment.NewLine
                    + ex.Message,
                    "Lỗi dữ liệu",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                BatTrangThaiTai(false);
            }
        }

        private void BatTrangThaiTai(bool dangTai)
        {
            Cursor = dangTai ? Cursors.WaitCursor : Cursors.Default;
            btnTimKiem.Enabled = !dangTai;
            btnLamMoi.Enabled = !dangTai;
            btnXuatExcel.Enabled = !dangTai;
            dtpTuNgay.Enabled = !dangTai;
            dtpDenNgay.Enabled = !dangTai;
        }

        private void LocDuLieu()
        {
            IEnumerable<LichSuMuonSachModel> query = _duLieuGoc;

            string trangThai = cboTrangThai.SelectedItem?.ToString() ?? "-- Tất cả --";
            if (trangThai != "-- Tất cả --")
            {
                query = query.Where(x => string.Equals(
                    x.TrangThai,
                    trangThai,
                    StringComparison.CurrentCultureIgnoreCase));
            }

            string tuKhoa = txtTuKhoa.Text.Trim();
            if (!string.IsNullOrWhiteSpace(tuKhoa))
            {
                query = query.Where(x =>
                    ChuaTuKhoa(x.MaPhieuMuonText, tuKhoa)
                    || ChuaTuKhoa(x.TenDocGia, tuKhoa)
                    || ChuaTuKhoa(x.MaDocGia, tuKhoa)
                    || ChuaTuKhoa(x.TenSach, tuKhoa)
                    || ChuaTuKhoa(x.MaSach, tuKhoa));
            }

            _duLieuLoc = query
                .OrderByDescending(x => x.NgayMuon)
                .ThenByDescending(x => x.MaPhieuMuon)
                .ToList();

            _trangHienTai = 1;
            HienThiTrang();
        }

        private static bool ChuaTuKhoa(string? source, string keyword)
        {
            return !string.IsNullOrWhiteSpace(source)
                && source.Contains(keyword, StringComparison.CurrentCultureIgnoreCase);
        }

        private void HienThiTrang()
        {
            int tongBanGhi = _duLieuLoc.Count;
            _tongTrang = Math.Max(1, (int)Math.Ceiling(tongBanGhi / (double)_soDongMoiTrang));
            _trangHienTai = Math.Max(1, Math.Min(_trangHienTai, _tongTrang));

            List<LichSuMuonGridRow> data = _duLieuLoc
                .Skip((_trangHienTai - 1) * _soDongMoiTrang)
                .Take(_soDongMoiTrang)
                .Select((item, index) => new LichSuMuonGridRow
                {
                    STT = (_trangHienTai - 1) * _soDongMoiTrang + index + 1,
                    MaPhieuMuon = item.MaPhieuMuon,
                    MaPhieuMuonText = item.MaPhieuMuonText,
                    NgayMuonText = item.NgayMuon.HasValue ? item.NgayMuon.Value.ToString("dd/MM/yyyy") : "-",
                    DocGiaText = item.TenDocGia + Environment.NewLine + item.MaDocGia,
                    SachText = item.TenSach + Environment.NewLine + item.MaSach,
                    NgayHenTraText = item.HanTra.HasValue ? item.HanTra.Value.ToString("dd/MM/yyyy") : "-",
                    NgayTraText = item.NgayTra.HasValue ? item.NgayTra.Value.ToString("dd/MM/yyyy") : "-",
                    TrangThai = item.TrangThai,
                    SoNgayMuonText = $"{item.SoNgayMuon} ngày"
                })
                .ToList();

            dgvLichSuMuon.DataSource = null;
            dgvLichSuMuon.DataSource = data;
            dgvLichSuMuon.ClearSelection();

            int batDau = tongBanGhi == 0 ? 0 : (_trangHienTai - 1) * _soDongMoiTrang + 1;
            int ketThuc = Math.Min(_trangHienTai * _soDongMoiTrang, tongBanGhi);
            lblTongSo.Text = $"Hiển thị {batDau:N0}–{ketThuc:N0} của {tongBanGhi:N0} phiếu mượn";
            _txtTrang.Text = _trangHienTai.ToString();
            _lblTongTrang.Text = $"/ {_tongTrang:N0}";
            _btnGoTrang.Enabled = tongBanGhi > 0;

            bool coTrangTruoc = _trangHienTai > 1;
            bool coTrangSau = _trangHienTai < _tongTrang;
            btnTrangDau.Enabled = coTrangTruoc;
            btnTrangTruoc.Enabled = coTrangTruoc;
            btnTrangSau.Enabled = coTrangSau;
            btnTrangCuoi.Enabled = coTrangSau;

            TaoNutSoTrang();
        }

        private void TaoNutSoTrang()
        {
            foreach (Control control in flowSoTrang.Controls)
            {
                control.Dispose();
            }
            flowSoTrang.Controls.Clear();

            const int soNutToiDa = 3;
            int batDau = Math.Max(1, _trangHienTai - soNutToiDa / 2);
            int ketThuc = Math.Min(_tongTrang, batDau + soNutToiDa - 1);
            batDau = Math.Max(1, ketThuc - soNutToiDa + 1);

            for (int trang = batDau; trang <= ketThuc; trang++)
            {
                int soTrang = trang;
                Guna2Button button = new Guna2Button
                {
                    Text = soTrang.ToString(),
                    Size = new Size(58, 38),
                    BorderRadius = 6,
                    Margin = new Padding(0, 0, 4, 0),
                    Font = TaoFontNutTrang(soTrang)
                };

                if (soTrang == _trangHienTai)
                {
                    button.FillColor = Color.FromArgb(35, 85, 220);
                    button.ForeColor = Color.White;
                    button.BorderThickness = 0;
                }
                else
                {
                    button.FillColor = Color.White;
                    button.ForeColor = Color.FromArgb(35, 55, 110);
                    button.BorderColor = Color.FromArgb(215, 222, 235);
                    button.BorderThickness = 1;
                }

                button.Click += (_, _) =>
                {
                    _trangHienTai = soTrang;
                    HienThiTrang();
                };

                flowSoTrang.Controls.Add(button);
            }

            CanChinhPhanTrang();
        }

        private static Font TaoFontNutTrang(int page)
        {
            int length = page.ToString().Length;
            float size = length >= 5 ? 6.5F : length == 4 ? 7F : length == 3 ? 8F : 9F;
            return new Font("Segoe UI", size, FontStyle.Bold);
        }

        private void btnTrangDau_Click(object? sender, EventArgs e)
        {
            _trangHienTai = 1;
            HienThiTrang();
        }

        private void btnTrangTruoc_Click(object? sender, EventArgs e)
        {
            if (_trangHienTai <= 1) return;
            _trangHienTai--;
            HienThiTrang();
        }

        private void btnTrangSau_Click(object? sender, EventArgs e)
        {
            if (_trangHienTai >= _tongTrang) return;
            _trangHienTai++;
            HienThiTrang();
        }

        private void btnTrangCuoi_Click(object? sender, EventArgs e)
        {
            _trangHienTai = _tongTrang;
            HienThiTrang();
        }

        private void cboSoDong_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (_dangKhoiTao) return;

            string text = cboSoDong.SelectedItem?.ToString() ?? "10";
            if (!int.TryParse(text.Split(' ')[0], out int soDong)) return;

            _soDongMoiTrang = soDong;
            _trangHienTai = 1;
            HienThiTrang();
        }

        private void btnTimKiem_Click(object? sender, EventArgs e)
        {
            TaiDuLieu();
        }

        private void btnLamMoi_Click(object? sender, EventArgs e)
        {
            _dangKhoiTao = true;
            var khoangNgay = _thongKeService.GetBorrowHistoryDateRange();
            // Hiển thị toàn bộ lịch sử từ ngày giao dịch đầu tiên đến đúng ngày hôm nay.
            // Không dùng ngày giao dịch cuối cùng làm "Đến ngày", vì dữ liệu có thể cũ hơn ngày máy.
            dtpTuNgay.Value = khoangNgay?.TuNgay ?? DateTime.Today.AddDays(-7);
            dtpDenNgay.Value = DateTime.Today;
            cboTrangThai.SelectedIndex = 0;
            cboSoDong.SelectedIndex = 1;
            txtTuKhoa.Clear();
            _dangKhoiTao = false;

            _soDongMoiTrang = 8;
            _trangHienTai = 1;
            TaiDuLieu();
        }

        private void cboTrangThai_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (_dangKhoiTao || !IsHandleCreated) return;
            LocDuLieu();
        }

        private void txtTuKhoa_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            TaiDuLieu();
            e.SuppressKeyPress = true;
        }

        private void dgvLichSuMuon_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            string tenCot = dgvLichSuMuon.Columns[e.ColumnIndex].Name;
            if (tenCot == "colMaPhieuMuon")
            {
                e.CellStyle.ForeColor = Color.FromArgb(30, 95, 230);
                e.CellStyle.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            }
            else if (tenCot == "colDocGia" || tenCot == "colSach")
            {
                e.CellStyle.WrapMode = DataGridViewTriState.True;
            }
        }

        private void dgvLichSuMuon_CellPainting(object? sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            if (dgvLichSuMuon.Columns[e.ColumnIndex].Name != "colTrangThai") return;

            string trangThai = e.FormattedValue?.ToString() ?? string.Empty;
            e.PaintBackground(e.CellBounds, true);

            (Color mauNen, Color mauChu) = trangThai switch
            {
                "Đang mượn" => (Color.FromArgb(235, 250, 240), Color.FromArgb(25, 155, 80)),
                "Đã trả" => (Color.FromArgb(235, 243, 255), Color.FromArgb(35, 105, 220)),
                "Quá hạn" => (Color.FromArgb(255, 238, 238), Color.FromArgb(235, 55, 65)),
                _ => (Color.FromArgb(242, 244, 248), Color.FromArgb(85, 95, 120))
            };

            using Font font = new Font("Segoe UI", 8F, FontStyle.Bold);
            Size textSize = TextRenderer.MeasureText(trangThai, font);
            int width = Math.Min(e.CellBounds.Width - 12, textSize.Width + 24);
            int height = 26;
            Rectangle badge = new Rectangle(
                e.CellBounds.X + (e.CellBounds.Width - width) / 2,
                e.CellBounds.Y + (e.CellBounds.Height - height) / 2,
                width,
                height);

            using SolidBrush brush = new SolidBrush(mauNen);
            e.Graphics.FillRectangle(brush, badge);
            TextRenderer.DrawText(
                e.Graphics,
                trangThai,
                font,
                badge,
                mauChu,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);

            e.Handled = true;
        }

        private void dgvLichSuMuon_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (dgvLichSuMuon.Rows[e.RowIndex].DataBoundItem is not LichSuMuonGridRow row) return;

            MessageBox.Show(
                $"Mã phiếu: {row.MaPhieuMuonText}"
                + Environment.NewLine
                + $"Độc giả: {row.DocGiaText.Replace(Environment.NewLine, " - ")}"
                + Environment.NewLine
                + $"Sách: {row.SachText.Replace(Environment.NewLine, " - ")}"
                + Environment.NewLine
                + $"Ngày mượn: {row.NgayMuonText}"
                + Environment.NewLine
                + $"Hạn trả: {row.NgayHenTraText}"
                + Environment.NewLine
                + $"Ngày trả: {row.NgayTraText}"
                + Environment.NewLine
                + $"Trạng thái: {row.TrangThai}",
                "Chi tiết phiếu mượn",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        // Xuất CSV UTF-8; Microsoft Excel mở trực tiếp và giữ đúng tiếng Việt.
        private void btnXuatExcel_Click(object? sender, EventArgs e)
        {
            if (_duLieuLoc.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using SaveFileDialog dialog = new SaveFileDialog
            {
                Filter = "Excel CSV (*.csv)|*.csv",
                FileName = $"LichSuMuonSach_{DateTime.Now:ddMMyyyy_HHmm}.csv",
                AddExtension = true,
                DefaultExt = "csv"
            };

            if (dialog.ShowDialog(this) != DialogResult.OK) return;

            try
            {
                StringBuilder csv = new StringBuilder();
                csv.AppendLine("STT;Mã phiếu mượn;Ngày mượn;Mã độc giả;Độc giả;Mã sách;Sách;Ngày hẹn trả;Ngày trả;Trạng thái;Số ngày mượn");

                for (int i = 0; i < _duLieuLoc.Count; i++)
                {
                    LichSuMuonSachModel item = _duLieuLoc[i];
                    string[] values =
                    {
                        (i + 1).ToString(),
                        item.MaPhieuMuonText,
                        item.NgayMuon?.ToString("dd/MM/yyyy") ?? "",
                        item.MaDocGia,
                        item.TenDocGia,
                        item.MaSach,
                        item.TenSach,
                        item.HanTra?.ToString("dd/MM/yyyy") ?? "",
                        item.NgayTra?.ToString("dd/MM/yyyy") ?? "",
                        item.TrangThai,
                        item.SoNgayMuon.ToString()
                    };

                    csv.AppendLine(string.Join(";", values.Select(EscapeCsv)));
                }

                File.WriteAllText(dialog.FileName, csv.ToString(), new UTF8Encoding(true));

                MessageBox.Show(
                    "Xuất dữ liệu thành công. File có thể mở trực tiếp bằng Microsoft Excel.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể xuất dữ liệu."
                    + Environment.NewLine
                    + Environment.NewLine
                    + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private static string EscapeCsv(string? value)
        {
            string text = value ?? string.Empty;
            if (text.Contains(';') || text.Contains('"') || text.Contains('\n') || text.Contains('\r'))
            {
                return $"\"{text.Replace("\"", "\"\"")}\"";
            }
            return text;
        }

        private void btnDong_Click(object? sender, EventArgs e)
        {
            Close();
        }

        private void FrmLichSuMuonSachGanDay_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Close();
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.F5)
            {
                TaiDuLieu();
                e.Handled = true;
            }
            else if (e.Control && e.KeyCode == Keys.F)
            {
                txtTuKhoa.Focus();
                txtTuKhoa.SelectAll();
                e.SuppressKeyPress = true;
            }
            else if (e.KeyCode == Keys.Left && _trangHienTai > 1)
            {
                _trangHienTai--;
                HienThiTrang();
            }
            else if (e.KeyCode == Keys.Right && _trangHienTai < _tongTrang)
            {
                _trangHienTai++;
                HienThiTrang();
            }
        }

        // Giữ để Designer cũ không báo lỗi sự kiện; chức năng thật đã gán ở GanSuKien().
        private void btnNext_Click(object sender, EventArgs e)
        {
        }
    }

    public sealed class LichSuMuonGridRow
    {
        public int STT { get; set; }
        public int MaPhieuMuon { get; set; }
        public string MaPhieuMuonText { get; set; } = string.Empty;
        public string NgayMuonText { get; set; } = string.Empty;
        public string DocGiaText { get; set; } = string.Empty;
        public string SachText { get; set; } = string.Empty;
        public string NgayHenTraText { get; set; } = string.Empty;
        public string NgayTraText { get; set; } = string.Empty;
        public string TrangThai { get; set; } = string.Empty;
        public string SoNgayMuonText { get; set; } = string.Empty;
    }
}

