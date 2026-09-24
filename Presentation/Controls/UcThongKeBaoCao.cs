using BusinessLayer.Services;
using DataLayer.Models;
using Presentation.Helpers;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms.DataVisualization.Charting;

namespace Presentation.Controls
{
    public partial class UcThongKeBaoCao : UserControl
    {
        private const string SeriesMuonArea = "MuonArea";
        private const string SeriesTraArea = "TraArea";
        private const string SeriesMuon = "Số lượt mượn";
        private const string SeriesTra = "Số lượt trả";

        private readonly ThongKeService? _service;
        private bool _dangTai;
        private bool _daKhoiTao;
        private bool _laKhoangTuyChon;
        private int _tongDauSachTheoTheLoai;

        private sealed class BaoCaoQuaHanGridRow
        {
            public string NgayText { get; init; } = string.Empty;
            public string SoSachQuaHan { get; init; } = string.Empty;
            public string TienPhatText { get; init; } = string.Empty;
        }

        private sealed class PhieuNhapThangGridRow
        {
            public string Thang { get; init; } = string.Empty;
            public string SoPhieuNhap { get; init; } = string.Empty;
            public string TongSoBanSao { get; init; } = string.Empty;
            public string TongTienText { get; init; } = string.Empty;
        }

        public UcThongKeBaoCao()
        {
            InitializeComponent();
            if (DesignModeHelper.IsDesignMode(this)) return;

            components ??= new System.ComponentModel.Container();

            _service = new ThongKeService();
            CauHinhGiaoDien();
            Load += UcThongKeBaoCao_Load;
            dtpTuNgay.ValueChanged += BoLoc_ValueChanged;
            dtpDenNgay.ValueChanged += BoLoc_ValueChanged;
            cboLoaiThongKe.SelectedIndexChanged += BoLoc_ValueChanged;
            btnXuatBaoCao.Click += BtnXuatBaoCao_Click;
            btnXuatBaoCao.Enabled = CoQuyenXuatBaoCao();
            chartTheLoai.PostPaint += ChartTheLoai_PostPaint;
        }

        private void UcThongKeBaoCao_Load(object? sender, EventArgs e)
        {
            if (_daKhoiTao) return;

            // Chặn ValueChanged chạy TaiDuLieu trước khi các Series của Chart được tạo.
            // Đây là nguyên nhân gây lỗi: không tìm thấy "Số lượt mượn" trong SeriesCollection.
            _dangTai = true;
            try
            {
                KhoiTaoBieuDo();
                KhoiTaoMauBangDuLieu();
                KhoiTaoBoLoc();
                CapNhatThongTinNguoiDung();
                _daKhoiTao = true;
            }
            finally
            {
                _dangTai = false;
            }

            TaiDuLieu();
        }

        private void KhoiTaoBoLoc()
        {
            // Mỗi lựa chọn đại diện cho một khoảng thời gian rõ ràng, không dùng mục "Theo ngày"
            // vì mục này trước đây không cập nhật ngày bắt đầu và làm biểu đồ sai kỳ.
            cboLoaiThongKe.BeginUpdate();
            cboLoaiThongKe.Items.Clear();
            cboLoaiThongKe.Items.AddRange(new object[]
            {
                "7 ngày",
                "30 ngày",
                "Tháng này",
                "Tháng trước"
            });
            cboLoaiThongKe.EndUpdate();
            cboLoaiThongKe.Width = 190;
            cboLoaiThongKe.ItemHeight = 36;
            cboLoaiThongKe.Font = new Font("Segoe UI", 10F, FontStyle.Regular);

            DateTime today = DateTime.Today;
            dtpTuNgay.MaxDate = today;
            dtpDenNgay.MaxDate = today;
            dtpDenNgay.Value = today;
            dtpTuNgay.Value = today.AddDays(-6);
            cboLoaiThongKe.SelectedIndex = 0;
            _laKhoangTuyChon = false;
            CapNhatTieuDeBieuDo();
        }

        private void KhoiTaoBieuDo()
        {
            KhoiTaoBieuDoMuonTra();
            KhoiTaoBieuDoTheLoai();
        }

