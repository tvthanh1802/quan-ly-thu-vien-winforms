using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace Presentation.Helpers
{
    internal static class BookCoverImageHelper
    {
        private static readonly string[] SupportedExtensions = { ".jpg", ".jpeg", ".png", ".webp", ".bmp" };

        public static Image LoadForGrid(string? databaseValue, int maSach, int width = 44, int height = 52)
        {
            using Image source = LoadOriginalOrDefault(databaseValue, maSach, Math.Max(width, 120), Math.Max(height, 150));
            return ResizeContain(source, width, height, Color.White);
        }

        public static Image LoadForDetail(string? databaseValue, int maSach, int width = 420, int height = 620)
        {
            using Image source = LoadOriginalOrDefault(databaseValue, maSach, width, height);
            return ResizeContain(source, width, height, Color.FromArgb(235, 238, 244));
        }

        public static Image LoadOriginalOrDefault(string? databaseValue, int maSach, int defaultWidth = 420, int defaultHeight = 620)
        {
            string? path = ResolvePath(databaseValue, maSach);
            if (!string.IsNullOrWhiteSpace(path))
            {
                try
                {
                    using FileStream stream = new(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete);
                    using Image image = Image.FromStream(stream, useEmbeddedColorManagement: true, validateImageData: true);
                    return new Bitmap(image);
                }
                catch
                {
                    // File exists but is invalid/corrupt. Fall through to a safe default image.
                }
            }

            return CreateDefaultCover(maSach, defaultWidth, defaultHeight);
        }

        public static string? ResolvePath(string? databaseValue, int maSach)
        {
            string value = (databaseValue ?? string.Empty).Trim().Trim('"');
            value = value.Replace('/', Path.DirectorySeparatorChar)
                         .Replace('\\', Path.DirectorySeparatorChar);

            var fileNames = new List<string>();
            if (!string.IsNullOrWhiteSpace(value))
            {
                string name = Path.GetFileName(value);
                if (!string.IsNullOrWhiteSpace(name))
                {
                    fileNames.Add(name);
                    if (string.IsNullOrWhiteSpace(Path.GetExtension(name)))
                        fileNames.AddRange(SupportedExtensions.Select(ext => name + ext));
                }
            }

            // The supplied dataset stores covers as book_0001.jpg ... book_0520.jpg.
            fileNames.AddRange(SupportedExtensions.Select(ext => $"book_{maSach:D4}{ext}"));
            fileNames.AddRange(SupportedExtensions.Select(ext => $"book_{maSach:D3}{ext}"));
            fileNames.AddRange(SupportedExtensions.Select(ext => $"book_{maSach}{ext}"));
            fileNames.AddRange(SupportedExtensions.Select(ext => $"S{maSach:D5}{ext}"));
            fileNames.AddRange(SupportedExtensions.Select(ext => $"{maSach}{ext}"));

            var candidates = new List<string>();
            if (Path.IsPathRooted(value))
                candidates.Add(value);

            foreach (string root in GetSearchRoots())
            {
                if (!string.IsNullOrWhiteSpace(value))
                    candidates.Add(Path.Combine(root, value.TrimStart(Path.DirectorySeparatorChar)));

                foreach (string fileName in fileNames.Distinct(StringComparer.OrdinalIgnoreCase))
                {
                    candidates.Add(Path.Combine(root, "Images", "BookCovers", fileName));
                    candidates.Add(Path.Combine(root, "BookCovers", fileName));
                    candidates.Add(Path.Combine(root, fileName));
                }
            }

            return candidates
                .Select(SafeFullPath)
                .Where(p => !string.IsNullOrWhiteSpace(p))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .FirstOrDefault(File.Exists);
        }

        private static IEnumerable<string> GetSearchRoots()
        {
            var roots = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                Application.StartupPath,
                AppContext.BaseDirectory,
                Environment.CurrentDirectory
            };

            AddParents(roots, Application.StartupPath, 7);
            AddParents(roots, AppContext.BaseDirectory, 7);
            AddParents(roots, Environment.CurrentDirectory, 7);

            // Add both common source-layout roots.
            foreach (string root in roots.ToArray())
            {
                roots.Add(Path.Combine(root, "Presentation"));
                roots.Add(Path.Combine(root, "..", "Presentation"));
            }

            return roots.Select(SafeFullPath).OfType<string>().Where(x => !string.IsNullOrWhiteSpace(x));
        }

        private static void AddParents(HashSet<string> roots, string start, int maxLevels)
        {
            DirectoryInfo? dir = new DirectoryInfo(start);
            for (int i = 0; i < maxLevels && dir != null; i++, dir = dir.Parent)
                roots.Add(dir.FullName);
        }

        private static string? SafeFullPath(string path)
        {
            try { return Path.GetFullPath(path); }
            catch { return null; }
        }

        private static Bitmap ResizeContain(Image source, int width, int height, Color background)
        {
            var result = new Bitmap(width, height);
            using Graphics graphics = Graphics.FromImage(result);
            graphics.Clear(background);
            graphics.CompositingQuality = CompositingQuality.HighQuality;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            float ratio = Math.Min(width / (float)source.Width, height / (float)source.Height);
            int targetWidth = Math.Max(1, (int)Math.Round(source.Width * ratio));
            int targetHeight = Math.Max(1, (int)Math.Round(source.Height * ratio));
            int x = (width - targetWidth) / 2;
            int y = (height - targetHeight) / 2;
            graphics.DrawImage(source, new Rectangle(x, y, targetWidth, targetHeight));
            return result;
        }

        private static Bitmap CreateDefaultCover(int maSach, int width, int height)
        {
            width = Math.Max(width, 80);
            height = Math.Max(height, 110);
            var bitmap = new Bitmap(width, height);
            using Graphics g = Graphics.FromImage(bitmap);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            using var background = new LinearGradientBrush(
                new Rectangle(0, 0, width, height),
                Color.FromArgb(70, 75, 220),
                Color.FromArgb(36, 163, 223),
                45f);
            g.FillRectangle(background, 0, 0, width, height);

            int margin = Math.Max(6, width / 10);
            var bookRect = new Rectangle(margin, height / 5, width - margin * 2, height / 3);
            using var whitePen = new Pen(Color.FromArgb(235, 255, 255, 255), Math.Max(2, width / 35f));
            g.DrawRectangle(whitePen, bookRect);
            g.DrawLine(whitePen, bookRect.Left + bookRect.Width / 2, bookRect.Top, bookRect.Left + bookRect.Width / 2, bookRect.Bottom);

            using var titleFont = new Font("Segoe UI", Math.Max(8f, width / 9f), FontStyle.Bold, GraphicsUnit.Pixel);
            using var codeFont = new Font("Segoe UI", Math.Max(7f, width / 11f), FontStyle.Regular, GraphicsUnit.Pixel);
            using var whiteBrush = new SolidBrush(Color.White);
            using var lightBrush = new SolidBrush(Color.FromArgb(220, 255, 255, 255));

            string title = "EPU LIBRARY";
            SizeF titleSize = g.MeasureString(title, titleFont);
            g.DrawString(title, titleFont, whiteBrush, (width - titleSize.Width) / 2f, height * 0.63f);

            string code = maSach > 0 ? $"S{maSach:D5}" : "NO COVER";
            SizeF codeSize = g.MeasureString(code, codeFont);
            g.DrawString(code, codeFont, lightBrush, (width - codeSize.Width) / 2f, height * 0.78f);
            return bitmap;
        }
    }
}
