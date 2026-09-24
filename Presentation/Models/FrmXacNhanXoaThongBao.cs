using DataLayer.Models;
using Presentation.Helpers;

namespace Presentation.Models;

public sealed partial class FrmXacNhanXoaThongBao : Form
{
    private readonly ThongBaoDashboardModel _data;

    public FrmXacNhanXoaThongBao(ThongBaoDashboardModel data)
    {
        _data = data ?? throw new ArgumentNullException(nameof(data));
        InitializeComponent();
        GanDuLieu();
        GanSuKien();
        CapNhatNutXoa();
    }

    private void GanDuLieu()
    {
        lblMaThongBao.Text = $"TB-{(_data.NgayGui?.Year ?? DateTime.Today.Year)}-{_data.MaThongBao:0000}";
        lblTieuDe.Text = GiaTri(_data.TieuDe);
        lblLoai.Text = GiaTri(_data.TenLoai);
        lblDoiTuong.Text = LayDoiTuongNhan();
        lblNgayGui.Text = _data.NgayGui?.ToString("dd/MM/yyyy HH:mm") ?? "Không xác định";
        lblTrangThai.Text = _data.DaDoc ? "●  Đã đọc" : "●  Chưa đọc";

        Color accent = ThongBaoUiHelper.ParseColor(_data.Mau, Color.FromArgb(244, 116, 35));
        lblLoai.ForeColor = accent;
        lblTrangThai.ForeColor = _data.DaDoc
            ? Color.FromArgb(28, 154, 83)
            : Color.FromArgb(240, 55, 63);
    }

    private void GanSuKien()
    {
        KeyDown += (_, e) =>
        {
            if (e.KeyCode == Keys.Escape)
                Dong(DialogResult.Cancel);
        };
        btnHuy.Click += (_, _) => Dong(DialogResult.Cancel);
        chkXacNhan.CheckedChanged += (_, _) => CapNhatNutXoa();
        btnXoa.Click += (_, _) =>
        {
            if (chkXacNhan.Checked)
                Dong(DialogResult.OK);
        };
    }

    private void CapNhatNutXoa()
    {
        btnXoa.Enabled = chkXacNhan.Checked;
        btnXoa.FillColor = chkXacNhan.Checked
            ? Color.FromArgb(239, 55, 61)
            : Color.FromArgb(238, 168, 171);
    }

    private void Dong(DialogResult result)
    {
        DialogResult = result;
        Close();
    }

    private string LayDoiTuongNhan()
        => _data.LaThongBaoChung
            ? "Toàn hệ thống"
            : CurrentUser.MaNhanVien.HasValue
                ? "Nhân viên"
                : CurrentUser.MaDocGia.HasValue
                    ? "Độc giả"
                    : "Người dùng";

    private static string GiaTri(string? value)
        => string.IsNullOrWhiteSpace(value) ? "Không xác định" : value.Trim();
}
