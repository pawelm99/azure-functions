using CrmEarlyBound;

namespace Integration.Domain.Services.Interfaces
{
    public interface ICurrencyService
    {
        public Task<TransactionCurrency?> RetrieveTransactionCurrencyBySymbolAsync(string code, CancellationToken cancellationToken = default);
    }
}
