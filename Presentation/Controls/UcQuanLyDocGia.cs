using BusinessLayer.Services;
using DataLayer.Models;
using Guna.UI2.WinForms;
using Presentation.Helpers;
using Presentation.Models;
using System.Globalization;
using System.Text;
using System.ComponentModel;

namespace Presentation.Controls
{
    public partial class UcQuanLyDocGia : UserControl
    {
        private readonly DocGiaService _docGiaService = new();
        private readonly HashSet<int> _maDocGiaDaChon = new();
        private readonly List<Guna2Button> _nutSoTrang;
        private List<DocGiaGridModel> _duLieuGoc = new();
        private List<DocGiaGridModel> _duLieuLoc = new();
        private int _trangHienTai = 1;
        private int _soDongMoiTrang = 5;
        private int _tongTrang = 1;
        private bool _dangKhoiTaoBoLoc;

        public UcQuanLyDocGia()
        {
            InitializeComponent();
            dgvDocGia.AutoGenerateColumns = false;
            _nutSoTrang = new List<Guna2Button> { btnPage1, btnPage2, btnPage3, btnPage4, btnLastPage };

            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            GanSuKien();
            ApDungPhanQuyen();
        }

        private void ApDungPhanQuyen()
        {
            btnThemDocGia.Enabled = PermissionHelper.CanAdd("DOCGIA.DANHSACH");
            btnCapThe.Enabled = PermissionHelper.CanAdd("DOCGIA.THEDOCGIA");
            btnThuLePhi.Enabled = PermissionHelper.CanAdd("DOCGIA.LEPHI");
            btnXuatExcel.Enabled = PermissionHelper.CanExport("DOCGIA.DANHSACH");
            colSua.Visible = PermissionHelper.CanEdit("DOCGIA.DANHSACH");
            colKhoa.Visible = PermissionHelper.CanEdit("DOCGIA.THEDOCGIA");
        }

