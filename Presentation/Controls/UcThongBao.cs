using BusinessLayer.Services;
using DataLayer.Models;
using Presentation.Helpers;
using Presentation.Models;

namespace Presentation.Controls;

public partial class UcThongBao : UserControl
{
    private ThongKeService? _service;
    private ThongKeService Service => _service ??= new ThongKeService();
    private readonly BindingSource _binding = new();
    private List<ThongBaoDashboardModel> _nguon = [];
    private List<ThongBaoDashboardModel> _loc = [];
    private List<LoaiThongBaoLookupModel> _loai = [];
    private ThongBaoDashboardModel? _dangChon;
    private ThongKeNguoiNhanModel _thongKeNguoiNhan = new();
    private bool _dangKhoiTao;
    private int _trang = 1;
    private const int SoDongMoiTrang = 6;
    private bool CoQuyenXemTatCa => PermissionHelper.CanView("HETHONG.THONGBAO");

    public UcThongBao()
    {
        InitializeComponent();
        dgvThongBao.AutoGenerateColumns = false;
        dgvThongBao.CellFormatting += DgvThongBao_CellFormatting;
    }

    private async void UcThongBao_Load(object? sender, EventArgs e)
    {
        if (DesignModeHelper.IsDesignMode(this)) return;

        KhoiTaoNguoiDung();
        _dangKhoiTao = true;
        try
        {
            int? maNhanVien = CurrentUser.MaNhanVien;
            int? maDocGia = CurrentUser.MaDocGia;
            CauHinhQuyenQuanTri();
            Task<List<LoaiThongBaoLookupModel>> loaiTask = Task.Run(() =>
                new ThongKeService().GetNotificationTypes());
            Task<List<ThongBaoDashboardModel>> thongBaoTask = Task.Run(() =>
                new ThongKeService().GetAllNotifications(maNhanVien, maDocGia, CoQuyenXemTatCa)
                    .OrderByDescending(x => x.NgayGui)
                    .ThenByDescending(x => x.MaThongBao)
                    .ToList());
            Task<ThongKeNguoiNhanModel> thongKeTask = Task.Run(() => CoQuyenXemTatCa
                ? new ThongKeService().GetNotificationRecipientStatistics()
                : new ThongKeNguoiNhanModel());

            await Task.WhenAll(loaiTask, thongBaoTask, thongKeTask);
            _loai = await loaiTask;
            _nguon = await thongBaoTask;
            _thongKeNguoiNhan = await thongKeTask;
            KhoiTaoBoLoc();
            CapNhatKpi();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Không thể tải dữ biệu thông báo.\n\n" + ex.Message, "Lỗi dữ biệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            _dangKhoiTao = false;
        }

        LocDuLieu(true);
    }

    private void KhoiTaoBoLoc()
    {
        cboLoai.Items.Clear();
        cboLoai.Items.Add("Tất cả");
        foreach (var item in _loai) cboLoai.Items.Add(item.TenLoai);
        cboDoiTuong.SelectedIndex = cboTrangThai.SelectedIndex = cboNgayGui.SelectedIndex = 0;
        cboLoai.SelectedIndex = 0;
    }

    private void KhoiTaoNguoiDung()
    {
        string ten = string.IsNullOrWhiteSpace(CurrentUser.HoTen) ? CurrentUser.TenDangNhap : CurrentUser.HoTen;
        lblUser.Text = string.IsNullOrWhiteSpace(ten) ? "Xin chào, Admin" : "Xin chào, " + ten;
        lblRole.Text = string.IsNullOrWhiteSpace(CurrentUser.VaiTro) ? "Quản trị viên" : CurrentUser.VaiTro;
        picAvatar.Image = CurrentUser.LaTaiKhoanDocGia
            ? DatabaseImageHelper.LoadReaderAvatar(CurrentUser.AnhDaiDien, ten, 48, 48)
            : DatabaseImageHelper.LoadStaffAvatar(CurrentUser.AnhDaiDien, ten, 48, 48);
    }


    private void CauHinhQuyenQuanTri()
    {
        btnTaoThongBao.Visible = PermissionHelper.CanAdd("HETHONG.THONGBAO");
        btnGuiLai.Visible = PermissionHelper.CanAdd("HETHONG.THONGBAO");
        colSua.Visible = PermissionHelper.CanEdit("HETHONG.THONGBAO");
        colXoa.Visible = PermissionHelper.CanDelete("HETHONG.THONGBAO");
    }

