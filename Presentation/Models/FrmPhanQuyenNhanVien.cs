using BusinessLayer.Models;
using BusinessLayer.Services;
using DataLayer.Models;
using Presentation.Helpers;

namespace Presentation.Models;

public partial class FrmPhanQuyenNhanVien : Form
{
    private readonly int _maNhanVien;
    private readonly NhanVienService _nhanVienService = new();
    private readonly TaiKhoanService _taiKhoanService = new();
    private NhanVienDetailModel? _employee;
    private List<PhanQuyenGridModel> _permissions = [];
    private bool _loading;

    private readonly Dictionary<string, Guna.UI2.WinForms.Guna2CheckBox[]> _moduleChecks;

    public FrmPhanQuyenNhanVien(int maNhanVien)
    {
        _maNhanVien = maNhanVien;
        InitializeComponent();
        _moduleChecks = new(StringComparer.OrdinalIgnoreCase)
        {
            ["SACH"] = [chkBooksView, chkBooksAdd, chkBooksEdit, chkBooksDelete],
            ["DOCGIA"] = [chkReadersView, chkReadersAdd, chkReadersEdit, chkReadersLock, chkReadersFee],
            ["MUONTRA"] = [chkBorrowCreate, chkBorrowReturn, chkBorrowFine, chkBorrowCollect, chkBorrowHistory],
            ["NHAPSACH"] = [chkImportCreate, chkImportTitle, chkImportSupplier, chkImportPrint],
            ["BAOCAO"] = [chkReportView, chkReportExport],
            ["HETHONG"] = [chkSystemConfig, chkSystemUsers, chkSystemBackup, chkSystemAudit]
        };
        WireEvents();
    }

    private void WireEvents()
    {
        Load += FrmPhanQuyenNhanVien_Load;
        cboRole.SelectedIndexChanged += CboRole_SelectedIndexChanged;
        btnSelectAll.Click += (_, _) => SetAllChecks(true);
        btnClearAll.Click += (_, _) => SetAllChecks(false);
        btnRoleDefault.Click += (_, _) => LoadRolePermissions(defaults: true);
        btnReset.Click += (_, _) => LoadRolePermissions(defaults: false);
        btnCancel.Click += (_, _) => Close();
        btnSave.Click += (_, _) => SavePermissions(closeAfterSave: false);
        btnSaveClose.Click += (_, _) => SavePermissions(closeAfterSave: true);
        foreach (var check in AllChecks()) check.CheckedChanged += (_, _) => { if (!_loading) RefreshSummary(); };
    }

    private void FrmPhanQuyenNhanVien_Load(object? sender, EventArgs e)
    {
        try
        {
            _loading = true;
            if (!LaQuanTri())
                throw new UnauthorizedAccessException("Chỉ quản trị viên được phép phân quyền nhân viên.");
            _employee = _nhanVienService.GetChiTiet(_maNhanVien)
                        ?? throw new InvalidOperationException("Không tìm thấy nhân viên.");
            if (!_employee.MaTaiKhoan.HasValue)
                throw new InvalidOperationException("Nhân viên này chưa có tài khoản để phân quyền.");

            BindEmployee();
            BindLookups();
            BindAdministrator();
            LoadRolePermissions(defaults: false);
            BindAuditSummary();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Không thể phân quyền", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            Close();
        }
        finally { _loading = false; RefreshSummary(); }
    }

    private void BindEmployee()
    {
        if (_employee == null) return;
        lblEmployeeName.Text = _employee.HoTen;
        lblEmployeeCode.Text = _employee.MaNhanVienText;
        lblPosition.Text = _employee.ChucVu;
        lblEmail.Text = string.IsNullOrWhiteSpace(_employee.Email) ? "-" : _employee.Email;
        lblPhone.Text = string.IsNullOrWhiteSpace(_employee.SoDienThoai) ? "-" : _employee.SoDienThoai;
        lblStartDate.Text = _employee.NgayVaoLam.ToString("dd/MM/yyyy");
        lblDepartment.Text = "Thư viện";
        lblAddress.Text = string.IsNullOrWhiteSpace(_employee.DiaChi) ? "-" : _employee.DiaChi;
        picAvatar.Image?.Dispose();
        picAvatar.Image = DatabaseImageHelper.LoadStaffAvatar(_employee.AnhDaiDien, _employee.HoTen, 120, 120);
        badgeWorkStatus.Text = _employee.DangLamViec ? "●  Đang làm việc" : "●  Tạm nghỉ";
    }

    private void BindAdministrator()
    {
        lblAdminHello.Text = $"Xin chào, {(string.IsNullOrWhiteSpace(CurrentUser.HoTen) ? CurrentUser.TenDangNhap : CurrentUser.HoTen)}";
        lblAdminRole.Text = string.IsNullOrWhiteSpace(CurrentUser.VaiTro) ? "Quản trị viên" : CurrentUser.VaiTro;
        CurrentUserAvatarHelper.Apply(picAdmin);
    }

