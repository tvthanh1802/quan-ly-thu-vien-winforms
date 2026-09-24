using BusinessLayer.Services;
using DataLayer.Models;
using Presentation.Helpers;
using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Windows.Forms;

namespace Presentation.Models
{
    public partial class FrmInPhieuNhap : Form
    {
        private readonly NhapSachService _nhapSachService;
        private readonly int _maPhieuNhap;
        private NhapSachDetailModel? _model;

        public FrmInPhieuNhap() : this(1)
        {
        }

        public FrmInPhieuNhap(int maPhieuNhap)
        {
            InitializeComponent();
            _nhapSachService = new NhapSachService();
            _maPhieuNhap = maPhieuNhap;
            KhoiTaoForm();
        }

        private void KhoiTaoForm()
        {
            if (DesignModeHelper.IsDesignMode(this)) return;

            KeyPreview = true;
            StartPosition = FormStartPosition.CenterScreen;

            Load += FrmInPhieuNhap_Load;

            btnInNgay.Click += (s, e) => ThucHienIn();
            btnInPhieuNhap.Click += (s, e) => ThucHienIn();
            btnXuatPdfTop.Click += (s, e) => ThucHienXuatPdf();
            btnXuatPdf.Click += (s, e) => ThucHienXuatPdf();
            btnThietLapTrang.Click += BtnThietLapTrang_Click;
            btnLamMoi.Click += (s, e) => NapDuLieuIn();

            btnDong.Click += (s, e) => Close();
            btnCloseBox.Click += (s, e) => Close();
        }

