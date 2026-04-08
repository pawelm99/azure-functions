using Integration.Domain.Services.Interfaces;
using Integration.Infrastructure.CRM.Currency;
using EGC =  Integration.Infrastructure.CRM.Models;

namespace Integration.Domain.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly ICurrencyCrmClient _currencyClient;


        public CurrencyService(ICurrencyCrmClient currencyClient)
        {
            _currencyClient = currencyClient;
        }


        public async Task<EGC.TransactionCurrency?> RetrieveTransactionCurrencyBySymbolAsync(string code, CancellationToken cancellationToken = default)
            => await _currencyClient.RetrieveTransactionCurrencyBySymbolAsync(code, cancellationToken);
    }
}
