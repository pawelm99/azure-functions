namespace Integration.Infrastructure.CRM.Invoice
{
    public interface IInvoiceCrmClient
    {
        Task<Guid> CreateInvoiceAsync(Application.Models.Invoice invoice, CancellationToken cancellationToken = default);
    }
}
