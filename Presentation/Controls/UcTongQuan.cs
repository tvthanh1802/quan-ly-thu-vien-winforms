using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using System.Globalization;
using System.Windows.Forms.DataVisualization.Charting;
using System.Reflection;
using Guna.UI2.WinForms;
using BusinessLayer.Services;
using DataLayer.Models;
using Presentation.Helpers;
using Presentation.Models;
namespace Presentation.Controls
{
    public partial class UcTongQuan : UserControl
    {
        private ThongKeService? _thongKeService;
        // tổng sách để vẽ ở giữa biểu đồ danh mục
        private int _totalDanhMuc = 0;

        public UcTongQuan()
        {
            InitializeComponent();
        }


        // Mở UserControl theo tên (sử dụng phản chiếu) — nếu chưa có sẽ báo
        private void OpenUserControlByName(string typeName)
        {
            var frm = this.FindForm() as FrmMain;
            if (frm == null) return;

            // tìm kiểu trong assembly hiện tại
            var asm = Assembly.GetExecutingAssembly();
            var fullName = $"Presentation.Controls.{typeName}";
            var t = asm.GetType(fullName);

            if (t == null)
            {
                MessageBox.Show($"Chưa tìm thấy UserControl: {typeName}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (!typeof(UserControl).IsAssignableFrom(t))
            {
                MessageBox.Show($"Loại {typeName} không phải UserControl.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var uc = Activator.CreateInstance(t) as UserControl;
            if (uc != null)
            {
                frm.OpenControl(uc);
            }
        }

        private void lnkXemTatCaThongBao_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using var frm = new FrmThongBaoHeThong();
            frm.ShowDialog(FindForm());
        }

        private void lnkXemTatCaMuon_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using var frm = new FrmLichSuMuonSachGanDay();
            frm.ShowDialog(FindForm());
        }

        private void lnkTopDanhMuc_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Mở Form Top danh mục sách thay vì mở UserControl Quản lý sách
            using var frm = new FrmTopDanhMucSach();
            frm.ShowDialog(FindForm());
        }


        private void LoadRecentBorrowHistory()
        {
            dgvLichSuMuon.AutoGenerateColumns = false;

            var data = _thongKeService!.GetRecentBorrowHistory();

            // chuyển định dạng ngày sang chuỗi để hiển thị
            var rows = data.Select(x => new
            {
                x.MaPhieuMuon,
                HoTenDocGia = x.DocGia,
                x.TenSach,
                NgayMuon = x.NgayMuon.ToString("dd/MM/yyyy"),
                HanTra = x.HanTra.ToString("dd/MM/yyyy"),
                x.TrangThai
            }).ToList();

            dgvLichSuMuon.DataSource = rows;
        }

        private void UcTongQuan_Load(object sender, EventArgs e)
        {
            try
            {
                // Skip runtime initialization when in designer
                if (DesignModeHelper.IsDesignMode(this))
                    return;

                // initialize service and controls
                _thongKeService = new ThongKeService();

                // Cấu hình biểu đồ và timer đã nằm trong InitializeComponent().
                timerClock.Tick -= timerClock_Tick;
                timerClock.Tick += timerClock_Tick;

                LoadCurrentUser();
                UpdateClock();

                var loi = new List<string>();
                ThuTaiMuc("thẻ thống kê", LoadSummaryCards, loi);
                ThuTaiMuc("biểu đồ mượn - trả", LoadBorrowReturnChart, loi);
                ThuTaiMuc("thông báo", LoadNotifications, loi);
                ThuTaiMuc("số thông báo chưa đọc", LoadNotificationBadge, loi);
                ThuTaiMuc("lịch sử mượn", LoadRecentBorrowHistory, loi);
                ThuTaiMuc("top danh mục", LoadTopDanhMuc, loi);

                timerClock.Start();

                if (loi.Count > 0)
                {
                    MessageBox.Show(
                        "Một số khu vực chưa tải được:\n- " + string.Join("\n- ", loi),
                        "Thông báo dữ liệu",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Lỗi hệ thống ucTongQuan", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static void ThuTaiMuc(
            string tenMuc,
            Action hanhDong,
            ICollection<string> danhSachLoi)
        {
            try
            {
                hanhDong();
            }
            catch (Exception ex)
            {
                danhSachLoi.Add($"{tenMuc}: {ex.GetBaseException().Message}");
            }
        }
        // cấu hình biểu đồ danh mục được thực hiện trong CauHinhBieuDoDanhMuc
        private void LoadCurrentUser()
        {
            string displayName = string.IsNullOrWhiteSpace(CurrentUser.HoTen)
                ? (string.IsNullOrWhiteSpace(CurrentUser.TenDangNhap) ? "Admin" : CurrentUser.TenDangNhap)
                : CurrentUser.HoTen;

            lblUser.Text = $"Xin chào, {displayName}";
            lblRole.Text = string.IsNullOrWhiteSpace(CurrentUser.VaiTro)
                ? "Quản trị viên"
                : CurrentUser.VaiTro;

            lblUser.AutoEllipsis = true;
            CurrentUserAvatarHelper.Apply(picAvatar);
        }
        private void UpdateClock()
        {
            var culture = new System.Globalization.CultureInfo("vi-VN");

            // chức năng: cập nhật ngày (theo vi-VN) và giờ (định dạng 12h có AM/PM)
            lblDate.Text = DateTime.Now.ToString(
                "dddd, dd/MM/yyyy",
                culture);

            // sử dụng InvariantCulture để đảm bảo hiển thị AM/PM (ví dụ 6:00:00 AM)
            lblTime.Text = DateTime.Now.ToString("h:mm:ss tt", System.Globalization.CultureInfo.InvariantCulture);
        }
        private void timerClock_Tick(object? sender, EventArgs e)
        {
            UpdateClock();
        }

        // Các handler rỗng được Designer tham chiếu — giữ để tránh lỗi biên dịch
        private void pictureBox1_Click(object sender, EventArgs e) { }
        private void lblTienPhat_Click(object sender, EventArgs e) { }
        private void lblTongDocGia_Click(object sender, EventArgs e) { }
        private void label14_Click(object sender, EventArgs e) { }
        private void lblQuaHan_Click(object sender, EventArgs e) { }
        private void lblNotificationCount_Click(object sender, EventArgs e) { }

        private void LoadSummaryCards()
        {
            int totalCopies = _thongKeService!.GetTotalCopies();
            int totalReaders = _thongKeService.GetTotalReaders();
            int currentlyBorrowed = _thongKeService.GetCurrentlyBorrowed();
            int overdue = _thongKeService.GetOverdueCount();
            decimal fines = _thongKeService.GetFineThisMonth();

            int newCopiesThisMonth = _thongKeService.GetNewBooksThisMonth();
            int newReadersThisMonth = _thongKeService.GetNewReadersThisMonth();
            decimal finesPreviousMonth = _thongKeService.GetFinePreviousMonth();

            lblTongSach.Text = totalCopies.ToString("N0");
            lblTongSachSub.Text = $"↑ {newCopiesThisMonth:N0} sách mới trong tháng";

            lblTongDocGia.Text = totalReaders.ToString("N0");
            lblTongDocGiaSub.Text = $"↑ {newReadersThisMonth:N0} độc giả mới trong tháng";

            lblDangMuon.Text = currentlyBorrowed.ToString("N0");
            lblDangMuonSub.Text = $"Hiện đang có {currentlyBorrowed:N0} sách";

            lblQuaHan.Text = overdue.ToString("N0");
            lblQuaHanSub.Text = overdue > 0 ? "⚠ Cần xử lý kịp thời" : "✓ Trạng thái an toàn";

            lblTienPhat.Text = fines.ToString("N0");
            double percent = finesPreviousMonth == 0 ? 0 : (double)((fines - finesPreviousMonth) * 100 / finesPreviousMonth);
            lblTienPhatSub.Text = finesPreviousMonth == 0 ? "↑ Tháng này có phát sinh" : $"{(percent >= 0 ? "↑" : "↓")} {Math.Abs(percent):0.#}% so với tháng trước";
        }


        private void LoadBorrowReturnChart()
        {
            if (!chartMuonTra.ChartAreas.Any(x => x.Name == "MainArea") ||
                !chartMuonTra.Series.Any(x => x.Name == "MuonArea") ||
                !chartMuonTra.Series.Any(x => x.Name == "TraArea") ||
                !chartMuonTra.Series.Any(x => x.Name == "Mượn") ||
                !chartMuonTra.Series.Any(x => x.Name == "Trả"))
            {
                throw new InvalidOperationException("Cấu hình biểu đồ mượn - trả trong Designer không hợp lệ.");
            }

            var (labels, borrows, returns) = _thongKeService!.GetLastNDaysBorrowReturn(7);

            Series muonArea = chartMuonTra.Series["MuonArea"];
            Series traArea = chartMuonTra.Series["TraArea"];
            Series muon = chartMuonTra.Series["Mượn"];
            Series tra = chartMuonTra.Series["Trả"];

            muonArea.Points.Clear();
            traArea.Points.Clear();
            muon.Points.Clear();
            tra.Points.Clear();

            int maximum = 0;

            for (int i = 0; i < labels.Length; i++)
            {
                string label = labels[i];
                int borrowValue = borrows[i];
                int returnValue = returns[i];

                int indexMuonArea = muonArea.Points.AddXY(i, borrowValue);
                int indexTraArea = traArea.Points.AddXY(i, returnValue);
                int indexMuon = muon.Points.AddXY(i, borrowValue);
                int indexTra = tra.Points.AddXY(i, returnValue);

                muonArea.Points[indexMuonArea].AxisLabel = label;
                traArea.Points[indexTraArea].AxisLabel = label;
                muon.Points[indexMuon].AxisLabel = label;
                tra.Points[indexTra].AxisLabel = label;

                muon.Points[indexMuon].Label = borrowValue.ToString();
                tra.Points[indexTra].Label = returnValue.ToString();

                maximum = Math.Max(maximum, Math.Max(borrowValue, returnValue));
            }

            ChartArea area = chartMuonTra.ChartAreas["MainArea"];

            int axisMaximum = Math.Max(10, ((maximum + 9) / 10) * 10);

            area.AxisY.Minimum = 0;
            area.AxisY.Maximum = axisMaximum;
            area.AxisY.Interval = Math.Max(1, axisMaximum / 5);

            area.AxisX.Interval = 1;
            area.AxisX.LabelStyle.Interval = 1;
            area.AxisX.LabelStyle.Angle = 0;
        }
        private void LoadNotifications()
        {
            UcThongBaoItem[] slots = { thongBaoItem1, thongBaoItem2, thongBaoItem3, thongBaoItem4 };
            List<ThongBaoDashboardModel> notifications =
                _thongKeService!.GetLatestNotifications(slots.Length, CurrentUser.MaNhanVien, CurrentUser.MaDocGia);

            for (int i = 0; i < slots.Length; i++)
            {
                bool coDuLieu = i < notifications.Count;
                slots[i].Visible = coDuLieu;
                if (!coDuLieu) continue;

                ThongBaoDashboardModel notification = notifications[i];
                slots[i].SetData(notification.MaThongBao, notification.TieuDe, notification.NoiDung,
                    _thongKeService.FormatNotificationTime(notification.NgayGui),
                    notification.Icon, notification.Mau, notification.DaDoc);
                slots[i].NotificationClicked -= NotificationItem_Click;
                slots[i].NotificationClicked += NotificationItem_Click;
            }
        }
        private void NotificationItem_Click(object? sender, int maThongBao)
        {
            bool updated = _thongKeService!.MarkNotificationAsReadForRecipient(
                maThongBao, CurrentUser.MaNhanVien, CurrentUser.MaDocGia);
            if (updated)
            {
                LoadNotifications();
                LoadNotificationBadge();
            }
        }

        private void LoadNotificationBadge()
        {
            int unreadCount = _thongKeService!.GetUnreadNotificationCount(
                CurrentUser.MaNhanVien,
                CurrentUser.MaDocGia);
            lblNotificationCount.Text = unreadCount > 99 ? "99+" : unreadCount.ToString();
            lblNotificationCount.Visible = unreadCount > 0;
        }
        private void LoadLichSuMuonMau()
        {
            // sample removed - data is loaded from database via LoadRecentBorrowHistory
        }


        private void LoadTopDanhMuc()
        {
            if (!chartDanhMuc.ChartAreas.Any(x => x.Name == "DanhMucArea") ||
                !chartDanhMuc.Series.Any(x => x.Name == "DanhMuc"))
            {
                throw new InvalidOperationException("Cấu hình biểu đồ danh mục trong Designer không hợp lệ.");
            }

            List<TopDanhMucModel> categories =
                _thongKeService!.GetTopBookCategories(4);

            Series series = chartDanhMuc.Series["DanhMuc"];
            series.Points.Clear();

            int totalBooks = _thongKeService.GetTotalBooks();
            lblTongDanhMuc.Text = totalBooks.ToString("N0");
            _totalDanhMuc = totalBooks;

            Guna2Panel[] panels = { pnlDanhMuc1, pnlDanhMuc2, pnlDanhMuc3, pnlDanhMuc4 };
            Label[] names = { lblDanhMuc1, lblDanhMuc2, lblDanhMuc3, lblDanhMuc4 };
            Label[] percentages = { lblTyLeDanhMuc1, lblTyLeDanhMuc2, lblTyLeDanhMuc3, lblTyLeDanhMuc4 };

            for (int i = 0; i < panels.Length; i++)
            {
                bool coDuLieu = i < categories.Count;
                panels[i].Visible = coDuLieu;
                if (!coDuLieu) continue;

                TopDanhMucModel category = categories[i];
                int pointIndex = series.Points.AddY(category.SoLuongSach);
                series.Points[pointIndex].Color = panels[i].FillColor;
                series.Points[pointIndex].ToolTip =
                    $"{category.TenTheLoai}: {category.SoLuongSach:N0} đầu sách ({category.TyLePhanTram:N1}%)";
                names[i].Text = category.TenTheLoai;
                percentages[i].Text = $"{category.TyLePhanTram:N0}%";
            }

            pnlTongDanhMuc.BringToFront();
        }

        // vẽ trung tâm cho biểu đồ doughnut (hiển thị tổng sách)
        private void chartDanhMuc_PostPaint(object sender, ChartPaintEventArgs e)
        {
            if (chartDanhMuc.Series.Count == 0) return;

            var series = chartDanhMuc.Series[0];
            if (series.Points.Count == 0) return;

            // tính vị trí trung tâm của ChartArea
            var area = chartDanhMuc.ChartAreas[series.ChartArea];
            var g = e.ChartGraphics.Graphics;

            // chuyển đổi tọa độ từ chart area sang pixel
            var rect = e.ChartGraphics.GetAbsoluteRectangle(area.Position.ToRectangleF());

            // vẽ vòng tròn trắng ở giữa
            int diameter = (int)(Math.Min(rect.Width, rect.Height) * 0.45);
            int centerX = (int)(rect.X + rect.Width / 2);
            int centerY = (int)(rect.Y + rect.Height / 2);

            var circleRect = new Rectangle(centerX - diameter / 2, centerY - diameter / 2, diameter, diameter);

            using (var brush = new SolidBrush(Color.White))
            {
                g.FillEllipse(brush, circleRect);
            }

            // vẽ chữ tổng sách
            string text = _totalDanhMuc.ToString("N0") + "\nTổng sách";
            using (var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
            using (var font = new Font("Segoe UI", 12f, FontStyle.Bold))
            using (var brush = new SolidBrush(Color.FromArgb(40, 45, 80)))
            {
                g.DrawString(text, font, brush, circleRect, sf);
            }
        }


        private void cardQuaHan_Paint(object sender, PaintEventArgs e)
        {

        }

        private void lblDangMuonSub_Click(object sender, EventArgs e)
        {

        }

        private void pnlTongQuanHeaderIcon_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using var pen = new Pen(Color.White, 2.1F)
            {
                StartCap = LineCap.Round,
                EndCap = LineCap.Round,
                LineJoin = LineJoin.Round
            };

            PointF[] leftPage =
            {
                new(7, 11), new(14, 11), new(20, 15), new(20, 30), new(14, 27), new(7, 27)
            };
            PointF[] rightPage =
            {
                new(33, 11), new(26, 11), new(20, 15), new(20, 30), new(26, 27), new(33, 27)
            };

            e.Graphics.DrawPolygon(pen, leftPage);
            e.Graphics.DrawPolygon(pen, rightPage);
            e.Graphics.DrawLine(pen, 20, 15, 20, 30);
            e.Graphics.DrawLine(pen, 7, 30, 15, 30);
            e.Graphics.DrawLine(pen, 25, 30, 33, 30);
        }

    }
}
