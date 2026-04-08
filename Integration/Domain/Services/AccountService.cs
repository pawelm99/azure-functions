using Integration.Application.Models;
using Integration.Domain.Services.Interfaces;
using Integration.Infrastructure.CRM.Account;
using EGC =  Integration.Infrastructure.CRM.Models;

namespace Integration.Domain.Services
{
    public class AccountService : IAccountService
    {
        public readonly IAccountCrmClient _accountClient;


        public AccountService(IAccountCrmClient accountClient)
        {
            _accountClient = accountClient;
        }


        public async Task<Guid> CreateSellerAsync(Vendor vendor, CancellationToken cancellationToken = default)
            => await _accountClient.CreateSellerAsync(vendor, cancellationToken);


        public async Task<Guid> CreateCustomerAsync(Customer customer, CancellationToken cancellationToken = default)
            => await _accountClient.CreateCustomerAsync(customer, cancellationToken);


        public async Task<EGC.Account?> RetrieveAccountAsync(string accountName, CancellationToken cancellationToken = default)
            => await _accountClient.RetrieveAccountAsync(accountName, cancellationToken);
    }
}
