using BusinessLayer.Services;
using DataLayer.Models;
using Presentation.Helpers;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace Presentation.Models;

public partial class FrmThemDocGia : Form
{
    private DocGiaService? _docGiaService;
    private bool _dangTaiDanhMuc;

    public FrmThemDocGia()
    {
        InitializeComponent();

        // Không khởi tạo DbContext hoặc truy vấn CSDL khi WinForms Designer mở form.
        if (DesignModeHelper.IsDesignMode(this))
            return;

        _docGiaService = new DocGiaService();
        CauHinhForm();
        GanSuKien();
    }

    private void CauHinhForm()
    {
        KeyPreview = true;
        StartPosition = FormStartPosition.CenterParent;
        AcceptButton = btnLuuDocGia;
        CancelButton = btnHuy;

        btnLuuDocGia.Text = "Lưu độc giả";
        rdoNam.Checked = true;

        dtpNgaySinh.MinDate = new DateTime(1900, 1, 1);
        dtpNgaySinh.MaxDate = DateTime.Today.AddDays(-1);
        dtpNgaySinh.Value = DateTime.Today.AddYears(-18);

        dtpNgayBatDau.Value = DateTime.Today;
        dtpNgayHetHan.Value = DateTime.Today.AddYears(1);

        cboLoaiDocGia.Items.Clear();
        cboLoaiDocGia.Items.AddRange(new object[]
        {
            "-- Chọn loại độc giả --",
            "Sinh viên",
            "Giảng viên",
            "Nhân viên",
            "Khác"
        });
        cboLoaiDocGia.SelectedIndex = 0;

        cboTrangThaiThe.Items.Clear();
        cboTrangThaiThe.Items.AddRange(new object[]
        {
            "Còn hiệu lực",
            "Bị khóa"
        });
        cboTrangThaiThe.SelectedIndex = 0;

        numHanMucMuon.Minimum = 1;
        numHanMucMuon.Maximum = 20;
        numHanMucMuon.Increment = 1;
        numHanMucMuon.TabStop = false;

        numSoNgayMuon.Minimum = 1;
        numSoNgayMuon.Maximum = 365;
        numSoNgayMuon.Increment = 1;
        numSoNgayMuon.TabStop = false;

        numLePhiNam.Minimum = 0;
        numLePhiNam.Maximum = 10_000_000;
        numLePhiNam.Increment = 10_000;

        txtHoTen.MaxLength = 50;
        txtSoDienThoai.MaxLength = 10;
        txtEmail.MaxLength = 100;
        txtDiaChiThuongTru.MaxLength = 255;
        txtDiaChiHienTai.MaxLength = 255;
        txtMaSo.MaxLength = 20;
        txtGhiChu.MaxLength = 255;
        txtGhiChuNoiBo.MaxLength = 255;

        lblDemGhiChu.Text = "0/255";
        lblDemGhiChuNoiBo.Text = "0/255";
    }


    private static void CauHinhDatePicker(Guna.UI2.WinForms.Guna2DateTimePicker picker)
    {
        picker.BackColor = Color.White;
        picker.FillColor = Color.White;
        picker.ForeColor = Color.FromArgb(30, 45, 80);
        picker.BorderColor = Color.FromArgb(205, 214, 230);
        picker.HoverState.FillColor = Color.White;
        picker.FocusedColor = Color.FromArgb(35, 85, 220);
        picker.CustomFormat = "dd/MM/yyyy";
        picker.Format = DateTimePickerFormat.Custom;
        picker.ShowUpDown = false;
    }

