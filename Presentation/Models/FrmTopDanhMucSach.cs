using BusinessLayer.Services;
using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Presentation.Helpers;

namespace Presentation.Models
{
    public partial class FrmTopDanhMucSach : Form
    {
        private readonly ThongKeService _thongKeService = null!;

        private List<TopDanhMucThongKeModel> _duLieu =
            new List<TopDanhMucThongKeModel>();

        private bool _dangKhoiTaoComboBox;

        private readonly Color[] _mauBieuDo =
        {
            Color.FromArgb(70, 130, 245),
            Color.FromArgb(55, 190, 120),
            Color.FromArgb(255, 145, 35),
            Color.FromArgb(125, 70, 225),
            Color.FromArgb(45, 185, 190),
            Color.FromArgb(235, 75, 155),
            Color.FromArgb(100, 165, 235),
            Color.FromArgb(250, 195, 40)
        };

        public FrmTopDanhMucSach()
        {
            InitializeComponent();

            if (DesignModeHelper.IsDesignMode(this))
                return;

            _thongKeService = new ThongKeService();

            CauHinhForm();
            CauHinhDataGridView();
            CauHinhComboBox();
            GanSuKien();
        }

        #region Khởi tạo

        private void CauHinhForm()
        {
            KeyPreview = true;

            // Cho phép kéo Form bằng thanh tiêu đề.
            guna2DragControl1.TargetControl = pnlTitleBar;

            // Tạo bóng cho Form.
            guna2ShadowForm1.SetShadowForm(this);

            // Không cho DataGridView tự tạo cột.
            dgvTopDanhMuc.AutoGenerateColumns = false;
        }

        private void CauHinhComboBox()
        {
            _dangKhoiTaoComboBox = true;

            cboKieuThongKe.Items.Clear();

            cboKieuThongKe.Items.AddRange(
                new object[]
                {
                    "Theo số lượng sách",
                    "Theo sách có sẵn",
                    "Theo sách đang mượn",
                    "Theo sách quá hạn"
                });

            cboKieuThongKe.SelectedIndex = 0;

            _dangKhoiTaoComboBox = false;
        }

        private void GanSuKien()
        {
            Load -= FrmTopDanhMucSach_Load;
            Load += FrmTopDanhMucSach_Load;

            btnDong.Click -= btnDong_Click;
            btnDong.Click += btnDong_Click;


            cboThang.SelectedIndexChanged -=
                cboThang_SelectedIndexChanged;

            cboThang.SelectedIndexChanged +=
                cboThang_SelectedIndexChanged;

            cboKieuThongKe.SelectedIndexChanged -=
                cboKieuThongKe_SelectedIndexChanged;

            cboKieuThongKe.SelectedIndexChanged +=
                cboKieuThongKe_SelectedIndexChanged;

            dgvTopDanhMuc.CellFormatting -=
                dgvTopDanhMuc_CellFormatting;

            dgvTopDanhMuc.CellFormatting +=
                dgvTopDanhMuc_CellFormatting;

            dgvTopDanhMuc.CellPainting -=
                dgvTopDanhMuc_CellPainting;

            dgvTopDanhMuc.CellPainting +=
                dgvTopDanhMuc_CellPainting;

            dgvTopDanhMuc.CellDoubleClick -=
                dgvTopDanhMuc_CellDoubleClick;

            dgvTopDanhMuc.CellDoubleClick +=
                dgvTopDanhMuc_CellDoubleClick;

            KeyDown -= FrmTopDanhMucSach_KeyDown;
            KeyDown += FrmTopDanhMucSach_KeyDown;
        }

        private void FrmTopDanhMucSach_Load(
            object? sender,
            EventArgs e)
        {
            TaoDanhSachThang();
            TaiDuLieu();
        }

        #endregion

        #region ComboBox tháng

