using BusinessLayer.Services;
using DataLayer.Models;
using Presentation.Helpers;
using Presentation.Models;
using System.ComponentModel;
using System.Globalization;

namespace Presentation.Controls;

public partial class UcNhanVien : UserControl
{
    private NhanVienService? _service;
    private readonly List<NhanVienGridModel> _duLieuGoc = new();
    private readonly List<NhanVienGridModel> _duLieuLoc = new();
    private readonly HashSet<int> _nhanVienDaChon = new();
    private bool _dangKhoiTao;
    private int _trangHienTai = 1;
    private int _soDongMoiTrang = 7;
    private int _tongTrang = 1;
    private int? _maNhanVienDangChon;

    public UcNhanVien()
    {
        InitializeComponent();

        if (DesignModeHelper.IsDesignMode(this))
        {
            return;
        }

        _service = new NhanVienService();
        GanSuKien();
        ApDungPhanQuyen();
        // ensure DataGridView uses designer-defined columns only
        dgvNhanVien.AutoGenerateColumns = false;
    }

    private void ApDungPhanQuyen()
    {
        bool canEditEmployee = PermissionHelper.CanEdit("HETHONG.NHANVIEN");
        bool canManageAccount = PermissionHelper.CanEdit("HETHONG.TAIKHOAN");
        btnThemNhanVien.Enabled = PermissionHelper.CanAdd("HETHONG.NHANVIEN");
        btnXuatExcel.Enabled = PermissionHelper.CanExport("HETHONG.NHANVIEN");
        btnDatLaiMatKhau.Enabled = btnKhoaTaiKhoan.Enabled = btnLuuQuyen.Enabled = canManageAccount;
        colSua.Visible = canEditEmployee;
        colKhoa.Visible = canManageAccount;
        colXoa.Visible = PermissionHelper.CanDelete("HETHONG.NHANVIEN");
    }

    private static bool KiemTraQuyen(bool allowed, string message)
    {
        if (allowed) return true;
        MessageBox.Show(message, "Không đủ quyền", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return false;
    }

    private void GanSuKien()
    {
        txtTimKiem.TextChanged += BoLoc_Changed;
        cboChucVu.SelectedIndexChanged += BoLoc_Changed;
        cboTrangThai.SelectedIndexChanged += BoLoc_Changed;
        dtpNgayVaoLam.ValueChanged += BoLoc_Changed;

        btnLamMoi.Click += BtnLamMoi_Click;
        btnXuatExcel.Click += BtnXuatExcel_Click;
        btnThemNhanVien.Click += BtnThemNhanVien_Click;

        dgvNhanVien.CellClick += DgvNhanVien_CellClick;
        dgvNhanVien.CellContentClick += DgvNhanVien_CellContentClick;
        dgvNhanVien.CellValueChanged += DgvNhanVien_CellValueChanged;
        dgvNhanVien.CurrentCellDirtyStateChanged += DgvNhanVien_CurrentCellDirtyStateChanged;
        dgvNhanVien.ColumnHeaderMouseClick += DgvNhanVien_ColumnHeaderMouseClick;
        dgvNhanVien.DataBindingComplete += (_, _) => dgvNhanVien.ClearSelection();

        btnTrangDau.Click += (_, _) => ChuyenTrang(1);
        btnTrangTruoc.Click += (_, _) => ChuyenTrang(_trangHienTai - 1);
        btnTrangSau.Click += (_, _) => ChuyenTrang(_trangHienTai + 1);
        btnTrangCuoi.Click += (_, _) => ChuyenTrang(_tongTrang);
        btnPage1.Click += BtnSoTrang_Click;
        btnPage2.Click += BtnSoTrang_Click;
        btnPage3.Click += BtnSoTrang_Click;
        btnPage4.Click += BtnSoTrang_Click;
        btnLastPage.Click += BtnSoTrang_Click;
        btnGo.Click += BtnGo_Click;
        txtTrang.KeyPress += TxtTrang_KeyPress;
        txtTrang.KeyDown += TxtTrang_KeyDown;

        btnDatLaiMatKhau.Click += BtnDatLaiMatKhau_Click;
        btnKhoaTaiKhoan.Click += BtnKhoaTaiKhoan_Click;
        btnLuuQuyen.Click += BtnLuuQuyen_Click;
        if (cboLoaiThongKe != null)
            cboLoaiThongKe.SelectedIndexChanged += CboLoaiThongKe_SelectedIndexChanged;
    }

    private void UcNhanVien_Load(object? sender, EventArgs e)
    {
        if (DesignModeHelper.IsDesignMode(this)) return;
        KhoiTaoBoLoc();
        if (cboLoaiThongKe != null && cboLoaiThongKe.Items.Count > 0) cboLoaiThongKe.SelectedIndex = 0;
        TaiDuLieu();
    }

    // Event raised when user changes chart type selection in pnlChart
    public event Action<string>? LoaiThongKeChanged;

    private void CboLoaiThongKe_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (cboLoaiThongKe == null) return;
        string keyword = cboLoaiThongKe.SelectedItem?.ToString() ?? string.Empty;
        LoaiThongKeChanged?.Invoke(keyword);
    }

