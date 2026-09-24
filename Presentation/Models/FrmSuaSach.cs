using BusinessLayer.DTOs;
using BusinessLayer.Services;
using DataLayer.Models;
using Presentation.Helpers;
using System.ComponentModel;
using System.Diagnostics;

namespace Presentation.Models
{
    public partial class FrmSuaSach : Form
    {
        private static readonly string[] VaiTroTacGia =
        {
            "Tác giả chính", "Đồng tác giả", "Dịch giả", "Biên soạn", "Hiệu đính"
        };

        private int _maSach;
        private SachService _sachService = null!;
        private readonly BindingList<TacGiaChonRow> _tacGiaDaChon = new();

        private string? _anhBiaCu;
        private string? _duongDanAnhMoi;
        private Image? _anhDangHienThi;
        private bool _dangLuu;
        private bool _dangTaiDuLieu;
        private bool _daTaiDuLieu;

        // Constructor dành cho WinForms Designer.
        public FrmSuaSach()
        {
            InitializeComponent();
            if (IsInDesignMode()) return;

            _sachService = new SachService();
            CauHinhForm();
            GanSuKien();
        }

        // Constructor dùng khi mở từ UcQuanLySach.
        public FrmSuaSach(int maSach) : this()
        {
            _maSach = maSach;
        }

        private static bool IsInDesignMode()
        {
            return LicenseManager.UsageMode == LicenseUsageMode.Designtime ||
                   string.Equals(Process.GetCurrentProcess().ProcessName, "devenv",
                       StringComparison.OrdinalIgnoreCase);
        }

        private void CauHinhForm()
        {
            CauHinhNhapLieu();
            CauHinhBangTacGia();
            HienThiAnhMacDinh();
        }

        private void CauHinhNhapLieu()
        {
            txtMaSach.ReadOnly = true;
            txtMaSachHienThi.ReadOnly = true;
            txtMoTa.MaxLength = 1000;
            txtGhiChu.MaxLength = 255;

            numSoTrang.Minimum = 1;
            numSoTrang.Maximum = 10000;
            numGiaBia.Minimum = 0;
            numGiaBia.Maximum = 100000000;
            numGiaNhap.Minimum = 0;
            numGiaNhap.Maximum = 100000000;

            // Form sửa chỉ bổ sung bản sao mới. Số lượng 0 nghĩa là không bổ sung.
            numSoLuongNhap.Minimum = 0;
            numSoLuongNhap.Maximum = 1000;
            numSoLuongNhap.Value = 0;

            dtpNamXuatBan.MaxDate = DateTime.Today;
            dtpNgayNhap.MaxDate = DateTime.Today;
            dtpNgayNhap.Value = DateTime.Today;

            GanDanhSachChuoi(cboNgonNgu,
                "-- Chọn ngôn ngữ --", "Tiếng Việt", "Tiếng Anh", "Tiếng Pháp",
                "Tiếng Trung", "Tiếng Nhật", "Tiếng Hàn", "Khác");
            GanDanhSachChuoi(cboTrangThaiSach, "Đang hoạt động", "Ngừng hoạt động");
            GanDanhSachChuoi(cboTinhTrang, "Tốt", "Hơi cũ", "Rách nhẹ", "Hỏng");

            // Bản sao mới luôn bắt đầu ở trạng thái Có sẵn.
            GanDanhSachChuoi(cboTrangThaiCuon, "Có sẵn");
            cboTrangThaiCuon.Enabled = false;

            label15.Text = "JPG, PNG, BMP - tối đa 5 MB";
        }

        private static void GanDanhSachChuoi(
            Guna.UI2.WinForms.Guna2ComboBox combo,
            params string[] values)
        {
            combo.DataSource = null;
            combo.Items.Clear();
            combo.Items.AddRange(values.Cast<object>().ToArray());
            if (combo.Items.Count > 0) combo.SelectedIndex = 0;
        }