        private void KhoiTaoBieuDoMuonTra()
        {
            chartMuonTra.Series.Clear();
            chartMuonTra.ChartAreas.Clear();
            chartMuonTra.Legends.Clear();
            chartMuonTra.Titles.Clear();

            chartMuonTra.BorderSkin.SkinStyle = BorderSkinStyle.None;
            chartMuonTra.BackColor = Color.White;
            chartMuonTra.BorderlineColor = Color.Transparent;
            chartMuonTra.BorderlineWidth = 0;
            chartMuonTra.Palette = ChartColorPalette.None;
            chartMuonTra.AntiAliasing = AntiAliasingStyles.All;
            chartMuonTra.TextAntiAliasingQuality = TextAntiAliasingQuality.High;

            var area = new ChartArea("MainArea")
            {
                BackColor = Color.White,
                BackGradientStyle = GradientStyle.None,
                BackSecondaryColor = Color.White,
                BorderColor = Color.Transparent,
                BorderWidth = 0,
                ShadowColor = Color.Transparent
            };

            area.Position.Auto = false;
            area.Position.X = 1;
            area.Position.Y = 12;
            area.Position.Width = 98;
            area.Position.Height = 86;
            area.InnerPlotPosition.Auto = true;

            area.AxisX.MajorGrid.Enabled = false;
            area.AxisX.MajorTickMark.Enabled = false;
            area.AxisX.LineColor = Color.FromArgb(220, 224, 235);
            area.AxisX.Interval = 1;
            area.AxisX.IsMarginVisible = true;
            area.AxisX.LabelStyle.Font = new Font("Segoe UI", 8.5F);
            area.AxisX.LabelStyle.ForeColor = Color.FromArgb(70, 80, 110);

            area.AxisY.Minimum = 0;
            area.AxisY.Interval = 10;
            area.AxisY.LineColor = Color.Transparent;
            area.AxisY.MajorTickMark.Enabled = false;
            area.AxisY.MajorGrid.LineColor = Color.FromArgb(225, 230, 240);
            area.AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
            area.AxisY.LabelStyle.Font = new Font("Segoe UI", 8.5F);
            area.AxisY.LabelStyle.ForeColor = Color.FromArgb(70, 80, 110);
            chartMuonTra.ChartAreas.Add(area);

            var legend = new Legend("Legend")
            {
                Docking = Docking.Top,
                Alignment = StringAlignment.Near,
                LegendStyle = LegendStyle.Row,
                BackColor = Color.Transparent,
                BorderColor = Color.Transparent,
                ShadowColor = Color.Transparent,
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(40, 45, 80),
                IsTextAutoFit = false
            };
            legend.Position.Auto = false;
            legend.Position.X = 2;
            legend.Position.Y = 0;
            legend.Position.Width = 55;
            legend.Position.Height = 10;
            chartMuonTra.Legends.Add(legend);

            var muonArea = TaoSeriesVung(
                SeriesMuonArea,
                Color.FromArgb(90, 89, 42, 245),
                Color.FromArgb(0, 89, 42, 245));

            var traArea = TaoSeriesVung(
                SeriesTraArea,
                Color.FromArgb(80, 0, 170, 90),
                Color.FromArgb(0, 0, 170, 90));

            var muon = TaoSeriesDuong(
                SeriesMuon,
                Color.FromArgb(89, 42, 245),
                LabelAlignmentStyles.Top);

            var tra = TaoSeriesDuong(
                SeriesTra,
                Color.FromArgb(0, 170, 90),
                LabelAlignmentStyles.Bottom);

            chartMuonTra.Series.Add(muonArea);
            chartMuonTra.Series.Add(traArea);
            chartMuonTra.Series.Add(muon);
            chartMuonTra.Series.Add(tra);
        }

        private static Series TaoSeriesVung(string ten, Color mauDau, Color mauCuoi)
        {
            var series = new Series(ten)
            {
                ChartType = SeriesChartType.SplineArea,
                ChartArea = "MainArea",
                Color = mauDau,
                BackSecondaryColor = mauCuoi,
                BackGradientStyle = GradientStyle.TopBottom,
                BorderWidth = 0,
                IsVisibleInLegend = false,
                XValueType = ChartValueType.Int32
            };
            series.SetCustomProperty("LineTension", "0.4");
            return series;
        }

        private static Series TaoSeriesDuong(
            string ten,
            Color mau,
            LabelAlignmentStyles huongNhan)
        {
            var series = new Series(ten)
            {
                ChartType = SeriesChartType.Spline,
                ChartArea = "MainArea",
                Legend = "Legend",
                Color = mau,
                BorderWidth = 3,
                MarkerStyle = MarkerStyle.Circle,
                MarkerSize = 7,
                MarkerBorderWidth = 2,
                MarkerBorderColor = mau,
                MarkerColor = Color.White,
                IsValueShownAsLabel = true,
                LabelForeColor = mau,
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                XValueType = ChartValueType.Int32
            };
            series.SetCustomProperty("LineTension", "0.4");
            series.SmartLabelStyle.Enabled = true;
            series.SmartLabelStyle.AllowOutsidePlotArea = LabelOutsidePlotAreaStyle.Yes;
            series.SmartLabelStyle.MovingDirection = huongNhan;
            return series;
        }

        private void KhoiTaoBieuDoTheLoai()
        {
            chartTheLoai.Series.Clear();
            chartTheLoai.ChartAreas.Clear();
            chartTheLoai.Legends.Clear();
            chartTheLoai.Titles.Clear();

            chartTheLoai.BorderSkin.SkinStyle = BorderSkinStyle.None;
            chartTheLoai.BackColor = Color.White;
            chartTheLoai.BorderlineColor = Color.Transparent;
            chartTheLoai.BorderlineWidth = 0;
            chartTheLoai.Palette = ChartColorPalette.None;
            chartTheLoai.AntiAliasing = AntiAliasingStyles.All;
            chartTheLoai.TextAntiAliasingQuality = TextAntiAliasingQuality.High;

            var area = new ChartArea("CategoryArea")
            {
                BackColor = Color.White,
                BorderColor = Color.Transparent
            };
            area.Position.Auto = false;
            area.Position.X = 1;
            area.Position.Y = 1;
            area.Position.Width = 98;
            area.Position.Height = 98;
            area.InnerPlotPosition.Auto = true;
            chartTheLoai.ChartAreas.Add(area);

            var pie = new Series("TheLoai")
            {
                ChartType = SeriesChartType.Doughnut,
                ChartArea = "CategoryArea",
                IsValueShownAsLabel = false,
                IsVisibleInLegend = false,
                BorderColor = Color.White,
                BorderWidth = 2
            };
            pie["DoughnutRadius"] = "62";
            pie["PieStartAngle"] = "270";
            chartTheLoai.Series.Add(pie);

            // Nội dung giữa vòng tròn được vẽ bằng PostPaint để luôn nằm trên Chart.
            lblTongDanhMuc.Visible = false;
            lblTongDanhMucSub.Visible = false;
        }

        private void KhoiTaoMauBangDuLieu()
        {
        }