        private void TaoDanhSachThang()
        {
            _dangKhoiTaoComboBox = true;

            cboThang.DataSource = null;
            cboThang.Items.Clear();

            List<ThangThongKeItem> danhSachThang =
                new List<ThangThongKeItem>();

            DateTime hienTai = DateTime.Today;

            // Hiển thị 24 tháng gần nhất.
            for (int i = 0; i < 24; i++)
            {
                DateTime ngay =
                    new DateTime(
                        hienTai.Year,
                        hienTai.Month,
                        1)
                    .AddMonths(-i);

                string tenHienThi;

                if (i == 0)
                {
                    tenHienThi =
                        $"Tháng này: {ngay:MM/yyyy}";
                }
                else if (i == 1)
                {
                    tenHienThi =
                        $"Tháng trước: {ngay:MM/yyyy}";
                }
                else
                {
                    tenHienThi = ngay.ToString("MM/yyyy");
                }

                danhSachThang.Add(
                    new ThangThongKeItem
                    {
                        Thang = ngay.Month,
                        Nam = ngay.Year,
                        TenHienThi = tenHienThi
                    });
            }

            cboThang.DisplayMember =
                nameof(ThangThongKeItem.TenHienThi);

            cboThang.ValueMember =
                nameof(ThangThongKeItem.GiaTri);

            cboThang.DataSource = danhSachThang;
            cboThang.SelectedIndex = 0;

            _dangKhoiTaoComboBox = false;
        }

        private ThangThongKeItem LayThangDangChon()
        {
            if (cboThang.SelectedItem
                is ThangThongKeItem item)
            {
                return item;
            }

            return new ThangThongKeItem
            {
                Thang = DateTime.Today.Month,
                Nam = DateTime.Today.Year,
                TenHienThi =
                    $"Tháng này: {DateTime.Today:MM/yyyy}"
            };
        }

        private void cboThang_SelectedIndexChanged(
            object? sender,
            EventArgs e)
        {
            if (_dangKhoiTaoComboBox ||
                !IsHandleCreated)
            {
                return;
            }

            TaiDuLieu();
        }

        #endregion

        #region Tải dữ liệu

