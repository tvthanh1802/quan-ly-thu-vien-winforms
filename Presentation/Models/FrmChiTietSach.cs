using BusinessLayer.Services;
using DataLayer.Models;
using Guna.UI2.WinForms;
using Presentation.Helpers;
using System.Globalization;
using System.ComponentModel;

namespace Presentation.Models
{
    public partial class FrmChiTietSach : Form
    {
        private readonly int _maSach;
        private readonly SachService _sachService;
        private ChiTietSachModel? _duLieu;
        private Image? _anhBiaDaTai;
        private Guna2DataGridView? _dgvLichSuMuon;
        private Guna2DataGridView? _dgvDanhGia;
        private bool _dangTaiDuLieu;

        public FrmChiTietSach() : this(0)
        {
        }

        public FrmChiTietSach(int maSach)
        {
            InitializeComponent();
            _maSach = maSach;
            _sachService = new SachService();

            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            CauHinhForm();
            TaoBangTabBoSung();
            GanSuKien();
        }

        private void CauHinhForm()
        {
            KeyPreview = true;
            guna2DragControl1.TargetControl = pnlTitleBar;
            guna2ShadowForm1.SetShadowForm(this);

            dgvDocGia.Name = "dgvTacGia";
            dgvDocGia.AutoGenerateColumns = false;
            guna2DataGridView1.Name = "dgvBanSao";
            guna2DataGridView1.AutoGenerateColumns = false;
            guna2DataGridView1.RowTemplate.Height = 71;
            guna2DataGridView1.ColumnHeadersHeight = 38;
            guna2DataGridView1.CellPainting += dgvBanSao_CellPainting;

            txtMoTa.ReadOnly = true;
            txtMoTa.Multiline = true;
            txtMoTa.ScrollBars = ScrollBars.Vertical;
        }

        private void TaoBangTabBoSung()
        {
            _dgvLichSuMuon = TaoDataGridViewTab("dgvLichSuMuon");
            _dgvLichSuMuon.Columns.AddRange(
                TaoCot("lsSTT", "STT", "STT", 55),
                TaoCot("lsMaPhieu", "Mã phiếu", "MaPhieuText", 110),
                TaoCot("lsDocGia", "Độc giả", "TenDocGia", 175),
                TaoCot("lsNgayMuon", "Ngày mượn", "NgayMuonText", 115),
                TaoCot("lsHanTra", "Hạn trả", "HanTraText", 115),
                TaoCot("lsNgayTra", "Ngày trả", "NgayTraText", 115),
                TaoCotFill("lsTrangThai", "Trạng thái", "TrangThai"));

            _dgvDanhGia = TaoDataGridViewTab("dgvDanhGia");
            _dgvDanhGia.Columns.AddRange(
                TaoCot("dgSTT", "STT", "STT", 55),
                TaoCot("dgDocGia", "Độc giả", "TenDocGia", 180),
                TaoCot("dgSoSao", "Số sao", "SoSaoText", 90),
                TaoCotFill("dgNoiDung", "Nội dung", "NoiDung"),
                TaoCot("dgNgay", "Ngày đánh giá", "NgayDanhGiaText", 135));

            pnlTabs.Controls.Add(_dgvLichSuMuon);
            pnlTabs.Controls.Add(_dgvDanhGia);
            _dgvLichSuMuon.BringToFront();
            _dgvDanhGia.BringToFront();
            guna2DataGridView1.BringToFront();
            pnlTabIndicator.BringToFront();
            btnTabBanSao.BringToFront();
            btnTabLichSuMuon.BringToFront();
            btnTabDanhGia.BringToFront();
        }

        private Guna2DataGridView TaoDataGridViewTab(string name)
        {
            var dgv = new Guna2DataGridView
            {
                Name = name,
                Location = guna2DataGridView1.Location,
                Size = guna2DataGridView1.Size,
                Anchor = guna2DataGridView1.Anchor,
                AutoGenerateColumns = false,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                RowHeadersVisible = false,
                MultiSelect = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                BackgroundColor = Color.White,
                GridColor = Color.FromArgb(230, 235, 243),
                ColumnHeadersHeight = 38,
                RowTemplate = { Height = 36 },
                Visible = false
            };

            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(245, 247, 252);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(60, 70, 90);
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgv.EnableHeadersVisualStyles = false;
            dgv.DefaultCellStyle.BackColor = Color.FromArgb(245, 247, 252);
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgv.DefaultCellStyle.ForeColor = Color.FromArgb(60, 70, 90);
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(245, 247, 252);
            dgv.DefaultCellStyle.SelectionForeColor = Color.FromArgb(60, 70, 90);
            dgv.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgv.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            return dgv;
        }

