using BusinessLayer.Services;
using DataLayer.Context;
using DataLayer.Entities;
using DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using Presentation.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentation.Models
{
    public partial class FrmSuaDocGia : Form
    {
        private readonly int _maDocGia;
        private readonly DocGiaService _docGiaService;
        private bool _dangTai;
        private bool _dangNapForm;
        private string? _anhDaiDien;

        public FrmSuaDocGia()
        {
            InitializeComponent();
            _maDocGia = 0;
            _docGiaService = new DocGiaService();
            KhoiTaoForm();
        }

        public FrmSuaDocGia(int maDocGia)
        {
            InitializeComponent();
            _maDocGia = maDocGia;
            _docGiaService = new DocGiaService();
            KhoiTaoForm();
        }

        private void KhoiTaoForm()
        {
            if (DesignModeHelper.IsDesignMode(this)) return;

            KeyPreview = true;
            StartPosition = FormStartPosition.CenterScreen;

            Load += FrmSuaDocGia_Load;
            btnLuu.Click += BtnLuu_Click;
            btnHuy.Click += BtnHuy_Click;
            btnCloseBox.Click += BtnHuy_Click;
            btnChonAnh.Click += BtnChonAnh_Click;
            cboKhoa.SelectedIndexChanged += CboKhoa_SelectedIndexChanged;
            cboLoaiDocGia.SelectedIndexChanged += CboLoaiDocGia_SelectedIndexChanged;
            txtGhiChu.TextChanged += TxtGhiChu_TextChanged;

            dgvSachDangMuon.CellFormatting += DgvSachDangMuon_CellFormatting;
            dgvLichSuMuon.CellFormatting += DgvLichSuMuon_CellFormatting;

            lblXemTatCaMuon.Click += (s, e) => MoLichSuMuon();
            lblXemTatCaLichSu.Click += (s, e) => MoLichSuMuon();
            lblXemLichSuLePhi.Click += (s, e) => MessageBox.Show("Xem lịch sử lệ phí độc giả.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            lblXemChiTietPhat.Click += (s, e) => MessageBox.Show("Xem chi tiết phiếu phạt độc giả.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async void FrmSuaDocGia_Load(object? sender, EventArgs e)
        {
            try
            {
                _dangNapForm = true;

                NapDanhSachLoaiDocGia();
                NapDanhSachTrangThaiDocGia();
                NapDanhSachTrangThaiThe();
                NapDanhSachNamLePhi();

                await NapDanhSachKhoaAsync();

                if (_maDocGia > 0)
                {
                    await TaiToanBoDuLieuAsync();
                }
                else
                {
                    HienThiDuLieuRong();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Đã xảy ra lỗi khi khởi tạo form: " + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                _dangNapForm = false;
            }
        }

        private void NapDanhSachLoaiDocGia()
        {
            cboLoaiDocGia.Items.Clear();
            cboLoaiDocGia.Items.AddRange(new object[]
            {
                "Sinh viên",
                "Giảng viên",
                "Nhân viên",
                "Khác"
            });
            cboLoaiDocGia.SelectedIndex = 0;
        }

        private void NapDanhSachTrangThaiDocGia()
        {
            cboTrangThaiDocGia.Items.Clear();
            cboTrangThaiDocGia.Items.AddRange(new object[]
            {
                "Hoạt động",
                "Ngừng hoạt động"
            });
            cboTrangThaiDocGia.SelectedIndex = 0;
        }

        private void NapDanhSachTrangThaiThe()
        {
            cboTrangThaiThe.Items.Clear();
            cboTrangThaiThe.Items.AddRange(new object[]
            {
                "Còn hiệu lực",
                "Bị khóa"
            });
            cboTrangThaiThe.SelectedIndex = 0;
        }

        private void NapDanhSachNamLePhi()
        {
            cboNamLePhi.Items.Clear();
            int namHienTai = DateTime.Today.Year;
            for (int nam = namHienTai - 3; nam <= namHienTai + 1; nam++)
            {
                cboNamLePhi.Items.Add(nam.ToString());
            }
            cboNamLePhi.SelectedItem = namHienTai.ToString();
        }

        private async Task NapDanhSachKhoaAsync()
        {
            List<LookupItemModel> dsKhoa = await Task.Run(() => _docGiaService.GetDanhSachKhoaLookup());
            cboKhoa.DataSource = dsKhoa;
            cboKhoa.DisplayMember = "Ten";
            cboKhoa.ValueMember = "Id";
            if (dsKhoa.Count > 0)
                cboKhoa.SelectedIndex = 0;
        }

        private async Task NapDanhSachLopAsync(int? maKhoa)
        {
            List<LookupItemModel> dsLop = await Task.Run(() => _docGiaService.GetDanhSachLopLookup(maKhoa));
            cboLop.DataSource = dsLop;
            cboLop.DisplayMember = "Ten";
            cboLop.ValueMember = "Id";
            if (dsLop.Count > 0)
                cboLop.SelectedIndex = 0;
            else
                cboLop.DataSource = null;
        }

        private async void CboKhoa_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (_dangNapForm || cboKhoa.SelectedValue is not int maKhoa) return;
            await NapDanhSachLopAsync(maKhoa);
        }

        private void CboLoaiDocGia_SelectedIndexChanged(object? sender, EventArgs e)
        {
            bool isSinhVien = cboLoaiDocGia.SelectedItem?.ToString() == "Sinh viên";
            cboKhoa.Enabled = isSinhVien;
            cboLop.Enabled = isSinhVien;
            txtMaSinhVien.Enabled = isSinhVien;
        }

        private void TxtGhiChu_TextChanged(object? sender, EventArgs e)
        {
            int len = txtGhiChu.Text.Length;
            lblDemGhiChu.Text = $"{len}/255";
            lblDemGhiChu.ForeColor = len > 255 ? Color.Red : Color.Gray;
        }

        private async Task TaiToanBoDuLieuAsync()
        {
            if (_dangTai) return;

            try
            {
                _dangTai = true;
                UseWaitCursor = true;
                btnLuu.Enabled = false;

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
                        "Không tìm thấy độc giả cần chỉnh sửa.",
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
                        .Include(x => x.ChiTietMuons)
                            .ThenInclude(x => x.MaCuonSachNavigation)
                                .ThenInclude(x => x.MaSachNavigation)
                        .Include(x => x.ChiTietMuons)
                            .ThenInclude(x => x.ChiTietTra)
                        .OrderByDescending(x => x.NgayMuon)
                        .ToListAsync()
                    : new List<PhieuMuon>();

                List<DongPhiThuongNien> dongPhis = maThe.HasValue
                    ? await context.DongPhiThuongNiens
                        .AsNoTracking()
                        .Where(x => x.MaThe == maThe.Value)
                        .OrderByDescending(x => x.Nam)
                        .ToListAsync()
                    : new List<DongPhiThuongNien>();

                List<PhieuPhat> phieuPhats = await context.PhieuPhats
                    .AsNoTracking()
                    .Where(x => x.MaDocGia == _maDocGia)
                    .ToListAsync();

                await GanThongTinDocGiaAsync(docGia, dongPhis);
                GanSachDangMuon(phieuMuons);
                GanLichSuMuon(phieuMuons);
                GanLePhi(dongPhis);
                GanTienPhat(phieuPhats);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể tải thông tin độc giả.\n" + ex.Message,
                    "Lỗi dữ liệu",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                btnLuu.Enabled = true;
                UseWaitCursor = false;
                _dangTai = false;
            }
        }

        private async Task GanThongTinDocGiaAsync(DocGium docGia, List<DongPhiThuongNien> dongPhis)
        {
            TheDocGium? the = docGia.TheDocGium;
            string maDocGiaText = $"DG{docGia.MaDocGia:D4}";

            lblMaDocGiaHeader.Text = maDocGiaText;
            txtMaDocGia.Text = maDocGiaText;
            txtHoTen.Text = docGia.HoTen;
            txtMaSinhVien.Text = docGia.MaSinhVien ?? string.Empty;
            lblMaSinhVien.Text = string.IsNullOrWhiteSpace(docGia.MaSinhVien) ? "-" : docGia.MaSinhVien;

            dtpNgaySinh.Value = docGia.NgaySinh.HasValue
                ? docGia.NgaySinh.Value.ToDateTime(TimeOnly.MinValue)
                : DateTime.Today.AddYears(-20);

            if (docGia.GioiTinh == "Nữ") rdoNu.Checked = true;
            else if (docGia.GioiTinh == "Khác") rdoKhac.Checked = true;
            else rdoNam.Checked = true;

            txtSoDienThoai.Text = docGia.SoDienThoai ?? string.Empty;
            txtEmail.Text = docGia.Email ?? string.Empty;
            txtDiaChi.Text = docGia.DiaChi ?? string.Empty;
            _anhDaiDien = docGia.AnhDaiDien;

            lblNgayDangKy.Text = docGia.NgayDangKy.ToString("dd/MM/yyyy");
            dtpNgayDangKy.Value = docGia.NgayDangKy.ToDateTime(TimeOnly.MinValue);

            lblCapNhatCuoi.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            lblNguoiCapNhat.Text = CurrentUser.HoTen ?? "Hệ thống";

            // Loại độc giả
            if (cboLoaiDocGia.Items.Contains(docGia.LoaiDocGia))
                cboLoaiDocGia.SelectedItem = docGia.LoaiDocGia;
            else
                cboLoaiDocGia.SelectedIndex = 0;

            // Khoa & Lớp: nạp tuần tự để không mất lớp hiện tại do sự kiện async.
            if (docGia.MaLopNavigation != null)
            {
                cboKhoa.SelectedValue = docGia.MaLopNavigation.MaKhoa;
                await NapDanhSachLopAsync(docGia.MaLopNavigation.MaKhoa);
                cboLop.SelectedValue = docGia.MaLop;
            }

            // Trạng thái độc giả
            cboTrangThaiDocGia.SelectedItem = docGia.TrangThai ? "Hoạt động" : "Ngừng hoạt động";

            // Thẻ độc giả
            if (the != null)
            {
                txtMaThe.Text = string.IsNullOrWhiteSpace(the.MaTheHienThi) ? $"TDG{the.MaThe:D6}" : the.MaTheHienThi;
                dtpNgayCapThe.Value = the.NgayCap.ToDateTime(TimeOnly.MinValue);
                dtpNgayHetHan.Value = the.NgayHetHan.ToDateTime(TimeOnly.MinValue);

                string ttThe = the.TrangThai == "Bị khóa" ? "Bị khóa" : "Còn hiệu lực";
                cboTrangThaiThe.SelectedItem = ttThe;
                txtGhiChu.Text = the.GhiChu ?? string.Empty;
            }
            else
            {
                txtMaThe.Text = "Chưa có";
                dtpNgayCapThe.Value = DateTime.Today;
                dtpNgayHetHan.Value = DateTime.Today.AddYears(1);
                cboTrangThaiThe.SelectedItem = "Còn hiệu lực";
                txtGhiChu.Text = string.Empty;
            }

            string trangThaiHeader = XacDinhTrangThaiTheText(docGia, the, dongPhis);
            DatBadgeHeader(btnTrangThaiHeader, trangThaiHeader);

            Image? oldImage = picDocGia.Image;
            picDocGia.Image = DatabaseImageHelper.LoadReaderAvatar(docGia.AnhDaiDien, docGia.HoTen, picDocGia.Width, picDocGia.Height);
            if (oldImage != null && !ReferenceEquals(oldImage, picDocGia.Image)) oldImage.Dispose();
        }

        private static string XacDinhTrangThaiTheText(
            DocGium docGia,
            TheDocGium? the,
            List<DongPhiThuongNien> dongPhis)
        {
            if (!docGia.TrangThai) return "Bị khóa";
            if (the == null) return "Chưa cấp thẻ";
            if (the.TrangThai == "Bị khóa") return "Bị khóa";

            DateOnly homNay = DateOnly.FromDateTime(DateTime.Today);
            if (the.NgayHetHan < homNay) return "Hết hạn";

            bool daDongPhi = dongPhis.Any(x => x.Nam == DateTime.Today.Year && x.TrangThai.Contains("Đã"));
            if (!daDongPhi) return "Chưa đóng lệ phí";
            if (the.NgayHetHan <= homNay.AddDays(30)) return "Sắp hết hạn";

            return "Còn hiệu lực";
        }

        private static void DatBadgeHeader(Guna.UI2.WinForms.Guna2Button button, string trangThai)
        {
            (Color fill, Color fore) = trangThai switch
            {
                "Còn hiệu lực" => (Color.FromArgb(209, 250, 223), Color.FromArgb(2, 122, 72)),
                "Sắp hết hạn" => (Color.FromArgb(254, 240, 199), Color.FromArgb(180, 83, 9)),
                "Hết hạn" => (Color.FromArgb(254, 228, 226), Color.FromArgb(180, 35, 24)),
                "Chưa đóng lệ phí" => (Color.FromArgb(255, 237, 213), Color.FromArgb(194, 65, 12)),
                _ => (Color.FromArgb(242, 244, 248), Color.FromArgb(70, 80, 95))
            };

            button.Text = trangThai;
            button.DisabledState.FillColor = fill;
            button.DisabledState.ForeColor = fore;
            button.FillColor = fill;
            button.ForeColor = fore;
        }

        private void GanSachDangMuon(List<PhieuMuon> phieuMuons)
        {
            DateOnly homNay = DateOnly.FromDateTime(DateTime.Today);

            var list = phieuMuons
                .SelectMany(pm => pm.ChiTietMuons.Select(ct => new { pm, ct }))
                .Where(x => x.ct.ChiTietTra == null && !x.ct.TrangThai.Contains("Đã trả"))
                .OrderBy(x => x.pm.HanTra)
                .Select((x, idx) => new
                {
                    STT = idx + 1,
                    TenSach = x.ct.MaCuonSachNavigation?.MaSachNavigation?.TenSach ?? "Sách chưa xác định",
                    MaCuon = x.ct.MaCuonSachNavigation?.MaCuonSachHienThi ?? $"CS{x.ct.MaCuonSach:D6}",
                    NgayMuon = x.pm.NgayMuon.ToString("dd/MM/yyyy"),
                    HanTra = x.pm.HanTra.ToString("dd/MM/yyyy"),
                    TrangThai = x.pm.HanTra < homNay
                        ? "Quá hạn"
                        : x.pm.HanTra <= homNay.AddDays(3)
                            ? "Sắp hết hạn"
                            : "Đang mượn"
                })
                .ToList();

            dgvSachDangMuon.AutoGenerateColumns = false;
            colSTT.DataPropertyName = "STT";
            colTenSach.DataPropertyName = "TenSach";
            colMaCuon.DataPropertyName = "MaCuon";
            colNgayMuon.DataPropertyName = "NgayMuon";
            colHanTra.DataPropertyName = "HanTra";
            colTrangThaiSach.DataPropertyName = "TrangThai";

            dgvSachDangMuon.DataSource = list;
            lblDemSachDangMuon.Text = $"({list.Count})";
        }

        private void GanLichSuMuon(List<PhieuMuon> phieuMuons)
        {
            var list = phieuMuons
                .Take(5)
                .Select(pm => new
                {
                    NgayMuon = pm.NgayMuon.ToString("dd/MM/yyyy"),
                    NgayTra = pm.ChiTietMuons.Select(ct => ct.ChiTietTra?.MaPhieuTraNavigation?.NgayTra)
                                             .FirstOrDefault()?.ToString("dd/MM/yyyy") ?? "-",
                    SoSach = pm.ChiTietMuons.Count,
                    TinhTrang = pm.ChiTietMuons.All(ct => ct.ChiTietTra != null) ? "Đã trả" : "Đang mượn"
                })
                .ToList();

            dgvLichSuMuon.AutoGenerateColumns = false;
            colLsNgayMuon.DataPropertyName = "NgayMuon";
            colLsNgayTra.DataPropertyName = "NgayTra";
            colLsSoSach.DataPropertyName = "SoSach";
            colLsTinhTrang.DataPropertyName = "TinhTrang";

            dgvLichSuMuon.DataSource = list;
        }

        private void GanLePhi(List<DongPhiThuongNien> dongPhis)
        {
            int nam = DateTime.Today.Year;
            DongPhiThuongNien? currentFee = dongPhis.FirstOrDefault(p => p.Nam == nam) ?? dongPhis.FirstOrDefault();

            if (currentFee != null)
            {
                cboNamLePhi.SelectedItem = currentFee.Nam.ToString();
                lblSoTienLePhi.Text = $"{currentFee.SoTien:N0} đ";
                lblNgayDongLePhi.Text = currentFee.NgayDong.ToString("dd/MM/yyyy");

                bool daThanhToan = currentFee.TrangThai.Contains("Đã");
                lblTrangThaiLePhi.Text = daThanhToan ? "Đã đóng" : "Chưa đóng";
                lblTrangThaiLePhi.ForeColor = daThanhToan ? Color.FromArgb(2, 122, 72) : Color.FromArgb(215, 45, 45);
            }
            else
            {
                lblSoTienLePhi.Text = "100.000 đ";
                lblNgayDongLePhi.Text = "-";
                lblTrangThaiLePhi.Text = "Chưa đóng";
                lblTrangThaiLePhi.ForeColor = Color.FromArgb(215, 45, 45);
            }
        }

        private void GanTienPhat(List<PhieuPhat> phieuPhats)
        {
            decimal tongPhat = phieuPhats.Sum(p => p.TongTien);
            decimal daThanhToan = phieuPhats.Where(p => p.TrangThai == "Đã thanh toán").Sum(p => p.TongTien);
            decimal conChuaThanhToan = tongPhat - daThanhToan;

            lblTongTienPhat.Text = $"{tongPhat:N0} đ";
            lblDaThanhToanPhat.Text = $"{daThanhToan:N0} đ";
            lblConChuaThanhToanPhat.Text = $"{conChuaThanhToan:N0} đ";
        }

        private void HienThiDuLieuRong()
        {
            lblMaDocGiaHeader.Text = "DG0000";
            txtMaDocGia.Text = "DG0000";
            txtHoTen.Text = string.Empty;
            txtMaSinhVien.Text = string.Empty;
            dtpNgaySinh.Value = DateTime.Today.AddYears(-20);
            rdoNam.Checked = true;
            txtSoDienThoai.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtDiaChi.Text = string.Empty;
            dtpNgayDangKy.Value = DateTime.Today;
            lblNgayDangKy.Text = DateTime.Today.ToString("dd/MM/yyyy");
            lblCapNhatCuoi.Text = "-";
            lblNguoiCapNhat.Text = CurrentUser.HoTen ?? "Hệ thống";

            txtMaThe.Text = "TDG000000";
            dtpNgayCapThe.Value = DateTime.Today;
            dtpNgayHetHan.Value = DateTime.Today.AddYears(1);
            cboTrangThaiThe.SelectedIndex = 0;
            txtGhiChu.Text = string.Empty;

            dgvSachDangMuon.DataSource = null;
            dgvLichSuMuon.DataSource = null;
        }

        private void DgvSachDangMuon_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvSachDangMuon.Columns[e.ColumnIndex].Name == "colTrangThaiSach")
            {
                string status = e.Value?.ToString() ?? string.Empty;
                if (status == "Sắp hết hạn")
                {
                    e.CellStyle.ForeColor = Color.FromArgb(180, 83, 9);
                    e.CellStyle.BackColor = Color.FromArgb(254, 240, 199);
                }
                else if (status == "Quá hạn")
                {
                    e.CellStyle.ForeColor = Color.FromArgb(180, 35, 24);
                    e.CellStyle.BackColor = Color.FromArgb(254, 228, 226);
                }
                else if (status == "Đang mượn")
                {
                    e.CellStyle.ForeColor = Color.FromArgb(2, 122, 72);
                    e.CellStyle.BackColor = Color.FromArgb(209, 250, 223);
                }
            }
        }

        private void DgvLichSuMuon_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvLichSuMuon.Columns[e.ColumnIndex].Name == "colLsTinhTrang")
            {
                string status = e.Value?.ToString() ?? string.Empty;
                if (status == "Đã trả")
                {
                    e.CellStyle.ForeColor = Color.FromArgb(2, 122, 72);
                    e.CellStyle.BackColor = Color.FromArgb(209, 250, 223);
                }
                else
                {
                    e.CellStyle.ForeColor = Color.FromArgb(180, 83, 9);
                    e.CellStyle.BackColor = Color.FromArgb(254, 240, 199);
                }
            }
        }

        private void BtnChonAnh_Click(object? sender, EventArgs e)
        {
            using OpenFileDialog ofd = new()
            {
                Title = "Chọn ảnh đại diện độc giả",
                Filter = "Tệp hình ảnh (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png"
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using Image source = Image.FromFile(ofd.FileName);
                    Image? oldImage = picDocGia.Image;
                    picDocGia.Image = new Bitmap(source);
                    oldImage?.Dispose();
                    _anhDaiDien = DatabaseImageHelper.CopyToProjectImages(
                        ofd.FileName,
                        "Avatars/Readers",
                        $"docgia_{_maDocGia:D3}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Không thể mở tệp ảnh: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnLuu_Click(object? sender, EventArgs e)
        {
            string hoTen = txtHoTen.Text.Trim();
            if (string.IsNullOrWhiteSpace(hoTen))
            {
                MessageBox.Show("Vui lòng nhập họ và tên độc giả.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtHoTen.Focus();
                return;
            }
            if (hoTen.Length > 50)
            {
                MessageBox.Show("Họ và tên độc giả không được vượt quá 50 ký tự.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtHoTen.Focus();
                return;
            }


            string loaiDocGia = cboLoaiDocGia.SelectedItem?.ToString() ?? "Sinh viên";
            int? maLop = null;
            if (loaiDocGia == "Sinh viên")
            {
                if (cboLop.SelectedValue is int idLop && idLop > 0)
                {
                    maLop = idLop;
                }
                else
                {
                    MessageBox.Show("Vui lòng chọn lớp học cho độc giả sinh viên.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cboLop.Focus();
                    return;
                }
            }

            string maSinhVien = txtMaSinhVien.Text.Trim();
            if (loaiDocGia == "Sinh viên" && string.IsNullOrWhiteSpace(maSinhVien))
            {
                MessageBox.Show("Vui lòng nhập mã sinh viên.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaSinhVien.Focus();
                return;
            }
            if (!string.IsNullOrEmpty(maSinhVien) && _docGiaService.TonTaiMaSinhVienKhac(maSinhVien, _maDocGia))
            {
                MessageBox.Show("Mã sinh viên này đã tồn tại ở một độc giả khác.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaSinhVien.Focus();
                return;
            }

            if (dtpNgaySinh.Value.Date >= DateTime.Today)
            {
                MessageBox.Show("Ngày sinh phải nhỏ hơn ngày hiện tại.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpNgaySinh.Focus();
                return;
            }

            string email = txtEmail.Text.Trim();
            if (!string.IsNullOrEmpty(email) && !System.Net.Mail.MailAddress.TryCreate(email, out _))
            {
                MessageBox.Show("Email không đúng định dạng.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return;
            }
            if (!string.IsNullOrEmpty(email) && _docGiaService.TonTaiEmailKhac(email, _maDocGia))
            {
                MessageBox.Show("Email này đã được sử dụng bởi độc giả khác.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return;
            }

            string soDienThoai = System.Text.RegularExpressions.Regex.Replace(txtSoDienThoai.Text, @"\D", string.Empty);
            if (soDienThoai.Length != 10 || !soDienThoai.StartsWith("0"))
            {
                MessageBox.Show("Số điện thoại phải gồm đúng 10 chữ số và bắt đầu bằng 0.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoDienThoai.Focus();
                return;
            }
            if (_docGiaService.TonTaiSoDienThoaiKhac(soDienThoai, _maDocGia))
            {
                MessageBox.Show("Số điện thoại này đã được sử dụng bởi độc giả khác.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoDienThoai.Focus();
                return;
            }

            bool trangThaiDocGia = cboTrangThaiDocGia.SelectedItem?.ToString() == "Hoạt động";

            CapNhatDocGiaInputModel inputModel = new()
            {
                MaDocGia = _maDocGia,
                HoTen = hoTen,
                NgaySinh = DateOnly.FromDateTime(dtpNgaySinh.Value),
                GioiTinh = rdoNu.Checked ? "Nữ" : rdoKhac.Checked ? "Khác" : "Nam",
                SoDienThoai = soDienThoai,
                Email = email,
                DiaChi = txtDiaChi.Text.Trim(),
                AnhDaiDien = _anhDaiDien,
                LoaiDocGia = loaiDocGia,
                MaSinhVien = string.IsNullOrEmpty(maSinhVien) ? null : maSinhVien,
                MaLop = maLop,
                NgayDangKy = DateOnly.FromDateTime(dtpNgayDangKy.Value),
                TrangThaiDocGia = trangThaiDocGia
            };

            try
            {
                btnLuu.Enabled = false;
                _docGiaService.CapNhatDocGia(inputModel);
                MessageBox.Show(
                    "Cập nhật thông tin độc giả thành công!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể lưu thay đổi: " + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                btnLuu.Enabled = true;
            }
        }

        private void BtnHuy_Click(object? sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void MoLichSuMuon()
        {
            using FrmLichSuMuonSachGanDay frm = new();
            frm.ShowDialog(this);
        }

    }
}