        private void TaiDuLieu()
        {
            try
            {
                BatTrangThaiDangTai(true);

                ThangThongKeItem thangDangChon =
                    LayThangDangChon();

                _duLieu = _thongKeService.GetTopDanhMuc(
                    thangDangChon.Thang,
                    thangDangChon.Nam);

                CapNhatCacTheThongKe();
                HienThiBang();
                VeBieuDo();
            }
            catch (Exception ex)
            {
                _duLieu.Clear();

                CapNhatCacTheThongKe();
                HienThiBang();
                VeBieuDo();

                MessageBox.Show(
                    "Không thể tải dữ liệu thống kê danh mục sách."
                    + Environment.NewLine
                    + Environment.NewLine
                    + ex.Message,
                    "Lỗi dữ liệu",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                BatTrangThaiDangTai(false);
            }
        }

        private void BatTrangThaiDangTai(bool dangTai)
        {
            Cursor = dangTai
                ? Cursors.WaitCursor
                : Cursors.Default;

            cboThang.Enabled = !dangTai;
            cboKieuThongKe.Enabled = !dangTai;
        }

        private void CapNhatCacTheThongKe()
        {
            int tongDanhMuc = _duLieu.Count;

            int tongSach =
                _duLieu.Sum(item => item.TongSach);

            int tongCoSan =
                _duLieu.Sum(item => item.CoSan);

            int tongDangMuon =
                _duLieu.Sum(item => item.DangMuon);

            int tongQuaHan =
                _duLieu.Sum(item => item.QuaHan);

            lblTongDanhMuc.Text =
                tongDanhMuc.ToString("N0");

            lblTongSach.Text =
                tongSach.ToString("N0");

            lblCoSan.Text =
                tongCoSan.ToString("N0");

            lblDangMuon.Text =
                tongDangMuon.ToString("N0");

            lblQuaHan.Text =
                tongQuaHan.ToString("N0");
        }

        #endregion

        #region DataGridView

        private void CauHinhDataGridView()
        {
            dgvTopDanhMuc.AutoGenerateColumns = false;

            dgvTopDanhMuc.AllowUserToAddRows = false;
            dgvTopDanhMuc.AllowUserToDeleteRows = false;
            dgvTopDanhMuc.AllowUserToResizeRows = false;
            dgvTopDanhMuc.AllowUserToOrderColumns = false;

            dgvTopDanhMuc.ReadOnly = true;
            dgvTopDanhMuc.RowHeadersVisible = false;
            dgvTopDanhMuc.MultiSelect = false;

            dgvTopDanhMuc.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            dgvTopDanhMuc.EnableHeadersVisualStyles = false;

            dgvTopDanhMuc.ColumnHeadersHeight = 58;

            dgvTopDanhMuc.ColumnHeadersHeightSizeMode =
                DataGridViewColumnHeadersHeightSizeMode
                    .DisableResizing;

            dgvTopDanhMuc.RowTemplate.Height = 71;

            dgvTopDanhMuc.BackgroundColor = Color.White;

            dgvTopDanhMuc.BorderStyle =
                BorderStyle.None;

            dgvTopDanhMuc.CellBorderStyle =
                DataGridViewCellBorderStyle.Single;

            dgvTopDanhMuc.GridColor =
                Color.FromArgb(225, 230, 240);

            dgvTopDanhMuc.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(245, 247, 252);

            dgvTopDanhMuc.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(60, 70, 90);

            dgvTopDanhMuc.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);

            dgvTopDanhMuc.ColumnHeadersDefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleCenter;

            dgvTopDanhMuc.ColumnHeadersDefaultCellStyle.WrapMode =
                DataGridViewTriState.True;

            dgvTopDanhMuc.DefaultCellStyle.BackColor = Color.FromArgb(245, 247, 252);

            dgvTopDanhMuc.DefaultCellStyle.ForeColor = Color.FromArgb(60, 70, 90);

            dgvTopDanhMuc.DefaultCellStyle.Font =
                new Font(
                    "Segoe UI",
                    9F,
                    FontStyle.Bold);

            dgvTopDanhMuc.DefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleLeft;
            dgvTopDanhMuc.DefaultCellStyle.WrapMode =
                DataGridViewTriState.True;

            dgvTopDanhMuc.DefaultCellStyle.SelectionBackColor = Color.FromArgb(245, 247, 252);

            dgvTopDanhMuc.DefaultCellStyle.SelectionForeColor = Color.FromArgb(60, 70, 90);

            dgvTopDanhMuc.AlternatingRowsDefaultCellStyle.BackColor =
                Color.FromArgb(251, 252, 255);

            // Gán đúng DataPropertyName cho các cột đã design.
            colSTT.DataPropertyName =
                nameof(TopDanhMucGridRow.STT);

            colDanhMuc.DataPropertyName =
                nameof(TopDanhMucGridRow.TenDanhMuc);

            colTongSach.DataPropertyName =
                nameof(TopDanhMucGridRow.TongSach);

            colCoSan.DataPropertyName =
                nameof(TopDanhMucGridRow.CoSan);

            colDangMuon.DataPropertyName =
                nameof(TopDanhMucGridRow.DangMuon);

            colQuaHan.DataPropertyName =
                nameof(TopDanhMucGridRow.QuaHan);

            colTyLe.DataPropertyName =
                nameof(TopDanhMucGridRow.TyLeText);

            colSTT.HeaderText = "STT";
            colDanhMuc.HeaderText = "Danh mục";

            colTongSach.HeaderText =
                "Tổng số sách\n(cuốn)";

            colCoSan.HeaderText =
                "Đang có sẵn\n(cuốn)";

            colDangMuon.HeaderText =
                "Đang mượn\n(cuốn)";

            colQuaHan.HeaderText =
                "Quá hạn\n(cuốn)";

            colTyLe.HeaderText =
                "Tỷ lệ %";

            colSTT.Width = 48;
            colSTT.MinimumWidth = 45;

            colDanhMuc.AutoSizeMode =
                DataGridViewAutoSizeColumnMode.Fill;

            colDanhMuc.MinimumWidth = 145;
            colDanhMuc.FillWeight = 180;

            colTongSach.Width = 115;
            colCoSan.Width = 110;
            colDangMuon.Width = 110;
            colQuaHan.Width = 95;
            colTyLe.Width = 130;

            colDanhMuc.DefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleLeft;

            colDanhMuc.DefaultCellStyle.Padding =
                new Padding(10, 0, 5, 0);

            colTongSach.DefaultCellStyle.Format = "N0";
            colCoSan.DefaultCellStyle.Format = "N0";
            colDangMuon.DefaultCellStyle.Format = "N0";
            colQuaHan.DefaultCellStyle.Format = "N0";

            colSTT.SortMode =
                DataGridViewColumnSortMode.NotSortable;

            colTyLe.SortMode =
                DataGridViewColumnSortMode.NotSortable;
        }

