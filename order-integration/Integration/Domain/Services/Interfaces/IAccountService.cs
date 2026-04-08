using Integration.Application.Models;
using Integration.Infrastructure.CRM.Models;

namespace Integration.Domain.Services.Interfaces
{
    public interface IAccountService
    {
        Task<Guid> CreateCustomerAsync(Customer customer, CancellationToken cancellationToken = default);
        Task<Guid> CreateSellerAsync(Vendor vendor, CancellationToken cancellationToken = default);
        Task<Account?> RetrieveAccountAsync(string accountName, CancellationToken cancellationToken = default);
    }
}
