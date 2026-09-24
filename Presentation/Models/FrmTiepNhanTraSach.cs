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
    public partial class FrmTiepNhanTraSach : Form
    {
        private int _maPhieuMuon;
        private readonly MuonTraService _muonTraService;
        private readonly List<SachTraInputItem> _danhSachSachTra = new();
        private decimal _tienPhatMoiNgay = DataLayer.Rules.MuonTraRules.TienPhatMoiNgayMacDinh;
        private decimal _tyLePhatHong;
        private decimal _tyLePhatMat;
        private bool _dangTai;

        /// <summary>
        /// Cho biết ít nhất một lần tiếp nhận sách đã được commit, kể cả tiền phạt còn chờ thu.
        /// </summary>
        public bool DaGhiNhanTraSach { get; private set; }

        public FrmTiepNhanTraSach()
        {
            InitializeComponent();
            _maPhieuMuon = 0;
            _muonTraService = new MuonTraService();
            KhoiTaoForm();
        }

        public FrmTiepNhanTraSach(int maPhieuMuon)
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

            Load += FrmTiepNhanTraSach_Load;

            btnTraCuuPhieu.Click += BtnTraCuuPhieu_Click;
            txtTraCuuPhieu.KeyDown += TxtTraCuuPhieu_KeyDown;

            dtpNgayTraThucTe.ValueChanged += DtpNgayTraThucTe_ValueChanged;
            txtGhiChuTra.TextChanged += TxtGhiChuTra_TextChanged;
            txtGhiChuTinhTrang.TextChanged += TxtGhiChuTinhTrang_TextChanged;

            dgvSachTra.CellValueChanged += DgvSachTra_CellValueChanged;
            dgvSachTra.CurrentCellDirtyStateChanged += DgvSachTra_CurrentCellDirtyStateChanged;
            dgvSachTra.ColumnHeaderMouseClick += DgvSachTra_ColumnHeaderMouseClick;

            btnXacNhanTraSach.Click += BtnXacNhanTraSach_Click;
            btnCloseBox.Click += (s, e) => Close();
        }

        private async void FrmTiepNhanTraSach_Load(object? sender, EventArgs e)
        {
            try
            {
                NapDanhSachNhanVien();
                QuyDinh quyDinh = _muonTraService.GetQuyDinhHienHanh();
                _tienPhatMoiNgay = quyDinh.TienPhatMoiNgay;
                _tyLePhatHong = quyDinh.TyLePhatHong;
                _tyLePhatMat = quyDinh.TyLePhatMat;
                dtpNgayTraThucTe.Value = DateTime.Today;

                if (_maPhieuMuon > 0)
                {
                    await TaiDuLieuPhieuMuonAsync(_maPhieuMuon);
                }
                else
                {
                    HienThiTrong();
                }

                CapNhatThongKeVaTongKet();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể khởi tạo form tiếp nhận trả sách: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void NapDanhSachNhanVien()
        {
            cboNhanVien.Items.Clear();
            cboNhanVien.Items.Add(string.IsNullOrWhiteSpace(CurrentUser.HoTen) ? "Chưa đăng nhập nhân viên" : CurrentUser.HoTen);
            cboNhanVien.SelectedIndex = 0;
        }

        private void DtpNgayTraThucTe_ValueChanged(object? sender, EventArgs e)
        {
            // Cập nhật lại ngày trả cho tất cả các dòng trong bảng
            DateOnly ngayTra = DateOnly.FromDateTime(dtpNgayTraThucTe.Value);
            foreach (var item in _danhSachSachTra)
            {
                item.NgayTraThucTe = ngayTra;
                // Tính lại phí trễ hạn (10.000đ / ngày quá hạn nếu trễ)
                int soNgayTre = Math.Max(0, ngayTra.DayNumber - item.HanTraDuKien.DayNumber);
                item.PhiTreHan = soNgayTre * _tienPhatMoiNgay;
            }

            HienThiGridSachTra();
        }

        private void TxtGhiChuTra_TextChanged(object? sender, EventArgs e)
        {
            int len = txtGhiChuTra.Text.Length;
            lblDemGhiChuTra.Text = $"{len}/255";
            lblDemGhiChuTra.ForeColor = len > 255 ? Color.Red : Color.Gray;
        }

        private void TxtGhiChuTinhTrang_TextChanged(object? sender, EventArgs e)
        {
            int len = txtGhiChuTinhTrang.Text.Length;
            lblDemNote.Text = $"{len}/255";
            lblDemNote.ForeColor = len > 255 ? Color.Red : Color.Gray;
        }

        private async void TxtTraCuuPhieu_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                await TraCuuPhieuMuonAsync();
            }
        }

        private async void BtnTraCuuPhieu_Click(object? sender, EventArgs e)
        {
            await TraCuuPhieuMuonAsync();
        }

        private async Task TraCuuPhieuMuonAsync()
        {
            string kw = txtTraCuuPhieu.Text.Trim();
            if (string.IsNullOrWhiteSpace(kw))
            {
                MessageBox.Show("Vui lòng nhập Mã phiếu mượn, Mã độc giả hoặc SĐT để tra cứu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                await using AppDbContext context = new();
                PhieuMuon? pm = await context.PhieuMuons
                    .AsNoTracking()
                    .Include(x => x.MaTheNavigation)
                        .ThenInclude(t => t.MaDocGiaNavigation)
                    .FirstOrDefaultAsync(x => x.MaPhieuMuon.ToString() == kw ||
                                              x.MaPhieuMuonHienThi == kw ||
                                              ("PM" + x.MaPhieuMuon.ToString("D6")) == kw ||
                                              x.MaTheNavigation.MaDocGiaNavigation.SoDienThoai == kw ||
                                              x.MaTheNavigation.MaDocGia.ToString() == kw);

                if (pm != null)
                {
                    await TaiDuLieuPhieuMuonAsync(pm.MaPhieuMuon);
                }
                else
                {
                    MessageBox.Show("Không tìm thấy phiếu mượn tương ứng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tra cứu phiếu mượn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
                    .Include(x => x.ChiTietMuons)
                        .ThenInclude(ct => ct.MaCuonSachNavigation)
                            .ThenInclude(cs => cs.MaSachNavigation)
                                .ThenInclude(s => s.SachTacGia)
                                    .ThenInclude(stg => stg.MaTacGiaNavigation)
                    .FirstOrDefaultAsync(x => x.MaPhieuMuon == maPhieuMuon);

                if (pm == null)
                {
                    MessageBox.Show("Không tìm thấy thông tin phiếu mượn.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                _maPhieuMuon = pm.MaPhieuMuon;
                DocGium docGia = pm.MaTheNavigation.MaDocGiaNavigation;

                lblMaPhieuMuon.Text = string.IsNullOrWhiteSpace(pm.MaPhieuMuonHienThi) ? $"PM{pm.MaPhieuMuon:D6}" : pm.MaPhieuMuonHienThi;
                lblNgayMuon.Text = pm.NgayMuon.ToString("dd/MM/yyyy");

                int tongSoNgay = Math.Max(1, (pm.HanTra.ToDateTime(TimeOnly.MinValue) - pm.NgayMuon.Date).Days);
                lblHanTraDuKien.Text = $"{pm.HanTra:dd/MM/yyyy} ({tongSoNgay} ngày)";

                lblHoTenDocGia.Text = $"Độc giả: {docGia.HoTen} (DG{docGia.MaDocGia:D6})";
                lblSdt.Text = string.IsNullOrWhiteSpace(docGia.SoDienThoai) ? "-" : docGia.SoDienThoai;
                lblEmail.Text = string.IsNullOrWhiteSpace(docGia.Email) ? "-" : docGia.Email;
                lblLop.Text = docGia.MaLopNavigation?.TenLop ?? "-";
                lblLoaiThe.Text = "Thẻ " + (docGia.LoaiDocGia?.ToLower() ?? "sinh viên");

                picDocGia.Image = DatabaseImageHelper.LoadReaderAvatar(docGia.AnhDaiDien, docGia.HoTen, 60, 60);

                _danhSachSachTra.Clear();
                DateOnly ngayTraThucTe = DateOnly.FromDateTime(dtpNgayTraThucTe.Value);

                foreach (var ct in pm.ChiTietMuons.Where(x => x.TrangThai == "Đang mượn" || x.TrangThai == "Quá hạn"))
                {
                    var s = ct.MaCuonSachNavigation.MaSachNavigation;
                    string tacGia = string.Join(", ", s.SachTacGia.Select(x => x.MaTacGiaNavigation.TenTacGia));
                    if (string.IsNullOrWhiteSpace(tacGia)) tacGia = "Nhiều tác giả";

                    int soNgayTre = Math.Max(0, ngayTraThucTe.DayNumber - pm.HanTra.DayNumber);
                    decimal phiTre = soNgayTre * _tienPhatMoiNgay;

                    _danhSachSachTra.Add(new SachTraInputItem
                    {
                        MaChiTietMuon = ct.MaChiTietMuon,
                        MaCuonSach = ct.MaCuonSach,
                        MaSach = s.MaSach,
                        MaSachText = s.MaSachHienThi ?? $"S{s.MaSach:D6}",
                        TenSach = s.TenSach,
                        TacGia = tacGia,
                        NgayMuon = DateOnly.FromDateTime(pm.NgayMuon),
                        HanTraDuKien = pm.HanTra,
                        NgayTraThucTe = ngayTraThucTe,
                        TinhTrangSach = "Tốt",
                        PhiTreHan = phiTre,
                        PhiHuHong = 0,
                        GiaSach = Math.Max(0, s.GiaBia ?? 0),
                        DaTra = true
                    });
                }

                HienThiGridSachTra();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể tải chi tiết phiếu mượn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _dangTai = false;
            }
        }

        private void HienThiTrong()
        {
            _maPhieuMuon = 0;
            lblMaPhieuMuon.Text = "-";
            lblNgayMuon.Text = "-";
            lblHanTraDuKien.Text = "-";

            lblHoTenDocGia.Text = "Chưa chọn phiếu";
            lblSdt.Text = "-";
            lblEmail.Text = "-";
            lblLop.Text = "-";
            lblLoaiThe.Text = "-";

            picDocGia.Image?.Dispose();
            picDocGia.Image = DatabaseImageHelper.LoadReaderAvatar(null, "?", 60, 60);

            _danhSachSachTra.Clear();
            dgvSachTra.Rows.Clear();
        }

        private void HienThiGridSachTra()
        {
            dgvSachTra.Rows.Clear();
            int stt = 1;
            foreach (var item in _danhSachSachTra)
            {
                int rowIndex = dgvSachTra.Rows.Add(
                    item.DaTra,
                    stt++,
                    item.MaSachText,
                    item.TenSach,
                    item.TacGia,
                    item.NgayMuon.ToString("dd/MM/yyyy"),
                    item.HanTraDuKien.ToString("dd/MM/yyyy"),
                    item.NgayTraThucTe.ToString("dd/MM/yyyy"),
                    item.TinhTrangSach,
                    item.DaTra ? $"{item.PhiTreHan:N0} đ" : "—",
                    "💬"
                );

                dgvSachTra.Rows[rowIndex].Tag = item;
                dgvSachTra.Rows[rowIndex].Cells["colTinhTrang"].ReadOnly = !item.DaTra;
                dgvSachTra.Rows[rowIndex].DefaultCellStyle.ForeColor = item.DaTra
                    ? Color.FromArgb(40, 50, 70)
                    : Color.FromArgb(150, 155, 165);
            }

            CapNhatThongKeVaTongKet();
        }

        private void DgvSachTra_CurrentCellDirtyStateChanged(object? sender, EventArgs e)
        {
            string? columnName = dgvSachTra.CurrentCell?.OwningColumn.Name;
            if (dgvSachTra.IsCurrentCellDirty && (columnName == "colTinhTrang" || columnName == "colChonTra"))
            {
                dgvSachTra.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void DgvSachTra_ColumnHeaderMouseClick(object? sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex != colChonTra.Index) return;

            bool chonTatCa = _danhSachSachTra.Any(x => !x.DaTra);
            foreach (SachTraInputItem item in _danhSachSachTra)
            {
                item.DaTra = chonTatCa;
            }
            HienThiGridSachTra();
        }

        private void DgvSachTra_CellValueChanged(object? sender, DataGridViewCellEventArgs e)
        {
            if (_dangTai || e.RowIndex < 0) return;

            DataGridViewRow row = dgvSachTra.Rows[e.RowIndex];
            if (row.Tag is not SachTraInputItem item) return;

            if (e.ColumnIndex == colChonTra.Index)
            {
                item.DaTra = Convert.ToBoolean(row.Cells["colChonTra"].Value ?? false);
                row.Cells["colTinhTrang"].ReadOnly = !item.DaTra;
                row.Cells["colPhiTreHan"].Value = item.DaTra ? $"{item.PhiTreHan:N0} đ" : "—";
                row.DefaultCellStyle.ForeColor = item.DaTra
                    ? Color.FromArgb(40, 50, 70)
                    : Color.FromArgb(150, 155, 165);
                CapNhatThongKeVaTongKet();
                return;
            }

            if (e.ColumnIndex == colTinhTrang.Index && item.DaTra)
            {
                string tt = row.Cells["colTinhTrang"].Value?.ToString() ?? "Tốt";
                item.TinhTrangSach = tt;

                item.PhiHuHong = tt switch
                {
                    "Rách / Hỏng" or "Rách nhẹ" => DataLayer.Rules.MuonTraRules.TinhTienPhatTheoTyLe(item.GiaSach, _tyLePhatHong),
                    "Mất sách" => DataLayer.Rules.MuonTraRules.TinhTienPhatTheoTyLe(item.GiaSach, _tyLePhatMat),
                    _ => 0m
                };

                CapNhatThongKeVaTongKet();
            }
        }

        private void CapNhatThongKeVaTongKet()
        {
            int tongDangMuon = _danhSachSachTra.Count;
            List<SachTraInputItem> sachDuocChon = _danhSachSachTra.Where(x => x.DaTra).ToList();
            int tongSachTra = sachDuocChon.Count;
            int sachChuaTra = tongDangMuon - tongSachTra;
            int sachTot = sachDuocChon.Count(x => x.TinhTrangSach == "Tốt");
            int sachHong = tongSachTra - sachTot;

            int pctTot = tongSachTra > 0 ? (int)Math.Round((double)sachTot / tongSachTra * 100) : 0;
            int pctHong = tongSachTra > 0 ? 100 - pctTot : 0;

            lblSoSachTot.Text = sachTot.ToString();
            lblPhanTramTot.Text = $"{pctTot}%";
            lblSoSachHong.Text = sachHong.ToString();
            lblPhanTramHong.Text = $"{pctHong}%";

            lblStatusHeader.Text = $"Đang mượn: {tongDangMuon} cuốn sách";
            lblSoSachTraCount.Text = $"{tongSachTra} cuốn";
            lblSoSachChuaTraCount.Text = $"{sachChuaTra} cuốn";
            colChonTra.HeaderText = tongSachTra == tongDangMuon && tongDangMuon > 0 ? "☑ Trả" : "☐ Trả";

            decimal phiTre = sachDuocChon.Sum(x => x.PhiTreHan);
            decimal phiHuHong = sachDuocChon.Sum(x => x.PhiHuHong);
            decimal tongTien = phiTre + phiHuHong;

            lblTongPhiTreHan.Text = $"Tổng số phí trễ hạn:  {phiTre:N0} đ";
            lblTkSoSachTra.Text = $"{tongSachTra} cuốn";
            lblTkSoSachThieu.Text = $"{sachChuaTra} cuốn";
            lblTkPhiTreHan.Text = $"{phiTre:N0} đ";
            lblTkPhiHuHong.Text = $"{phiHuHong:N0} đ";
            lblTkTongTien.Text = $"{tongTien:N0} đ";
            lblTkBangChu.Text = tongTien == 0 ? "(Bằng chữ: Không đồng)" : $"(Bằng chữ: {DocSoThanhChu(tongTien)})";
            btnXacNhanTraSach.Enabled = tongSachTra > 0;

            if (tongSachTra == 0)
            {
                lblSuccessTitle.Text = "Chưa chọn sách để trả";
                lblSuccessSub.Text = "Tích chọn ít nhất một cuốn sách.";
                pnlSuccessBox.FillColor = Color.FromArgb(255, 251, 235);
                pnlSuccessBox.BorderColor = Color.FromArgb(253, 230, 138);
            }
            else if (sachHong > 0)
            {
                lblSuccessTitle.Text = $"Có {sachHong} cuốn sách hư hỏng/mất";
                lblSuccessSub.Text = "Vui lòng thu tiền bồi thường.";
                pnlSuccessBox.FillColor = Color.FromArgb(254, 242, 242);
                pnlSuccessBox.BorderColor = Color.FromArgb(254, 202, 202);
            }
            else if (sachChuaTra > 0)
            {
                lblSuccessTitle.Text = $"Trả {tongSachTra}/{tongDangMuon} cuốn sách";
                lblSuccessSub.Text = $"Còn {sachChuaTra} cuốn tiếp tục mượn.";
                pnlSuccessBox.FillColor = Color.FromArgb(239, 246, 255);
                pnlSuccessBox.BorderColor = Color.FromArgb(191, 219, 254);
            }
            else
            {
                lblSuccessTitle.Text = $"Trả đủ {tongSachTra} cuốn sách";
                lblSuccessSub.Text = "Cảm ơn độc giả!";
                pnlSuccessBox.FillColor = Color.FromArgb(240, 253, 244);
                pnlSuccessBox.BorderColor = Color.FromArgb(187, 247, 208);
            }
        }


        private async void BtnXacNhanTraSach_Click(object? sender, EventArgs e)
        {
            if (!PermissionHelper.CanAdd("MUONTRA.TRA"))
            {
                MessageBox.Show("Bạn không có quyền tiếp nhận trả sách.", "Không đủ quyền", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_maPhieuMuon <= 0)
            {
                MessageBox.Show("Vui lòng tra cứu phiếu mượn trước khi xác nhận trả.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_danhSachSachTra.Count == 0)
            {
                MessageBox.Show("Không có sách nào trong danh sách trả.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!CurrentUser.MaNhanVien.HasValue)
            {
                MessageBox.Show("Phiên đăng nhập không có nhân viên hợp lệ. Vui lòng đăng nhập lại.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            List<SachTraInputItem> sachDuocChon = _danhSachSachTra.Where(x => x.DaTra).ToList();
            if (sachDuocChon.Count == 0)
            {
                MessageBox.Show("Vui lòng tích chọn ít nhất một cuốn sách cần trả.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            TiepNhanTraInputModel model = new()
            {
                MaPhieuMuon = _maPhieuMuon,
                NgayTraThucTe = dtpNgayTraThucTe.Value,
                MaNhanVienTiepNhan = CurrentUser.MaNhanVien.Value,
                GhiChu = txtGhiChuTra.Text.Trim(),
                GhiChuTinhTrang = txtGhiChuTinhTrang.Text.Trim(),
                DanhSachSachTra = sachDuocChon
            };

            try
            {
                TiepNhanTraResultModel ketQua = _muonTraService.TiepNhanTraSach(model);
                DaGhiNhanTraSach = true;
                bool daThanhToanPhat = XuLyTienPhatSauKhiTra(ketQua);
                if (!daThanhToanPhat)
                {
                    DialogResult = DialogResult.Cancel;
                    return;
                }

                MessageBox.Show($"Hoàn tất tiếp nhận trả sách!\nMã phiếu: {lblMaPhieuMuon.Text}\nSố sách đã trả: {ketQua.SoSachDaTra} cuốn\nSố sách còn mượn: {ketQua.SoSachConMuon} cuốn",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                if (ketQua.SoSachConMuon > 0)
                {
                    txtGhiChuTra.Clear();
                    txtGhiChuTinhTrang.Clear();
                    _dangTai = false;
                    await TaiDuLieuPhieuMuonAsync(_maPhieuMuon);
                }
                else
                {
                    DialogResult = DialogResult.OK;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể tiếp nhận trả sách: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private bool XuLyTienPhatSauKhiTra(TiepNhanTraResultModel ketQua)
        {
            if (!ketQua.CoTienPhat || !ketQua.MaPhieuPhat.HasValue)
            {
                return true;
            }

            string maPhieuPhatText = $"PP{ketQua.MaPhieuPhat.Value:D6}";
            MessageBox.Show(
                $"Sách đã được ghi nhận trả để dừng tính quá hạn.\n\n" +
                $"Phát sinh tiền phạt: {ketQua.TongTienPhat:N0} VNĐ\n" +
                $"Mã phiếu phạt: {maPhieuPhatText}\n\n" +
                "Phải thu tiền phạt trước khi hoàn tất giao dịch.",
                "Chờ thu tiền phạt", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            if (!PermissionHelper.CanEdit("MUONTRA.PHAT"))
            {
                MessageBox.Show(
                    $"Bạn không có quyền thu tiền phạt.\nPhiếu {maPhieuPhatText} vẫn CHƯA THANH TOÁN.\n" +
                    "Vui lòng chuyển cho nhân viên có quyền sử dụng nút Thu tiền phạt.",
                    "Chưa hoàn tất nghĩa vụ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            using FrmThuTienPhat frmThuTien = new(ketQua.MaPhieuPhat.Value);
            if (frmThuTien.ShowDialog(this) == DialogResult.OK)
            {
                return true;
            }

            MessageBox.Show(
                $"Sách đã được ghi nhận trả nhưng phiếu {maPhieuPhatText} vẫn CHƯA THANH TOÁN.\n" +
                "Độc giả không thể mượn sách mới cho đến khi nộp đủ tiền qua nút Thu tiền phạt.",
                "Chưa hoàn tất nghĩa vụ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        private static string DocSoThanhChu(decimal number)
        {
            long n = (long)number;
            if (n == 0) return "Không đồng";
            return $"{n:N0} đồng".Replace(",", ".");
        }

        private void pnlPhieuMuonInfo_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}