        private static DataGridViewTextBoxColumn TaoCot(
            string name, string header, string property, int width)
        {
            return new DataGridViewTextBoxColumn
            {
                Name = name,
                HeaderText = header,
                DataPropertyName = property,
                Width = width,
                ReadOnly = true,
                SortMode = DataGridViewColumnSortMode.NotSortable
            };
        }

        private static DataGridViewTextBoxColumn TaoCotFill(
            string name, string header, string property)
        {
            return new DataGridViewTextBoxColumn
            {
                Name = name,
                HeaderText = header,
                DataPropertyName = property,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                ReadOnly = true,
                SortMode = DataGridViewColumnSortMode.NotSortable
            };
        }

        private void GanSuKien()
        {
            Load += FrmChiTietSach_Load;
            btnDong.Click += (_, _) => Close();
            btnXemAnhLon.Click += btnXemAnhLon_Click;
            btnTabBanSao.Click += (_, _) => ChonTab(guna2DataGridView1, btnTabBanSao);
            btnTabLichSuMuon.Click += (_, _) => ChonTab(_dgvLichSuMuon!, btnTabLichSuMuon);
            btnTabDanhGia.Click += (_, _) => ChonTab(_dgvDanhGia!, btnTabDanhGia);
            KeyDown += FrmChiTietSach_KeyDown;
            FormClosed += (_, _) => GiaiPhongAnh();
        }

        private void FrmChiTietSach_Load(object? sender, EventArgs e)
        {
            if (_maSach <= 0)
            {
                if (!DesignMode)
                    Text = "Chi tiết sách - chưa chọn sách";
                return;
            }

            TaiDuLieu();
        }