    public void ReloadData() => TaiDuLieu();

    private void KhoiTaoBoLoc()
    {
        if (_service == null) return;
        _dangKhoiTao = true;
        try
        {
            cboChucVu.Items.Clear();
            cboChucVu.Items.Add("-- Tất cả chức vụ --");
            foreach (string chucVu in _service.GetChucVu()) cboChucVu.Items.Add(chucVu);
            cboChucVu.SelectedIndex = 0;

            cboTrangThai.Items.Clear();
            cboTrangThai.Items.AddRange(new object[]
            {
                "-- Tất cả trạng thái --",
                "Đang làm việc",
                "Tạm nghỉ",
                "Đã khóa"
            });
            cboTrangThai.SelectedIndex = 0;

            dtpNgayVaoLam.Value = DateTime.Today;
            dtpNgayVaoLam.Checked = false;
        }
        finally
        {
            _dangKhoiTao = false;
        }
    }

    private void TaiDuLieu()
    {
        if (_service == null || DesignModeHelper.IsDesignMode(this)) return;

        try
        {
            Cursor = Cursors.WaitCursor;
            List<NhanVienGridModel> data = _service.GetDanhSach();
            _duLieuGoc.Clear();
            _duLieuGoc.AddRange(data);
            CapNhatCards();
            LocDuLieu(false);
        }
        catch (Exception ex)
        {
            MessageBox.Show("Không thể tải danh sách nhân viên.\n\n" + ex.Message,
                "Lỗi dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    private void CapNhatCards()
    {
        if (_service == null) return;
        NhanVienStatisticsModel model = _service.GetStatistics();
        lblTongNhanVien.Text = model.TongNhanVien.ToString("N0");
        lblDangLamViec.Text = model.DangLamViec.ToString("N0");
        lblTamNghi.Text = model.TamNghi.ToString("N0");
        lblTaiKhoanBiKhoa.Text = model.TaiKhoanBiKhoa.ToString("N0");
    }

    private void BoLoc_Changed(object? sender, EventArgs e)
    {
        if (_dangKhoiTao) return;
        LocDuLieu(true);
    }

    private void LocDuLieu(bool veTrangDau)
    {
        IEnumerable<NhanVienGridModel> query = _duLieuGoc;
        string keyword = txtTimKiem.Text.Trim();
        if (!string.IsNullOrWhiteSpace(keyword))
        {
            query = query.Where(x =>
                ChuaTuKhoa(x.MaNhanVienText, keyword) ||
                ChuaTuKhoa(x.HoTen, keyword) ||
                ChuaTuKhoa(x.SoDienThoai, keyword) ||
                ChuaTuKhoa(x.Email, keyword) ||
                ChuaTuKhoa(x.TenDangNhap, keyword));
        }

        string chucVu = cboChucVu.SelectedItem?.ToString() ?? string.Empty;
        if (!string.IsNullOrWhiteSpace(chucVu) && !chucVu.StartsWith("--"))
            query = query.Where(x => string.Equals(x.ChucVu, chucVu, StringComparison.CurrentCultureIgnoreCase));

        string trangThai = cboTrangThai.SelectedItem?.ToString() ?? string.Empty;
        if (!string.IsNullOrWhiteSpace(trangThai) && !trangThai.StartsWith("--"))
            query = query.Where(x => x.TrangThai == trangThai);

        if (dtpNgayVaoLam.Checked)
        {
            DateOnly ngayVaoLam = DateOnly.FromDateTime(dtpNgayVaoLam.Value.Date);
            query = query.Where(x => x.NgayVaoLam == ngayVaoLam);
        }

        _duLieuLoc.Clear();
        _duLieuLoc.AddRange(query.OrderBy(x => x.MaNhanVien));
        if (veTrangDau) _trangHienTai = 1;
        HienThiTrang();
    }

    private static bool ChuaTuKhoa(string? value, string keyword) =>
        (value ?? string.Empty).Contains(keyword, StringComparison.CurrentCultureIgnoreCase);

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
                Chon = _nhanVienDaChon.Contains(x.MaNhanVien),
                STT = (_trangHienTai - 1) * _soDongMoiTrang + index + 1,
                x.MaNhanVien,
                x.MaNhanVienText,
                x.HoTen,
                x.GioiTinh,
                x.NgaySinhText,
                x.ChucVu,
                x.SoDienThoai,
                x.Email,
                x.NgayVaoLamText,
                x.TrangThai,
                x.TenDangNhap
            }).ToList();

        dgvNhanVien.AutoGenerateColumns = false;
        dgvNhanVien.DataSource = null;
        dgvNhanVien.DataSource = dataTrang;

        int batDau = tongBanGhi == 0 ? 0 : (_trangHienTai - 1) * _soDongMoiTrang + 1;
        int ketThuc = Math.Min(_trangHienTai * _soDongMoiTrang, tongBanGhi);
        lblPageInfo.Text = $"Hiển thị {batDau:N0}–{ketThuc:N0} của {tongBanGhi:N0} nhân viên";
        lblTongTrang.Text = $"/ {_tongTrang:N0}";
        txtTrang.Text = _trangHienTai.ToString(CultureInfo.InvariantCulture);

        TaoNutTrang();
        CapNhatTrangThaiNut();

        List<NhanVienGridModel> pageItems = _duLieuLoc
            .Skip((_trangHienTai - 1) * _soDongMoiTrang)
            .Take(_soDongMoiTrang)
            .ToList();
        if (_maNhanVienDangChon.HasValue && pageItems.Any(x => x.MaNhanVien == _maNhanVienDangChon.Value))
            TaiChiTiet(_maNhanVienDangChon.Value);
        else if (pageItems.Count > 0)
            TaiChiTiet(pageItems[0].MaNhanVien);
        else
            XoaChiTiet();
    }

