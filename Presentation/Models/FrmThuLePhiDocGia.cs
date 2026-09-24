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
    public partial class FrmThuLePhiDocGia : Form
    {
        private int _maDocGia;
        private readonly DocGiaService _docGiaService;
        private bool _dangTai;

        public FrmThuLePhiDocGia()
        {
            InitializeComponent();
            _maDocGia = 0;
            _docGiaService = new DocGiaService();
            KhoiTaoForm();
        }

        public FrmThuLePhiDocGia(int maDocGia)
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

            Load += FrmThuLePhiDocGia_Load;

            btnTraCuu.Click += BtnTraCuu_Click;
            txtTraCuu.KeyDown += TxtTraCuu_KeyDown;

            btnTuDong.Click += BtnTuDong_Click;
            btnXacNhanThu.Click += BtnXacNhanThu_Click;
            btnLamMoi.Click += BtnLamMoi_Click;
            btnInPhieuThu.Click += BtnInPhieuThu_Click;

            txtSoTien.TextChanged += TxtSoTien_TextChanged;
            txtSoTienNhan.TextChanged += TxtSoTienNhan_TextChanged;
            txtGhiChu.TextChanged += TxtGhiChu_TextChanged;

            cboHinhThucThu.SelectedIndexChanged += CboHinhThucThu_SelectedIndexChanged;
            cboLoaiLePhi.SelectedIndexChanged += CboLoaiLePhi_SelectedIndexChanged;
        }

        private async void FrmThuLePhiDocGia_Load(object? sender, EventArgs e)
        {
            try
            {
                NapDanhSachLoaiLePhi();
                NapDanhSachNamDong();
                NapDanhSachHinhThucThu();
                NapDanhSachNguoiThu();

                dtpNgayThu.Value = DateTime.Today;
                txtSoPhieuThu.Text = _docGiaService.GetNextMaPhieuThuHienThi();

                if (_maDocGia > 0)
                {
                    await TaiDuLieuDocGiaAsync(_maDocGia);
                }
                else
                {
                    HienThiChuaChonDocGia();
                }

                TinhTienThua();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể khởi tạo form thu lệ phí: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void NapDanhSachLoaiLePhi()
        {
            cboLoaiLePhi.Items.Clear();
            cboLoaiLePhi.Items.AddRange(new object[]
            {
                "Lệ phí thường niên",
                "Lệ phí cấp lại thẻ",
                "Lệ phí phạt trễ hạn",
                "Khác"
            });
            cboLoaiLePhi.SelectedIndex = 0;
        }

        private void NapDanhSachNamDong()
        {
            cboNamDong.Items.Clear();
            int namHienTai = DateTime.Today.Year;
            for (int nam = namHienTai - 2; nam <= namHienTai + 2; nam++)
            {
                cboNamDong.Items.Add(nam.ToString());
            }
            cboNamDong.SelectedItem = namHienTai.ToString();
        }

        private void ChonNamThuongNienChuaDong(IEnumerable<LichSuDongPhiGridModel> lichSu)
        {
            HashSet<int> namDaDong = lichSu
                .Where(x => string.Equals(x.LoaiLePhi, "Lệ phí thường niên", StringComparison.OrdinalIgnoreCase))
                .Select(x => x.NamDong)
                .ToHashSet();

            int namCanDong = DateTime.Today.Year;
            while (namDaDong.Contains(namCanDong))
            {
                namCanDong++;
            }

            string giaTriNam = namCanDong.ToString();
            if (!cboNamDong.Items.Contains(giaTriNam))
            {
                cboNamDong.Items.Add(giaTriNam);
            }
            cboNamDong.SelectedItem = giaTriNam;
        }

        private void NapDanhSachHinhThucThu()
        {
            cboHinhThucThu.Items.Clear();
            string[] items = new[] { "Tiền mặt", "Chuyển khoản", "Ví điện tử" };
            cboHinhThucThu.Items.AddRange(items);
            cboHinhThucThu.SelectedIndex = 0;

            cboPhuongThucTT.Items.Clear();
            cboPhuongThucTT.Items.AddRange(items);
            cboPhuongThucTT.SelectedIndex = 0;
        }

        private void NapDanhSachNguoiThu()
        {
            cboNguoiThu.Items.Clear();
            string nhanVienHienTai = CurrentUser.HoTen;
            if (string.IsNullOrWhiteSpace(nhanVienHienTai)) nhanVienHienTai = "Nguyễn Văn An";

            cboNguoiThu.Items.Add(nhanVienHienTai);
            cboNguoiThu.Items.Add("Trần Thị Mai");
            cboNguoiThu.Items.Add("Lê Văn Hùng");
            cboNguoiThu.SelectedIndex = 0;
        }

        private void CboHinhThucThu_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (cboHinhThucThu.SelectedIndex >= 0 && cboHinhThucThu.SelectedIndex < cboPhuongThucTT.Items.Count)
            {
                cboPhuongThucTT.SelectedIndex = cboHinhThucThu.SelectedIndex;
            }
        }

        private void CboLoaiLePhi_SelectedIndexChanged(object? sender, EventArgs e)
        {
            switch (cboLoaiLePhi.SelectedItem?.ToString())
            {
                case "Lệ phí cấp lại thẻ":
                    txtSoTien.Text = "50.000";
                    txtSoTienNhan.Text = "50.000";
                    lblFeeAmount.Text = "Mức lệ phí cấp lại thẻ:  50.000 VNĐ / lần";
                    break;
                case "Lệ phí phạt trễ hạn":
                    txtSoTien.Text = "20.000";
                    txtSoTienNhan.Text = "20.000";
                    lblFeeAmount.Text = "Mức lệ phí phạt trễ hạn:  20.000 VNĐ / quyển";
                    break;
                default:
                    txtSoTien.Text = "100.000";
                    txtSoTienNhan.Text = "100.000";
                    lblFeeAmount.Text = "Mức lệ phí thường niên:  100.000 VNĐ / năm";
                    break;
            }
            TinhTienThua();
        }

        private void TxtSoTien_TextChanged(object? sender, EventArgs e)
        {
            TinhTienThua();
        }

        private void TxtSoTienNhan_TextChanged(object? sender, EventArgs e)
        {
            TinhTienThua();
        }

        private void TxtGhiChu_TextChanged(object? sender, EventArgs e)
        {
            int len = txtGhiChu.Text.Length;
            lblDemGhiChu.Text = $"{len}/255";
            lblDemGhiChu.ForeColor = len > 255 ? Color.Red : Color.Gray;
        }

        private void TinhTienThua()
        {
            decimal soTien = ParseMoney(txtSoTien.Text);
            decimal soTienNhan = ParseMoney(txtSoTienNhan.Text);
            decimal tienThua = Math.Max(0, soTienNhan - soTien);

            txtTienThua.Text = $"{tienThua:N0}".Replace(",", ".");
        }

        private static decimal ParseMoney(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return 0;
            string clean = text.Replace(".", "").Replace(",", "").Trim();
            return decimal.TryParse(clean, out decimal val) ? val : 0;
        }

        private void BtnTuDong_Click(object? sender, EventArgs e)
        {
            txtSoPhieuThu.Text = _docGiaService.GetNextMaPhieuThuHienThi();
        }

        private async void TxtTraCuu_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                await TraCuuDocGiaAsync();
            }
        }

        private async void BtnTraCuu_Click(object? sender, EventArgs e)
        {
            await TraCuuDocGiaAsync();
        }

        private async Task TraCuuDocGiaAsync()
        {
            string kw = txtTraCuu.Text.Trim();
            if (string.IsNullOrWhiteSpace(kw))
            {
                MessageBox.Show("Vui lòng nhập Mã độc giả hoặc Số điện thoại để tra cứu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                await using AppDbContext context = new();
                DocGium? dg = await context.DocGia
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.MaDocGia.ToString() == kw ||
                                              ("DG" + x.MaDocGia.ToString("D4")) == kw ||
                                              x.SoDienThoai == kw ||
                                              x.Email == kw);

                if (dg != null)
                {
                    _maDocGia = dg.MaDocGia;
                    await TaiDuLieuDocGiaAsync(_maDocGia);
                }
                else
                {
                    MessageBox.Show("Không tìm thấy độc giả tương ứng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tra cứu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task TaiDuLieuDocGiaAsync(int maDocGia)
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
                    .FirstOrDefaultAsync(x => x.MaDocGia == maDocGia);

                if (docGia == null)
                {
                    MessageBox.Show("Không tìm thấy độc giả.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                _maDocGia = docGia.MaDocGia;
                lblMaDocGiaHeader.Text = $"DG{docGia.MaDocGia:D6}";
                lblHoTen.Text = docGia.HoTen;
                lblNgaySinh.Text = docGia.NgaySinh?.ToString("dd/MM/yyyy") ?? "-";
                lblDienThoai.Text = string.IsNullOrWhiteSpace(docGia.SoDienThoai) ? "-" : docGia.SoDienThoai;
                lblEmail.Text = string.IsNullOrWhiteSpace(docGia.Email) ? "-" : docGia.Email;
                lblDiaChi.Text = string.IsNullOrWhiteSpace(docGia.DiaChi) ? "-" : docGia.DiaChi;
                lblLoaiDocGia.Text = string.IsNullOrWhiteSpace(docGia.LoaiDocGia) ? "-" : docGia.LoaiDocGia;

                string tenKhoa = docGia.MaLopNavigation?.MaKhoaNavigation?.TenKhoa ?? "-";
                string tenLop = docGia.MaLopNavigation?.TenLop ?? "-";
                lblKhoaLop.Text = $"{tenKhoa} / {tenLop}";

                if (docGia.TheDocGium != null)
                {
                    lblCardMaThe.Text = string.IsNullOrWhiteSpace(docGia.TheDocGium.MaTheHienThi)
                        ? $"TDG{docGia.TheDocGium.MaThe:D6}"
                        : docGia.TheDocGium.MaTheHienThi;

                    lblCardLoaiThe.Text = "Thẻ " + (docGia.LoaiDocGia?.ToLower() ?? "độc giả");
                    lblCardNgayCap.Text = docGia.TheDocGium.NgayCap.ToString("dd/MM/yyyy");
                    lblCardNgayHetHan.Text = docGia.TheDocGium.NgayHetHan.ToString("dd/MM/yyyy");
                    lblCardTrangThai.Text = docGia.TheDocGium.TrangThai;

                    if (docGia.TheDocGium.TrangThai == "Bị khóa")
                    {
                        lblCardTrangThai.ForeColor = Color.FromArgb(220, 38, 38);
                        btnTrangThaiDocGia.Text = "Bị khóa";
                        btnTrangThaiDocGia.FillColor = Color.FromArgb(254, 226, 226);
                        btnTrangThaiDocGia.ForeColor = Color.FromArgb(220, 38, 38);
                    }
                    else
                    {
                        lblCardTrangThai.ForeColor = Color.FromArgb(22, 163, 74);
                        btnTrangThaiDocGia.Text = "Đang hoạt động";
                        btnTrangThaiDocGia.FillColor = Color.FromArgb(209, 250, 223);
                        btnTrangThaiDocGia.ForeColor = Color.FromArgb(2, 122, 72);
                    }
                }
                else
                {
                    lblCardMaThe.Text = "Chưa có";
                    lblCardTrangThai.Text = "Chưa cấp thẻ";
                }

                btnXacNhanThu.Enabled = docGia.TheDocGium != null;
                btnInPhieuThu.Enabled = false;
                picDocGia.Image = DatabaseImageHelper.LoadReaderAvatar(docGia.AnhDaiDien, docGia.HoTen, 100, 100);

                // Nạp lịch sử đóng lệ phí
                NapLichSuDongPhi(maDocGia);
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

        private void NapLichSuDongPhi(int maDocGia)
        {
            try
            {
                List<LichSuDongPhiGridModel> ds = _docGiaService.GetLichSuDongPhi(maDocGia);
                dgvLichSu.Rows.Clear();

                foreach (var item in ds)
                {
                    dgvLichSu.Rows.Add(
                        item.STT,
                        item.NamDong,
                        $"{item.SoTien:N0}".Replace(",", "."),
                        item.NgayDong.ToString("dd/MM/yyyy"),
                        item.HinhThucThu,
                        item.NguoiThu,
                        item.SoPhieuThu,
                        item.GhiChu
                    );
                }

                ChonNamThuongNienChuaDong(ds);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể tải lịch sử đóng phí: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void HienThiChuaChonDocGia()
        {
            lblMaDocGiaHeader.Text = "DG----";
            lblHoTen.Text = "Chưa chọn độc giả";
            lblNgaySinh.Text = lblDienThoai.Text = lblEmail.Text = lblDiaChi.Text = "-";
            lblLoaiDocGia.Text = lblKhoaLop.Text = "-";
            lblCardMaThe.Text = "Chưa có";
            lblCardLoaiThe.Text = lblCardNgayCap.Text = lblCardNgayHetHan.Text = "-";
            lblCardTrangThai.Text = "Chưa chọn";
            dgvLichSu.Rows.Clear();
            btnXacNhanThu.Enabled = false;
            btnInPhieuThu.Enabled = false;
        }

        private void BtnXacNhanThu_Click(object? sender, EventArgs e)
        {
            if (_maDocGia <= 0)
            {
                MessageBox.Show("Vui lòng chọn hoặc tra cứu độc giả trước khi thu lệ phí.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            decimal soTien = ParseMoney(txtSoTien.Text);
            if (soTien <= 0)
            {
                MessageBox.Show("Số tiền thu không hợp lệ.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoTien.Focus();
                return;
            }

            string soPhieuThu = txtSoPhieuThu.Text.Trim();
            if (string.IsNullOrWhiteSpace(soPhieuThu))
            {
                soPhieuThu = _docGiaService.GetNextMaPhieuThuHienThi();
                txtSoPhieuThu.Text = soPhieuThu;
            }

            int namDong = int.TryParse(cboNamDong.SelectedItem?.ToString(), out int y) ? y : DateTime.Today.Year;
            string hinhThucThu = cboHinhThucThu.SelectedItem?.ToString() ?? "Tiền mặt";
            string loaiLePhi = cboLoaiLePhi.SelectedItem?.ToString() ?? "Lệ phí thường niên";

            decimal soTienNhan = ParseMoney(txtSoTienNhan.Text);
            if (hinhThucThu == "Tiền mặt" && soTienNhan < soTien)
            {
                MessageBox.Show("Số tiền nhận chưa đủ số tiền cần thu.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoTienNhan.Focus();
                return;
            }

            ThuLePhiInputModel model = new()
            {
                MaDocGia = _maDocGia,
                LoaiLePhi = loaiLePhi,
                NamDongPhi = namDong,
                SoTien = soTien,
                NgayThu = DateOnly.FromDateTime(dtpNgayThu.Value),
                HinhThucThu = hinhThucThu,
                GhiChu = txtGhiChu.Text.Trim(),
                SoPhieuThu = soPhieuThu,
                MaNhanVienThu = CurrentUser.MaNhanVien,
                SoTienNhan = soTienNhan
            };

            try
            {
                _docGiaService.ThuLePhiChiTiet(model);
                MessageBox.Show(
                    $"Thu lệ phí thành công!\nSố phiếu thu: {soPhieuThu}\nSố tiền: {soTien:N0} VNĐ",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                btnInPhieuThu.Enabled = true;
                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể thu lệ phí: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnLamMoi_Click(object? sender, EventArgs e)
        {
            dtpNgayThu.Value = DateTime.Today;
            txtSoPhieuThu.Text = _docGiaService.GetNextMaPhieuThuHienThi();
            txtGhiChu.Clear();
            txtSoTienNhan.Text = txtSoTien.Text;
            TinhTienThua();

            if (_maDocGia > 0)
            {
                NapLichSuDongPhi(_maDocGia);
            }
        }

        private void BtnInPhieuThu_Click(object? sender, EventArgs e)
        {
            MessageBox.Show("Chức năng in phiếu thu chưa được cấu hình. Dữ liệu giao dịch đã được lưu an toàn.",
                "Chưa cấu hình máy in", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

    }
}

