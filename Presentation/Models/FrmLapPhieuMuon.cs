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
    public partial class FrmLapPhieuMuon : Form
    {
        private int _maDocGia;
        private string _tenDocGia = "Chưa chọn";
        private string _maDocGiaText = "DG000000";
        private readonly MuonTraService _muonTraService;
        private readonly SachService _sachService;
        private readonly List<SachMuonInputItem> _selectedBooks = new();
        private bool _dangTai;
        private int _soSachDangMuon;

        public FrmLapPhieuMuon()
        {
            InitializeComponent();
            _maDocGia = 0;
            _muonTraService = new MuonTraService();
            _sachService = new SachService();
            KhoiTaoForm();
        }

        public FrmLapPhieuMuon(int maDocGia)
        {
            InitializeComponent();
            _maDocGia = maDocGia;
            _muonTraService = new MuonTraService();
            _sachService = new SachService();
            KhoiTaoForm();
        }

        private void KhoiTaoForm()
        {
            if (DesignModeHelper.IsDesignMode(this)) return;

            KeyPreview = true;
            StartPosition = FormStartPosition.CenterScreen;

            Load += FrmLapPhieuMuon_Load;

            btnTimKiemDocGia.Click += async (_, _) => await TimVaChonDocGiaAsync();
            guna2TextBox1.KeyDown += async (_, e) =>
            {
                if (e.KeyCode != Keys.Enter) return;
                e.SuppressKeyPress = true;
                await TimVaChonDocGiaAsync();
            };

            btnTimKiemSach.Click += BtnTimKiemSach_Click;
            txtTimKiemSach.KeyDown += TxtTimKiemSach_KeyDown;
            cboTheLoai.SelectedIndexChanged += CboTheLoai_SelectedIndexChanged;

            txtGhiChuPhieu.MaxLength = 255;
            dtpNgayLap.ValueChanged += DtpNgayLap_ValueChanged;
            txtGhiChuPhieu.TextChanged += TxtGhiChuPhieu_TextChanged;

            dgvSachMuon.CellContentClick += DgvSachMuon_CellContentClick;
            dgvSachMuon.CellPainting += DgvSachMuon_CellPainting;

            btnXoaTatCa.Click += BtnXoaTatCa_Click;
            btnLuuTam.Click += BtnLuuTam_Click;
            btnXacNhanLapPhieu.Click += BtnXacNhanLapPhieu_Click;
            btnCloseBox.Click += (s, e) => Close();
        }

        private async void FrmLapPhieuMuon_Load(object? sender, EventArgs e)
        {
            try
            {
                NapDanhSachTheLoai();
                NapDanhSachNhanVien();

                dtpNgayLap.Value = DateTime.Today;
                dtpHanTraDuKien.Value = DateTime.Today.AddDays(14);
                lblTomTatMaPhieu.Text = _muonTraService.GetNextMaPhieuMuonHienThi();

                if (_maDocGia > 0)
                {
                    await TaiDuLieuDocGiaAsync(_maDocGia);
                }
                else
                {
                    HienThiChuaChonDocGia();
                }

                NapDanhSachSachCard();
                CapNhatTomTatPhieu();

                try
                {
                    await using AppDbContext context = new();
                    var docGiaIds = await context.DocGia.AsNoTracking().Where(x => x.TrangThai).Select(x => x.MaDocGia).ToListAsync();
                    var autoCompleteCollection = new AutoCompleteStringCollection();
                    foreach (var id in docGiaIds)
                    {
                        autoCompleteCollection.Add($"DG{id:D6}");
                    }
                    guna2TextBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                    guna2TextBox1.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    guna2TextBox1.AutoCompleteCustomSource = autoCompleteCollection;
                }
                catch { }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể khởi tạo form lập phiếu mượn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void NapDanhSachTheLoai()
        {
            cboTheLoai.Items.Clear();
            cboTheLoai.Items.Add("Tất cả thể loại");
            try
            {
                using AppDbContext context = new();
                var ds = context.TheLoais.AsNoTracking().Select(x => x.TenTheLoai).ToList();
                foreach (var tl in ds) cboTheLoai.Items.Add(tl);
            }
            catch
            {
                cboTheLoai.Items.AddRange(new object[] { "Tiểu thuyết", "Kỹ năng sống", "Kinh tế", "Công nghệ thông tin" });
            }
            cboTheLoai.SelectedIndex = 0;
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

        private void DtpNgayLap_ValueChanged(object? sender, EventArgs e)
        {
            dtpHanTraDuKien.Value = dtpNgayLap.Value.AddDays(14);
            lblTomTatHanTra.Text = dtpHanTraDuKien.Value.ToString("dd/MM/yyyy");
        }

        private void TxtGhiChuPhieu_TextChanged(object? sender, EventArgs e)
        {
            int len = txtGhiChuPhieu.Text.Length;
            lblDemGhiChu.Text = $"{len}/255";
            lblDemGhiChu.ForeColor = len > 255 ? Color.Red : Color.Gray;
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
                    HienThiChuaChonDocGia();
                    MessageBox.Show("Độc giả không tồn tại trong CSDL.", "Tra cứu độc giả",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                _tenDocGia = docGia.HoTen;
                _maDocGiaText = $"DG{docGia.MaDocGia:D6}";
                lblMaDocGiaHeader.Text = _maDocGiaText;
                lblHoTen.Text = docGia.HoTen;
                lblNgaySinh.Text = docGia.NgaySinh?.ToString("dd/MM/yyyy") ?? "-";
                lblGioiTinh.Text = docGia.GioiTinh ?? "-";
                lblDienThoai.Text = string.IsNullOrWhiteSpace(docGia.SoDienThoai) ? "-" : docGia.SoDienThoai;
                lblEmail.Text = string.IsNullOrWhiteSpace(docGia.Email) ? "-" : docGia.Email;
                lblLop.Text = docGia.MaLopNavigation?.TenLop ?? "-";
                lblKhoa.Text = docGia.MaLopNavigation?.MaKhoaNavigation?.TenKhoa ?? "-";
                lblDiaChi.Text = string.IsNullOrWhiteSpace(docGia.DiaChi) ? "-" : docGia.DiaChi;
                picDocGia.Image?.Dispose();
                picDocGia.Image = DatabaseImageHelper.LoadReaderAvatar(docGia.AnhDaiDien, docGia.HoTen, 85, 85);

                bool theHetHan = docGia.TheDocGium != null && docGia.TheDocGium.NgayHetHan < DateOnly.FromDateTime(DateTime.Today);
                bool theKhoa = docGia.TheDocGium != null && docGia.TheDocGium.TrangThai.Contains("khóa", StringComparison.CurrentCultureIgnoreCase);
                bool nienPhatChuaTra = await context.PhieuPhats.AnyAsync(x => x.MaDocGia == docGia.MaDocGia && x.TrangThai != "Đã thanh toán" && x.TrangThai != "Đã hủy");
                int dangMuonCount = await context.ChiTietMuons.CountAsync(x => x.MaPhieuMuonNavigation.MaTheNavigation.MaDocGia == docGia.MaDocGia && (x.TrangThai == "Đang mượn" || x.TrangThai == "Quá hạn"));
                _soSachDangMuon = dangMuonCount;

                bool theHopLe = docGia.TrangThai && docGia.TheDocGium != null && !theKhoa && !theHetHan && !nienPhatChuaTra && dangMuonCount < 3;
                _maDocGia = theHopLe ? docGia.MaDocGia : 0;

                if (theHetHan)
                {
                    btnTrangThaiDocGia.Text = "Thẻ hết hạn";
                    btnTrangThaiDocGia.FillColor = Color.FromArgb(254, 226, 226);
                    btnTrangThaiDocGia.ForeColor = Color.FromArgb(220, 38, 38);
                    MessageBox.Show("Thẻ độc giả đã hết hạn sử dụng.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (nienPhatChuaTra)
                {
                    btnTrangThaiDocGia.Text = "Nợ tiền phạt";
                    btnTrangThaiDocGia.FillColor = Color.FromArgb(254, 226, 226);
                    btnTrangThaiDocGia.ForeColor = Color.FromArgb(220, 38, 38);
                    MessageBox.Show("Độc giả phải nộp tiền phạt trước khi mượn sách.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (dangMuonCount >= 3)
                {
                    btnTrangThaiDocGia.Text = "Đã mượn 3/3";
                    btnTrangThaiDocGia.FillColor = Color.FromArgb(254, 226, 226);
                    btnTrangThaiDocGia.ForeColor = Color.FromArgb(220, 38, 38);
                    MessageBox.Show("Độc giả đã mượn tối đa 3 cuốn sách.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                btnTrangThaiDocGia.Text = theHopLe ? "Thẻ còn hiệu lực" :
                    docGia.TheDocGium == null ? "Chưa cấp thẻ" : "Thẻ bị khóa";
                btnTrangThaiDocGia.FillColor = theHopLe
                    ? Color.FromArgb(209, 250, 223)
                    : Color.FromArgb(254, 226, 226);
                btnTrangThaiDocGia.ForeColor = theHopLe
                    ? Color.FromArgb(2, 122, 72)
                    : Color.FromArgb(220, 38, 38);
                btnTrangThaiDocGia.DisabledState.FillColor = btnTrangThaiDocGia.FillColor;
                btnTrangThaiDocGia.DisabledState.ForeColor = btnTrangThaiDocGia.ForeColor;
                CapNhatTomTatPhieu();

                if (!theHopLe)
                {
                    MessageBox.Show("Độc giả này không đủ điều kiện mượn sách. Vui lòng chọn độc giả có thẻ còn hiệu lực.",
                        "Không thể chọn độc giả", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể tải thông tin độc giả: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _dangTai = false;
            }
        }

        private async Task TimVaChonDocGiaAsync()
        {
            string keyword = guna2TextBox1.Text.Trim();
            if (string.IsNullOrWhiteSpace(keyword))
            {
                MessageBox.Show("Vui lòng nhập Mã độc giả.", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                guna2TextBox1.Focus();
                return;
            }

            try
            {
                await using AppDbContext context = new();
                string maText = keyword.StartsWith("DG", StringComparison.OrdinalIgnoreCase)
                    ? keyword[2..].TrimStart('0')
                    : keyword;
                int.TryParse(maText, out int maDocGia);
                List<DocGium> ketQua = await context.DocGia.AsNoTracking()
                    .Where(x => x.TrangThai &&
                        (x.MaDocGia == maDocGia || x.HoTen.Contains(keyword) ||
                         (x.MaSinhVien != null && x.MaSinhVien.Contains(keyword)) ||
                         (x.SoDienThoai != null && x.SoDienThoai.Contains(keyword))))
                    .OrderBy(x => x.HoTen)
                    .Take(20)
                    .ToListAsync();

                if (ketQua.Count == 0)
                {
                    MessageBox.Show("Độc giả không tồn tại trong CSDL.", "Tra cứu độc giả",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (ketQua.Count == 1)
                {
                    await TaiDuLieuDocGiaAsync(ketQua[0].MaDocGia);
                    return;
                }

                using ContextMenuStrip menu = new();
                menu.Font = new Font("Segoe UI", 9F);
                foreach (DocGium docGia in ketQua)
                {
                    ToolStripMenuItem item = new($"DG{docGia.MaDocGia:D6}  •  {docGia.HoTen}  •  {docGia.MaSinhVien ?? docGia.SoDienThoai ?? "-"}")
                    {
                        Tag = docGia.MaDocGia
                    };
                    item.Click += async (_, _) => await TaiDuLieuDocGiaAsync((int)item.Tag!);
                    menu.Items.Add(item);
                }
                menu.Show(guna2TextBox1, new Point(0, guna2TextBox1.Height));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể tra cứu độc giả: " + ex.Message, "Lỗi dữ liệu",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void HienThiChuaChonDocGia()
        {
            _maDocGia = 0;
            _tenDocGia = "Chưa chọn độc giả";
            _maDocGiaText = "-";
            lblMaDocGiaHeader.Text = "CHƯA CHỌN";
            lblHoTen.Text = lblNgaySinh.Text = lblGioiTinh.Text = lblDienThoai.Text = "-";
            lblEmail.Text = lblLop.Text = lblKhoa.Text = lblDiaChi.Text = "-";
            btnTrangThaiDocGia.Text = "Chưa chọn thẻ";
            btnTrangThaiDocGia.FillColor = Color.FromArgb(241, 245, 249);
            btnTrangThaiDocGia.ForeColor = Color.FromArgb(100, 116, 139);
            btnTrangThaiDocGia.DisabledState.FillColor = btnTrangThaiDocGia.FillColor;
            btnTrangThaiDocGia.DisabledState.ForeColor = btnTrangThaiDocGia.ForeColor;
            picDocGia.Image?.Dispose();
            picDocGia.Image = DatabaseImageHelper.LoadReaderAvatar(null, "?", 85, 85);
            CapNhatTomTatPhieu();
        }

        private void TxtTimKiemSach_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                NapDanhSachSachCard();
            }
        }

        private void BtnTimKiemSach_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTimKiemSach.Text))
            {
                MessageBox.Show("Vui lòng nhập Mã cuốn sách.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            NapDanhSachSachCard();
        }

        private void CboTheLoai_SelectedIndexChanged(object? sender, EventArgs e)
        {
            NapDanhSachSachCard();
        }

        private void NapDanhSachSachCard()
        {
            flowBookCards.SuspendLayout();
            flowBookCards.Controls.Clear();

            string kw = txtTimKiemSach.Text.Trim();
            string tl = cboTheLoai.SelectedItem?.ToString() ?? "Tất cả thể loại";

            List<SachMuonInputItem> dsSach = _muonTraService.GetDanhSachSachLookup(kw, tl);
            if (!string.IsNullOrWhiteSpace(kw) && dsSach.Count == 0)
            {
                MessageBox.Show("Mã cuốn sách không tồn tại.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Label lblEmpty = new()
                {
                    Text = "Mã cuốn sách không tồn tại.",
                    AutoSize = true,
                    Font = new Font("Segoe UI", 9F, FontStyle.Italic),
                    ForeColor = Color.Red,
                    Margin = new Padding(20, 40, 0, 0)
                };
                flowBookCards.Controls.Add(lblEmpty);
            }
            else if (dsSach.Count == 0)
            {
                Label lblEmpty = new()
                {
                    Text = "Không tìm thấy sách phù hợp.",
                    AutoSize = true,
                    Font = new Font("Segoe UI", 9F, FontStyle.Italic),
                    ForeColor = Color.Gray,
                    Margin = new Padding(20, 40, 0, 0)
                };
                flowBookCards.Controls.Add(lblEmpty);
            }
            else
            {
                foreach (var item in dsSach)
                {
                    Panel card = TaoCardSach(item);
                    flowBookCards.Controls.Add(card);
                }
            }

            flowBookCards.ResumeLayout();
        }

        private Panel TaoCardSach(SachMuonInputItem item)
        {
            Guna.UI2.WinForms.Guna2Panel pnl = new()
            {
                Size = new Size(162, 122),
                BorderColor = Color.FromArgb(226, 230, 239),
                BorderRadius = 6,
                BorderThickness = 1,
                FillColor = Color.White,
                Margin = new Padding(0, 0, 10, 0)
            };

            // Thumbnail
            Guna.UI2.WinForms.Guna2PictureBox pic = new()
            {
                Size = new Size(42, 54),
                Location = new Point(8, 8),
                BorderRadius = 4,
                SizeMode = PictureBoxSizeMode.Zoom,
                FillColor = Color.FromArgb(238, 242, 248)
            };

            pic.Image = BookCoverImageHelper.LoadForGrid(item.AnhBia, item.MaSach, 42, 54);

            // MS Code
            Label lblMS = new()
            {
                Text = $"MS: {item.MaSachText}",
                Font = new Font("Segoe UI", 7F),
                ForeColor = Color.FromArgb(120, 130, 145),
                Location = new Point(54, 8),
                AutoSize = true
            };

            // Title
            Label lblTitle = new()
            {
                Text = item.TenSach,
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 40, 60),
                Location = new Point(54, 22),
                Size = new Size(100, 30)
            };

            // Author
            Label lblAuthor = new()
            {
                Text = item.TacGia,
                Font = new Font("Segoe UI", 7.5F),
                ForeColor = Color.FromArgb(100, 110, 125),
                Location = new Point(54, 48),
                Size = new Size(100, 16)
            };

            // Count
            Label lblCount = new()
            {
                Text = $"SL còn: {item.SoLuongCon} / {item.TongSoLuong}",
                Font = new Font("Segoe UI", 7.5F, FontStyle.Bold),
                ForeColor = item.SoLuongCon > 0 ? Color.FromArgb(0, 82, 204) : Color.Red,
                Location = new Point(8, 66),
                AutoSize = true
            };

            // Button + Thêm
            Guna.UI2.WinForms.Guna2Button btnAdd = new()
            {
                Text = "+ Thêm",
                Size = new Size(146, 26),
                Location = new Point(8, 88),
                BorderRadius = 4,
                BorderColor = Color.FromArgb(0, 82, 204),
                BorderThickness = 1,
                FillColor = Color.FromArgb(240, 246, 255),
                ForeColor = Color.FromArgb(0, 82, 204),
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnAdd.Click += (s, e) => ThemSachVaoDanhSach(item);

            pnl.Controls.Add(pic);
            pnl.Controls.Add(lblMS);
            pnl.Controls.Add(lblTitle);
            pnl.Controls.Add(lblAuthor);
            pnl.Controls.Add(lblCount);
            pnl.Controls.Add(btnAdd);

            return pnl;
        }

        private void ThemSachVaoDanhSach(SachMuonInputItem item)
        {
            // TC06: Check borrowed limit (already borrowed + currently selected)
            int currentTotal = _selectedBooks.Sum(x => x.SoLuong);
            if (_soSachDangMuon + currentTotal >= 3)
            {
                MessageBox.Show("Độc giả đã mượn tối đa 3 cuốn sách.", "Cảnh báo quy định", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Rule Check 3: Unique book titles
            if (_selectedBooks.Any(x => x.MaSach == item.MaSach))
            {
                MessageBox.Show("Theo quy định, độc giả không được mượn 2 cuốn thuộc cùng một đầu sách.", "Cảnh báo quy định", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Check book status (TC09, TC10, TC11)
            using (AppDbContext db = new())
            {
                var copyStatuses = db.CuonSaches.AsNoTracking().Where(x => x.MaSach == item.MaSach).Select(x => x.TrangThai).ToList();
                if (copyStatuses.Count > 0 && copyStatuses.All(st => string.Equals(st, "Mất", StringComparison.OrdinalIgnoreCase)))
                {
                    MessageBox.Show("Cuốn sách này đã bị báo mất.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (copyStatuses.Count > 0 && copyStatuses.All(st => string.Equals(st, "Hư hỏng", StringComparison.OrdinalIgnoreCase) || string.Equals(st, "Hỏng", StringComparison.OrdinalIgnoreCase)))
                {
                    MessageBox.Show("Cuốn sách này đã bị báo hư hỏng.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            if (item.SoLuongCon <= 0)
            {
                MessageBox.Show("Cuốn sách hiện không ở trạng thái sẵn sàng.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _selectedBooks.Add(new SachMuonInputItem
            {
                MaSach = item.MaSach,
                MaSachText = item.MaSachText,
                TenSach = item.TenSach,
                TacGia = item.TacGia,
                TheLoai = item.TheLoai,
                SoLuong = 1,
                NgayTraDuKien = DateOnly.FromDateTime(dtpHanTraDuKien.Value)
            });

            HienThiDanhSachMuonGrid();
        }

        private void HienThiDanhSachMuonGrid()
        {
            dgvSachMuon.Rows.Clear();
            int stt = 1;
            foreach (var item in _selectedBooks)
            {
                dgvSachMuon.Rows.Add(
                    stt++,
                    item.MaSachText,
                    item.TenSach,
                    item.TacGia,
                    item.TheLoai,
                    item.SoLuong,
                    dtpHanTraDuKien.Value.ToString("dd/MM/yyyy"),
                    string.Empty
                );
            }

            CapNhatTomTatPhieu();
        }

        private void DgvSachMuon_CellContentClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == colThaoTac.Index)
            {
                if (e.RowIndex < _selectedBooks.Count)
                {
                    _selectedBooks.RemoveAt(e.RowIndex);
                    HienThiDanhSachMuonGrid();
                }
            }
        }

        private void DgvSachMuon_CellPainting(object? sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex != colThaoTac.Index) return;

            e.PaintBackground(e.CellBounds, true);
            Color red = Color.FromArgb(220, 38, 38);
            using Font iconFont = new("Segoe UI Symbol", 13F, FontStyle.Regular);
            TextRenderer.DrawText(
                e.Graphics,
                "🗑",
                iconFont,
                e.CellBounds,
                red,
                TextFormatFlags.HorizontalCenter |
                TextFormatFlags.VerticalCenter |
                TextFormatFlags.NoPadding);
            e.Handled = true;
        }

        private void CapNhatTomTatPhieu()
        {
            int dauSach = _selectedBooks.Count;
            int tongSL = _selectedBooks.Sum(x => x.SoLuong);

            lblTongDauSach.Text = $"Tổng số đầu sách: {dauSach}";
            lblTongSoLuong.Text = $"Tổng số lượng: {tongSL} cuốn";

            lblTomTatDocGia.Text = $"{_tenDocGia} ({_maDocGiaText})";
            lblTomTatTongDauSach.Text = dauSach.ToString();
            lblTomTatTongSoLuong.Text = $"{tongSL} cuốn";
            lblTomTatHanTra.Text = dtpHanTraDuKien.Value.ToString("dd/MM/yyyy");
        }

        private void BtnXoaTatCa_Click(object? sender, EventArgs e)
        {
            LamMoiForm();
        }

        private void LamMoiForm()
        {
            _selectedBooks.Clear();
            _soSachDangMuon = 0;
            guna2TextBox1.Clear();
            txtTimKiemSach.Clear();
            if (cboTheLoai.Items.Count > 0) cboTheLoai.SelectedIndex = 0;
            dtpNgayLap.Value = DateTime.Today;
            dtpHanTraDuKien.Value = DateTime.Today.AddDays(14);
            txtGhiChuPhieu.Clear();
            HienThiChuaChonDocGia();
            HienThiDanhSachMuonGrid();
            NapDanhSachSachCard();
        }

        private void BtnLuuTam_Click(object? sender, EventArgs e)
        {
            MessageBox.Show("Đã lưu tạm thông tin phiếu mượn.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnXacNhanLapPhieu_Click(object? sender, EventArgs e)
        {
            if (!PermissionHelper.CanAdd("MUONTRA.MUON"))
            {
                MessageBox.Show("Bạn không có quyền lập phiếu mượn.", "Không đủ quyền", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!CurrentUser.MaNhanVien.HasValue)
            {
                MessageBox.Show("Phiên đăng nhập chưa liên kết với nhân viên. Vui lòng đăng nhập lại.", "Không thể lập phiếu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (_maDocGia <= 0)
            {
                MessageBox.Show("Vui lòng nhập Mã độc giả.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                guna2TextBox1.Focus();
                return;
            }

            if (_selectedBooks.Count == 0)
            {
                MessageBox.Show("Vui lòng nhập Mã cuốn sách.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTimKiemSach.Focus();
                return;
            }

            if (dtpHanTraDuKien.Value.Date < dtpNgayLap.Value.Date)
            {
                MessageBox.Show("Ngày hẹn trả phải sau Ngày mượn.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int days = (dtpHanTraDuKien.Value.Date - dtpNgayLap.Value.Date).Days;
            if (days > 14)
            {
                MessageBox.Show("Thời gian mượn tối đa là 14 ngày.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string maPhieuHienThi = lblTomTatMaPhieu.Text;

            LapPhieuMuonInputModel model = new()
            {
                MaDocGia = _maDocGia,
                MaNhanVien = CurrentUser.MaNhanVien.Value,
                NgayLap = dtpNgayLap.Value,
                HanTraDuKien = DateOnly.FromDateTime(dtpHanTraDuKien.Value),
                GhiChu = txtGhiChuPhieu.Text.Trim(),
                MaPhieuHienThi = maPhieuHienThi,
                DanhSachSach = _selectedBooks
            };

            try
            {
                _muonTraService.LapPhieuMuon(model);
                MessageBox.Show($"Lập phiếu mượn thành công!\nMã phiếu: {maPhieuHienThi}\nĐộc giả: {_tenDocGia}\nTổng số sách: {_selectedBooks.Count} cuốn",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                Exception loiGoc = ex;
                while (loiGoc.InnerException != null)
                {
                    loiGoc = loiGoc.InnerException;
                }

                MessageBox.Show("Không thể lập phiếu mượn: " + loiGoc.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private static Bitmap TaoAnhBiaMacDinh(string tenSach, int width, int height)
        {
            width = Math.Max(width, 30);
            height = Math.Max(height, 40);

            Bitmap bitmap = new(width, height);
            using Graphics g = Graphics.FromImage(bitmap);
            g.SmoothingMode = SmoothingMode.AntiAlias;

            using LinearGradientBrush background = new(
                new Rectangle(0, 0, width, height),
                Color.FromArgb(220, 226, 240),
                Color.FromArgb(190, 205, 235),
                45f);
            g.FillRectangle(background, 0, 0, width, height);

            string initial = string.IsNullOrWhiteSpace(tenSach) ? "S" : tenSach.Substring(0, 1).ToUpperInvariant();
            using Font font = new("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Pixel);
            using SolidBrush textBrush = new(Color.FromArgb(40, 60, 100));
            SizeF size = g.MeasureString(initial, font);
            g.DrawString(initial, font, textBrush,
                (width - size.Width) / 2f,
                (height - size.Height) / 2f);

            return bitmap;
        }

        private void pnlDocGia_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}

