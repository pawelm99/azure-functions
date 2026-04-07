using Integration.Application.Models;
using Integration.Domain.Services.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Integration.Activities
{
    public class CreateInvoicesActivity
    {
        public readonly IInvoiceService _invoiceService;
        public readonly ILogger _logger;


        public CreateInvoicesActivity(IInvoiceService invoiceService, ILoggerFactory loggerFactory)
        {
            _invoiceService = invoiceService;
            _logger = loggerFactory.CreateLogger<CreateInvoicesActivity>();
        }


        [Function(nameof(CreateInvoicesActivity))]
        public async Task<List<Invoice>> Run([ActivityTrigger] List<Invoice> invoices, CancellationToken cancellationToken = default)
        {
            foreach (Invoice invoice in invoices)
            {
                var invoiceId = await _invoiceService.CreateInvoiceAsync(invoice, cancellationToken);
                _logger.LogInformation($"Created invoice with id: {invoiceId}");
                Console.WriteLine($"Created invoice with id: {invoiceId}");
                invoice.Id = invoiceId;
            }
            return invoices;
        }
    }
}