        private void TaiDuLieu()
        {
            if (_dangTaiDuLieu) return;

            try
            {
                _dangTaiDuLieu = true;
                Cursor = Cursors.WaitCursor;
                _duLieu = _sachService.LayChiTietDayDu(_maSach);

                if (_duLieu == null)
                {
                    MessageBox.Show("Không tìm thấy đầu sách cần xem.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Close();
                    return;
                }

                HienThiThongTin(_duLieu);
                HienThiTacGia(_duLieu.TacGias);
                HienThiBanSao(_duLieu.BanSaos);
                HienThiLichSuMuon(_duLieu.LichSuMuons);
                HienThiDanhGia(_duLieu.DanhGias);
                ChonTab(guna2DataGridView1, btnTabBanSao);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể tải chi tiết sách.\n\n" + ex.Message,
                    "Lỗi dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _dangTaiDuLieu = false;
                Cursor = Cursors.Default;
            }
        }

        private void HienThiThongTin(ChiTietSachModel model)
        {
            Text = $"Chi tiết sách - {model.TenSach}";
            lblMaSach.Text = model.MaSachText;
            lblMaSachHienThi.Text = model.MaSachHienThi;
            lblTenSach.Text = model.TenSach;
            lblISBN.Text = model.Isbn;
            lblTheLoai.Text = model.TheLoai;
            lblNhaXuatBan.Text = model.NhaXuatBan;
            lblNamXuatBan.Text = model.NamXuatBan?.ToString() ?? "Chưa cập nhật";
            lblNgonNgu.Text = model.NgonNgu;
            lblSoTrang.Text = model.SoTrang?.ToString("N0") ?? "Chưa cập nhật";
            lblGiaBia.Text = model.GiaBia.HasValue
                ? model.GiaBia.Value.ToString("N0", CultureInfo.GetCultureInfo("vi-VN")) + " đ"
                : "Chưa cập nhật";
            txtMoTa.Text = model.MoTa;

            lblNgayThem.Text = $"<b>Ngày thêm:</b> {(model.NgayThem.HasValue ? model.NgayThem.Value.ToString("dd/MM/yyyy") : "-")}";
            lblCapNhatCuoi.Text = $"<b>Cập nhật cuối:</b> {(model.NgayCapNhat.HasValue ? model.NgayCapNhat.Value.ToString("dd/MM/yyyy HH:mm") : "-")}";
            labNguoiCapNhat.Text = $"<b>Người cập nhật:</b> {model.NguoiCapNhat}";

            CapNhatTrangThai(model.TrangThai);
            TaiAnhBia(model.AnhBia, model.MaSach);
        }

        private void HienThiTacGia(IReadOnlyList<TacGiaChiTietModel> data)
        {
            dgvDocGia.DataSource = data.Select((item, index) => new
            {
                STT = index + 1,
                item.TenTacGia,
                item.VaiTro
            }).ToList();
            dgvDocGia.ClearSelection();
        }

        private void HienThiBanSao(IReadOnlyList<BanSaoSachModel> data)
        {
            guna2DataGridView1.DataSource = data.Select((item, index) => new
            {
                STT = index + 1,
                item.MaCuonText,
                item.MaVach,
                item.ViTri,
                item.TinhTrang,
                item.TrangThai,
                NgayNhapText = item.NgayNhap?.ToString("dd/MM/yyyy") ?? "-"
            }).ToList();
            btnTabBanSao.Text = $"BẢN SAO ({data.Count})";
            guna2DataGridView1.ClearSelection();
        }

        private void HienThiLichSuMuon(IReadOnlyList<LichSuMuonSachChiTietModel> data)
        {
            if (_dgvLichSuMuon == null) return;
            _dgvLichSuMuon.DataSource = data.Select((item, index) => new
            {
                STT = index + 1,
                item.MaPhieuText,
                item.TenDocGia,
                NgayMuonText = item.NgayMuon?.ToString("dd/MM/yyyy HH:mm") ?? "-",
                HanTraText = item.HanTra?.ToString("dd/MM/yyyy") ?? "-",
                NgayTraText = item.NgayTra?.ToString("dd/MM/yyyy HH:mm") ?? "-",
                item.TrangThai
            }).ToList();
            btnTabLichSuMuon.Text = $"LỊCH SỬ MƯỢN ({data.Count})";
            _dgvLichSuMuon.ClearSelection();
        }

        private void HienThiDanhGia(IReadOnlyList<DanhGiaSachChiTietModel> data)
        {
            if (_dgvDanhGia == null) return;
            _dgvDanhGia.DataSource = data.Select((item, index) => new
            {
                STT = index + 1,
                item.TenDocGia,
                SoSaoText = new string('★', Math.Clamp(item.SoSao, 0, 5)) + $" ({item.SoSao}/5)",
                item.NoiDung,
                NgayDanhGiaText = item.NgayDanhGia?.ToString("dd/MM/yyyy HH:mm") ?? "-"
            }).ToList();
            btnTabDanhGia.Text = $"ĐÁNH GIÁ ({data.Count})";
            _dgvDanhGia.ClearSelection();
        }

        private void ChonTab(DataGridView bangHienThi, Guna2Button nutDangChon)
        {
            guna2DataGridView1.Visible = ReferenceEquals(bangHienThi, guna2DataGridView1);
            if (_dgvLichSuMuon != null)
                _dgvLichSuMuon.Visible = ReferenceEquals(bangHienThi, _dgvLichSuMuon);
            if (_dgvDanhGia != null)
                _dgvDanhGia.Visible = ReferenceEquals(bangHienThi, _dgvDanhGia);

            bangHienThi.BringToFront();
            pnlTabIndicator.Left = nutDangChon.Left;
            pnlTabIndicator.Width = nutDangChon.Width;
            pnlTabIndicator.BringToFront();

            foreach (Guna2Button button in new[] { btnTabBanSao, btnTabLichSuMuon, btnTabDanhGia })
            {
                bool active = ReferenceEquals(button, nutDangChon);
                button.ForeColor = active
                    ? Color.FromArgb(30, 90, 225)
                    : Color.FromArgb(55, 65, 95);
                button.Font = new Font("Segoe UI", 9F,
                    active ? FontStyle.Bold : FontStyle.Regular);
                button.BringToFront();
            }
        }

        private void CapNhatTrangThai(bool dangHoatDong)
        {
            btnTrangThaiSach.Enabled = false;
            btnTrangThaiSach.Text = dangHoatDong ? "Đang hoạt động" : "Ngừng hoạt động";

            Color fill = dangHoatDong
                ? Color.FromArgb(232, 248, 237)
                : Color.FromArgb(255, 238, 238);
            Color fore = dangHoatDong
                ? Color.FromArgb(25, 145, 75)
                : Color.FromArgb(225, 55, 65);
            Color border = dangHoatDong
                ? Color.FromArgb(175, 225, 190)
                : Color.FromArgb(240, 180, 185);

            btnTrangThaiSach.FillColor = fill;
            btnTrangThaiSach.ForeColor = fore;
            btnTrangThaiSach.BorderColor = border;
            btnTrangThaiSach.DisabledState.FillColor = fill;
            btnTrangThaiSach.DisabledState.ForeColor = fore;
            btnTrangThaiSach.DisabledState.BorderColor = border;
        }

        private void dgvBanSao_CellPainting(object? sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0 ||
                guna2DataGridView1.Columns[e.ColumnIndex].Name != "colTrangThaiCuon")
                return;

            if (e.Graphics == null) return;

            string trangThai = e.FormattedValue?.ToString() ?? string.Empty;
            e.PaintBackground(e.CellBounds, true);

            (Color background, Color foreground) = trangThai switch
            {
                "Có sẵn" or "Sẵn sàng" =>
                    (Color.FromArgb(232, 248, 237), Color.FromArgb(25, 150, 75)),
                "Đang mượn" =>
                    (Color.FromArgb(255, 246, 230), Color.FromArgb(235, 125, 20)),
                "Mất" or "Hỏng" =>
                    (Color.FromArgb(255, 238, 238), Color.FromArgb(230, 55, 65)),
                _ =>
                    (Color.FromArgb(240, 242, 246), Color.FromArgb(85, 95, 115))
            };

            Size textSize = TextRenderer.MeasureText(trangThai, guna2DataGridView1.Font);
            int badgeWidth = Math.Max(40, Math.Min(textSize.Width + 20, e.CellBounds.Width - 10));
            var badge = new Rectangle(
                e.CellBounds.X + (e.CellBounds.Width - badgeWidth) / 2,
                e.CellBounds.Y + (e.CellBounds.Height - 25) / 2,
                badgeWidth,
                25);

            using var brush = new SolidBrush(background);
            e.Graphics.FillRectangle(brush, badge);
            TextRenderer.DrawText(e.Graphics, trangThai,
                new Font("Segoe UI", 8F, FontStyle.Bold), badge, foreground,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter |
                TextFormatFlags.EndEllipsis);
            e.Handled = true;
        }