        private void BoLoc_ValueChanged(object? sender, EventArgs e)
        {
            if (!IsHandleCreated || _dangTai) return;

            if (sender == cboLoaiThongKe && cboLoaiThongKe.SelectedIndex >= 0)
            {
                _dangTai = true;
                try
                {
                    DateTime today = DateTime.Today;
                    DateTime start;
                    DateTime end;
                    switch (cboLoaiThongKe.SelectedIndex)
                    {
                        case 0: // 7 ngày gần nhất, tính cả hôm nay.
                            end = today;
                            start = end.AddDays(-6);
                            break;
                        case 1: // 30 ngày gần nhất, tính cả hôm nay.
                            end = today;
                            start = end.AddDays(-29);
                            break;
                        case 2: // Từ đầu tháng hiện tại đến hôm nay.
                            end = today;
                            start = new DateTime(today.Year, today.Month, 1);
                            break;
                        case 3: // Toàn bộ tháng liền trước.
                            start = new DateTime(today.Year, today.Month, 1).AddMonths(-1);
                            end = start.AddMonths(1).AddDays(-1);
                            break;
                        default:
                            return;
                    }

                    // Gán cả hai đầu khoảng; trước đây chỉ đổi Từ ngày nên "Tháng trước" không thể đúng.
                    dtpTuNgay.Value = start;
                    dtpDenNgay.Value = end;
                    _laKhoangTuyChon = false;
                    CapNhatTieuDeBieuDo();
                }
                finally
                {
                    _dangTai = false;
                }
            }
            else if (sender == dtpTuNgay || sender == dtpDenNgay)
            {
                // Người dùng sửa ngày thủ công: giữ dữ liệu theo khoảng tùy chọn và mô tả đúng trên tiêu đề.
                _laKhoangTuyChon = true;
                CapNhatTieuDeBieuDo();
            }

            TaiDuLieu();
        }

        private void CapNhatTieuDeBieuDo()
        {
            string period = _laKhoangTuyChon ? "KHOẢNG TÙY CHỌN" : cboLoaiThongKe.SelectedIndex switch
            {
                0 => "7 NGÀY GẦN NHẤT",
                1 => "30 NGÀY GẦN NHẤT",
                2 => "THÁNG NÀY",
                3 => "THÁNG TRƯỚC",
                _ => "KHOẢNG TÙY CHỌN"
            };
            lblChartTitle.Text = $"THỐNG KÊ MƯỢN - TRẢ {period}";
        }

        private void TaiDuLieu()
        {
            if (_service == null || _dangTai) return;
            DateTime tuNgay = dtpTuNgay.Value.Date;
            DateTime denNgay = dtpDenNgay.Value.Date;
            if (!KhoangNgayHopLe(tuNgay, denNgay, hienThiThongBao: true)) return;

            try
            {
                _dangTai = true;
                TaiSauCard(_service.GetThongKeCards(tuNgay, denNgay));
                TaiBieuDoMuonTra(tuNgay, denNgay);
                TaiTopSach(tuNgay, denNgay);
                TaiTopDocGia(tuNgay, denNgay);
                TaiThongKeTheLoai();
                TaiBaoCaoQuaHan(tuNgay, denNgay);
                TaiThongKePhieuNhap(denNgay);
                lblCapNhatLuc.Text = $"Số liệu được cập nhật đến {DateTime.Now:HH:mm:ss} ngày {DateTime.Now:dd/MM/yyyy}";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể tải dữ liệu thống kê.\n\n" + ex.Message,
                    "Thống kê & báo cáo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _dangTai = false;
            }
        }

        private void TaiSauCard(ThongKeCardModel model)
        {
            lblTongDauSach.Text = model.TongDauSach.ToString("N0");
            lblTongBanSao.Text = model.TongBanSao.ToString("N0");
            lblTongDocGia.Text = model.TongDocGia.ToString("N0");
            lblDangMuon.Text = model.DangMuon.ToString("N0");
            lblQuaHan.Text = model.QuaHanKyNay.ToString("N0");
            lblTongTienPhat.Text = $"{model.TienPhatKyNay:N0} đ";

            HienThiBienDong(lblTongDauSachSub, lblTongDauSachSub2, model.DauSachMoiKyNay, model.DauSachMoiKyTruoc, "đầu sách mới");
            HienThiBienDong(lblTongBanSaoSub, lblTongBanSaoSub2, model.BanSaoMoiKyNay, model.BanSaoMoiKyTruoc, "bản sao mới");
            HienThiBienDong(lblTongDocGiaSub, label2, model.DocGiaMoiKyNay, model.DocGiaMoiKyTruoc, "độc giả mới");
            HienThiBienDong(lblDangMuonSub, label3, model.LuotMuonKyNay, model.LuotMuonKyTruoc, "lượt mượn");
            HienThiBienDong(lblQuaHanSub, label6, model.QuaHanKyNay, model.QuaHanKyTruoc, "sách quá hạn");
            HienThiBienDongTien(lblTongTienPhatSub, label8, model.TienPhatKyNay, model.TienPhatKyTruoc);
        }

        private static void HienThiBienDong(Label lblBienDong, Label lblSoSanh,
            decimal hienTai, decimal kyTruoc, string donVi)
        {
            decimal chenhLech = hienTai - kyTruoc;
            lblSoSanh.Text = "so với kỳ trước";
            if (chenhLech == 0)
            {
                lblBienDong.Text = $"— Không đổi {donVi}";
                MetricTrendHelper.ApplyColor(lblBienDong, chenhLech);
                return;
            }
            bool tang = chenhLech > 0;
            lblBienDong.Text = $"{(tang ? "▲" : "▼")} {Math.Abs(chenhLech):N0} {donVi}";
            MetricTrendHelper.ApplyColor(lblBienDong, chenhLech);
        }

        private static void HienThiBienDongTien(Label lblBienDong, Label lblSoSanh, decimal hienTai, decimal kyTruoc)
        {
            decimal chenhLech = hienTai - kyTruoc;
            lblSoSanh.Text = "so với kỳ trước";
            if (chenhLech == 0)
            {
                lblBienDong.Text = "— Không đổi";
                MetricTrendHelper.ApplyColor(lblBienDong, chenhLech);
                return;
            }
            bool tang = chenhLech > 0;
            lblBienDong.Text = $"{(tang ? "▲" : "▼")} {Math.Abs(chenhLech):N0} đ";
            MetricTrendHelper.ApplyColor(lblBienDong, chenhLech);
        }

