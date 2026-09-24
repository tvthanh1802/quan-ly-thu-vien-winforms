using BusinessLayer.Models;
using BusinessLayer.Services;
using DataLayer.Models;
using Presentation.Helpers;
using System.Drawing.Printing;

namespace Presentation.Models;

public partial class FrmChiTietNhanVien : Form
{
    private readonly int _maNhanVien;
    private readonly NhanVienService _service = new();
    private readonly TaiKhoanService _taiKhoanService = new();
    private NhanVienDetailModel? _model;
    private bool _daCapNhat;

    public FrmChiTietNhanVien(int maNhanVien)
    {
        InitializeComponent(); _maNhanVien = maNhanVien;
        if (DesignModeHelper.IsDesignMode(this)) return;
        KeyPreview = true; CancelButton = btnDong;
        Load += (_, _) => TaiDuLieu(); btnDong.Click += (_, _) => { DialogResult = _daCapNhat ? DialogResult.OK : DialogResult.Cancel; Close(); };
        btnCapNhat.Click += BtnCapNhat_Click; btnIn.Click += BtnIn_Click;
        KeyDown += (_, e) => { if (e.KeyCode == Keys.Escape) btnDong.PerformClick(); };
    }

    private void TaiDuLieu()
    {
        try
        {
            _model = _service.GetChiTiet(_maNhanVien) ?? throw new InvalidOperationException("Không tìm thấy nhân viên cần xem.");
            NhanVienDetailModel x = _model;
            lblMa.Text = x.MaNhanVienText; lblHoTen.Text = x.HoTen; lblHoTenHoSo.Text = x.HoTen; lblTrangThai.Text = x.TrangThai;
            lblGioiTinh.Text = GiaTri(x.GioiTinh); lblNgaySinh.Text = x.NgaySinh?.ToString("dd/MM/yyyy") ?? "-"; lblDienThoai.Text = GiaTri(x.SoDienThoai); lblEmail.Text = GiaTri(x.Email); lblDiaChi.Text = GiaTri(x.DiaChi);
            lblChucVu.Text = lblChucVuHoSo.Text = GiaTri(x.ChucVu); lblNgayVaoLam.Text = lblNgayVaoLamHoSo.Text = x.NgayVaoLam.ToString("dd/MM/yyyy"); lblTrangThaiCongViec.Text = x.DangLamViec ? "Đang làm việc" : "Tạm nghỉ";
            lblTenDangNhap.Text = lblTaiKhoanHoSo.Text = GiaTri(x.TenDangNhap); lblVaiTro.Text = GiaTri(x.TenVaiTro); lblTrangThaiTaiKhoan.Text = !x.MaTaiKhoan.HasValue ? "Chưa có tài khoản" : x.TaiKhoanHoatDong ? "Hoạt động" : "Bị khóa";
            lblLanDangNhap.Text = x.LanDangNhapCuoi?.ToString("dd/MM/yyyy HH:mm") ?? "Chưa đăng nhập"; lblTongPhieu.Text = $"{x.TongPhieuXuLy:N0} phiếu"; lblPhieuThang.Text = $"{x.SoPhieuXuLyThangNay:N0} phiếu";
            DatBadge(lblTrangThai, x.TrangThai == "Đang làm việc"); DatBadge(lblTrangThaiCongViec, x.DangLamViec); DatBadge(lblTrangThaiTaiKhoan, x.TaiKhoanHoatDong && x.MaTaiKhoan.HasValue);
            Image? old = picAvatar.Image; picAvatar.Image = DatabaseImageHelper.LoadStaffAvatar(x.AnhDaiDien, x.HoTen, picAvatar.Width, picAvatar.Height); old?.Dispose();
            HienThiQuyen(x.MaVaiTro);
        }
        catch (Exception ex) { MessageBox.Show("Không thể tải hồ sơ nhân viên.\n" + ex.Message, "Lỗi dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error); Close(); }
    }

    private void HienThiQuyen(int? maVaiTro)
    {
        flowQuyen.Controls.Clear(); if (!maVaiTro.HasValue) return;
        var groups = _taiKhoanService.LayPhanQuyenTheoVaiTro(maVaiTro.Value).Where(x => !x.LaDongNhom && x.DuocXem).Select(x => x.MaNhom).Distinct().ToList();
        foreach (string group in groups) flowQuyen.Controls.Add(new Label { AutoSize = true, Text = "▣  " + TenNhom(group), ForeColor = Color.FromArgb(98, 55, 190), BackColor = Color.FromArgb(246, 240, 255), Padding = new Padding(10, 7, 10, 7), Margin = new Padding(0, 0, 8, 6), Font = new Font("Segoe UI", 8.5F, FontStyle.Bold) });
        lblSoQuyen.Text = $"{groups.Count} nhóm quyền";
    }

    private void BtnCapNhat_Click(object? sender, EventArgs e)
    {
        using var form = new FrmSuaNhanVien(_maNhanVien); if (form.ShowDialog(this) != DialogResult.OK) return; _daCapNhat = true; TaiDuLieu();
    }

    private void BtnIn_Click(object? sender, EventArgs e)
    {
        if (_model == null) return;
        using PrintDocument document = new(); document.DocumentName = $"HoSo_{_model.MaNhanVienText}"; document.PrintPage += InHoSo;
        using PrintPreviewDialog preview = new() { Document = document, Width = 1000, Height = 720 }; preview.ShowDialog(this);
    }

    private void InHoSo(object? sender, PrintPageEventArgs e)
    {
        if (_model == null || e.Graphics == null) return; Graphics g = e.Graphics; float y = e.MarginBounds.Top;
        using var title = new Font("Segoe UI", 18, FontStyle.Bold); using var head = new Font("Segoe UI", 11, FontStyle.Bold); using var normal = new Font("Segoe UI", 10);
        g.DrawString("HỒ SƠ NHÂN VIÊN", title, Brushes.Navy, e.MarginBounds.Left, y); y += 48; g.DrawString($"{_model.MaNhanVienText} - {_model.HoTen}", head, Brushes.Black, e.MarginBounds.Left, y); y += 36;
        string[] lines = { $"Giới tính: {GiaTri(_model.GioiTinh)}", $"Ngày sinh: {_model.NgaySinh?.ToString("dd/MM/yyyy") ?? "-"}", $"Điện thoại: {GiaTri(_model.SoDienThoai)}", $"Email: {GiaTri(_model.Email)}", $"Địa chỉ: {GiaTri(_model.DiaChi)}", $"Chức vụ: {GiaTri(_model.ChucVu)}", $"Ngày vào làm: {_model.NgayVaoLam:dd/MM/yyyy}", $"Trạng thái: {_model.TrangThai}", $"Tài khoản: {GiaTri(_model.TenDangNhap)}", $"Vai trò: {GiaTri(_model.TenVaiTro)}", $"Tổng phiếu xử lý: {_model.TongPhieuXuLy:N0}" };
        foreach (string line in lines) { g.DrawString(line, normal, Brushes.Black, e.MarginBounds.Left, y); y += 28; }
    }

    private static void DatBadge(Label label, bool active) { label.BackColor = active ? Color.FromArgb(229, 248, 234) : Color.FromArgb(255, 235, 237); label.ForeColor = active ? Color.FromArgb(25, 145, 70) : Color.FromArgb(210, 70, 75); }
    private static string TenNhom(string code) => code switch { "SACH" => "Quản lý sách", "DOCGIA" => "Quản lý độc giả", "MUONTRA" => "Mượn – Trả – Phạt", "NHAPSACH" => "Nhập sách", "BAOCAO" => "Thống kê báo cáo", "HETHONG" => "Tài khoản & hệ thống", _ => code };
    private static string GiaTri(string? value) => string.IsNullOrWhiteSpace(value) ? "-" : value;
}