        private static bool KiemTraQuyen(bool allowed, string message)
        {
            if (allowed) return true;
            MessageBox.Show(message, "Không đủ quyền", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        private void GanSuKien()
        {
            Load += UcQuanLyDocGia_Load;
            txtTimKiemSach.TextChanged += BoLoc_Changed;
            cboKhoa.SelectedIndexChanged += cboKhoa_SelectedIndexChanged;
            cboLop.SelectedIndexChanged += BoLoc_Changed;
            cboTrangThaiThe.SelectedIndexChanged += BoLoc_Changed;

            btnLamMoi.Click += btnLamMoi_Click;
            btnThemDocGia.Click += btnThemDocGia_Click;
            btnCapThe.Click += btnCapThe_Click;
            btnThuLePhi.Click += btnThuLePhi_Click;
            btnXuatExcel.Click += btnXuatExcel_Click;
            btnChonNhieu.Click += btnChonNhieu_Click;

            btnTrangDau.Click += (_, _) => ChuyenTrang(1);
            btnTrangTruoc.Click += (_, _) => ChuyenTrang(_trangHienTai - 1);
            btnTrangSau.Click += (_, _) => ChuyenTrang(_trangHienTai + 1);
            btnTrangCuoi.Click += (_, _) => ChuyenTrang(_tongTrang);
            btnGo.Click += (_, _) => ChuyenDenTrangNhap();
            txtTrang.KeyDown += txtTrang_KeyDown;
            txtTrang.KeyPress += txtTrang_KeyPress;
            foreach (Guna2Button button in _nutSoTrang)
                button.Click += NutSoTrang_Click;

            dgvDocGia.CellContentClick += dgvDocGia_CellContentClick;
            dgvDocGia.CellValueChanged += dgvDocGia_CellValueChanged;
            dgvDocGia.ColumnHeaderMouseClick += dgvDocGia_ColumnHeaderMouseClick;
            dgvDocGia.CurrentCellDirtyStateChanged += (_, _) =>
            {
                if (dgvDocGia.IsCurrentCellDirty && dgvDocGia.CurrentCell?.OwningColumn.Name == "colChon")
                    dgvDocGia.CommitEdit(DataGridViewDataErrorContexts.Commit);
            };
        }

        private void UcQuanLyDocGia_Load(object? sender, EventArgs e)
        {
            KhoiTaoComboBox();
            TaiDuLieu();
        }

        public void ReloadData() => TaiDuLieu();

        private void KhoiTaoComboBox()
        {
            _dangKhoiTaoBoLoc = true;
            try
            {
                cboKhoa.Items.Clear();
                cboKhoa.Items.Add("-- Tất cả khoa --");
                cboKhoa.Items.AddRange(_docGiaService.GetDanhSachKhoa().Cast<object>().ToArray());
                cboKhoa.SelectedIndex = 0;

                NapDanhSachLop(null);

                cboTrangThaiThe.Items.Clear();
                cboTrangThaiThe.Items.AddRange(new object[]
                {
                    "-- Tất cả trạng thái --", "Còn hiệu lực", "Sắp hết hạn", "Hết hạn",
                    "Chưa đóng lệ phí", "Bị khóa", "Chưa có thẻ"
                });
                cboTrangThaiThe.SelectedIndex = 0;
            }
            finally
            {
                _dangKhoiTaoBoLoc = false;
            }
        }

        private void NapDanhSachLop(string? tenKhoa)
        {
            cboLop.Items.Clear();
            cboLop.Items.Add("-- Tất cả lớp --");
            cboLop.Items.AddRange(_docGiaService.GetDanhSachLop(tenKhoa).Cast<object>().ToArray());
            cboLop.SelectedIndex = 0;
        }

        private void TaiDuLieu()
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                _duLieuGoc = _docGiaService.GetDanhSach();
                _maDocGiaDaChon.RemoveWhere(id => _duLieuGoc.All(x => x.MaDocGia != id));
                CapNhatCardVaThongTinNhanh();
                LocDuLieu();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể tải dữ liệu quản lý độc giả.\n\n" + ex.Message,
                    "Lỗi dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void CapNhatCardVaThongTinNhanh()
        {
            DocGiaStatisticsModel tk = _docGiaService.GetStatistics();
            lblTongDocGia.Text = tk.TongDocGia.ToString("N0");
            lblTheConHan.Text = tk.TheConHieuLuc.ToString("N0");
            lblSapHetHan.Text = tk.SapHetHan.ToString("N0");
            lblNoPhat.Text = tk.NoPhatChuaThanhToan.ToString("N0", CultureInfo.GetCultureInfo("vi-VN")) + "đ";

            SetDeltaLabel(lblTongDocGiaSub, tk.TongDocGia, tk.TongDocGiaThangTruoc);
            SetDeltaLabel(lblTheConHanSub, tk.TheConHieuLuc, tk.TheConHieuLucThangTruoc);
            SetDeltaLabel(lblSapHetHanSub, tk.SapHetHan, tk.SapHetHanThangTruoc);
            SetDeltaLabel(lblNoPhatSub, tk.NoPhatChuaThanhToan, tk.NoPhatThangTruoc);

            lblDocGiaDangMuon.Text = tk.DocGiaDangMuon.ToString("N0");
            lblTongGiaoDich.Text = tk.TongGiaoDichMuonTra.ToString("N0");
            lblPhieuPhat.Text = tk.SoPhieuPhatChuaThanhToan.ToString("N0");
        }

        private static void SetDeltaLabel(Label label, decimal hienTai, decimal thangTruoc)
        {
            decimal chenhLech = hienTai - thangTruoc;
            if (chenhLech == 0)
            {
                label.Text = "— Không thay đổi so với tháng trước";
                MetricTrendHelper.ApplyColor(label, chenhLech);
                return;
            }

            string muiTen = chenhLech > 0 ? "↑" : "↓";
            string phanTram = thangTruoc == 0
                ? string.Empty
                : $" ({Math.Abs(chenhLech / thangTruoc * 100):0.0}%)";
            label.Text = $"{muiTen} {Math.Abs(chenhLech):N0}{phanTram} so với tháng trước";

            MetricTrendHelper.ApplyColor(label, chenhLech);
        }

        private void cboKhoa_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (_dangKhoiTaoBoLoc) return;
            _dangKhoiTaoBoLoc = true;
            try
            {
                string? khoa = cboKhoa.SelectedIndex <= 0 ? null : cboKhoa.SelectedItem?.ToString();
                NapDanhSachLop(khoa);
            }
            finally
            {
                _dangKhoiTaoBoLoc = false;
            }
            LocDuLieu();
        }

        private void BoLoc_Changed(object? sender, EventArgs e)
        {
            if (!_dangKhoiTaoBoLoc) LocDuLieu();
        }

        private void LocDuLieu()
        {
            IEnumerable<DocGiaGridModel> query = _duLieuGoc;
            string key = txtTimKiemSach.Text.Trim();
            if (!string.IsNullOrWhiteSpace(key))
            {
                string cleanMa = key.StartsWith("DG", StringComparison.OrdinalIgnoreCase)
                    ? key[2..].TrimStart('0')
                    : key;
                query = query.Where(x =>
                    x.MaDocGiaText.Contains(key, StringComparison.CurrentCultureIgnoreCase) ||
                    x.MaDocGia.ToString() == cleanMa ||
                    x.HoTen.Contains(key, StringComparison.CurrentCultureIgnoreCase) ||
                    x.Email.Contains(key, StringComparison.CurrentCultureIgnoreCase) ||
                    x.SoDienThoai.Contains(key, StringComparison.CurrentCultureIgnoreCase));
            }

            if (cboKhoa.SelectedIndex > 0)
                query = query.Where(x => x.TenKhoa == cboKhoa.SelectedItem?.ToString());
            if (cboLop.SelectedIndex > 0)
                query = query.Where(x => x.TenLop == cboLop.SelectedItem?.ToString());
            if (cboTrangThaiThe.SelectedIndex > 0)
                query = query.Where(x => x.TrangThaiThe == cboTrangThaiThe.SelectedItem?.ToString());

            _duLieuLoc = query.OrderBy(x => x.HoTen).ToList();
            _trangHienTai = 1;
            HienThiTrang();
        }

        private void HienThiTrang()
        {
            int tong = _duLieuLoc.Count;
            _tongTrang = Math.Max(1, (int)Math.Ceiling(tong / (double)_soDongMoiTrang));
            _trangHienTai = Math.Clamp(_trangHienTai, 1, _tongTrang);

            List<DocGiaGridRow> rows = _duLieuLoc
                .Skip((_trangHienTai - 1) * _soDongMoiTrang)
                .Take(_soDongMoiTrang)
                .Select((x, i) => new DocGiaGridRow
                {
                    STT = (_trangHienTai - 1) * _soDongMoiTrang + i + 1,
                    MaDocGia = x.MaDocGia,
                    MaDocGiaText = x.MaDocGiaText,
                    Anh = DatabaseImageHelper.LoadReaderAvatar(x.AnhDaiDien, x.HoTen, 36, 36),
                    HoTen = x.HoTen,
                    TenLop = x.TenLop,
                    Email = x.Email,
                    SoDienThoai = x.SoDienThoai,
                    NgayCapText = x.NgayCap?.ToString("dd/MM/yyyy") ?? "-",
                    HanTheText = x.NgayHetHan?.ToString("dd/MM/yyyy") ?? "-",
                    TrangThaiThe = x.TrangThaiThe,
                    PhiNamText = x.PhiNam.ToString("N0") + "đ",
                    DangMuon = x.DangMuon,
                    NoPhatText = x.NoPhat.ToString("N0") + "đ"
                }).ToList();

            dgvDocGia.DataSource = null;
            dgvDocGia.DataSource = rows;
            for (int i = 0; i < dgvDocGia.Rows.Count; i++)
            {
                var row = (DocGiaGridRow)dgvDocGia.Rows[i].DataBoundItem!;
                dgvDocGia.Rows[i].Cells["colChon"].Value = _maDocGiaDaChon.Contains(row.MaDocGia);
            }
            dgvDocGia.ClearSelection();
            CapNhatTieuDeCotChonDocGia();

            int batDau = tong == 0 ? 0 : (_trangHienTai - 1) * _soDongMoiTrang + 1;
            int ketThuc = Math.Min(_trangHienTai * _soDongMoiTrang, tong);
            lblPageInfo.Text = $"Hiển thị {batDau:N0}–{ketThuc:N0} của {tong:N0} độc giả";
            txtTrang.Text = _trangHienTai.ToString();
            lblTongTrang.Text = $"/ {_tongTrang}";

            btnTrangDau.Enabled = btnTrangTruoc.Enabled = _trangHienTai > 1;
            btnTrangSau.Enabled = btnTrangCuoi.Enabled = _trangHienTai < _tongTrang;
            CapNhatNutSoTrang();
        }

        private void CapNhatNutSoTrang()
        {
            int start = Math.Max(1, _trangHienTai - 2);
            int end = Math.Min(_tongTrang, start + _nutSoTrang.Count - 1);
            start = Math.Max(1, end - _nutSoTrang.Count + 1);

            for (int i = 0; i < _nutSoTrang.Count; i++)
            {
                Guna2Button button = _nutSoTrang[i];
                int page = start + i;
                button.Visible = page <= end;
                if (!button.Visible) continue;
                button.Tag = page;
                button.Text = page.ToString();
                button.Checked = page == _trangHienTai;
            }
            lblDots.Visible = _tongTrang > _nutSoTrang.Count && end < _tongTrang;
        }


        private void NutSoTrang_Click(object? sender, EventArgs e)
        {
            if (sender is Guna2Button { Tag: int page }) ChuyenTrang(page);
        }

        private void ChuyenTrang(int page)
        {
            _trangHienTai = Math.Clamp(page, 1, _tongTrang);
            HienThiTrang();
        }

        private void ChuyenDenTrangNhap()
        {
            if (!int.TryParse(txtTrang.Text.Trim(), out int page) || page < 1 || page > _tongTrang)
            {
                MessageBox.Show($"Số trang không hợp lệ (Phải từ 1 đến {_tongTrang}).", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTrang.Focus();
                txtTrang.SelectAll();
                return;
            }
            ChuyenTrang(page);
        }

        private void txtTrang_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            ChuyenDenTrangNhap();
            e.SuppressKeyPress = true;
        }

        private void txtTrang_KeyPress(object? sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) e.Handled = true;
        }

