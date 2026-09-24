using BusinessLayer.Services;
using DataLayer.Context;
using DataLayer.Entities;
using DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using Presentation.Helpers;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentation.Models
{
    public partial class FrmGiaHanThe : Form
    {
        private readonly int _maDocGia;
        private readonly DocGiaService _docGiaService;
        private bool _dangTai;
        private TheDocGium? _theDocGia;
        private decimal _phiThuongNien = 100_000m;

        public FrmGiaHanThe()
        {
            InitializeComponent();
            _maDocGia = 0;
            _docGiaService = new DocGiaService();
            KhoiTaoForm();
        }

        public FrmGiaHanThe(int maDocGia)
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

            Load += FrmGiaHanThe_Load;
            btnGiaHan.Click += BtnGiaHan_Click;
            btnLamMoi.Click += BtnLamMoi_Click;
            btnHuy.Click += BtnHuy_Click;
            btnCloseBox.Click += BtnHuy_Click;

            numSoThangGiaHan.ValueChanged += NumSoThangGiaHan_ValueChanged;
            cboHinhThucThanhToan.SelectedIndexChanged += CboHinhThucThanhToan_SelectedIndexChanged;
            cboLyDoGiaHan.SelectedIndexChanged += CboLyDoGiaHan_SelectedIndexChanged;
        }

        private async void FrmGiaHanThe_Load(object? sender, EventArgs e)
        {
            try
            {
                _dangTai = true;
                cboHinhThucThanhToan.Items.Clear();
                cboHinhThucThanhToan.Items.AddRange(new object[] { "Tiền mặt", "Chuyển khoản", "Thẻ nội bộ" });
                cboHinhThucThanhToan.SelectedIndex = 0;

                cboLyDoGiaHan.Items.Clear();
                cboLyDoGiaHan.Items.AddRange(new object[] { "Gia hạn định kỳ", "Hỗ trợ học tập", "Nghiên cứu khoa học", "Khác" });
                cboLyDoGiaHan.SelectedIndex = 0;

                dtpNgayGiaHan.Value = DateTime.Today;

                if (_maDocGia > 0)
                {
                    await TaiDuLieuDocGiaAsync();
                }
                else
                {
                    HienThiDuLieuMacDinh();
                }

                _phiThuongNien = _docGiaService.GetQuyDinhDocGia().PhiThuongNien;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể khởi tạo form gia hạn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _dangTai = false;
                CapNhatThongTinGiaHan();
            }
        }

        private async Task TaiDuLieuDocGiaAsync()
        {
            try
            {
                await using AppDbContext context = new();

                DocGium? docGia = await context.DocGia
                    .AsNoTracking()
                    .Include(x => x.MaLopNavigation)
                        .ThenInclude(x => x!.MaKhoaNavigation)
                    .Include(x => x.TheDocGium)
                    .FirstOrDefaultAsync(x => x.MaDocGia == _maDocGia);

                if (docGia == null)
                {
                    MessageBox.Show("Không tìm thấy độc giả.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Close();
                    return;
                }

                lblMaDocGia.Text = $"DG{docGia.MaDocGia:D6}";
                lblHoTen.Text = docGia.HoTen;
                lblLop.Text = docGia.MaLopNavigation?.TenLop ?? "-";
                lblKhoaBoMon.Text = docGia.MaLopNavigation?.MaKhoaNavigation?.TenKhoa ?? "-";
                lblDienThoai.Text = string.IsNullOrWhiteSpace(docGia.SoDienThoai) ? "-" : docGia.SoDienThoai;
                lblEmail.Text = string.IsNullOrWhiteSpace(docGia.Email) ? "-" : docGia.Email;
                lblDiaChi.Text = string.IsNullOrWhiteSpace(docGia.DiaChi) ? "-" : docGia.DiaChi;

                picDocGia.Image = DatabaseImageHelper.LoadReaderAvatar(docGia.AnhDaiDien, docGia.HoTen, 120, 120);

                _theDocGia = docGia.TheDocGium;

                if (_theDocGia != null)
                {
                    lblMaThe.Text = string.IsNullOrWhiteSpace(_theDocGia.MaTheHienThi) ? $"TDG{_theDocGia.MaThe:D6}" : _theDocGia.MaTheHienThi;
                    lblNgayCap.Text = _theDocGia.NgayCap.ToString("dd/MM/yyyy");
                    lblHanThe.Text = _theDocGia.NgayHetHan.ToString("dd/MM/yyyy");

                    // Status
                    if (_theDocGia.TrangThai == "Bị khóa")
                    {
                        btnTrangThaiDocGia.Text = "Bị khóa";
                        btnTrangThaiDocGia.FillColor = Color.FromArgb(254, 226, 226);
                        btnTrangThaiDocGia.ForeColor = Color.FromArgb(220, 38, 38);

                        lblTrangThaiThe.Text = "Bị khóa";
                        lblTrangThaiThe.ForeColor = Color.FromArgb(220, 38, 38);
                    }
                    else
                    {
                        btnTrangThaiDocGia.Text = "Đang hoạt động";
                        btnTrangThaiDocGia.FillColor = Color.FromArgb(220, 252, 231);
                        btnTrangThaiDocGia.ForeColor = Color.FromArgb(21, 128, 61);

                        DateTime today = DateTime.Today;
                        DateTime expiredDate = _theDocGia.NgayHetHan.ToDateTime(TimeOnly.MinValue);
                        int daysLeft = (expiredDate - today).Days;

                        if (daysLeft < 0)
                        {
                            lblTrangThaiThe.Text = "Đã hết hạn";
                            lblTrangThaiThe.ForeColor = Color.FromArgb(220, 38, 38);
                            pnlCanhBaoHan.FillColor = Color.FromArgb(254, 226, 226);
                            lblCanhBaoHan.Text = "Thẻ đã hết hạn sử dụng. Cần gia hạn ngay.";
                            lblCanhBaoHan.ForeColor = Color.FromArgb(220, 38, 38);
                        }
                        else if (daysLeft <= 30)
                        {
                            lblTrangThaiThe.Text = "Sắp hết hạn";
                            lblTrangThaiThe.ForeColor = Color.FromArgb(217, 119, 6);
                            pnlCanhBaoHan.FillColor = Color.FromArgb(255, 245, 230);
                            lblCanhBaoHan.Text = $"Thẻ sẽ hết hạn trong {daysLeft} ngày tới";
                            lblCanhBaoHan.ForeColor = Color.FromArgb(217, 119, 6);
                        }
                        else
                        {
                            lblTrangThaiThe.Text = "Đang hoạt động";
                            lblTrangThaiThe.ForeColor = Color.FromArgb(21, 128, 61);
                            pnlCanhBaoHan.FillColor = Color.FromArgb(240, 253, 244);
                            lblCanhBaoHan.Text = "Thẻ của độc giả vẫn đang trong thời hạn sử dụng.";
                            lblCanhBaoHan.ForeColor = Color.FromArgb(21, 128, 61);
                        }
                    }
                }
                else
                {
                    lblMaThe.Text = "Chưa cấp";
                    lblNgayCap.Text = "-";
                    lblHanThe.Text = "-";
                    lblTrangThaiThe.Text = "Chưa cấp";
                    btnTrangThaiDocGia.Text = "Chưa cấp thẻ";
                    btnTrangThaiDocGia.FillColor = Color.FromArgb(254, 243, 199);
                    btnTrangThaiDocGia.ForeColor = Color.FromArgb(217, 119, 6);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể tải dữ liệu độc giả: " + ex.Message, "Lỗi");
            }
        }

        private void HienThiDuLieuMacDinh()
        {
            lblMaDocGia.Text = "DG000125";
            lblHoTen.Text = "Nguyễn Văn An";
            lblLop.Text = "DHTI17A2";
            lblKhoaBoMon.Text = "Công nghệ thông tin";
            lblDienThoai.Text = "0987 654 321";
            lblEmail.Text = "nguyenvanan@epu.edu.vn";
            lblDiaChi.Text = "123 Đường Trần Phú, P. Mộ Lao, Q. Hà Đông, Hà Nội";
            picDocGia.Image = DatabaseImageHelper.LoadReaderAvatar(null, "Nguyễn Văn An", 120, 120);

            lblMaThe.Text = "TDG000125";
            lblNgayCap.Text = "15/09/2024";
            lblHanThe.Text = "15/09/2025";
            lblTrangThaiThe.Text = "Sắp hết hạn";
            lblTrangThaiThe.ForeColor = Color.FromArgb(217, 119, 6);
            btnTrangThaiDocGia.Text = "Đang hoạt động";
            btnTrangThaiDocGia.FillColor = Color.FromArgb(220, 252, 231);
            btnTrangThaiDocGia.ForeColor = Color.FromArgb(21, 128, 61);

            pnlCanhBaoHan.FillColor = Color.FromArgb(255, 245, 230);
            lblCanhBaoHan.Text = "Thẻ sẽ hết hạn trong 7 ngày tới";
            lblCanhBaoHan.ForeColor = Color.FromArgb(217, 119, 6);
        }

        private void CapNhatThongTinGiaHan()
        {
            if (_dangTai) return;

            DateTime ngayGiaHan = dtpNgayGiaHan.Value;
            int soThang = (int)numSoThangGiaHan.Value;

            // Calculate new expiry date
            DateOnly hanCu;
            if (_theDocGia != null)
            {
                hanCu = _theDocGia.NgayHetHan;
            }
            else
            {
                hanCu = DateOnly.FromDateTime(DateTime.Today);
            }

            DateTime baseDate = hanCu.ToDateTime(TimeOnly.MinValue);
            if (baseDate < DateTime.Today)
            {
                baseDate = DateTime.Today;
            }
            DateTime ngayHetHanMoi = baseDate.AddMonths(soThang);
            dtpGiaHanDenNgay.Value = ngayHetHanMoi;

            lblSummaryHanCu.Text = hanCu.ToString("dd/MM/yyyy");
            lblSummaryHanMoi.Text = ngayHetHanMoi.ToString("dd/MM/yyyy");

            int andDays = (ngayHetHanMoi - baseDate).Days;
            lblSummarySoNgayCong.Text = $"{andDays} ngày";

            decimal lePhi = (_phiThuongNien / 12m) * soThang;
            lePhi = Math.Round(lePhi / 1000m) * 1000m;
            txtLePhiGiaHan.Text = $"{lePhi:N0}đ";
            lblSummaryThuPhi.Text = $"{lePhi:N0}đ";
        }

        private void NumSoThangGiaHan_ValueChanged(object? sender, EventArgs e)
        {
            CapNhatThongTinGiaHan();
        }

        private void CboHinhThucThanhToan_SelectedIndexChanged(object? sender, EventArgs e)
        {
        }

        private void CboLyDoGiaHan_SelectedIndexChanged(object? sender, EventArgs e)
        {
        }

        private void BtnLamMoi_Click(object? sender, EventArgs e)
        {
            numSoThangGiaHan.Value = 12;
            dtpNgayGiaHan.Value = DateTime.Today;
            cboHinhThucThanhToan.SelectedIndex = 0;
            cboLyDoGiaHan.SelectedIndex = 0;
            txtGhiChu.Clear();
            CapNhatThongTinGiaHan();
        }

        private void BtnHuy_Click(object? sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void BtnGiaHan_Click(object? sender, EventArgs e)
        {
            if (_theDocGia == null)
            {
                MessageBox.Show("Độc giả này chưa có thẻ để gia hạn.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_theDocGia.TrangThai == "Bị khóa")
            {
                MessageBox.Show("Thẻ đang bị khóa. Không thể gia hạn.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                GiaHanTheInputModel model = new()
                {
                    MaDocGia = _maDocGia,
                    NgayGiaHan = DateOnly.FromDateTime(dtpNgayGiaHan.Value),
                    NgayHetHanMoi = DateOnly.FromDateTime(dtpGiaHanDenNgay.Value),
                    SoThangGiaHan = (int)numSoThangGiaHan.Value,
                    LePhiGiaHan = (_phiThuongNien / 12m) * (int)numSoThangGiaHan.Value,
                    HinhThucThanhToan = cboHinhThucThanhToan.SelectedItem?.ToString() ?? "Tiền mặt",
                    LyDoGiaHan = cboLyDoGiaHan.SelectedItem?.ToString() ?? "Gia hạn định kỳ",
                    GhiChu = txtGhiChu.Text.Trim(),
                    MaNhanVienThu = CurrentUser.MaNhanVien
                };
                _docGiaService.GiaHanThe(model);
                MessageBox.Show("Gia hạn thẻ thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi gia hạn thẻ: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