        private void TaiAnhBia(string? anhBia, int maSach)
        {
            GiaiPhongAnh();

            try
            {
                // This always returns a valid image. If no file can be resolved,
                // a generated EPU Library placeholder is shown instead.
                _anhBiaDaTai = BookCoverImageHelper.LoadForDetail(
                    anhBia,
                    maSach,
                    Math.Max(240, picAnhBia.Width),
                    Math.Max(360, picAnhBia.Height));

                picAnhBia.SizeMode = PictureBoxSizeMode.Zoom;
                picAnhBia.Image = _anhBiaDaTai;
                btnXemAnhLon.Enabled = true;
            }
            catch
            {
                _anhBiaDaTai = BookCoverImageHelper.LoadForDetail(null, maSach);
                picAnhBia.Image = _anhBiaDaTai;
                btnXemAnhLon.Enabled = true;
            }
        }

        private void btnXemAnhLon_Click(object? sender, EventArgs e)
        {
            if (_anhBiaDaTai == null) return;

            using var frm = new Form
            {
                Text = $"Ảnh bìa - {_duLieu?.TenSach}",
                StartPosition = FormStartPosition.CenterParent,
                Size = new Size(600, 780),
                MinimumSize = new Size(420, 560),
                BackColor = Color.FromArgb(30, 33, 40)
            };
            var picture = new PictureBox
            {
                Dock = DockStyle.Fill,
                SizeMode = PictureBoxSizeMode.Zoom,
                Image = new Bitmap(_anhBiaDaTai)
            };
            frm.Controls.Add(picture);
            frm.FormClosed += (_, _) => picture.Image?.Dispose();
            frm.ShowDialog(this);
        }

        private void FrmChiTietSach_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Close();
                e.SuppressKeyPress = true;
            }
            else if (e.KeyCode == Keys.F5 && _maSach > 0)
            {
                TaiDuLieu();
                e.SuppressKeyPress = true;
            }
        }

        private void GiaiPhongAnh()
        {
            picAnhBia.Image = null;
            _anhBiaDaTai?.Dispose();
            _anhBiaDaTai = null;
        }

        private void pnlThongTinSach_Paint(object sender, PaintEventArgs e)
        {
        }

        private void pnlTabs_Paint(object sender, PaintEventArgs e)
        {
        }
    }
}

