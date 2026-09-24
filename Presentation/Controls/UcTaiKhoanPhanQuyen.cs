using BusinessLayer.Models;
using BusinessLayer.Services;
using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace Presentation.Controls;

public partial class UcTaiKhoanPhanQuyen : UserControl
{
    private readonly TaiKhoanService _service = new();
    private List<TaiKhoanGridModel> _taiKhoanGoc = new List<TaiKhoanGridModel>();
    private List<TaiKhoanGridModel> _taiKhoanLoc = new List<TaiKhoanGridModel>();
    private List<VaiTroGridModel> _vaiTro = new List<VaiTroGridModel>();
    private List<PhanQuyenGridModel> _phanQuyen = new List<PhanQuyenGridModel>();
    private int _trangHienTai = 1;
    private int _kichThuocTrang = 10;
    private bool _dangTaiCombo;
    private bool _coThayDoiQuyen;
    private int _maVaiTroDangSua;

    public UcTaiKhoanPhanQuyen()
    {
        InitializeComponent();
        if (DangODegsignMode()) return;
        CauHinhGiaoDien();
        GanSuKien();
        lblUser.Text = $"Xin chào, {Presentation.Helpers.CurrentUser.HoTen}";
        lblRole.Text = Presentation.Helpers.CurrentUser.VaiTro;
        btnThemTaiKhoan.Enabled = Presentation.Helpers.PermissionHelper.CanAdd("HETHONG.TAIKHOAN");
        bool canEdit = Presentation.Helpers.PermissionHelper.CanEdit("HETHONG.TAIKHOAN");
        bool canDelete = Presentation.Helpers.PermissionHelper.CanDelete("HETHONG.TAIKHOAN");
        btnThemVaiTro.Enabled = btnLuuPhanQuyen.Enabled = canEdit;
        dataGridViewImageColumn2.Visible = dataGridViewImageColumn3.Visible = dataGridViewImageColumn4.Visible = canEdit;
        colSuaVaiTro.Visible = canEdit;
        colXoaVaiTro.Visible = canDelete;
    }