        private void FrmInPhieuNhap_Load(object? sender, EventArgs e)
        {
            try
            {
                NapDanhSachMayIn();
                NapCauHinhGiay();

                _model = _nhapSachService.GetChiTiet(_maPhieuNhap);
                NapDuLieuIn();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể khởi tạo bản xem trước: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void NapDanhSachMayIn()
        {
            cboMayIn.Items.Clear();
            foreach (string printer in PrinterSettings.InstalledPrinters)
            {
                cboMayIn.Items.Add(printer);
            }

            if (cboMayIn.Items.Count > 0)
            {
                PrinterSettings defaultPrinter = new();
                int idx = cboMayIn.Items.IndexOf(defaultPrinter.PrinterName);
                cboMayIn.SelectedIndex = idx >= 0 ? idx : 0;
            }
            else
            {
                cboMayIn.Items.Add("Microsoft Print to PDF");
                cboMayIn.SelectedIndex = 0;
            }
        }

        private void NapCauHinhGiay()
        {
            cboKhoGiay.Items.Clear();
            cboKhoGiay.Items.AddRange(new object[] { "A4 (210 x 297 mm)", "A5 (148 x 210 mm)", "Letter (216 x 279 mm)" });
            cboKhoGiay.SelectedIndex = 0;

            cboHuongIn.Items.Clear();
            cboHuongIn.Items.AddRange(new object[] { "Dọc", "Ngang" });
            cboHuongIn.SelectedIndex = 0;
        }

        private void NapDuLieuIn()
        {
            string maText = _model != null && !string.IsNullOrWhiteSpace(_model.MaPhieuText)
                ? _model.MaPhieuText
                : $"PN{_maPhieuNhap:D6}";

            string ngayText = _model?.NgayNhap.ToString("dd/MM/yyyy") ?? "-";
            string nccText = HienThiRong(_model?.TenNhaCungCap);
            string sdtText = HienThiRong(_model?.SoDienThoaiNcc);
            string diaChiText = HienThiRong(_model?.DiaChiNcc);
            string nguoiLapText = HienThiRong(_model?.NguoiLap);

            lblPaperMaPhieu.Text = $"Mã phiếu nhập:  {maText}";
            lblPaperNgayNhap.Text = $"Ngày nhập:  {ngayText}";
            lblPaperNccValue.Text = nccText;
            lblPaperSdtValue.Text = sdtText;
            lblPaperDiaChiValue.Text = diaChiText;
            lblPaperNguoiLapValue.Text = nguoiLapText;

            txtGhiChuIn.Text = $"Phiếu nhập sách {maText}\nNgày nhập {ngayText}";

            // Grid preview
            dgvPreviewGrid.Rows.Clear();
            int stt = 1;
            int tongCuon = 0;

            if (_model != null)
            {
                foreach (var item in _model.DauSaches)
                {
                    tongCuon += item.SoLuong;
                    dgvPreviewGrid.Rows.Add(
                        stt++,
                        item.MaSachText,
                        item.TenSach,
                        HienThiRong(item.TheLoai),
                        item.SoLuong,
                        $"{item.DonGia:N0} đ",
                        $"{item.ThanhTien:N0} đ"
                    );
                }
            }

            lblSumDauSachVal.Text = dgvPreviewGrid.Rows.Count.ToString();
            lblSumSoCuonVal.Text = tongCuon.ToString();

            decimal tamTinh = _model?.TamTinh ?? 0m;
            decimal chietKhau = _model?.ChietKhau ?? 0m;
            decimal tongTien = _model?.TongTien ?? 0m;

            lblTamTinhVal.Text = $"{tamTinh:N0} đ";
            lblChietKhauVal.Text = $"{chietKhau:N0} đ";
            lblTongTienVal.Text = $"{tongTien:N0} đ";
        }

        private static string HienThiRong(string? value) =>
            string.IsNullOrWhiteSpace(value) ? "-" : value;

        private void ThucHienIn()
        {
            using PrintDocument document = TaoTaiLieuIn();
            if (cboMayIn.SelectedItem != null)
                document.PrinterSettings.PrinterName = cboMayIn.SelectedItem.ToString() ?? string.Empty;

            using PrintPreviewDialog preview = new() { Document = document, Width = 900, Height = 700 };
            preview.ShowDialog(this);
        }

        private PrintDocument TaoTaiLieuIn()
        {
            PrintDocument document = new() { DocumentName = _model?.MaPhieuText ?? $"PN{_maPhieuNhap:D6}" };
            int currentItemIndex = 0;

            document.BeginPrint += (_, _) =>
            {
                currentItemIndex = 0;
            };

            document.PrintPage += (_, ev) =>
            {
                Graphics g = ev.Graphics;
                float y = 40;
                float leftMargin = 50;
                float rightMargin = ev.MarginBounds.Right > 0 ? ev.MarginBounds.Right : 750;

                using Font fontTitle = new("Segoe UI", 16, FontStyle.Bold);
                using Font fontSubtitle = new("Segoe UI", 9.5F, FontStyle.Italic);
                using Font fontHeader = new("Segoe UI", 9.5F, FontStyle.Bold);
                using Font fontRegular = new("Segoe UI", 9F);
                using Font fontSmall = new("Segoe UI", 8.5F);

                using Pen penBorder = new(Color.Black, 1);
                using Brush brushHeaderBg = new SolidBrush(Color.FromArgb(240, 240, 240));

                if (currentItemIndex == 0)
                {
                    g.DrawString("THƯ VIỆN TRUNG TÂM QUẢN LÝ TÀI LIỆU", fontSubtitle, Brushes.Gray, leftMargin, y);
                    y += 22;
                    g.DrawString("PHIẾU NHẬP SÁCH KHỔ VẬT LÝ", fontTitle, Brushes.Black, leftMargin, y);
                    y += 35;

                    string maText = _model?.MaPhieuText ?? $"PN{_maPhieuNhap:D6}";
                    string ngayText = _model?.NgayNhap.ToString("dd/MM/yyyy") ?? "-";
                    string trangThai = _model?.TrangThai ?? "-";

                    g.DrawString($"Số phiếu: {maText}    |    Ngày nhập: {ngayText}    |    Trạng thái: {trangThai}", fontHeader, Brushes.Black, leftMargin, y);
                    y += 25;

                    string ncc = _model?.TenNhaCungCap ?? "-";
                    string sdt = _model?.SoDienThoaiNcc ?? "-";
                    g.DrawString($"Nhà cung cấp: {ncc} (SĐT: {sdt})", fontRegular, Brushes.Black, leftMargin, y);
                    y += 20;

                    string diaChi = _model?.DiaChiNcc ?? "-";
                    g.DrawString($"Địa chỉ NCC: {diaChi}", fontSmall, Brushes.Black, leftMargin, y);
                    y += 20;

                    string nguoiLap = _model?.NguoiLap ?? "-";
                    g.DrawString($"Người lập phiếu: {nguoiLap}", fontRegular, Brushes.Black, leftMargin, y);
                    y += 30;
                }

                float[] colWidths = new float[] { 40, 85, 210, 100, 55, 90, 105 };
                string[] colHeaders = new string[] { "STT", "Mã sách", "Tên sách", "Thể loại", "SL", "Đơn giá", "Thành tiền" };

                float cellHeight = 26;
                float curX = leftMargin;

                g.FillRectangle(brushHeaderBg, leftMargin, y, colWidths.Sum(), cellHeight);
                g.DrawRectangle(penBorder, leftMargin, y, colWidths.Sum(), cellHeight);

                for (int i = 0; i < colHeaders.Length; i++)
                {
                    RectangleF rect = new(curX, y, colWidths[i], cellHeight);
                    StringAlignment align = i switch
                    {
                        0 => StringAlignment.Center,
                        >= 4 => StringAlignment.Far,
                        _ => StringAlignment.Near
                    };
                    using StringFormat sf = new() { Alignment = align, LineAlignment = StringAlignment.Center };
                    g.DrawString(colHeaders[i], fontHeader, Brushes.Black, rect, sf);
                    curX += colWidths[i];
                }
                y += cellHeight;

                var items = _model?.DauSaches ?? new();
                while (currentItemIndex < items.Count)
                {
                    if (y + cellHeight > (ev.MarginBounds.Bottom > 0 ? ev.MarginBounds.Bottom : 1050) - 80)
                    {
                        ev.HasMorePages = true;
                        return;
                    }

                    var item = items[currentItemIndex];
                    curX = leftMargin;

                    g.DrawRectangle(penBorder, leftMargin, y, colWidths.Sum(), cellHeight);

                    string[] rowValues = new string[]
                    {
                        (currentItemIndex + 1).ToString(),
                        item.MaSachText,
                        item.TenSach,
                        item.TheLoai,
                        item.SoLuong.ToString(),
                        $"{item.DonGia:N0} đ",
                        $"{item.ThanhTien:N0} đ"
                    };

                    for (int i = 0; i < rowValues.Length; i++)
                    {
                        RectangleF rect = new(curX + 3, y + 2, colWidths[i] - 6, cellHeight - 4);
                        StringAlignment align = i switch
                        {
                            0 => StringAlignment.Center,
                            >= 4 => StringAlignment.Far,
                            _ => StringAlignment.Near
                        };
                        using StringFormat sf = new() { Alignment = align, LineAlignment = StringAlignment.Center, FormatFlags = StringFormatFlags.NoWrap };
                        g.DrawString(rowValues[i], fontRegular, Brushes.Black, rect, sf);
                        curX += colWidths[i];
                    }

                    y += cellHeight;
                    currentItemIndex++;
                }

                ev.HasMorePages = false;

                y += 15;
                decimal tamTinh = _model?.TamTinh ?? 0m;
                decimal chietKhau = _model?.ChietKhau ?? 0m;
                decimal tongTien = _model?.TongTien ?? 0m;

                g.DrawString($"Tạm tính: {tamTinh:N0} đ    |    Chiết khấu: {chietKhau:N0} đ", fontRegular, Brushes.Black, leftMargin, y);
                y += 22;
                g.DrawString($"TỔNG TIỀN PHIẾU NHẬP: {tongTien:N0} đ", fontHeader, Brushes.Black, leftMargin, y);
                y += 40;

                float col1X = leftMargin + 40;
                float col2X = rightMargin - 220;

                g.DrawString("Đại diện Nhà cung cấp", fontHeader, Brushes.Black, col1X, y);
                g.DrawString("Người lập phiếu", fontHeader, Brushes.Black, col2X, y);
                y += 20;
                g.DrawString("(Ký, ghi rõ họ tên)", fontSubtitle, Brushes.Gray, col1X, y);
                g.DrawString("(Ký, ghi rõ họ tên)", fontSubtitle, Brushes.Gray, col2X, y);
            };
            return document;
        }

        private void ThucHienXuatPdf()
        {
            if (_model == null)
            {
                MessageBox.Show("Không có dữ liệu phiếu nhập để xuất.", "Xuất PDF",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            const string pdfPrinter = "Microsoft Print to PDF";
            bool installed = PrinterSettings.InstalledPrinters.Cast<string>()
                .Any(x => string.Equals(x, pdfPrinter, StringComparison.OrdinalIgnoreCase));
            if (!installed)
            {
                MessageBox.Show("Máy chưa cài Microsoft Print to PDF.", "Không thể xuất PDF",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using SaveFileDialog sfd = new()
            {
                Filter = "PDF Document|*.pdf",
                FileName = $"PhieuNhap_{_model.MaPhieuText}.pdf"
            };
            if (sfd.ShowDialog() != DialogResult.OK) return;

            try
            {
                using PrintDocument document = TaoTaiLieuIn();
                document.PrinterSettings.PrinterName = pdfPrinter;
                document.PrinterSettings.PrintToFile = true;
                document.PrinterSettings.PrintFileName = sfd.FileName;
                document.PrintController = new StandardPrintController();
                document.Print();

                if (!File.Exists(sfd.FileName) || new FileInfo(sfd.FileName).Length == 0)
                    throw new IOException("Driver PDF không tạo được tệp đầu ra.");

                MessageBox.Show($"Đã xuất file PDF tại:\n{sfd.FileName}", "Xuất PDF thành công",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể xuất PDF.\n" + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnThietLapTrang_Click(object? sender, EventArgs e)
        {
            using PageSetupDialog psd = new();
            using PrintDocument doc = new();
            psd.Document = doc;
            psd.ShowDialog(this);
        }
    }
}