    private static bool KiemTraQuyen(bool allowed, string message)
    {
        if (allowed) return true;
        MessageBox.Show(message, "Không đủ quyền", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return false;
    }

    private void TaiDuLieu()
    {
        try
        {
            UseWaitCursor = true;
            _nguon = Service.GetAllNotifications(
                    CurrentUser.MaNhanVien, CurrentUser.MaDocGia, CoQuyenXemTatCa)
                .OrderByDescending(x => x.NgayGui).ThenByDescending(x => x.MaThongBao).ToList();
            CapNhatKpi();
            LocDuLieu(true);
        }
        catch (Exception ex)
        {
            MessageBox.Show("Không thể tải dữ biệu thông báo.\n\n" + ex.Message, "Lỗi dữ biệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally { UseWaitCursor = false; }
    }

    private void CapNhatKpi()
    {
        lblTongThongBao.Text = _nguon.Count.ToString("N0");
        int chuaDoc = _nguon.Count(x => !x.DaDoc);
        lblChuaDoc.Text = chuaDoc.ToString("N0");
        lblHomNay.Text = _nguon.Count(x => x.NgayGui?.Date == DateTime.Today).ToString("N0");
        lblQuanTrong.Text = _nguon.Count(LaQuanTrong).ToString("N0");
        lblNotificationCount.Text = Math.Min(99, chuaDoc).ToString();
        lblNotificationCount.Visible = chuaDoc > 0;
    }

    private static bool LaQuanTrong(ThongBaoDashboardModel x) =>
        x.TenLoai.Contains("hạn", StringComparison.CurrentCultureIgnoreCase)
        || x.TenLoai.Contains("cảnh báo", StringComparison.CurrentCultureIgnoreCase)
        || x.TieuDe.Contains("khẩn", StringComparison.CurrentCultureIgnoreCase);

    private void LocDuLieu(bool veTrangDau)
    {
        if (cboLoai.SelectedIndex < 0) return;
        IEnumerable<ThongBaoDashboardModel> query = _nguon;
        string keyword = txtTimKiem.Text.Trim();
        if (keyword.Length > 0) query = query.Where(x => x.TieuDe.Contains(keyword, StringComparison.CurrentCultureIgnoreCase) || x.NoiDung.Contains(keyword, StringComparison.CurrentCultureIgnoreCase));
        if (cboLoai.SelectedIndex > 0)
        {
            string tenLoai = cboLoai.SelectedItem?.ToString() ?? "";
            query = query.Where(x => x.TenLoai.Equals(tenLoai, StringComparison.CurrentCultureIgnoreCase));
        }
        query = cboDoiTuong.SelectedIndex switch
        {
            1 => query.Where(x => x.MaNhanVien.HasValue),
            2 => query.Where(x => x.MaDocGia.HasValue),
            3 => query.Where(x => x.LaThongBaoChung),
            _ => query
        };
        query = cboTrangThai.SelectedIndex switch { 1 => query.Where(x => !x.DaDoc), 2 => query.Where(x => x.DaDoc), _ => query };
        DateTime? tuNgay = cboNgayGui.SelectedIndex switch { 1 => DateTime.Today, 2 => DateTime.Today.AddDays(-6), 3 => DateTime.Today.AddDays(-29), _ => null };
        if (tuNgay.HasValue) query = query.Where(x => x.NgayGui >= tuNgay.Value);
        _loc = query.ToList();
        if (veTrangDau) _trang = 1;
        HienThiTrang();
    }

    private void HienThiTrang()
    {
        int tongTrang = Math.Max(1, (int)Math.Ceiling(_loc.Count / (double)SoDongMoiTrang));
        _trang = Math.Clamp(_trang, 1, tongTrang);
        int skip = (_trang - 1) * SoDongMoiTrang;
        var rows = _loc.Skip(skip).Take(SoDongMoiTrang).Select((x, i) => new ThongBaoGridRow
        {
            MaThongBao = x.MaThongBao,
            STT = skip + i + 1,
            ThongBao = x.TieuDe + "\n" + x.NoiDung,
            ThoiGian = (x.NgayGui?.ToString("dd/MM/yyyy\nHH:mm") ?? "—"),
            DoiTuong = TenNhomDoiTuong(x),
            TrangThai = x.DaDoc ? "● Đã đọc" : "● Chưa đọc"
        }).ToList();
        _binding.DataSource = rows; dgvThongBao.DataSource = _binding;
        int dau = _loc.Count == 0 ? 0 : skip + 1, cuoi = Math.Min(skip + SoDongMoiTrang, _loc.Count);
        lblPageInfo.Text = $"Hiển thị {dau:N0}–{cuoi:N0} của {_loc.Count:N0} thông báo";
        btnTrangTruoc.Enabled = _trang > 1; btnTrangSau.Enabled = _trang < tongTrang;
        CapNhatNutTrang(tongTrang);
        if (rows.Count == 0) XoaChiTiet(); else { dgvThongBao.ClearSelection(); dgvThongBao.Rows[0].Selected = true; HienThiChiTiet(rows[0].MaThongBao); }
    }

    private void CapNhatNutTrang(int tongTrang)
    {
        Guna.UI2.WinForms.Guna2Button[] nutTrang = [btnTrang1, btnTrang2, btnTrang3, btnTrang4, btnTrang5];
        int trangBatDau = tongTrang <= 5 ? 1 : Math.Clamp(_trang - 2, 1, tongTrang - 4);

        for (int i = 0; i < nutTrang.Length; i++)
        {
            int soTrang = trangBatDau + i;
            nutTrang[i].Text = soTrang.ToString();
            nutTrang[i].Tag = soTrang;
            nutTrang[i].Visible = soTrang <= tongTrang;
            nutTrang[i].Checked = soTrang == _trang;
        }

        bool hienTrangCuoi = tongTrang > trangBatDau + 4;
        lblChamTrang.Visible = hienTrangCuoi;
        btnTrangCuoi.Visible = hienTrangCuoi;
        btnTrangCuoi.Text = tongTrang.ToString();
        btnTrangCuoi.Tag = tongTrang;
        btnTrangCuoi.Checked = _trang == tongTrang;
    }

    private void NutTrang_Click(object? sender, EventArgs e)
    {
        if (sender is Guna.UI2.WinForms.Guna2Button { Tag: int trang }) ChuyenTrang(trang);
    }

    private void BoLoc_Changed(object? sender, EventArgs e)
    {
        if (!_dangKhoiTao) LocDuLieu(true);
    }
    private void BtnTaoThongBao_Click(object? sender, EventArgs e) => MoFormThem();
    private void BtnDanhDauDaDoc_Click(object? sender, EventArgs e) => DanhDauDaDoc();
    private void BtnLamMoi_Click(object? sender, EventArgs e) => TaiDuLieu();
    private void BtnTrangTruoc_Click(object? sender, EventArgs e) => ChuyenTrang(_trang - 1);
    private void BtnTrangSau_Click(object? sender, EventArgs e) => ChuyenTrang(_trang + 1);
    private void BtnDongChiTiet_Click(object? sender, EventArgs e) => XoaChiTiet();
    private void BtnGuiLai_Click(object? sender, EventArgs e) => MoFormThem();
    private void DgvThongBao_SelectionChanged(object? sender, EventArgs e) => HienThiChiTietDongDangChon();

    private void ChuyenTrang(int trang) { _trang = trang; HienThiTrang(); }

    private void DgvThongBao_CellClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0 || dgvThongBao.Rows[e.RowIndex].DataBoundItem is not ThongBaoGridRow row) return;
        if (e.ColumnIndex == colXem.Index) MoFormXem(row.MaThongBao);
        else if (e.ColumnIndex == colSua.Index) MoFormSua(row.MaThongBao);
        else if (e.ColumnIndex == colXoa.Index) MoFormXoa(row.MaThongBao);
        else HienThiChiTiet(row.MaThongBao);
    }

    private void HienThiChiTietDongDangChon()
    {
        if (dgvThongBao.CurrentRow?.DataBoundItem is ThongBaoGridRow row) HienThiChiTiet(row.MaThongBao);
    }

    private void HienThiChiTiet(int maThongBao)
    {
        _dangChon = _nguon.FirstOrDefault(x => x.MaThongBao == maThongBao);
        if (_dangChon == null) return;
        lblChiTietTieuDe.Text = _dangChon.TieuDe;
        lblChiTietMeta.Text = $"Loại thông báo:   {_dangChon.TenLoai}\nĐối tượng:           {_dangChon.DoiTuongNhan}\nNgày gửi:             {_dangChon.NgayGui:dd/MM/yyyy HH:mm}\nNgười gửi:            {(string.IsNullOrWhiteSpace(CurrentUser.HoTen) ? "Hệ thống" : CurrentUser.HoTen)}";
        txtNoiDung.Text = _dangChon.NoiDung;
        bool quanTrong = LaQuanTrong(_dangChon);
        lblMucDo.Text = quanTrong ? "⚠ Quan trọng" : (_dangChon.DaDoc ? "● Đã đọc" : "● Chưa đọc");
        lblSoNhanVien.Text = $"{_thongKeNguoiNhan.SoNhanVien:N0} người";
        lblSoDocGia.Text = $"{_thongKeNguoiNhan.SoDocGia:N0} người";
        lblTongNguoiNhan.Text = $"{_thongKeNguoiNhan.TongCong:N0} người";
    }

    private static string TenNhomDoiTuong(ThongBaoDashboardModel thongBao)
    {
        if (thongBao.LaThongBaoChung) return "Nhân viên & Độc giả";
        if (thongBao.MaNhanVien.HasValue) return "Nhân viên";
        if (thongBao.MaDocGia.HasValue) return "Độc giả";
        return thongBao.DoiTuongNhan;
    }

    private void XoaChiTiet() { _dangChon = null; lblChiTietTieuDe.Text = "Chọn một thông báo"; lblChiTietMeta.Text = "Loại thông báo: —\nĐối tượng: —\nNgày gửi: —\nNgười gửi: —"; txtNoiDung.Text = ""; }

    private void MoFormThem()
    {
        if (!KiemTraQuyen(PermissionHelper.CanAdd("HETHONG.THONGBAO"), "Bạn không có quyền tạo hoặc gửi bại thông báo.")) return;
        using var f = new FrmThemThongBao();
        if (f.ShowDialog(FindForm()) == DialogResult.OK) TaiDuLieu();
    }
    private void MoFormXem(int id) { var x = _nguon.FirstOrDefault(i => i.MaThongBao == id); if (x == null) return; using var f = new FrmChiTietThongBao(x); f.ShowDialog(FindForm()); }
    private void MoFormSua(int id)
    {
        if (!KiemTraQuyen(PermissionHelper.CanEdit("HETHONG.THONGBAO"), "Bạn không có quyền sửa thông báo.")) return;
        using var f = new FrmChinhSuaThongBao(id);
        if (f.ShowDialog(FindForm()) == DialogResult.OK) TaiDuLieu();
    }
    private void MoFormXoa(int id)
    {
        if (!KiemTraQuyen(PermissionHelper.CanDelete("HETHONG.THONGBAO"), "Bạn không có quyền xóa thông báo.")) return;
        var x = _nguon.FirstOrDefault(i => i.MaThongBao == id); if (x == null) return;
        using var f = new FrmXacNhanXoaThongBao(x); if (f.ShowDialog(FindForm()) != DialogResult.OK) return;
        if (Service.DeleteNotification(id)) TaiDuLieu(); else MessageBox.Show("Không tìm thấy thông báo cần xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }

    private void DanhDauDaDoc()
    {
        var items = _dangChon == null ? _nguon.Where(x => !x.DaDoc).ToList() : [_dangChon];
        foreach (var x in items.Where(x => !x.DaDoc)) if (Service.UpdateNotificationReadStatus(x.MaThongBao, true)) x.DaDoc = true;
        CapNhatKpi(); LocDuLieu(false);
    }

    private sealed class ThongBaoGridRow
    {
        public int MaThongBao { get; init; }
        public int STT { get; init; }
        public string ThongBao { get; init; } = "";
        public string ThoiGian { get; init; } = "";
        public string DoiTuong { get; init; } = "";
        public string TrangThai { get; init; } = "";
    }

    private void DgvThongBao_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.RowIndex < 0) return;
        if (e.ColumnIndex == colXem.Index)
        {
            e.Value = Properties.Resources.eye;
        }
        else if (e.ColumnIndex == colSua.Index)
        {
            e.Value = Properties.Resources.draw_9514973;
        }
        else if (e.ColumnIndex == colXoa.Index)
        {
            e.Value = Properties.Resources.delete;
        }
    }

    private void lblChuThich_Click(object sender, EventArgs e)
    {

    }
}
