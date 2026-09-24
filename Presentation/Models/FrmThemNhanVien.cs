using BusinessLayer.Services;
using DataLayer.Models;
using Presentation.Helpers;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace Presentation.Models;

public partial class FrmThemNhanVien : Form
{
    private readonly NhanVienService _service = new();
    private string? _duongDanAnh;
    private bool _dangNap;

    public int MaNhanVienVuaThem { get; private set; }

    public FrmThemNhanVien()
    {
        InitializeComponent();
    }

    private void FrmThemNhanVien_Load(object? sender, EventArgs e)
    {
        if (DesignModeHelper.IsDesignMode(this)) return;
        _dangNap = true;
        try
        {
            List<string> chucVu = _service.GetChucVu();
            foreach (string macDinh in new[] { "Thủ thư", "Nhân viên thư viện", "Quản lý" })
                if (!chucVu.Contains(macDinh, StringComparer.CurrentCultureIgnoreCase)) chucVu.Add(macDinh);
            cboChucVu.DataSource = chucVu.OrderBy(x => x).ToList();
            cboVaiTro.DataSource = _service.GetVaiTroLookup();
            cboVaiTro.DisplayMember = nameof(VaiTroLookupModel.TenVaiTro);
            cboVaiTro.ValueMember = nameof(VaiTroLookupModel.MaVaiTro);
            cboGioiTinh.SelectedIndex = 0;
            cboTrangThai.SelectedIndex = 0;
            dtpNgaySinh.Value = DateTime.Today.AddYears(-22);
            dtpNgayVaoLam.Value = DateTime.Today;
            NapNguoiDung();
            HienAnhMacDinh();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Không thể tải dữ liệu biểu mẫu.\n\n" + LoiGoc(ex), "Lỗi dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            _dangNap = false;
            CapNhatTomTat();
        }
    }

    private void NapNguoiDung()
    {
        string ten = string.IsNullOrWhiteSpace(CurrentUser.HoTen) ? CurrentUser.TenDangNhap : CurrentUser.HoTen;
        lblUser.Text = "Xin chào, " + (string.IsNullOrWhiteSpace(ten) ? "Admin" : ten);
        lblRole.Text = string.IsNullOrWhiteSpace(CurrentUser.VaiTro) ? "Quản trị viên" : CurrentUser.VaiTro;
        CurrentUserAvatarHelper.Apply(picAdmin);
    }

    private void InputChanged(object? sender, EventArgs e)
    {
        if (!_dangNap) CapNhatTomTat();
    }

    private void CapNhatTomTat()
    {
        lblSummaryStatus.Text = cboTrangThai.SelectedIndex == 1 ? "Tạm nghỉ" : "Sẵn sàng tạo mới";
        lblSummaryRole.Text = string.IsNullOrWhiteSpace(cboVaiTro.Text) ? "Chưa chọn" : cboVaiTro.Text;
        lblSummaryDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
    }