        private void TaiBieuDoMuonTra(DateTime tuNgay, DateTime denNgay)
        {
            if (!chartMuonTra.ChartAreas.Any(x => x.Name == "MainArea") ||
                !chartMuonTra.Series.Any(x => x.Name == SeriesMuonArea) ||
                !chartMuonTra.Series.Any(x => x.Name == SeriesTraArea) ||
                !chartMuonTra.Series.Any(x => x.Name == SeriesMuon) ||
                !chartMuonTra.Series.Any(x => x.Name == SeriesTra))
            {
                KhoiTaoBieuDoMuonTra();
            }

            var raw = _service!.GetBorrowReturn(tuNgay, denNgay);
            var points = TaoDiemBieuDo(raw.Labels, raw.Borrows, raw.Returns, tuNgay, denNgay);
            Series muonArea = chartMuonTra.Series[SeriesMuonArea];
            Series traArea = chartMuonTra.Series[SeriesTraArea];
            Series muon = chartMuonTra.Series[SeriesMuon];
            Series tra = chartMuonTra.Series[SeriesTra];

            foreach (Series series in new[] { muonArea, traArea, muon, tra }) series.Points.Clear();

            int maximum = 0;
            for (int i = 0; i < points.Count; i++)
            {
                var point = points[i];
                int indexMuonArea = muonArea.Points.AddXY(i, point.Muon);
                int indexTraArea = traArea.Points.AddXY(i, point.Tra);
                int indexMuon = muon.Points.AddXY(i, point.Muon);
                int indexTra = tra.Points.AddXY(i, point.Tra);

                foreach (DataPoint dataPoint in new[]
                         { muonArea.Points[indexMuonArea], traArea.Points[indexTraArea], muon.Points[indexMuon], tra.Points[indexTra] })
                {
                    dataPoint.AxisLabel = point.Nhan;
                }

                muon.Points[indexMuon].Label = point.Muon > 0 ? point.Muon.ToString("N0") : string.Empty;
                tra.Points[indexTra].Label = point.Tra > 0 ? point.Tra.ToString("N0") : string.Empty;
                maximum = Math.Max(maximum, Math.Max(point.Muon, point.Tra));
            }

            bool hasData = maximum > 0;
            muon.MarkerSize = tra.MarkerSize = points.Count > 14 ? 5 : 7;
            muon.IsValueShownAsLabel = tra.IsValueShownAsLabel = hasData && points.Count <= 14;

            ChartArea area = chartMuonTra.ChartAreas["MainArea"];
            int roundStep = maximum <= 20 ? 5 : maximum <= 100 ? 10 : 50;
            int axisMaximum = Math.Max(5, ((maximum + roundStep - 1) / roundStep) * roundStep);
            area.AxisY.Minimum = 0;
            area.AxisY.Maximum = axisMaximum;
            area.AxisY.Interval = Math.Max(1, axisMaximum / 5);
            area.AxisX.Interval = 1;
            area.AxisX.LabelStyle.Interval = 1;
            area.AxisX.LabelStyle.Angle = 0;
            area.AxisX.IsLabelAutoFit = true;
            area.AxisX.LabelAutoFitStyle = LabelAutoFitStyles.DecreaseFont;
            // Tiêu đề mô tả kỳ do combobox quyết định; chỉ bổ sung đơn vị gom nhóm khi cần.
            CapNhatTieuDeBieuDo();
            if (points.Count < raw.Labels.Length)
                lblChartTitle.Text += " • GOM THEO TUẦN";

            chartMuonTra.Annotations.Clear();
            if (!hasData)
            {
                chartMuonTra.Annotations.Add(new TextAnnotation
                {
                    Text = "Không có giao dịch mượn – trả trong khoảng thời gian đã chọn",
                    ForeColor = Color.FromArgb(111, 123, 151),
                    Font = new Font("Segoe UI", 10F, FontStyle.Italic),
                    X = 50,
                    Y = 48,
                    Alignment = ContentAlignment.MiddleCenter,
                    AnchorAlignment = ContentAlignment.MiddleCenter
                });
            }
            chartMuonTra.Invalidate();
        }

        private static List<(string Nhan, int Muon, int Tra)> TaoDiemBieuDo(
            string[] labels, int[] borrows, int[] returns, DateTime tuNgay, DateTime denNgay)
        {
            if (labels.Length <= 14)
            {
                return labels.Select((label, index) =>
                    (label, borrows.ElementAtOrDefault(index), returns.ElementAtOrDefault(index))).ToList();
            }

            var result = new List<(string Nhan, int Muon, int Tra)>();
            for (int start = 0; start < labels.Length; start += 7)
            {
                int count = Math.Min(7, labels.Length - start);
                DateTime weekStart = tuNgay.Date.AddDays(start);
                DateTime weekEnd = tuNgay.Date.AddDays(start + count - 1);
                string label = weekStart.Month == weekEnd.Month
                    ? $"{weekStart:dd}–{weekEnd:dd/MM}"
                    : $"{weekStart:dd/MM}–{weekEnd:dd/MM}";
                result.Add((label, borrows.Skip(start).Take(count).Sum(), returns.Skip(start).Take(count).Sum()));
            }
            return result;
        }

        private void TaiTopSach(DateTime tuNgay, DateTime denNgay)
        {
            flpTopSach.SuspendLayout();
            try
            {
                flpTopSach.Controls.Clear();
                var items = _service!.GetTopSachMuon(tuNgay, denNgay);
                if (items.Count == 0)
                {
                    flpTopSach.Controls.Add(TaoThongBaoKhongCoDuLieu(
                        flpTopSach,
                        "Không có lượt mượn sách trong khoảng đã chọn."));
                    return;
                }

                int stt = 1;
                foreach (var item in items)
                {
                    flpTopSach.Controls.Add(TaoDongXepHang(
                        stt++,
                        item.TenSach,
                        $"{item.SoLuotMuon:N0} lượt mượn",
                        item.AnhBia,
                        false));
                }
            }
            finally
            {
                flpTopSach.ResumeLayout();
            }
        }

