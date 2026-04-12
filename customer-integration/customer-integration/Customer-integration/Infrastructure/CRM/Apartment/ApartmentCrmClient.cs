using customer_integration.Infrastructure.CRM.Models;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk.Query;

namespace customer_integration.Infrastructure.CRM.Apartment
{
    public class ApartmentCrmClient : IApartmentCrmClient
    {
        private readonly ServiceClient _serviceClient;

        public ApartmentCrmClient(ServiceClient serviceClient)
        {
            _serviceClient = serviceClient;
        }


        public async Task<dev_Apartment?> GetApartmentByNameAsync(string name, CancellationToken cancellationToken)
            => (await _serviceClient.RetrieveMultipleAsync(new QueryExpression
            {
                EntityName = dev_Apartment.EntityLogicalName,
                ColumnSet = new ColumnSet(dev_Apartment.Fields.dev_name, dev_Apartment.Fields.dev_appartment),
                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(dev_Apartment.Fields.dev_name, ConditionOperator.Equal, name)
                    }
                }
            }, cancellationToken))?.Entities?.Select(x => x?.ToEntity<dev_Apartment>())?.FirstOrDefault();
    }
}
