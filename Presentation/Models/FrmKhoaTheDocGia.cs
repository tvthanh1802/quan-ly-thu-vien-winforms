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
    public partial class FrmKhoaTheDocGia : Form
    {
        private readonly int _maDocGia;
        private readonly DocGiaService _docGiaService;
        private bool _dangTai;

        public FrmKhoaTheDocGia()
        {
            InitializeComponent();
            _maDocGia = 0;
            _docGiaService = new DocGiaService();
            KhoiTaoForm();
        }

        public FrmKhoaTheDocGia(int maDocGia)
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

            Load += FrmKhoaTheDocGia_Load;
            btnKhoaThe.Click += BtnKhoaThe_Click;
            btnHuy.Click += BtnHuy_Click;
            btnCloseBox.Click += BtnHuy_Click;

            dtpNgayKhoa.ValueChanged += DtpNgayKhoa_ValueChanged;
            cboThoiHanKhoa.SelectedIndexChanged += CboThoiHanKhoa_SelectedIndexChanged;
            cboLyDoKhoa.SelectedIndexChanged += CboLyDoKhoa_SelectedIndexChanged;
            txtGhiChu.TextChanged += TxtGhiChu_TextChanged;

            picCardBarcode.Paint += PicCardBarcode_Paint;
        }

        private async void FrmKhoaTheDocGia_Load(object? sender, EventArgs e)
        {
            try
            {
                NapDanhSachLyDoKhoa();
                NapDanhSachThoiHanKhoa();

                dtpNgayKhoa.Value = DateTime.Today;

                if (_maDocGia > 0)
                {
                    await TaiDuLieuDocGiaAsync();
                }
                else
                {
                    HienThiDuLieuMacDinh();
                }

                TinhNgayMoKhoa();
                CapNhatLockBadgePreview();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể khởi tạo form khóa thẻ: " + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void NapDanhSachLyDoKhoa()
        {
            cboLyDoKhoa.Items.Clear();
            cboLyDoKhoa.Items.AddRange(new object[]
            {
                "Vi phạm quy định thư viện",
                "Trễ hạn trả sách quá nhiều lần",
                "Mất thẻ thư viện",
                "Làm hỏng / mất sách chưa bồi thường",
                "Nghi vấn mượn thẻ người khác",
                "Khác"
            });
            cboLyDoKhoa.SelectedIndex = 0;
        }

        private void NapDanhSachThoiHanKhoa()
        {
            cboThoiHanKhoa.Items.Clear();
            cboThoiHanKhoa.Items.AddRange(new object[]
            {
                "7 ngày",
                "14 ngày",
                "30 ngày",
                "60 ngày",
                "90 ngày",
                "Vĩnh viễn"
            });
            cboThoiHanKhoa.SelectedIndex = 2; // Mặc định 30 ngày
        }

        private void DtpNgayKhoa_ValueChanged(object? sender, EventArgs e)
        {
            TinhNgayMoKhoa();
            CapNhatLockBadgePreview();
        }

        private void CboThoiHanKhoa_SelectedIndexChanged(object? sender, EventArgs e)
        {
            TinhNgayMoKhoa();
            CapNhatLockBadgePreview();
        }

        private void CboLyDoKhoa_SelectedIndexChanged(object? sender, EventArgs e)
        {
            CapNhatLockBadgePreview();
        }

        private void TxtGhiChu_TextChanged(object? sender, EventArgs e)
        {
            int len = txtGhiChu.Text.Length;
            lblDemGhiChu.Text = $"{len}/255";
            lblDemGhiChu.ForeColor = len > 255 ? Color.Red : Color.Gray;
        }

        private void TinhNgayMoKhoa()
        {
            string thoiHan = cboThoiHanKhoa.SelectedItem?.ToString() ?? "30 ngày";
            DateTime ngayKhoa = dtpNgayKhoa.Value;

            if (thoiHan == "Vĩnh viễn")
            {
                dtpNgayMoKhoaDuKien.Enabled = false;
                lblLockDenNgay.Text = "Vĩnh viễn";
            }
            else
            {
                dtpNgayMoKhoaDuKien.Enabled = true;
                int soNgay = thoiHan switch
                {
                    "7 ngày" => 7,
                    "14 ngày" => 14,
                    "60 ngày" => 60,
                    "90 ngày" => 90,
                    _ => 30
                };
                DateTime ngayMoKhoa = ngayKhoa.AddDays(soNgay);
                dtpNgayMoKhoaDuKien.Value = ngayMoKhoa;
                lblLockDenNgay.Text = ngayMoKhoa.ToString("dd/MM/yyyy");
            }

            lblLockTuNgay.Text = ngayKhoa.ToString("dd/MM/yyyy");
        }

        private void CapNhatLockBadgePreview()
        {
            lblLockTuNgay.Text = dtpNgayKhoa.Value.ToString("dd/MM/yyyy");
            string lyDo = cboLyDoKhoa.SelectedItem?.ToString() ?? "Vi phạm quy định thư viện";
            lblLockLyDo.Text = lyDo;
        }

        private async Task TaiDuLieuDocGiaAsync()
        {
            if (_dangTai) return;

            try
            {
                _dangTai = true;
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

                string maDocGiaText = $"DG{docGia.MaDocGia:D4}";
                lblMaDocGiaHeader.Text = maDocGiaText;
                lblHoTen.Text = docGia.HoTen;
                lblMaSinhVien.Text = string.IsNullOrWhiteSpace(docGia.MaSinhVien) ? "-" : docGia.MaSinhVien;
                lblNgaySinh.Text = docGia.NgaySinh?.ToString("dd/MM/yyyy") ?? "-";
                lblGioiTinh.Text = string.IsNullOrWhiteSpace(docGia.GioiTinh) ? "-" : docGia.GioiTinh;
                lblLoaiDocGia.Text = string.IsNullOrWhiteSpace(docGia.LoaiDocGia) ? "-" : docGia.LoaiDocGia;
                lblKhoa.Text = docGia.MaLopNavigation?.MaKhoaNavigation?.TenKhoa ?? "-";
                lblLop.Text = docGia.MaLopNavigation?.TenLop ?? "-";
                lblDienThoai.Text = string.IsNullOrWhiteSpace(docGia.SoDienThoai) ? "-" : docGia.SoDienThoai;
                lblEmail.Text = string.IsNullOrWhiteSpace(docGia.Email) ? "-" : docGia.Email;
                lblDiaChi.Text = string.IsNullOrWhiteSpace(docGia.DiaChi) ? "-" : docGia.DiaChi;

                // Chi tiết thẻ
                if (docGia.TheDocGium != null)
                {
                    string maThe = string.IsNullOrWhiteSpace(docGia.TheDocGium.MaTheHienThi)
                        ? $"TDG{docGia.TheDocGium.MaThe:D6}"
                        : docGia.TheDocGium.MaTheHienThi;

                    lblCardMaThe.Text = maThe;
                    lblCardNgayCap.Text = docGia.TheDocGium.NgayCap.ToString("dd/MM/yyyy");
                    lblCardNgayHetHan.Text = docGia.TheDocGium.NgayHetHan.ToString("dd/MM/yyyy");

                    btnTrangThaiDocGia.Text = docGia.TheDocGium.TrangThai;
                    if (docGia.TheDocGium.TrangThai == "Bị khóa")
                    {
                        btnTrangThaiDocGia.FillColor = Color.FromArgb(254, 226, 226);
                        btnTrangThaiDocGia.ForeColor = Color.FromArgb(220, 38, 38);
                        btnKhoaThe.Enabled = false;
                    }
                    else
                    {
                        btnKhoaThe.Enabled = true;
                    }
                }
                else
                {
                    lblCardMaThe.Text = "Chưa có";
                    btnTrangThaiDocGia.Text = "Chưa cấp";
                    btnTrangThaiDocGia.FillColor = Color.FromArgb(254, 240, 199);
                    btnTrangThaiDocGia.ForeColor = Color.FromArgb(180, 83, 9);
                    btnKhoaThe.Enabled = false;
                }

                lblCardHoTen.Text = docGia.HoTen;
                lblCardLoai.Text = string.IsNullOrWhiteSpace(docGia.LoaiDocGia) ? "Sinh viên" : docGia.LoaiDocGia;

                Image avatarImg = DatabaseImageHelper.LoadReaderAvatar(docGia.AnhDaiDien, docGia.HoTen, 130, 130);
                picDocGia.Image = avatarImg;
                picCardAvatar.Image = DatabaseImageHelper.LoadReaderAvatar(docGia.AnhDaiDien, docGia.HoTen, 100, 125);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể tải thông tin độc giả: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _dangTai = false;
            }
        }

        private void HienThiDuLieuMacDinh()
        {
            lblMaDocGiaHeader.Text = "DG0001";
            lblHoTen.Text = "Nguyễn Thị Lan";
            lblMaSinhVien.Text = "2021210001";
            lblNgaySinh.Text = "12/06/2003";
            lblGioiTinh.Text = "Nữ";
            lblLoaiDocGia.Text = "Sinh viên";
            lblKhoa.Text = "Kế toán";
            lblLop.Text = "KT21A";
            lblDienThoai.Text = "0987 123 456";
            lblEmail.Text = "lannt21@epu.edu.vn";
            lblDiaChi.Text = "123 Đường Láng, Đống Đa, Hà Nội";

            lblCardMaThe.Text = "TDG000001";
            lblCardHoTen.Text = "Nguyễn Thị Lan";
            lblCardLoai.Text = "Sinh viên";
            lblCardNgayCap.Text = "15/09/2024";
            lblCardNgayHetHan.Text = "15/09/2025";

            picDocGia.Image = DatabaseImageHelper.LoadReaderAvatar(null, "Nguyễn Thị Lan", 130, 130);
            picCardAvatar.Image = DatabaseImageHelper.LoadReaderAvatar(null, "Nguyễn Thị Lan", 100, 125);
        }

        private void PicCardBarcode_Paint(object? sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.None;
            int width = picCardBarcode.Width;
            int height = picCardBarcode.Height;

            g.Clear(Color.White);

            using Pen thinPen = new(Color.FromArgb(30, 40, 60), 1.5f);
            using Pen thickPen = new(Color.FromArgb(30, 40, 60), 3f);
            using Pen vThickPen = new(Color.FromArgb(30, 40, 60), 4.5f);

            int x = 20;
            int barHeight = height - 20;

            Random rnd = new(lblCardMaThe.Text.GetHashCode());
            while (x < width - 20)
            {
                int style = rnd.Next(0, 4);
                switch (style)
                {
                    case 0:
                        g.DrawLine(thinPen, x, 5, x, barHeight);
                        x += 3;
                        break;
                    case 1:
                        g.DrawLine(thickPen, x, 5, x, barHeight);
                        x += 5;
                        break;
                    case 2:
                        g.DrawLine(vThickPen, x, 5, x, barHeight);
                        x += 7;
                        break;
                    default:
                        x += 4;
                        break;
                }
            }

            using Font codeFont = new("Courier New", 8.5F, FontStyle.Bold);
            using SolidBrush textBrush = new(Color.FromArgb(30, 40, 60));
            string codeText = lblCardMaThe.Text;
            SizeF textSize = g.MeasureString(codeText, codeFont);
            g.DrawString(codeText, codeFont, textBrush, (width - textSize.Width) / 2f, barHeight + 2);
        }

        private void BtnKhoaThe_Click(object? sender, EventArgs e)
        {
            string thoiHan = cboThoiHanKhoa.SelectedItem?.ToString() ?? "30 ngày";
            DateOnly ngayKhoa = DateOnly.FromDateTime(dtpNgayKhoa.Value);
            DateOnly? ngayMoKhoa = thoiHan == "Vĩnh viễn" ? null : DateOnly.FromDateTime(dtpNgayMoKhoaDuKien.Value);

            KhoaTheInputModel model = new()
            {
                MaDocGia = _maDocGia,
                LyDoKhoa = cboLyDoKhoa.SelectedItem?.ToString() ?? "Vi phạm quy định thư viện",
                NgayKhoa = ngayKhoa,
                ThoiHanKhoa = thoiHan,
                NgayMoKhoaDuKien = ngayMoKhoa,
                GhiChu = txtGhiChu.Text.Trim()
            };

            try
            {
                btnKhoaThe.Enabled = false;
                _docGiaService.KhoaTheDocGia(model);
                MessageBox.Show(
                    $"Đã tạm khóa thẻ độc giả thành công!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể khóa thẻ: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnKhoaThe.Enabled = true;
            }
        }

        private void BtnHuy_Click(object? sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

    }
}