    private void BindLookups()
    {
        var roles = _taiKhoanService.LayDanhSachVaiTro();
        cboRole.DataSource = roles;
        cboRole.DisplayMember = nameof(VaiTroGridModel.TenVaiTro);
        cboRole.ValueMember = nameof(VaiTroGridModel.MaVaiTro);
        cboPermissionGroup.Items.Clear();
        cboPermissionGroup.Items.AddRange(["Quản trị hệ thống", "Nghiệp vụ thư viện", "Nghiệp vụ chuyên môn", "Tra cứu báo cáo"]);
        cboScope.Items.Clear(); cboScope.Items.AddRange(["Toàn hệ thống", "Phân hệ được cấp", "Chỉ xem dữ liệu"]);
        cboAccountStatus.Items.Clear(); cboAccountStatus.Items.AddRange(["●  Hoạt động", "●  Bị khóa"]);
        if (_employee?.MaVaiTro is int roleId) cboRole.SelectedValue = roleId;
        cboPermissionGroup.SelectedItem = ResolveGroup(cboRole.Text);
        cboScope.SelectedItem = ResolveScope(cboRole.Text);
        cboAccountStatus.SelectedIndex = _employee?.TaiKhoanHoatDong == true ? 0 : 1;
    }

    private void CboRole_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (_loading || cboRole.SelectedValue is not int) return;
        cboPermissionGroup.SelectedItem = ResolveGroup(cboRole.Text);
        cboScope.SelectedItem = ResolveScope(cboRole.Text);
        LoadRolePermissions(defaults: false);
    }

    private void LoadRolePermissions(bool defaults)
    {
        if (cboRole.SelectedValue is not int roleId) return;
        _loading = true;
        try
        {
            _permissions = defaults
                ? _taiKhoanService.LayPhanQuyenMacDinh(roleId)
                : _taiKhoanService.LayPhanQuyenTheoVaiTro(roleId);
            ApplyPermissionsToChecks();
        }
        finally { _loading = false; RefreshSummary(); }
    }

    private void ApplyPermissionsToChecks()
    {
        foreach (var pair in _moduleChecks)
        {
            var rows = _permissions.Where(x => !x.LaDongNhom && x.MaNhom.Equals(pair.Key, StringComparison.OrdinalIgnoreCase)).ToList();
            bool[] values = pair.Key switch
            {
                "SACH" => [rows.Any(x => x.DuocXem), rows.Any(x => x.DuocThem), rows.Any(x => x.DuocSua), rows.Any(x => x.DuocXoa)],
                "DOCGIA" => [rows.Any(x => x.DuocXem), rows.Any(x => x.DuocThem), rows.Any(x => x.DuocSua), rows.Any(x => x.DuocXoa), rows.Any(x => x.DuocIn)],
                "MUONTRA" => [rows.Any(x => x.DuocThem), rows.Any(x => x.DuocSua), rows.Any(x => x.DuocXoa), rows.Any(x => x.DuocIn), rows.Any(x => x.DuocXem)],
                "NHAPSACH" => [rows.Any(x => x.DuocThem), rows.Any(x => x.DuocSua), rows.Any(x => x.DuocXoa), rows.Any(x => x.DuocIn)],
                "BAOCAO" => [rows.Any(x => x.DuocXem), rows.Any(x => x.DuocXuatExcel)],
                "HETHONG" => [rows.Any(x => x.DuocSua), rows.Any(x => x.DuocThem), rows.Any(x => x.DuocXuatExcel), rows.Any(x => x.DuocXem)],
                _ => []
            };
            for (int i = 0; i < pair.Value.Length; i++) pair.Value[i].Checked = i < values.Length && values[i];
        }
    }

    private void CollectChecksIntoPermissions()
    {
        ApplyGroup("SACH", chkBooksView.Checked, chkBooksAdd.Checked, chkBooksEdit.Checked, chkBooksDelete.Checked, false, false);
        ApplyGroup("DOCGIA", chkReadersView.Checked, chkReadersAdd.Checked, chkReadersEdit.Checked, chkReadersLock.Checked, chkReadersFee.Checked, false);
        ApplyGroup("MUONTRA", chkBorrowHistory.Checked, chkBorrowCreate.Checked, chkBorrowReturn.Checked, chkBorrowFine.Checked, chkBorrowCollect.Checked, false);
        ApplyGroup("NHAPSACH", true, chkImportCreate.Checked, chkImportTitle.Checked, chkImportSupplier.Checked, chkImportPrint.Checked, false);
        ApplyGroup("BAOCAO", chkReportView.Checked, false, false, false, false, chkReportExport.Checked);
        ApplyGroup("HETHONG", chkSystemAudit.Checked, chkSystemUsers.Checked, chkSystemConfig.Checked, false, false, chkSystemBackup.Checked);
    }

    private void ApplyGroup(string code, bool view, bool add, bool edit, bool delete, bool print, bool export)
    {
        foreach (var item in _permissions.Where(x => !x.LaDongNhom && x.MaNhom.Equals(code, StringComparison.OrdinalIgnoreCase)))
        {
            item.DuocXem = view; item.DuocThem = add; item.DuocSua = edit; item.DuocXoa = delete; item.DuocIn = print; item.DuocXuatExcel = export;
        }
    }

    private void SetAllChecks(bool value)
    {
        _loading = true;
        try { foreach (var check in AllChecks()) check.Checked = value; }
        finally { _loading = false; RefreshSummary(); }
    }

    private IEnumerable<Guna.UI2.WinForms.Guna2CheckBox> AllChecks() => _moduleChecks.Values.SelectMany(x => x);

    private void RefreshSummary()
    {
        int granted = AllChecks().Count(x => x.Checked);
        lblGrantedCount.Text = granted.ToString();
        lblCurrentRole.Text = string.IsNullOrWhiteSpace(cboRole.Text) ? "-" : cboRole.Text;
        lblCurrentGroup.Text = string.IsNullOrWhiteSpace(cboPermissionGroup.Text) ? "-" : cboPermissionGroup.Text;
        string level = granted >= 20 ? "Cao" : granted >= 10 ? "Trung bình" : "Thấp";
        badgeAccessLevel.Text = level;
        badgeAccountStatus.Text = cboAccountStatus.SelectedIndex == 1 ? "Bị khóa" : "Hoạt động";
    }

    private void BindAuditSummary()
    {
        string actor = string.IsNullOrWhiteSpace(CurrentUser.HoTen) ? "Admin" : CurrentUser.HoTen;
        string now = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
        lblUpdatedAt.Text = now; lblUpdatedBy.Text = actor;
        lblAudit1Time.Text = now; lblAudit1User.Text = actor;
        lblAudit2Time.Text = _employee?.NgayVaoLam.ToString("dd/MM/yyyy HH:mm") ?? "-"; lblAudit2User.Text = "Admin";
        lblAudit3Time.Text = _employee?.NgayVaoLam.ToString("dd/MM/yyyy HH:mm") ?? "-"; lblAudit3User.Text = "Admin";
    }

    private void SavePermissions(bool closeAfterSave)
    {
        if (_employee == null || cboRole.SelectedValue is not int roleId) return;

        bool targetActive = cboAccountStatus.SelectedIndex != 1;
        bool isCurrentAccount = _employee.MaTaiKhoan == CurrentUser.MaTaiKhoan;
        if (isCurrentAccount && !targetActive)
        {
            MessageBox.Show("Bạn không thể tự khóa tài khoản của chính mình.", "Không thể thực hiện",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        if (isCurrentAccount && roleId != CurrentUser.MaVaiTro)
        {
            MessageBox.Show("Bạn không thể tự thay đổi vai trò của chính mình.", "Không thể thực hiện",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        string confirm = "Lưu vai trò và bộ quyền đang chọn?\n\n" +
                         "Lưu ý: quyền chi tiết được áp dụng cho TẤT CẢ tài khoản cùng vai trò.";
        if (MessageBox.Show(confirm, "Xác nhận quyền theo vai trò", MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning) != DialogResult.Yes) return;
        try
        {
            Cursor = Cursors.WaitCursor;
            btnSave.Enabled = btnSaveClose.Enabled = false;
            CollectChecksIntoPermissions();
            _taiKhoanService.LuuPhanQuyen(roleId, _permissions);
            if (_employee.MaVaiTro != roleId && !_nhanVienService.CapNhatVaiTroTheoMa(_maNhanVien, roleId))
                throw new InvalidOperationException("Không thể cập nhật vai trò của nhân viên.");
            if (_employee.TaiKhoanHoatDong != targetActive && _employee.MaTaiKhoan.HasValue)
                _taiKhoanService.DoiTrangThaiTaiKhoan(_employee.MaTaiKhoan.Value, targetActive);

            _employee = _nhanVienService.GetChiTiet(_maNhanVien)
                        ?? throw new InvalidOperationException("Không thể tải lại nhân viên sau khi lưu.");
            BindEmployee();
            BindAuditSummary();
            MessageBox.Show("Đã lưu quyền theo vai trò thành công.", "Thành công",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            if (closeAfterSave)
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Không thể lưu phân quyền", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            Cursor = Cursors.Default;
            btnSave.Enabled = btnSaveClose.Enabled = true;
        }
    }

    private static bool LaQuanTri() =>
        CurrentUser.MaVaiTro == 1 ||
        CurrentUser.VaiTro.Contains("quản trị", StringComparison.CurrentCultureIgnoreCase);

    private static string ResolveGroup(string role) => role.Contains("quản trị", StringComparison.CurrentCultureIgnoreCase) ? "Quản trị hệ thống" : role.Contains("thủ thư", StringComparison.CurrentCultureIgnoreCase) ? "Nghiệp vụ thư viện" : "Nghiệp vụ chuyên môn";
    private static string ResolveScope(string role) => role.Contains("quản trị", StringComparison.CurrentCultureIgnoreCase) ? "Toàn hệ thống" : "Phân hệ được cấp";
}
