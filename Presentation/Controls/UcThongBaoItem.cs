using FontAwesome.Sharp;
using Presentation.Helpers;

namespace Presentation.Controls
{
    public partial class UcThongBaoItem : UserControl
    {
        private int _maThongBao;
        private readonly ToolTip _toolTip = new();

        public event EventHandler<int>? NotificationClicked;

        public UcThongBaoItem()
        {
            InitializeComponent();

            pnlItem.Click += Item_Click;
            pnlIcon.Click += Item_Click;
            icoThongBao.Click += Item_Click;
            lblTieuDe.Click += Item_Click;
            lblNoiDung.Click += Item_Click;
            lblThoiGian.Click += Item_Click;
        }

        public void SetData(
            int maThongBao,
            string tieuDe,
            string noiDung,
            string thoiGian,
            string iconName,
            string colorValue,
            bool daDoc)
        {
            _maThongBao = maThongBao;

            lblTieuDe.Text = tieuDe;
            lblNoiDung.Text = noiDung;
            lblThoiGian.Text = thoiGian;

            IconChar icon = ThongBaoUiHelper.ParseIcon(iconName);
            Color iconColor = ThongBaoUiHelper.ParseColor(colorValue);

            icoThongBao.BackgroundImage = null;
            icoThongBao.IconChar = icon;
            icoThongBao.IconColor = iconColor;
            icoThongBao.BackColor = Color.Transparent;

            pnlIcon.FillColor = ThongBaoUiHelper.CreateLightColor(iconColor);

            lblThoiGian.ForeColor = iconColor;

            // Thông báo chưa đọc dùng chữ đậm
            lblTieuDe.Font = new Font(
                "Segoe UI",
                8f,
                daDoc ? FontStyle.Regular : FontStyle.Bold);

            pnlItem.FillColor = daDoc
                ? Color.White
                : Color.FromArgb(252, 252, 255);

            _toolTip.SetToolTip(lblTieuDe, tieuDe);
            _toolTip.SetToolTip(lblNoiDung, noiDung);
        }


        private void Item_Click(object? sender, EventArgs e)
        {
            NotificationClicked?.Invoke(
                this,
                _maThongBao);
        }

    }
}