        private void TaiTopDocGia(DateTime tuNgay, DateTime denNgay)
        {
            flpTopDocGia.SuspendLayout();
            try
            {
                flpTopDocGia.Controls.Clear();
                var items = _service!.GetTopDocGiaMuon(tuNgay, denNgay);
                if (items.Count == 0)
                {
                    flpTopDocGia.Controls.Add(TaoThongBaoKhongCoDuLieu(
                        flpTopDocGia,
                        "Không có lượt mượn của độc giả trong khoảng đã chọn."));
                    return;
                }

                int stt = 1;
                foreach (var item in items)
                {
                    flpTopDocGia.Controls.Add(TaoDongXepHang(
                        stt++,
                        item.HoTen,
                        $"{item.SoLuotMuon:N0} lượt mượn",
                        item.AnhDaiDien,
                        true));
                }
            }
            finally
            {
                flpTopDocGia.ResumeLayout();
            }
        }

        private static Control TaoThongBaoKhongCoDuLieu(FlowLayoutPanel flow, string noiDung)
        {
            return new Label
            {
                Text = noiDung,
                AutoSize = false,
                Width = Math.Max(220, flow.ClientSize.Width - 12),
                Height = 90,
                Margin = new Padding(0, 28, 0, 0),
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 9F, FontStyle.Italic),
                ForeColor = Color.FromArgb(115, 125, 150),
                BackColor = Color.White
            };
        }

        private Control TaoDongXepHang(int stt, string title, string sub, string? imagePath, bool avatar)
        {
            int width = avatar ? Math.Max(330, flpTopDocGia.ClientSize.Width - 8) : Math.Max(350, flpTopSach.ClientSize.Width - 8);
            var pnl = new Panel { Width = width, Height = 58, Margin = new Padding(0), BackColor = Color.White };
            var rank = new Label { Text = stt.ToString(), AutoSize = false, TextAlign = ContentAlignment.MiddleCenter, Location = new Point(3, 14), Size = new Size(26, 26), BackColor = Color.FromArgb(244, 246, 252), ForeColor = Color.FromArgb(50, 65, 110), Font = new Font("Segoe UI", 8F, FontStyle.Bold) };
            var pic = new PictureBox { Location = new Point(38, 7), Size = new Size(42, 44), SizeMode = PictureBoxSizeMode.Zoom, BackColor = Color.FromArgb(244, 246, 252) };
            pic.Image = avatar
                ? DatabaseImageHelper.LoadReaderAvatar(imagePath, title, 42, 44)
                : BookCoverImageHelper.LoadForGrid(imagePath, 0, 42, 44);
            var lblTitle = new Label { Text = title, Location = new Point(89, 7), Size = new Size(width - 95, 22), Font = new Font("Segoe UI", 8.5F, FontStyle.Bold), ForeColor = Color.FromArgb(35, 50, 90), AutoEllipsis = true };
            var lblSub = new Label { Text = sub, Location = new Point(89, 30), Size = new Size(width - 95, 20), Font = new Font("Segoe UI", 8F), ForeColor = Color.FromArgb(75, 88, 125) };
            pnl.Controls.AddRange(new Control[] { rank, pic, lblTitle, lblSub });
            return pnl;
        }

        private void TaiThongKeTheLoai()
        {
            if (!chartTheLoai.ChartAreas.Any(x => x.Name == "CategoryArea") ||
                !chartTheLoai.Series.Any(x => x.Name == "TheLoai"))
            {
                KhoiTaoBieuDoTheLoai();
            }

            var items = _service!.GetTopBookCategories(4);
            Series series = chartTheLoai.Series["TheLoai"];
            series.Points.Clear();
            flpChuThichTheLoai.Controls.Clear();

            Color[] colors =
            {
                Color.FromArgb(102, 77, 245),
                Color.FromArgb(42, 125, 245),
                Color.FromArgb(78, 204, 128),
                Color.FromArgb(255, 145, 20),
                Color.FromArgb(250, 45, 90)
            };

            _tongDauSachTheoTheLoai = _service.GetTotalBooks();

            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];
                Color mau = colors[i % colors.Length];
                int pointIndex = series.Points.AddY(item.SoLuongSach);
                DataPoint point = series.Points[pointIndex];
                point.Color = mau;
                point.BorderColor = Color.White;
                point.BorderWidth = 2;
                point.ToolTip = $"{item.TenTheLoai}: {item.SoLuongSach:N0} đầu sách ({item.TyLePhanTram:0.0}%)";

