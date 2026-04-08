using Integration.Infrastructure.CRM.Models;
using Integration.Domain.Services.Interfaces;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;
using EGC =  Integration.Infrastructure.CRM.Models;

namespace Integration.Infrastructure.CRM.Order
{
    internal class OrderCrmClient : IOrderCrmClient
    {
        private readonly ServiceClient _serviceClient;
        private readonly IConfigurationService _configuration;


        public OrderCrmClient(ServiceClient serviceClient, IConfigurationService configuration)
        {
            _serviceClient = serviceClient;
            _configuration = configuration;
        }


        public async Task<Guid> CreateOrderAsync(List<Application.Models.Invoice> invoices, CancellationToken cancellationToken = default)
        {
            EGC.dev_Configuration? configuration = await _configuration.RetrieveConfigurationAsync(cancellationToken);
            if (configuration == null) return Guid.Empty;

            Guid orderId = await _serviceClient.CreateAsync(new EGC.dev_Order
            {
                dev_Name = $"SO-{DateTime.UtcNow.ToString("dd/MM/yyyy")}-{configuration.dev_ordernumber}",
                dev_customer = new EntityReference(EGC.Account.EntityLogicalName, invoices.First().Customer.Id),
                StatusCode = EGC.dev_Order_StatusCode.Confirmed,
                StateCode = EGC.dev_OrderState.Active,
                dev_orderdate = invoices.First().InvoiceDate.HasValue ? invoices.First().InvoiceDate!.Value.UtcDateTime : null,
            }, cancellationToken);

            foreach (var invoice in invoices)
            {
                await _serviceClient.UpdateAsync(new dev_Invoice
                {
                    Id = invoice.Id,
                    dev_order = new EntityReference(EGC.dev_Order.EntityLogicalName, orderId),
                });

            }
            await _configuration.UpdateOrderNumber(configuration);
            return orderId;
        }
    }
}
