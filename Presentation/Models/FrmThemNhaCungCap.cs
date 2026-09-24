using BusinessLayer.Services;
using DataLayer.Context;
using DataLayer.Entities;
using DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using Presentation.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentation.Models
{
    public partial class FrmThemNhaCungCap : Form
    {
        private readonly NhapSachService _nhapSachService;
        private readonly List<string> _danhSachNhomSach = new() { "Tin học", "Kinh tế", "Giáo trình" };
        private string? _duongDanLogo;

        public FrmThemNhaCungCap()
        {
            InitializeComponent();
            _nhapSachService = new NhapSachService();
            KhoiTaoForm();
        }

        private void KhoiTaoForm()
        {
            if (DesignModeHelper.IsDesignMode(this)) return;

            KeyPreview = true;
            StartPosition = FormStartPosition.CenterScreen;

            Load += FrmThemNhaCungCap_Load;

            cboLoaiNcc.SelectedIndexChanged += CboLoaiNcc_SelectedIndexChanged;
            cboKhuVucCungCap.SelectedIndexChanged += CboKhuVucCungCap_SelectedIndexChanged;
            cboTrangThai.SelectedIndexChanged += CboTrangThai_SelectedIndexChanged;

            txtGhiChu.TextChanged += TxtGhiChu_TextChanged;
            btnChonLogo.Click += BtnChonLogo_Click;
            btnChonFile.Click += BtnChonFile_Click;

            btnLuuTam.Click += (s, e) => LuuNhaCungCap();
            btnLuuNhaCungCap.Click += (s, e) => LuuNhaCungCap();
            btnHuy.Click += (s, e) => Close();
            btnCloseBox.Click += (s, e) => Close();
        }

        private async void FrmThemNhaCungCap_Load(object? sender, EventArgs e)
        {
            try
            {
                string nextMaNcc = _nhapSachService.GetNextMaNccHienThi();
                txtMaNcc.Text = string.IsNullOrWhiteSpace(nextMaNcc) ? "NCC00128" : nextMaNcc;

                NapDanhSachLoaiNcc();
                NapDanhSachDieuKhoan();
                NapDanhSachKhuVuc();
                NapDanhSachPhuongThuc();
                NapDanhSachDanhGia();
                NapDanhSachTrangThai();

                dtpNgayBatDau.Value = DateTime.Today;

                CapNhatNhomSachTags();
                CapNhatThongKe();
                await TaiDanhSachNccGanDayAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể khởi tạo form thêm nhà cung cấp: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void NapDanhSachLoaiNcc()
        {
            cboLoaiNcc.Items.Clear();
            cboLoaiNcc.Items.AddRange(new object[] { "Nhà sách / phát hành", "Nhà xuất bản", "Đại lý phân phối", "Khác" });
            cboLoaiNcc.SelectedIndex = 0;
            lblValueCardLoaiNcc.Text = "Nhà sách / phát hành";
        }

        private void NapDanhSachDieuKhoan()
        {
            cboDieuKhoanThanhToan.Items.Clear();
            cboDieuKhoanThanhToan.Items.AddRange(new object[] { "Trả chậm 30 ngày", "Trả chậm 60 ngày", "Thanh toán ngay", "Gối đầu" });
            cboDieuKhoanThanhToan.SelectedIndex = 0;
        }

        private void NapDanhSachKhuVuc()
        {
            cboKhuVucCungCap.Items.Clear();
            cboKhuVucCungCap.Items.AddRange(new object[] { "Hà Nội", "TP. Hồ Chí Minh", "Đà Nẵng", "Toàn quốc" });
            cboKhuVucCungCap.SelectedIndex = 0;
            lblValueCardKhuVuc.Text = "Hà Nội";
        }

        private void NapDanhSachPhuongThuc()
        {
            cboPhuongThucThanhToan.Items.Clear();
            cboPhuongThucThanhToan.Items.AddRange(new object[] { "Chuyển khoản", "Tiền mặt", "Ví điện tử" });
            cboPhuongThucThanhToan.SelectedIndex = 0;
        }

        private void NapDanhSachDanhGia()
        {
            cboDanhGiaBanDau.Items.Clear();
            cboDanhGiaBanDau.Items.AddRange(new object[] { "Tốt", "Xuất sắc", "Khá" });
            cboDanhGiaBanDau.SelectedIndex = 0;
        }

        private void NapDanhSachTrangThai()
        {
            cboTrangThai.Items.Clear();
            cboTrangThai.Items.Add("Đang hoạt động");
            cboTrangThai.Items.Add("Ngừng hoạt động");
            cboTrangThai.SelectedIndex = 0;
            lblValueCardTrangThai.Text = "Đang hoạt động";
        }

        private void CapNhatNhomSachTags()
        {
            flpNhomSachTags.Controls.Clear();
            foreach (string ns in _danhSachNhomSach)
            {
                Panel tagPanel = new()
                {
                    AutoSize = true,
                    BackColor = Color.FromArgb(239, 246, 255),
                    Margin = new Padding(3, 3, 3, 3),
                    Padding = new Padding(4, 2, 4, 2)
                };

                Label lblName = new()
                {
                    Text = ns + "  ×",
                    Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(37, 99, 235),
                    AutoSize = true,
                    Cursor = Cursors.Hand
                };

                string currentName = ns;
                lblName.Click += (s, e) =>
                {
                    _danhSachNhomSach.Remove(currentName);
                    CapNhatNhomSachTags();
                };

                tagPanel.Controls.Add(lblName);
                flpNhomSachTags.Controls.Add(tagPanel);
            }
        }

        private void CboLoaiNcc_SelectedIndexChanged(object? sender, EventArgs e)
        {
            lblValueCardLoaiNcc.Text = cboLoaiNcc.SelectedItem?.ToString() ?? "Nhà sách / phát hành";
        }

        private void CboKhuVucCungCap_SelectedIndexChanged(object? sender, EventArgs e)
        {
            lblValueCardKhuVuc.Text = cboKhuVucCungCap.SelectedItem?.ToString() ?? "Hà Nội";
        }

        private void CboTrangThai_SelectedIndexChanged(object? sender, EventArgs e)
        {
            lblValueCardTrangThai.Text = cboTrangThai.SelectedItem?.ToString() ?? "Đang hoạt động";
        }

        private void CapNhatThongKe()
        {
            lblValueCardLoaiNcc.Text = cboLoaiNcc.SelectedItem?.ToString() ?? "Nhà sách / phát hành";
            lblValueCardKhuVuc.Text = cboKhuVucCungCap.SelectedItem?.ToString() ?? "Hà Nội";
            lblValueCardTrangThai.Text = cboTrangThai.SelectedItem?.ToString() ?? "Đang hoạt động";
            lblValueCardCongNo.Text = "0 đ";
        }

        private void TxtGhiChu_TextChanged(object? sender, EventArgs e)
        {
            int len = txtGhiChu.Text.Length;
            lblDemGhiChu.Text = $"{len}/500";
            lblDemGhiChu.ForeColor = len > 500 ? Color.Red : Color.Gray;
        }

        private void BtnChonLogo_Click(object? sender, EventArgs e)
        {
            using OpenFileDialog ofd = new()
            {
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp",
                Title = "Chọn logo nhà cung cấp"
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                _duongDanLogo = ofd.FileName;
                using Image source = Image.FromFile(ofd.FileName);
                Image? oldImage = picLogo.Image;
                picLogo.Image = new Bitmap(source);
                oldImage?.Dispose();
            }
        }

        private void BtnChonFile_Click(object? sender, EventArgs e)
        {
            using OpenFileDialog ofd = new()
            {
                Filter = "All Files|*.*",
                Title = "Chọn tài liệu đính kèm"
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                lblAttachmentText.Text = Path.GetFileName(ofd.FileName);
            }
        }

        private async Task TaiDanhSachNccGanDayAsync()
        {
            await Task.CompletedTask;
            flpRecentNccList.Controls.Clear();

            var listNcc = _nhapSachService.GetNhaCungCaps();

            if (listNcc.Count == 0)
            {
                Label lblEmpty = new()
                {
                    Text = "Chưa có nhà cung cấp nào.",
                    Font = new Font("Segoe UI", 9F, FontStyle.Italic),
                    ForeColor = Color.Gray,
                    AutoSize = true,
                    Margin = new Padding(10)
                };
                flpRecentNccList.Controls.Add(lblEmpty);
                return;
            }

            Color[] colors = new[] { Color.FromArgb(220, 38, 38), Color.FromArgb(22, 163, 74), Color.FromArgb(37, 99, 235), Color.FromArgb(217, 119, 6) };

            for (int i = 0; i < listNcc.Count && i < 5; i++)
            {
                var ncc = listNcc[i];
                Panel item = new()
                {
                    Size = new Size(295, 75),
                    Margin = new Padding(0, 0, 0, 10),
                    BackColor = Color.White
                };

                string badgeText = ncc.TenNhaCungCap.Length > 6 ? ncc.TenNhaCungCap.Substring(0, 6).ToUpper() : ncc.TenNhaCungCap.ToUpper();

                Guna.UI2.WinForms.Guna2Button btnBadgeLogo = new()
                {
                    Size = new Size(52, 52),
                    Location = new Point(0, 5),
                    FillColor = colors[i % colors.Length],
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 7F, FontStyle.Bold),
                    Text = badgeText,
                    BorderRadius = 6,
                    Enabled = false
                };

                Label lblNccName = new()
                {
                    Text = ncc.TenNhaCungCap,
                    Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(30, 40, 60),
                    Location = new Point(60, 5),
                    Size = new Size(230, 34)
                };

                Label lblPhoneStart = new()
                {
                    Text = $"📞 {(string.IsNullOrWhiteSpace(ncc.SoDienThoai) ? "-" : ncc.SoDienThoai)}\n✉️ {(string.IsNullOrWhiteSpace(ncc.Email) ? "-" : ncc.Email)}",
                    Font = new Font("Segoe UI", 7.5F),
                    ForeColor = Color.FromArgb(100, 110, 125),
                    Location = new Point(60, 38),
                    Size = new Size(230, 30)
                };

                item.Controls.Add(btnBadgeLogo);
                item.Controls.Add(lblNccName);
                item.Controls.Add(lblPhoneStart);

                flpRecentNccList.Controls.Add(item);
            }
        }

        private void LuuNhaCungCap()
        {
            if (string.IsNullOrWhiteSpace(txtTenNcc.Text))
            {
                MessageBox.Show("Vui lòng nhập Tên nhà cung cấp.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenNcc.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtSoDienThoai.Text))
            {
                MessageBox.Show("Vui lòng nhập Số điện thoại nhà cung cấp.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoDienThoai.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtDiaChi.Text))
            {
                MessageBox.Show("Vui lòng nhập Địa chỉ nhà cung cấp.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDiaChi.Focus();
                return;
            }

            if (!ThuDocSoTien(txtHanMucCongNo.Text, out decimal hanMuc) || hanMuc < 0)
            {
                MessageBox.Show("Hạn mức công nợ phải là số không âm.", "Dữ liệu không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtHanMucCongNo.Focus();
                return;
            }

            string strChietKhau = txtChietKhauMacDinh.Text.Replace("%", string.Empty).Trim();
            if (!double.TryParse(strChietKhau, out double chietKhau) || chietKhau < 0 || chietKhau > 100)
            {
                MessageBox.Show("Chiết khấu phải nằm trong khoảng 0 đến 100%.", "Dữ liệu không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtChietKhauMacDinh.Focus();
                return;
            }

            ThemNhaCungCapInputModel model = new()
            {
                MaNccHienThi = txtMaNcc.Text.Trim(),
                TenNcc = txtTenNcc.Text.Trim(),
                NguoiDaiDien = txtNguoiDaiDien.Text.Trim(),
                SoDienThoai = txtSoDienThoai.Text.Trim(),
                Email = txtEmail.Text.Trim(),
                DiaChi = txtDiaChi.Text.Trim(),
                MaSoThue = txtMaSoThue.Text.Trim(),
                Website = txtWebsite.Text.Trim(),
                LoaiNcc = cboLoaiNcc.SelectedItem?.ToString() ?? "Nhà sách / phát hành",
                TrangThai = cboTrangThai.SelectedItem?.ToString() == "Đang hoạt động",
                Logo = _duongDanLogo,
                DieuKhoanThanhToan = cboDieuKhoanThanhToan.SelectedItem?.ToString() ?? "Trả chậm 30 ngày",
                NgayBatDauHopTac = dtpNgayBatDau.Value,
                KhuVucCungCap = cboKhuVucCungCap.SelectedItem?.ToString() ?? "Hà Nội",
                NhomSachCungCap = _danhSachNhomSach,
                GhiChu = txtGhiChu.Text.Trim(),
                HanMucCongNo = hanMuc,
                ChietKhauMacDinh = chietKhau,
                PhuongThucThanhToan = cboPhuongThucThanhToan.SelectedItem?.ToString() ?? "Chuyển khoản",
                DanhGiaBanDau = cboDanhGiaBanDau.SelectedItem?.ToString() ?? "Tốt",
                CongNoHienTai = 0m
            };

            try
            {
                _nhapSachService.ThemNhaCungCap(model);
                MessageBox.Show($"Thêm nhà cung cấp {txtTenNcc.Text} ({txtMaNcc.Text}) thành công!",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể lưu nhà cung cấp: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static bool ThuDocSoTien(string text, out decimal value)
        {
            string normalized = text.Replace("đ", string.Empty).Replace(".", string.Empty).Replace(",", string.Empty).Trim();
            return decimal.TryParse(normalized, out value);
        }
    }
}
