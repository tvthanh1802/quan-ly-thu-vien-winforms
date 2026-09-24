using FontAwesome.Sharp;

namespace Presentation.Helpers;

public static class ThongBaoUiHelper
{
    public static IconChar ParseIcon(string? iconName)
    {
        if (string.IsNullOrWhiteSpace(iconName)) return IconChar.Bell;
        if (Enum.TryParse(iconName, true, out IconChar icon)) return icon;

        string normalized = iconName.Replace("-", "").Replace("_", "").Replace(" ", "");
        foreach (IconChar value in Enum.GetValues(typeof(IconChar)))
        {
            if (value.ToString().Replace("_", "").Equals(normalized, StringComparison.OrdinalIgnoreCase))
                return value;
        }
        return IconChar.Bell;
    }

    public static Color ParseColor(string? colorValue, Color? fallback = null)
    {
        Color defaultColor = fallback ?? Color.FromArgb(100, 50, 235);
        if (string.IsNullOrWhiteSpace(colorValue)) return defaultColor;
        try
        {
            if (colorValue.StartsWith("#")) return ColorTranslator.FromHtml(colorValue);
            string[] rgb = colorValue.Split(',');
            if (rgb.Length == 3 && int.TryParse(rgb[0], out int red)
                && int.TryParse(rgb[1], out int green) && int.TryParse(rgb[2], out int blue))
                return Color.FromArgb(Math.Clamp(red, 0, 255), Math.Clamp(green, 0, 255), Math.Clamp(blue, 0, 255));
            Color named = Color.FromName(colorValue);
            if (named.IsKnownColor || named.IsNamedColor) return named;
        }
        catch (ArgumentException)
        {
            // Giá trị màu từ dữ liệu không hợp lệ; dùng màu mặc định có chủ đích.
        }
        return defaultColor;
    }

    public static Color CreateLightColor(Color source, double whiteRatio = 0.86)
    {
        whiteRatio = Math.Clamp(whiteRatio, 0, 1);
        return Color.FromArgb(
            (int)(source.R * (1 - whiteRatio) + 255 * whiteRatio),
            (int)(source.G * (1 - whiteRatio) + 255 * whiteRatio),
            (int)(source.B * (1 - whiteRatio) + 255 * whiteRatio));
    }
}
