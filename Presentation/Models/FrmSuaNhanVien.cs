using BusinessLayer.Services;
using DataLayer.Models;
using Presentation.Helpers;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace Presentation.Models;

public partial class FrmSuaNhanVien : Form
{
    private readonly int _maNhanVien;
    private readonly NhanVienService _service = new();
    private NhanVienDetailModel? _banDau;
    private string? _anhMoi;
    private bool _luuVaDong;

    public FrmSuaNhanVien(int id)
    {
        InitializeComponent();
        _maNhanVien = id;
        if (DesignModeHelper.IsDesignMode(this)) return;

        AcceptButton = btnLuuNhanVien;
        CancelButton = btnHuy;
        Load += TaiForm;
        btnChonAnh.Click += ChonAnh;
        btnXoaAnh.Click += (_, _) => XoaAnh();
        btnKhoiPhuc.Click += (_, _) => GanDuLieu(_banDau);
        btnHuy.Click += (_, _) => { DialogResult = DialogResult.Cancel; Close(); };
        btnLuuNhanVien.Click += (_, _) => { _luuVaDong = false; Luu(); };
        btnLuuDong.Click += (_, _) => { _luuVaDong = true; Luu(); };
        btnPhanQuyen.Click += PhanQuyen;
        btnDatLaiMatKhau.Click += DatLaiMatKhau;
        txtSoDienThoai.KeyPress += (_, e) =>
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) e.Handled = true;
        };
    }

    private static bool LaQuanTri() =>
        CurrentUser.MaVaiTro == 1 ||
        CurrentUser.VaiTro.Contains("quản trị", StringComparison.CurrentCultureIgnoreCase);

    private bool YeuCauQuanTri()
    {
        if (LaQuanTri()) return true;
        MessageBox.Show("Chỉ quản trị viên được thực hiện thao tác này.", "Không đủ quyền",
            MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return false;
    }

    private void TaiForm(object? sender, EventArgs e)
    {
        try
        {
            cboChucVu.DataSource = _service.GetChucVu().OrderBy(x => x).ToList();
            cboVaiTro.DataSource = _service.GetVaiTroLookup();
            cboVaiTro.DisplayMember = nameof(VaiTroLookupModel.TenVaiTro);
            cboVaiTro.ValueMember = nameof(VaiTroLookupModel.MaVaiTro);
            _banDau = _service.GetChiTiet(_maNhanVien)
                      ?? throw new InvalidOperationException("Không tìm thấy nhân viên.");
            GanDuLieu(_banDau);
            lblXinChao.Text = $"Xin chào, {CurrentUser.HoTen}";
            lblVaiTroHeader.Text = CurrentUser.VaiTro;
            lblNguoiCapNhat.Text = CurrentUser.HoTen;
        }
        catch (Exception ex)
        {
            MessageBox.Show("Không thể tải thông tin.\n" + Loi(ex), "Lỗi dữ liệu",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            Close();
        }
    }

    private void GanDuLieu(NhanVienDetailModel? model)
    {
        if (model == null) return;
        _anhMoi = null;
        txtMaNhanVien.Text = model.MaNhanVienText;
        txtHoTen.Text = model.HoTen;
        txtSoDienThoai.Text = model.SoDienThoai;
        txtEmail.Text = model.Email;
        txtDiaChi.Text = model.DiaChi;
        txtTenDangNhap.Text = model.TenDangNhap;
        dtpNgaySinh.Value = model.NgaySinh?.ToDateTime(TimeOnly.MinValue) ?? DateTime.Today.AddYears(-22);
        dtpNgayVaoLam.Value = model.NgayVaoLam.ToDateTime(TimeOnly.MinValue);
        rdoNam.Checked = model.GioiTinh == "Nam";
        rdoNu.Checked = model.GioiTinh == "Nữ";
        rdoKhac.Checked = !rdoNam.Checked && !rdoNu.Checked;
        rdoDangLamViec.Checked = model.DangLamViec;
        rdoTamNghi.Checked = !model.DangLamViec;
        cboChucVu.SelectedItem = cboChucVu.Items.Cast<object>().FirstOrDefault(x =>
            string.Equals(x.ToString(), model.ChucVu, StringComparison.CurrentCultureIgnoreCase));
        cboVaiTro.SelectedValue = model.MaVaiTro ?? 0;
        btnTrangThaiTaiKhoan.Text = model.TaiKhoanHoatDong ? "●  Hoạt động" : "●  Đã khóa";
        lblLanDangNhapCuoi.Text = model.LanDangNhapCuoi?.ToString("dd/MM/yyyy HH:mm") ?? "Chưa đăng nhập";
        bool duocQuanTriTaiKhoan = model.MaTaiKhoan.HasValue && LaQuanTri();
        btnPhanQuyen.Enabled = duocQuanTriTaiKhoan;
        btnDatLaiMatKhau.Enabled = duocQuanTriTaiKhoan;
        HienAnh(model.AnhDaiDien, model.HoTen);
    }

    private bool KiemTra()
    {
        if (string.IsNullOrWhiteSpace(txtHoTen.Text))
            return BaoLoi(txtHoTen, "Vui lòng nhập họ tên.");
        if (dtpNgaySinh.Value.Date >= DateTime.Today)
            return BaoLoi(dtpNgaySinh, "Ngày sinh phải nhỏ hơn ngày hiện tại.");
        if (dtpNgayVaoLam.Value.Date > DateTime.Today)
            return BaoLoi(dtpNgayVaoLam, "Ngày vào làm không được lớn hơn hôm nay.");
        if (_maNhanVien == CurrentUser.MaNhanVien && rdoTamNghi.Checked)
            return BaoLoi(rdoDangLamViec, "Bạn không thể tự chuyển chính mình sang trạng thái tạm nghỉ.");

        string soDienThoai = Regex.Replace(txtSoDienThoai.Text, @"\D", "");
        if (soDienThoai.Length > 0 && soDienThoai.Length is < 9 or > 11)
            return BaoLoi(txtSoDienThoai, "Số điện thoại phải có 9–11 số.");
        txtSoDienThoai.Text = soDienThoai;
        if (soDienThoai.Length > 0 && _service.TonTaiSoDienThoaiKhac(_maNhanVien, soDienThoai))
            return BaoLoi(txtSoDienThoai, "Số điện thoại đã được sử dụng.");

        string email = txtEmail.Text.Trim();
        if (email.Length > 0 && !EmailHopLe(email)) return BaoLoi(txtEmail, "Email không hợp lệ.");
        if (email.Length > 0 && _service.TonTaiEmailKhac(_maNhanVien, email))
            return BaoLoi(txtEmail, "Email đã được sử dụng.");

        string user = txtTenDangNhap.Text.Trim();
        if (user.Length < 4 || !Regex.IsMatch(user, @"^[A-Za-z0-9._-]+$"))
            return BaoLoi(txtTenDangNhap, "Tên đăng nhập phải có ít nhất 4 ký tự và chỉ gồm chữ, số, dấu chấm, gạch dưới hoặc gạch ngang.");
        if (_service.TonTaiTenDangNhapKhac(_maNhanVien, user))
            return BaoLoi(txtTenDangNhap, "Tên đăng nhập đã tồn tại.");
        if (cboChucVu.SelectedItem == null || cboVaiTro.SelectedValue == null)
            return BaoLoi(cboChucVu, "Vui lòng chọn chức vụ và vai trò.");
        return true;
    }

    private void Luu()
    {
        if (_banDau == null || !KiemTra()) return;
        try
        {
            Cursor = Cursors.WaitCursor;
            string? anh = _anhMoi == null
                ? Null(_banDau.AnhDaiDien)
                : string.IsNullOrEmpty(_anhMoi)
                    ? null
                    : DatabaseImageHelper.CopyToProjectImages(_anhMoi, "Avatars/Staff", $"nhanvien_{_maNhanVien:D5}");
            var input = new CapNhatNhanVienInputModel
            {
                MaNhanVien = _maNhanVien,
                HoTen = txtHoTen.Text,
                GioiTinh = rdoNam.Checked ? "Nam" : rdoNu.Checked ? "Nữ" : "Khác",
                NgaySinh = DateOnly.FromDateTime(dtpNgaySinh.Value),
                ChucVu = cboChucVu.SelectedItem?.ToString() ?? string.Empty,
                SoDienThoai = Null(txtSoDienThoai.Text),
                Email = Null(txtEmail.Text),
                DiaChi = Null(txtDiaChi.Text),
                AnhDaiDien = anh,
                NgayVaoLam = DateOnly.FromDateTime(dtpNgayVaoLam.Value),
                TrangThai = rdoDangLamViec.Checked,
                TenDangNhap = txtTenDangNhap.Text,
                MaVaiTro = Convert.ToInt32(cboVaiTro.SelectedValue)
            };
            if (!_service.CapNhatNhanVien(input))
                throw new InvalidOperationException("Không thể cập nhật nhân viên.");

            _banDau = _service.GetChiTiet(_maNhanVien);
            MessageBox.Show("Cập nhật nhân viên thành công.", "Thành công",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            if (_luuVaDong)
            {
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                GanDuLieu(_banDau);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Không thể cập nhật.\n" + Loi(ex), "Lỗi",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    private void PhanQuyen(object? sender, EventArgs e)
    {
        if (!YeuCauQuanTri()) return;
        using var form = new FrmPhanQuyenNhanVien(_maNhanVien);
        if (form.ShowDialog(this) == DialogResult.OK)
        {
            _banDau = _service.GetChiTiet(_maNhanVien);
            GanDuLieu(_banDau);
        }
    }

    private void DatLaiMatKhau(object? sender, EventArgs e)
    {
        if (!YeuCauQuanTri()) return;
        using var form = new FrmDatLaiMatKhauNhanVien(_maNhanVien);
        form.ShowDialog(this);
    }

    private void ChonAnh(object? sender, EventArgs e)
    {
        using OpenFileDialog dialog = new() { Filter = "Ảnh JPG hoặc PNG|*.jpg;*.jpeg;*.png" };
        if (dialog.ShowDialog(this) != DialogResult.OK) return;
        if (new FileInfo(dialog.FileName).Length > 2 * 1024 * 1024)
        {
            MessageBox.Show("Ảnh không được lớn hơn 2 MB.");
            return;
        }
        using Image source = Image.FromFile(dialog.FileName);
        Image? old = picAvatar.Image;
        picAvatar.Image = new Bitmap(source);
        old?.Dispose();
        _anhMoi = dialog.FileName;
        lblTenAnh.Text = Path.GetFileName(dialog.FileName);
    }

    private void XoaAnh()
    {
        _anhMoi = string.Empty;
        HienAnh(null, txtHoTen.Text);
        lblTenAnh.Text = "Ảnh mặc định";
    }

    private void HienAnh(string? path, string ten)
    {
        Image? old = picAvatar.Image;
        picAvatar.Image = DatabaseImageHelper.LoadStaffAvatar(path, ten, 104, 104);
        old?.Dispose();
        lblTenAnh.Text = DatabaseImageHelper.ResolvePath(path) is string resolved
            ? Path.GetFileName(resolved)
            : "JPG, PNG · Tối đa 2 MB";
    }

    private static bool EmailHopLe(string email)
    {
        try { return new MailAddress(email).Address.Equals(email, StringComparison.OrdinalIgnoreCase); }
        catch { return false; }
    }

    private static string? Null(string? value) => string.IsNullOrWhiteSpace(value) ? null : value.Trim();

    private static string Loi(Exception ex)
    {
        while (ex.InnerException != null) ex = ex.InnerException;
        return ex.Message;
    }

    private static bool BaoLoi(Control control, string message)
    {
        MessageBox.Show(message, "Thiếu hoặc sai thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        control.Focus();
        return false;
    }
}