    private void TaoNutTrang()
    {
        Guna.UI2.WinForms.Guna2Button[] buttons = { btnPage1, btnPage2, btnPage3, btnPage4, btnLastPage };
        List<int> pages = TaoDanhSachTrang(_trangHienTai, _tongTrang, buttons.Length);

        for (int i = 0; i < buttons.Length; i++)
        {
            var button = buttons[i];
            if (i < pages.Count)
            {
                int page = pages[i];
                button.Visible = true;
                button.Text = page.ToString(CultureInfo.InvariantCulture);
                button.Tag = page;
                button.AutoSize = page >= 100;
                button.Padding = new Padding(6, 0, 6, 0);
                bool active = page == _trangHienTai;
                button.FillColor = active ? Color.FromArgb(35, 85, 220) : Color.White;
                button.ForeColor = active ? Color.White : Color.FromArgb(35, 48, 90);
                button.BorderColor = active ? Color.FromArgb(35, 85, 220) : Color.FromArgb(210, 220, 238);
                button.BorderThickness = 1;
            }
            else
            {
                button.Visible = false;
            }
        }

        lblDots.Visible = _tongTrang > buttons.Length && pages.Count > 1 && pages[^1] - pages[^2] > 1;
    }

    private static List<int> TaoDanhSachTrang(int current, int total, int maxButtons)
    {
        if (total <= maxButtons)
            return Enumerable.Range(1, total).ToList();

        int interiorSlots = Math.Max(0, maxButtons - 2);
        int start = Math.Max(2, current - interiorSlots / 2);
        int end = Math.Min(total - 1, start + interiorSlots - 1);
        start = Math.Max(2, end - interiorSlots + 1);

        var result = new List<int> { 1 };
        for (int page = start; page <= end; page++) result.Add(page);
        result.Add(total);
        return result.Distinct().Take(maxButtons).ToList();
    }

