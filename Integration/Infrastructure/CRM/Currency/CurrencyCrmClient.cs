using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk.Query;
using EGC =  Integration.Infrastructure.CRM.Models;

namespace Integration.Infrastructure.CRM.Currency
{
    internal class CurrencyCrmClient : ICurrencyCrmClient
    {
        private readonly ServiceClient _serviceClient;


        public CurrencyCrmClient(ServiceClient serviceClient)
        {
            _serviceClient = serviceClient;
        }


        public async Task<EGC.TransactionCurrency?> RetrieveTransactionCurrencyBySymbolAsync(string code, CancellationToken cancellationToken = default)
           => (await _serviceClient.RetrieveMultipleAsync(new QueryExpression
           {
               TopCount = 1,
               EntityName = EGC.TransactionCurrency.EntityLogicalName,
               ColumnSet = new ColumnSet(false),
               Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(EGC.TransactionCurrency.Fields.ISOCurrencyCode, ConditionOperator.Equal, code),
                        new ConditionExpression(EGC.TransactionCurrency.Fields.StateCode, ConditionOperator.Equal, (int)EGC.TransactionCurrencyState.Active),
                        new ConditionExpression(EGC.TransactionCurrency.Fields.StatusCode, ConditionOperator.Equal, (int)EGC.TransactionCurrency_StatusCode.Active),
                    }
                }
           }, cancellationToken)).Entities
            .Select(ac => ac.ToEntity<EGC.TransactionCurrency>())
            .FirstOrDefault();
    }
}
