using Integration.Application.Models;

namespace Integration.Infrastructure.CRM.Account
{
    public interface IAccountCrmClient
    {
        Task<Guid> CreateCustomerAsync(Customer customer, CancellationToken cancellationToken = default);
        Task<Guid> CreateSellerAsync(Vendor vendor, CancellationToken cancellationToken = default);
        Task<CrmEarlyBound.Account?> RetrieveAccountAsync(string accountName, CancellationToken cancellationToken = default);
    }
}