    private void CapNhatTrangThaiNut()
    {
        btnTrangDau.Enabled = _trangHienTai > 1;
        btnTrangTruoc.Enabled = _trangHienTai > 1;
        btnTrangSau.Enabled = _trangHienTai < _tongTrang;
        btnTrangCuoi.Enabled = _trangHienTai < _tongTrang;
    }

    private void ChuyenTrang(int page)
    {
        int target = Math.Clamp(page, 1, _tongTrang);
        if (target == _trangHienTai) return;
        LuuCheckboxTrangHienTai();
        _trangHienTai = target;
        HienThiTrang();
    }

    private void BtnSoTrang_Click(object? sender, EventArgs e)
    {
        if (sender is Guna.UI2.WinForms.Guna2Button button && button.Tag is int page) ChuyenTrang(page);
    }

    private void BtnGo_Click(object? sender, EventArgs e)
    {
        if (!int.TryParse(txtTrang.Text.Trim(), out int page) || page < 1 || page > _tongTrang)
        {
            MessageBox.Show($"Vui lòng nhập số trang từ 1 đến {_tongTrang:N0}.", "Trang không hợp lệ",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

    private void DgvNhanVien_CurrentCellDirtyStateChanged(object? sender, EventArgs e)
    {
        if (dgvNhanVien.IsCurrentCellDirty && dgvNhanVien.CurrentCell?.OwningColumn == colChon)
            dgvNhanVien.CommitEdit(DataGridViewDataErrorContexts.Commit);
    }

    private void DgvNhanVien_CellValueChanged(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0 || e.ColumnIndex != colChon.Index) return;
        int? maNhanVien = LayMaNhanVienTuDong(e.RowIndex);
        if (!maNhanVien.HasValue) return;
        bool selected = Convert.ToBoolean(dgvNhanVien.Rows[e.RowIndex].Cells[colChon.Index].Value ?? false);
        if (selected) _nhanVienDaChon.Add(maNhanVien.Value);
        else _nhanVienDaChon.Remove(maNhanVien.Value);
    }

    private void LuuCheckboxTrangHienTai()
    {
        foreach (DataGridViewRow row in dgvNhanVien.Rows)
        {
            int? id = LayMaNhanVienTuDong(row.Index);
            if (!id.HasValue) continue;
            bool selected = Convert.ToBoolean(row.Cells[colChon.Index].Value ?? false);
            if (selected) _nhanVienDaChon.Add(id.Value);
            else _nhanVienDaChon.Remove(id.Value);
        }
    }

    private void DgvNhanVien_ColumnHeaderMouseClick(object? sender, DataGridViewCellMouseEventArgs e)
    {
        if (e.ColumnIndex != colChon.Index) return;
        List<NhanVienGridModel> page = _duLieuLoc.Skip((_trangHienTai - 1) * _soDongMoiTrang).Take(_soDongMoiTrang).ToList();
        bool allSelected = page.Count > 0 && page.All(x => _nhanVienDaChon.Contains(x.MaNhanVien));
        foreach (var item in page)
        {
            if (allSelected) _nhanVienDaChon.Remove(item.MaNhanVien);
            else _nhanVienDaChon.Add(item.MaNhanVien);
        }
        HienThiTrang();
    }

    private int? LayMaNhanVienTuDong(int rowIndex)
    {
        if (rowIndex < 0 || rowIndex >= dgvNhanVien.Rows.Count) return null;
        string maText = dgvNhanVien.Rows[rowIndex].Cells[colMaNhanVien.Index].Value?.ToString() ?? string.Empty;
        return _duLieuLoc.FirstOrDefault(x => x.MaNhanVienText == maText)?.MaNhanVien;
    }

    private void DgvNhanVien_CellClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;
        int? id = LayMaNhanVienTuDong(e.RowIndex);
        if (id.HasValue) TaiChiTiet(id.Value);
    }

    private void DgvNhanVien_CellContentClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
        int? id = LayMaNhanVienTuDong(e.RowIndex);
        if (!id.HasValue) return;
        string column = dgvNhanVien.Columns[e.ColumnIndex].Name;

        switch (column)
        {
            case "colXem":
                _maNhanVienDangChon = id.Value;
                using (var form = new FrmChiTietNhanVien(id.Value))
                {
                    if (form.ShowDialog(FindForm()) == DialogResult.OK) TaiDuLieu();
                }
                break;
            case "colSua":
                _maNhanVienDangChon = id.Value;
                BtnCapNhat_Click(sender, e);
                break;
            case "colKhoa":
                ToggleTaiKhoan(id.Value);
                break;
            case "colXoa":
                ChoNghiViec(id.Value);
                break;
        }
    }