        private void HienThiBang()
        {
            List<TopDanhMucGridRow> danhSachHienThi =
                _duLieu
                    .Select((item, index) =>
                        new TopDanhMucGridRow
                        {
                            STT = (index + 1).ToString(),

                            MaDanhMuc =
                                item.MaDanhMuc,

                            TenDanhMuc =
                                item.TenDanhMuc,

                            TongSach =
                                item.TongSach,

                            CoSan =
                                item.CoSan,

                            DangMuon =
                                item.DangMuon,

                            QuaHan =
                                item.QuaHan,

                            TyLe =
                                item.TyLe,

                            TyLeText =
                                $"{item.TyLe:0.00}%",

                            LaDongTongCong = false
                        })
                    .ToList();

            if (_duLieu.Count > 0)
            {
                danhSachHienThi.Add(
                    new TopDanhMucGridRow
                    {
                        STT = string.Empty,

                        MaDanhMuc = 0,

                        TenDanhMuc =
                            "TỔNG CỘNG",

                        TongSach =
                            _duLieu.Sum(x => x.TongSach),

                        CoSan =
                            _duLieu.Sum(x => x.CoSan),

                        DangMuon =
                            _duLieu.Sum(x => x.DangMuon),

                        QuaHan =
                            _duLieu.Sum(x => x.QuaHan),

                        TyLe = 100,

                        TyLeText = "100%",

                        LaDongTongCong = true
                    });
            }

            dgvTopDanhMuc.DataSource = null;
            dgvTopDanhMuc.DataSource = danhSachHienThi;

            dgvTopDanhMuc.ClearSelection();
            if (dgvTopDanhMuc.Rows.Count > 0)
                dgvTopDanhMuc.FirstDisplayedScrollingRowIndex = 0;

        }

        private void dgvTopDanhMuc_CellFormatting(
            object? sender,
            DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 ||
                e.ColumnIndex < 0)
            {
                return;
            }

            DataGridViewRow gridRow =
                dgvTopDanhMuc.Rows[e.RowIndex];

            if (gridRow.DataBoundItem
                is not TopDanhMucGridRow row)
            {
                return;
            }

            string tenCot =
                dgvTopDanhMuc
                    .Columns[e.ColumnIndex]
                    .Name;

            if (row.LaDongTongCong)
            {
                e.CellStyle.BackColor =
                    Color.FromArgb(244, 247, 253);

                e.CellStyle.SelectionBackColor =
                    Color.FromArgb(244, 247, 253);

                e.CellStyle.ForeColor =
                    Color.FromArgb(20, 45, 105);

                e.CellStyle.SelectionForeColor =
                    Color.FromArgb(20, 45, 105);

                e.CellStyle.Font =
                    new Font(
                        "Segoe UI",
                        9F,
                        FontStyle.Bold);

                if (tenCot == "colDanhMuc")
                {
                    e.CellStyle.Alignment =
                        DataGridViewContentAlignment
                            .MiddleCenter;
                }

                return;
            }