    private void GanSuKien()
    {
        Load += FrmThemDocGia_Load;
        cboLoaiDocGia.SelectedIndexChanged += cboLoaiDocGia_SelectedIndexChanged;
        cboKhoa.SelectedIndexChanged += cboKhoa_SelectedIndexChanged;
        dtpNgayBatDau.ValueChanged += dtpNgayBatDau_ValueChanged;
        txtSoDienThoai.KeyPress += txtSoDienThoai_KeyPress;
        txtGhiChu.TextChanged += txtGhiChu_TextChanged;
        txtGhiChuNoiBo.TextChanged += txtGhiChuNoiBo_TextChanged;
        btnLuuDocGia.Click += btnLuuDocGia_Click;
        btnHuy.Click += btnHuy_Click;
        KeyDown += FrmThemDocGia_KeyDown;
    }

    private void FrmThemDocGia_Load(object? sender, EventArgs e)
    {
        if (_docGiaService == null)
            return;

        try
        {
            Cursor = Cursors.WaitCursor;
            TaiDanhMucKhoaLop();
            TaiQuyDinhHienHanh();
            CapNhatTrangThaiControlTheoLoaiDocGia();
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                "Không thể tải dữ liệu cho form thêm độc giả.\n" + ex.Message,
                "Lỗi dữ liệu",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    private void TaiDanhMucKhoaLop()
    {
        if (_docGiaService == null)
            return;

        _dangTaiDanhMuc = true;
        try
        {
            List<LookupItemModel> dsKhoa = new()
            {
                new LookupItemModel { Id = 0, Ten = "-- Chọn khoa --" }
            };
            dsKhoa.AddRange(_docGiaService.GetDanhSachKhoaLookup());

            cboKhoa.DataSource = dsKhoa;
            cboKhoa.DisplayMember = nameof(LookupItemModel.Ten);
            cboKhoa.ValueMember = nameof(LookupItemModel.Id);
            cboKhoa.SelectedIndex = 0;

            NapLopTheoKhoa(null);
        }
        finally
        {
            _dangTaiDanhMuc = false;
        }
    }

    private void NapLopTheoKhoa(int? maKhoa)
    {
        if (_docGiaService == null)
            return;

        List<LookupItemModel> dsLop = new()
        {
            new LookupItemModel { Id = 0, Ten = "-- Chọn lớp --" }
        };

        if (maKhoa.HasValue && maKhoa.Value > 0)
            dsLop.AddRange(_docGiaService.GetDanhSachLopLookup(maKhoa));

        cboLop.DataSource = dsLop;
        cboLop.DisplayMember = nameof(LookupItemModel.Ten);
        cboLop.ValueMember = nameof(LookupItemModel.Id);
        cboLop.SelectedIndex = 0;
    }

    private void TaiQuyDinhHienHanh()
    {
        if (_docGiaService == null)
            return;

        QuyDinhDocGiaModel quyDinh = _docGiaService.GetQuyDinhDocGia();
        numHanMucMuon.Value = GioiHanGiaTri(
            quyDinh.SoSachMuonToiDa,
            numHanMucMuon.Minimum,
            numHanMucMuon.Maximum);
        numSoNgayMuon.Value = GioiHanGiaTri(
            quyDinh.SoNgayMuonToiDa,
            numSoNgayMuon.Minimum,
            numSoNgayMuon.Maximum);
        numLePhiNam.Value = GioiHanGiaTri(
            quyDinh.PhiThuongNien,
            numLePhiNam.Minimum,
            numLePhiNam.Maximum);
    }

    private static decimal GioiHanGiaTri(decimal value, decimal minimum, decimal maximum)
        => Math.Max(minimum, Math.Min(maximum, value));

    private void cboKhoa_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (_dangTaiDanhMuc || _docGiaService == null)
            return;

        int maKhoa = LaySelectedId(cboKhoa);

        _dangTaiDanhMuc = true;
        try
        {
            NapLopTheoKhoa(maKhoa > 0 ? maKhoa : null);
        }
        finally
        {
            _dangTaiDanhMuc = false;
        }
    }

    private void cboLoaiDocGia_SelectedIndexChanged(object? sender, EventArgs e)
        => CapNhatTrangThaiControlTheoLoaiDocGia();

    private void CapNhatTrangThaiControlTheoLoaiDocGia()
    {
        bool laSinhVien = string.Equals(
            cboLoaiDocGia.SelectedItem?.ToString(),
            "Sinh viên",
            StringComparison.CurrentCultureIgnoreCase);

        cboKhoa.Enabled = laSinhVien;
        cboLop.Enabled = laSinhVien;

        label13.Text = laSinhVien ? "Khoa *" : "Khoa";
        label14.Text = laSinhVien ? "Lớp *" : "Lớp";
        label22.Text = laSinhVien ? "Mã sinh viên *" : "Mã cán bộ / Mã số";
        txtMaSo.PlaceholderText = laSinhVien
            ? "Nhập mã sinh viên"
            : "Nhập mã cán bộ hoặc mã số (nếu có)";

        if (!laSinhVien)
        {
            _dangTaiDanhMuc = true;
            try
            {
                cboKhoa.SelectedIndex = 0;
                NapLopTheoKhoa(null);
            }
            finally
            {
                _dangTaiDanhMuc = false;
            }
        }
    }

    private void dtpNgayBatDau_ValueChanged(object? sender, EventArgs e)
    {
        DateTime ngayHetHanMacDinh = dtpNgayBatDau.Value.Date.AddYears(1);
        if (ngayHetHanMacDinh <= dtpNgayHetHan.MaxDate)
            dtpNgayHetHan.Value = ngayHetHanMacDinh;
    }

    private static int LaySelectedId(ComboBox comboBox)
    {
        if (comboBox.SelectedValue == null)
            return 0;

        return int.TryParse(comboBox.SelectedValue.ToString(), out int id) ? id : 0;
    }

    private void txtSoDienThoai_KeyPress(object? sender, KeyPressEventArgs e)
    {
        if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            e.Handled = true;
    }

    private void txtGhiChu_TextChanged(object? sender, EventArgs e)
        => lblDemGhiChu.Text = $"{txtGhiChu.Text.Length}/255";

    private void txtGhiChuNoiBo_TextChanged(object? sender, EventArgs e)
        => lblDemGhiChuNoiBo.Text = $"{txtGhiChuNoiBo.Text.Length}/255";

    private bool KiemTraDuLieu()
    {
        if (_docGiaService == null)
            return false;

        string hoTen = txtHoTen.Text.Trim();
        if (string.IsNullOrWhiteSpace(hoTen))
            return BaoLoi(txtHoTen, "Họ và tên không được để trống.");

        if (hoTen.Length > 50)
            return BaoLoi(txtHoTen, "Họ và tên độc giả không được vượt quá 50 ký tự.");

        if (dtpNgaySinh.Value.Date >= DateTime.Today)
            return BaoLoi(dtpNgaySinh, "Ngày sinh phải nhỏ hơn ngày hiện tại.");

        int tuoi = DateTime.Today.Year - dtpNgaySinh.Value.Year;
        if (dtpNgaySinh.Value.Date > DateTime.Today.AddYears(-tuoi)) tuoi--;
        if (tuoi < 6)
            return BaoLoi(dtpNgaySinh, "Độc giả phải từ 6 tuổi trở lên.");

        if (string.IsNullOrWhiteSpace(txtSoDienThoai.Text))
            return BaoLoi(txtSoDienThoai, "SĐT không được để trống.");

        if (Regex.IsMatch(txtSoDienThoai.Text.Trim(), @"[a-zA-Z]"))
            return BaoLoi(txtSoDienThoai, "SĐT chỉ được chứa số.");

        string soDienThoai = ChuanHoaSoDienThoai(txtSoDienThoai.Text);
        if (soDienThoai.Length != 10 || !soDienThoai.StartsWith("0"))
            return BaoLoi(txtSoDienThoai, "SĐT phải gồm đúng 10 chữ số và bắt đầu bằng số 0.");

        txtSoDienThoai.Text = soDienThoai;

        if (!string.IsNullOrWhiteSpace(txtEmail.Text) && !EmailHopLe(txtEmail.Text.Trim()))
            return BaoLoi(txtEmail, "Email không đúng định dạng.");

        if (string.IsNullOrWhiteSpace(txtDiaChiThuongTru.Text))
            return BaoLoi(txtDiaChiThuongTru, "Vui lòng nhập địa chỉ thường trú.");

        string loaiDocGia = cboLoaiDocGia.SelectedItem?.ToString() ?? string.Empty;
        if (loaiDocGia.StartsWith("--", StringComparison.Ordinal) || loaiDocGia.Length == 0)
            return BaoLoi(cboLoaiDocGia, "Vui lòng chọn loại độc giả.");

        bool laSinhVien = loaiDocGia == "Sinh viên";
        if (laSinhVien)
        {
            if (LaySelectedId(cboKhoa) <= 0)
                return BaoLoi(cboKhoa, "Vui lòng chọn khoa.");

            if (LaySelectedId(cboLop) <= 0)
                return BaoLoi(cboLop, "Vui lòng chọn lớp.");

            if (string.IsNullOrWhiteSpace(txtMaSo.Text))
                return BaoLoi(txtMaSo, "Vui lòng nhập mã sinh viên.");
        }

        if (dtpNgayHetHan.Value.Date <= dtpNgayBatDau.Value.Date)
            return BaoLoi(dtpNgayHetHan, "Ngày hết hạn phải sau ngày bắt đầu hiệu lực.");

        string? maSo = ChuanHoaChuoiRong(txtMaSo.Text);
        if (maSo != null && _docGiaService.TonTaiMaSinhVien(maSo))
            return BaoLoi(txtMaSo, "Mã sinh viên hoặc mã số này đã tồn tại.");

        if (_docGiaService.TonTaiSoDienThoai(soDienThoai))
            return BaoLoi(txtSoDienThoai, "Số điện thoại này đã tồn tại.");

        string? email = ChuanHoaChuoiRong(txtEmail.Text);
        if (email != null && _docGiaService.TonTaiEmail(email))
            return BaoLoi(txtEmail, "Email này đã tồn tại.");

        return true;
    }

    private static string ChuanHoaSoDienThoai(string value)
        => Regex.Replace(value ?? string.Empty, @"\D", string.Empty);

    private static bool EmailHopLe(string email)
    {
        if (string.IsNullOrWhiteSpace(email)) return false;
        return Regex.IsMatch(email, @"\A[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}\z");
    }

    private bool BaoLoi(Control control, string message)
    {
        MessageBox.Show(
            message,
            "Thiếu hoặc sai thông tin",
            MessageBoxButtons.OK,
            MessageBoxIcon.Warning);
        control.Focus();
        return false;
    }

    private ThemDocGiaInputModel TaoModel()
    {
        string gioiTinh = rdoNam.Checked
            ? "Nam"
            : rdoNu.Checked
                ? "Nữ"
                : "Khác";

        string loaiDocGia = cboLoaiDocGia.SelectedItem?.ToString() ?? "Khác";
        bool laSinhVien = loaiDocGia == "Sinh viên";

        string diaChi = GhepNoiCoGioiHan(
            255,
            $"Thường trú: {txtDiaChiThuongTru.Text.Trim()}",
            string.IsNullOrWhiteSpace(txtDiaChiHienTai.Text)
                ? null
                : $"Hiện tại: {txtDiaChiHienTai.Text.Trim()}");

        string? ghiChuThe = GhepNoiCoGioiHanNullable(
            255,
            string.IsNullOrWhiteSpace(txtGhiChu.Text)
                ? null
                : $"Ghi chú: {txtGhiChu.Text.Trim()}",
            string.IsNullOrWhiteSpace(txtGhiChuNoiBo.Text)
                ? null
                : $"Nội bộ: {txtGhiChuNoiBo.Text.Trim()}");

        return new ThemDocGiaInputModel
        {
            HoTen = txtHoTen.Text.Trim(),
            NgaySinh = DateOnly.FromDateTime(dtpNgaySinh.Value.Date),
            GioiTinh = gioiTinh,
            SoDienThoai = ChuanHoaSoDienThoai(txtSoDienThoai.Text),
            Email = ChuanHoaChuoiRong(txtEmail.Text),
            DiaChi = diaChi,
            LoaiDocGia = loaiDocGia,
            MaSinhVien = ChuanHoaChuoiRong(txtMaSo.Text),
            MaLop = laSinhVien ? LaySelectedId(cboLop) : null
        };
    }

    private static string? ChuanHoaChuoiRong(string? value)
        => string.IsNullOrWhiteSpace(value) ? null : value.Trim();

    private static string GhepNoiCoGioiHan(int maxLength, params string?[] parts)
    {
        string result = string.Join(" | ", parts.Where(p => !string.IsNullOrWhiteSpace(p)));
        return result.Length <= maxLength ? result : result[..maxLength];
    }

    private static string? GhepNoiCoGioiHanNullable(int maxLength, params string?[] parts)
    {
        string result = GhepNoiCoGioiHan(maxLength, parts);
        return result.Length == 0 ? null : result;
    }

    private void btnLuuDocGia_Click(object? sender, EventArgs e)
    {
        if (_docGiaService == null || !KiemTraDuLieu())
            return;

        DialogResult confirm = MessageBox.Show(
            "Bạn có chắc muốn lưu độc giả mới?",
            "Xác nhận",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (confirm != DialogResult.Yes)
            return;

        try
        {
            Cursor = Cursors.WaitCursor;
            btnLuuDocGia.Enabled = false;

            ThemDocGiaInputModel model = TaoModel();
            int maDocGia = _docGiaService.ThemDocGia(model, CurrentUser.MaNhanVien);

            MessageBox.Show(
                $"Thêm độc giả thành công.\nMã độc giả: DG{maDocGia:D4}",
                "Thành công",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                "Không thể thêm độc giả.\n" + LayThongBaoLoi(ex),
                "Lỗi",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
        finally
        {
            Cursor = Cursors.Default;
            btnLuuDocGia.Enabled = true;
        }
    }

    private static string LayThongBaoLoi(Exception ex)
    {
        Exception current = ex;
        while (current.InnerException != null)
            current = current.InnerException;
        return current.Message;
    }

    private void btnHuy_Click(object? sender, EventArgs e)
    {
        ResetFormInput();
    }

    private void ResetFormInput()
    {
        txtHoTen.Clear();
        dtpNgaySinh.Value = DateTime.Today.AddYears(-18);
        rdoNam.Checked = true;
        txtSoDienThoai.Clear();
        txtEmail.Clear();
        txtDiaChiThuongTru.Clear();
        txtDiaChiHienTai.Clear();
        txtGhiChu.Clear();
        txtMaSo.Clear();
        txtGhiChuNoiBo.Clear();
        if (cboLoaiDocGia.Items.Count > 0) cboLoaiDocGia.SelectedIndex = 0;
        if (cboKhoa.Items.Count > 0) cboKhoa.SelectedIndex = 0;
        if (cboLop.Items.Count > 0) cboLop.SelectedIndex = 0;
        dtpNgayBatDau.Value = DateTime.Today;
        dtpNgayHetHan.Value = DateTime.Today.AddYears(1);
    }

    private void FrmThemDocGia_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Escape)
        {
            btnHuy.PerformClick();
            e.Handled = true;
            return;
        }

        if (e.Control && e.KeyCode == Keys.S)
        {
            btnLuuDocGia.PerformClick();
            e.SuppressKeyPress = true;
            e.Handled = true;
        }
    }

    // Event cũ đang được Designer gắn; giữ lại để Designer không báo thiếu phương thức.
    private void rdoNam_CheckedChanged(object sender, EventArgs e)
    {
    }
}
