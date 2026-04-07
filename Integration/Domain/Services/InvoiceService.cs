using Integration.Application.Models;
using Integration.Domain.Services.Interfaces;
using Integration.Infrastructure.CRM.Invoice;

namespace Integration.Domain.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IInvoiceCrmClient _invoiceClient;


        public InvoiceService(IInvoiceCrmClient invoiceClient)
        {
            _invoiceClient = invoiceClient;
        }


        public async Task<Guid> CreateInvoiceAsync(Invoice invoice, CancellationToken cancellationToken = default)
            => await _invoiceClient.CreateInvoiceAsync(invoice, cancellationToken);
    }
}
