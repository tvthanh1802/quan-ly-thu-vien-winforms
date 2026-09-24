using System.Drawing;
using System.Windows.Forms;

namespace Presentation.Helpers;

/// <summary>
/// Chuẩn màu thống nhất cho nhãn biến động trên các thẻ KPI.
/// Hướng tăng/giảm quyết định màu, không đánh giá tốt hoặc xấu.
/// </summary>
public static class MetricTrendHelper
{
    public static readonly Color IncreaseColor = Color.FromArgb(20, 155, 80);
    public static readonly Color DecreaseColor = Color.FromArgb(235, 65, 70);
    public static readonly Color UnchangedColor = Color.FromArgb(100, 104, 130);

    public static void ApplyColor(Label label, decimal difference)
    {
        label.ForeColor = difference > 0
            ? IncreaseColor
            : difference < 0
                ? DecreaseColor
                : UnchangedColor;
    }

    public static string Arrow(decimal difference) => difference > 0 ? "↑" : difference < 0 ? "↓" : "—";
}
