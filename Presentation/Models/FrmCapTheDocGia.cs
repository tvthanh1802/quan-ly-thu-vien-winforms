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
    public partial class FrmCapTheDocGia : Form
    {
        private readonly int _maDocGia;
        private readonly DocGiaService _docGiaService;
        private bool _dangTai;

        public FrmCapTheDocGia()
        {
            InitializeComponent();
            _maDocGia = 0;
            _docGiaService = new DocGiaService();
            KhoiTaoForm();
        }

        public FrmCapTheDocGia(int maDocGia)
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

            Load += FrmCapTheDocGia_Load;
            btnCapThe.Click += BtnCapThe_Click;
            btnHuy.Click += BtnHuy_Click;
            btnCloseBox.Click += BtnHuy_Click;
            btnTuDong.Click += BtnTuDong_Click;

            dtpNgayCapThe.ValueChanged += DtpNgayCapThe_ValueChanged;
            cboThoiHanThe.SelectedIndexChanged += CboThoiHanThe_SelectedIndexChanged;
            txtMaThe.TextChanged += TxtMaThe_TextChanged;
            chkThuLePhiNgay.CheckedChanged += ChkThuLePhiNgay_CheckedChanged;

            txtGhiChuThe.TextChanged += TxtGhiChuThe_TextChanged;
            txtGhiChuLePhi.TextChanged += TxtGhiChuLePhi_TextChanged;

            picCardBarcode.Paint += PicCardBarcode_Paint;
        }

        private async void FrmCapTheDocGia_Load(object? sender, EventArgs e)
        {
            try
            {
                NapDanhSachTrangThaiThe();
                NapDanhSachThoiHanThe();
                NapDanhSachNamDongPhi();
                NapDanhSachHinhThucThu();

                dtpNgayCapThe.Value = DateTime.Today;
                dtpNgayDongPhi.Value = DateTime.Today;

                if (_maDocGia > 0)
                {
                    await TaiDuLieuDocGiaAsync();
                }
                else
                {
                    HienThiDuLieuMacDinh();
                }

                TinhNgayHetHan();
                CapNhatCardPreview();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể khởi tạo form cấp thẻ: " + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
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

        private void NapDanhSachThoiHanThe()
        {
            cboThoiHanThe.Items.Clear();
            cboThoiHanThe.Items.AddRange(new object[]
            {
                "6 tháng",
                "12 tháng",
                "24 tháng",
                "36 tháng"
            });
            cboThoiHanThe.SelectedIndex = 1; // Mặc định 12 tháng
        }

        private void NapDanhSachNamDongPhi()
        {
            cboNamDongPhi.Items.Clear();
            int namHienTai = DateTime.Today.Year;
            for (int nam = namHienTai - 2; nam <= namHienTai + 2; nam++)
            {
                cboNamDongPhi.Items.Add(nam.ToString());
            }
            cboNamDongPhi.SelectedItem = namHienTai.ToString();
        }

        private void NapDanhSachHinhThucThu()
        {
            cboHinhThucThu.Items.Clear();
            cboHinhThucThu.Items.AddRange(new object[]
            {
                "Tiền mặt",
                "Chuyển khoản",
                "Ví điện tử"
            });
            cboHinhThucThu.SelectedIndex = 0;
        }

        private void DtpNgayCapThe_ValueChanged(object? sender, EventArgs e)
        {
            TinhNgayHetHan();
            CapNhatCardPreview();
        }

        private void CboThoiHanThe_SelectedIndexChanged(object? sender, EventArgs e)
        {
            TinhNgayHetHan();
        }

        private void TxtMaThe_TextChanged(object? sender, EventArgs e)
        {
            CapNhatCardPreview();
        }

        private void ChkThuLePhiNgay_CheckedChanged(object? sender, EventArgs e)
        {
            pnlFeeInputs.Enabled = chkThuLePhiNgay.Checked;
        }

        private void TxtGhiChuThe_TextChanged(object? sender, EventArgs e)
        {
            int len = txtGhiChuThe.Text.Length;
            lblDemGhiChuThe.Text = $"{len}/255";
            lblDemGhiChuThe.ForeColor = len > 255 ? Color.Red : Color.Gray;
        }

        private void TxtGhiChuLePhi_TextChanged(object? sender, EventArgs e)
        {
            int len = txtGhiChuLePhi.Text.Length;
            lblDemGhiChuLePhi.Text = $"{len}/255";
            lblDemGhiChuLePhi.ForeColor = len > 255 ? Color.Red : Color.Gray;
        }

        private void TinhNgayHetHan()
        {
            int thang = cboThoiHanThe.SelectedItem?.ToString() switch
            {
                "6 tháng" => 6,
                "24 tháng" => 24,
                "36 tháng" => 36,
                _ => 12
            };

            DateTime ngayCap = dtpNgayCapThe.Value;
            DateTime ngayHetHan = ngayCap.AddMonths(thang);
            dtpNgayHetHan.Value = ngayHetHan;

            lblThongBaoNgayHetHanText.Text = $"Thẻ sẽ hết hạn vào: {ngayHetHan:dd/MM/yyyy}";
            CapNhatCardPreview();
        }

        private void CapNhatCardPreview()
        {
            lblCardMaThe.Text = string.IsNullOrWhiteSpace(txtMaThe.Text) ? "TDG000022" : txtMaThe.Text.Trim();
            lblCardHoTen.Text = string.IsNullOrWhiteSpace(lblHoTen.Text) ? "Nguyễn Thị Lan" : lblHoTen.Text;
            lblCardLoai.Text = string.IsNullOrWhiteSpace(lblLoaiDocGia.Text) ? "Sinh viên" : lblLoaiDocGia.Text;
            lblCardNgayCap.Text = dtpNgayCapThe.Value.ToString("dd/MM/yyyy");
            lblCardNgayHetHan.Text = dtpNgayHetHan.Value.ToString("dd/MM/yyyy");
            picCardBarcode.Invalidate();
        }

        private void BtnTuDong_Click(object? sender, EventArgs e)
        {
            try
            {
                txtMaThe.Text = _docGiaService.GetNextMaTheHienThi();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể tạo mã thẻ: " + ex.Message, "Lỗi dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

                // Thẻ độc giả
                if (docGia.TheDocGium != null)
                {
                    MessageBox.Show("Độc giả đã có thẻ. Vui lòng dùng chức năng gia hạn hoặc cấp lại thẻ.",
                        "Không thể cấp thẻ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnCapThe.Enabled = false;
                    txtMaThe.Text = string.IsNullOrWhiteSpace(docGia.TheDocGium.MaTheHienThi)
                        ? $"TDG{docGia.TheDocGium.MaThe:D6}"
                        : docGia.TheDocGium.MaTheHienThi;
                    btnTrangThaiDocGia.Text = docGia.TheDocGium.TrangThai;
                }
                else
                {
                    txtMaThe.Text = _docGiaService.GetNextMaTheHienThi();
                    btnTrangThaiDocGia.Text = "Chưa cấp";
                    btnTrangThaiDocGia.FillColor = Color.FromArgb(254, 240, 199);
                    btnTrangThaiDocGia.ForeColor = Color.FromArgb(180, 83, 9);
                }

                Image avatarImg = DatabaseImageHelper.LoadReaderAvatar(docGia.AnhDaiDien, docGia.HoTen, 130, 130);
                picDocGia.Image = avatarImg;
                picCardAvatar.Image = DatabaseImageHelper.LoadReaderAvatar(docGia.AnhDaiDien, docGia.HoTen, 95, 115);
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

            txtMaThe.Text = "TDG000022";
            picDocGia.Image = DatabaseImageHelper.LoadReaderAvatar(null, "Nguyễn Thị Lan", 130, 130);
            picCardAvatar.Image = DatabaseImageHelper.LoadReaderAvatar(null, "Nguyễn Thị Lan", 95, 115);
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

        private void BtnCapThe_Click(object? sender, EventArgs e)
        {
            string maThe = txtMaThe.Text.Trim();
            if (string.IsNullOrWhiteSpace(maThe))
            {
                MessageBox.Show("Vui lòng nhập hoặc tạo mã thẻ.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaThe.Focus();
                return;
            }

            DateOnly ngayCap = DateOnly.FromDateTime(dtpNgayCapThe.Value);
            DateOnly ngayHetHan = DateOnly.FromDateTime(dtpNgayHetHan.Value);

            if (ngayHetHan <= ngayCap)
            {
                MessageBox.Show("Ngày hết hạn phải sau ngày cấp thẻ.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpNgayHetHan.Focus();
                return;
            }

            decimal soTienLePhi = 0;
            if (chkThuLePhiNgay.Checked)
            {
                string textMoney = txtSoTienLePhi.Text.Replace(".", "").Replace(",", "").Trim();
                if (!decimal.TryParse(textMoney, out soTienLePhi) || soTienLePhi < 0)
                {
                    MessageBox.Show("Số tiền lệ phí không hợp lệ.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtSoTienLePhi.Focus();
                    return;
                }
            }

            int namDongPhi = int.TryParse(cboNamDongPhi.SelectedItem?.ToString(), out int y) ? y : DateTime.Today.Year;
            string hinhThucThu = cboHinhThucThu.SelectedItem?.ToString() ?? "Tiền mặt";
            string trangThaiThe = cboTrangThaiThe.SelectedItem?.ToString() ?? "Còn hiệu lực";

            CapTheMoiInputModel model = new()
            {
                MaDocGia = _maDocGia,
                MaTheHienThi = maThe,
                NgayCap = ngayCap,
                NgayHetHan = ngayHetHan,
                TrangThaiThe = trangThaiThe,
                GhiChuThe = txtGhiChuThe.Text.Trim(),

                ThuLePhiNgay = chkThuLePhiNgay.Checked,
                NamDongPhi = namDongPhi,
                SoTienLePhi = soTienLePhi,
                NgayDongPhi = DateOnly.FromDateTime(dtpNgayDongPhi.Value),
                HinhThucThu = hinhThucThu,
                GhiChuLePhi = txtGhiChuLePhi.Text.Trim()
            };

            try
            {
                btnCapThe.Enabled = false;
                _docGiaService.CapTheMoi(model, CurrentUser.MaNhanVien);
                MessageBox.Show(
                    $"Cấp thẻ {maThe} cho độc giả thành công!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể cấp thẻ: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnCapThe.Enabled = true;
            }
        }

        private void BtnHuy_Click(object? sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

    }
}

