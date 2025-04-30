using System.Text;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;
using Domain.Interfaces;

namespace Infrastructure.Service
{
    public class PdfToMarkdownService : IPdfToMarkdownService
    {
        public string ConvertPdfToMarkdown(byte[] pdfFileBytes)
        {
            if (pdfFileBytes == null || pdfFileBytes.Length == 0)
                throw new ArgumentException("PDF File can't be empty", nameof(pdfFileBytes));

            var sb = new StringBuilder();
            try
            {
                using (var memoryStream = new MemoryStream(pdfFileBytes))
                using (PdfDocument document = PdfDocument.Open(memoryStream))
                {
                    var allLines = new List<TextLine>();
                    foreach (Page page in document.GetPages())
                    {
                        try
                        {
                            var lines = ExtractLinesFromPage(page);
                            allLines.AddRange(lines);
                        }
                        catch (Exception pageEx)
                        {
                            Console.Error.WriteLine($"Error processing page {page.Number}: {pageEx.Message}");
                            continue;
                        }
                    }

                    double bodyFontSize = DetermineBodyFontSize(allLines);

                    var headingSizes = allLines
                        .Where(line => line.FontSize > bodyFontSize)
                        .Select(line => line.FontSize)
                        .Distinct()
                        .OrderByDescending(x => x)
                        .ToList();

                    int currentPageIndex = 0;
                    foreach (Page page in document.GetPages())
                    {
                        List<TextLine> lines;
                        try
                        {
                            lines = allLines
                                .Skip(currentPageIndex)
                                .TakeWhile(l => l.PageNumber == page.Number)
                                .ToList();
                            currentPageIndex += lines.Count;
                        }
                        catch
                        {
                            lines = ExtractLinesFromPage(page);
                        }

                        if (lines.Count == 0) continue;

                        bool[] usedLine = new bool[lines.Count];
                        for (int i = 0; i < lines.Count; i++)
                        {
                            if (usedLine[i]) continue;
                            TextLine line = lines[i];

                            var tableLines = new List<TextLine> { line };
                            for (int j = i + 1; j < lines.Count; j++)
                            {
                                if (usedLine[j]) continue;
                                if (AreLinesAligned(line, lines[j]))
                                {
                                    tableLines.Add(lines[j]);
                                    usedLine[j] = true;
                                }
                                else
                                {
                                    break;
                                }
                            }

                            if (tableLines.Count > 1)
                            {
                                AppendTableMarkdown(sb, tableLines);
                                usedLine[i] = true;
                            }
                            else
                            {
                                AppendLineMarkdown(sb, line, bodyFontSize, headingSizes);
                                usedLine[i] = true;
                            }
                        }

                        if (page.Number < document.NumberOfPages)
                        {
                            sb.AppendLine();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to convert PDF to Markdown: {ex.Message}");
            }
            return sb.ToString();
        }
        public async Task<string> ConvertPdfToMarkdownAsync(byte[] pdfFileBytes)
        {
            return await Task.Run(() => ConvertPdfToMarkdown(pdfFileBytes));
        }
        private class TextLine
        {
            public int PageNumber { get; set; }
            public double Y { get; set; }
            public double FontSize { get; set; }
            public List<(string Text, double X)> Words { get; set; } = new List<(string, double)>();
        }
        private List<TextLine> ExtractLinesFromPage(Page page)
        {
            var lines = new List<TextLine>();

            IEnumerable<Word> pdfWords = page.GetWords();

            var sortedWords = pdfWords.OrderByDescending(w => w.BoundingBox.Bottom).ThenBy(w => w.BoundingBox.Left);


            const double yTolerance = 3.0;
            TextLine? currentLine = null;
            double currentLineY = Double.NaN;
            foreach (var word in sortedWords)
            {
                string text = word.Text;

                double wordY = word.BoundingBox.Bottom;
                double wordX = word.BoundingBox.Left;

                double wordFontSize = 0;
                if (word.Letters.Any())
                {
                    var firstLetter = word.Letters.First();
                    wordFontSize = firstLetter.PointSize > 0 ? firstLetter.PointSize : (double)firstLetter.FontSize;
                }

                if (currentLine == null)
                {
                    currentLine = new TextLine
                    {
                        PageNumber = page.Number,
                        Y = wordY,
                        FontSize = wordFontSize
                    };
                    currentLine.Words.Add((EscapeMarkdown(text), wordX));
                    currentLineY = wordY;
                }
                else if (Math.Abs(currentLineY - wordY) <= yTolerance)
                {
                    currentLine.Words.Add((EscapeMarkdown(text), wordX));

                    if (wordFontSize > currentLine.FontSize)
                        currentLine.FontSize = wordFontSize;
                }
                else
                {
                    currentLine.Words.Sort((a, b) => a.X.CompareTo(b.X));
                    lines.Add(currentLine);

                    currentLine = new TextLine
                    {
                        PageNumber = page.Number,
                        Y = wordY,
                        FontSize = wordFontSize
                    };
                    currentLine.Words.Add((EscapeMarkdown(text), wordX));
                    currentLineY = wordY;
                }
            }

            if (currentLine != null)
            {
                currentLine.Words.Sort((a, b) => a.X.CompareTo(b.X));
                lines.Add(currentLine);
            }

            return lines;
        }
        private double DetermineBodyFontSize(List<TextLine> allLines)
        {
            if (allLines == null || allLines.Count == 0) return 0;
            var freq = new Dictionary<double, int>();
            foreach (var line in allLines)
            {
                double size = Math.Round(line.FontSize, 1);
                if (size <= 0) continue;
                if (!freq.ContainsKey(size))
                    freq[size] = 0;
                freq[size]++;
            }
            if (freq.Count == 0) return 0;
            double modeSize = freq.OrderByDescending(kv => kv.Value).First().Key;
            double minSize = freq.Keys.Min();
            if (Math.Abs(modeSize - minSize) < 1e-6 && freq.Count > 1)
            {
                var candidate = freq
                    .Where(kv => kv.Key > modeSize)
                    .OrderByDescending(kv => kv.Value)
                    .FirstOrDefault();
                if (candidate.Value > 0)
                {
                    modeSize = candidate.Key;
                }
            }
            return modeSize;
        }
        private bool AreLinesAligned(TextLine line1, TextLine line2)
        {
            if (line1.Words.Count != line2.Words.Count) return false;
            int colCount = line1.Words.Count;
            for (int i = 0; i < colCount; i++)
            {
                double x1 = line1.Words[i].X;
                double x2 = line2.Words[i].X;
                if (Math.Abs(x1 - x2) > 5.0)
                {
                    return false;
                }
            }
            return true;
        }
        private void AppendLineMarkdown(StringBuilder sb, TextLine line, double bodyFontSize, List<double> headingSizes)
        {
            string lineText = string.Join(" ", line.Words.Select(w => w.Text));
            if (line.FontSize > bodyFontSize && headingSizes.Count > 0)
            {
                int level = headingSizes.IndexOf(line.FontSize) + 1;
                if (level < 1) level = 1;
                if (level > 6) level = 6;

                if (sb.Length > 0 && sb[sb.Length - 1] != '\n')
                {
                    sb.AppendLine();
                }
                sb.Append(new string('#', level));
                sb.Append(' ');
                sb.AppendLine(lineText.Trim());
                sb.AppendLine();
            }
            else
            {
                sb.AppendLine(lineText.Trim());
            }
        }
        private void AppendTableMarkdown(StringBuilder sb, List<TextLine> tableLines)
        {
            if (tableLines == null || tableLines.Count == 0) return;
            tableLines.Sort((a, b) => b.Y.CompareTo(a.Y));
            TextLine headerLine = tableLines[0];
            int colCount = headerLine.Words.Count;
            sb.Append("| ");

            foreach (var cell in headerLine.Words)
            {
                string cellText = string.IsNullOrWhiteSpace(cell.Text) ? " " : cell.Text;
                sb.Append(cellText);
                sb.Append(" | ");
            }
            sb.AppendLine();
            sb.Append("|");

            for (int c = 0; c < colCount; c++)
            {
                sb.Append(" --- |");
            }
            sb.AppendLine();

            for (int r = 1; r < tableLines.Count; r++)
            {
                sb.Append("| ");
                foreach (var cell in tableLines[r].Words)
                {
                    string cellText = string.IsNullOrWhiteSpace(cell.Text) ? " " : cell.Text;
                    sb.Append(cellText);
                    sb.Append(" | ");
                }
                sb.AppendLine();
            }
            sb.AppendLine();
        }
        private string EscapeMarkdown(string text)
        {
            if (text == null) return "";

            var sb = new StringBuilder();
            foreach (char c in text)
            {
                switch (c)
                {
                    case '.': 
                        sb.Append('.'); 
                        break;
                    default:
                        sb.Append(c);
                        break;
                }
            }
            return sb.ToString();
        }
    }
}
