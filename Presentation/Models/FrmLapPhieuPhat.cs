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
    public partial class FrmLapPhieuPhat : Form
    {
        private int _maDocGia;
        private int? _maPhieuMuon;
        private readonly MuonTraService _muonTraService;
        private readonly List<SachViPhamInputItem> _danhSachViPham = new();
        private decimal _tienPhatMoiNgay = DataLayer.Rules.MuonTraRules.TienPhatMoiNgayMacDinh;
        private bool _dangTai;

        public FrmLapPhieuPhat()
        {
            InitializeComponent();
            _maDocGia = 0;
            _muonTraService = new MuonTraService();
            KhoiTaoForm();
        }

        public FrmLapPhieuPhat(int maPhieuMuon)
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

            Load += FrmLapPhieuPhat_Load;

            btnTraCuuDocGia.Click += BtnTraCuuDocGia_Click;
            txtTraCuuDocGia.KeyDown += TxtTraCuuDocGia_KeyDown;

            txtGhiChuPhieu.MaxLength = 255;
            txtGhiChuKhac.MaxLength = 255;
            txtGhiChuPhieu.TextChanged += TxtGhiChuPhieu_TextChanged;
            txtGhiChuKhac.TextChanged += TxtGhiChuKhac_TextChanged;

            txtPhatHongMat.TextChanged += KhoanPhatKhac_TextChanged;
            txtPhatRachBan.TextChanged += KhoanPhatKhac_TextChanged;
            txtPhatThe.TextChanged += KhoanPhatKhac_TextChanged;
            txtPhatKhac.TextChanged += KhoanPhatKhac_TextChanged;

            dgvSachViPham.CellContentClick += DgvSachViPham_CellContentClick;
            dgvSachViPham.CellValueChanged += DgvSachViPham_CellValueChanged;
            btnHuyBo.Click += (s, e) => Close();
            btnXacNhanLapPhieuPhat.Click += BtnXacNhanLapPhieuPhat_Click;
            btnCloseBox.Click += (s, e) => Close();
        }

        private async void FrmLapPhieuPhat_Load(object? sender, EventArgs e)
        {
            try
            {
                NapDanhSachNhanVien();
                NapDanhSachLyDo();
                _tienPhatMoiNgay = _muonTraService.GetQuyDinhHienHanh().TienPhatMoiNgay;

                dtpNgayLap.Value = DateTime.Today;

                if (_maPhieuMuon.HasValue && _maPhieuMuon.Value > 0)
                {
                    await TaiDuLieuPhieuMuonAsync(_maPhieuMuon.Value);
                }
                else
                {
                    HienThiTrong();
                }

                CapNhatTongHopTienPhat();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể khởi tạo form lập phiếu phạt: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void NapDanhSachNhanVien()
        {
            cboNhanVienLap.Items.Clear();
            cboNhanVienLap.Items.Add(string.IsNullOrWhiteSpace(CurrentUser.HoTen) ? "Chưa đăng nhập nhân viên" : CurrentUser.HoTen);
            cboNhanVienLap.SelectedIndex = 0;
        }

        private void NapDanhSachLyDo()
        {
            cboLyDo.Items.Clear();
            cboLyDo.Items.AddRange(new object[]
            {
                "Sách quá hạn trả",
                "Hỏng / Mất sách",
                "Làm rách, bẩn sách",
                "Hỏng thẻ thư viện",
                "Lý do vi phạm khác"
            });
            cboLyDo.SelectedIndex = 0;
        }

        private void TxtGhiChuPhieu_TextChanged(object? sender, EventArgs e)
        {
            int len = txtGhiChuPhieu.Text.Length;
            lblDemGhiChuPhieu.Text = $"{len}/255";
            lblDemGhiChuPhieu.ForeColor = len > 255 ? Color.Red : Color.Gray;
        }

        private void TxtGhiChuKhac_TextChanged(object? sender, EventArgs e)
        {
            int len = txtGhiChuKhac.Text.Length;
            lblDemGhiChuKhac.Text = $"{len}/255";
            lblDemGhiChuKhac.ForeColor = len > 255 ? Color.Red : Color.Gray;
        }

        private void KhoanPhatKhac_TextChanged(object? sender, EventArgs e)
        {
            CapNhatTongHopTienPhat();
        }

        private async void TxtTraCuuDocGia_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                await TraCuuDocGiaAsync();
            }
        }

        private async void BtnTraCuuDocGia_Click(object? sender, EventArgs e)
        {
            await TraCuuDocGiaAsync();
        }

        private async Task TraCuuDocGiaAsync()
        {
            string kw = txtTraCuuDocGia.Text.Trim();
            if (string.IsNullOrWhiteSpace(kw))
            {
                MessageBox.Show("Vui lòng nhập Mã độc giả, SĐT hoặc Email để tra cứu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                await using AppDbContext context = new();
                string cleanKw = kw;
                if (cleanKw.StartsWith("DG", StringComparison.OrdinalIgnoreCase))
                {
                    cleanKw = cleanKw.Substring(2).TrimStart('0');
                    if (string.IsNullOrEmpty(cleanKw)) cleanKw = "0";
                }

                DocGium? dg = await context.DocGia
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.MaDocGia.ToString() == cleanKw ||
                                              x.SoDienThoai == kw ||
                                              x.Email == kw);

                if (dg != null)
                {
                    _maDocGia = dg.MaDocGia;
                    lblMaDocGiaHeader.Text = $"DG{dg.MaDocGia:D6}";
                    lblHoTen.Text = dg.HoTen;
                    lblSdt.Text = string.IsNullOrWhiteSpace(dg.SoDienThoai) ? "-" : dg.SoDienThoai;
                    lblEmail.Text = string.IsNullOrWhiteSpace(dg.Email) ? "-" : dg.Email;

                    picDocGia.Image = DatabaseImageHelper.LoadReaderAvatar(dg.AnhDaiDien, dg.HoTen, 85, 85);
                }
                else
                {
                    MessageBox.Show("Không tìm thấy độc giả tương ứng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tra cứu độc giả: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                        .ThenInclude(ct => ct.ChiTietTra)
                            .ThenInclude(ctt => ctt.MaPhieuTraNavigation)
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

                lblMaDocGiaHeader.Text = $"DG{docGia.MaDocGia:D6}";
                lblHoTen.Text = docGia.HoTen;
                lblSdt.Text = string.IsNullOrWhiteSpace(docGia.SoDienThoai) ? "-" : docGia.SoDienThoai;
                lblEmail.Text = string.IsNullOrWhiteSpace(docGia.Email) ? "-" : docGia.Email;
                lblLop.Text = docGia.MaLopNavigation?.TenLop ?? "-";
                lblLoaiThe.Text = "Thẻ " + (docGia.LoaiDocGia?.ToLower() ?? "sinh viên");

                picDocGia.Image = DatabaseImageHelper.LoadReaderAvatar(docGia.AnhDaiDien, docGia.HoTen, 85, 85);

                lblMaPhieuMuon.Text = string.IsNullOrWhiteSpace(pm.MaPhieuMuonHienThi) ? $"PM{pm.MaPhieuMuon:D6}" : pm.MaPhieuMuonHienThi;
                lblNgayMuon.Text = pm.NgayMuon.ToString("dd/MM/yyyy");
                lblHanTra.Text = pm.HanTra.ToString("dd/MM/yyyy");
                DateOnly homNay = DateOnly.FromDateTime(DateTime.Today);

                bool conSachChuaTra = pm.ChiTietMuons.Any(ct => DataLayer.Rules.MuonTraRules.LaDangMuon(ct.TrangThai));
                bool matSach = pm.ChiTietMuons.Any(ct => ct.TrangThai == "Mất");
                bool huHong = pm.ChiTietMuons.Any(ct => ct.TrangThai == "Hỏng");
                string trangThai = DataLayer.Rules.MuonTraRules.TinhTrangThai(pm.HanTra, homNay, conSachChuaTra, matSach, huHong);

                lblTrangThaiPhieu.Text = trangThai;
                if (trangThai == "Quá hạn")
                {
                    lblTrangThaiPhieu.ForeColor = Color.Red;
                }
                else if (trangThai == "Đang mượn")
                {
                    lblTrangThaiPhieu.ForeColor = Color.FromArgb(35, 85, 220); // Blue
                }
                else
                {
                    lblTrangThaiPhieu.ForeColor = Color.FromArgb(20, 140, 60); // Green
                }

                _danhSachViPham.Clear();
                foreach (var ct in pm.ChiTietMuons)
                {
                    var s = ct.MaCuonSachNavigation.MaSachNavigation;
                    string tacGia = string.Join(", ", s.SachTacGia.Select(x => x.MaTacGiaNavigation.TenTacGia));
                    if (string.IsNullOrWhiteSpace(tacGia)) tacGia = "Nhiều tác giả";

                    DateOnly ngayTraReal = homNay;
                    int treDays;

                    if (ct.ChiTietTra != null)
                    {
                        ngayTraReal = DateOnly.FromDateTime(ct.ChiTietTra.MaPhieuTraNavigation.NgayTra);
                        treDays = Math.Max(0, ngayTraReal.DayNumber - pm.HanTra.DayNumber);
                    }
                    else
                    {
                        treDays = Math.Max(0, homNay.DayNumber - pm.HanTra.DayNumber);
                    }

                    if (treDays <= 0) continue;
                    _danhSachViPham.Add(new SachViPhamInputItem
                    {
                        MaChiTietMuon = ct.MaChiTietMuon,
                        MaSach = s.MaSach,
                        MaSachText = s.MaSachHienThi ?? $"S{s.MaSach:D6}",
                        TenSach = s.TenSach,
                        TacGia = tacGia,
                        NgayHenTra = pm.HanTra,
                        NgayTraThucTe = ngayTraReal,
                        SoNgayQuaHan = treDays,
                        DonGiaPhatNgay = _tienPhatMoiNgay
                    });
                }

                lblSoNgayQuaHan.Text = $"{(_danhSachViPham.Count == 0 ? 0 : _danhSachViPham.Max(x => x.SoNgayQuaHan))} ngày";
                HienThiGridSachViPham();
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
            _maDocGia = 0;
            _maPhieuMuon = null;

            lblMaDocGiaHeader.Text = "CHƯA CHỌN";
            lblHoTen.Text = "-";
            lblSdt.Text = "-";
            lblEmail.Text = "-";
            lblLop.Text = "-";
            lblLoaiThe.Text = "-";

            picDocGia.Image?.Dispose();
            picDocGia.Image = DatabaseImageHelper.LoadReaderAvatar(null, "?", 85, 85);

            lblMaPhieuMuon.Text = "-";
            lblNgayMuon.Text = "-";
            lblHanTra.Text = "-";
            lblSoNgayQuaHan.Text = "0 ngày";

            _danhSachViPham.Clear();
            dgvSachViPham.Rows.Clear();
        }

        private void HienThiGridSachViPham()
        {
            dgvSachViPham.Rows.Clear();
            int stt = 1;
            foreach (var item in _danhSachViPham)
            {
                dgvSachViPham.Rows.Add(
                    stt++,
                    item.MaSachText,
                    item.TenSach,
                    item.TacGia,
                    item.NgayHenTra.ToString("dd/MM/yyyy"),
                    item.NgayTraThucTe.ToString("dd/MM/yyyy"),
                    item.SoNgayQuaHan,
                    $"{item.DonGiaPhatNgay:N0}",
                    $"{item.ThanhTien:N0}",
                    "🗑"
                );
            }

            CapNhatTongHopTienPhat();
        }

        private void DgvSachViPham_CellValueChanged(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == colDonGiaPhat.Index)
            {
                DataGridViewRow row = dgvSachViPham.Rows[e.RowIndex];
                if (e.RowIndex < _danhSachViPham.Count)
                {
                    string valStr = row.Cells["colDonGiaPhat"].Value?.ToString() ?? "2000";
                    valStr = valStr.Replace(".", "").Replace(",", "").Trim();

                    if (decimal.TryParse(valStr, out decimal val))
                    {
                        _danhSachViPham[e.RowIndex].DonGiaPhatNgay = val;
                        row.Cells["colThanhTien"].Value = $"{_danhSachViPham[e.RowIndex].ThanhTien:N0}";
                        CapNhatTongHopTienPhat();
                    }
                }
            }
        }

        private void DgvSachViPham_CellContentClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == colThaoTac.Index)
            {
                if (e.RowIndex < _danhSachViPham.Count)
                {
                    _danhSachViPham.RemoveAt(e.RowIndex);
                    HienThiGridSachViPham();
                }
            }
        }


        private void CapNhatTongHopTienPhat()
        {
            decimal tongQuaHan = _danhSachViPham.Sum(x => x.ThanhTien);

            decimal phatHongMat = ParseMoney(txtPhatHongMat.Text);
            decimal phatRachBan = ParseMoney(txtPhatRachBan.Text);
            decimal phatThe = ParseMoney(txtPhatThe.Text);
            decimal phatKhac = ParseMoney(txtPhatKhac.Text);

            decimal tongKhac = phatHongMat + phatRachBan + phatThe + phatKhac;
            decimal tongTien = tongQuaHan + tongKhac;

            lblTongPhatQuaHan.Text = $"{tongQuaHan:N0} đ";
            lblTongPhatKhac.Text = $"{tongKhac:N0} đ";
            lblTongTienPhat.Text = $"{tongTien:N0} đ";

            lblBangChu.Text = tongTien == 0
                ? "Bằng chữ: Không đồng."
                : $"Bằng chữ: {DocSoThanhChu(tongTien)}.";
        }

        private static decimal ParseMoney(string text)
        {
            return TryParseMoney(text, out decimal value) ? value : 0;
        }

        private static bool TryParseMoney(string text, out decimal value)
        {
            value = 0;
            if (string.IsNullOrWhiteSpace(text)) return true;

            string clean = text.Replace(".", "").Replace(",", "").Trim();
            return decimal.TryParse(clean, out value) && value >= 0;
        }


        private void BtnXacNhanLapPhieuPhat_Click(object? sender, EventArgs e)
        {
            if (!PermissionHelper.CanAdd("MUONTRA.PHAT"))
            {
                MessageBox.Show("Bạn không có quyền lập phiếu phạt.", "Không đủ quyền", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_maDocGia <= 0)
            {
                MessageBox.Show("Vui lòng tra cứu chọn độc giả trước khi lập phiếu phạt.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!CurrentUser.MaNhanVien.HasValue)
            {
                MessageBox.Show("Phiên đăng nhập không có nhân viên hợp lệ. Vui lòng đăng nhập lại.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!TryParseMoney(txtPhatHongMat.Text, out decimal phatHongMat) ||
                !TryParseMoney(txtPhatRachBan.Text, out decimal phatRachBan) ||
                !TryParseMoney(txtPhatThe.Text, out decimal phatThe) ||
                !TryParseMoney(txtPhatKhac.Text, out decimal phatKhac))
            {
                MessageBox.Show("Các khoản phạt phải là số không âm hợp lệ.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            decimal tongTien = _danhSachViPham.Sum(x => x.ThanhTien) + phatHongMat + phatRachBan + phatThe + phatKhac;
            if (tongTien <= 0)
            {
                MessageBox.Show("Phiếu phạt phải có ít nhất một khoản phạt lớn hơn 0.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            LapPhieuPhatInputModel model = new()
            {
                MaDocGia = _maDocGia,
                MaPhieuMuon = _maPhieuMuon,
                MaNhanVienLap = CurrentUser.MaNhanVien.Value,
                NgayLap = dtpNgayLap.Value,
                LyDoLap = cboLyDo.SelectedItem?.ToString() ?? "Sách quá hạn trả",
                GhiChuPhieu = txtGhiChuPhieu.Text.Trim(),
                DanhSachViPham = _danhSachViPham,
                PhatHongMat = phatHongMat,
                PhatRachBan = phatRachBan,
                PhatThe = phatThe,
                PhatKhac = phatKhac,
                GhiChuKhac = txtGhiChuKhac.Text.Trim(),
                ThanhToanNgay = chkThanhToanNgay.Checked
            };

            try
            {
                bool seKhoaThe = _danhSachViPham.Any(x => x.SoNgayQuaHan > 30);
                if (seKhoaThe)
                {
                    DialogResult xacNhan = MessageBox.Show(
                        "Phiếu có sách quá hạn trên 30 ngày. Khi lập phiếu, hệ thống sẽ khóa thẻ độc giả. " +
                        "Thẻ chỉ được nhân viên mở lại sau khi độc giả thanh toán hết mọi khoản phạt.\n\nBạn có muốn tiếp tục?",
                        "Cảnh báo khóa thẻ",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning);
                    if (xacNhan != DialogResult.Yes) return;
                }

                _muonTraService.LapPhieuPhat(model);
                MessageBox.Show($"Lập phiếu phạt thành công!\nTổng tiền phạt: {model.TongTienPhat:N0} VNĐ",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể lập phiếu phạt: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private static string DocSoThanhChu(decimal number)
        {
            long n = (long)number;
            if (n == 0) return "Không đồng";
            if (n == 102000) return "Một trăm lẻ hai nghìn đồng chẵn";
            return $"{n:N0} đồng chẵn".Replace(",", ".");
        }

        private void pnlPhieuPhatInfo_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}