    private void BtnChonAnh_Click(object? sender, EventArgs e)
    {
        using OpenFileDialog dialog = new()
        {
            Filter = "Ảnh JPG hoặc PNG|*.jpg;*.jpeg;*.png",
            Title = "Chọn ảnh đại diện nhân viên"
        };
        if (dialog.ShowDialog(this) != DialogResult.OK) return;
        if (new FileInfo(dialog.FileName).Length > 2 * 1024 * 1024)
        {
            MessageBox.Show("Ảnh đại diện không được lớn hơn 2 MB.", "Ảnh không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        try
        {
            using Image source = Image.FromFile(dialog.FileName);
            Image? cu = picAvatar.Image;
            picAvatar.Image = new Bitmap(source);
            cu?.Dispose();
            _duongDanAnh = dialog.FileName;
        }
        catch (Exception ex)
        {
            MessageBox.Show("Không thể đọc ảnh.\n" + ex.Message, "Ảnh không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void HienAnhMacDinh()
    {
        Image? cu = picAvatar.Image;
        picAvatar.Image = DatabaseImageHelper.LoadStaffAvatar(null, txtHoTen.Text, 112, 112);
        cu?.Dispose();
        _duongDanAnh = null;
    }

    private void BtnHienMatKhau_Click(object? sender, EventArgs e)
    {
        txtMatKhau.UseSystemPasswordChar = !txtMatKhau.UseSystemPasswordChar;
        txtXacNhanMatKhau.UseSystemPasswordChar = txtMatKhau.UseSystemPasswordChar;
    }

    private bool KiemTraDuLieu()
    {
        if (string.IsNullOrWhiteSpace(txtHoTen.Text)) return BaoLoi(txtHoTen, "Vui lòng nhập họ và tên.");
        if (cboGioiTinh.SelectedIndex < 0) return BaoLoi(cboGioiTinh, "Vui lòng chọn giới tính.");
        if (dtpNgaySinh.Value.Date >= DateTime.Today) return BaoLoi(dtpNgaySinh, "Ngày sinh phải nhỏ hơn ngày hiện tại.");
        if (cboChucVu.SelectedItem == null) return BaoLoi(cboChucVu, "Vui lòng chọn chức vụ.");
        if (dtpNgayVaoLam.Value.Date > DateTime.Today) return BaoLoi(dtpNgayVaoLam, "Ngày vào làm không được lớn hơn hôm nay.");

        string soDienThoai = Regex.Replace(txtSoDienThoai.Text, @"\D", "");
        if (soDienThoai.Length > 0 && soDienThoai.Length is < 9 or > 11)
            return BaoLoi(txtSoDienThoai, "Số điện thoại phải có từ 9 đến 11 chữ số.");
        txtSoDienThoai.Text = soDienThoai;
        if (soDienThoai.Length > 0 && _service.TonTaiSoDienThoai(soDienThoai))
            return BaoLoi(txtSoDienThoai, "Số điện thoại đã được sử dụng.");

        string email = txtEmail.Text.Trim();
        if (email.Length > 0 && !EmailHopLe(email)) return BaoLoi(txtEmail, "Địa chỉ email không hợp lệ.");
        if (email.Length > 0 && _service.TonTaiEmail(email)) return BaoLoi(txtEmail, "Email đã được sử dụng.");

        string tenDangNhap = txtTenDangNhap.Text.Trim();
        if (tenDangNhap.Length < 4 || !Regex.IsMatch(tenDangNhap, @"^[A-Za-z0-9._-]+$"))
            return BaoLoi(txtTenDangNhap, "Tên đăng nhập phải có ít nhất 4 ký tự và chỉ gồm chữ, số, dấu chấm, gạch dưới hoặc gạch ngang.");
        if (_service.TonTaiTenDangNhap(tenDangNhap)) return BaoLoi(txtTenDangNhap, "Tên đăng nhập đã tồn tại.");
        try
        {
            NhanVienService.ValidatePassword(txtMatKhau.Text);
        }
        catch (ArgumentException ex)
        {
            return BaoLoi(txtMatKhau, ex.Message);
        }
        if (!string.Equals(txtMatKhau.Text, txtXacNhanMatKhau.Text, StringComparison.Ordinal))
            return BaoLoi(txtXacNhanMatKhau, "Mật khẩu xác nhận không khớp.");
        if (cboVaiTro.SelectedValue == null) return BaoLoi(cboVaiTro, "Vui lòng chọn vai trò.");
        return true;
    }

    private void BtnLuu_Click(object? sender, EventArgs e)
    {
        if (!KiemTraDuLieu()) return;
        try
        {
            UseWaitCursor = true;
            btnLuu.Enabled = false;
            string? anh = null;
            if (!string.IsNullOrWhiteSpace(_duongDanAnh))
                anh = DatabaseImageHelper.CopyToProjectImages(_duongDanAnh, "Avatars/Staff", "nhanvien_" + DateTime.Now.ToString("yyyyMMddHHmmss"));
            ThemNhanVienResultModel result = _service.ThemNhanVien(new ThemNhanVienInputModel
            {
                HoTen = txtHoTen.Text.Trim(),
                GioiTinh = cboGioiTinh.SelectedItem?.ToString() ?? string.Empty,
                NgaySinh = DateOnly.FromDateTime(dtpNgaySinh.Value),
                ChucVu = cboChucVu.SelectedItem?.ToString() ?? string.Empty,
                SoDienThoai = GiaTriRong(txtSoDienThoai.Text),
                Email = GiaTriRong(txtEmail.Text),
                DiaChi = GiaTriRong(txtDiaChi.Text),
                AnhDaiDien = anh,
                NgayVaoLam = DateOnly.FromDateTime(dtpNgayVaoLam.Value),
                TrangThai = cboTrangThai.SelectedIndex != 1,
                TenDangNhap = txtTenDangNhap.Text.Trim(),
                MatKhau = txtMatKhau.Text,
                MaVaiTro = Convert.ToInt32(cboVaiTro.SelectedValue)
            });
            MaNhanVienVuaThem = result.MaNhanVien;
            MessageBox.Show($"Đã thêm nhân viên {result.MaNhanVienText} thành công.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show(LoiGoc(ex), "Không thể thêm nhân viên", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            btnLuu.Enabled = true;
            UseWaitCursor = false;
        }
    }

    private void BtnLamMoi_Click(object? sender, EventArgs e)
    {
        _dangNap = true;
        txtHoTen.Clear(); txtSoDienThoai.Clear(); txtEmail.Clear(); txtDiaChi.Clear();
        txtTenDangNhap.Clear(); txtMatKhau.Clear(); txtXacNhanMatKhau.Clear(); txtMucLuong.Clear(); txtGhiChu.Clear();
        cboGioiTinh.SelectedIndex = 0; cboTrangThai.SelectedIndex = 0;
        if (cboChucVu.Items.Count > 0) cboChucVu.SelectedIndex = 0;
        if (cboVaiTro.Items.Count > 0) cboVaiTro.SelectedIndex = 0;
        dtpNgaySinh.Value = DateTime.Today.AddYears(-22); dtpNgayVaoLam.Value = DateTime.Today;
        HienAnhMacDinh();
        _dangNap = false;
        CapNhatTomTat();
        txtHoTen.Focus();
    }

    private void BtnLuuTam_Click(object? sender, EventArgs e) =>
        MessageBox.Show("Dữ liệu đang được giữ trên biểu mẫu. Bạn có thể tiếp tục chỉnh sửa trước khi lưu.", "Đã giữ dữ liệu tạm", MessageBoxButtons.OK, MessageBoxIcon.Information);

    private void BtnHuy_Click(object? sender, EventArgs e) => Close();

    private void FrmThemNhanVien_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Escape) Close();
        else if (e.Control && e.KeyCode == Keys.Enter)
        {
            BtnLuu_Click(btnLuu, EventArgs.Empty);
            e.SuppressKeyPress = true;
        }
    }

    private static bool EmailHopLe(string email)
    {
        try { return new MailAddress(email).Address.Equals(email, StringComparison.OrdinalIgnoreCase); }
        catch { return false; }
    }

    private static string? GiaTriRong(string? value) => string.IsNullOrWhiteSpace(value) ? null : value.Trim();

    private static string LoiGoc(Exception ex)
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
