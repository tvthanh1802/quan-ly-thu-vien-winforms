using DataLayer.Models;
using Presentation.Helpers;

namespace Presentation.Models;

public sealed partial class FrmChiTietThongBao : Form
{
    private readonly ThongBaoDashboardModel _thongBao;

    public FrmChiTietThongBao(ThongBaoDashboardModel thongBao)
    {
        _thongBao = thongBao ?? throw new ArgumentNullException(nameof(thongBao));
        InitializeComponent();
        GanSuKien();
        HienThiDuLieu();
    }

    private void GanSuKien()
    {
        btnDong.Click += BtnDong_Click;
        KeyDown += FrmChiTietThongBao_KeyDown;
    }

    private void HienThiDuLieu()
    {
        Color accent = ThongBaoUiHelper.ParseColor(_thongBao.Mau);

        iconThongBao.IconChar = ThongBaoUiHelper.ParseIcon(_thongBao.Icon);
        lblTieuDe.Text = string.IsNullOrWhiteSpace(_thongBao.TieuDe)
            ? "Thông báo hệ thống"
            : _thongBao.TieuDe.Trim();
        lblLoai.Text = string.IsNullOrWhiteSpace(_thongBao.TenLoai)
            ? "Thông báo"
            : _thongBao.TenLoai.Trim();
        btnTrangThai.Text = _thongBao.DaDoc ? "●  Đã đọc" : "●  Chưa đọc";
        lblMaThongBao.Text = $"TB{_thongBao.MaThongBao:D6}";
        lblNgayGui.Text = _thongBao.NgayGui?.ToString("dd/MM/yyyy HH:mm")
            ?? "Không xác định";
        lblDoiTuong.Text = _thongBao.LaThongBaoChung
            ? "Toàn hệ thống"
            : "Tài khoản hiện tại";
        txtNoiDung.Text = string.IsNullOrWhiteSpace(_thongBao.NoiDung)
            ? "Không có nội dung."
            : _thongBao.NoiDung.Trim();

        ApDungTrangThaiHienThi(accent, _thongBao.DaDoc);
    }

    private void BtnDong_Click(object? sender, EventArgs e) => Close();

    private void FrmChiTietThongBao_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Escape) return;
        e.SuppressKeyPress = true;
        Close();
    }

    private void ApDungTrangThaiHienThi(Color accent, bool daDoc)
    {
        iconThongBao.IconColor = accent;
        lblLoai.ForeColor = accent;
        pnlIcon.FillColor = Color.FromArgb(
            (accent.R + 1020) / 5,
            (accent.G + 1020) / 5,
            (accent.B + 1020) / 5);
        btnTrangThai.FillColor = daDoc
            ? Color.FromArgb(238, 251, 244)
            : Color.FromArgb(255, 240, 241);
        btnTrangThai.ForeColor = daDoc
            ? Color.FromArgb(25, 145, 82)
            : Color.FromArgb(231, 65, 70);
        btnTrangThai.DisabledState.FillColor = btnTrangThai.FillColor;
        btnTrangThai.DisabledState.ForeColor = btnTrangThai.ForeColor;
    }
}