                var row = new Label
                {
                    Width = Math.Max(230, flpChuThichTheLoai.ClientSize.Width - 5),
                    Height = 28,
                    Margin = new Padding(0),
                    Font = new Font("Segoe UI", 8F),
                    ForeColor = mau,
                    Text = $"●  {item.TenTheLoai,-22} {item.SoLuongSach:N0} ({item.TyLePhanTram:0.0}%)"
                };
                flpChuThichTheLoai.Controls.Add(row);
            }

            chartTheLoai.Invalidate();
        }

        private void ChartTheLoai_PostPaint(object? sender, ChartPaintEventArgs e)
        {
            if (chartTheLoai.Series.Count == 0 ||
                chartTheLoai.Series[0].Points.Count == 0 ||
                chartTheLoai.ChartAreas.Count == 0)
            {
                return;
            }

            ChartArea area = chartTheLoai.ChartAreas[0];
            RectangleF areaRect = e.ChartGraphics.GetAbsoluteRectangle(area.Position.ToRectangleF());
            float size = Math.Min(areaRect.Width, areaRect.Height);
            float centerX = areaRect.X + areaRect.Width / 2F;
            float centerY = areaRect.Y + areaRect.Height / 2F;
            float hole = size * 0.45F;
            var centerRect = new RectangleF(
                centerX - hole / 2F,
                centerY - hole / 2F,
                hole,
                hole);

            var g = e.ChartGraphics.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            using (var whiteBrush = new SolidBrush(Color.White))
            {
                g.FillEllipse(whiteBrush, centerRect);
            }

            using var centerFormat = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };
            using var normalBrush = new SolidBrush(Color.FromArgb(75, 85, 115));
            using var valueBrush = new SolidBrush(Color.FromArgb(28, 37, 86));
            using var titleFont = new Font("Segoe UI", 8.5F, FontStyle.Regular);
            using var valueFont = new Font("Segoe UI", 13F, FontStyle.Bold);
            using var subFont = new Font("Segoe UI", 8F, FontStyle.Regular);

            float lineHeight = centerRect.Height / 3F;
            var titleRect = new RectangleF(centerRect.X, centerRect.Y + 1, centerRect.Width, lineHeight);
            var valueRect = new RectangleF(centerRect.X, centerRect.Y + lineHeight - 2, centerRect.Width, lineHeight + 5);
            var subRect = new RectangleF(centerRect.X, centerRect.Y + lineHeight * 2F, centerRect.Width, lineHeight);

            g.DrawString("Tổng", titleFont, normalBrush, titleRect, centerFormat);
            g.DrawString(_tongDauSachTheoTheLoai.ToString("N0"), valueFont, valueBrush, valueRect, centerFormat);
            g.DrawString("đầu sách", subFont, normalBrush, subRect, centerFormat);
        }

        private void TaiBaoCaoQuaHan(DateTime tuNgay, DateTime denNgay)
        {
            // Truy vấn đã có dữ liệu; bind qua model hiển thị để DataPropertyName của cột hoạt động ổn định.
            List<BaoCaoQuaHanGridRow> rows = _service!.GetBaoCaoQuaHanTheoNgay(tuNgay, denNgay)
                .Select(x => new BaoCaoQuaHanGridRow
                {
                    NgayText = x.Ngay.ToString("dd/MM/yyyy"),
                    SoSachQuaHan = x.SoSachQuaHan.ToString("N0"),
                    TienPhatText = $"{x.TienPhat:N0} đ"
                }).ToList();

            dgvBaoCaoQuaHan.AutoGenerateColumns = false;
            colNgayQuaHan.DataPropertyName = nameof(BaoCaoQuaHanGridRow.NgayText);
            colSoSachQuaHan.DataPropertyName = nameof(BaoCaoQuaHanGridRow.SoSachQuaHan);
            colTienPhatQuaHan.DataPropertyName = nameof(BaoCaoQuaHanGridRow.TienPhatText);
            dgvBaoCaoQuaHan.DataSource = null;
            dgvBaoCaoQuaHan.DataSource = rows;
            dgvBaoCaoQuaHan.ClearSelection();
        }

        private void TaiThongKePhieuNhap(DateTime denNgay)
        {
            List<PhieuNhapThangGridRow> rows = _service!.GetThongKePhieuNhapTheoThang(denNgay)
                .Select(x => new PhieuNhapThangGridRow
                {
                    Thang = x.Thang.ToString("MM/yyyy"),
                    SoPhieuNhap = x.SoPhieuNhap.ToString("N0"),
                    TongSoBanSao = x.TongSoBanSaoNhap.ToString("N0"),
                    TongTienText = $"{x.TongTienNhap:N0} đ"
                }).ToList();

            dgvThongKePhieuNhap.AutoGenerateColumns = false;
            colThang.DataPropertyName = nameof(PhieuNhapThangGridRow.Thang);
            colSoPhieuNhap.DataPropertyName = nameof(PhieuNhapThangGridRow.SoPhieuNhap);
            colSoBanSaoNhap.DataPropertyName = nameof(PhieuNhapThangGridRow.TongSoBanSao);
            colTongTienNhap.DataPropertyName = nameof(PhieuNhapThangGridRow.TongTienText);
            dgvThongKePhieuNhap.DataSource = null;
            dgvThongKePhieuNhap.DataSource = rows;
            dgvThongKePhieuNhap.ClearSelection();
        }

        private void CapNhatThongTinNguoiDung()
        {
            lblUser.Text = string.IsNullOrWhiteSpace(CurrentUser.HoTen) ? "Xin chào" : $"Xin chào, {CurrentUser.HoTen}";
            lblRole.Text = string.IsNullOrWhiteSpace(CurrentUser.VaiTro) ? "Người dùng" : CurrentUser.VaiTro;
        }


        private static bool CoQuyenXuatBaoCao() =>
            PermissionHelper.CanExport("BAOCAO.MUONTRA") ||
            PermissionHelper.CanExport("BAOCAO.SACH") ||
            PermissionHelper.CanExport("BAOCAO.DOCGIA");

        private void BtnXuatBaoCao_Click(object? sender, EventArgs e)
        {
            if (!CoQuyenXuatBaoCao())
            {
                MessageBox.Show("Bạn không có quyền xuất báo cáo.", "Không đủ quyền", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            DateTime tuNgay = dtpTuNgay.Value.Date;
            DateTime denNgay = dtpDenNgay.Value.Date;
            if (!KhoangNgayHopLe(tuNgay, denNgay, hienThiThongBao: true)) return;

            using var dialog = new SaveFileDialog
            {
                Filter = "Excel Workbook (*.xlsx)|*.xlsx|Tệp PDF (*.pdf)|*.pdf|Tệp CSV (*.csv)|*.csv",
                FileName = $"ThongKeBaoCao_{tuNgay:ddMMyyyy}_{denNgay:ddMMyyyy}.xlsx"
            };
            if (dialog.ShowDialog() != DialogResult.OK) return;

            try
            {
                string extension = Path.GetExtension(dialog.FileName).ToLowerInvariant();
                if (extension == ".xlsx")
                {
                    string[] headers = { "Mục thống kê / Ngày", "Chỉ tiêu / Số lượng", "Số tiền / Chi tiết" };
                    List<string[]> rows = new()
                    {
                        new[] { "Khoảng thời gian", $"{tuNgay:dd/MM/yyyy} - {denNgay:dd/MM/yyyy}", "-" },
                        new[] { "Tổng số đầu sách", lblTongDauSach.Text, "-" },
                        new[] { "Tổng số bản sao", lblTongBanSao.Text, "-" },
                        new[] { "Tổng số độc giả", lblTongDocGia.Text, "-" },
                        new[] { "Đang mượn", lblDangMuon.Text, "-" },
                        new[] { "Sách quá hạn", lblQuaHan.Text, "-" },
                        new[] { "Tổng tiền phạt", lblTongTienPhat.Text, "-" }
                    };

                    foreach (DataGridViewRow row in dgvBaoCaoQuaHan.Rows)
                    {
                        if (!row.IsNewRow)
                        {
                            rows.Add(new[] {
                                $"Ngày quá hạn: {row.Cells[0].Value}",
                                $"{row.Cells[1].Value} cuốn",
                                $"{row.Cells[2].Value}"
                            });
                        }
                    }

                    ExcelHelper.ExportToXlsx(dialog.FileName, "Thống kê báo cáo", headers, rows);
                }
                else if (extension == ".pdf")
                {
                    XuatBaoCaoPdf(dialog.FileName, tuNgay, denNgay);
                }
                else
                {
                    var sb = new StringBuilder();
                    sb.AppendLine("THỐNG KÊ & BÁO CÁO THƯ VIỆN EPU");
                    sb.AppendLine($"Khoảng thời gian;{dtpTuNgay.Value:dd/MM/yyyy};{dtpDenNgay.Value:dd/MM/yyyy}");
                    sb.AppendLine();
                    sb.AppendLine("Chỉ tiêu;Giá trị");
                    sb.AppendLine($"Tổng số đầu sách;{lblTongDauSach.Text}");
                    sb.AppendLine($"Tổng số bản sao;{lblTongBanSao.Text}");
                    sb.AppendLine($"Tổng số độc giả;{lblTongDocGia.Text}");
                    sb.AppendLine($"Đang mượn;{lblDangMuon.Text}");
                    sb.AppendLine($"Sách quá hạn;{lblQuaHan.Text}");
                    sb.AppendLine($"Tổng tiền phạt;{lblTongTienPhat.Text}");
                    File.WriteAllText(dialog.FileName, sb.ToString(), new UTF8Encoding(true));
                }
                MessageBox.Show("Xuất báo cáo thành công.", "Thống kê & báo cáo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể xuất báo cáo.\n\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void XuatBaoCaoPdf(string filePath, DateTime tuNgay, DateTime denNgay)
        {
            try
            {
                using System.Drawing.Printing.PrintDocument pd = new();
                pd.PrinterSettings.PrinterName = "Microsoft Print to PDF";
                pd.PrinterSettings.PrintToFile = true;
                pd.PrinterSettings.PrintFileName = filePath;

                pd.PrintPage += (s, e) =>
                {
                    Graphics g = e.Graphics!;
                    float y = 40;
                    using Font titleFont = new("Segoe UI", 16, FontStyle.Bold);
                    using Font subFont = new("Segoe UI", 10, FontStyle.Bold);
                    using Font bodyFont = new("Segoe UI", 10);

                    g.DrawString("THỐNG KÊ & BÁO CÁO THƯ VIỆN EPU", titleFont, Brushes.Navy, 50, y); y += 40;
                    g.DrawString($"Khoảng thời gian: {tuNgay:dd/MM/yyyy} - {denNgay:dd/MM/yyyy}", subFont, Brushes.Black, 50, y); y += 30;
                    g.DrawString($"Tổng số đầu sách: {lblTongDauSach.Text}", bodyFont, Brushes.Black, 50, y); y += 22;
                    g.DrawString($"Tổng số bản sao: {lblTongBanSao.Text}", bodyFont, Brushes.Black, 50, y); y += 22;
                    g.DrawString($"Tổng số độc giả: {lblTongDocGia.Text}", bodyFont, Brushes.Black, 50, y); y += 22;
                    g.DrawString($"Đang mượn: {lblDangMuon.Text}", bodyFont, Brushes.Black, 50, y); y += 22;
                    g.DrawString($"Sách quá hạn: {lblQuaHan.Text}", bodyFont, Brushes.Black, 50, y); y += 22;
                    g.DrawString($"Tổng tiền phạt: {lblTongTienPhat.Text}", bodyFont, Brushes.Black, 50, y); y += 30;
                };

                if (pd.PrinterSettings.IsValid)
                {
                    pd.Print();
                }
                else
                {
                    StringBuilder sb = new();
                    sb.AppendLine("%PDF-1.4");
                    sb.AppendLine("%THỐNG KÊ & BÁO CÁO THƯ VIỆN EPU");
                    sb.AppendLine($"%Khoảng thời gian: {tuNgay:dd/MM/yyyy} - {denNgay:dd/MM/yyyy}");
                    sb.AppendLine($"%Tổng số đầu sách: {lblTongDauSach.Text}");
                    sb.AppendLine($"%Tổng số bản sao: {lblTongBanSao.Text}");
                    sb.AppendLine($"%Tổng số độc giả: {lblTongDocGia.Text}");
                    sb.AppendLine($"%Đang mượn: {lblDangMuon.Text}");
                    sb.AppendLine($"%Sách quá hạn: {lblQuaHan.Text}");
                    sb.AppendLine($"%Tổng tiền phạt: {lblTongTienPhat.Text}");
                    File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);
                }
            }
            catch
            {
                StringBuilder sb = new();
                sb.AppendLine("%PDF-1.4");
                sb.AppendLine("%THỐNG KÊ & BÁO CÁO THƯ VIỆN EPU");
                sb.AppendLine($"%Khoảng thời gian: {tuNgay:dd/MM/yyyy} - {denNgay:dd/MM/yyyy}");
                sb.AppendLine($"%Tổng số đầu sách: {lblTongDauSach.Text}");
                sb.AppendLine($"%Tổng số bản sao: {lblTongBanSao.Text}");
                sb.AppendLine($"%Tổng số độc giả: {lblTongDocGia.Text}");
                sb.AppendLine($"%Đang mượn: {lblDangMuon.Text}");
                sb.AppendLine($"%Sách quá hạn: {lblQuaHan.Text}");
                sb.AppendLine($"%Tổng tiền phạt: {lblTongTienPhat.Text}");
                File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);
            }
        }

        private static bool KhoangNgayHopLe(DateTime tuNgay, DateTime denNgay, bool hienThiThongBao)
        {
            if (tuNgay.Date > denNgay.Date)
            {
                if (hienThiThongBao)
                {
                    MessageBox.Show("Từ ngày không được lớn hơn đến ngày.", "Bộ lọc không hợp lệ",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                return false;
            }

            if (denNgay.Date > DateTime.Today)
            {
                if (hienThiThongBao)
                {
                    MessageBox.Show("Ngày kết thúc không được lớn hơn ngày hiện tại.", "Bộ lọc không hợp lệ",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                return false;
            }

            return true;
        }

        private static string EscapeCsv(string? value)
        {
            string text = value ?? string.Empty;
            return text.IndexOfAny(new[] { ';', '"', '\r', '\n' }) >= 0
                ? $"\"{text.Replace("\"", "\"\"")}\""
                : text;
        }

        private static string LayChuCaiDau(string text)
        {
            return string.Join("", text.Split(' ', StringSplitOptions.RemoveEmptyEntries).Take(2).Select(x => char.ToUpperInvariant(x[0])));
        }


        private void CauHinhGiaoDien()
        {
            SuspendLayout();
            BackColor = Color.FromArgb(248, 250, 255);
            lblTitle.Font = new Font("Segoe UI", 17F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(23, 38, 89);
            lblSubTitle.Font = new Font("Segoe UI", 9.5F);
            lblSubTitle.ForeColor = Color.FromArgb(103, 115, 148);
            lblCapNhatLuc.Font = new Font("Segoe UI", 8.5F);
            lblCapNhatLuc.ForeColor = Color.FromArgb(105, 119, 153);
            picAvatar.BorderStyle = BorderStyle.None;
            lblUser.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblRole.Font = new Font("Segoe UI", 9F);
            lblRole.ForeColor = Color.FromArgb(100, 111, 142);
            btnNotify.Visible = false;
            lblNotificationCount.Visible = false;

            btnXuatBaoCao.Animated = true;
            btnXuatBaoCao.Cursor = Cursors.Hand;
            btnXuatBaoCao.BorderThickness = 0;
            btnXuatBaoCao.BorderRadius = 9;
            btnXuatBaoCao.FillColor = Color.FromArgb(35, 99, 235);
            btnXuatBaoCao.HoverState.FillColor = Color.FromArgb(28, 78, 216);
            btnXuatBaoCao.ShadowDecoration.Enabled = true;
            btnXuatBaoCao.ShadowDecoration.Depth = 4;
            btnXuatBaoCao.ShadowDecoration.Color = Color.FromArgb(35, 99, 235);

            foreach (var picker in new[] { dtpTuNgay, dtpDenNgay })
            {
                picker.BorderColor = Color.FromArgb(218, 225, 239);
                picker.FocusedColor = Color.FromArgb(35, 99, 235);
                picker.FillColor = Color.White;
                picker.BorderRadius = 8;
            }
            cboLoaiThongKe.BorderColor = Color.FromArgb(218, 225, 239);
            cboLoaiThongKe.FocusedState.BorderColor = Color.FromArgb(35, 99, 235);

            TrangTriCard(cardTongDauSach, Color.FromArgb(35, 99, 235));
            TrangTriCard(cardTongBanSao, Color.FromArgb(21, 167, 91));
            TrangTriCard(cardTongDocGia, Color.FromArgb(247, 99, 32));
            TrangTriCard(cardDangMuon, Color.FromArgb(134, 51, 218));
            TrangTriCard(cardQuaHan, Color.FromArgb(239, 68, 88));
            TrangTriCard(cardTongTienPhat, Color.FromArgb(15, 151, 137));

            foreach (var panel in new[]
                     { pnlBieuDoMuonTra, pnlTopSach, pnlTopDocGia, pnlThongKeTheLoai, pnlBaoCaoQuaHan, pnlThongKePhieuNhap })
            {
                panel.BorderColor = Color.FromArgb(225, 231, 243);
                panel.BorderRadius = 12;
                panel.BorderThickness = 1;
                panel.FillColor = Color.White;
                panel.ShadowDecoration.Enabled = true;
                panel.ShadowDecoration.Depth = 4;
                panel.ShadowDecoration.Color = Color.FromArgb(145, 125, 220);
            }

            foreach (Label title in new[] { lblChartTitle, label1, label10, lblTopDanhMuc, label12, label15 })
            {
                title.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
                title.ForeColor = Color.FromArgb(27, 43, 91);
            }

            ResumeLayout(false);
        }

        private static void TrangTriCard(Guna.UI2.WinForms.Guna2Panel card, Color accent)
        {
            card.BorderColor = Color.FromArgb(225, 231, 243);
            card.BorderRadius = 12;
            card.BorderThickness = 1;
            card.FillColor = Color.White;
            card.ShadowDecoration.Enabled = true;
            card.ShadowDecoration.Depth = 5;
            card.ShadowDecoration.Color = Color.FromArgb(145, 125, 220);
            foreach (Label label in card.Controls.OfType<Label>())
                if (label.Font.Size >= 14) label.ForeColor = accent;
        }




    }
}
