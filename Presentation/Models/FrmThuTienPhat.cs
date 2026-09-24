using BusinessLayer.Services;
using DataLayer.Context;
using DataLayer.Entities;
using DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using Presentation.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentation.Models
{
    public partial class FrmThuTienPhat : Form
    {
        private int _maPhieuPhat;
        private int _maDocGia;
        private readonly MuonTraService _muonTraService;
        private readonly List<ChiTietPhatGridModel> _danhSachChiTietPhat = new();
        private bool _dangTai;

        public FrmThuTienPhat()
        {
            InitializeComponent();
            _maPhieuPhat = 0;
            _muonTraService = new MuonTraService();
            KhoiTaoForm();
        }

        public FrmThuTienPhat(int maPhieuPhat)
        {
            InitializeComponent();
            _maPhieuPhat = maPhieuPhat;
            _muonTraService = new MuonTraService();
            KhoiTaoForm();
        }

        private void KhoiTaoForm()
        {
            if (DesignModeHelper.IsDesignMode(this)) return;

            KeyPreview = true;
            StartPosition = FormStartPosition.CenterScreen;

            Load += FrmThuTienPhat_Load;

            btnTraCuuDocGia.Click += BtnTraCuuDocGia_Click;
            txtTraCuuDocGia.KeyDown += TxtTraCuuDocGia_KeyDown;

            btnTraCuuPhieuPhat.Click += BtnTraCuuPhieuPhat_Click;
            txtTraCuuPhieuPhat.KeyDown += TxtTraCuuPhieuPhat_KeyDown;

            txtGhiChuThanhToan.TextChanged += TxtGhiChuThanhToan_TextChanged;
            txtGhiChuHinhThuc.TextChanged += TxtGhiChuHinhThuc_TextChanged;
            txtSoTienKhachDua.TextChanged += TxtSoTienKhachDua_TextChanged;

            txtGhiChuThanhToan.MaxLength = 255;
            txtGhiChuHinhThuc.MaxLength = 255;
            btnXacNhanThanhToan.Click += BtnXacNhanThanhToan_Click;
            btnInBienLai.Click += BtnInBienLai_Click;
            btnLuuBienLai.Click += BtnLuuBienLai_Click;
            btnCloseBox.Click += (s, e) => Close();
        }

        private async void FrmThuTienPhat_Load(object? sender, EventArgs e)
        {
            try
            {
                NapDanhSachNhanVien();
                dtpNgayThanhToan.Value = DateTime.Today;

                if (_maPhieuPhat > 0)
                {
                    await TaiDuLieuPhieuPhatAsync(_maPhieuPhat);
                }
                else
                {
                    HienThiTrong();
                }

                CapNhatTinhToanTienThua();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể khởi tạo form thu tiền phạt: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void NapDanhSachNhanVien()
        {
            cboNhanVienLap.Items.Clear();
            cboNhanVienThu.Items.Clear();
            string curUser = string.IsNullOrWhiteSpace(CurrentUser.HoTen) ? "Chưa đăng nhập nhân viên" : CurrentUser.HoTen;
            cboNhanVienLap.Items.Add(curUser);
            cboNhanVienLap.SelectedIndex = 0;
            cboNhanVienThu.Items.Add(curUser);
            cboNhanVienThu.SelectedIndex = 0;
        }

        private void TxtGhiChuThanhToan_TextChanged(object? sender, EventArgs e)
        {
            int len = txtGhiChuThanhToan.Text.Length;
            lblDemGhiChuThanhToan.Text = $"{len}/255";
            lblDemGhiChuThanhToan.ForeColor = len > 255 ? Color.Red : Color.Gray;
        }

        private void TxtGhiChuHinhThuc_TextChanged(object? sender, EventArgs e)
        {
            int len = txtGhiChuHinhThuc.Text.Length;
            lblDemGhiChuHinhThuc.Text = $"{len}/255";
            lblDemGhiChuHinhThuc.ForeColor = len > 255 ? Color.Red : Color.Gray;
        }

        private void TxtSoTienKhachDua_TextChanged(object? sender, EventArgs e)
        {
            CapNhatTinhToanTienThua();
        }

        private async void TxtTraCuuDocGia_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                await TraCuuDocGiaAsync();
            }
        }

        private async void BtnTraCuuDocGia_Click(object? sender, EventArgs e)
        {
            await TraCuuDocGiaAsync();
        }

        private async Task TraCuuDocGiaAsync()
        {
            string kw = txtTraCuuDocGia.Text.Trim();
            if (string.IsNullOrWhiteSpace(kw))
            {
                MessageBox.Show("Vui lòng nhập Mã độc giả, SĐT hoặc Email để tra cứu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                await using AppDbContext context = new();
                string cleanKw = kw;
                if (cleanKw.StartsWith("DG", StringComparison.OrdinalIgnoreCase))
                {
                    cleanKw = cleanKw.Substring(2).TrimStart('0');
                    if (string.IsNullOrEmpty(cleanKw)) cleanKw = "0";
                }

                DocGium? dg = await context.DocGia
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.MaDocGia.ToString() == cleanKw ||
                                              x.SoDienThoai == kw ||
                                              x.Email == kw);

                if (dg != null)
                {
                    _maDocGia = dg.MaDocGia;
                    lblMaDocGiaHeader.Text = $"DG{dg.MaDocGia:D6}";
                    lblHoTen.Text = dg.HoTen;
                    lblSdt.Text = string.IsNullOrWhiteSpace(dg.SoDienThoai) ? "-" : dg.SoDienThoai;
                    lblEmail.Text = string.IsNullOrWhiteSpace(dg.Email) ? "-" : dg.Email;

                    picDocGia.Image = DatabaseImageHelper.LoadReaderAvatar(dg.AnhDaiDien, dg.HoTen, 85, 85);
                }
                else
                {
                    MessageBox.Show("Không tìm thấy độc giả tương ứng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tra cứu độc giả: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void TxtTraCuuPhieuPhat_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                await TraCuuPhieuPhatAsync();
            }
        }

        private async void BtnTraCuuPhieuPhat_Click(object? sender, EventArgs e)
        {
            await TraCuuPhieuPhatAsync();
        }

        private async Task TraCuuPhieuPhatAsync()
        {
            string kw = txtTraCuuPhieuPhat.Text.Trim();
            if (string.IsNullOrWhiteSpace(kw))
            {
                MessageBox.Show("Vui lòng nhập Mã phiếu phạt để tra cứu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                await using AppDbContext context = new();
                string cleanPpKw = kw;
                if (cleanPpKw.StartsWith("PP", StringComparison.OrdinalIgnoreCase))
                {
                    cleanPpKw = cleanPpKw.Substring(2).TrimStart('0');
                    if (string.IsNullOrEmpty(cleanPpKw)) cleanPpKw = "0";
                }

                PhieuPhat? pp = await context.PhieuPhats
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.MaPhieuPhat.ToString() == cleanPpKw ||
                                              x.MaPhieuPhatHienThi == kw);

                if (pp != null)
                {
                    await TaiDuLieuPhieuPhatAsync(pp.MaPhieuPhat);
                }
                else
                {
                    MessageBox.Show("Không tìm thấy phiếu phạt tương ứng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tra cứu phiếu phạt: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task TaiDuLieuPhieuPhatAsync(int maPhieuPhat)
        {
            if (_dangTai) return;

            try
            {
                _dangTai = true;
                await using AppDbContext context = new();

                PhieuPhat? pp = await context.PhieuPhats
                    .AsNoTracking()
                    .Include(x => x.MaDocGiaNavigation)
                        .ThenInclude(dg => dg.MaLopNavigation)
                    .Include(x => x.MaPhieuMuonNavigation)
                    .Include(x => x.ChiTietPhats)
                        .ThenInclude(ct => ct.MaChiTietMuonNavigation!)
                            .ThenInclude(ctm => ctm.MaCuonSachNavigation)
                                .ThenInclude(cs => cs.MaSachNavigation)
                    .FirstOrDefaultAsync(x => x.MaPhieuPhat == maPhieuPhat);

                if (pp == null)
                {
                    MessageBox.Show("Không tìm thấy thông tin phiếu phạt.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                _maPhieuPhat = pp.MaPhieuPhat;
                DocGium docGia = pp.MaDocGiaNavigation;
                _maDocGia = docGia.MaDocGia;

                lblMaDocGiaHeader.Text = $"DG{docGia.MaDocGia:D6}";
                lblHoTen.Text = docGia.HoTen;
                lblSdt.Text = string.IsNullOrWhiteSpace(docGia.SoDienThoai) ? "-" : docGia.SoDienThoai;
                lblEmail.Text = string.IsNullOrWhiteSpace(docGia.Email) ? "-" : docGia.Email;
                lblLop.Text = docGia.MaLopNavigation?.TenLop ?? "-";
                lblLoaiThe.Text = "Thẻ " + (docGia.LoaiDocGia?.ToLower() ?? "sinh viên");

                picDocGia.Image = DatabaseImageHelper.LoadReaderAvatar(docGia.AnhDaiDien, docGia.HoTen, 85, 85);

                txtTraCuuPhieuPhat.Text = string.IsNullOrWhiteSpace(pp.MaPhieuPhatHienThi) ? $"PP{pp.MaPhieuPhat:D6}" : pp.MaPhieuPhatHienThi;
                dtpNgayLapPhieu.Value = pp.NgayLap;

                lblTrangThaiPhieu.Text = pp.TrangThai;
                bool coTheThanhToan = pp.TrangThai != "Đã thanh toán" && pp.TrangThai != "Đã hủy";
                btnXacNhanThanhToan.Enabled = coTheThanhToan;
                lblTrangThaiPhieu.ForeColor = coTheThanhToan
                    ? Color.FromArgb(220, 38, 38)
                    : Color.FromArgb(22, 163, 74);

                _danhSachChiTietPhat.Clear();
                int stt = 1;
                int[] maChiTietMuonIds = pp.ChiTietPhats.Where(x => x.MaChiTietMuon.HasValue).Select(x => x.MaChiTietMuon!.Value).Distinct().ToArray();
                Dictionary<int, DateTime> ngayTraTheoChiTiet = await context.ChiTietTras
                    .AsNoTracking()
                    .Where(x => maChiTietMuonIds.Contains(x.MaChiTietMuon))
                    .Select(x => new { x.MaChiTietMuon, x.MaPhieuTraNavigation.NgayTra })
                    .ToDictionaryAsync(x => x.MaChiTietMuon, x => x.NgayTra);
                foreach (var ct in pp.ChiTietPhats)
                {
                    var s = ct.MaChiTietMuonNavigation?.MaCuonSachNavigation?.MaSachNavigation;
                    string tenSach = s?.TenSach ?? ct.NoiDung;
                    string maSachText = s?.MaSachHienThi ?? (s != null ? $"S{s.MaSach:D6}" : "-");

                    DateOnly hanTra = pp.MaPhieuMuonNavigation?.HanTra ?? DateOnly.FromDateTime(pp.NgayLap);
                    DateTime? ngayTraDt = ct.MaChiTietMuon.HasValue && ngayTraTheoChiTiet.TryGetValue(ct.MaChiTietMuon.Value, out DateTime ngayTraValue)
                        ? ngayTraValue
                        : null;
                    DateOnly ngayTra = ngayTraDt.HasValue ? DateOnly.FromDateTime(ngayTraDt.Value) : DateOnly.FromDateTime(pp.NgayLap);

                    _danhSachChiTietPhat.Add(new ChiTietPhatGridModel
                    {
                        STT = stt++,
                        MaSachText = maSachText,
                        TenSach = tenSach,
                        NgayTraDuKien = hanTra,
                        NgayTraThucTe = ngayTra,
                        SoNgayQuaHan = ct.SoNgayTre > 0 ? ct.SoNgayTre : 0,
                        DonGiaPhatNgay = ct.SoNgayTre > 0 ? ct.SoTien / ct.SoNgayTre : 0,
                        ThanhTien = ct.SoTien
                    });
                }

                if (_danhSachChiTietPhat.Count == 0)
                {
                    _danhSachChiTietPhat.Add(new ChiTietPhatGridModel
                    {
                        STT = 1,
                        MaSachText = "-",
                        TenSach = pp.GhiChu ?? "Phạt vi phạm quy định thư viện",
                        NgayTraDuKien = pp.MaPhieuMuonNavigation?.HanTra ?? DateOnly.FromDateTime(pp.NgayLap),
                        NgayTraThucTe = DateOnly.FromDateTime(pp.NgayLap),
                        SoNgayQuaHan = 0,
                        DonGiaPhatNgay = 0,
                        ThanhTien = pp.TongTien
                    });
                }

                HienThiGridChiTietPhat();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể tải chi tiết phiếu phạt: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _dangTai = false;
            }
        }

        private void HienThiTrong()
        {
            _maDocGia = 0;
            _maPhieuPhat = 0;

            lblMaDocGiaHeader.Text = "CHƯA CHỌN";
            lblHoTen.Text = "-";
            lblSdt.Text = "-";
            lblEmail.Text = "-";
            lblLop.Text = "-";
            lblLoaiThe.Text = "-";

            picDocGia.Image?.Dispose();
            picDocGia.Image = DatabaseImageHelper.LoadReaderAvatar(null, "?", 85, 85);

            txtTraCuuPhieuPhat.Text = "-";
            dtpNgayLapPhieu.Value = DateTime.Today;
            lblTrangThaiPhieu.Text = "-";

            _danhSachChiTietPhat.Clear();
            dgvChiTietPhat.Rows.Clear();
        }

        private void HienThiGridChiTietPhat()
        {
            dgvChiTietPhat.Rows.Clear();
            foreach (var item in _danhSachChiTietPhat)
            {
                dgvChiTietPhat.Rows.Add(
                    item.STT,
                    item.MaSachText,
                    item.TenSach,
                    item.NgayTraDuKien.ToString("dd/MM/yyyy"),
                    item.NgayTraThucTe.ToString("dd/MM/yyyy"),
                    item.SoNgayQuaHan,
                    $"{item.DonGiaPhatNgay:N0}",
                    $"{item.ThanhTien:N0}"
                );
            }

            decimal tongTien = _danhSachChiTietPhat.Sum(x => x.ThanhTien);
            lblTongCongPhat.Text = $"{tongTien:N0} đ";

            lblTongTienPhat.Text = $"{tongTien:N0} đ";
            if (lblTrangThaiPhieu.Text == "Đã thanh toán")
            {
                lblDaThanhToan.Text = $"{tongTien:N0} đ";
                lblConPhaiThu.Text = "0 đ";
            }
            else
            {
                lblDaThanhToan.Text = "0 đ";
                lblConPhaiThu.Text = $"{tongTien:N0} đ";
            }

            CapNhatTinhToanTienThua();
        }

        private void CapNhatTinhToanTienThua()
        {
            decimal tongTien = _danhSachChiTietPhat.Sum(x => x.ThanhTien);
            decimal khachDua = ParseMoney(txtSoTienKhachDua.Text);
            decimal tienThua = Math.Max(0, khachDua - tongTien);

            txtSoTienThua.Text = $"{tienThua:N0}";
        }

        private static decimal ParseMoney(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return 0;
            string clean = text.Replace(".", "").Replace(",", "").Trim();
            return decimal.TryParse(clean, out decimal val) ? val : 0;
        }


        private void BtnXacNhanThanhToan_Click(object? sender, EventArgs e)
        {
            if (!PermissionHelper.CanEdit("MUONTRA.PHAT"))
            {
                MessageBox.Show("Bạn không có quyền thu tiền phạt.", "Không đủ quyền", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_maPhieuPhat <= 0)
            {
                MessageBox.Show("Vui lòng tra cứu phiếu phạt trước khi xác nhận thanh toán.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!btnXacNhanThanhToan.Enabled)
            {
                MessageBox.Show("Phiếu phạt đã thanh toán hoặc đã hủy.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (!CurrentUser.MaNhanVien.HasValue)
            {
                MessageBox.Show("Phiên đăng nhập không có nhân viên hợp lệ. Vui lòng đăng nhập lại.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (dtpNgayThanhToan.Value.Date < dtpNgayLapPhieu.Value.Date)
            {
                MessageBox.Show("Ngày thanh toán không được trước ngày lập phiếu phạt.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string hinhThuc = rdoTienMat.Checked ? "Tiền mặt" : (rdoChuyenKhoan.Checked ? "Chuyển khoản" : "Ví điện tử");
            decimal tongTien = _danhSachChiTietPhat.Sum(x => x.ThanhTien);

            ThuTienPhatInputModel model = new()
            {
                MaPhieuPhat = _maPhieuPhat,
                MaDocGia = _maDocGia,
                NgayThanhToan = dtpNgayThanhToan.Value,
                MaNhanVienThu = CurrentUser.MaNhanVien.Value,
                HinhThucThanhToan = hinhThuc,
                SoTienKhachDua = ParseMoney(txtSoTienKhachDua.Text),
                TongTienPhat = tongTien,
                GhiChu = $"{txtGhiChuThanhToan.Text.Trim()} {txtGhiChuHinhThuc.Text.Trim()}".Trim()
            };

            if (string.Equals(hinhThuc, "Tiền mặt", StringComparison.OrdinalIgnoreCase) && model.SoTienKhachDua < tongTien)
            {
                MessageBox.Show("Số tiền khách đưa không đủ để thanh toán tiền phạt.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                bool success = _muonTraService.ThuTienPhatChiTiet(model);
                if (!success)
                {
                    MessageBox.Show("Thanh toán tiền phạt không thành công. Có thể phiếu đã thanh toán hoặc đã hủy.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                lblTrangThaiPhieu.Text = "Đã thanh toán";
                lblTrangThaiPhieu.ForeColor = Color.FromArgb(22, 163, 74);
                btnXacNhanThanhToan.Enabled = false;

                MessageBox.Show($"Xác nhận thanh toán tiền phạt thành công!\nMã phiếu: {txtTraCuuPhieuPhat.Text}\nSố tiền đã thu: {tongTien:N0} VNĐ",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể xác nhận thanh toán tiền phạt: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void BtnInBienLai_Click(object? sender, EventArgs e)
        {
            if (!PermissionHelper.CanPrint("MUONTRA.PHAT"))
            {
                MessageBox.Show("Bạn không có quyền in biên lai tiền phạt.", "Không đủ quyền", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_maPhieuPhat <= 0)
            {
                MessageBox.Show("Vui lòng tra cứu phiếu phạt trước khi in biên lai.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using System.Drawing.Printing.PrintDocument pd = new();
                pd.DocumentName = $"BienLaiThuPhat_{txtTraCuuPhieuPhat.Text}";
                pd.PrintPage += (s, ev) =>
                {
                    Graphics g = ev.Graphics!;
                    using Font fontHeader = new("Segoe UI", 16, FontStyle.Bold);
                    using Font fontSubHeader = new("Segoe UI", 10, FontStyle.Italic);
                    using Font fontTitle = new("Segoe UI", 14, FontStyle.Bold);
                    using Font fontBodyBold = new("Segoe UI", 10, FontStyle.Bold);
                    using Font fontBody = new("Segoe UI", 10, FontStyle.Regular);
                    using Brush brush = new SolidBrush(Color.Black);

                    float y = 40;
                    g.DrawString("TRƯỜNG ĐẠI HỌC ĐIỆN LỰC - THƯ VIỆN EPU", fontHeader, brush, 100, y); y += 30;
                    g.DrawString("Địa chỉ: 235 Hoàng Quốc Việt, Bắc Từ Liêm, Hà Nội", fontSubHeader, brush, 150, y); y += 40;
                    g.DrawString("--------------------------------------------------------------------------------", fontBody, brush, 80, y); y += 25;

                    g.DrawString("BIÊN LAI THU TIỀN PHẠT VI PHẠM", fontTitle, brush, 180, y); y += 40;

                    g.DrawString($"Mã phiếu phạt: {txtTraCuuPhieuPhat.Text}", fontBodyBold, brush, 80, y);
                    g.DrawString($"Ngày lập: {dtpNgayLapPhieu.Value:dd/MM/yyyy}", fontBody, brush, 450, y); y += 25;

                    g.DrawString($"Họ và tên độc giả: {lblHoTen.Text} ({lblMaDocGiaHeader.Text})", fontBody, brush, 80, y); y += 25;
                    g.DrawString($"SĐT: {lblSdt.Text}  |  Email: {lblEmail.Text}  |  Lớp: {lblLop.Text}", fontBody, brush, 80, y); y += 30;
                    g.DrawString("--------------------------------------------------------------------------------", fontBody, brush, 80, y); y += 25;

                    g.DrawString("CHI TIẾT CÁC KHOẢN PHẠT:", fontBodyBold, brush, 80, y); y += 30;
                    g.DrawString("STT | Mã sách  | Tên sách                             | Số ngày quá hạn | Số tiền", fontBodyBold, brush, 80, y); y += 25;

                    foreach (var item in _danhSachChiTietPhat)
                    {
                        string tenSachShort = item.TenSach.Length > 30 ? item.TenSach.Substring(0, 27) + "..." : item.TenSach.PadRight(30);
                        g.DrawString($"{item.STT,3} | {item.MaSachText,-8} | {tenSachShort} | Quá hạn {item.SoNgayQuaHan,2} ngày    | {item.ThanhTien:N0} đ", fontBody, brush, 80, y);
                        y += 25;
                    }

                    y += 10;
                    g.DrawString("--------------------------------------------------------------------------------", fontBody, brush, 80, y); y += 25;
                    decimal tongTien = _danhSachChiTietPhat.Sum(x => x.ThanhTien);
                    g.DrawString($"TỔNG TIỀN PHẠT: {tongTien:N0} VNĐ", fontTitle, brush, 80, y); y += 35;
                    g.DrawString($"Trạng thái thanh toán: {lblTrangThaiPhieu.Text}", fontBodyBold, brush, 80, y);
                    g.DrawString($"Ngày thu: {dtpNgayThanhToan.Value:dd/MM/yyyy}", fontBody, brush, 450, y); y += 30;

                    string hinhThuc = rdoTienMat.Checked ? "Tiền mặt" : (rdoChuyenKhoan.Checked ? "Chuyển khoản" : "Ví điện tử");
                    g.DrawString($"Hình thức thanh toán: {hinhThuc}", fontBody, brush, 80, y); y += 40;

                    g.DrawString("Người nộp tiền                                     Người thu tiền", fontBodyBold, brush, 150, y); y += 20;
                    g.DrawString("(Ký, ghi rõ họ tên)                                (Ký, ghi rõ họ tên)", fontSubHeader, brush, 140, y); y += 60;
                    g.DrawString($"{lblHoTen.Text}                                     {CurrentUser.HoTen}", fontBodyBold, brush, 140, y);
                };

                using PrintPreviewDialog ppd = new();
                ppd.Document = pd;
                ppd.Width = 800;
                ppd.Height = 600;
                ppd.ShowDialog(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể xem/in biên lai: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnLuuBienLai_Click(object? sender, EventArgs e)
        {
            if (!PermissionHelper.CanExport("MUONTRA.PHAT"))
            {
                MessageBox.Show("Bạn không có quyền lưu/xuất biên lai tiền phạt.", "Không đủ quyền", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_maPhieuPhat <= 0)
            {
                MessageBox.Show("Vui lòng tra cứu phiếu phạt trước khi lưu biên lai.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using SaveFileDialog sfd = new();
                sfd.Filter = "File Văn bản (*.txt)|*.txt|Tất cả tập tin (*.*)|*.*";
                sfd.FileName = $"BienLai_{txtTraCuuPhieuPhat.Text}.txt";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    using System.IO.StreamWriter writer = new(sfd.FileName, false, System.Text.Encoding.UTF8);
                    writer.WriteLine("TRƯỜNG ĐẠI HỌC ĐIỆN LỰC - THƯ VIỆN EPU");
                    writer.WriteLine("BIÊN LAI THU TIỀN PHẠT VI PHẠM");
                    writer.WriteLine("==============================================");
                    writer.WriteLine($"Mã phiếu phạt: {txtTraCuuPhieuPhat.Text}");
                    writer.WriteLine($"Ngày lập phiếu: {dtpNgayLapPhieu.Value:dd/MM/yyyy}");
                    writer.WriteLine($"Độc giả: {lblHoTen.Text} ({lblMaDocGiaHeader.Text})");
                    writer.WriteLine($"SĐT: {lblSdt.Text} | Email: {lblEmail.Text}");
                    writer.WriteLine("----------------------------------------------");
                    writer.WriteLine("Chi tiết các khoản phạt:");
                    foreach (var item in _danhSachChiTietPhat)
                    {
                        writer.WriteLine($"- Sách: {item.TenSach} ({item.MaSachText}) | Trễ: {item.SoNgayQuaHan} ngày | Thành tiền: {item.ThanhTien:N0} đ");
                    }
                    writer.WriteLine("----------------------------------------------");
                    decimal tongTien = _danhSachChiTietPhat.Sum(x => x.ThanhTien);
                    writer.WriteLine($"TỔNG TIỀN PHẠT: {tongTien:N0} VNĐ");
                    writer.WriteLine($"Trạng thái: {lblTrangThaiPhieu.Text}");
                    writer.WriteLine($"Ngày thu: {dtpNgayThanhToan.Value:dd/MM/yyyy}");
                    writer.WriteLine($"Người thu: {CurrentUser.HoTen}");
                    MessageBox.Show($"Đã lưu biên lai nộp phạt thành công!\nĐường dẫn: {sfd.FileName}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể lưu file biên lai: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lblTrangThaiPhieu_Click(object? sender, EventArgs e)
        {
        }
    }
}

