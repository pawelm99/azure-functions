using Integration.Application.Models;

namespace Integration.Domain.Services.Interfaces
{
    public interface IDocumentService
    {
        Task<List<Invoice>?> ExtractInvoiceFromPdfAsync(string message);
    }
}
