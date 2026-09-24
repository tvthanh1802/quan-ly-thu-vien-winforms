using BusinessLayer.Services;
using DataLayer.Context;
using DataLayer.Entities;
using DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using Presentation.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentation.Models
{
    public partial class FrmLapPhieuNhap : Form
    {
        private readonly NhapSachService _nhapSachService;
        private readonly List<SachNhapInputItem> _danhSachSach = new();
        private readonly List<NhaCungCapLookupModel> _danhSachNcc = new();

        public FrmLapPhieuNhap()
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

            Load += FrmLapPhieuNhap_Load;

            cboNhaCungCap.SelectedIndexChanged += CboNhaCungCap_SelectedIndexChanged;
            btnThemNCC.Click += BtnThemNCC_Click;

            btnThemDong.Click += BtnThemDong_Click;
            btnXoaDong.Click += BtnXoaDong_Click;
            btnChonSach.Click += BtnChonSach_Click;
            txtTraCuuSach.KeyDown += TxtTraCuuSach_KeyDown;

            dgvSachNhap.CellValueChanged += DgvSachNhap_CellValueChanged;
            dgvSachNhap.CellContentClick += DgvSachNhap_CellContentClick;
            dgvSachNhap.CellDoubleClick += DgvSachNhap_CellDoubleClick;
            dgvSachNhap.CellValidating += DgvSachNhap_CellValidating;
            dgvSachNhap.DataError += (_, e) => e.ThrowException = false;

            btnLuuTam.Click += (s, e) => LuuPhieuNhap("Đang nhập");
            btnHoanThanh.Click += (s, e) => LuuPhieuNhap("Hoàn thành");
            btnHuy.Click += (s, e) => Close();
            btnCloseBox.Click += (s, e) => Close();
        }

        private async void FrmLapPhieuNhap_Load(object? sender, EventArgs e)
        {
            try
            {
                txtMaPhieu.Text = _nhapSachService.GetNextMaPhieuNhapHienThi();
                dtpNgayNhap.MaxDate = DateTime.Today;
                dtpNgayNhap.Value = DateTime.Today;
                txtNguoiLap.Text = CurrentUser.HoTen;
                txtMaPhieu.ReadOnly = true;
                txtNguoiLap.ReadOnly = true;

                if (!CurrentUser.MaNhanVien.HasValue)
                    throw new InvalidOperationException("Tài khoản hiện tại không gắn với hồ sơ nhân viên. Không thể lập phiếu nhập.");

                NapDanhSachNhaCungCap();
                HienThiDuLieuMacDinh();
                await TaiDanhSachNhapGanDayAsync();
                CapNhatThongKe();
            }
            catch (Exception ex)
            {
                btnLuuTam.Enabled = false;
                btnHoanThanh.Enabled = false;
                MessageBox.Show("Không thể khởi tạo form lập phiếu nhập: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void NapDanhSachNhaCungCap()
        {
            _danhSachNcc.Clear();
            cboNhaCungCap.DataSource = null;
            _danhSachNcc.AddRange(_nhapSachService.GetNhaCungCaps());

            cboNhaCungCap.DataSource = _danhSachNcc.ToList();
            cboNhaCungCap.DisplayMember = nameof(NhaCungCapLookupModel.TenNhaCungCap);
            cboNhaCungCap.ValueMember = nameof(NhaCungCapLookupModel.MaNhaCungCap);
            cboNhaCungCap.SelectedIndex = _danhSachNcc.Count > 0 ? 0 : -1;
            CapNhatThongTinNhaCungCap();
        }

        private void CboNhaCungCap_SelectedIndexChanged(object? sender, EventArgs e)
        {
            CapNhatThongTinNhaCungCap();
        }

        private void CapNhatThongTinNhaCungCap()
        {
            if (cboNhaCungCap.SelectedItem is not NhaCungCapLookupModel ncc)
            {
                lblTenNcc.Text = "Chưa chọn nhà cung cấp";
                lblValueCardNcc.Text = "Chưa chọn";
                lblSdtNcc.Text = lblEmailNcc.Text = lblDiaChiNcc.Text = "-";
                return;
            }

            lblTenNcc.Text = ncc.TenNhaCungCap;
            lblValueCardNcc.Text = ncc.TenNhaCungCap;
            lblSdtNcc.Text = string.IsNullOrWhiteSpace(ncc.SoDienThoai) ? "-" : ncc.SoDienThoai;
            lblEmailNcc.Text = string.IsNullOrWhiteSpace(ncc.Email) ? "-" : ncc.Email;
            lblDiaChiNcc.Text = string.IsNullOrWhiteSpace(ncc.DiaChi) ? "-" : ncc.DiaChi;
        }

        private void HienThiDuLieuMacDinh()
        {
            _danhSachSach.Clear();
            HienThiGridSach();
        }

        private void HienThiGridSach()
        {
            dgvSachNhap.Rows.Clear();
            int stt = 1;

            foreach (var item in _danhSachSach)
            {
                dgvSachNhap.Rows.Add(
                    stt++,
                    item.MaSachText,
                    item.TenSach,
                    item.TheLoai,
                    item.SoLuongNhap,
                    $"{item.DonGiaNhap:N0} đ",
                    $"{item.ThanhTien:N0} đ",
                    item.GhiChu,
                    "🗑"
                );
            }

            CapNhatThongKe();
        }

        private void DgvSachNhap_CellValueChanged(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= _danhSachSach.Count) return;
            SachNhapInputItem item = _danhSachSach[e.RowIndex];

            if (e.ColumnIndex == colSoLuongNhap.Index &&
                int.TryParse(dgvSachNhap.Rows[e.RowIndex].Cells[colSoLuongNhap.Name].Value?.ToString(), out int soLuong))
            {
                item.SoLuongNhap = soLuong;
            }
            else if (e.ColumnIndex == colDonGiaNhap.Index &&
                     ThuDocSoTien(dgvSachNhap.Rows[e.RowIndex].Cells[colDonGiaNhap.Name].Value, out decimal donGia))
            {
                item.DonGiaNhap = donGia;
            }
            else if (e.ColumnIndex == colGhiChu.Index)
            {
                item.GhiChu = dgvSachNhap.Rows[e.RowIndex].Cells[colGhiChu.Name].Value?.ToString()?.Trim() ?? "-";
            }

            dgvSachNhap.Rows[e.RowIndex].Cells[colThanhTien.Name].Value = $"{item.ThanhTien:N0} đ";
            CapNhatThongKe();
        }

        private void DgvSachNhap_CellValidating(object? sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (e.ColumnIndex == colSoLuongNhap.Index &&
                (!int.TryParse(e.FormattedValue?.ToString(), out int soLuong) || soLuong <= 0))
            {
                e.Cancel = true;
                MessageBox.Show("Số lượng nhập phải là số nguyên lớn hơn 0.", "Dữ liệu không hợp lệ",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (e.ColumnIndex == colDonGiaNhap.Index &&
                     (!ThuDocSoTien(e.FormattedValue, out decimal donGia) || donGia < 0))
            {
                e.Cancel = true;
                MessageBox.Show("Đơn giá nhập phải là số không âm.", "Dữ liệu không hợp lệ",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private static bool ThuDocSoTien(object? value, out decimal result)
        {
            string text = value?.ToString() ?? string.Empty;
            text = text.Replace("đ", string.Empty).Replace(".", string.Empty).Replace(",", string.Empty).Trim();
            return decimal.TryParse(text, out result);
        }

        private void DgvSachNhap_CellContentClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < _danhSachSach.Count && e.ColumnIndex == colXoa.Index)
            {
                _danhSachSach.RemoveAt(e.RowIndex);
                HienThiGridSach();
            }
        }

        private void DgvSachNhap_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= _danhSachSach.Count) return;
            if (e.ColumnIndex == colSoLuongNhap.Index || 
                e.ColumnIndex == colDonGiaNhap.Index || 
                e.ColumnIndex == colGhiChu.Index || 
                e.ColumnIndex == colXoa.Index)
            {
                return;
            }

            var item = _danhSachSach[e.RowIndex];
            using FrmChiTietSach frm = new(item.MaSach);
            frm.ShowDialog(this);
        }

        private void CapNhatThongKe()
        {
            int totalDauSach = _danhSachSach.Count;
            int totalSoCuon = _danhSachSach.Sum(x => x.SoLuongNhap);
            decimal tamTinh = _danhSachSach.Sum(x => x.ThanhTien);

            lblValueCardDauSach.Text = totalDauSach.ToString();
            lblValueCardSoCuon.Text = totalSoCuon.ToString();

            lblTamTinh.Text = $"{tamTinh:N0} đ";
            lblChietKhau.Text = "0 đ";
            lblThueVat.Text = "0 đ";
            lblTongTien.Text = $"{tamTinh:N0} đ";

            lblTongSoDong.Text = $"Tổng số dòng: {totalDauSach}";
            lblTongSoCuon.Text = totalSoCuon.ToString();
        }

        private async Task TaiDanhSachNhapGanDayAsync()
        {
            await Task.CompletedTask;
            dgvNhapGanDay.Rows.Clear();
            var list = _nhapSachService.GetNhapGanDay(3);
            foreach (var item in list)
            {
                dgvNhapGanDay.Rows.Add(
                    item.MaPhieuText,
                    item.NgayNhap.ToString("dd/MM/yyyy"),
                    item.TenNhaCungCap,
                    $"{item.TongTien:N0} đ",
                    item.TrangThai);
            }
        }

        private void BtnThemDong_Click(object? sender, EventArgs e) => MoHopThoaiChonSach();

        private void BtnXoaDong_Click(object? sender, EventArgs e)
        {
            if (dgvSachNhap.CurrentRow == null || dgvSachNhap.CurrentRow.Index >= _danhSachSach.Count)
            {
                MessageBox.Show("Vui lòng chọn dòng cần xóa.", "Xóa dòng",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            _danhSachSach.RemoveAt(dgvSachNhap.CurrentRow.Index);
            HienThiGridSach();
        }

        private void BtnChonSach_Click(object? sender, EventArgs e) => MoHopThoaiChonSach();

        private void TxtTraCuuSach_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            e.SuppressKeyPress = true;
            MoHopThoaiChonSach(txtTraCuuSach.Text.Trim());
        }

        private void MoHopThoaiChonSach(string? tuKhoa = null)
        {
            using FrmChonSach frm = new(tuKhoa);
            if (frm.ShowDialog(this) != DialogResult.OK || frm.SachDaChon == null) return;

            SachNhapLookupModel sach = frm.SachDaChon;
            SachNhapInputItem? daCo = _danhSachSach.FirstOrDefault(x => x.MaSach == sach.MaSach);
            if (daCo != null)
            {
                daCo.SoLuongNhap++;
                HienThiGridSach();
                return;
            }

            _danhSachSach.Add(new SachNhapInputItem
            {
                MaChiTietNhap = _danhSachSach.Count + 1,
                MaSach = sach.MaSach,
                MaSachText = sach.MaSachText,
                TenSach = sach.TenSach,
                TheLoai = sach.TheLoai,
                SoLuongNhap = 1,
                DonGiaNhap = sach.GiaThamKhao,
                GhiChu = "-"
            });
            txtTraCuuSach.Clear();
            HienThiGridSach();
        }

        private void BtnThemNCC_Click(object? sender, EventArgs e)
        {
            using FrmThemNhaCungCap frm = new();
            if (frm.ShowDialog(this) == DialogResult.OK)
                NapDanhSachNhaCungCap();
        }

        private void LuuPhieuNhap(string trangThai)
        {
            dgvSachNhap.EndEdit();
            if (!KiemTraDuLieuTruocKhiLuu()) return;

            NhaCungCapLookupModel ncc = (NhaCungCapLookupModel)cboNhaCungCap.SelectedItem!;
            LapPhieuNhapInputModel model = new()
            {
                NgayNhap = dtpNgayNhap.Value,
                MaNhanVienLap = CurrentUser.MaNhanVien!.Value,
                MaNcc = ncc.MaNhaCungCap,
                TrangThai = trangThai,
                GhiChuChung = txtGhiChuChung.Text.Trim(),
                DanhSachSachNhap = _danhSachSach.Select(x => new SachNhapInputItem
                {
                    MaSach = x.MaSach,
                    MaSachText = x.MaSachText,
                    TenSach = x.TenSach,
                    TheLoai = x.TheLoai,
                    SoLuongNhap = x.SoLuongNhap,
                    DonGiaNhap = x.DonGiaNhap,
                    GhiChu = x.GhiChu
                }).ToList()
            };

            try
            {
                _nhapSachService.LapPhieuNhap(model);
                string moTaKho = trangThai == "Hoàn thành"
                    ? " và đã cập nhật các cuốn sách vào kho"
                    : "; chưa cập nhật số lượng kho";
                MessageBox.Show($"Đã lưu phiếu {txtMaPhieu.Text} ({trangThai}){moTaKho}.",
                    "Lưu phiếu nhập thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể lưu phiếu nhập: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool KiemTraDuLieuTruocKhiLuu()
        {
            if (!CurrentUser.MaNhanVien.HasValue)
            {
                MessageBox.Show("Tài khoản hiện tại không gắn với nhân viên.", "Không thể lập phiếu",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (cboNhaCungCap.SelectedItem is not NhaCungCapLookupModel)
            {
                MessageBox.Show("Vui lòng chọn nhà cung cấp đang hoạt động.", "Thiếu thông tin",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (_danhSachSach.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn ít nhất một đầu sách thực tế.", "Thiếu sách nhập",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (_danhSachSach.Any(x => x.SoLuongNhap <= 0 || x.DonGiaNhap < 0))
            {
                MessageBox.Show("Số lượng phải lớn hơn 0 và đơn giá không được âm.", "Dữ liệu không hợp lệ",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (dtpNgayNhap.Value.Date > DateTime.Today)
            {
                MessageBox.Show("Ngày nhập không được lớn hơn ngày hiện tại.", "Ngày nhập không hợp lệ",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (txtGhiChuChung.Text.Length > 255)
            {
                MessageBox.Show("Ghi chú chung không được vượt quá 255 ký tự.", "Ghi chú quá dài",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtGhiChuChung.Focus();
                return false;
            }
            return true;
        }
    }
}
