using BusinessLayer.Services;
using DataLayer.Context;
using DataLayer.Entities;
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
    public partial class FrmChiTietPhieuMuon : Form
    {
        private int _maPhieuMuon;
        private readonly MuonTraService _muonTraService;

        public FrmChiTietPhieuMuon()
        {
            InitializeComponent();
            _maPhieuMuon = 0;
            _muonTraService = new MuonTraService();
            KhoiTaoForm();
        }

        public FrmChiTietPhieuMuon(int maPhieuMuon)
        {
            InitializeComponent();
            _maPhieuMuon = maPhieuMuon;
            _muonTraService = new MuonTraService();
            KhoiTaoForm();
        }

        private void KhoiTaoForm()
        {
            if (DesignModeHelper.IsDesignMode(this)) return;

            KeyPreview = true;
            StartPosition = FormStartPosition.CenterScreen;

            Load += FrmChiTietPhieuMuon_Load;

            btnDong.Click += (s, e) => Close();
            btnCloseBox.Click += (s, e) => Close();
        }

        private async void FrmChiTietPhieuMuon_Load(object? sender, EventArgs e)
        {
            try
            {
                if (_maPhieuMuon > 0)
                {
                    await TaiDuLieuPhieuMuonAsync(_maPhieuMuon);
                }
                else
                {
                    HienThiTrong();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể tải chi tiết phiếu mượn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task TaiDuLieuPhieuMuonAsync(int maPhieuMuon)
        {
            try
            {
                await using AppDbContext context = new();

                PhieuMuon? pm = await context.PhieuMuons
                    .AsNoTracking()
                    .Include(x => x.MaTheNavigation)
                        .ThenInclude(t => t.MaDocGiaNavigation)
                            .ThenInclude(dg => dg.MaLopNavigation)
                    .Include(x => x.MaNhanVienNavigation)
                    .Include(x => x.ChiTietMuons)
                        .ThenInclude(ct => ct.MaCuonSachNavigation)
                            .ThenInclude(cs => cs.MaSachNavigation)
                                .ThenInclude(s => s.MaTheLoaiNavigation)
                    .Include(x => x.ChiTietMuons)
                        .ThenInclude(ct => ct.MaCuonSachNavigation)
                            .ThenInclude(cs => cs.MaSachNavigation)
                                .ThenInclude(s => s.SachTacGia)
                                    .ThenInclude(stg => stg.MaTacGiaNavigation)
                    .Include(x => x.PhieuPhats)
                        .ThenInclude(x => x.ChiTietPhats)
                    .FirstOrDefaultAsync(x => x.MaPhieuMuon == maPhieuMuon);

                if (pm == null)
                {
                    MessageBox.Show("Không tìm thấy phiếu mượn.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                _maPhieuMuon = pm.MaPhieuMuon;
                DocGium docGia = pm.MaTheNavigation.MaDocGiaNavigation;

                // 1. THÔNG TIN PHIẾU MƯỢN
                lblMaPhieuMuon.Text = string.IsNullOrWhiteSpace(pm.MaPhieuMuonHienThi) ? $"PM{pm.MaPhieuMuon:D6}" : pm.MaPhieuMuonHienThi;
                lblNgayLapPhieu.Text = pm.NgayMuon.ToString("dd/MM/yyyy HH:mm");
                lblNhanVienLap.Text = pm.MaNhanVienNavigation?.HoTen ?? "Nguyễn Văn An";

                int tongSoNgay = Math.Max(1, (pm.HanTra.ToDateTime(TimeOnly.MinValue) - pm.NgayMuon.Date).Days);
                lblHanTraDuKien.Text = $"{pm.HanTra:dd/MM/yyyy} ({tongSoNgay} ngày)";

                btnTrangThaiPhieu.Text = pm.TrangThai;
                if (pm.TrangThai == "Quá hạn" || pm.TrangThai == "Vi phạm")
                {
                    btnTrangThaiPhieu.FillColor = Color.FromArgb(254, 226, 226);
                    btnTrangThaiPhieu.ForeColor = Color.FromArgb(220, 38, 38);
                }
                else if (pm.TrangThai == "Đã trả")
                {
                    btnTrangThaiPhieu.FillColor = Color.FromArgb(209, 250, 223);
                    btnTrangThaiPhieu.ForeColor = Color.FromArgb(2, 122, 72);
                }
                else
                {
                    btnTrangThaiPhieu.FillColor = Color.FromArgb(254, 243, 199);
                    btnTrangThaiPhieu.ForeColor = Color.FromArgb(217, 119, 6);
                }

                lblGhiChuPhieu.Text = string.IsNullOrWhiteSpace(pm.GhiChu) ? "-" : pm.GhiChu;

                // 2. THÔNG TIN ĐỘC GIẢ
                lblMaDocGia.Text = $"DG{docGia.MaDocGia:D6}";
                lblHoTen.Text = docGia.HoTen;
                lblNgaySinh.Text = docGia.NgaySinh?.ToString("dd/MM/yyyy") ?? "-";
                lblSdt.Text = string.IsNullOrWhiteSpace(docGia.SoDienThoai) ? "-" : docGia.SoDienThoai;
                lblEmail.Text = string.IsNullOrWhiteSpace(docGia.Email) ? "-" : docGia.Email;
                lblLop.Text = docGia.MaLopNavigation?.TenLop ?? "-";
                lblLoaiThe.Text = "Thẻ " + (docGia.LoaiDocGia?.ToLower() ?? "sinh viên");
                lblDiaChi.Text = string.IsNullOrWhiteSpace(docGia.DiaChi) ? "-" : docGia.DiaChi;

                picDocGia.Image = DatabaseImageHelper.LoadReaderAvatar(docGia.AnhDaiDien, docGia.HoTen, 85, 85);

                // 3. THỐNG KÊ PHIẾU MƯỢN
                int tongHead = pm.ChiTietMuons.Select(x => x.MaCuonSachNavigation.MaSach).Distinct().Count();
                int tongQuantity = pm.ChiTietMuons.Count;
                int daTraCount = pm.ChiTietMuons.Count(x => !DataLayer.Rules.MuonTraRules.LaDangMuon(x.TrangThai));
                int conLaiCount = pm.ChiTietMuons.Count(x => DataLayer.Rules.MuonTraRules.LaDangMuon(x.TrangThai));

                lblTongSoDauSach.Text = tongHead.ToString();
                lblTongSoLuong.Text = $"{tongQuantity} cuốn";
                lblDaTraCount.Text = $"{daTraCount} cuốn";
                lblConLaiCount.Text = $"{conLaiCount} cuốn";

                // 4. DANH SÁCH SÁCH ĐÃ MƯỢN
                dgvSachMuon.Rows.Clear();
                int stt = 1;
                foreach (var ct in pm.ChiTietMuons)
                {
                    var s = ct.MaCuonSachNavigation.MaSachNavigation;
                    string tacGia = string.Join(", ", s.SachTacGia.Select(x => x.MaTacGiaNavigation.TenTacGia));
                    if (string.IsNullOrWhiteSpace(tacGia)) tacGia = "Nhiều tác giả";

                    string theLoai = s.MaTheLoaiNavigation?.TenTheLoai ?? "Khác";

                    dgvSachMuon.Rows.Add(
                        stt++,
                        s.MaSachHienThi ?? $"S{s.MaSach:D6}",
                        s.TenSach,
                        tacGia,
                        theLoai,
                        ct.MaCuonSachNavigation.MaCuonSachHienThi ?? $"C{ct.MaCuonSach:D5}",
                        pm.NgayMuon.ToString("dd/MM/yyyy"),
                        pm.HanTra.ToString("dd/MM/yyyy"),
                        ct.TrangThai,
                        "-"
                    );
                }

                // 6. THÔNG TIN PHẠT
                decimal totalQuaHan = pm.PhieuPhats.SelectMany(x => x.ChiTietPhats).Where(x => x.LoaiPhat == "Trễ hạn").Sum(x => x.SoTien);
                decimal totalHongMat = pm.PhieuPhats.SelectMany(x => x.ChiTietPhats).Where(x => x.LoaiPhat == "Hư hỏng" || x.LoaiPhat == "Mất sách").Sum(x => x.SoTien);
                decimal totalPhat = pm.PhieuPhats.Sum(x => x.TongTien);
                lblTongPhatQuaHan.Text = $"{totalQuaHan:N0} đ";
                lblPhatHongMat.Text = $"{totalHongMat:N0} đ";
                lblTongCongPhat.Text = $"{totalPhat:N0} đ";

                if (totalPhat == 0)
                {
                    lblPhatNoticeText.Text = "Độc giả chưa có phát sinh phí phạt\ntrong phiếu mượn này.";
                    pnlPhatNotice.FillColor = Color.FromArgb(240, 253, 244);
                    pnlPhatNotice.BorderColor = Color.FromArgb(187, 247, 208);
                    iconPhatInfo.IconColor = Color.FromArgb(22, 163, 74);
                    lblPhatNoticeText.ForeColor = Color.FromArgb(22, 101, 52);
                }
                else
                {
                    lblPhatNoticeText.Text = $"Phiếu mượn này phát sinh tổng tiền phạt là {totalPhat:N0} VNĐ.";
                    pnlPhatNotice.FillColor = Color.FromArgb(254, 242, 242);
                    pnlPhatNotice.BorderColor = Color.FromArgb(254, 202, 202);
                    iconPhatInfo.IconColor = Color.FromArgb(220, 38, 38);
                    lblPhatNoticeText.ForeColor = Color.FromArgb(153, 27, 27);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải chi tiết phiếu mượn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void HienThiTrong()
        {
            _maPhieuMuon = 0;

            lblMaPhieuMuon.Text = "-";
            lblNgayLapPhieu.Text = "-";
            lblNhanVienLap.Text = "-";
            lblHanTraDuKien.Text = "-";

            btnTrangThaiPhieu.Text = "-";
            btnTrangThaiPhieu.FillColor = Color.FromArgb(241, 245, 249);
            btnTrangThaiPhieu.ForeColor = Color.FromArgb(100, 116, 139);
            lblGhiChuPhieu.Text = "-";

            lblMaDocGia.Text = "-";
            lblHoTen.Text = "-";
            lblNgaySinh.Text = "-";
            lblSdt.Text = "-";
            lblEmail.Text = "-";
            lblLop.Text = "-";
            lblLoaiThe.Text = "-";
            lblDiaChi.Text = "-";

            picDocGia.Image?.Dispose();
            picDocGia.Image = DatabaseImageHelper.LoadReaderAvatar(null, "?", 85, 85);

            lblTongSoDauSach.Text = "0";
            lblTongSoLuong.Text = "0 cuốn";
            lblDaTraCount.Text = "0 cuốn";
            lblConLaiCount.Text = "0 cuốn";

            dgvSachMuon.Rows.Clear();

            lblTongPhatQuaHan.Text = "0 đ";
            lblPhatHongMat.Text = "0 đ";
            lblTongCongPhat.Text = "0 đ";
        }



        private void pnlThongTinPhat_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}