    private void TaiChiTiet(int maNhanVien)
    {
        if (_service == null) return;
        NhanVienDetailModel? model = _service.GetChiTiet(maNhanVien);
        if (model == null) return;
        _maNhanVienDangChon = maNhanVien;

        lblHoTenChiTiet.Text = model.HoTen;
        lblMaNhanVienChiTiet.Text = $"Mã NV: {model.MaNhanVienText}";
        lblChucVuChiTiet.Text = GiaTri(model.ChucVu);
        lblSoDienThoaiChiTiet.Text = GiaTri(model.SoDienThoai);
        lblEmailChiTiet.Text = GiaTri(model.Email);
        lblNgaySinhChiTiet.Text = model.NgaySinh?.ToString("dd/MM/yyyy") ?? "-";
        lblGioiTinhChiTiet.Text = GiaTri(model.GioiTinh);
        lblDiaChiChiTiet.Text = GiaTri(model.DiaChi);
        lblNgayVaoLamChiTiet.Text = model.NgayVaoLam.ToString("dd/MM/yyyy");
        lblTenDangNhapChiTiet.Text = GiaTri(model.TenDangNhap);
        lblVaiTroChiTiet.Text = GiaTri(model.TenVaiTro);
        guna2PictureBox1.Image?.Dispose();
        guna2PictureBox1.Image = DatabaseImageHelper.LoadStaffAvatar(
            model.AnhDaiDien,
            model.HoTen,
            Math.Max(72, guna2PictureBox1.Width),
            Math.Max(72, guna2PictureBox1.Height));

        HienThiTrangThaiChiTiet(model.TrangThai);
        btnKhoaTaiKhoan.Text = model.TaiKhoanHoatDong ? "Khóa tài khoản" : "Mở khóa tài khoản";
        bool coQuyenQuanTri = PermissionHelper.CanEdit("HETHONG.TAIKHOAN");
        btnKhoaTaiKhoan.Enabled = model.MaTaiKhoan.HasValue && coQuyenQuanTri;
        btnDatLaiMatKhau.Enabled = model.MaTaiKhoan.HasValue && coQuyenQuanTri;
        HienThiQuyenTheoVaiTro(model.MaVaiTro);
    }

    private static string GiaTri(string? value) => string.IsNullOrWhiteSpace(value) ? "-" : value;

