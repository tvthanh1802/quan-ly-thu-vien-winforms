using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Xml;

namespace Presentation.Helpers
{
    internal static class ExcelHelper
    {
        private const int MaximumExcelColumns = 16_384;
        private const int MaximumExcelRows = 1_048_576;
        private const int HeaderRow = 5;
        private const int FirstDataRow = HeaderRow + 1;

        public static void ExportToXlsx(string filePath, string sheetName, string[] headers, List<string[]> rows)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("Đường dẫn lưu file không hợp lệ.", nameof(filePath));
            if (!string.Equals(Path.GetExtension(filePath), ".xlsx", StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException("File xuất phải có phần mở rộng .xlsx.", nameof(filePath));
            if (headers == null || headers.Length == 0)
                throw new ArgumentException("Danh sách cột không được để trống.", nameof(headers));
            if (headers.Length > MaximumExcelColumns)
                throw new ArgumentException($"Excel chỉ hỗ trợ tối đa {MaximumExcelColumns:N0} cột.", nameof(headers));

            rows ??= new List<string[]>();
            if (rows.Count > MaximumExcelRows - FirstDataRow + 1)
                throw new ArgumentException("Số dòng dữ liệu vượt quá giới hạn của Excel.", nameof(rows));

            string fullPath = Path.GetFullPath(filePath);
            string? directory = Path.GetDirectoryName(fullPath);
            if (string.IsNullOrWhiteSpace(directory) || !Directory.Exists(directory))
                throw new DirectoryNotFoundException("Thư mục lưu file không tồn tại.");

            string temporaryPath = Path.Combine(
                directory,
                $".{Path.GetFileNameWithoutExtension(fullPath)}.{Guid.NewGuid():N}.tmp.xlsx");

            try
            {
                using (var workbook = new ClosedXML.Excel.XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add(NormalizeSheetName(sheetName));
                    string reportTitle = "BÁO CÁO " + worksheet.Name.ToUpperInvariant();

                    worksheet.Cell(1, 1).Value = reportTitle;
                    worksheet.Cell(2, 1).Value =
                        $"HỆ THỐNG QUẢN LÝ THƯ VIỆN • Xuất lúc {DateTime.Now:HH:mm:ss dd/MM/yyyy}";
                    worksheet.Cell(3, 1).Value = $"Tổng số bản ghi: {rows.Count:N0}";

                    for (int column = 0; column < headers.Length; column++)
                        worksheet.Cell(HeaderRow, column + 1).Value = CleanXmlText(headers[column]);

                    for (int rowIndex = 0; rowIndex < rows.Count; rowIndex++)
                    {
                        string[] row = rows[rowIndex] ?? Array.Empty<string>();
                        for (int column = 0; column < headers.Length; column++)
                        {
                            string value = column < row.Length ? row[column] ?? string.Empty : string.Empty;
                            worksheet.Cell(rowIndex + FirstDataRow, column + 1).Value = CleanXmlText(value);
                        }
                    }

                    var titleRange = worksheet.Range(1, 1, 1, headers.Length).Merge();
                    var subtitleRange = worksheet.Range(2, 1, 2, headers.Length).Merge();
                    var countRange = worksheet.Range(3, 1, 3, headers.Length).Merge();
                    titleRange.Style.Font.Bold = true;
                    titleRange.Style.Font.FontSize = 16;
                    titleRange.Style.Font.FontColor = ClosedXML.Excel.XLColor.White;
                    titleRange.Style.Fill.BackgroundColor = ClosedXML.Excel.XLColor.FromHtml("1E3A8A");
                    subtitleRange.Style.Font.FontColor = ClosedXML.Excel.XLColor.FromHtml("475467");
                    countRange.Style.Font.Bold = true;

                    var headerRange = worksheet.Range(HeaderRow, 1, HeaderRow, headers.Length);
                    headerRange.Style.Font.Bold = true;
                    headerRange.Style.Font.FontColor = ClosedXML.Excel.XLColor.White;
                    headerRange.Style.Fill.BackgroundColor = ClosedXML.Excel.XLColor.FromHtml("1E3A8A");
                    headerRange.Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Center;
                    headerRange.Style.Alignment.Vertical = ClosedXML.Excel.XLAlignmentVerticalValues.Center;

                    int lastRow = Math.Max(HeaderRow, rows.Count + HeaderRow);
                    var dataRange = worksheet.Range(HeaderRow, 1, lastRow, headers.Length);
                    dataRange.Style.Border.OutsideBorder = ClosedXML.Excel.XLBorderStyleValues.Thin;
                    dataRange.Style.Border.InsideBorder = ClosedXML.Excel.XLBorderStyleValues.Thin;
                    dataRange.Style.Border.OutsideBorderColor = ClosedXML.Excel.XLColor.FromHtml("E2E8F0");
                    dataRange.Style.Border.InsideBorderColor = ClosedXML.Excel.XLColor.FromHtml("E2E8F0");
                    worksheet.SheetView.FreezeRows(HeaderRow);
                    worksheet.Range(HeaderRow, 1, lastRow, headers.Length).SetAutoFilter();
                    worksheet.Columns(1, headers.Length).AdjustToContents();
                    foreach (var column in worksheet.Columns(1, headers.Length))
                        column.Width = Math.Min(45, Math.Max(12, column.Width));

                    workbook.SaveAs(temporaryPath);
                }

                File.Move(temporaryPath, fullPath, true);
            }
            catch
            {
                if (File.Exists(temporaryPath))
                    File.Delete(temporaryPath);
                throw;
            }
        }

        public static (string[] headers, List<string[]> rows) ImportFromXlsx(string filePath)
        {
            using var archive = ZipFile.OpenRead(filePath);

            var sharedStrings = new List<string>();
            var entry = archive.GetEntry("xl/sharedStrings.xml");
            if (entry != null)
            {
                using var s = entry.Open();
                var doc = new XmlDocument();
                doc.Load(s);
                var nodes = doc.GetElementsByTagName("si");
                foreach (XmlNode si in nodes)
                {
                    string text = string.Concat(si.SelectNodes(".//*")!
                        .Cast<XmlNode>()
                        .Where(node => node.LocalName == "t")
                        .Select(node => node.InnerText));
                    sharedStrings.Add(text);
                }
            }

            var sheet = archive.GetEntry("xl/worksheets/sheet1.xml");
            if (sheet == null) throw new FileNotFoundException("Worksheet not found in xlsx");

            var rows = new List<string[]>();
            using (var s = sheet.Open())
            {
                var doc = new XmlDocument();
                doc.Load(s);
                var rowNodes = doc.GetElementsByTagName("row");
                foreach (XmlNode row in rowNodes)
                {
                    var valuesByColumn = new Dictionary<int, string>();
                    int maxColumn = -1;
                    foreach (XmlNode cell in row.ChildNodes)
                    {
                        if (cell.LocalName != "c") continue;
                        string reference = cell.Attributes?["r"]?.Value ?? string.Empty;
                        int columnIndex = ColumnIndex(reference);
                        if (columnIndex < 0) columnIndex = maxColumn + 1;

                        string type = cell.Attributes?["t"]?.Value ?? string.Empty;
                        string raw = cell.ChildNodes.Cast<XmlNode>()
                            .FirstOrDefault(node => node.LocalName == "v")?.InnerText ?? string.Empty;
                        string val = type == "s"
                            && int.TryParse(raw, out int sharedIndex)
                            && sharedIndex >= 0 && sharedIndex < sharedStrings.Count
                                ? sharedStrings[sharedIndex]
                                : raw;
                        if (!string.IsNullOrWhiteSpace(val))
                        {
                            valuesByColumn[columnIndex] = val;
                            maxColumn = Math.Max(maxColumn, columnIndex);
                        }
                    }

                    if (maxColumn >= 0)
                    {
                        string[] values = Enumerable.Range(0, maxColumn + 1)
                            .Select(index => valuesByColumn.GetValueOrDefault(index, string.Empty))
                            .ToArray();
                        rows.Add(values);
                    }
                }
            }

            if (rows.Count == 0) return (Array.Empty<string>(), new List<string[]>());

            // Tìm dòng header thực sự bằng cách bỏ qua các dòng tiêu đề báo cáo
            int headerRowIndex = rows.FindIndex(row =>
                row.Length >= 2 &&
                !row[0].StartsWith("BÁO CÁO", StringComparison.OrdinalIgnoreCase) &&
                !row[0].StartsWith("HỆ THỐNG", StringComparison.OrdinalIgnoreCase) &&
                !row[0].StartsWith("Tổng số", StringComparison.OrdinalIgnoreCase));

            if (headerRowIndex < 0) return (Array.Empty<string>(), new List<string[]>());
            var headers = rows[headerRowIndex];
            var data = rows.Skip(headerRowIndex + 1)
                .Where(row => row.Any(value => !string.IsNullOrWhiteSpace(value)))
                .ToList();
            return (headers, data);
        }

        private static string CleanXmlText(string? value)
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;

            var result = new StringBuilder(value.Length);
            for (int index = 0; index < value.Length; index++)
            {
                char character = value[index];
                if (character is '\t' or '\n' or '\r' || (character >= ' ' && character <= '\uD7FF') ||
                    (character >= '\uE000' && character <= '\uFFFD'))
                {
                    result.Append(character);
                    continue;
                }

                if (char.IsHighSurrogate(character) && index + 1 < value.Length && char.IsLowSurrogate(value[index + 1]))
                {
                    result.Append(character);
                    result.Append(value[++index]);
                }
            }

            return result.ToString();
        }

        private static string NormalizeSheetName(string? sheetName)
        {
            string result = string.IsNullOrWhiteSpace(sheetName) ? "Báo cáo" : sheetName.Trim();
            foreach (char invalid in new[] { '[', ']', ':', '*', '?', '/', '\\' }) result = result.Replace(invalid, '-');
            return result.Length > 31 ? result[..31] : result;
        }

        private static int ColumnIndex(string reference)
        {
            int result = 0;
            bool found = false;
            foreach (char character in reference)
            {
                if (!char.IsLetter(character)) break;
                found = true;
                result = result * 26 + char.ToUpperInvariant(character) - 'A' + 1;
            }
            return found ? result - 1 : -1;
        }
    }
}

