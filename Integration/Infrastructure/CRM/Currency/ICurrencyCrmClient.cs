using EGC =  Integration.Infrastructure.CRM.Models;

namespace Integration.Infrastructure.CRM.Currency
{
    public interface ICurrencyCrmClient
    {
        Task<EGC.TransactionCurrency?> RetrieveTransactionCurrencyBySymbolAsync(string code, CancellationToken cancellationToken = default);
    }
}
