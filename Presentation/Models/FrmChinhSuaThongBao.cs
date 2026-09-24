using BusinessLayer.Services;
using DataLayer.Models;

namespace Presentation.Models;

public sealed partial class FrmChinhSuaThongBao : Form
{
    private readonly int _maThongBao;
    private readonly ThongKeService _service = new();
    private ThongBaoEditModel? _model;
    private bool _dangNap;

    public FrmChinhSuaThongBao(int maThongBao)
    {
        _maThongBao = maThongBao;
        InitializeComponent();
        Load += FrmChinhSuaThongBao_Load;
        KeyDown += FrmChinhSuaThongBao_KeyDown;
    }

    private void FrmChinhSuaThongBao_Load(object? sender, EventArgs e)
    {
        try
        {
            _dangNap = true;
            cboLoai.DataSource = _service.GetNotificationTypes();
            cboLoai.DisplayMember = nameof(LoaiThongBaoLookupModel.TenLoai);
            cboLoai.ValueMember = nameof(LoaiThongBaoLookupModel.MaLoaiThongBao);
            _model = _service.GetNotificationForEdit(_maThongBao);
            if (_model is null)
            {
                MessageBox.Show("Không tìm thấy thông báo cần chỉnh sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Dong(DialogResult.Cancel);
                return;
            }

            txtMa.Text = $"TB{_model.MaThongBao:D6}";
            txtTieuDe.Text = _model.TieuDe;
            txtNoiDung.Text = _model.NoiDung;
            txtNguoiNhan.Text = _model.DoiTuongNhan;
            cboLoai.SelectedValue = _model.MaLoaiThongBao;
            cboTrangThai.SelectedIndex = _model.DaDoc ? 1 : 0;
            dtpNgayGui.Value = _model.NgayGui;
        }
        catch (Exception ex)
        {
            MessageBox.Show("Không thể tải thông báo.\n\n" + RootMessage(ex), "Lỗi dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Dong(DialogResult.Cancel);
        }
        finally
        {
            _dangNap = false;
            CapNhatXemTruoc();
        }
    }

    private void InputPreviewChanged(object? sender, EventArgs e)
    {
        if (!_dangNap) CapNhatXemTruoc();
    }

    private void CapNhatXemTruoc()
    {
        lblCountTitle.Text = $"{txtTieuDe.TextLength}/200";
        lblPreviewTieuDe.Text = string.IsNullOrWhiteSpace(txtTieuDe.Text) ? "Tiêu đề thông báo" : txtTieuDe.Text.Trim();
        lblPreviewNoiDung.Text = string.IsNullOrWhiteSpace(txtNoiDung.Text) ? "Nội dung thông báo sẽ hiển thị tại đây." : txtNoiDung.Text.Trim();
        lblPreviewLoai.Text = cboLoai.SelectedItem is LoaiThongBaoLookupModel loai ? loai.TenLoai : "Thông tin";
        lblPreviewTrangThai.Text = cboTrangThai.SelectedItem?.ToString() ?? "Chưa đọc";
        lblPreviewNgay.Text = "Ngày gửi: " + dtpNgayGui.Value.ToString("dd/MM/yyyy HH:mm");
        lblPreviewNguoiNhan.Text = "Đối tượng: " + txtNguoiNhan.Text;
    }

    private void BtnLuu_Click(object? sender, EventArgs e)
    {
        if (_model is null || cboLoai.SelectedValue is null) return;
        if (string.IsNullOrWhiteSpace(txtTieuDe.Text) || string.IsNullOrWhiteSpace(txtNoiDung.Text))
        {
            MessageBox.Show("Vui lòng nhập đầy đủ tiêu đề và nội dung.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        _model.MaLoaiThongBao = Convert.ToInt32(cboLoai.SelectedValue);
        _model.TieuDe = txtTieuDe.Text.Trim();
        _model.NoiDung = txtNoiDung.Text.Trim();
        _model.NgayGui = dtpNgayGui.Value;
        _model.DaDoc = cboTrangThai.SelectedIndex == 1;
        try
        {
            if (!_service.UpdateNotification(_model))
            {
                MessageBox.Show("Không tìm thấy thông báo cần cập nhật.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            Dong(DialogResult.OK);
        }
        catch (Exception ex)
        {
            MessageBox.Show(RootMessage(ex), "Không thể cập nhật thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnHuy_Click(object? sender, EventArgs e) => Dong(DialogResult.Cancel);

    private void FrmChinhSuaThongBao_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Escape) Dong(DialogResult.Cancel);
        else if (e.Control && e.KeyCode == Keys.S)
        {
            BtnLuu_Click(btnLuu, EventArgs.Empty);
            e.SuppressKeyPress = true;
        }
    }

    private void Dong(DialogResult result)
    {
        DialogResult = result;
        Close();
    }

    private static string RootMessage(Exception ex)
    {
        while (ex.InnerException != null) ex = ex.InnerException;
        return ex.Message;
    }
}