            if (tenCot == "colDanhMuc")
            {
                e.CellStyle.Alignment =
                    DataGridViewContentAlignment
                        .MiddleLeft;
            }

            if (tenCot == "colTongSach")
            {
                e.CellStyle.Font =
                    new Font(
                        "Segoe UI",
                        8.5F,
                        FontStyle.Bold);

                e.CellStyle.ForeColor =
                    Color.FromArgb(25, 45, 95);
            }

            if (tenCot == "colCoSan")
            {
                e.CellStyle.ForeColor =
                    Color.FromArgb(30, 145, 90);
            }

            if (tenCot == "colDangMuon")
            {
                e.CellStyle.ForeColor =
                    Color.FromArgb(235, 125, 25);
            }

            if (tenCot == "colQuaHan" &&
                row.QuaHan > 0)
            {
                e.CellStyle.ForeColor =
                    Color.FromArgb(235, 55, 65);

                e.CellStyle.Font =
                    new Font(
                        "Segoe UI",
                        8.5F,
                        FontStyle.Bold);
            }
        }

        private void dgvTopDanhMuc_CellPainting(
            object? sender,
            DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 ||
                e.ColumnIndex < 0)
            {
                return;
            }

            if (dgvTopDanhMuc
                    .Columns[e.ColumnIndex]
                    .Name != "colTyLe")
            {
                return;
            }

            if (dgvTopDanhMuc.Rows[e.RowIndex]
                    .DataBoundItem
                is not TopDanhMucGridRow row)
            {
                return;
            }

            if (row.LaDongTongCong)
            {
                e.PaintBackground(
                    e.CellBounds,
                    false);

                TextRenderer.DrawText(
                    e.Graphics,
                    "100%",
                    new Font(
                        "Segoe UI",
                        9F,
                        FontStyle.Bold),
                    e.CellBounds,
                    Color.FromArgb(20, 45, 105),
                    TextFormatFlags.HorizontalCenter |
                    TextFormatFlags.VerticalCenter);

                e.Handled = true;
                return;
            }

            e.PaintBackground(
                e.CellBounds,
                true);

            double tyLe =
                Math.Max(
                    0,
                    Math.Min(100, row.TyLe));

            int khoangCachTrai = 8;
            int chieuRongThanh = 55;
            int chieuCaoThanh = 8;

            Rectangle thanhNen =
                new Rectangle(
                    e.CellBounds.X + khoangCachTrai,
                    e.CellBounds.Y
                    + e.CellBounds.Height / 2
                    - chieuCaoThanh / 2,
                    chieuRongThanh,
                    chieuCaoThanh);

            using (SolidBrush brushNen =
                   new SolidBrush(
                       Color.FromArgb(235, 239, 247)))
            {
                e.Graphics.FillRectangle(
                    brushNen,
                    thanhNen);
            }

            int chieuRongGiaTri =
                (int)Math.Round(
                    thanhNen.Width
                    * tyLe
                    / 100.0);

            Rectangle thanhGiaTri =
                new Rectangle(
                    thanhNen.X,
                    thanhNen.Y,
                    chieuRongGiaTri,
                    thanhNen.Height);

            Color mau =
                _mauBieuDo[
                    e.RowIndex % _mauBieuDo.Length];

            using (SolidBrush brushGiaTri =
                   new SolidBrush(mau))
            {
                e.Graphics.FillRectangle(
                    brushGiaTri,
                    thanhGiaTri);
            }

            Rectangle vungChu =
                new Rectangle(
                    thanhNen.Right + 6,
                    e.CellBounds.Y,
                    Math.Max(
                        0,
                        e.CellBounds.Right
                        - thanhNen.Right
                        - 8),
                    e.CellBounds.Height);

            TextRenderer.DrawText(
                e.Graphics,
                $"{tyLe:0.00}%",
                new Font(
                    "Segoe UI",
                    8F,
                    FontStyle.Bold),
                vungChu,
                Color.FromArgb(25, 55, 125),
                TextFormatFlags.Left |
                TextFormatFlags.VerticalCenter |
                TextFormatFlags.EndEllipsis);

