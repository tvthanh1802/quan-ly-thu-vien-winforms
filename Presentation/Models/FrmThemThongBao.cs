using BusinessLayer.Services;
using DataLayer.Models;
using Presentation.Helpers;

namespace Presentation.Models;

public sealed partial class FrmThemThongBao : Form
{
    private readonly ThongKeService _service = new();
    private ThongKeNguoiNhanModel _thongKe = new();
    private bool _dangNap;

    public FrmThemThongBao() => InitializeComponent();

    private void FrmThemThongBao_Load(object? sender, EventArgs e)
    {
        if (DesignModeHelper.IsDesignMode(this)) return;
        _dangNap = true;
        try
        {
            cboLoai.DataSource = _service.GetNotificationTypes();
            cboLoai.DisplayMember = nameof(LoaiThongBaoLookupModel.TenLoai);
            cboLoai.ValueMember = nameof(LoaiThongBaoLookupModel.MaLoaiThongBao);
            _thongKe = _service.GetNotificationRecipientStatistics();
            KhoiTaoNguoiDung();
            dtpNgayGui.Value = DateTime.Now.AddMinutes(5);
            chkToanHeThong.Checked = true;
            CapNhatThongKeNguoiNhan();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Không thể tải dữ liệu tạo thông báo.\n\n" + RootMessage(ex), "Lỗi dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally { _dangNap = false; CapNhatXemTruoc(); }
    }

    private void KhoiTaoNguoiDung()
    {
        string ten = string.IsNullOrWhiteSpace(CurrentUser.HoTen) ? CurrentUser.TenDangNhap : CurrentUser.HoTen;
        ten = string.IsNullOrWhiteSpace(ten) ? "Admin" : ten;
        lblUser.Text = "Xin chào, " + ten;
        lblRole.Text = string.IsNullOrWhiteSpace(CurrentUser.VaiTro) ? "Quản trị viên" : CurrentUser.VaiTro;
        lblNguoiGuiValue.Text = ten;
        lblPreviewNguoiGui.Text = "Người gửi: " + ten;
        picAvatar.Image = CurrentUser.LaTaiKhoanDocGia
            ? DatabaseImageHelper.LoadReaderAvatar(CurrentUser.AnhDaiDien, ten, 48, 48)
            : DatabaseImageHelper.LoadStaffAvatar(CurrentUser.AnhDaiDien, ten, 48, 48);
    }

    private void DoiTuong_CheckedChanged(object? sender, EventArgs e)
    {
        if (_dangNap) return;
        _dangNap = true;
        try
        {
            if (sender == chkToanHeThong && chkToanHeThong.Checked)
            {
                chkNhanVien.Checked = false;
                chkDocGia.Checked = false;
            }
            else if (sender != chkToanHeThong && (chkNhanVien.Checked || chkDocGia.Checked))
                chkToanHeThong.Checked = false;
        }
        finally { _dangNap = false; }
        CapNhatThongKeNguoiNhan();
        CapNhatXemTruoc();
    }

    private void InputPreviewChanged(object? sender, EventArgs e)
    {
        if (!_dangNap) CapNhatXemTruoc();
    }

    private void CapNhatThongKeNguoiNhan()
    {
        int nhanVien = chkToanHeThong.Checked || chkNhanVien.Checked ? _thongKe.SoNhanVien : 0;
        int docGia = chkToanHeThong.Checked || chkDocGia.Checked ? _thongKe.SoDocGia : 0;
        lblNhanVienCount.Text = nhanVien.ToString("N0");
        lblDocGiaCount.Text = docGia.ToString("N0");
        lblTongCount.Text = (nhanVien + docGia).ToString("N0");
        lblSummaryRecipients.Text = (nhanVien + docGia).ToString("N0") + " người nhận";
    }

    private void CapNhatXemTruoc()
    {
        lblCountTitle.Text = $"{txtTieuDe.TextLength}/200";
        lblCountContent.Text = $"{txtNoiDung.TextLength}/2000";
        lblPreviewTieuDe.Text = string.IsNullOrWhiteSpace(txtTieuDe.Text) ? "Tiêu đề thông báo sẽ hiển thị tại đây" : txtTieuDe.Text.Trim();
        lblPreviewNoiDung.Text = string.IsNullOrWhiteSpace(txtNoiDung.Text) ? "Nội dung thông báo sẽ được xem trước tại khu vực này." : txtNoiDung.Text.Trim();
        lblPreviewLoai.Text = cboLoai.SelectedItem is LoaiThongBaoLookupModel loai ? loai.TenLoai : "Thông báo";
        lblDoiTuongValue.Text = LayTenDoiTuong();
        lblPreviewDoiTuong.Text = "Đối tượng: " + LayTenDoiTuong();
        lblLichGuiValue.Text = dtpNgayGui.Value.ToString("dd/MM/yyyy  HH:mm");
        lblPreviewNgayGui.Text = "Gửi lúc: " + dtpNgayGui.Value.ToString("dd/MM/yyyy HH:mm");
        bool sanSang = cboLoai.SelectedValue != null && !string.IsNullOrWhiteSpace(txtTieuDe.Text)
            && !string.IsNullOrWhiteSpace(txtNoiDung.Text)
            && (chkToanHeThong.Checked || chkNhanVien.Checked || chkDocGia.Checked);
        lblReady.Text = sanSang ? "●  Sẵn sàng gửi" : "●  Chưa đủ thông tin";
    }

    private string LayTenDoiTuong()
    {
        if (chkToanHeThong.Checked) return "Toàn hệ thống";
        if (chkNhanVien.Checked && chkDocGia.Checked) return "Nhân viên & Độc giả";
        if (chkNhanVien.Checked) return "Nhân viên";
        if (chkDocGia.Checked) return "Độc giả";
        return "Chưa chọn";
    }

    private void BtnGui_Click(object? sender, EventArgs e)
    {
        if (cboLoai.SelectedValue is null) { BaoLoi("Vui lòng chọn loại thông báo.", cboLoai); return; }
        if (string.IsNullOrWhiteSpace(txtTieuDe.Text)) { BaoLoi("Vui lòng nhập tiêu đề thông báo.", txtTieuDe); return; }
        if (string.IsNullOrWhiteSpace(txtNoiDung.Text)) { BaoLoi("Vui lòng nhập nội dung thông báo.", txtNoiDung); return; }
        if (!chkToanHeThong.Checked && !chkNhanVien.Checked && !chkDocGia.Checked)
        {
            MessageBox.Show("Vui lòng chọn ít nhất một đối tượng nhận.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        if (dtpNgayGui.Value < DateTime.Now.AddMinutes(-1)) { BaoLoi("Thời gian gửi không được ở trong quá khứ.", dtpNgayGui); return; }

        var input = new ThemThongBaoInputModel
        {
            MaLoaiThongBao = Convert.ToInt32(cboLoai.SelectedValue),
            TieuDe = txtTieuDe.Text.Trim(), NoiDung = txtNoiDung.Text.Trim(), NgayGui = dtpNgayGui.Value,
            GuiToanHeThong = chkToanHeThong.Checked, GuiNhanVien = chkNhanVien.Checked, GuiDocGia = chkDocGia.Checked
        };
        try
        {
            UseWaitCursor = true; btnGui.Enabled = false;
            ThemThongBaoResultModel result = _service.CreateNotification(input);
            MessageBox.Show($"Đã tạo {result.SoBanGhiDaTao:N0} thông báo cho {result.SoNhanVienNhan:N0} nhân viên và {result.SoDocGiaNhan:N0} độc giả.",
                "Tạo thông báo thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            DialogResult = DialogResult.OK; Close();
        }
        catch (Exception ex) { MessageBox.Show(RootMessage(ex), "Không thể tạo thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        finally { btnGui.Enabled = true; UseWaitCursor = false; }
    }

    private static void BaoLoi(string message, Control control)
    {
        MessageBox.Show(message, "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        control.Focus();
    }

    private void BtnLamMoi_Click(object? sender, EventArgs e)
    {
        _dangNap = true;
        txtTieuDe.Clear(); txtNoiDung.Clear();
        if (cboLoai.Items.Count > 0) cboLoai.SelectedIndex = 0;
        dtpNgayGui.Value = DateTime.Now.AddMinutes(5);
        chkToanHeThong.Checked = true; chkNhanVien.Checked = false; chkDocGia.Checked = false;
        _dangNap = false;
        CapNhatThongKeNguoiNhan(); CapNhatXemTruoc(); txtTieuDe.Focus();
    }

    private void BtnLuuNhap_Click(object? sender, EventArgs e) =>
        MessageBox.Show("Nội dung nháp đang được giữ trên biểu mẫu. Bạn có thể tiếp tục chỉnh sửa trước khi gửi.",
            "Đã giữ bản nháp", MessageBoxButtons.OK, MessageBoxIcon.Information);

    private void BtnHuy_Click(object? sender, EventArgs e) => Close();

    private void FrmThemThongBao_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Escape) Close();
        else if (e.Control && e.KeyCode == Keys.Enter)
        {
            BtnGui_Click(btnGui, EventArgs.Empty); e.SuppressKeyPress = true;
        }
    }

    private static string RootMessage(Exception ex)
    {
        while (ex.InnerException != null) ex = ex.InnerException;
        return ex.Message;
    }
}
