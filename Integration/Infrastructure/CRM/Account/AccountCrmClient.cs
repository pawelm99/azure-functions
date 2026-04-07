using Integration.Application.Models;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk.Query;
using EGC = CrmEarlyBound;

namespace Integration.Infrastructure.CRM.Account
{
    internal class AccountCrmClient : IAccountCrmClient
    {
        private readonly ServiceClient _serviceClient;


        public AccountCrmClient(ServiceClient serviceClient)
        {
            _serviceClient = serviceClient;
        }


        public async Task<Guid> CreateSellerAsync(Vendor vendor, CancellationToken cancellationToken = default)
            => await _serviceClient.CreateAsync(new EGC.Account
            {
                dev_tin = vendor.TaxId,
                Name = vendor.Name,
                Address1_PostalCode = vendor.Address?.PostalCode,
                Address1_City = vendor.Address?.City,
                Address1_StateOrProvince = vendor.Address?.State,
                Address1_County = vendor.Address?.CountryRegion,
                Address1_Line1 = vendor.AddressRecipient,
            }, cancellationToken);


        public async Task<Guid> CreateCustomerAsync(Customer customer, CancellationToken cancellationToken = default)
          => await _serviceClient.CreateAsync(new EGC.Account
          {
              dev_tin = customer.TaxId,
              Name = customer.Name,
              Address1_PostalCode = customer.Address?.PostalCode,
              Address1_City = customer.Address?.City,
              Address1_StateOrProvince = customer.Address?.State,
              Address1_County = customer.Address?.CountryRegion,
              Address1_Line1 = customer.AddressRecipient,
          }, cancellationToken);


        public async Task<EGC.Account?> RetrieveAccountAsync(string tin, CancellationToken cancellationToken = default)
         => (await _serviceClient.RetrieveMultipleAsync(new QueryExpression
         {
             TopCount = 1,
             EntityName = EGC.Account.EntityLogicalName,
             ColumnSet = new ColumnSet(false),
             Criteria =
             {
                 Conditions =
                 {
                     new ConditionExpression(EGC.Account.Fields.dev_tin, ConditionOperator.Equal, tin)
                 }
             }
         }, cancellationToken))
            .Entities
            .Select(ac => ac.ToEntity<EGC.Account>())
            .FirstOrDefault();
    }
}
