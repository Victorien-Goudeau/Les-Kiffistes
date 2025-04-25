using System.Text;
using UglyToad.PdfPig;
using Domain.Interfaces;

namespace Infrastructure.Service
{
    public class PdfToMarkdownService : IPdfToMarkdownService
    {
        public async Task<string> ConvertPdfToMarkdownAsync(byte[] pdfFileBytes)
        {
            if (pdfFileBytes == null || pdfFileBytes.Length == 0)
                throw new ArgumentException("PDF File can't be empty", nameof(pdfFileBytes));

            using (var memoryStream = new MemoryStream(pdfFileBytes))
            using (PdfDocument document = PdfDocument.Open(memoryStream))
            {
                var markdown = new StringBuilder();

                markdown.AppendLine("# Document 1");
                foreach (var page in document.GetPages())
                {
                    var pageText = page.Text;
                    markdown.AppendLine(pageText);
                    markdown.AppendLine();
                }

                return await Task.FromResult(markdown.ToString());
            }
        }
    }
}