    private static bool KiemTraQuyen(bool allowed, string message)
    {
        if (allowed) return true;
        MessageBox.Show(message, "Không đủ quyền", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return false;
    }

    private static bool DangODegsignMode() => LicenseManager.UsageMode == LicenseUsageMode.Designtime;




    private void GanSuKien()
    {
        btnTabDanhSachTaiKhoan.Click += (_, _) => HienThiTabTaiKhoan();
        btnTabPhanQuyen.Click += (_, _) => HienThiTabPhanQuyen();

        txtTimKiemTaiKhoan.TextChanged += (_, _) => LocTaiKhoan();
        cboVaiTro.SelectedIndexChanged += (_, _) => { if (!_dangTaiCombo) LocTaiKhoan(); };
        cboTrangThai.SelectedIndexChanged += (_, _) => { if (!_dangTaiCombo) LocTaiKhoan(); };
        btnLamMoi.Click += (_, _) => LamMoiTaiKhoan();
        btnThemTaiKhoan.Click += BtnThemTaiKhoan_Click;

        dgvVaiTro.CellContentClick += DgvTaiKhoan_CellContentClick;
        dgvVaiTro.CellDoubleClick += (_, e) => { if (e.RowIndex >= 0) XemChiTietTaiKhoan(e.RowIndex); };

        btnTrangDau.Click += (_, _) => ChuyenTrang(1);
        btnTrangTruoc.Click += (_, _) => ChuyenTrang(_trangHienTai - 1);
        btnTrangSau.Click += (_, _) => ChuyenTrang(_trangHienTai + 1);
        btnTrangCuoi.Click += (_, _) => ChuyenTrang(TongTrang());
        btnPage1.Click += BtnTrangSo_Click;
        btnPage2.Click += BtnTrangSo_Click;
        btnLastPage.Click += BtnTrangSo_Click;
        btnGo.Click += (_, _) => { if (int.TryParse(txtTrang.Text, out int page)) ChuyenTrang(page); };
        txtTrang.KeyPress += (_, e) => { if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) e.Handled = true; };

        btnLamMoiPhanQuyen.Click += (_, _) => { if (XacNhanBoThayDoi()) TaiTatCaDuLieu(); };
        btnThemVaiTro.Click += BtnThemVaiTro_Click;
        guna2DataGridView1.CellClick += DgvVaiTro_CellClick;
        guna2DataGridView1.CellContentClick += DgvVaiTro_CellContentClick;
        guna2DataGridView1.CellPainting += DgvVaiTro_CellPainting;
        guna2DataGridView1.CellMouseEnter += (_, e) =>
        {
            if (e.ColumnIndex == colSuaVaiTro.Index || e.ColumnIndex == colXoaVaiTro.Index)
                guna2DataGridView1.Cursor = Cursors.Hand;
        };
        guna2DataGridView1.CellMouseLeave += (_, _) => guna2DataGridView1.Cursor = Cursors.Default;
        cboVaiTroPhanQuyen.SelectedIndexChanged += (_, _) =>
        {
            if (_dangTaiCombo) return;
            if (!XacNhanBoThayDoi()) { _dangTaiCombo = true; cboVaiTroPhanQuyen.SelectedValue = _maVaiTroDangSua; _dangTaiCombo = false; return; }
            TaiQuyenVaiTroDangChon();
        };
        txtTimChucNang.TextChanged += (_, _) => LocPhanQuyen();
        cboNhomChucNang.SelectedIndexChanged += (_, _) => { if (!_dangTaiCombo) LocPhanQuyen(); };
        btnChonTatCa.Click += (_, _) => GanTatCaQuyen(true);
        btnBoChonTatCa.Click += (_, _) => GanTatCaQuyen(false);
        btnDatLaiMacDinh.Click += (_, _) => { if (MessageBox.Show("Đặt lại quyền mặc định cho vai trò này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) TaiQuyenVaiTroDangChon(forceDefault: true); };
        btnLuuPhanQuyen.Click += BtnLuuPhanQuyen_Click;
        dgvPhanQuyen.CellClick += DgvPhanQuyen_CellClick;
        dgvPhanQuyen.CurrentCellDirtyStateChanged += (_, _) =>
        {
            if (dgvPhanQuyen.IsCurrentCellDirty && dgvPhanQuyen.CurrentCell is DataGridViewCheckBoxCell)
                dgvPhanQuyen.CommitEdit(DataGridViewDataErrorContexts.Commit);
        };
        dgvPhanQuyen.CellValueChanged += DgvPhanQuyen_CellValueChanged;
    }

    private void UcTaiKhoanPhanQuyen_Load(object? sender, EventArgs e)
    {
        HienThiTabTaiKhoan();
        TaiTatCaDuLieu();
    }


    private void TaiTatCaDuLieu()
    {
        try
        {
            Cursor = Cursors.WaitCursor;
            _dangTaiCombo = true;
            _vaiTro = _service.LayDanhSachVaiTro();
            _taiKhoanGoc = _service.LayDanhSachTaiKhoan();

            var vaiTroLoc = new List<VaiTroGridModel>
            {
                new() { MaVaiTro = 0, TenVaiTro = "Tất cả vai trò" }
            };
            vaiTroLoc.AddRange(_vaiTro.Select(x => new VaiTroGridModel
            {
                MaVaiTro = x.MaVaiTro, TenVaiTro = x.TenVaiTro, MoTaText = x.MoTaText, SoNguoiDung = x.SoNguoiDung
            }));
            cboVaiTro.DataSource = vaiTroLoc;
            cboVaiTro.DisplayMember = nameof(VaiTroGridModel.TenVaiTro);
            cboVaiTro.ValueMember = nameof(VaiTroGridModel.MaVaiTro);
            cboVaiTro.SelectedIndex = 0;

            cboVaiTroPhanQuyen.DataSource = _vaiTro.ToList();
            cboVaiTroPhanQuyen.DisplayMember = nameof(VaiTroGridModel.TenVaiTro);
            cboVaiTroPhanQuyen.ValueMember = nameof(VaiTroGridModel.MaVaiTro);

            guna2DataGridView1.DataSource = _vaiTro;
            _dangTaiCombo = false;
            LocTaiKhoan();
            if (_vaiTro.Count > 0)
            {
                cboVaiTroPhanQuyen.SelectedIndex = 0;
                TaiQuyenVaiTroDangChon();
            }
        }
        catch (Exception ex)
        {
            _dangTaiCombo = false;
            MessageBox.Show("Không thể tải dữ liệu tài khoản và phân quyền.\n" + ex.Message, "Lỗi dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally { Cursor = Cursors.Default; }
    }

    private void LocTaiKhoan()
    {
        IEnumerable<TaiKhoanGridModel> query = _taiKhoanGoc;
        string keyword = txtTimKiemTaiKhoan.Text.Trim();
        if (keyword.Length > 0)
            query = query.Where(x => x.TenDangNhap.Contains(keyword, StringComparison.CurrentCultureIgnoreCase)
                || x.HoTen.Contains(keyword, StringComparison.CurrentCultureIgnoreCase)
                || x.Email.Contains(keyword, StringComparison.CurrentCultureIgnoreCase)
                || x.SoDienThoai.Contains(keyword, StringComparison.CurrentCultureIgnoreCase));

        if (cboVaiTro.SelectedItem is VaiTroGridModel role && role.MaVaiTro > 0)
            query = query.Where(x => x.MaVaiTro == role.MaVaiTro);

        if (cboTrangThai.SelectedIndex > 0)
            query = query.Where(x => x.TrangThai == cboTrangThai.Text);

        _taiKhoanLoc = query.ToList();
        _trangHienTai = 1;
        HienThiTrangTaiKhoan();
    }

    private int TongTrang() => Math.Max(1, (int)Math.Ceiling(_taiKhoanLoc.Count / (double)_kichThuocTrang));

    private void ChuyenTrang(int page)
    {
        _trangHienTai = Math.Clamp(page, 1, TongTrang());
        HienThiTrangTaiKhoan();
    }

    private void HienThiTrangTaiKhoan()
    {
        int totalPages = TongTrang();
        _trangHienTai = Math.Clamp(_trangHienTai, 1, totalPages);
        int start = (_trangHienTai - 1) * _kichThuocTrang;
        var page = _taiKhoanLoc.Skip(start).Take(_kichThuocTrang).Select((x, i) => new TaiKhoanGridModel
        {
            STT = start + i + 1, MaTaiKhoan = x.MaTaiKhoan, TenDangNhap = x.TenDangNhap, HoTen = x.HoTen,
            MaVaiTro = x.MaVaiTro, TenVaiTro = x.TenVaiTro, Email = x.Email, SoDienThoai = x.SoDienThoai,
            TrangThai = x.TrangThai, DangHoatDong = x.DangHoatDong
        }).ToList();
        dgvVaiTro.DataSource = page;

        int from = _taiKhoanLoc.Count == 0 ? 0 : start + 1;
        int to = Math.Min(start + _kichThuocTrang, _taiKhoanLoc.Count);
        lblPageInfo.Text = $"Hiển thị {from:N0}–{to:N0} của {_taiKhoanLoc.Count:N0} tài khoản";
        lblTongTrang.Text = $"/ {totalPages}";
        txtTrang.Text = _trangHienTai.ToString();
        CapNhatNutTrang(totalPages);
    }

    private void CapNhatNutTrang(int totalPages)
    {
        int start = Math.Max(1, _trangHienTai - 1);
        if (start + 1 > totalPages) start = Math.Max(1, totalPages - 1);
        btnPage1.Text = start.ToString();
        btnPage1.Visible = totalPages >= 1;
        btnPage2.Text = Math.Min(totalPages, start + 1).ToString();
        btnPage2.Visible = totalPages >= 2;
        btnLastPage.Text = totalPages.ToString();
        btnLastPage.Visible = totalPages > start + 1;
        lblDots.Visible = totalPages > start + 2;
        btnTrangDau.Enabled = btnTrangTruoc.Enabled = _trangHienTai > 1;
        btnTrangSau.Enabled = btnTrangCuoi.Enabled = _trangHienTai < totalPages;
        DinhDangNutTrang(btnPage1);
        DinhDangNutTrang(btnPage2);
        DinhDangNutTrang(btnLastPage);
    }

    private void DinhDangNutTrang(Guna.UI2.WinForms.Guna2Button button)
    {
        bool active = button.Visible && button.Text == _trangHienTai.ToString();
        button.FillColor = active ? Color.FromArgb(35, 90, 220) : Color.White;
        button.ForeColor = active ? Color.White : Color.FromArgb(35, 48, 90);
        button.BorderThickness = active ? 0 : 1;
        button.MinimumSize = new Size(Math.Max(42, TextRenderer.MeasureText(button.Text, button.Font).Width + 20), 36);
    }

    private void BtnTrangSo_Click(object? sender, EventArgs e)
    {
        if (sender is Control button && int.TryParse(button.Text, out int page)) ChuyenTrang(page);
    }

    private void LamMoiTaiKhoan()
    {
        txtTimKiemTaiKhoan.Clear();
        cboVaiTro.SelectedIndex = 0;
        cboTrangThai.SelectedIndex = 0;
        TaiTatCaDuLieu();
    }

    private TaiKhoanGridModel? LayTaiKhoanTheoDong(int rowIndex)
        => rowIndex >= 0 && rowIndex < dgvVaiTro.Rows.Count ? dgvVaiTro.Rows[rowIndex].DataBoundItem as TaiKhoanGridModel : null;

    private void DgvTaiKhoan_CellContentClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
        var model = LayTaiKhoanTheoDong(e.RowIndex);
        if (model == null) return;
        string col = dgvVaiTro.Columns[e.ColumnIndex].Name;
        if (col == dataGridViewImageColumn1.Name) XemChiTietTaiKhoan(e.RowIndex);
        else if (col == dataGridViewImageColumn2.Name) SuaVaiTroTaiKhoan(model);
        else if (col == dataGridViewImageColumn3.Name) DatLaiMatKhau(model);
        else if (col == dataGridViewImageColumn4.Name) KhoaMoTaiKhoan(model);
    }

    private void XemChiTietTaiKhoan(int rowIndex)
    {
        var x = LayTaiKhoanTheoDong(rowIndex);
        if (x == null) return;
        MessageBox.Show($"Tên đăng nhập: {x.TenDangNhap}\nHọ tên: {x.HoTen}\nVai trò: {x.TenVaiTro}\nEmail: {x.Email}\nSố điện thoại: {x.SoDienThoai}\nTrạng thái: {x.TrangThai}", "Chi tiết tài khoản", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void SuaVaiTroTaiKhoan(TaiKhoanGridModel model)
    {
        if (!KiemTraQuyen(Presentation.Helpers.PermissionHelper.CanEdit("HETHONG.TAIKHOAN"), "Bạn không có quyền thay đổi vai trò tài khoản.")) return;
        if (model.MaTaiKhoan == Presentation.Helpers.CurrentUser.MaTaiKhoan)
        {
            MessageBox.Show("Bạn không thể tự thay đổi vai trò của chính mình.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning); return;
        }
        using var form = TaoFormChonVaiTro("Cập nhật vai trò", model.MaVaiTro);
        if (form.ShowDialog(FindForm()) != DialogResult.OK || form.Tag is not int maVaiTro || maVaiTro == model.MaVaiTro) return;
        try { _service.CapNhatVaiTroTaiKhoan(model.MaTaiKhoan, maVaiTro); TaiTatCaDuLieu(); MessageBox.Show("Cập nhật vai trò thành công."); }
        catch (Exception ex) { MessageBox.Show(ex.Message, "Không thể cập nhật vai trò", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
    }

    private void DatLaiMatKhau(TaiKhoanGridModel model)
    {
        if (!KiemTraQuyen(Presentation.Helpers.PermissionHelper.CanEdit("HETHONG.TAIKHOAN"), "Bạn không có quyền đặt lại mật khẩu tài khoản.")) return;
        using var form = TaoFormDatLaiMatKhau(model.TenDangNhap);
        if (form.ShowDialog(FindForm()) != DialogResult.OK || form.Tag is not string password) return;
        try { _service.DatLaiMatKhau(model.MaTaiKhoan, password); MessageBox.Show("Đặt lại mật khẩu thành công.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        catch (Exception ex) { MessageBox.Show(ex.Message, "Không thể đặt lại mật khẩu", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
    }

    private void KhoaMoTaiKhoan(TaiKhoanGridModel model)
    {
        if (!KiemTraQuyen(Presentation.Helpers.PermissionHelper.CanEdit("HETHONG.TAIKHOAN"), "Bạn không có quyền khóa hoặc mở khóa tài khoản.")) return;
        if (model.DangHoatDong && model.MaTaiKhoan == Presentation.Helpers.CurrentUser.MaTaiKhoan)
        {
            MessageBox.Show("Bạn không thể tự khóa tài khoản của chính mình.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        string action = model.DangHoatDong ? "khóa" : "mở khóa";
        if (MessageBox.Show($"Bạn có chắc muốn {action} tài khoản '{model.TenDangNhap}'?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
        try { _service.DoiTrangThaiTaiKhoan(model.MaTaiKhoan, !model.DangHoatDong); TaiTatCaDuLieu(); }
        catch (Exception ex) { MessageBox.Show(ex.Message, $"Không thể {action} tài khoản", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
    }

    private void BtnThemTaiKhoan_Click(object? sender, EventArgs e)
    {
        if (!KiemTraQuyen(Presentation.Helpers.PermissionHelper.CanAdd("HETHONG.TAIKHOAN"), "Bạn không có quyền thêm tài khoản.")) return;
        using var form = TaoFormThemTaiKhoan();
        if (form.ShowDialog(FindForm()) != DialogResult.OK || form.Tag is not AccountInput input) return;
        try
        {
            _service.ThemTaiKhoan(input.TenDangNhap, input.MatKhau, input.MaVaiTro);
            TaiTatCaDuLieu();
            MessageBox.Show("Thêm tài khoản thành công.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex) { MessageBox.Show(ex.Message, "Không thể thêm tài khoản", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
    }



    private void TaiQuyenVaiTroDangChon(bool forceDefault = false)
    {
        if (cboVaiTroPhanQuyen.SelectedValue is not int maVaiTro)
        {
            return;
        }

        _phanQuyen = forceDefault ? _service.LayPhanQuyenMacDinh(maVaiTro) : _service.LayPhanQuyenTheoVaiTro(maVaiTro);
        _maVaiTroDangSua = maVaiTro;
        _coThayDoiQuyen = forceDefault;
        CapNhatDanhSachNhom();
        bool systemRole = _service.LaVaiTroQuanTri(maVaiTro);
        dgvPhanQuyen.ReadOnly = systemRole || !Presentation.Helpers.PermissionHelper.CanEdit("HETHONG.TAIKHOAN");
        btnChonTatCa.Enabled = btnBoChonTatCa.Enabled = btnDatLaiMacDinh.Enabled = !systemRole && !dgvPhanQuyen.ReadOnly;
        lblVaiTroDangChon.Text = cboVaiTroPhanQuyen.Text.ToUpperInvariant();
        LocPhanQuyen();
    }

    private void CapNhatDanhSachNhom()
    {
        string current = cboNhomChucNang.Text;
        _dangTaiCombo = true;
        cboNhomChucNang.Items.Clear(); cboNhomChucNang.Items.Add("Tất cả nhóm");
        foreach (string name in _phanQuyen.Where(x => x.LaDongNhom).Select(x => x.NhomChucNang).Distinct()) cboNhomChucNang.Items.Add(name);
        cboNhomChucNang.SelectedItem = cboNhomChucNang.Items.Contains(current) ? current : "Tất cả nhóm";
        _dangTaiCombo = false;
    }

    private bool XacNhanBoThayDoi()
    {
        if (!_coThayDoiQuyen) return true;
        return MessageBox.Show("Các thay đổi phân quyền chưa được lưu. Bạn có muốn bỏ các thay đổi này?", "Dữ liệu chưa lưu", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes;
    }


    private void LocPhanQuyen()
    {
        string keyword = txtTimChucNang.Text.Trim();
        string selectedGroup = cboNhomChucNang.SelectedIndex > 0
            ? cboNhomChucNang.Text
            : string.Empty;

        Dictionary<string, string> tenNhomTheoMa = _phanQuyen
            .Where(x => x.LaDongNhom)
            .GroupBy(x => x.MaNhom, StringComparer.OrdinalIgnoreCase)
            .ToDictionary(
                x => x.Key,
                x => x.First().NhomChucNang,
                StringComparer.OrdinalIgnoreCase);

        List<PhanQuyenGridModel> data = _phanQuyen
            .Where(x => !x.LaDongNhom)
            .Where(x =>
            {
                string tenNhom = tenNhomTheoMa.TryGetValue(x.MaNhom, out string? groupName)
                    ? groupName
                    : x.NhomChucNang;

                bool dungNhom = selectedGroup.Length == 0
                                || tenNhom.Equals(selectedGroup, StringComparison.CurrentCultureIgnoreCase);

                bool dungTuKhoa = keyword.Length == 0
                                  || tenNhom.Contains(keyword, StringComparison.CurrentCultureIgnoreCase)
                                  || x.TenChucNang.Contains(keyword, StringComparison.CurrentCultureIgnoreCase);

                return dungNhom && dungTuKhoa;
            })
            .ToList();

        foreach (PhanQuyenGridModel item in data)
        {
            if (tenNhomTheoMa.TryGetValue(item.MaNhom, out string? tenNhom))
            {
                item.NhomChucNang = tenNhom;
            }
        }

        dgvPhanQuyen.DataSource = null;
        dgvPhanQuyen.DataSource = data;
        dgvPhanQuyen.Invalidate();
    }


    private void DgvPhanQuyen_CellClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0)
        {
            dgvPhanQuyen.Invalidate();
        }
    }




    private void DgvPhanQuyen_CellValueChanged(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0 || e.ColumnIndex < 0 || dgvPhanQuyen.Rows[e.RowIndex].DataBoundItem is not PhanQuyenGridModel item || item.LaDongNhom) return;
        if (e.ColumnIndex == colXemQuyen.Index && !item.DuocXem) item.DuocThem = item.DuocSua = item.DuocXoa = item.DuocIn = item.DuocXuatExcel = false;
        if ((e.ColumnIndex == colThemQuyen.Index || e.ColumnIndex == colSuaQuyen.Index || e.ColumnIndex == colXoaQuyen.Index || e.ColumnIndex == colInQuyen.Index || e.ColumnIndex == colXuatExcelQuyen.Index) && (item.DuocThem || item.DuocSua || item.DuocXoa || item.DuocIn || item.DuocXuatExcel)) item.DuocXem = true;
        _coThayDoiQuyen = true;
        dgvPhanQuyen.InvalidateRow(e.RowIndex);
    }

    private void GanTatCaQuyen(bool value)
    {
        dgvPhanQuyen.EndEdit();
        foreach (var item in _phanQuyen.Where(x => !x.LaDongNhom)) item.DuocXem = item.DuocThem = item.DuocSua = item.DuocXoa = item.DuocIn = item.DuocXuatExcel = value;
        _coThayDoiQuyen = true;
        LocPhanQuyen();
    }

    private void BtnLuuPhanQuyen_Click(object? sender, EventArgs e)
    {
        if (!KiemTraQuyen(Presentation.Helpers.PermissionHelper.CanEdit("HETHONG.TAIKHOAN"), "Bạn không có quyền lưu phân quyền.")) return;
        if (cboVaiTroPhanQuyen.SelectedValue is not int maVaiTro) return;
        dgvPhanQuyen.EndEdit();
        try
        {
            _service.LuuPhanQuyen(maVaiTro, _phanQuyen); _coThayDoiQuyen = false;
            if (maVaiTro == Presentation.Helpers.CurrentUser.MaVaiTro) Presentation.Helpers.PermissionHelper.Load(maVaiTro);
            MessageBox.Show("Lưu phân quyền thành công.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex) { MessageBox.Show(ex.Message, "Không thể lưu phân quyền", MessageBoxButtons.OK, MessageBoxIcon.Error); }
    }

    private void DgvVaiTro_CellClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0 || guna2DataGridView1.Rows[e.RowIndex].DataBoundItem is not VaiTroGridModel role) return;
        cboVaiTroPhanQuyen.SelectedValue = role.MaVaiTro;
    }

    private void DgvVaiTro_CellContentClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0 || e.ColumnIndex < 0 || guna2DataGridView1.Rows[e.RowIndex].DataBoundItem is not VaiTroGridModel role) return;
        string name = guna2DataGridView1.Columns[e.ColumnIndex].Name;
        if (name == colSuaVaiTro.Name) SuaVaiTro(role);
        else if (name == colXoaVaiTro.Name) XoaVaiTro(role);
    }


    private void BtnThemVaiTro_Click(object? sender, EventArgs e)
    {
        if (!KiemTraQuyen(Presentation.Helpers.PermissionHelper.CanEdit("HETHONG.TAIKHOAN"), "Bạn không có quyền thêm vai trò.")) return;
        using var form = TaoFormNhapVaiTro("Thêm vai trò", string.Empty, string.Empty);
        if (form.ShowDialog(FindForm()) != DialogResult.OK || form.Tag is not RoleInput input) return;
        try { _service.ThemVaiTro(input.TenVaiTro, input.MoTa); TaiTatCaDuLieu(); }
        catch (Exception ex) { MessageBox.Show(ex.Message, "Không thể thêm vai trò", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
    }

    private void SuaVaiTro(VaiTroGridModel role)
    {
        if (!KiemTraQuyen(Presentation.Helpers.PermissionHelper.CanEdit("HETHONG.TAIKHOAN"), "Bạn không có quyền sửa vai trò.")) return;
        using var form = TaoFormNhapVaiTro("Sửa vai trò", role.TenVaiTro, role.MoTaText);
        if (form.ShowDialog(FindForm()) != DialogResult.OK || form.Tag is not RoleInput input) return;
        try { _service.SuaVaiTro(role.MaVaiTro, input.TenVaiTro, input.MoTa); TaiTatCaDuLieu(); }
        catch (Exception ex) { MessageBox.Show(ex.Message, "Không thể sửa vai trò", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
    }

    private void XoaVaiTro(VaiTroGridModel role)
    {
        if (!KiemTraQuyen(Presentation.Helpers.PermissionHelper.CanDelete("HETHONG.TAIKHOAN"), "Bạn không có quyền xóa vai trò.")) return;
        if (MessageBox.Show($"Xóa vai trò '{role.TenVaiTro}'?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
        try { _service.XoaVaiTro(role.MaVaiTro); TaiTatCaDuLieu(); }
        catch (Exception ex) { MessageBox.Show(ex.Message, "Không thể xóa vai trò", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
    }

    private void HienThiTabTaiKhoan()
    {
        pnlDanhSachTaiKhoan.Visible = true;
        pnlDanhSachTaiKhoan.BringToFront();
        pnlPhanQuyen.Visible = false;
        btnTabDanhSachTaiKhoan.ForeColor = Color.FromArgb(35, 90, 220);
        btnTabPhanQuyen.ForeColor = Color.FromArgb(50, 55, 85);
        pnlTabIndicator.Left = btnTabDanhSachTaiKhoan.Left;
        pnlTabIndicator.Width = btnTabDanhSachTaiKhoan.Width;
    }

    private void HienThiTabPhanQuyen()
    {
        pnlDanhSachTaiKhoan.Visible = false;
        pnlPhanQuyen.Visible = true;
        pnlPhanQuyen.BringToFront();
        btnTabDanhSachTaiKhoan.ForeColor = Color.FromArgb(50, 55, 85);
        btnTabPhanQuyen.ForeColor = Color.FromArgb(35, 90, 220);
        pnlTabIndicator.Left = btnTabPhanQuyen.Left;
        pnlTabIndicator.Width = btnTabPhanQuyen.Width;
    }

    private Form TaoFormChonVaiTro(string title, int selectedRole)
    {
        var form = TaoDialogBase(title, 420, 190);
        var combo = new ComboBox { Left = 25, Top = 55, Width = 350, DropDownStyle = ComboBoxStyle.DropDownList, DataSource = _vaiTro.ToList(), DisplayMember = nameof(VaiTroGridModel.TenVaiTro), ValueMember = nameof(VaiTroGridModel.MaVaiTro) };
        combo.SelectedValue = selectedRole;
        var ok = TaoNutDialog("Lưu", 275, 110, true);
        var cancel = TaoNutDialog("Hủy", 165, 110, false);
        ok.Click += (_, _) => { if (combo.SelectedValue is int id) { form.Tag = id; form.DialogResult = DialogResult.OK; } };
        cancel.Click += (_, _) => form.DialogResult = DialogResult.Cancel;
        form.Controls.AddRange(new Control[] { new Label { Left = 25, Top = 25, Text = "Vai trò", AutoSize = true }, combo, ok, cancel });
        return form;
    }

    private Form TaoFormDatLaiMatKhau(string username)
    {
        var form = TaoDialogBase("Đặt lại mật khẩu", 460, 285);
        var pass = new TextBox { Left = 165, Top = 55, Width = 240, UseSystemPasswordChar = true, MaxLength = 100 };
        var confirm = new TextBox { Left = 165, Top = 105, Width = 240, UseSystemPasswordChar = true, MaxLength = 100 };
        var ok = TaoNutDialog("Xác nhận", 310, 170, true); var cancel = TaoNutDialog("Hủy", 200, 170, false);
        ok.Click += (_, _) => { if (pass.Text != confirm.Text) { MessageBox.Show("Mật khẩu xác nhận không khớp."); return; } form.Tag = pass.Text; form.DialogResult = DialogResult.OK; };
        cancel.Click += (_, _) => form.DialogResult = DialogResult.Cancel; form.AcceptButton = ok; form.CancelButton = cancel;
        form.Controls.AddRange(new Control[] { new Label { Left=25,Top=20,Text=$"Tài khoản: {username}",AutoSize=true },new Label { Left=25,Top=59,Text="Mật khẩu mới",AutoSize=true },pass,new Label { Left=25,Top=109,Text="Xác nhận",AutoSize=true },confirm,ok,cancel });
        return form;
    }

    private Form TaoFormThemTaiKhoan()
    {
        var form = TaoDialogBase("Thêm tài khoản", 470, 365);
        var user = new TextBox { Left = 165, Top = 40, Width = 255, MaxLength = 50 };
        var pass = new TextBox { Left = 165, Top = 85, Width = 255, UseSystemPasswordChar = true, MaxLength = 100 };
        var confirm = new TextBox { Left = 165, Top = 130, Width = 255, UseSystemPasswordChar = true, MaxLength = 100 };
        var combo = new ComboBox { Left = 165, Top = 175, Width = 255, DropDownStyle = ComboBoxStyle.DropDownList, DataSource = _vaiTro.ToList(), DisplayMember = nameof(VaiTroGridModel.TenVaiTro), ValueMember = nameof(VaiTroGridModel.MaVaiTro) };
        var ok = TaoNutDialog("Thêm", 320, 245, true); var cancel = TaoNutDialog("Hủy", 210, 245, false);
        ok.Click += (_, _) => { if (pass.Text != confirm.Text) { MessageBox.Show("Mật khẩu xác nhận không khớp."); return; } if (combo.SelectedValue is not int id) return; form.Tag = new AccountInput(user.Text.Trim(), pass.Text, id); form.DialogResult = DialogResult.OK; };
        cancel.Click += (_, _) => form.DialogResult = DialogResult.Cancel;
        form.AcceptButton = ok; form.CancelButton = cancel;
        form.Controls.AddRange(new Control[] { new Label { Left=25,Top=44,Text="Tên đăng nhập",AutoSize=true },user,new Label { Left=25,Top=89,Text="Mật khẩu",AutoSize=true },pass,new Label { Left=25,Top=134,Text="Xác nhận mật khẩu",AutoSize=true },confirm,new Label { Left=25,Top=179,Text="Vai trò",AutoSize=true },combo,ok,cancel });
        return form;
    }

    private Form TaoFormNhapVaiTro(string title, string ten, string moTa)
    {
        var form = TaoDialogBase(title, 470, 300);
        var txtTen = new TextBox { Left = 135, Top = 45, Width = 285, Text = ten, MaxLength = 50 };
        var txtMoTa = new TextBox { Left = 135, Top = 90, Width = 285, Height = 75, Multiline = true, Text = moTa, MaxLength = 255 };
        var ok = TaoNutDialog("Lưu", 320, 210, true); var cancel = TaoNutDialog("Hủy", 210, 210, false);
        ok.Click += (_, _) => { form.Tag = new RoleInput(txtTen.Text.Trim(), txtMoTa.Text.Trim()); form.DialogResult = DialogResult.OK; };
        cancel.Click += (_, _) => form.DialogResult = DialogResult.Cancel; form.AcceptButton = ok; form.CancelButton = cancel;
        form.Controls.AddRange(new Control[] { new Label { Left = 25, Top = 49, Text = "Tên vai trò", AutoSize = true }, txtTen,
            new Label { Left = 25, Top = 94, Text = "Mô tả", AutoSize = true }, txtMoTa, ok, cancel });
        return form;
    }

    private static Form TaoDialogBase(string title, int width, int height) => new()
    {
        Text = title, Width = width, Height = height, StartPosition = FormStartPosition.CenterParent,
        FormBorderStyle = FormBorderStyle.FixedDialog, MaximizeBox = false, MinimizeBox = false, ShowInTaskbar = false,
        AutoScaleMode = AutoScaleMode.Dpi, Font = new Font("Segoe UI", 10F), BackColor = Color.White
    };

    private static Button TaoNutDialog(string text, int left, int top, bool primary) => new()
    {
        Text = text, Left = left, Top = top, Width = 95, Height = 36,
        BackColor = primary ? Color.FromArgb(35, 90, 220) : Color.White,
        ForeColor = primary ? Color.White : Color.FromArgb(35, 48, 90), FlatStyle = FlatStyle.Flat
    };

    private sealed record AccountInput(string TenDangNhap, string MatKhau, int MaVaiTro);
    private sealed record RoleInput(string TenVaiTro, string MoTa);

private void CauHinhGiaoDien()
    {
        SuspendLayout();
        BackColor = Color.FromArgb(248, 250, 255);

        lblTitle.Font = new Font("Segoe UI", 17F, FontStyle.Bold);
        lblTitle.ForeColor = Color.FromArgb(24, 38, 92);
        lblSubTitle.Font = new Font("Segoe UI", 9.5F);
        lblSubTitle.ForeColor = Color.FromArgb(105, 116, 150);
        picAvatar.BorderStyle = BorderStyle.None;
        picAvatar.IconSize = 60;
        lblUser.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
        lblRole.Font = new Font("Segoe UI", 9F);
        lblRole.ForeColor = Color.FromArgb(98, 108, 140);

        pnlTabs.BorderColor = Color.FromArgb(226, 232, 244);
        pnlTabs.BorderRadius = 12;
        pnlTabs.FillColor = Color.White;
        pnlTabs.ShadowDecoration.Enabled = true;
        pnlTabs.ShadowDecoration.Depth = 4;
        pnlTabs.ShadowDecoration.Color = Color.FromArgb(145, 125, 220);
        pnlTabIndicator.FillColor = Color.FromArgb(38, 100, 235);
        pnlTabIndicator.BorderThickness = 0;
        foreach (var tab in new[] { btnTabDanhSachTaiKhoan, btnTabPhanQuyen })
        {
            tab.Animated = true;
            tab.Cursor = Cursors.Hand;
            tab.FillColor = Color.White;
            tab.HoverState.FillColor = Color.FromArgb(244, 247, 255);
            tab.HoverState.ForeColor = Color.FromArgb(38, 100, 235);
        }

        foreach (Guna.UI2.WinForms.Guna2Panel panel in new[]
                 { pnlDanhSachTaiKhoan, pnlPhanQuyen, guna2Panel1, pnlDanhSachVaiTro, pnlChiTietQuyen })
        {
            panel.BorderColor = Color.FromArgb(226, 232, 244);
            panel.BorderRadius = 12;
            panel.BorderThickness = 1;
            panel.FillColor = Color.White;
        }
        foreach (Guna.UI2.WinForms.Guna2Panel panel in new[] { guna2Panel1, pnlDanhSachVaiTro, pnlChiTietQuyen })
        {
            panel.ShadowDecoration.Enabled = true;
            panel.ShadowDecoration.Depth = 5;
            panel.ShadowDecoration.Color = Color.FromArgb(145, 125, 220);
        }
        foreach (Guna.UI2.WinForms.Guna2Panel panel in new[] { pnlFilterTaiKhoan, pnlFilterPhanQuyen })
        {
            panel.BorderColor = Color.FromArgb(226, 232, 244);
            panel.BorderRadius = 10;
            panel.BorderThickness = 1;
            panel.FillColor = Color.FromArgb(253, 254, 255);
        }

        label4.Text = "Nhóm chức năng";
        label5.ForeColor = label9.ForeColor = Color.FromArgb(27, 43, 92);
        lblVaiTroDangChon.ForeColor = Color.FromArgb(38, 100, 235);

        dgvVaiTro.AutoGenerateColumns = false;
        guna2DataGridView1.AutoGenerateColumns = false;
        dgvPhanQuyen.AutoGenerateColumns = false;
        CauHinhBangDanhSachVaiTro();

        dataGridViewTextBoxColumn1.Tag = "STT";
        dataGridViewTextBoxColumn2.Tag = "TenDangNhap";
        dataGridViewTextBoxColumn3.Tag = "HoTen";
        dataGridViewTextBoxColumn4.Tag = "TenVaiTro";
        dataGridViewTextBoxColumn5.Tag = "Email";
        dataGridViewTextBoxColumn6.Tag = "SoDienThoai";
        dataGridViewTextBoxColumn7.Tag = "TrangThai";

        foreach (Control control in new Control[]
                 { guna2Button1, textBox1, label7, label8 })
            control.Visible = false;

        label6.Visible = true;
        guna2Button2.Visible = true;
        guna2Button3.Visible = true;
        guna2Button4.Visible = true;

        TrangTriNut(btnThemTaiKhoan, Color.FromArgb(38, 100, 235), Color.White, false);
        TrangTriNut(btnThemVaiTro, Color.FromArgb(38, 100, 235), Color.White, false);
        TrangTriNut(btnLuuPhanQuyen, Color.FromArgb(38, 100, 235), Color.White, false);
        TrangTriNut(btnLamMoi, Color.White, Color.FromArgb(45, 58, 105), true);
        TrangTriNut(btnLamMoiPhanQuyen, Color.White, Color.FromArgb(45, 58, 105), true);
        TrangTriNut(btnChonTatCa, Color.FromArgb(232, 250, 239), Color.FromArgb(22, 151, 83), true);
        TrangTriNut(btnBoChonTatCa, Color.FromArgb(255, 247, 238), Color.FromArgb(225, 116, 35), true);
        TrangTriNut(btnDatLaiMacDinh, Color.White, Color.FromArgb(38, 100, 235), true);
        TrangTriNut(btnGo, Color.FromArgb(38, 100, 235), Color.White, false);

        foreach (var input in new[] { txtTimKiemTaiKhoan, txtTimChucNang })
        {
            input.BorderColor = Color.FromArgb(218, 225, 239);
            input.FocusedState.BorderColor = Color.FromArgb(71, 96, 235);
            input.HoverState.BorderColor = Color.FromArgb(154, 169, 225);
        }
        foreach (var combo in new[] { cboVaiTro, cboTrangThai, cboVaiTroPhanQuyen, cboNhomChucNang })
        {
            combo.BorderColor = Color.FromArgb(218, 225, 239);
            combo.FocusedState.BorderColor = Color.FromArgb(71, 96, 235);
            combo.HoverState.BorderColor = Color.FromArgb(154, 169, 225);
        }

        cboTrangThai.Items.Clear();
        cboTrangThai.Items.AddRange(new object[] { "Tất cả trạng thái", "Hoạt động", "Bị khóa" });
        cboTrangThai.SelectedIndex = 0;
        cboNhomChucNang.Items.Clear();
        cboNhomChucNang.Items.AddRange(new object[]
        {
            "Tất cả nhóm", "Quản lý sách", "Quản lý độc giả", "Mượn – Trả – Phạt",
            "Nhập sách", "Thống kê báo cáo", "Hệ thống"
        });
        cboNhomChucNang.SelectedIndex = 0;

        foreach (var page in new[] { btnPage1, btnPage2, btnLastPage })
        {
            page.AutoSize = true;
        }

        ResumeLayout(false);
    }

    private void CauHinhBangDanhSachVaiTro()
    {
        var grid = guna2DataGridView1;
        grid.BackgroundColor = Color.White;
        grid.BorderStyle = BorderStyle.None;
        grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        grid.GridColor = Color.FromArgb(226, 232, 242);
        grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
        grid.ColumnHeadersHeight = 48;
        grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        grid.RowTemplate.Height = 71;
        grid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
        grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        grid.DefaultCellStyle.Font = new Font("Segoe UI", 9F);
        grid.DefaultCellStyle.ForeColor = Color.FromArgb(31, 48, 92);
        grid.DefaultCellStyle.BackColor = Color.White;
        grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(247, 250, 255);
        grid.DefaultCellStyle.SelectionForeColor = Color.FromArgb(31, 48, 92);
        grid.DefaultCellStyle.WrapMode = DataGridViewTriState.False;
        grid.DefaultCellStyle.Padding = new Padding(5, 0, 5, 0);
        grid.AlternatingRowsDefaultCellStyle.BackColor = Color.White;
        grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
        grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(45, 58, 100);
        grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(248, 250, 254);
        grid.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(248, 250, 254);
        grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
        grid.EnableHeadersVisualStyles = false;

        colSTTVaiTro.Width = 42;
        colSTTVaiTro.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        colTenVaiTro.Width = 128;
        colMoTa.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        colMoTa.MinimumWidth = 145;
        colSoNguoiDung.Width = 92;
        colSoNguoiDung.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        colSuaVaiTro.Width = 44;
        colXoaVaiTro.Width = 44;
        colSuaVaiTro.HeaderText = "THAO TÁC";
        colXoaVaiTro.HeaderText = string.Empty;
        colSuaVaiTro.DefaultCellStyle.Padding = new Padding(11);
        colXoaVaiTro.DefaultCellStyle.Padding = new Padding(11);

        grid.DataBindingComplete += (_, _) =>
        {
            for (int i = 0; i < grid.Rows.Count; i++)
            {
                grid.Rows[i].Height = 58;
                grid.Rows[i].Cells[colSTTVaiTro.Index].Value = i + 1;
            }
            label6.Text = grid.Rows.Count == 0
                ? "Không có vai trò"
                : $"Hiển thị 1–{grid.Rows.Count:N0} của {grid.Rows.Count:N0} vai trò";
        };

        label6.Location = new Point(12, 548);
        label6.Font = new Font("Segoe UI", 8.5F);
        label6.ForeColor = Color.FromArgb(54, 69, 110);
        guna2Button3.Location = new Point(303, 540);
        guna2Button4.Location = new Point(358, 540);
        guna2Button2.Location = new Point(417, 540);
        guna2Button3.Size = guna2Button2.Size = new Size(38, 38);
        guna2Button4.Size = new Size(42, 38);
        foreach (var button in new[] { guna2Button2, guna2Button3, guna2Button4 })
        {
            button.BorderRadius = 7;
            button.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        }
        guna2Button3.Text = "‹";
        guna2Button2.Text = "›";
        guna2Button4.Text = "1";
        guna2Button3.FillColor = guna2Button2.FillColor = Color.White;
        guna2Button3.BorderColor = guna2Button2.BorderColor = Color.FromArgb(218, 225, 238);
        guna2Button3.ForeColor = guna2Button2.ForeColor = Color.FromArgb(46, 63, 108);
        guna2Button4.FillColor = Color.FromArgb(32, 91, 229);
        guna2Button4.ForeColor = Color.White;
    }

    private void DgvVaiTro_CellPainting(object? sender, DataGridViewCellPaintingEventArgs e)
    {
        if (e.RowIndex < 0 || e.ColumnIndex != colTenVaiTro.Index ||
            guna2DataGridView1.Rows[e.RowIndex].DataBoundItem is not VaiTroGridModel role)
            return;

        e.PaintBackground(e.CellBounds, true);
        Color[] accentColors =
        {
            Color.FromArgb(119, 74, 235), Color.FromArgb(34, 105, 226),
            Color.FromArgb(230, 119, 39), Color.FromArgb(42, 166, 89),
            Color.FromArgb(232, 67, 98)
        };
        Color accent = accentColors[Math.Abs(role.MaVaiTro) % accentColors.Length];
        Color background = Color.FromArgb(
            (accent.R + 255 * 5) / 6,
            (accent.G + 255 * 5) / 6,
            (accent.B + 255 * 5) / 6);

        string text = role.TenVaiTro;
        using var badgeFont = new Font("Segoe UI", 8.2F, FontStyle.Bold);
        Size textSize = TextRenderer.MeasureText(text, badgeFont);
        int badgeWidth = Math.Min(e.CellBounds.Width - 14, textSize.Width + 24);
        var badge = new Rectangle(e.CellBounds.X + 7,
            e.CellBounds.Y + (e.CellBounds.Height - 30) / 2, badgeWidth, 30);

        using var path = new GraphicsPath();
        int radius = 7;
        path.AddArc(badge.X, badge.Y, radius * 2, radius * 2, 180, 90);
        path.AddArc(badge.Right - radius * 2, badge.Y, radius * 2, radius * 2, 270, 90);
        path.AddArc(badge.Right - radius * 2, badge.Bottom - radius * 2, radius * 2, radius * 2, 0, 90);
        path.AddArc(badge.X, badge.Bottom - radius * 2, radius * 2, radius * 2, 90, 90);
        path.CloseFigure();
        using var brush = new SolidBrush(background);
        e.Graphics.FillPath(brush, path);
        TextRenderer.DrawText(e.Graphics, text, badgeFont,
            new Rectangle(badge.X + 10, badge.Y, badge.Width - 14, badge.Height), accent,
            TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.EndEllipsis);
        e.Handled = true;
    }

    private static void TrangTriNut(Guna.UI2.WinForms.Guna2Button button, Color fill, Color foreground, bool bordered)
    {
        button.Animated = true;
        button.Cursor = Cursors.Hand;
        button.BorderRadius = 8;
        button.FillColor = fill;
        button.ForeColor = foreground;
        button.BorderThickness = bordered ? 1 : 0;
        button.BorderColor = bordered ? ControlPaint.Light(foreground, 0.55F) : Color.Transparent;
        button.HoverState.FillColor = ControlPaint.Light(fill, 0.08F);
        button.HoverState.ForeColor = foreground;
    }


}
