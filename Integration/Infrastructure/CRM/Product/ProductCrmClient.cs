using Integration.Domain.Services.Interfaces;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using EGC =  Integration.Infrastructure.CRM.Models;

namespace Integration.Infrastructure.CRM.Product
{
    internal class ProductCrmClient : IProductCrmClient
    {
        private readonly ServiceClient _serviceClient;
        private readonly ICurrencyService _currencyService;


        public ProductCrmClient(ServiceClient serviceClient, ICurrencyService currencyService)
        {
            _serviceClient = serviceClient;
            _currencyService = currencyService;
        }


        public async Task<Guid> CreateProductAsync(Data.Domain.Product product, CancellationToken cancellationToken = default)
        {
            EGC.TransactionCurrency currency = await _currencyService.RetrieveTransactionCurrencyBySymbolAsync(product.CurrencyCode!, cancellationToken);
            return await _serviceClient.CreateAsync(new EGC.dev_Produkt
            {
                dev_NazwaProduktu = product.Name,
                dev_Cena = new Money(product.Price.HasValue ? Math.Round((decimal)product.Price.Value, 2) : 0),
                dev_quantity = product.Quantity,
                TransactionCurrencyId = currency.ToEntityReference(),
            }, cancellationToken);
        }


        public async Task<EGC.dev_Produkt?> RetrieveProductAsync(string description, CancellationToken cancellationToken = default)
             => (await _serviceClient.RetrieveMultipleAsync(new QueryExpression
             {
                 TopCount = 1,
                 EntityName = EGC.dev_Produkt.EntityLogicalName,
                 ColumnSet = new ColumnSet(false),
                 Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(EGC.dev_Produkt.Fields.dev_NazwaProduktu, ConditionOperator.Equal, description),
                        new ConditionExpression(EGC.dev_Produkt.Fields.StateCode, ConditionOperator.Equal, (int)EGC.dev_ConfigurationState.Active),
                        new ConditionExpression(EGC.dev_Produkt.Fields.StatusCode, ConditionOperator.Equal, (int)EGC.dev_Configuration_StatusCode.Active),
                    }
                }
             }, cancellationToken)).Entities
            .Select(ac => ac.ToEntity<EGC.dev_Produkt>())
            .FirstOrDefault();
    }
}
