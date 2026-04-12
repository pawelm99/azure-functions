using Integration.Domain.Services.Interfaces;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;
using EGC =  Integration.Infrastructure.CRM.Models;

namespace Integration.Infrastructure.CRM.Invoice
{
    internal class InvoiceCrmClient : IInvoiceCrmClient
    {
        private readonly ServiceClient _serviceClient;
        private readonly ICurrencyService _currencyService;


        public InvoiceCrmClient(ServiceClient serviceClient, ICurrencyService currencyService)
        {
            _serviceClient = serviceClient;
            _currencyService = currencyService;
        }


        public async Task<Guid> CreateInvoiceAsync(Application.Models.Invoice invoice, CancellationToken cancellationToken = default)
        {
            EGC.TransactionCurrency? currency = invoice.InvoiceTotal != null
                ? await _currencyService.RetrieveTransactionCurrencyBySymbolAsync(invoice.InvoiceTotal.CurrencyCode!, cancellationToken) : null;

            return await _serviceClient.CreateAsync(new EGC.dev_Invoice
            {
                dev_invoicenumber = invoice.InvoiceId,
                dev_customer = new EntityReference(EGC.Account.EntityLogicalName, invoice.Customer.Id),
                dev_vendor = new EntityReference(EGC.Account.EntityLogicalName, invoice.Vendor.Id),
                dev_invoicedate = invoice.InvoiceDate.HasValue ? invoice.InvoiceDate.Value.UtcDateTime : null,
                dev_grossamount = new Money(invoice.InvoiceTotal?.Amount != null ? Math.Round((decimal)invoice.InvoiceTotal.Amount, 2) : 0),
                dev_Paymentterms = invoice.PaymentTerm,
                dev_iban = invoice.PaymentDetails?.FirstOrDefault()?.IBAN,
                dev_netamount = new Money(invoice.SubTotal?.Amount != null ? Math.Round((decimal)invoice.SubTotal.Amount, 2) : 0),
                TransactionCurrencyId = currency != null ? currency.ToEntityReference() : null,
                dev_VendortaxID = invoice.Vendor.TaxId
            }, cancellationToken);
        }
    }
}