            e.Handled = true;
        }

        #endregion

        #region Biểu đồ

        private void CauHinhBieuDo()
        {
            chartTopDanhMuc.Series.Clear();
            chartTopDanhMuc.ChartAreas.Clear();
            chartTopDanhMuc.Legends.Clear();
            chartTopDanhMuc.Titles.Clear();

            chartTopDanhMuc.BackColor = Color.White;

            ChartArea chartArea =
                new ChartArea("MainArea")
                {
                    BackColor = Color.White
                };

            chartArea.AxisX.MajorGrid.Enabled = false;
            chartArea.AxisX.Interval = 1;

            chartArea.AxisX.LineColor =
                Color.FromArgb(220, 225, 235);

            chartArea.AxisX.LabelStyle.ForeColor =
                Color.FromArgb(25, 45, 85);

            chartArea.AxisX.LabelStyle.Font =
                new Font(
                    "Segoe UI",
                    8F,
                    FontStyle.Regular);

            chartArea.AxisX.LabelStyle.Angle = 0;

            chartArea.AxisX.Title =
                "Danh mục sách";

            chartArea.AxisX.TitleFont =
                new Font(
                    "Segoe UI",
                    8F,
                    FontStyle.Bold);

            chartArea.AxisX.TitleForeColor =
                Color.FromArgb(25, 45, 85);

            chartArea.AxisY.Minimum = 0;

            chartArea.AxisY.LabelStyle.Format =
                "N0";

            chartArea.AxisY.LabelStyle.ForeColor =
                Color.FromArgb(25, 45, 85);

            chartArea.AxisY.LabelStyle.Font =
                new Font(
                    "Segoe UI",
                    8F,
                    FontStyle.Regular);

            chartArea.AxisY.LineColor =
                Color.Transparent;

            chartArea.AxisY.MajorTickMark.Enabled =
                false;

            chartArea.AxisY.MajorGrid.LineColor =
                Color.FromArgb(230, 234, 242);

            chartArea.AxisY.MajorGrid.LineDashStyle =
                ChartDashStyle.Solid;

            // leave extra top space for labels above bars
            chartArea.Position =
                new ElementPosition(
                    5,
                    4,
                    93,
                    88);

            // InnerPlotPosition controls the plotting rectangle inside chart area
            // Reduce height slightly to make room above columns for their labels
            chartArea.InnerPlotPosition =
                new ElementPosition(
                    10,
                    7,
                    87,
                    74);

            chartTopDanhMuc.ChartAreas.Add(
                chartArea);
        }

        private void VeBieuDo()
        {
            CauHinhBieuDo();

            if (_duLieu.Count == 0)
            {
                Title titleKhongDuLieu =
                    new Title
                    {
                        Text =
                            "Không có dữ liệu trong tháng đã chọn",

                        Font =
                            new Font(
                                "Segoe UI",
                                10F,
                                FontStyle.Regular),

                        ForeColor =
                            Color.FromArgb(110, 120, 145),

                        Docking =
                            Docking.Top,

                        Alignment =
                            ContentAlignment.MiddleCenter
                    };

                chartTopDanhMuc.Titles.Add(
                    titleKhongDuLieu);

                return;
            }

            Series series =
                new Series("TopDanhMuc")
                {
                    ChartType =
                        SeriesChartType.Column,

                    ChartArea =
                        "MainArea",

                    IsXValueIndexed =
                        true,

                    IsValueShownAsLabel =
                        true,

                    Font =
                        new Font(
                            "Segoe UI",
                            8F,
                            FontStyle.Bold),

                    LabelForeColor =
                        Color.FromArgb(
                            20,
                            55,
                            160),

                    BorderWidth =
                        0
                };

            series["PointWidth"] = "0.60";
            series["DrawingStyle"] = "Default";
            // ensure labels are shown with numeric format
            series.IsValueShownAsLabel = true;
            series.LabelFormat = "N0";
            series.Font = new Font("Segoe UI", 9F, FontStyle.Bold);

            List<TopDanhMucThongKeModel> topDanhMuc =
                _duLieu
                    .OrderByDescending(
                        LayGiaTriTheoKieuThongKe)
                    .ThenBy(item => item.TenDanhMuc)
                    .Take(8)
                    .ToList();

            // adjust Y axis maximum to a nice rounded value so top labels have space
            int maxVal = topDanhMuc.Count == 0 ? 0 : topDanhMuc.Max(x => LayGiaTriTheoKieuThongKe(x));
            var area = chartTopDanhMuc.ChartAreas["MainArea"];
            int maxY = RoundUpNice(maxVal);
            double axisMaximum = Math.Max(maxY, 1);
            if (axisMaximum <= maxVal)
            {
                axisMaximum = RoundUpNice(maxVal + Math.Max(1, (int)Math.Ceiling(maxVal * 0.15)));
            }
            area.AxisY.Maximum = axisMaximum;
            area.AxisY.Interval = Math.Max(1, Math.Ceiling(axisMaximum / 5));

            for (int i = 0;
                 i < topDanhMuc.Count;
                 i++)
            {
                TopDanhMucThongKeModel item =
                    topDanhMuc[i];

                int giaTri =
                    LayGiaTriTheoKieuThongKe(item);

                DataPoint point =
                    new DataPoint();

                point.SetValueXY(
                    XuLyTenDanhMucChoBieuDo(
                        item.TenDanhMuc),
                    giaTri);

                point.Color =
                    _mauBieuDo[
                        i % _mauBieuDo.Length];

                point.BackSecondaryColor = point.Color;
                point.BackGradientStyle = GradientStyle.None;

                // label displayed above each column
                point.Label = giaTri.ToString("N0");
                // color the label the same as the column for emphasis
                point.LabelForeColor = point.Color;
                // keep transparent background for label
                point.LabelBackColor = Color.Transparent;

                point.ToolTip =
                    $"{item.TenDanhMuc}"
                    + Environment.NewLine
                    + $"Tổng sách: {item.TongSach:N0}"
                    + Environment.NewLine
                    + $"Có sẵn: {item.CoSan:N0}"
                    + Environment.NewLine
                    + $"Đang mượn: {item.DangMuon:N0}"
                    + Environment.NewLine
                    + $"Quá hạn: {item.QuaHan:N0}"
                    + Environment.NewLine
                    + $"Tỷ lệ: {item.TyLe:0.00}%";

                series.Points.Add(point);
            }

            chartTopDanhMuc.Series.Add(series);

            CapNhatTenTrucY();
        }

        private static int RoundUpNice(int value)
        {
            if (value <= 10) return 10;
            int magnitude = (int)Math.Pow(10, (int)Math.Log10(value));
            int normalized = (int)Math.Ceiling((double)value / magnitude);
            return normalized * magnitude;
        }

        private int LayGiaTriTheoKieuThongKe(
            TopDanhMucThongKeModel item)
        {
            return cboKieuThongKe.SelectedIndex
                switch
                {
                    1 => item.CoSan,
                    2 => item.DangMuon,
                    3 => item.QuaHan,
                    _ => item.TongSach
                };
        }

        private void CapNhatTenTrucY()
        {
            lblTrucY.Text =
                cboKieuThongKe.SelectedIndex
                switch
                {
                    1 => "Sách đang có sẵn (cuốn)",
                    2 => "Sách đang mượn (cuốn)",
                    3 => "Sách quá hạn (cuốn)",
                    _ => "Số lượng sách (cuốn)"
                };
        }

        private static string XuLyTenDanhMucChoBieuDo(
            string tenDanhMuc)
        {
            if (string.IsNullOrWhiteSpace(
                    tenDanhMuc))
            {
                return string.Empty;
            }

            string ten = tenDanhMuc.Trim();

            if (ten.Length <= 12)
            {
                return ten;
            }

            string[] tu =
                ten.Split(
                    ' ',
                    StringSplitOptions
                        .RemoveEmptyEntries);

            if (tu.Length <= 1)
            {
                return ten.Length > 15
                    ? ten.Substring(0, 15) + "..."
                    : ten;
            }

            int viTriChia =
                tu.Length / 2;

            string dongMot =
                string.Join(
                    " ",
                    tu.Take(viTriChia));

            string dongHai =
                string.Join(
                    " ",
                    tu.Skip(viTriChia));

            return dongMot
                   + Environment.NewLine
                   + dongHai;
        }

        private static Color TaoMauNhat(
            Color mauGoc)
        {
            int red =
                (mauGoc.R + 255) / 2;

            int green =
                (mauGoc.G + 255) / 2;

            int blue =
                (mauGoc.B + 255) / 2;

            return Color.FromArgb(
                red,
                green,
                blue);
        }

        private void cboKieuThongKe_SelectedIndexChanged(
            object? sender,
            EventArgs e)
        {
            if (_dangKhoiTaoComboBox)
            {
                return;
            }

            VeBieuDo();
        }

        #endregion

        #region Xem chi tiết

        private TopDanhMucGridRow? LayDongDangChon()
        {
            if (dgvTopDanhMuc.CurrentRow == null)
            {
                return null;
            }

            return dgvTopDanhMuc.CurrentRow.DataBoundItem
                as TopDanhMucGridRow;
        }


        private void dgvTopDanhMuc_CellDoubleClick(
            object? sender,
            DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            HienThiChiTietDanhMuc();
        }

        private void HienThiChiTietDanhMuc()
        {
            TopDanhMucGridRow? row =
                LayDongDangChon();

            if (row == null)
            {
                MessageBox.Show(
                    "Vui lòng chọn một danh mục trong bảng.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            if (row.LaDongTongCong)
            {
                MessageBox.Show(
                    "Dòng tổng cộng không phải là một danh mục.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            string noiDung =
                $"Danh mục: {row.TenDanhMuc}"
                + Environment.NewLine
                + Environment.NewLine
                + $"Tổng số sách: {row.TongSach:N0} cuốn"
                + Environment.NewLine
                + $"Đang có sẵn: {row.CoSan:N0} cuốn"
                + Environment.NewLine
                + $"Đang mượn: {row.DangMuon:N0} cuốn"
                + Environment.NewLine
                + $"Quá hạn: {row.QuaHan:N0} cuốn"
                + Environment.NewLine
                + $"Tỷ lệ: {row.TyLe:0.00}%";

            MessageBox.Show(
                noiDung,
                "Chi tiết danh mục sách",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        #endregion

        #region Đóng và phím tắt

        private void btnDong_Click(
            object? sender,
            EventArgs e)
        {
            Close();
        }


        private void FrmTopDanhMucSach_KeyDown(
            object? sender,
            KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Close();
                e.Handled = true;
                return;
            }

            if (e.KeyCode == Keys.F5)
            {
                TaiDuLieu();
                e.Handled = true;
            }
        }

        #endregion
    }

    public sealed class ThangThongKeItem
    {
        public int Thang { get; set; }

        public int Nam { get; set; }

        public string TenHienThi { get; set; } =
            string.Empty;

        public string GiaTri =>
            $"{Nam:D4}-{Thang:D2}";

        public override string ToString()
        {
            return TenHienThi;
        }
    }

    public sealed class TopDanhMucGridRow
    {
        public string STT { get; set; } =
            string.Empty;

        public int MaDanhMuc { get; set; }

        public string TenDanhMuc { get; set; } =
            string.Empty;

        public int TongSach { get; set; }

        public int CoSan { get; set; }

        public int DangMuon { get; set; }

        public int QuaHan { get; set; }

        public double TyLe { get; set; }

        public string TyLeText { get; set; } =
            string.Empty;

        public bool LaDongTongCong { get; set; }
    }
}

