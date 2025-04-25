namespace Domain.Interfaces
{
    public interface IPdfToMarkdownService
    {
        Task<string> ConvertPdfToMarkdownAsync(byte[] pdfFileBytes);
    }
}