    private void HienThiTrangThaiChiTiet(string trangThai)
    {
        btnTrangThaiNhanVien.Text = trangThai;
        (btnTrangThaiNhanVien.FillColor, btnTrangThaiNhanVien.ForeColor) = trangThai switch
        {
            "Đang làm việc" => (Color.FromArgb(230, 248, 235), Color.FromArgb(25, 145, 70)),
            "Tạm nghỉ" => (Color.FromArgb(255, 244, 228), Color.FromArgb(230, 120, 25)),
            _ => (Color.FromArgb(255, 235, 237), Color.FromArgb(225, 55, 65))
        };
    }

    private void HienThiQuyenTheoVaiTro(int? maVaiTro)
    {
        if (!maVaiTro.HasValue)
        {
            chkQuanLySach.Checked = chkMuonTra.Checked = chkNhapSach.Checked = false;
            chkThongKeBaoCao.Checked = chkQuanTriHeThong.Checked = false;
            return;
        }

        List<BusinessLayer.Models.PhanQuyenGridModel> permissions = new TaiKhoanService()
            .LayPhanQuyenTheoVaiTro(maVaiTro.Value)
            .Where(x => !x.LaDongNhom && x.DuocXem)
            .ToList();

        bool CoNhom(string maNhom) => permissions.Any(x =>
            x.MaNhom.Equals(maNhom, StringComparison.OrdinalIgnoreCase));

        chkQuanLySach.Checked = CoNhom("SACH");
        chkMuonTra.Checked = CoNhom("MUONTRA");
        chkNhapSach.Checked = CoNhom("NHAPSACH");
        chkThongKeBaoCao.Checked = CoNhom("BAOCAO");
        chkQuanTriHeThong.Checked = CoNhom("HETHONG");
    }

    private static Bitmap TaoAvatar(string hoTen, int size)
    {
        size = Math.Clamp(size, 48, 160);
        Bitmap bitmap = new(size, size);
        using Graphics graphics = Graphics.FromImage(bitmap);
        graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        graphics.Clear(Color.Transparent);
        using var background = new SolidBrush(Color.FromArgb(225, 236, 255));
        graphics.FillEllipse(background, 0, 0, size - 1, size - 1);
        string[] parts = hoTen.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        string initials = parts.Length == 0 ? "?" : parts.Length == 1
            ? parts[0][0].ToString().ToUpperInvariant()
            : $"{parts[0][0]}{parts[^1][0]}".ToUpperInvariant();
        using var font = new Font("Segoe UI", size * 0.28F, FontStyle.Bold, GraphicsUnit.Pixel);
        using var brush = new SolidBrush(Color.FromArgb(35, 85, 220));
        SizeF measure = graphics.MeasureString(initials, font);
        graphics.DrawString(initials, font, brush, (size - measure.Width) / 2F, (size - measure.Height) / 2F - 1F);
        return bitmap;
    }

    private void XoaChiTiet()
    {
        _maNhanVienDangChon = null;
        lblHoTenChiTiet.Text = "Chưa chọn nhân viên";
        lblMaNhanVienChiTiet.Text = "Mã NV: -";
        lblChucVuChiTiet.Text = lblSoDienThoaiChiTiet.Text = lblEmailChiTiet.Text = "-";
        lblNgaySinhChiTiet.Text = lblGioiTinhChiTiet.Text = lblDiaChiChiTiet.Text = "-";
        lblNgayVaoLamChiTiet.Text = lblTenDangNhapChiTiet.Text = lblVaiTroChiTiet.Text = "-";
        btnTrangThaiNhanVien.Text = "-";
        btnTrangThaiNhanVien.FillColor = Color.FromArgb(240, 242, 246);
        btnTrangThaiNhanVien.ForeColor = Color.Gray;
        guna2PictureBox1.Image?.Dispose();
        guna2PictureBox1.Image = TaoAvatar("?", Math.Max(72, guna2PictureBox1.Width));
        chkQuanLySach.Checked = chkMuonTra.Checked = chkNhapSach.Checked = false;
        chkThongKeBaoCao.Checked = chkQuanTriHeThong.Checked = false;
    }

