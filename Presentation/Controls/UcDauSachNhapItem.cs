using DataLayer.Models;
using Presentation.Helpers;

namespace Presentation.Controls
{
    public partial class UcDauSachNhapItem : UserControl
    {
        private Image? _currentImage;

        public UcDauSachNhapItem()
        {
            InitializeComponent();
            Disposed += (_, _) =>
            {
                picAnhBia.Image = null;
                _currentImage?.Dispose();
                _currentImage = null;
            };
        }

        public void SetData(DauSachNhapItemModel model)
        {
            lblTenSach.Text = model.TenSach;
            lblTacGia.Text = string.IsNullOrWhiteSpace(model.TacGia) ? "Chưa cập nhật tác giả" : model.TacGia;
            lblSoLuong.Text = $"SL: {model.SoLuong:N0}";
            lblThanhTien.Text = $"{model.ThanhTien:N0} đ";

            picAnhBia.Image = null;
            _currentImage?.Dispose();
            _currentImage = BookCoverImageHelper.LoadForGrid(model.AnhBia, model.MaSach, 45, 54);
            picAnhBia.Image = _currentImage;
        }

    }
}
