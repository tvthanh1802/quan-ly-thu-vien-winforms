using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace Presentation.Helpers;

internal static class DatabaseImageHelper
{
    private static readonly string[] Extensions = [".png", ".jpg", ".jpeg", ".webp", ".bmp"];

    public static Image LoadReaderAvatar(string? databaseValue, string displayName, int width, int height) =>
        LoadAvatar(databaseValue, displayName, width, height, Color.FromArgb(79, 70, 229), Color.FromArgb(238, 242, 255));

    public static Image LoadStaffAvatar(string? databaseValue, string displayName, int width, int height) =>
        LoadAvatar(databaseValue, displayName, width, height, Color.FromArgb(37, 99, 235), Color.FromArgb(239, 246, 255));

    public static string? ResolvePath(string? databaseValue)
    {
        string value = (databaseValue ?? string.Empty).Trim().Trim('"');
        if (value.Length == 0) return null;
        value = value.Replace('/', Path.DirectorySeparatorChar).Replace('\\', Path.DirectorySeparatorChar);
        var candidates = new List<string>();
        if (Path.IsPathRooted(value)) candidates.Add(value);
        string fileName = Path.GetFileName(value);
        foreach (string root in SearchRoots())
        {
            candidates.Add(Path.Combine(root, value.TrimStart(Path.DirectorySeparatorChar)));
            candidates.Add(Path.Combine(root, "Presentation", value.StartsWith("Presentation" + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase)
                ? value[("Presentation".Length + 1)..] : value));
            if (!string.IsNullOrWhiteSpace(fileName))
            {
                candidates.Add(Path.Combine(root, "Images", "Avatars", "Readers", fileName));
                candidates.Add(Path.Combine(root, "Images", "Avatars", "Staff", fileName));
                candidates.Add(Path.Combine(root, "Images", "BookCovers", fileName));
            }
        }
        return candidates.Select(SafeFullPath).Where(x => x != null).Distinct(StringComparer.OrdinalIgnoreCase).FirstOrDefault(File.Exists);
    }

    public static Image LoadImageOrDefault(string? databaseValue, Func<Image> fallback)
    {
        string? path = ResolvePath(databaseValue);
        if (path != null)
        {
            try
            {
                using var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete);
                using Image source = Image.FromStream(stream, true, true);
                return new Bitmap(source);
            }
            catch { }
        }
        return fallback();
    }

    public static string CopyToProjectImages(string sourcePath, string category, string targetFileName)
    {
        if (!File.Exists(sourcePath)) throw new FileNotFoundException("Không tìm thấy file ảnh đã chọn.", sourcePath);
        string extension = Path.GetExtension(sourcePath).ToLowerInvariant();
        if (!Extensions.Contains(extension, StringComparer.OrdinalIgnoreCase)) throw new InvalidOperationException("Định dạng ảnh không được hỗ trợ.");
        string relative = $"Presentation/Images/{category}/{targetFileName}{extension}";
        string? presentationRoot = FindPresentationRoot();
        string outputRoot = presentationRoot ?? AppContext.BaseDirectory;
        string target = presentationRoot != null
            ? Path.Combine(presentationRoot, "Images", category.Replace('/', Path.DirectorySeparatorChar), targetFileName + extension)
            : Path.Combine(outputRoot, "Images", category.Replace('/', Path.DirectorySeparatorChar), targetFileName + extension);
        Directory.CreateDirectory(Path.GetDirectoryName(target)!);
        if (!Path.GetFullPath(sourcePath).Equals(Path.GetFullPath(target), StringComparison.OrdinalIgnoreCase)) File.Copy(sourcePath, target, true);
        return relative;
    }

    private static Image LoadAvatar(string? path, string name, int width, int height, Color accent, Color background)
    {
        Image source = LoadImageOrDefault(path, () => CreateInitialAvatar(name, width, height, accent, background));
        if (source.Width == width && source.Height == height) return source;

        try
        {
            return ResizeContain(source, width, height, background);
        }
        finally
        {
            source.Dispose();
        }
    }

    private static Bitmap ResizeContain(Image source, int width, int height, Color background)
    {
        width = Math.Max(24, width);
        height = Math.Max(24, height);
        var result = new Bitmap(width, height);
        using Graphics graphics = Graphics.FromImage(result);
        graphics.Clear(background);
        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
        graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
        graphics.CompositingQuality = CompositingQuality.HighQuality;

        float scale = Math.Min((float)width / source.Width, (float)height / source.Height);
        int drawWidth = Math.Max(1, (int)Math.Round(source.Width * scale));
        int drawHeight = Math.Max(1, (int)Math.Round(source.Height * scale));
        int x = (width - drawWidth) / 2;
        int y = (height - drawHeight) / 2;
        graphics.DrawImage(source, new Rectangle(x, y, drawWidth, drawHeight));
        return result;
    }

    private static Bitmap CreateInitialAvatar(string name, int width, int height, Color accent, Color background)
    {
        width = Math.Max(24, width); height = Math.Max(24, height); var bitmap = new Bitmap(width, height);
        using Graphics g = Graphics.FromImage(bitmap); g.SmoothingMode = SmoothingMode.AntiAlias; g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit; g.Clear(background);
        string initial = string.Join("", (name ?? "?").Split(' ', StringSplitOptions.RemoveEmptyEntries).TakeLast(2).Select(x => char.ToUpperInvariant(x[0])));
        if (initial.Length == 0) initial = "?";
        using var brush = new SolidBrush(accent); using var font = new Font("Segoe UI", Math.Max(10, Math.Min(width, height) * .29f), FontStyle.Bold, GraphicsUnit.Pixel);
        SizeF size = g.MeasureString(initial, font); g.DrawString(initial, font, brush, (width - size.Width) / 2, (height - size.Height) / 2); return bitmap;
    }

    private static IEnumerable<string> SearchRoots()
    {
        var roots = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { Application.StartupPath, AppContext.BaseDirectory, Environment.CurrentDirectory };
        foreach (string start in roots.ToArray()) { DirectoryInfo? dir = new(start); for (int i = 0; i < 8 && dir != null; i++, dir = dir.Parent) roots.Add(dir.FullName); }
        foreach (string root in roots.ToArray()) roots.Add(Path.Combine(root, "Presentation"));
        return roots.Select(SafeFullPath).OfType<string>();
    }

    private static string? FindPresentationRoot() => SearchRoots().FirstOrDefault(x => File.Exists(Path.Combine(x, "Presentation.csproj")));
    private static string? SafeFullPath(string path) { try { return Path.GetFullPath(path); } catch { return null; } }
}
