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
    public partial class FrmThemDauSach : Form
    {
        private readonly SachService _sachService;
        private readonly List<LookupItemModel> _danhSachTacGia = new();
        private string? _duongDanAnhBia;

        public FrmThemDauSach()
        {
            InitializeComponent();
            _sachService = new SachService();
            KhoiTaoForm();
        }

        private void KhoiTaoForm()
        {
            if (DesignModeHelper.IsDesignMode(this)) return;

            KeyPreview = true;
            StartPosition = FormStartPosition.CenterScreen;

            Load += FrmThemDauSach_Load;

            cboTheLoai.SelectedIndexChanged += CboTheLoai_SelectedIndexChanged;
            flpTacGiaTags.DoubleClick += FlpTacGiaTags_DoubleClick;
            numSoLuongBanSao.ValueChanged += NumSoLuongBanSao_ValueChanged;
            txtGiaNhapMoiCuon.TextChanged += TxtGiaNhapMoiCuon_TextChanged;

            txtMoTa.TextChanged += TxtMoTa_TextChanged;
            txtGhiChu.TextChanged += TxtGhiChu_TextChanged;

            btnChonAnh.Click += BtnChonAnh_Click;
            btnLuuTam.Click += (s, e) => LuuDauSach(taoBanSao: false);
            btnLuuVaTaoBanSao.Click += (s, e) => LuuDauSach(taoBanSao: true);
            btnHuy.Click += (s, e) => Close();
            btnCloseBox.Click += (s, e) => Close();
        }

        private async void FrmThemDauSach_Load(object? sender, EventArgs e)
        {
            try
            {
                txtTenSach.MaxLength = 100;
                string nextMaSach = _sachService.GetNextMaSachHienThi();
                txtMaSach.Text = string.IsNullOrWhiteSpace(nextMaSach) ? "DS000128" : nextMaSach;

                NapDanhSachTheLoai();
                NapDanhSachNhaXuatBan();
                NapDanhSachNgonNgu();
                NapDanhSachViTriGoiY();
                NapDanhSachTinhTrang();

                dtpNgayNhap.Value = DateTime.Today;

                CapNhatAuthorTags();
                CapNhatThongKe();
                await TaiDanhSachDauSachGanDayAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể khởi tạo form thêm đầu sách: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void NapDanhSachTheLoai()
        {
            cboTheLoai.DataSource = _sachService.LayTheLoai().ToList();
            cboTheLoai.DisplayMember = nameof(LookupItemModel.Ten);
            cboTheLoai.ValueMember = nameof(LookupItemModel.Id);
            lblValueCardTheLoai.Text = cboTheLoai.SelectedItem is LookupItemModel item ? item.Ten : "-";
        }

        private void NapDanhSachNhaXuatBan()
        {
            cboNhaXuatBan.DataSource = _sachService.LayNhaXuatBan().ToList();
            cboNhaXuatBan.DisplayMember = nameof(LookupItemModel.Ten);
            cboNhaXuatBan.ValueMember = nameof(LookupItemModel.Id);
        }

        private void NapDanhSachNgonNgu()
        {
            cboNgonNgu.Items.Clear();
            cboNgonNgu.Items.AddRange(new object[] { "Tiếng Việt", "Tiếng Anh", "Tiếng Pháp", "Tiếng Nhật" });
            cboNgonNgu.SelectedIndex = 0;
        }

        private void NapDanhSachViTriGoiY()
        {
            cboViTriGoiY.DataSource = _sachService.LayViTri().ToList();
            cboViTriGoiY.DisplayMember = nameof(LookupItemModel.Ten);
            cboViTriGoiY.ValueMember = nameof(LookupItemModel.Id);
        }

        private void NapDanhSachTinhTrang()
        {
            cboTrangThai.Items.Clear();
            cboTrangThai.Items.Add("Đang hoạt động");
            cboTrangThai.Items.Add("Ngừng hoạt động");
            cboTrangThai.SelectedIndex = 0;

            cboTinhTrangBanSao.Items.Clear();
            cboTinhTrangBanSao.Items.Add("Tốt");
            cboTinhTrangBanSao.Items.Add("Mới 100%");
            cboTinhTrangBanSao.SelectedIndex = 0;

            cboTuTaoMaVach.Items.Clear();
            cboTuTaoMaVach.Items.Add("Có");
            cboTuTaoMaVach.Items.Add("Không");
            cboTuTaoMaVach.SelectedIndex = 0;
        }

        private void CapNhatAuthorTags()
        {
            flpTacGiaTags.Controls.Clear();
            foreach (LookupItemModel tg in _danhSachTacGia)
            {
                Panel tagPanel = new()
                {
                    AutoSize = true,
                    BackColor = Color.FromArgb(239, 246, 255),
                    Margin = new Padding(3),
                    Padding = new Padding(4, 2, 4, 2)
                };

                Label lblName = new()
                {
                    Text = tg.Ten + "  ×",
                    Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(37, 99, 235),
                    AutoSize = true,
                    Cursor = Cursors.Hand
                };

                int currentId = tg.Id;
                lblName.Click += (_, _) =>
                {
                    _danhSachTacGia.RemoveAll(x => x.Id == currentId);
                    CapNhatAuthorTags();
                    CapNhatThongKe();
                };

                tagPanel.Controls.Add(lblName);
                flpTacGiaTags.Controls.Add(tagPanel);
            }

            lblValueCardTacGia.Text = _danhSachTacGia.Count.ToString();
        }

        private void FlpTacGiaTags_DoubleClick(object? sender, EventArgs e)
        {
            List<LookupItemModel> conLai = _sachService.LayTacGia()
                .Where(x => _danhSachTacGia.All(tg => tg.Id != x.Id)).ToList();
            if (conLai.Count == 0)
            {
                MessageBox.Show("Không còn tác giả nào để chọn.", "Chọn tác giả",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using Form dialog = new() { Text = "Chọn tác giả", StartPosition = FormStartPosition.CenterParent, Width = 420, Height = 150 };
            ComboBox combo = new() { DataSource = conLai, DisplayMember = nameof(LookupItemModel.Ten), ValueMember = nameof(LookupItemModel.Id), Dock = DockStyle.Top, DropDownStyle = ComboBoxStyle.DropDownList };
            Button ok = new() { Text = "Chọn", Dock = DockStyle.Bottom, DialogResult = DialogResult.OK };
            dialog.Controls.Add(combo);
            dialog.Controls.Add(ok);
            dialog.AcceptButton = ok;
            if (dialog.ShowDialog(this) == DialogResult.OK && combo.SelectedItem is LookupItemModel item)
            {
                _danhSachTacGia.Add(item);
                CapNhatAuthorTags();
                CapNhatThongKe();
            }
        }

        private void CboTheLoai_SelectedIndexChanged(object? sender, EventArgs e)
        {
            lblValueCardTheLoai.Text = cboTheLoai.SelectedItem is LookupItemModel item ? item.Ten : "-";
        }

        private void NumSoLuongBanSao_ValueChanged(object? sender, EventArgs e)
        {
            CapNhatThongKe();
        }

        private void TxtGiaNhapMoiCuon_TextChanged(object? sender, EventArgs e)
        {
            CapNhatThongKe();
        }

        private void CapNhatThongKe()
        {
            int numCopies = (int)numSoLuongBanSao.Value;
            lblValueCardBanSao.Text = numCopies.ToString();
            lblInfoBannerText.Text = $"Sau khi lưu, hệ thống sẽ tự động tạo {numCopies} bản sao vật lý và quản lý trong kho thư viện.";

            string strGia = txtGiaNhapMoiCuon.Text.Replace("đ", string.Empty).Replace(".", string.Empty).Trim();
            if (decimal.TryParse(strGia, out decimal giaMoiCuon))
            {
                decimal tongGia = numCopies * giaMoiCuon;
                lblValueCardTongGia.Text = $"{tongGia:N0} đ";
            }
            else
            {
                lblValueCardTongGia.Text = $"{numCopies * 150000m:N0} đ";
            }
        }

        private void TxtMoTa_TextChanged(object? sender, EventArgs e)
        {
            int len = txtMoTa.Text.Length;
            lblDemMoTa.Text = $"{len}/1000";
            lblDemMoTa.ForeColor = len > 1000 ? Color.Red : Color.Gray;
        }

        private void TxtGhiChu_TextChanged(object? sender, EventArgs e)
        {
            int len = txtGhiChu.Text.Length;
            lblDemGhiChu.Text = $"{len}/500";
            lblDemGhiChu.ForeColor = len > 500 ? Color.Red : Color.Gray;
        }

        private void BtnChonAnh_Click(object? sender, EventArgs e)
        {
            using OpenFileDialog ofd = new()
            {
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp",
                Title = "Chọn ảnh bìa sách"
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                _duongDanAnhBia = ofd.FileName;
                using Image source = Image.FromFile(ofd.FileName);
                Image? oldImage = picAnhBia.Image;
                picAnhBia.Image = new Bitmap(source);
                oldImage?.Dispose();
            }
        }

        private async Task TaiDanhSachDauSachGanDayAsync()
        {
            await Task.CompletedTask;
            flpRecentBooksList.Controls.Clear();

            var recentBooks = _sachService.LayDanhSach(new BusinessLayer.DTOs.SachFilterDto
            {
                Trang = 1,
                SoDongMoiTrang = 5,
                SapXep = "MaSachDesc"
            }).DuLieu;

            if (recentBooks.Count == 0)
            {
                Label lblEmpty = new()
                {
                    Text = "Chưa có đầu sách nào.",
                    Font = new Font("Segoe UI", 9F, FontStyle.Italic),
                    ForeColor = Color.Gray,
                    AutoSize = true,
                    Margin = new Padding(10)
                };
                flpRecentBooksList.Controls.Add(lblEmpty);
                return;
            }

            foreach (var sach in recentBooks)
            {
                Panel item = new()
                {
                    Size = new Size(295, 75),
                    Margin = new Padding(0, 0, 0, 10),
                    BackColor = Color.White
                };

                Guna.UI2.WinForms.Guna2PictureBox picCover = new()
                {
                    Size = new Size(50, 68),
                    Location = new Point(0, 3),
                    FillColor = Color.FromArgb(239, 246, 255),
                    SizeMode = PictureBoxSizeMode.Zoom,
                    BorderRadius = 4
                };
                if (!string.IsNullOrWhiteSpace(sach.AnhBia))
                {
                    string? imgPath = DatabaseImageHelper.ResolvePath(sach.AnhBia);
                    if (imgPath != null && File.Exists(imgPath))
                    {
                        using var stream = new FileStream(imgPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                        using Image source = Image.FromStream(stream);
                        picCover.Image = new Bitmap(source);
                    }
                }

                Label lblBookTitle = new()
                {
                    Text = sach.TenSach,
                    Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(30, 40, 60),
                    Location = new Point(60, 5),
                    Size = new Size(230, 34)
                };

                Label lblAuthorYear = new()
                {
                    Text = $"👤 {sach.TacGia}\n📅 NXB: {sach.NamXuatBan}",
                    Font = new Font("Segoe UI", 7.5F),
                    ForeColor = Color.FromArgb(100, 110, 125),
                    Location = new Point(60, 38),
                    Size = new Size(230, 30)
                };

                item.Controls.Add(picCover);
                item.Controls.Add(lblBookTitle);
                item.Controls.Add(lblAuthorYear);

                flpRecentBooksList.Controls.Add(item);
            }
        }

        private void LuuDauSach(bool taoBanSao)
        {
            string tenSach = txtTenSach.Text.Trim();
            if (string.IsNullOrWhiteSpace(tenSach))
            {
                MessageBox.Show("Vui lòng nhập Tên sách.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenSach.Focus();
                return;
            }
            if (tenSach.Length > 100)
            {
                MessageBox.Show("Tên sách không được vượt quá 100 ký tự.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenSach.Focus();
                return;
            }

            if (cboTheLoai.SelectedItem is not LookupItemModel theLoai || theLoai.Id <= 0)
            {
                MessageBox.Show("Vui lòng chọn Thể loại sách.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboTheLoai.Focus();
                return;
            }
            if (cboNhaXuatBan.SelectedItem is not LookupItemModel nxb || nxb.Id <= 0)
            {
                MessageBox.Show("Vui lòng chọn Nhà xuất bản hợp lệ.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboNhaXuatBan.Focus();
                return;
            }
            if (_danhSachTacGia.Count == 0)
            {
                MessageBox.Show("Nhấp đúp vùng tác giả để chọn ít nhất một tác giả.", "Thiếu tác giả", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!int.TryParse(txtNamXuatBan.Text.Trim(), out int namXb) || namXb < 1000 || namXb > DateTime.Today.Year)
            {
                MessageBox.Show("Năm xuất bản phải từ 1000 đến năm hiện tại.", "Dữ liệu không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNamXuatBan.Focus();
                return;
            }
            if (!int.TryParse(txtSoTrang.Text.Trim(), out int soTrang) || soTrang <= 0)
            {
                MessageBox.Show("Số trang phải là số nguyên lớn hơn 0.", "Dữ liệu không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoTrang.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtGiaBia.Text))
            {
                MessageBox.Show("Vui lòng nhập Giá bìa (Giá bìa không được để trống).", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtGiaBia.Focus();
                return;
            }

            if (!ThuDocSoTien(txtGiaBia.Text, out decimal giaBia) || giaBia <= 0)
            {
                MessageBox.Show("Giá bìa phải là số thực lớn hơn 0.", "Dữ liệu không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtGiaBia.Focus();
                return;
            }

            if (!ThuDocSoTien(txtGiaNhapMoiCuon.Text, out decimal giaNhap) || giaNhap < 0)
            {
                MessageBox.Show("Giá nhập phải là số không âm.", "Dữ liệu không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtGiaNhapMoiCuon.Focus();
                return;
            }

            if (taoBanSao && numSoLuongBanSao.Value <= 0)
            {
                MessageBox.Show("Số lượng bản sao phải lớn hơn 0.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                numSoLuongBanSao.Focus();
                return;
            }

            string? anhBia = null;
            if (!string.IsNullOrWhiteSpace(_duongDanAnhBia) && File.Exists(_duongDanAnhBia))
            {
                string targetName = $"book_{DateTime.Now:yyyyMMddHHmmss}_{Guid.NewGuid():N}"[..42];
                anhBia = DatabaseImageHelper.CopyToProjectImages(_duongDanAnhBia, "BookCovers", targetName);
            }

            BusinessLayer.DTOs.SachSaveDto model = new()
            {
                MaSachHienThi = txtMaSach.Text.Trim(),
                TenSach = txtTenSach.Text.Trim(),
                Isbn = txtIsbn.Text.Trim(),
                MaTheLoai = theLoai.Id,
                MaNhaXuatBan = nxb.Id,
                NamXuatBan = namXb,
                NgonNgu = cboNgonNgu.SelectedItem?.ToString() ?? "Tiếng Việt",
                SoTrang = soTrang,
                GiaBia = giaBia,
                TrangThai = cboTrangThai.SelectedItem?.ToString() == "Đang hoạt động",
                AnhBia = anhBia,
                MoTa = string.Join(Environment.NewLine, new[] { txtMoTa.Text.Trim(), txtGhiChu.Text.Trim() }.Where(x => !string.IsNullOrWhiteSpace(x))),
                MaViTri = cboViTriGoiY.SelectedItem is LookupItemModel viTri ? viTri.Id : null,
                MaTacGia = _danhSachTacGia.Select(x => x.Id).ToList(),
                SoLuong = taoBanSao ? (int)numSoLuongBanSao.Value : 0,
                GiaNhap = giaNhap,
                NgayNhap = DateOnly.FromDateTime(dtpNgayNhap.Value),
                TinhTrangCuon = cboTinhTrangBanSao.SelectedItem?.ToString() ?? "Tốt",
                TienToMaVach = cboTuTaoMaVach.SelectedItem?.ToString() == "Có" ? "893" : null,
                GhiChuCuon = txtGhiChuBanSao.Text.Trim()
            };

            try
            {
                _sachService.Them(model);
                MessageBox.Show($"Thêm đầu sách mới {txtMaSach.Text} thành công!{(taoBanSao ? $" Đã tạo {(int)numSoLuongBanSao.Value} bản sao vật lý." : string.Empty)}",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể lưu đầu sách: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ResetFormInput()
        {
            txtTenSach.Clear();
            txtIsbn.Clear();
            txtNamXuatBan.Clear();
            txtSoTrang.Clear();
            txtGiaBia.Clear();
            txtGiaNhapMoiCuon.Clear();
            txtMoTa.Clear();
            txtGhiChu.Clear();
            txtGhiChuBanSao.Clear();
            _danhSachTacGia.Clear();
            CapNhatAuthorTags();
            if (cboTheLoai.Items.Count > 0) cboTheLoai.SelectedIndex = 0;
            if (cboNhaXuatBan.Items.Count > 0) cboNhaXuatBan.SelectedIndex = 0;
            if (cboNgonNgu.Items.Count > 0) cboNgonNgu.SelectedIndex = 0;
            if (cboViTriGoiY.Items.Count > 0) cboViTriGoiY.SelectedIndex = 0;
            if (cboTrangThai.Items.Count > 0) cboTrangThai.SelectedIndex = 0;
            if (cboTinhTrangBanSao.Items.Count > 0) cboTinhTrangBanSao.SelectedIndex = 0;
            numSoLuongBanSao.Value = 1;
            dtpNgayNhap.Value = DateTime.Today;
        }

        private static bool ThuDocSoTien(string text, out decimal value)
        {
            string normalized = text.Replace("đ", string.Empty).Replace(".", string.Empty).Replace(",", string.Empty).Trim();
            return decimal.TryParse(normalized, out value);
        }

        private void FrmThemDauSach_Load_1(object sender, EventArgs e)
        {

        }
    }
}