        private void CauHinhBangTacGia()
        {
            dgvDocGia.AutoGenerateColumns = false;
            dgvDocGia.ReadOnly = false;
            dgvDocGia.AllowUserToAddRows = false;
            dgvDocGia.AllowUserToDeleteRows = false;
            dgvDocGia.RowHeadersVisible = false;
            dgvDocGia.RowTemplate.Height = 71;
            dgvDocGia.ColumnHeadersHeight = 36;

            if (dgvDocGia.Columns.Contains("colVaiTro"))
                dgvDocGia.Columns.Remove("colVaiTro");
            if (dgvDocGia.Columns.Contains("colXoaTacGia"))
                dgvDocGia.Columns.Remove("colXoaTacGia");

            var roleColumn = new DataGridViewComboBoxColumn
            {
                Name = "colVaiTro",
                HeaderText = "Vai trò",
                DataPropertyName = nameof(TacGiaChonRow.VaiTro),
                DataSource = VaiTroTacGia,
                FlatStyle = FlatStyle.Flat,
                Width = 145
            };

            var deleteColumn = new DataGridViewButtonColumn
            {
                Name = "colXoaTacGia",
                HeaderText = "Xóa",
                Text = "Xóa",
                UseColumnTextForButtonValue = true,
                FlatStyle = FlatStyle.Flat,
                Width = 65
            };

            dgvDocGia.Columns.Insert(2, roleColumn);
            dgvDocGia.Columns.Insert(3, deleteColumn);

            colTacGiaSTT.DataPropertyName = nameof(TacGiaChonRow.STT);
            colTenTacGia.DataPropertyName = nameof(TacGiaChonRow.TenTacGia);
            colTacGiaSTT.ReadOnly = true;
            colTenTacGia.ReadOnly = true;

            dgvDocGia.DataSource = _tacGiaDaChon;
        }

        private void GanSuKien()
        {
            Load += FrmSuaSach_Load;
            btnDoiAnh.Click += btnDoiAnh_Click;
            btnThemTacGia.Click += btnThemTacGia_Click;
            dgvDocGia.CellContentClick += dgvDocGia_CellContentClick;
            dgvDocGia.DataError += (_, e) => e.ThrowException = false;

            txtMoTa.TextChanged += (_, _) =>
                lblDemMoTa.Text = $"{txtMoTa.Text.Length}/1000";
            txtGhiChu.TextChanged += (_, _) =>
                lblDemGhiChu.Text = $"{txtGhiChu.Text.Length}/255";

            txtTienToMaVach.KeyPress += ChiChoNhapSo_KeyPress;
            txtISBN.KeyPress += Isbn_KeyPress;

            btnDatLai.Click += async (_, _) => await TaiDuLieuAsync();
            btnDong.Click += (_, _) => Close();
            btnLuuSach.Click += btnLuuSach_Click;
            KeyDown += FrmSuaSach_KeyDown;
            FormClosed += (_, _) => GiaiPhongAnh();
        }

        private async void FrmSuaSach_Load(object? sender, EventArgs e)
        {
            if (_daTaiDuLieu || _maSach <= 0) return;
            await TaiDuLieuAsync();
        }

