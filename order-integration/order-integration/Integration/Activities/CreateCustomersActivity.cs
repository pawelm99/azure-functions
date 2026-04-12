using Integration.Application.Models;
using Integration.Domain.Services.Interfaces;
using Integration.Infrastructure.CRM.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Integration.Activities
{
    public class CreateCustomersActivity
    {

        private readonly IAccountService _accountService;
        private readonly ILogger _logger;


        public CreateCustomersActivity(IAccountService accountService, ILoggerFactory loggerFactory)
        {
            _accountService = accountService;
            _logger = loggerFactory.CreateLogger<CreateCustomersActivity>();
        }
      

        [Function(nameof(CreateCustomersActivity))]
        public async Task<List<Invoice>> Run([ActivityTrigger] List<Invoice> invoices, CancellationToken cancellationToken = default)
        {
            foreach (Invoice invoice in invoices)
            {
                Account? account = string.IsNullOrWhiteSpace(invoice.Customer.TaxId)
                    ? null : await _accountService.RetrieveAccountAsync(invoice.Customer.TaxId!, cancellationToken);

                if (account != null)
                {
                    _logger.LogInformation($"Exists customer with id: {account.Id}");
                    invoice.Customer.Id = account.Id;
                    continue;
                }
                if (string.IsNullOrWhiteSpace(invoice.Customer.TaxId)) continue;

                var customerId = await _accountService.CreateCustomerAsync(invoice.Customer, cancellationToken);
                _logger.LogInformation($"Created customer with id: {customerId}");
                invoice.Customer.Id = customerId;
            }
            return invoices;
        }
    }
}
