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
    public partial class FrmSuaPhieuMuon : Form
    {
        private int _maPhieuMuon;
        private int _maDocGia;
        private readonly MuonTraService _muonTraService;
        private readonly List<SachSuaInputItem> _danhSachSach = new();
        private bool _dangTai;

        public FrmSuaPhieuMuon()
        {
            InitializeComponent();
            _maPhieuMuon = 0;
            _muonTraService = new MuonTraService();
            KhoiTaoForm();
        }

        public FrmSuaPhieuMuon(int maPhieuMuon)
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

            Load += FrmSuaPhieuMuon_Load;

            txtGhiChuPhieu.MaxLength = 255;
            txtGhiChuSua.MaxLength = 255;
            txtGhiChuPhieu.TextChanged += TxtGhiChuPhieu_TextChanged;
            txtGhiChuSua.TextChanged += TxtGhiChuSua_TextChanged;

            dtpNgayLapPhieu.ValueChanged += Dates_ValueChanged;
            dtpHanTra.ValueChanged += Dates_ValueChanged;

            dgvSachMuon.CellContentClick += DgvSachMuon_CellContentClick;
            btnThemSach.Click += BtnThemSach_Click;

            btnHuyBo.Click += (s, e) => Close();
            btnLuuThayDoi.Click += BtnLuuThayDoi_Click;
            btnCloseBox.Click += (s, e) => Close();
        }

        private async void FrmSuaPhieuMuon_Load(object? sender, EventArgs e)
        {
            try
            {
                NapDanhSachNhanVien();

                if (_maPhieuMuon > 0)
                {
                    await TaiDuLieuPhieuMuonAsync(_maPhieuMuon);
                }
                else
                {
                    HienThiTrong();
                }

                CapNhatQuyDinhMuon();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể khởi tạo form sửa phiếu mượn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void NapDanhSachNhanVien()
        {
            cboNhanVienLap.Items.Clear();
            string curUser = CurrentUser.HoTen;
            if (string.IsNullOrWhiteSpace(curUser)) curUser = "Nguyễn Văn An";

            cboNhanVienLap.Items.Add(curUser);
            cboNhanVienLap.Items.Add("Trần Thị Mai");
            cboNhanVienLap.Items.Add("Lê Văn Hùng");
            cboNhanVienLap.SelectedIndex = 0;
        }

        private void Dates_ValueChanged(object? sender, EventArgs e)
        {
            TimeSpan duration = dtpHanTra.Value.Date - dtpNgayLapPhieu.Value.Date;
            int days = Math.Max(1, (int)duration.TotalDays);
            lblNoteDuration.Text = $"({days} ngày)";
        }

        private void TxtGhiChuPhieu_TextChanged(object? sender, EventArgs e)
        {
            int len = txtGhiChuPhieu.Text.Length;
            lblDemGhiChuPhieu.Text = $"{len}/255";
            lblDemGhiChuPhieu.ForeColor = len > 255 ? Color.Red : Color.Gray;
        }

        private void TxtGhiChuSua_TextChanged(object? sender, EventArgs e)
        {
            int len = txtGhiChuSua.Text.Length;
            lblDemGhiChuSua.Text = $"{len}/255";
            lblDemGhiChuSua.ForeColor = len > 255 ? Color.Red : Color.Gray;
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
                                .ThenInclude(s => s.MaTheLoaiNavigation)
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
                _maDocGia = docGia.MaDocGia;

                txtMaPhieuMuon.Text = string.IsNullOrWhiteSpace(pm.MaPhieuMuonHienThi) ? $"PM{pm.MaPhieuMuon:D6}" : pm.MaPhieuMuonHienThi;
                dtpNgayLapPhieu.Value = pm.NgayMuon;
                dtpHanTra.Value = pm.HanTra.ToDateTime(TimeOnly.MinValue);
                txtGhiChuPhieu.Text = string.IsNullOrWhiteSpace(pm.GhiChu) ? "-" : pm.GhiChu;

                lblMaDocGiaHeader.Text = $"DG{docGia.MaDocGia:D6}";
                lblHoTen.Text = docGia.HoTen;
                lblNgaySinh.Text = docGia.NgaySinh?.ToString("dd/MM/yyyy") ?? "-";
                lblGioiTinh.Text = docGia.GioiTinh ?? "Nữ";
                lblSdt.Text = string.IsNullOrWhiteSpace(docGia.SoDienThoai) ? "-" : docGia.SoDienThoai;
                lblEmail.Text = string.IsNullOrWhiteSpace(docGia.Email) ? "-" : docGia.Email;
                lblLop.Text = docGia.MaLopNavigation?.TenLop ?? "-";
                lblLoaiThe.Text = "Thẻ " + (docGia.LoaiDocGia?.ToLower() ?? "sinh viên");

                picDocGia.Image = DatabaseImageHelper.LoadReaderAvatar(docGia.AnhDaiDien, docGia.HoTen, 85, 85);

                _danhSachSach.Clear();
                foreach (var ct in pm.ChiTietMuons)
                {
                    var s = ct.MaCuonSachNavigation.MaSachNavigation;
                    string tacGia = string.Join(", ", s.SachTacGia.Select(x => x.MaTacGiaNavigation.TenTacGia));
                    if (string.IsNullOrWhiteSpace(tacGia)) tacGia = "Nhiều tác giả";

                    string theLoai = s.MaTheLoaiNavigation?.TenTheLoai ?? "Khác";

                    _danhSachSach.Add(new SachSuaInputItem
                    {
                        MaChiTietMuon = ct.MaChiTietMuon,
                        MaCuonSach = ct.MaCuonSach,
                        MaSach = s.MaSach,
                        MaSachText = s.MaSachHienThi ?? $"S{s.MaSach:D6}",
                        TenSach = s.TenSach,
                        TacGia = tacGia,
                        TheLoai = theLoai,
                        MaCuonSachText = ct.MaCuonSachNavigation.MaCuonSachHienThi ?? $"C{ct.MaCuonSach:D5}",
                        NgayMuon = DateOnly.FromDateTime(pm.NgayMuon),
                        HanTraDuKien = pm.HanTra,
                        TrangThai = ct.TrangThai
                    });
                }

                HienThiGridSach();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể tải thông tin phiếu mượn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            txtMaPhieuMuon.Text = "-";
            dtpNgayLapPhieu.Value = DateTime.Today;
            dtpHanTra.Value = DateTime.Today;
            txtGhiChuPhieu.Text = "-";

            lblMaDocGiaHeader.Text = "CHƯA CHỌN";
            lblHoTen.Text = "-";
            lblNgaySinh.Text = "-";
            lblGioiTinh.Text = "-";
            lblSdt.Text = "-";
            lblEmail.Text = "-";
            lblLop.Text = "-";
            lblLoaiThe.Text = "-";

            picDocGia.Image?.Dispose();
            picDocGia.Image = DatabaseImageHelper.LoadReaderAvatar(null, "?", 85, 85);

            _danhSachSach.Clear();
            dgvSachMuon.Rows.Clear();
        }

        private void HienThiGridSach()
        {
            dgvSachMuon.Rows.Clear();
            int stt = 1;
            foreach (var item in _danhSachSach)
            {
                dgvSachMuon.Rows.Add(
                    stt++,
                    item.MaSachText,
                    item.TenSach,
                    item.TacGia,
                    item.TheLoai,
                    item.MaCuonSachText,
                    item.NgayMuon.ToString("dd/MM/yyyy"),
                    item.HanTraDuKien.ToString("dd/MM/yyyy"),
                    item.TrangThai,
                    "📝",
                    "🗑"
                );
            }

            CapNhatQuyDinhMuon();
        }

        private void CapNhatQuyDinhMuon()
        {
            int currentCount = _danhSachSach.Count;
            int remainCount = Math.Max(0, 3 - currentCount);

            lblDangMuonCount.Text = $"{currentCount} cuốn";
            lblSoSachMuonThem.Text = $"{remainCount} cuốn";
            lblSoSachMuonThem.ForeColor = remainCount == 0 ? Color.FromArgb(220, 38, 38) : Color.FromArgb(22, 163, 74);
            btnThemSach.Enabled = remainCount > 0;
        }

        private void DgvSachMuon_CellContentClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= _danhSachSach.Count) return;

            if (e.ColumnIndex == colXoaRow.Index)
            {
                SachSuaInputItem item = _danhSachSach[e.RowIndex];
                if (!DataLayer.Rules.MuonTraRules.LaDangMuon(item.TrangThai))
                {
                    MessageBox.Show("Không thể xóa sách đã trả, mất hoặc hỏng khỏi phiếu.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (MessageBox.Show($"Xóa '{item.TenSach}' khỏi phiếu mượn?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
                _danhSachSach.RemoveAt(e.RowIndex);
                HienThiGridSach();
            }
        }
        private async void BtnThemSach_Click(object? sender, EventArgs e)
        {
            if (_danhSachSach.Count >= 3)
            {
                MessageBox.Show("Phiếu mượn đã đủ tối đa 3 cuốn.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                await using AppDbContext context = new();
                var existingBookIds = _danhSachSach.Select(x => x.MaSach).ToHashSet();
                var copies = await context.CuonSaches.AsNoTracking()
                    .Include(x => x.MaSachNavigation).ThenInclude(x => x.MaTheLoaiNavigation)
                    .Include(x => x.MaSachNavigation).ThenInclude(x => x.SachTacGia).ThenInclude(x => x.MaTacGiaNavigation)
                    .Where(x => x.TrangThai == "Có sẵn" && !existingBookIds.Contains(x.MaSach))
                    .OrderBy(x => x.MaSachNavigation.TenSach).ThenBy(x => x.MaCuonSach)
                    .Take(30).ToListAsync();

                if (copies.Count == 0)
                {
                    MessageBox.Show("Không còn cuốn sách phù hợp để thêm. Mỗi phiếu chỉ được mượn một cuốn trên mỗi đầu sách.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                using ContextMenuStrip menu = new() { Font = new Font("Segoe UI", 9F) };
                foreach (var copy in copies)
                {
                    var book = copy.MaSachNavigation;
                    string authors = string.Join(", ", book.SachTacGia.Select(x => x.MaTacGiaNavigation.TenTacGia));
                    var selected = new SachSuaInputItem
                    {
                        MaCuonSach = copy.MaCuonSach,
                        MaSach = book.MaSach,
                        MaSachText = book.MaSachHienThi ?? $"S{book.MaSach:D6}",
                        TenSach = book.TenSach,
                        TacGia = string.IsNullOrWhiteSpace(authors) ? "Nhiều tác giả" : authors,
                        TheLoai = book.MaTheLoaiNavigation?.TenTheLoai ?? "Khác",
                        MaCuonSachText = copy.MaCuonSachHienThi ?? $"C{copy.MaCuonSach:D5}",
                        NgayMuon = DateOnly.FromDateTime(dtpNgayLapPhieu.Value),
                        HanTraDuKien = DateOnly.FromDateTime(dtpHanTra.Value),
                        TrangThai = "Đang mượn"
                    };
                    ToolStripMenuItem option = new($"{selected.MaCuonSachText}  •  {selected.TenSach}  •  {selected.TacGia}");
                    option.Click += (_, _) => { _danhSachSach.Add(selected); HienThiGridSach(); };
                    menu.Items.Add(option);
                }
                menu.Show(btnThemSach, new Point(0, btnThemSach.Height));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể tải danh sách sách có sẵn: " + ex.Message, "Lỗi dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void BtnLuuThayDoi_Click(object? sender, EventArgs e)
        {
            if (_maPhieuMuon <= 0)
            {
                MessageBox.Show("Vui lòng chọn phiếu mượn cần lưu thay đổi.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_danhSachSach.Count == 0)
            {
                MessageBox.Show("Phiếu mượn phải chứa ít nhất 1 cuốn sách.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dtpHanTra.Value.Date < dtpNgayLapPhieu.Value.Date)
            {
                MessageBox.Show("Hạn trả không được trước ngày lập phiếu.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpHanTra.Focus();
                return;
            }

            if (!CurrentUser.MaNhanVien.HasValue)
            {
                MessageBox.Show("Phiên đăng nhập không có nhân viên hợp lệ. Vui lòng đăng nhập lại.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SuaPhieuMuonInputModel model = new()
            {
                MaPhieuMuon = _maPhieuMuon,
                MaDocGia = _maDocGia,
                NgayMuon = dtpNgayLapPhieu.Value,
                HanTra = DateOnly.FromDateTime(dtpHanTra.Value),
                MaNhanVienLap = CurrentUser.MaNhanVien.Value,
                GhiChuPhieu = txtGhiChuPhieu.Text.Trim(),
                GhiChuSua = txtGhiChuSua.Text.Trim(),
                DanhSachSach = _danhSachSach
            };

            try
            {
                _muonTraService.SuaPhieuMuon(model);
                MessageBox.Show($"Cập nhật thông tin phiếu mượn {txtMaPhieuMuon.Text} thành công!",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể cập nhật phiếu mượn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void pnlDocGiaInfo_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}