        private void dgvDocGia_CellValueChanged(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || dgvDocGia.Columns[e.ColumnIndex].Name != "colChon") return;
            if (dgvDocGia.Rows[e.RowIndex].DataBoundItem is not DocGiaGridRow row) return;
            bool chon = Convert.ToBoolean(dgvDocGia.Rows[e.RowIndex].Cells["colChon"].Value ?? false);
            if (chon) _maDocGiaDaChon.Add(row.MaDocGia); else _maDocGiaDaChon.Remove(row.MaDocGia);
            CapNhatTieuDeCotChonDocGia();
        }


        private void dgvDocGia_CellContentClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || dgvDocGia.Rows[e.RowIndex].DataBoundItem is not DocGiaGridRow row) return;
            string column = dgvDocGia.Columns[e.ColumnIndex].Name;
            if (column == "colXem") XemChiTiet(row.MaDocGia);
            else if (column == "colSua") SuaDocGia(row.MaDocGia);
            else if (column == "colKhoa") ToggleKhoaThe(row.MaDocGia, row.TrangThaiThe);
            else if (column == "colXoa") XoaDocGia(row.MaDocGia);
        }

        private void btnChonNhieu_Click(object? sender, EventArgs e)
            => ChonBoChonTatCaTrangDocGia();

        private void dgvDocGia_ColumnHeaderMouseClick(object? sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex >= 0 && dgvDocGia.Columns[e.ColumnIndex].Name == "colChon")
                ChonBoChonTatCaTrangDocGia();
        }

        private void ChonBoChonTatCaTrangDocGia()
        {
            bool chonTatCa = dgvDocGia.Rows.Cast<DataGridViewRow>()
                .Any(r => !Convert.ToBoolean(r.Cells["colChon"].Value ?? false));
            foreach (DataGridViewRow row in dgvDocGia.Rows)
            {
                row.Cells["colChon"].Value = chonTatCa;
                if (row.DataBoundItem is DocGiaGridRow item)
                {
                    if (chonTatCa) _maDocGiaDaChon.Add(item.MaDocGia);
                    else _maDocGiaDaChon.Remove(item.MaDocGia);
                }
            }
            CapNhatTieuDeCotChonDocGia();
        }

        private void CapNhatTieuDeCotChonDocGia()
        {
            bool all = dgvDocGia.Rows.Count > 0 && dgvDocGia.Rows.Cast<DataGridViewRow>()
                .All(r => Convert.ToBoolean(r.Cells["colChon"].Value ?? false));
            dgvDocGia.Columns["colChon"].HeaderText = all ? "☑" : "☐";
        }

        private void btnLamMoi_Click(object? sender, EventArgs e)
        {
            _dangKhoiTaoBoLoc = true;
            txtTimKiemSach.Clear();
            cboKhoa.SelectedIndex = 0;
            NapDanhSachLop(null);
            cboTrangThaiThe.SelectedIndex = 0;
            _dangKhoiTaoBoLoc = false;
            _maDocGiaDaChon.Clear();
            TaiDuLieu();
        }

        private void btnThemDocGia_Click(object? sender, EventArgs e)
        {
            if (!KiemTraQuyen(PermissionHelper.CanAdd("DOCGIA.DANHSACH"), "Bạn không có quyền thêm độc giả.")) return;
            using FrmThemDocGia frm = new();
            if (frm.ShowDialog(FindForm()) == DialogResult.OK)
            {
                _maDocGiaDaChon.Clear();
                TaiDuLieu();
            }
        }

        private void btnCapThe_Click(object? sender, EventArgs e)
        {
            if (!KiemTraQuyen(PermissionHelper.CanAdd("DOCGIA.THEDOCGIA"), "Bạn không có quyền cấp thẻ độc giả.")) return;
            int? ma = LayMaDocGiaDangChon();
            if (!ma.HasValue) return;
            using FrmCapTheDocGia frm = new(ma.Value);
            if (frm.ShowDialog(FindForm()) == DialogResult.OK)
            {
                TaiLaiSauThaoTac();
            }
        }


        private void btnThuLePhi_Click(object? sender, EventArgs e)
        {
            if (!KiemTraQuyen(PermissionHelper.CanAdd("DOCGIA.LEPHI"), "Bạn không có quyền thu lệ phí độc giả.")) return;
            int? ma = LayMaDocGiaDangChon();
            using FrmThuLePhiDocGia frm = ma.HasValue ? new(ma.Value) : new();
            if (frm.ShowDialog(FindForm()) == DialogResult.OK)
            {
                TaiLaiSauThaoTac();
            }
        }

        private void ToggleKhoaThe(int maDocGia, string trangThaiThe)
        {
            if (!KiemTraQuyen(PermissionHelper.CanEdit("DOCGIA.THEDOCGIA"), "Bạn không có quyền khóa hoặc mở khóa thẻ.")) return;

            if (!string.Equals(trangThaiThe, "Bị khóa", StringComparison.OrdinalIgnoreCase))
            {
                using FrmKhoaTheDocGia frm = new(maDocGia);
                if (frm.ShowDialog(FindForm()) == DialogResult.OK)
                {
                    TaiLaiSauThaoTac();
                }
                return;
            }

            MoKhoaThe(maDocGia);
        }

        private void MoKhoaThe(int maDocGia)
        {
            if (!CurrentUser.MaNhanVien.HasValue)
            {
                MessageBox.Show("Chỉ tài khoản nhân viên mới được phép mở khóa thẻ độc giả.",
                    "Không đủ quyền", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show(
                "Bạn có chắc chắn muốn mở khóa thẻ cho độc giả này?",
                "Xác nhận mở khóa thẻ",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
            if (result != DialogResult.Yes) return;

            try
            {
                _docGiaService.MoKhoaTheDocGia(maDocGia);
                MessageBox.Show("Mở khóa thẻ độc giả thành công.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                TaiLaiSauThaoTac();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể mở khóa thẻ: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private int? LayMaDocGiaDangChon()
        {
            if (_maDocGiaDaChon.Count > 1)
            {
                MessageBox.Show("Thao tác này chỉ áp dụng cho một độc giả. Vui lòng chỉ chọn một người.",
                    "Chọn một độc giả", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;
            }
            if (_maDocGiaDaChon.Count == 1) return _maDocGiaDaChon.Single();
            if (dgvDocGia.CurrentRow?.DataBoundItem is DocGiaGridRow row) return row.MaDocGia;
            MessageBox.Show("Vui lòng chọn một độc giả.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return null;
        }

        private void TaiLaiSauThaoTac()
        {
            _maDocGiaDaChon.Clear();
            TaiDuLieu();
        }

        private void XemChiTiet(int maDocGia)
        {
            using Presentation.Models.FrmChiTietDocGia frm = new(maDocGia);
            frm.ShowDialog(FindForm());
        }

        private void SuaDocGia(int maDocGia)
        {
            if (!KiemTraQuyen(PermissionHelper.CanEdit("DOCGIA.DANHSACH"), "Bạn không có quyền sửa độc giả.")) return;
            using FrmSuaDocGia frm = new(maDocGia);
            if (frm.ShowDialog(FindForm()) == DialogResult.OK)
            {
                TaiLaiSauThaoTac();
            }
        }

        private void XoaDocGia(int maDocGia)
        {
            if (!KiemTraQuyen(PermissionHelper.CanDelete("DOCGIA.DANHSACH"), "Bạn không có quyền xóa độc giả.")) return;
            DialogResult dr = MessageBox.Show("Bạn có chắc chắn muốn xóa độc giả này khỏi CSDL?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr != DialogResult.Yes) return;

            try
            {
                _docGiaService.XoaDocGia(maDocGia);
                MessageBox.Show("Xóa độc giả thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                TaiLaiSauThaoTac();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể xóa độc giả: " + ex.Message, "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnXuatExcel_Click(object? sender, EventArgs e)
        {
            if (!KiemTraQuyen(PermissionHelper.CanExport("DOCGIA.DANHSACH"), "Bạn không có quyền xuất danh sách độc giả.")) return;
            dgvDocGia.EndEdit();
            List<DocGiaGridModel> data = _maDocGiaDaChon.Count > 0
                ? _duLieuLoc.Where(x => _maDocGiaDaChon.Contains(x.MaDocGia)).ToList()
                : _duLieuLoc.ToList();

            if (data.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using SaveFileDialog dialog = new()
            {
                Filter = "Excel Workbook (*.xlsx)|*.xlsx",
                FileName = _maDocGiaDaChon.Count > 0
                    ? $"DocGiaDaChon_{DateTime.Now:yyyyMMdd_HHmm}.xlsx"
                    : $"DanhSachDocGiaDaLoc_{DateTime.Now:yyyyMMdd_HHmm}.xlsx"
            };
            if (dialog.ShowDialog() != DialogResult.OK) return;

            string[] headers =
            {
                "STT", "Mã độc giả", "Họ tên", "Khoa", "Lớp", "Email", "SĐT",
                "Ngày cấp", "Hạn thẻ", "Trạng thái", "Phí năm", "Đang mượn", "Nợ phạt"
            };
            List<string[]> rows = data.Select((x, i) => new[]
            {
                (i + 1).ToString(), x.MaDocGiaText, x.HoTen, x.TenKhoa, x.TenLop,
                x.Email, x.SoDienThoai, x.NgayCap?.ToString("dd/MM/yyyy") ?? string.Empty,
                x.NgayHetHan?.ToString("dd/MM/yyyy") ?? string.Empty, x.TrangThaiThe,
                x.PhiNam.ToString("N0"), x.DangMuon.ToString(), x.NoPhat.ToString("N0")
            }).ToList();

            ExcelHelper.ExportToXlsx(dialog.FileName, "Danh sách độc giả", headers, rows);
            MessageBox.Show(_maDocGiaDaChon.Count > 0
                ? $"Đã xuất {data.Count:N0} độc giả được chọn."
                : $"Đã xuất {data.Count:N0} độc giả theo bộ lọc hiện tại.",
                "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private static string Csv(string value) => "\"" + value.Replace("\"", "\"\"") + "\"";


        private void pnlBottom_Paint(object? sender, PaintEventArgs e) { }

        private sealed class DocGiaGridRow
        {
            public int STT { get; set; }
            public int MaDocGia { get; set; }
            public string MaDocGiaText { get; set; } = string.Empty;
            public Image? Anh { get; set; }
            public string HoTen { get; set; } = string.Empty;
            public string TenLop { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public string SoDienThoai { get; set; } = string.Empty;
            public string NgayCapText { get; set; } = string.Empty;
            public string HanTheText { get; set; } = string.Empty;
            public string TrangThaiThe { get; set; } = string.Empty;
            public string PhiNamText { get; set; } = string.Empty;
            public int DangMuon { get; set; }
            public string NoPhatText { get; set; } = string.Empty;
        }



    }
}