        private async Task TaiDuLieuAsync()
        {
            if (_dangTaiDuLieu || _maSach <= 0) return;

            try
            {
                _dangTaiDuLieu = true;
                Cursor = Cursors.WaitCursor;
                btnLuuSach.Enabled = false;
                btnDatLai.Enabled = false;

                TaiDanhMuc();
                ChiTietSachModel? model =
                    await Task.Run(() => _sachService.LayChiTietDayDu(_maSach));

                if (model == null)
                {
                    MessageBox.Show("Không tìm thấy sách cần sửa.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Close();
                    return;
                }

                GanDuLieuLenForm(model);
                _daTaiDuLieu = true;
            }
            catch (Exception ex)
            {
                HienLoi("Không thể tải thông tin sách.", ex);
            }
            finally
            {
                _dangTaiDuLieu = false;
                btnLuuSach.Enabled = true;
                btnDatLai.Enabled = true;
                Cursor = Cursors.Default;
            }
        }

        private void TaiDanhMuc()
        {
            BindLookup(cboTheLoai, _sachService.LayTheLoai(), "-- Chọn thể loại --");
            BindLookup(cboNhaXuatBan, _sachService.LayNhaXuatBan(), "-- Chọn nhà xuất bản --");
            BindLookup(cboTacGia, _sachService.LayTacGia(), "-- Chọn tác giả --");
            BindLookup(cboViTri, _sachService.LayViTri(), "-- Chọn vị trí --");
        }

        private static void BindLookup(
            Guna.UI2.WinForms.Guna2ComboBox combo,
            IReadOnlyList<LookupItemModel> source,
            string placeholder)
        {
            var data = new List<LookupItemModel>
            {
                new() { Id = 0, Ten = placeholder }
            };
            data.AddRange(source);
            combo.DataSource = data;
            combo.DisplayMember = nameof(LookupItemModel.Ten);
            combo.ValueMember = nameof(LookupItemModel.Id);
            combo.SelectedIndex = 0;
        }

        private void GanDuLieuLenForm(ChiTietSachModel model)
        {
            _duongDanAnhMoi = null;
            txtMaSach.Text = model.MaSachText;
            txtMaSachHienThi.Text = model.MaSachHienThi;
            txtTenSach.Text = model.TenSach;
            txtISBN.Text = model.Isbn == "Chưa cập nhật" ? string.Empty : model.Isbn;

            cboTheLoai.SelectedValue = model.MaTheLoai;
            cboNhaXuatBan.SelectedValue = model.MaNhaXuatBan ?? 0;

            int nam = model.NamXuatBan.GetValueOrDefault(DateTime.Today.Year);
            nam = Math.Clamp(nam, 1000, DateTime.Today.Year);
            dtpNamXuatBan.Value = new DateTime(nam, 1, 1);

            ChonComboTheoText(cboNgonNgu, model.NgonNgu);

            numSoTrang.Value = GioiHan(model.SoTrang ?? 1,
                numSoTrang.Minimum, numSoTrang.Maximum);
            numGiaBia.Value = GioiHan(model.GiaBia ?? 0,
                numGiaBia.Minimum, numGiaBia.Maximum);

            txtMoTa.Text = model.MoTa == "Chưa có mô tả cho đầu sách này."
                ? string.Empty
                : model.MoTa;

            ChonComboTheoText(cboTrangThaiSach,
                model.TrangThai ? "Đang hoạt động" : "Ngừng hoạt động");

            lblNgayTaoTitle.Text =
                $"<b>Ngày tạo:</b> {model.NgayThem?.ToString("dd/MM/yyyy HH:mm") ?? "-"}";
            lblNguoiTaoTitle.Text =
                $"<b>Người cập nhật:</b> {model.NguoiCapNhat}";

            _anhBiaCu = model.AnhBia;
            TaiAnhBia(model.AnhBia);

            _tacGiaDaChon.Clear();
            foreach (TacGiaChiTietModel item in model.TacGias)
            {
                _tacGiaDaChon.Add(new TacGiaChonRow
                {
                    STT = _tacGiaDaChon.Count + 1,
                    MaTacGia = item.MaTacGia,
                    TenTacGia = item.TenTacGia,
                    VaiTro = string.IsNullOrWhiteSpace(item.VaiTro)
                        ? "Tác giả"
                        : item.VaiTro
                });
            }

            numSoLuongNhap.Value = 0;
            cboViTri.SelectedIndex = 0;
            dtpNgayNhap.Value = DateTime.Today;
            numGiaNhap.Value = 0;
            cboTinhTrang.SelectedIndex = 0;
            cboTrangThaiCuon.SelectedIndex = 0;
            txtTienToMaVach.Clear();
            txtGhiChu.Clear();

            lblDemMoTa.Text = $"{txtMoTa.Text.Length}/1000";
            lblDemGhiChu.Text = "0/255";
        }

        private static decimal GioiHan(decimal value, decimal min, decimal max)
            => Math.Max(min, Math.Min(max, value));

        private static void ChonComboTheoText(
            Guna.UI2.WinForms.Guna2ComboBox combo,
            string? text)
        {
            int index = combo.FindStringExact(text ?? string.Empty);
            combo.SelectedIndex = index >= 0 ? index : 0;
        }

        private void btnThemTacGia_Click(object? sender, EventArgs e)
        {
            int maTacGia = LayIdCombo(cboTacGia);
            if (maTacGia <= 0)
            {
                BaoThieu("Vui lòng chọn tác giả.", cboTacGia);
                return;
            }

            if (_tacGiaDaChon.Any(x => x.MaTacGia == maTacGia))
            {
                MessageBox.Show("Tác giả này đã có trong danh sách.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            _tacGiaDaChon.Add(new TacGiaChonRow
            {
                STT = _tacGiaDaChon.Count + 1,
                MaTacGia = maTacGia,
                TenTacGia = cboTacGia.Text,
                VaiTro = _tacGiaDaChon.Count == 0
                    ? "Tác giả chính"
                    : "Đồng tác giả"
            });

            cboTacGia.SelectedIndex = 0;
        }

        private void dgvDocGia_CellContentClick(
            object? sender,
            DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            if (dgvDocGia.Columns[e.ColumnIndex].Name != "colXoaTacGia") return;

            if (e.RowIndex < _tacGiaDaChon.Count)
            {
                _tacGiaDaChon.RemoveAt(e.RowIndex);
                CapNhatSttTacGia();
            }
        }

        private void CapNhatSttTacGia()
        {
            for (int i = 0; i < _tacGiaDaChon.Count; i++)
                _tacGiaDaChon[i].STT = i + 1;
            dgvDocGia.Refresh();
        }

        private void btnDoiAnh_Click(object? sender, EventArgs e)
        {
            using var dialog = new OpenFileDialog
            {
                Title = "Chọn ảnh bìa sách",
                Filter = "Tệp ảnh (*.jpg;*.jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp",
                CheckFileExists = true,
                Multiselect = false
            };

            if (dialog.ShowDialog(this) != DialogResult.OK) return;

            var file = new FileInfo(dialog.FileName);
            if (file.Length > 5L * 1024 * 1024)
            {
                MessageBox.Show("Ảnh bìa không được vượt quá 5 MB.", "Ảnh quá lớn",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using FileStream stream = new(
                    dialog.FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                using Image source = Image.FromStream(stream, true, true);

                GiaiPhongAnh();
                _anhDangHienThi = new Bitmap(source);
                picAnhBia.Image = _anhDangHienThi;
                _duongDanAnhMoi = dialog.FileName;
            }
            catch (Exception ex)
            {
                HienLoi("Không thể đọc tệp ảnh đã chọn.", ex);
            }
        }

        private void TaiAnhBia(string? databaseValue)
        {
            GiaiPhongAnh();
            _anhDangHienThi =
                BookCoverImageHelper.LoadForDetail(databaseValue, _maSach, 234, 355);
            picAnhBia.Image = _anhDangHienThi;
        }

        private void HienThiAnhMacDinh()
        {
            GiaiPhongAnh();
            _anhDangHienThi =
                BookCoverImageHelper.LoadForDetail(null, _maSach, 234, 355);
            picAnhBia.Image = _anhDangHienThi;
        }

        private bool KiemTraDuLieu()
        {
            if (string.IsNullOrWhiteSpace(txtTenSach.Text))
                return BaoThieu("Vui lòng nhập tên sách.", txtTenSach);

            if (LayIdCombo(cboTheLoai) <= 0)
                return BaoThieu("Vui lòng chọn thể loại.", cboTheLoai);

            if (LayIdCombo(cboNhaXuatBan) <= 0)
                return BaoThieu("Vui lòng chọn nhà xuất bản.", cboNhaXuatBan);

            if (cboNgonNgu.SelectedIndex <= 0)
                return BaoThieu("Vui lòng chọn ngôn ngữ.", cboNgonNgu);

            if (_tacGiaDaChon.Count == 0)
                return BaoThieu("Sách cần có ít nhất một tác giả.", cboTacGia);

            if (!string.IsNullOrWhiteSpace(txtISBN.Text) &&
                _sachService.IsbnDaTonTai(txtISBN.Text.Trim(), _maSach))
                return BaoThieu("ISBN đã được dùng cho đầu sách khác.", txtISBN);

            if (numSoLuongNhap.Value > 0 && LayIdCombo(cboViTri) <= 0)
                return BaoThieu("Vui lòng chọn vị trí cho bản sao bổ sung.", cboViTri);

            if (!string.IsNullOrWhiteSpace(txtTienToMaVach.Text) &&
                !txtTienToMaVach.Text.All(char.IsDigit))
                return BaoThieu("Tiền tố mã vạch chỉ được chứa số.", txtTienToMaVach);

            return true;
        }

        private SachSaveDto TaoDto(string? anhBia)
        {
            dgvDocGia.EndEdit();

            return new SachSaveDto
            {
                MaSach = _maSach,
                MaSachHienThi = txtMaSachHienThi.Text,
                TenSach = txtTenSach.Text.Trim(),
                Isbn = ChuanHoaRong(txtISBN.Text),
                MaTheLoai = LayIdCombo(cboTheLoai),
                MaNhaXuatBan = LayIdCombo(cboNhaXuatBan),
                NamXuatBan = dtpNamXuatBan.Value.Year,
                NgonNgu = cboNgonNgu.Text,
                SoTrang = Convert.ToInt32(numSoTrang.Value),
                GiaBia = numGiaBia.Value,
                MoTa = ChuanHoaRong(txtMoTa.Text),
                AnhBia = anhBia,
                TrangThai = cboTrangThaiSach.Text == "Đang hoạt động",

                SoLuong = Convert.ToInt32(numSoLuongNhap.Value),
                MaViTri = numSoLuongNhap.Value > 0
                    ? LayIdCombo(cboViTri)
                    : null,
                NgayNhap = DateOnly.FromDateTime(dtpNgayNhap.Value),
                GiaNhap = numGiaNhap.Value,
                TinhTrangCuon = cboTinhTrang.Text,
                TrangThaiCuon = "Có sẵn",
                TienToMaVach = ChuanHoaRong(txtTienToMaVach.Text),
                GhiChuCuon = ChuanHoaRong(txtGhiChu.Text),

                MaTacGia = _tacGiaDaChon.Select(x => x.MaTacGia).ToList(),
                TacGiaChiTiet = _tacGiaDaChon.Select(x => new SachTacGiaSaveDto
                {
                    MaTacGia = x.MaTacGia,
                    VaiTro = x.VaiTro
                }).ToList()
            };
        }

        private async void btnLuuSach_Click(object? sender, EventArgs e)
        {
            if (_dangLuu || !KiemTraDuLieu()) return;

            if (MessageBox.Show(
                    "Lưu các thay đổi của đầu sách?" +
                    (numSoLuongNhap.Value > 0
                        ? $"\nĐồng thời bổ sung {numSoLuongNhap.Value:N0} bản sao mới."
                        : string.Empty),
                    "Xác nhận",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            string? anhDaLuu = null;
            try
            {
                _dangLuu = true;
                btnLuuSach.Enabled = false;
                Cursor = Cursors.WaitCursor;

                anhDaLuu = LuuAnhMoiNeuCo();
                SachSaveDto dto = TaoDto(anhDaLuu ?? _anhBiaCu);

                await Task.Run(() => _sachService.CapNhatDayDu(dto));

                MessageBox.Show(
                    "Cập nhật thông tin sách thành công." +
                    (dto.SoLuong > 0
                        ? $"\nĐã bổ sung {dto.SoLuong:N0} bản sao."
                        : string.Empty),
                    "Thành công",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                if (!string.IsNullOrWhiteSpace(anhDaLuu))
                    XoaAnhDaLuu(anhDaLuu);
                HienLoi("Không thể cập nhật sách.", ex);
            }
            finally
            {
                _dangLuu = false;
                btnLuuSach.Enabled = true;
                Cursor = Cursors.Default;
            }
        }

        private string? LuuAnhMoiNeuCo()
        {
            if (string.IsNullOrWhiteSpace(_duongDanAnhMoi) || !File.Exists(_duongDanAnhMoi))
                return null;

            string targetName = $"book_{_maSach:D4}_{DateTime.Now:yyyyMMddHHmmss}_{Guid.NewGuid():N}"[..50];
            return DatabaseImageHelper.CopyToProjectImages(
                _duongDanAnhMoi,
                "BookCovers",
                targetName);
        }

        private static IEnumerable<string> LayThuMucAnhBia()
        {
            var folders = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                Path.Combine(AppContext.BaseDirectory, "Images", "BookCovers")
            };

            DirectoryInfo? current = new(AppContext.BaseDirectory);
            for (int i = 0; i < 7 && current != null; i++, current = current.Parent)
            {
                string presentation =
                    Path.Combine(current.FullName, "Presentation", "Images", "BookCovers");
                if (Directory.Exists(Path.Combine(current.FullName, "Presentation")))
                    folders.Add(presentation);

                string direct =
                    Path.Combine(current.FullName, "Images", "BookCovers");
                if (Directory.Exists(Path.Combine(current.FullName, "Images")))
                    folders.Add(direct);
            }

            return folders;
        }

        private static void XoaAnhDaLuu(string databaseValue)
        {
            try
            {
                string? path = DatabaseImageHelper.ResolvePath(databaseValue);
                if (path != null && File.Exists(path)) File.Delete(path);
            }
            catch { }
        }

        private void FrmSuaSach_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Close();
                e.SuppressKeyPress = true;
            }
            else if (e.Control && e.KeyCode == Keys.S)
            {
                btnLuuSach.PerformClick();
                e.SuppressKeyPress = true;
            }
            else if (e.KeyCode == Keys.F5)
            {
                _ = TaiDuLieuAsync();
                e.SuppressKeyPress = true;
            }
        }

        private static int LayIdCombo(Guna.UI2.WinForms.Guna2ComboBox combo)
        {
            try { return Convert.ToInt32(combo.SelectedValue ?? 0); }
            catch { return 0; }
        }

        private static string? ChuanHoaRong(string? value)
            => string.IsNullOrWhiteSpace(value) ? null : value.Trim();

        private static bool BaoThieu(string message, Control control)
        {
            MessageBox.Show(message, "Thiếu hoặc sai thông tin",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            control.Focus();
            return false;
        }

        private static void ChiChoNhapSo_KeyPress(
            object? sender,
            KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        private static void Isbn_KeyPress(object? sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) &&
                !char.IsDigit(e.KeyChar) &&
                e.KeyChar != '-')
                e.Handled = true;
        }

        private void GiaiPhongAnh()
        {
            picAnhBia.Image = null;
            _anhDangHienThi?.Dispose();
            _anhDangHienThi = null;
        }

        private static void HienLoi(string message, Exception ex)
        {
            Exception root = ex;
            while (root.InnerException != null) root = root.InnerException;

            MessageBox.Show(
                $"{message}\n\nChi tiết: {root.Message}",
                "Lỗi",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }

        private void pnlTitleBar_Paint(object sender, PaintEventArgs e)
        {
            // Giữ sự kiện do Designer tạo.
        }

        private sealed class TacGiaChonRow : INotifyPropertyChanged
        {
            private int _stt;
            private string _vaiTro = "Tác giả";

            public int STT
            {
                get => _stt;
                set
                {
                    _stt = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(STT)));
                }
            }

            public int MaTacGia { get; set; }
            public string TenTacGia { get; set; } = string.Empty;

            public string VaiTro
            {
                get => _vaiTro;
                set
                {
                    _vaiTro = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(VaiTro)));
                }
            }

            public event PropertyChangedEventHandler? PropertyChanged;
        }
    }
}
