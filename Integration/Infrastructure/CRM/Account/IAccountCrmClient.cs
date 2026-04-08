using Integration.Application.Models;
using EGC = Integration.Infrastructure.CRM.Models;

namespace Integration.Infrastructure.CRM.Account
{
    public interface IAccountCrmClient
    {
        Task<Guid> CreateCustomerAsync(Customer customer, CancellationToken cancellationToken = default);
        Task<Guid> CreateSellerAsync(Vendor vendor, CancellationToken cancellationToken = default);
        Task<EGC.Account?> RetrieveAccountAsync(string accountName, CancellationToken cancellationToken = default);
    }
}
