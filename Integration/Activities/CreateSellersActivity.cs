using CrmEarlyBound;
using Integration.Application.Models;
using Integration.Domain.Services.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Integration.Activities
{
    public class CreateSellersActivity
    {
        private readonly IAccountService _accountService;
        private readonly ILogger _logger;


        public CreateSellersActivity(IAccountService accountService, ILoggerFactory loggerFactory)
        {
            _accountService = accountService;
            _logger = loggerFactory.CreateLogger<CreateSellersActivity>();
        }


        [Function(nameof(CreateSellersActivity))]
        public async Task<List<Invoice>> Run([ActivityTrigger] List<Invoice> invoices, CancellationToken cancellationToken = default)
        {
            foreach (Invoice invoice in invoices)
            {
                Account? account = string.IsNullOrWhiteSpace(invoice.Vendor.TaxId) 
                    ? null : await _accountService.RetrieveAccountAsync(invoice.Vendor.TaxId!, cancellationToken);

                if (account != null)
                {
                    _logger.LogInformation($"Exists seller with id: {account.Id}");
                    Console.WriteLine($"Exists seller with id: {account.Id}");
                    invoice.Vendor.Id = account.Id;
                    continue;
                }
                if (string.IsNullOrWhiteSpace(invoice.Vendor.TaxId)) continue;

                var sallerId = await _accountService.CreateSellerAsync(invoice.Vendor, cancellationToken);
                _logger.LogInformation($"Created seller with id: {sallerId}");
                Console.WriteLine($"Created seller with id: {sallerId}");
                invoice.Vendor.Id = sallerId;
            }
            return invoices;
        }
    }
}
