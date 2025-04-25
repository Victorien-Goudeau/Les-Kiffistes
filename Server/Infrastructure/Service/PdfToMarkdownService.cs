using Domain.Interfaces;
using Spire.Pdf;

namespace Infrastructure.Service
{
    public class PdfToMarkdownService : IPdfToMarkdownService
    {
        public async Task<string> ConvertPdfToMarkdownAsync(byte[] pdfFileBytes)
        {
            // Load the PDF document from the byte array
            using (var memoryStream = new MemoryStream(pdfFileBytes))
            {
                PdfDocument pdf = new PdfDocument();
                pdf.LoadFromStream(memoryStream);

                // Convert the PDF to Markdown format
                using (var markdownStream = new MemoryStream())
                {
                    pdf.SaveToStream(markdownStream, FileFormat.Markdown);
                    markdownStream.Position = 0;

                    using (var reader = new StreamReader(markdownStream))
                    {
                        return await reader.ReadToEndAsync();
                    }
                }
            }
        }
    }    
}