    private void BtnLamMoi_Click(object? sender, EventArgs e)
    {
        _dangKhoiTao = true;
        try
        {
            txtTimKiem.Clear();
            if (cboChucVu.Items.Count > 0) cboChucVu.SelectedIndex = 0;
            if (cboTrangThai.Items.Count > 0) cboTrangThai.SelectedIndex = 0;
            dtpNgayVaoLam.Value = DateTime.Today;
            dtpNgayVaoLam.Checked = false;
            _nhanVienDaChon.Clear();
            _trangHienTai = 1;
        }
        finally
        {
            _dangKhoiTao = false;
        }
        TaiDuLieu();
    }

    private void BtnXuatExcel_Click(object? sender, EventArgs e)
    {
        if (!KiemTraQuyen(PermissionHelper.CanExport("HETHONG.NHANVIEN"), "Bạn không có quyền xuất danh sách nhân viên.")) return;
        LuuCheckboxTrangHienTai();
        List<NhanVienGridModel> data = _nhanVienDaChon.Count > 0
            ? _duLieuLoc.Where(x => _nhanVienDaChon.Contains(x.MaNhanVien)).ToList()
            : _duLieuLoc.ToList();

        if (data.Count == 0)
        {
            MessageBox.Show("Không có dữ liệu để xuất.", "Xuất Excel", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        using SaveFileDialog dialog = new()
        {
            Filter = "Excel Workbook (*.xlsx)|*.xlsx",
            FileName = $"DanhSachNhanVien_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx",
            Title = "Chọn nơi lưu danh sách nhân viên"
        };
        if (dialog.ShowDialog(FindForm()) != DialogResult.OK) return;

        string[] headers = { "STT", "Mã NV", "Họ tên", "Giới tính", "Ngày sinh", "Chức vụ", "SĐT", "Email", "Địa chỉ", "Ngày vào làm", "Trạng thái", "Tài khoản", "Vai trò" };
        List<string[]> rows = data.Select((x, index) => new[]
        {
            (index + 1).ToString(CultureInfo.InvariantCulture), x.MaNhanVienText, x.HoTen, x.GioiTinh,
            x.NgaySinhText, x.ChucVu, x.SoDienThoai, x.Email, x.DiaChi, x.NgayVaoLamText,
            x.TrangThai, x.TenDangNhap, x.TenVaiTro
        }).ToList();

        try
        {
            ExcelHelper.ExportToXlsx(dialog.FileName, "Nhân viên", headers, rows);
            MessageBox.Show($"Đã xuất {rows.Count:N0} nhân viên.", "Xuất Excel thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show("Không thể xuất Excel.\n\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnThemNhanVien_Click(object? sender, EventArgs e)
    {
        if (!KiemTraQuyen(PermissionHelper.CanAdd("HETHONG.NHANVIEN"), "Bạn không có quyền thêm nhân viên.")) return;
        using var form = new FrmThemNhanVien();
        if (form.ShowDialog(FindForm()) != DialogResult.OK) return;

        _maNhanVienDangChon = form.MaNhanVienVuaThem;
        _trangHienTai = 1;
        TaiDuLieu();
    }

    private void BtnCapNhat_Click(object? sender, EventArgs e)
    {
        if (!KiemTraQuyen(PermissionHelper.CanEdit("HETHONG.NHANVIEN"), "Bạn không có quyền cập nhật nhân viên.")) return;
        if (!_maNhanVienDangChon.HasValue)
        {
            MessageBox.Show("Vui lòng chọn nhân viên cần cập nhật.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        using var form = new FrmSuaNhanVien(_maNhanVienDangChon.Value);
        if (form.ShowDialog(FindForm()) != DialogResult.OK) return;

        TaiDuLieu();
    }

    private void BtnPhanQuyen_Click(object? sender, EventArgs e)
    {
        if (!_maNhanVienDangChon.HasValue)
        {
            MessageBox.Show("Vui lòng chọn nhân viên cần phân quyền.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        if (!KiemTraQuyen(PermissionHelper.CanEdit("HETHONG.TAIKHOAN"), "Bạn không có quyền phân quyền nhân viên.")) return;
        using var form = new FrmPhanQuyenNhanVien(_maNhanVienDangChon.Value);
        if (form.ShowDialog(FindForm()) == DialogResult.OK) TaiDuLieu();
    }

    private void BtnDatLaiMatKhau_Click(object? sender, EventArgs e)
    {
        if (_service == null || !_maNhanVienDangChon.HasValue) return;
        if (!KiemTraQuyen(PermissionHelper.CanEdit("HETHONG.TAIKHOAN"), "Bạn không có quyền đặt lại mật khẩu.")) return;

        NhanVienDetailModel? model = _service.GetChiTiet(_maNhanVienDangChon.Value);
        if (model?.MaTaiKhoan == null)
        {
            MessageBox.Show("Nhân viên này chưa có tài khoản.", "Đặt lại mật khẩu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        using var form = new FrmDatLaiMatKhauNhanVien(_maNhanVienDangChon.Value);
        if (form.ShowDialog(FindForm()) == DialogResult.OK)
            TaiChiTiet(_maNhanVienDangChon.Value);
    }

    private void BtnKhoaTaiKhoan_Click(object? sender, EventArgs e)
    {
        if (_maNhanVienDangChon.HasValue) ToggleTaiKhoan(_maNhanVienDangChon.Value);
    }

    private void ToggleTaiKhoan(int maNhanVien)
    {
        if (_service == null || !KiemTraQuyen(PermissionHelper.CanEdit("HETHONG.TAIKHOAN"), "Bạn không có quyền khóa hoặc mở khóa tài khoản.")) return;
        NhanVienDetailModel? model = _service.GetChiTiet(maNhanVien);
        if (model?.MaTaiKhoan == null)
        {
            MessageBox.Show("Nhân viên này chưa có tài khoản.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        if (model.TaiKhoanHoatDong && model.MaTaiKhoan == CurrentUser.MaTaiKhoan)
        {
            MessageBox.Show("Bạn không thể tự khóa tài khoản của chính mình.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        string action = model.TaiKhoanHoatDong ? "khóa" : "mở khóa";
        if (MessageBox.Show($"Bạn có chắc muốn {action} tài khoản {model.TenDangNhap}?", "Xác nhận",
            MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
        if (_service.ToggleTaiKhoan(maNhanVien)) TaiDuLieu();
    }

    private void ChoNghiViec(int maNhanVien)
    {
        if (_service == null || !KiemTraQuyen(PermissionHelper.CanDelete("HETHONG.NHANVIEN"), "Bạn không có quyền cho nhân viên nghỉ việc.")) return;
        NhanVienDetailModel? model = _service.GetChiTiet(maNhanVien);
        if (model == null) return;
        if (model.MaNhanVien == CurrentUser.MaNhanVien)
        {
            MessageBox.Show("Bạn không thể tự cho chính mình nghỉ việc và khóa tài khoản.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        if (MessageBox.Show($"Chuyển {model.HoTen} sang trạng thái tạm nghỉ và khóa tài khoản?\nDữ liệu lịch sử sẽ được giữ nguyên.",
            "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
        if (_service.ChoNghiViec(maNhanVien)) TaiDuLieu();
    }

    private void BtnLuuQuyen_Click(object? sender, EventArgs e)
    {
        if (!_maNhanVienDangChon.HasValue ||
            !KiemTraQuyen(PermissionHelper.CanEdit("HETHONG.TAIKHOAN"), "Bạn không có quyền phân quyền nhân viên.")) return;

        using var form = new FrmPhanQuyenNhanVien(_maNhanVienDangChon.Value);
        if (form.ShowDialog(FindForm()) == DialogResult.OK)
        {
            TaiDuLieu();
        }
    }

}
