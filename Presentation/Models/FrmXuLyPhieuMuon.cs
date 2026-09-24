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
    public partial class FrmXuLyPhieuMuon : Form
    {
        private int _maPhieuMuon;
        private int _maDocGia;
        private readonly MuonTraService _muonTraService;
        private readonly List<SachXuLyInputItem> _danhSachSach = new();
        private decimal _tienPhatMoiNgay = DataLayer.Rules.MuonTraRules.TienPhatMoiNgayMacDinh;
        private bool _dangTai;

        public FrmXuLyPhieuMuon()
        {
            InitializeComponent();
            _maPhieuMuon = 0;
            _muonTraService = new MuonTraService();
            KhoiTaoForm();
        }

        public FrmXuLyPhieuMuon(int maPhieuMuon)
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

            Load += FrmXuLyPhieuMuon_Load;

            cboLoaiXuLy.SelectedIndexChanged += CboLoaiXuLy_SelectedIndexChanged;
            cboTinhTrangChung.SelectedIndexChanged += CboTinhTrangChung_SelectedIndexChanged;
            dtpNgayTraThucTe.ValueChanged += DtpNgayTraThucTe_ValueChanged;
            txtGhiChuXuLy.TextChanged += TxtGhiChuXuLy_TextChanged;

            dgvSachXuLy.CellValueChanged += DgvSachXuLy_CellValueChanged;
            dgvSachXuLy.CurrentCellDirtyStateChanged += DgvSachXuLy_CurrentCellDirtyStateChanged;

            btnHuyBo.Click += (s, e) => Close();
            btnLuuKetQuaXuLy.Click += BtnLuuKetQuaXuLy_Click;
            btnCloseBox.Click += (s, e) => Close();
        }

        private async void FrmXuLyPhieuMuon_Load(object? sender, EventArgs e)
        {
            try
            {
                NapDanhSachLoaiXuLy();
                NapDanhSachTinhTrangChung();
                _tienPhatMoiNgay = _muonTraService.GetQuyDinhHienHanh().TienPhatMoiNgay;

                dtpNgayTraThucTe.Value = DateTime.Today;

                if (_maPhieuMuon > 0)
                {
                    await TaiDuLieuPhieuMuonAsync(_maPhieuMuon);
                }
                else
                {
                    HienThiTrong();
                }

                CapNhatThongTinPhat();
                NapTimelineHistory();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể khởi tạo form xử lý phiếu mượn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void NapDanhSachLoaiXuLy()
        {
            cboLoaiXuLy.Items.Clear();
            cboLoaiXuLy.Items.AddRange(new object[] { "Trả sách", "Báo hỏng sách", "Báo mất sách", "Gia hạn sách" });
            cboLoaiXuLy.SelectedIndex = 0;
        }

        private void NapDanhSachTinhTrangChung()
        {
            cboTinhTrangChung.Items.Clear();
            cboTinhTrangChung.Items.AddRange(new object[] { "Bình thường", "Bị hỏng nhẹ", "Bị hỏng nặng", "Mất sách" });
            cboTinhTrangChung.SelectedIndex = 0;
        }

        private void CboLoaiXuLy_SelectedIndexChanged(object? sender, EventArgs e)
        {
            string sel = cboLoaiXuLy.SelectedItem?.ToString() ?? "Trả sách";
            if (sel == "Báo hỏng sách") cboTinhTrangChung.SelectedItem = "Bị hỏng nhẹ";
            else if (sel == "Báo mất sách") cboTinhTrangChung.SelectedItem = "Mất sách";
        }

        private void CboTinhTrangChung_SelectedIndexChanged(object? sender, EventArgs e)
        {
            string tt = cboTinhTrangChung.SelectedItem?.ToString() ?? "Bình thường";
            foreach (var item in _danhSachSach)
            {
                item.TinhTrangKhiTra = tt;
            }
            HienThiGridSach();
        }

        private void DtpNgayTraThucTe_ValueChanged(object? sender, EventArgs e)
        {
            DateOnly ngayTra = DateOnly.FromDateTime(dtpNgayTraThucTe.Value);
            foreach (var item in _danhSachSach)
            {
                item.NgayTraThucTe = ngayTra;
            }
            HienThiGridSach();
        }

        private void TxtGhiChuXuLy_TextChanged(object? sender, EventArgs e)
        {
            int len = txtGhiChuXuLy.Text.Length;
            lblDemGhiChuXuLy.Text = $"{len}/255";
            lblDemGhiChuXuLy.ForeColor = len > 255 ? Color.Red : Color.Gray;
        }

        private async Task TaiDuLieuPhieuMuonAsync(int maPhieuMuon)
        {
            if (_dangTai) return;

            try
            {
                _dangTai = true;
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
                                .ThenInclude(s => s.SachTacGia)
                                    .ThenInclude(stg => stg.MaTacGiaNavigation)
                    .FirstOrDefaultAsync(x => x.MaPhieuMuon == maPhieuMuon);

                if (pm == null)
                {
                    MessageBox.Show("Không tìm thấy phiếu mượn.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                _maPhieuMuon = pm.MaPhieuMuon;
                DocGium docGia = pm.MaTheNavigation.MaDocGiaNavigation;
                _maDocGia = docGia.MaDocGia;

                lblMaPhieuMuon.Text = string.IsNullOrWhiteSpace(pm.MaPhieuMuonHienThi) ? $"PM{pm.MaPhieuMuon:D6}" : pm.MaPhieuMuonHienThi;
                lblNgayLap.Text = pm.NgayMuon.ToString("dd/MM/yyyy HH:mm");
                lblNhanVienLap.Text = pm.MaNhanVienNavigation?.HoTen ?? "Nguyễn Văn An";

                int tongSoNgay = Math.Max(1, (pm.HanTra.ToDateTime(TimeOnly.MinValue) - pm.NgayMuon.Date).Days);
                lblHanTraDuKien.Text = $"{pm.HanTra:dd/MM/yyyy} ({tongSoNgay} ngày)";

                lblMaDocGia.Text = $"DG{docGia.MaDocGia:D6}";
                lblHoTen.Text = docGia.HoTen;
                lblNgaySinh.Text = docGia.NgaySinh?.ToString("dd/MM/yyyy") ?? "-";
                lblGioiTinh.Text = docGia.GioiTinh ?? "Nữ";
                lblSdt.Text = string.IsNullOrWhiteSpace(docGia.SoDienThoai) ? "-" : docGia.SoDienThoai;
                lblEmail.Text = string.IsNullOrWhiteSpace(docGia.Email) ? "-" : docGia.Email;
                lblLop.Text = docGia.MaLopNavigation?.TenLop ?? "-";
                lblLoaiThe.Text = "Thẻ " + (docGia.LoaiDocGia?.ToLower() ?? "sinh viên");
                lblDiaChi.Text = string.IsNullOrWhiteSpace(docGia.DiaChi) ? "-" : docGia.DiaChi;

                picDocGia.Image = DatabaseImageHelper.LoadReaderAvatar(docGia.AnhDaiDien, docGia.HoTen, 85, 85);

                _danhSachSach.Clear();
                DateOnly ngayTra = DateOnly.FromDateTime(dtpNgayTraThucTe.Value);

                foreach (var ct in pm.ChiTietMuons.Where(x => x.TrangThai == "Đang mượn" || x.TrangThai == "Quá hạn"))
                {
                    var s = ct.MaCuonSachNavigation.MaSachNavigation;
                    string tacGia = string.Join(", ", s.SachTacGia.Select(x => x.MaTacGiaNavigation.TenTacGia));
                    if (string.IsNullOrWhiteSpace(tacGia)) tacGia = "Nhiều tác giả";

                    _danhSachSach.Add(new SachXuLyInputItem
                    {
                        MaChiTietMuon = ct.MaChiTietMuon,
                        MaCuonSach = ct.MaCuonSach,
                        MaSach = s.MaSach,
                        MaSachText = s.MaSachHienThi ?? $"S{s.MaSach:D6}",
                        TenSach = s.TenSach,
                        TacGia = tacGia,
                        MaCuonSachText = ct.MaCuonSachNavigation.MaCuonSachHienThi ?? $"C{ct.MaCuonSach:D5}",
                        NgayMuon = DateOnly.FromDateTime(pm.NgayMuon),
                        HanTraDuKien = pm.HanTra,
                        NgayTraThucTe = ngayTra,
                        TinhTrangKhiTra = "Bình thường"
                    });
                }

                HienThiGridSach();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải thông tin phiếu mượn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _dangTai = false;
            }
        }

        private void HienThiTrong()
        {
            _maPhieuMuon = 0;
            _maDocGia = 0;

            lblMaPhieuMuon.Text = "-";
            lblNgayLap.Text = "-";
            lblNhanVienLap.Text = "-";
            lblHanTraDuKien.Text = "-";

            lblMaDocGia.Text = "-";
            lblHoTen.Text = "-";
            lblNgaySinh.Text = "-";
            lblGioiTinh.Text = "-";
            lblSdt.Text = "-";
            lblEmail.Text = "-";
            lblLop.Text = "-";
            lblLoaiThe.Text = "-";
            lblDiaChi.Text = "-";

            picDocGia.Image?.Dispose();
            picDocGia.Image = DatabaseImageHelper.LoadReaderAvatar(null, "?", 85, 85);

            _danhSachSach.Clear();
            dgvSachXuLy.Rows.Clear();
        }

        private void HienThiGridSach()
        {
            dgvSachXuLy.Rows.Clear();
            int stt = 1;
            foreach (var item in _danhSachSach)
            {
                string statusIcon = item.TinhTrangKhiTra switch
                {
                    "Bị hỏng nhẹ" => "⚠️",
                    "Bị hỏng nặng" => "🚫",
                    "Mất sách" => "❌",
                    _ => "✔"
                };

                dgvSachXuLy.Rows.Add(
                    stt++,
                    item.MaSachText,
                    item.TenSach,
                    item.TacGia,
                    item.MaCuonSachText,
                    item.NgayMuon.ToString("dd/MM/yyyy"),
                    item.HanTraDuKien.ToString("dd/MM/yyyy"),
                    item.NgayTraThucTe.ToString("dd/MM/yyyy"),
                    item.TinhTrangKhiTra,
                    item.SoNgayQuaHan,
                    statusIcon
                );
            }

            CapNhatThongTinPhat();
            NapTimelineHistory();
        }

        private void DgvSachXuLy_CurrentCellDirtyStateChanged(object? sender, EventArgs e)
        {
            if (dgvSachXuLy.IsCurrentCellDirty && dgvSachXuLy.CurrentCell?.OwningColumn.Name == "colTinhTrangKhiTra")
            {
                dgvSachXuLy.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void DgvSachXuLy_CellValueChanged(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < _danhSachSach.Count && e.ColumnIndex == colTinhTrangKhiTra.Index)
            {
                string tt = dgvSachXuLy.Rows[e.RowIndex].Cells["colTinhTrangKhiTra"].Value?.ToString() ?? "Bình thường";
                _danhSachSach[e.RowIndex].TinhTrangKhiTra = tt;
                HienThiGridSach();
            }
        }

        private void CapNhatThongTinPhat()
        {
            int sachQuaHan = _danhSachSach.Count(x => x.SoNgayQuaHan > 0);
            int tongNgayQuaHan = _danhSachSach.Sum(x => x.SoNgayQuaHan);
            decimal phiTreHan = tongNgayQuaHan * _tienPhatMoiNgay;

            decimal phiHuHong = 0;
            foreach (var item in _danhSachSach)
            {
                if (item.TinhTrangKhiTra == "Bị hỏng nhẹ") phiHuHong += 20000m;
                else if (item.TinhTrangKhiTra == "Bị hỏng nặng") phiHuHong += 50000m;
                else if (item.TinhTrangKhiTra == "Mất sách") phiHuHong += 150000m;
            }

            decimal tongTien = phiTreHan + phiHuHong;

            lblTongSachQuaHan.Text = $"{sachQuaHan} cuốn";
            lblTongNgayQuaHan.Text = $"{tongNgayQuaHan} ngày";
            lblPhiTreHan.Text = $"{phiTreHan:N0} đ";
            lblPhiHuHong.Text = $"{phiHuHong:N0} đ";
            lblTongTienPhat.Text = $"{tongTien:N0} đ";

            if (tongTien == 0)
            {
                lblNoFineText.Text = "Độc giả không có khoản phạt nào.";
                pnlNoFineNotice.FillColor = Color.FromArgb(240, 253, 244);
                pnlNoFineNotice.BorderColor = Color.FromArgb(187, 247, 208);
                iconFineCheck.IconColor = Color.FromArgb(22, 163, 74);
            }
            else
            {
                lblNoFineText.Text = $"Độc giả có phát sinh phí phạt: {tongTien:N0} VNĐ";
                pnlNoFineNotice.FillColor = Color.FromArgb(254, 242, 242);
                pnlNoFineNotice.BorderColor = Color.FromArgb(254, 202, 202);
                iconFineCheck.IconColor = Color.FromArgb(220, 38, 38);
            }
        }

        private void NapTimelineHistory()
        {
            flpTimelineHistory.Controls.Clear();
            string curUser = string.IsNullOrWhiteSpace(CurrentUser.HoTen) ? "Chưa đăng nhập" : CurrentUser.HoTen;

            int total = _danhSachSach.Count;
            int count = 1;

            foreach (var item in _danhSachSach)
            {
                Panel pnlItem = new()
                {
                    Size = new Size(370, 52),
                    Margin = new Padding(0, 0, 0, 8),
                    BackColor = Color.White
                };

                Label lblDot = new()
                {
                    Text = "●",
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    ForeColor = item.TinhTrangKhiTra == "Bình thường" ? Color.FromArgb(22, 163, 74) : Color.FromArgb(217, 119, 6),
                    Location = new Point(0, 0),
                    AutoSize = true
                };

                Label lblTimeText = new()
                {
                    Text = dtpNgayTraThucTe.Value.ToString("dd/MM/yyyy HH:mm"),
                    Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(60, 70, 90),
                    Location = new Point(20, 2),
                    AutoSize = true
                };

                Label lblDetail = new()
                {
                    Text = $"Trả {count++}/{total} sách: {item.TenSach} ({item.MaCuonSachText})\n- Tình trạng: {item.TinhTrangKhiTra}\n- Xử lý bởi: {curUser}",
                    Font = new Font("Segoe UI", 7.5F),
                    ForeColor = Color.FromArgb(80, 90, 110),
                    Location = new Point(20, 18),
                    Size = new Size(345, 32)
                };

                pnlItem.Controls.Add(lblDot);
                pnlItem.Controls.Add(lblTimeText);
                pnlItem.Controls.Add(lblDetail);

                flpTimelineHistory.Controls.Add(pnlItem);
            }
        }


        private void BtnLuuKetQuaXuLy_Click(object? sender, EventArgs e)
        {
            if (!PermissionHelper.CanEdit("MUONTRA.TRA"))
            {
                MessageBox.Show("Bạn không có quyền xử lý phiếu mượn.", "Không đủ quyền", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_maPhieuMuon <= 0)
            {
                MessageBox.Show("Vui lòng chọn phiếu mượn trước khi lưu xử lý.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string loaiXuLy = cboLoaiXuLy.SelectedItem?.ToString() ?? "Trả sách";
            if (loaiXuLy != "Gia hạn sách" && _danhSachSach.Count == 0)
            {
                MessageBox.Show("Không có sách đang mượn để xử lý.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!CurrentUser.MaNhanVien.HasValue)
            {
                MessageBox.Show("Phiên đăng nhập không có nhân viên hợp lệ. Vui lòng đăng nhập lại.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            XuLyPhieuMuonInputModel model = new()
            {
                MaPhieuMuon = _maPhieuMuon,
                MaDocGia = _maDocGia,
                LoaiXuLy = loaiXuLy,
                NgayTraThucTe = dtpNgayTraThucTe.Value,
                TinhTrangChung = cboTinhTrangChung.SelectedItem?.ToString() ?? "Bình thường",
                MaNhanVienXuLy = CurrentUser.MaNhanVien.Value,
                GhiChu = txtGhiChuXuLy.Text.Trim(),
                DanhSachSach = _danhSachSach
            };

            try
            {
                _muonTraService.XuLyPhieuMuon(model);
                MessageBox.Show($"Lưu kết quả xử lý phiếu mượn {lblMaPhieuMuon.Text} thành công!",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể lưu kết quả xử lý: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void pnlPhieuMuonInfo_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}

