using DataLayer.Models;
using Presentation.Helpers;
using System.Drawing.Drawing2D;

namespace Presentation.Controls;

internal sealed partial class UcSachMuonItem : UserControl
{
    private Image? _coverImage;
    private Color _accentColor = Color.FromArgb(22, 119, 255);
    private Color _statusBackColor = Color.FromArgb(232, 245, 255);

    public UcSachMuonItem()
    {
        InitializeComponent();

        // Custom paint — phải đặt sau InitializeComponent() để không bị Designer ghi đè
        DoubleBuffered = true;
        SetStyle(
            ControlStyles.AllPaintingInWmPaint |
            ControlStyles.OptimizedDoubleBuffer |
            ControlStyles.ResizeRedraw |
            ControlStyles.UserPaint,
            true);

        Resize += (_, _) => CanChinhTheoChieuRong();
    }

    public void SetData(SachMuonItemModel model)
    {
        _lblTenSach.Text = model.TenSach;
        _lblMaSach.Text = $"Mã sách: {model.MaSachText}  •  Cuốn: {model.MaCuonText}";
        _lblHanTra.Text = $"Hạn trả: {model.HanTra:dd/MM/yyyy}";
        _lblTrangThai.Text = model.TrangThai;
        _accentColor = LayMauTrangThai(model.TrangThai);
        _statusBackColor = LayNenTrangThai(model.TrangThai);
        _lblTrangThai.ForeColor = _accentColor;

        _picAnhBia.Image = null;
        _coverImage?.Dispose();
        _coverImage = BookCoverImageHelper.LoadForGrid(model.AnhBia, model.MaSach, 58, 68);
        _picAnhBia.Image = _coverImage;
        Invalidate();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        Rectangle cardRect = new(1, 1, Width - 3, Height - 3);
        using GraphicsPath cardPath = RoundedRect(cardRect, 14);
        using SolidBrush cardBrush = new(Color.White);
        using Pen borderPen = new(Color.FromArgb(226, 232, 244));
        e.Graphics.FillPath(cardBrush, cardPath);
        e.Graphics.DrawPath(borderPen, cardPath);

        using SolidBrush accentBrush = new(Color.FromArgb(28, _accentColor));
        e.Graphics.FillEllipse(accentBrush, new Rectangle(10, 10, 66, 74));

        Rectangle badgeRect = _lblTrangThai.Bounds;
        badgeRect.Inflate(2, 0);
        using GraphicsPath badgePath = RoundedRect(badgeRect, 10);
        using SolidBrush badgeBrush = new(_statusBackColor);
        e.Graphics.FillPath(badgeBrush, badgePath);

        base.OnPaint(e);
    }

    private void CanChinhTheoChieuRong()
    {
        int right = Math.Max(190, Width - 18);
        _lblTrangThai.Left = Math.Max(260, right - 92);
        _lblTrangThai.Width = 86;
        _lblTenSach.Width = Math.Max(120, _lblTrangThai.Left - _lblTenSach.Left - 10);
        _lblMaSach.Width = Math.Max(120, right - _lblMaSach.Left);
        _lblHanTra.Width = Math.Max(120, right - _lblHanTra.Left);
    }

    private static GraphicsPath RoundedRect(Rectangle bounds, int radius)
    {
        int diameter = radius * 2;
        GraphicsPath path = new();
        path.AddArc(bounds.X, bounds.Y, diameter, diameter, 180, 90);
        path.AddArc(bounds.Right - diameter, bounds.Y, diameter, diameter, 270, 90);
        path.AddArc(bounds.Right - diameter, bounds.Bottom - diameter, diameter, diameter, 0, 90);
        path.AddArc(bounds.X, bounds.Bottom - diameter, diameter, diameter, 90, 90);
        path.CloseFigure();
        return path;
    }

    private static Color LayMauTrangThai(string? value)
    {
        return value switch
        {
            "Quá hạn" => Color.FromArgb(234, 88, 12),
            "Đã trả" => Color.FromArgb(37, 99, 235),
            "Mất" or "Mất sách" => Color.FromArgb(225, 29, 72),
            "Hỏng" or "Hư hỏng" => Color.FromArgb(124, 58, 237),
            _ => Color.FromArgb(22, 163, 74)
        };
    }

    private static Color LayNenTrangThai(string? value)
    {
        return value switch
        {
            "Quá hạn" => Color.FromArgb(255, 237, 213),
            "Đã trả" => Color.FromArgb(219, 234, 254),
            "Mất" or "Mất sách" => Color.FromArgb(255, 228, 230),
            "Hỏng" or "Hư hỏng" => Color.FromArgb(243, 232, 255),
            _ => Color.FromArgb(220, 252, 231)
        };
    }
}
