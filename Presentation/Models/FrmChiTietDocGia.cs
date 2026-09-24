using Presentation.Helpers;
using DataLayer.Context;
using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace Presentation.Models
{
    public partial class FrmChiTietDocGia : Form
    {
        private readonly int _maDocGia;
        private bool _dangTai;

        public FrmChiTietDocGia()
        {
            InitializeComponent();
            _maDocGia = 0;
            KhoiTaoForm();
        }

        public FrmChiTietDocGia(int maDocGia)
        {
            InitializeComponent();
            _maDocGia = maDocGia;
            KhoiTaoForm();
        }

        private bool DangOLucThietKe =>
            LicenseManager.UsageMode == LicenseUsageMode.Designtime || DesignMode;

        private void KhoiTaoForm()
        {
            if (DangOLucThietKe) return;

            Load += FrmChiTietDocGia_Load;
            btnDong.Click += BtnDong_Click;
            btnTabSachDangMuon.Click += (_, _) => HienThiTab(0);
            btnTabLichSuMuonTra.Click += (_, _) => HienThiTab(1);
            btnTabLePhi.Click += (_, _) => HienThiTab(2);
            btnTabPhieuPhat.Click += (_, _) => HienThiTab(3);

            dgvSachDangMuon.CellFormatting += DgvSachDangMuon_CellFormatting;
            dgvLichSuMuonTra.CellFormatting += DgvLichSuMuonTra_CellFormatting;
            dgvLePhi.CellFormatting += DgvLePhi_CellFormatting;
            guna2DataGridView1.CellFormatting += DgvPhieuPhat_CellFormatting;
            dgvDongPhiThuongNien.CellFormatting += DgvDongPhiThuongNien_CellFormatting;
            dgvPhatChuaThanhToan.CellFormatting += DgvPhatChuaThanhToan_CellFormatting;

            CaiDatGiaoDien();
        }

        private async void FrmChiTietDocGia_Load(object? sender, EventArgs e)
        {
            HienThiTab(0);

            if (_maDocGia <= 0)
            {
                HienThiDuLieuRong();
                return;
            }

            await TaiToanBoDuLieuAsync();
        }

        private async Task TaiToanBoDuLieuAsync()
        {
            if (_dangTai) return;

            try
            {
                _dangTai = true;
                UseWaitCursor = true;
                btnDong.Enabled = false;

                await using AppDbContext context = new();

                DocGium? docGia = await context.DocGia
                    .AsNoTracking()
                    .Include(x => x.MaLopNavigation)
                        .ThenInclude(x => x!.MaKhoaNavigation)
                    .Include(x => x.TheDocGium)
                    .FirstOrDefaultAsync(x => x.MaDocGia == _maDocGia);

                if (docGia == null)
                {
                    MessageBox.Show(
                        "Không tìm thấy độc giả cần xem.",
                        "Thông báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    Close();
                    return;
                }

                int? maThe = docGia.TheDocGium?.MaThe;

                List<PhieuMuon> phieuMuons = maThe.HasValue
                    ? await context.PhieuMuons
                        .AsNoTracking()
                        .Where(x => x.MaThe == maThe.Value)
                        .Include(x => x.MaNhanVienNavigation)
                        .Include(x => x.ChiTietMuons)
                            .ThenInclude(x => x.MaCuonSachNavigation)
                                .ThenInclude(x => x.MaSachNavigation)
                        .Include(x => x.ChiTietMuons)
                            .ThenInclude(x => x.ChiTietTra)
                                .ThenInclude(x => x!.MaPhieuTraNavigation)
                        .OrderByDescending(x => x.NgayMuon)
                        .ToListAsync()
                    : new List<PhieuMuon>();

                List<DongPhiThuongNien> dongPhis = maThe.HasValue
                    ? await context.DongPhiThuongNiens
                        .AsNoTracking()
                        .Where(x => x.MaThe == maThe.Value)
                        .Include(x => x.MaNhanVienThuNavigation)
                        .OrderByDescending(x => x.Nam)
                        .ToListAsync()
                    : new List<DongPhiThuongNien>();

                List<PhieuPhat> phieuPhats = await context.PhieuPhats
                    .AsNoTracking()
                    .Where(x => x.MaDocGia == _maDocGia)
                    .Include(x => x.MaNhanVienNavigation)
                    .Include(x => x.MaPhieuMuonNavigation)
                    .Include(x => x.ChiTietPhats)
                    .OrderByDescending(x => x.NgayLap)
                    .ToListAsync();

                GanThongTinDocGia(docGia, dongPhis);
                GanSachDangMuon(phieuMuons);
                GanLichSuMuonTra(phieuMuons);
                GanLePhi(dongPhis);
                GanPhieuPhat(phieuPhats);
                GanThongKe(phieuMuons, phieuPhats);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể tải chi tiết độc giả.\n" + ex.Message,
                    "Lỗi dữ liệu",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                btnDong.Enabled = true;
                UseWaitCursor = false;
                _dangTai = false;
            }
        }

        private void GanThongTinDocGia(
            DocGium docGia,
            IReadOnlyCollection<DongPhiThuongNien> dongPhis)
        {
            TheDocGium? the = docGia.TheDocGium;
            string maDocGiaText = $"DG{docGia.MaDocGia:0000}";

            lblMaDocGia.Text = maDocGiaText;
            lblHoTen.Text = docGia.HoTen;
            lblNgaySinh.Text = docGia.NgaySinh?.ToString("dd/MM/yyyy") ?? "-";
            lblSoDienThoai.Text = GiaTriHoacGach(docGia.SoDienThoai);
            lblEmail.Text = GiaTriHoacGach(docGia.Email);
            lblDiaChi.Text = GiaTriHoacGach(docGia.DiaChi);
            lblMaSinhVien.Text = GiaTriHoacGach(docGia.MaSinhVien);
            lblNgayDangKy.Text = docGia.NgayDangKy.ToString("dd/MM/yyyy");
            lblCapNhatCuoi.Text = "-";
            lblNguoiCapNhat.Text = "-";

            // Label có tên mặc định trong Designer nhưng đang dùng để hiển thị giới tính.
            guna2HtmlLabel8.Text = GiaTriHoacGach(docGia.GioiTinh);

            lblLoaiDocGia.Text = GiaTriHoacGach(docGia.LoaiDocGia);
            lblLop.Text = docGia.MaLopNavigation?.TenLop ?? "-";
            lblKhoa.Text = docGia.MaLopNavigation?.MaKhoaNavigation?.TenKhoa ?? "-";

            lblMaThe.Text = the?.MaTheHienThi ?? (the == null ? "-" : $"TDG{the.MaThe:000000}");
            lblNgayCapThe.Text = the?.NgayCap.ToString("dd/MM/yyyy") ?? "-";
            lblNgayHetHan.Text = the?.NgayHetHan.ToString("dd/MM/yyyy") ?? "-";

            string trangThai = XacDinhTrangThaiThe(docGia, the, dongPhis);
            DatBadgeTrangThaiThe(btnTrangThaiThe, trangThai);
            DatBadgeTrangThaiThe(btnTrangThaiTheChiTiet, trangThai);

            if (the != null)
            {
                DateOnly homNay = DateOnly.FromDateTime(DateTime.Today);
                lblNgayHetHan.ForeColor = the.NgayHetHan < homNay
                    ? Color.FromArgb(225, 55, 65)
                    : the.NgayHetHan <= homNay.AddDays(30)
                        ? Color.FromArgb(235, 135, 15)
                        : Color.FromArgb(35, 50, 85);
            }

            Image? oldImage = picDocGia.Image;
            picDocGia.Image = DatabaseImageHelper.LoadReaderAvatar(docGia.AnhDaiDien, docGia.HoTen, picDocGia.Width, picDocGia.Height);
            if (oldImage != null && !ReferenceEquals(oldImage, picDocGia.Image)) oldImage.Dispose();
        }

        private static string XacDinhTrangThaiThe(
            DocGium docGia,
            TheDocGium? the,
            IReadOnlyCollection<DongPhiThuongNien> dongPhis)
        {
            if (!docGia.TrangThai) return "Bị khóa";
            if (the == null) return "Chưa cấp thẻ";

            string trangThaiDb = the.TrangThai?.Trim() ?? string.Empty;
            if (trangThaiDb.Contains("khóa", StringComparison.OrdinalIgnoreCase)) return "Bị khóa";

            DateOnly homNay = DateOnly.FromDateTime(DateTime.Today);
            if (the.NgayHetHan < homNay) return "Hết hạn";

            bool daDongPhiNamNay = dongPhis.Any(x =>
                x.Nam == DateTime.Today.Year &&
                x.LoaiLePhi == "Lệ phí thường niên" &&
                DaThanhToan(x.TrangThai));

            if (!daDongPhiNamNay) return "Chưa đóng lệ phí";
            if (the.NgayHetHan <= homNay.AddDays(30)) return "Sắp hết hạn";
            return "Còn hiệu lực";
        }

        private void GanSachDangMuon(IReadOnlyCollection<PhieuMuon> phieuMuons)
        {
            DateOnly homNay = DateOnly.FromDateTime(DateTime.Today);

            var data = phieuMuons
                .SelectMany(pm => pm.ChiTietMuons.Select(ct => new { pm, ct }))
                .Where(x => x.ct.ChiTietTra == null &&
                            !x.ct.TrangThai.Contains("Đã trả", StringComparison.OrdinalIgnoreCase))
                .OrderBy(x => x.pm.HanTra)
                .Select((x, index) => new
                {
                    STT = index + 1,
                    TenSach = x.ct.MaCuonSachNavigation.MaSachNavigation.TenSach,
                    MaCuon = x.ct.MaCuonSachNavigation.MaCuonSachHienThi ?? $"CS{x.ct.MaCuonSach:000000}",
                    NgayMuonText = x.pm.NgayMuon.ToString("dd/MM/yyyy"),
                    HanTraText = x.pm.HanTra.ToString("dd/MM/yyyy"),
                    TinhTrang = x.pm.HanTra < homNay
                        ? "Quá hạn"
                        : x.pm.HanTra <= homNay.AddDays(3)
                            ? "Sắp hết hạn"
                            : "Đang mượn"
                })
                .ToList();

            dgvSachDangMuon.DataSource = data;
            btnTabSachDangMuon.Text = $"SÁCH ĐANG MƯỢN ({data.Count})";
        }

        private void GanLichSuMuonTra(IReadOnlyCollection<PhieuMuon> phieuMuons)
        {
            var data = phieuMuons
                .SelectMany(pm => pm.ChiTietMuons.Select(ct => new { pm, ct }))
                .OrderByDescending(x => x.pm.NgayMuon)
                .Select((x, index) => new
                {
                    STT = index + 1,
                    MaPhieu = x.pm.MaPhieuMuonHienThi ?? $"PM{x.pm.MaPhieuMuon:000000}",
                    TenSach = x.ct.MaCuonSachNavigation?.MaSachNavigation?.TenSach ?? "Sách chưa xác định",
                    NgayMuonText = x.pm.NgayMuon.ToString("dd/MM/yyyy"),
                    NgayTraText = x.ct.ChiTietTra?.MaPhieuTraNavigation?.NgayTra.ToString("dd/MM/yyyy HH:mm") ?? "-",
                    TrangThai = x.ct.ChiTietTra == null ? x.ct.TrangThai : "Đã trả",
                    NguoiThucHien = x.pm.MaNhanVienNavigation?.HoTen ?? "-"
                })
                .ToList();

            dgvLichSuMuonTra.DataSource = data;
        }

        private void GanLePhi(IReadOnlyCollection<DongPhiThuongNien> dongPhis)
        {
            var data = dongPhis
                .OrderByDescending(x => x.Nam)
                .Select(x => new
                {
                    Nam = x.Nam,
                    SoTien = $"{x.SoTien:N0} đ",
                    NgayDongText = x.NgayDong.ToString("dd/MM/yyyy"),
                    NguoiThu = x.MaNhanVienThuNavigation?.HoTen ?? "-",
                    TrangThaiLePhi = $"{x.LoaiLePhi} - {x.TrangThai}"
                })
                .ToList();

            dgvLePhi.DataSource = data;

            dgvDongPhiThuongNien.DataSource = dongPhis
                .OrderByDescending(x => x.Nam)
                .Take(3)
                .Select(x => new
                {
                    NamDongPhi = x.Nam,
                    SoTienDongPhi = $"{x.SoTien:N0} đ",
                    NgayDongPhi = x.NgayDong.ToString("dd/MM/yyyy"),
                    TrangThaiDongPhi = x.TrangThai
                })
                .ToList();
        }

        private void GanPhieuPhat(IReadOnlyCollection<PhieuPhat> phieuPhats)
        {
            var tatCa = phieuPhats
                .Select(x => new
                {
                    NgayLap = x.NgayLap.ToString("dd/MM/yyyy"),
                    MaPhieuPhat = x.MaPhieuPhatHienThi ?? $"PP{x.MaPhieuPhat:000000}",
                    NgayMuonText = x.MaPhieuMuonNavigation?.NgayMuon.ToString("dd/MM/yyyy") ?? "-",
                    LyDo = LayLyDoPhat(x),
                    SoTien = $"{x.TongTien:N0} đ",
                    TrangThaiPhat = x.TrangThai
                })
                .ToList();

            guna2DataGridView1.DataSource = tatCa;

            List<PhieuPhat> chuaThanhToan = phieuPhats
                .Where(x => !DaThanhToan(x.TrangThai))
                .ToList();

            dgvPhatChuaThanhToan.DataSource = chuaThanhToan
                .Take(4)
                .Select(x => new
                {
                    NgayLapPhat = x.NgayLap.ToString("dd/MM/yyyy"),
                    LyDoPhat = LayLyDoPhat(x),
                    SoTienPhatChuaThanhToan = $"{x.TongTien:N0} đ",
                    TrangThaiChuaThanhToan = x.TrangThai
                })
                .ToList();

            lblTongTienPhatChuaThanhToan.Text = $"{chuaThanhToan.Sum(x => x.TongTien):N0} đ";
        }

        private void GanThongKe(
            IReadOnlyCollection<PhieuMuon> phieuMuons,
            IReadOnlyCollection<PhieuPhat> phieuPhats)
        {
            DateOnly homNay = DateOnly.FromDateTime(DateTime.Today);

            List<(PhieuMuon pm, ChiTietMuon ct)> tatCa = phieuMuons
                .SelectMany(pm => pm.ChiTietMuons.Select(ct => (pm, ct)))
                .ToList();

            int dangMuon = tatCa.Count(x =>
                x.ct.ChiTietTra == null &&
                !x.ct.TrangThai.Contains("Đã trả", StringComparison.OrdinalIgnoreCase));

            int quaHan = tatCa.Count(x =>
                x.ct.ChiTietTra == null &&
                x.pm.HanTra < homNay);

            decimal tongTienPhat = phieuPhats.Sum(x => x.TongTien);

            lblTongSachDaMuon.Text = $"{tatCa.Count:N0} cuốn";
            lblDanngMuon.Text = $"{dangMuon:N0} cuốn";
            guna2HtmlLabel21.Text = $"{quaHan:N0} cuốn";
            guna2HtmlLabel24.Text = $"{tongTienPhat:N0} đ";
        }

        private void HienThiTab(int index)
        {
            pnlSachDangMuon.Visible = index == 0;
            pnlLichSuMuonTra.Visible = index == 1;
            pnlLePhi.Visible = index == 2;
            pnlPhieuPhat.Visible = index == 3;

            Control panel = index switch
            {
                1 => pnlLichSuMuonTra,
                2 => pnlLePhi,
                3 => pnlPhieuPhat,
                _ => pnlSachDangMuon
            };
            panel.BringToFront();

            Guna.UI2.WinForms.Guna2Button[] buttons =
            {
                btnTabSachDangMuon,
                btnTabLichSuMuonTra,
                btnTabLePhi,
                btnTabPhieuPhat
            };

            foreach (var button in buttons)
                button.ForeColor = Color.FromArgb(70, 80, 112);

            Guna.UI2.WinForms.Guna2Button selected = buttons[Math.Clamp(index, 0, buttons.Length - 1)];
            selected.ForeColor = Color.FromArgb(25, 85, 225);
            pnlTabIndicator.Left = selected.Left;
            pnlTabIndicator.Width = selected.Width;
        }

        private void CaiDatGiaoDien()
        {
            BackColor = Color.FromArgb(247, 249, 253);
            DoubleBuffered = true;

            Guna.UI2.WinForms.Guna2Panel[] panels =
            {
                pnlHoSoTrai,
                pnlThongTinCaNhan,
                pnlThongTinHocTap,
                pnlThongTinThe,
                pnlTabBody,
                pnlThongKe,
                pnlDongPhiThuongNien,
                pnlPhatChuaThanhToan
            };

            foreach (var panel in panels)
            {
                panel.FillColor = Color.White;
                panel.BorderColor = Color.FromArgb(220, 227, 240);
                panel.BorderThickness = 1;
                panel.BorderRadius = 10;
            }

            CauHinhGrid(dgvSachDangMuon);
            CauHinhGrid(dgvLichSuMuonTra);
            CauHinhGrid(dgvLePhi);
            CauHinhGrid(guna2DataGridView1);
            CauHinhGrid(dgvDongPhiThuongNien);
            CauHinhGrid(dgvPhatChuaThanhToan);

            btnDong.BorderRadius = 7;
            btnDong.Cursor = Cursors.Hand;
        }

        private static void CauHinhGrid(DataGridView grid)
        {
            grid.AutoGenerateColumns = false;
            grid.AllowUserToAddRows = false;
            grid.AllowUserToDeleteRows = false;
            grid.AllowUserToResizeRows = false;
            grid.ReadOnly = true;
            grid.RowHeadersVisible = false;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.MultiSelect = false;
            grid.BackgroundColor = Color.White;
            grid.BorderStyle = BorderStyle.None;
            grid.GridColor = Color.FromArgb(226, 231, 240);
            grid.EnableHeadersVisualStyles = false;
            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(245, 247, 252);
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(60, 70, 90);
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            grid.DefaultCellStyle.BackColor = Color.FromArgb(245, 247, 252);
            grid.DefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            grid.DefaultCellStyle.ForeColor = Color.FromArgb(60, 70, 90);
            grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(245, 247, 252);
            grid.DefaultCellStyle.SelectionForeColor = Color.FromArgb(60, 70, 90);
            grid.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            grid.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
        }

        private void HienThiDuLieuRong()
        {
            lblMaDocGia.Text = "DG----";
            lblHoTen.Text = "Chưa chọn độc giả";
            btnTabSachDangMuon.Text = "SÁCH ĐANG MƯỢN (0)";
            dgvSachDangMuon.DataSource = null;
            dgvLichSuMuonTra.DataSource = null;
            dgvLePhi.DataSource = null;
            guna2DataGridView1.DataSource = null;
            dgvDongPhiThuongNien.DataSource = null;
            dgvPhatChuaThanhToan.DataSource = null;
        }

        private static string GiaTriHoacGach(string? value) =>
            string.IsNullOrWhiteSpace(value) ? "-" : value.Trim();

        private static bool DaThanhToan(string? trangThai)
        {
            if (string.IsNullOrWhiteSpace(trangThai) ||
                trangThai.Contains("Chưa", StringComparison.OrdinalIgnoreCase) ||
                trangThai.Contains("hủy", StringComparison.OrdinalIgnoreCase) ||
                trangThai.Contains("Hoàn", StringComparison.OrdinalIgnoreCase))
                return false;

            return trangThai.Contains("Đã thanh toán", StringComparison.OrdinalIgnoreCase) ||
                   trangThai.Contains("Đã thu", StringComparison.OrdinalIgnoreCase);
        }

        private static string LayLyDoPhat(PhieuPhat phieuPhat)
        {
            string? chiTiet = phieuPhat.ChiTietPhats
                .Select(x => !string.IsNullOrWhiteSpace(x.NoiDung) ? x.NoiDung : x.LoaiPhat)
                .FirstOrDefault(x => !string.IsNullOrWhiteSpace(x));

            return !string.IsNullOrWhiteSpace(chiTiet)
                ? chiTiet
                : GiaTriHoacGach(phieuPhat.GhiChu);
        }

        private static void DatBadgeTrangThaiThe(
            Guna.UI2.WinForms.Guna2Button button,
            string trangThai)
        {
            (Color fill, Color fore, Color border) = trangThai switch
            {
                "Còn hiệu lực" =>
                    (Color.FromArgb(228, 247, 234), Color.FromArgb(22, 145, 68), Color.FromArgb(183, 230, 198)),
                "Sắp hết hạn" =>
                    (Color.FromArgb(255, 244, 224), Color.FromArgb(225, 125, 10), Color.FromArgb(250, 217, 160)),
                "Hết hạn" =>
                    (Color.FromArgb(255, 235, 238), Color.FromArgb(225, 55, 65), Color.FromArgb(248, 190, 198)),
                "Chưa đóng lệ phí" =>
                    (Color.FromArgb(255, 240, 224), Color.FromArgb(215, 115, 5), Color.FromArgb(248, 210, 165)),
                "Bị khóa" =>
                    (Color.FromArgb(238, 241, 246), Color.FromArgb(85, 95, 115), Color.FromArgb(210, 215, 225)),
                _ =>
                    (Color.FromArgb(238, 241, 246), Color.FromArgb(85, 95, 115), Color.FromArgb(210, 215, 225))
            };

            button.Text = trangThai;
            button.FillColor = fill;
            button.ForeColor = fore;
            button.BorderColor = border;
            button.BorderThickness = 1;
            button.BorderRadius = 7;
            button.Enabled = true;
            button.TabStop = false;
            button.Cursor = Cursors.Default;
        }


        private static void ToMauTrangThai(DataGridViewCellFormattingEventArgs e, string text)
        {
            if (text.Contains("Còn hiệu lực", StringComparison.OrdinalIgnoreCase) ||
                text.Contains("Đang mượn", StringComparison.OrdinalIgnoreCase) ||
                text.Contains("Đã", StringComparison.OrdinalIgnoreCase))
            {
                e.CellStyle.ForeColor = Color.FromArgb(20, 145, 70);
                e.CellStyle.BackColor = Color.FromArgb(231, 248, 236);
            }
            else if (text.Contains("Sắp", StringComparison.OrdinalIgnoreCase) ||
                     text.Contains("Chờ", StringComparison.OrdinalIgnoreCase))
            {
                e.CellStyle.ForeColor = Color.FromArgb(225, 125, 10);
                e.CellStyle.BackColor = Color.FromArgb(255, 245, 225);
            }
            else if (text.Contains("Quá hạn", StringComparison.OrdinalIgnoreCase) ||
                     text.Contains("Chưa", StringComparison.OrdinalIgnoreCase) ||
                     text.Contains("Hết hạn", StringComparison.OrdinalIgnoreCase))
            {
                e.CellStyle.ForeColor = Color.FromArgb(225, 55, 65);
                e.CellStyle.BackColor = Color.FromArgb(255, 235, 238);
            }
        }

        private void DgvSachDangMuon_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvSachDangMuon.Columns[e.ColumnIndex].Name == "colTinhTrang")
                ToMauTrangThai(e, e.Value?.ToString() ?? string.Empty);
        }

        private void DgvLichSuMuonTra_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvLichSuMuonTra.Columns[e.ColumnIndex].Name == "colTrangThaiMuonTra")
                ToMauTrangThai(e, e.Value?.ToString() ?? string.Empty);
        }

        private void DgvLePhi_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvLePhi.Columns[e.ColumnIndex].Name == "colTrangThaiLePhi")
                ToMauTrangThai(e, e.Value?.ToString() ?? string.Empty);
        }

        private void DgvPhieuPhat_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0 && guna2DataGridView1.Columns[e.ColumnIndex].Name == "colTrangThaiPhat")
                ToMauTrangThai(e, e.Value?.ToString() ?? string.Empty);
        }

        private void DgvDongPhiThuongNien_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvDongPhiThuongNien.Columns[e.ColumnIndex].Name == "colTrangThaiDongPhi")
                ToMauTrangThai(e, e.Value?.ToString() ?? string.Empty);
        }

        private void DgvPhatChuaThanhToan_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvPhatChuaThanhToan.Columns[e.ColumnIndex].Name == "colTrangThaiChuaThanhToan")
                ToMauTrangThai(e, e.Value?.ToString() ?? string.Empty);
        }

        private void BtnDong_Click(object? sender, EventArgs e) => Close();

        // Giữ lại vì Designer hiện tại vẫn đang liên kết hai sự kiện này.
        private void guna2HtmlLabel5_Click(object? sender, EventArgs e) { }
        private void pnlThongTinCaNhan_Paint(object? sender, PaintEventArgs e) { }
    }
}


