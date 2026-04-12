using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk.Query;
using EGC = customer_integration.Infrastructure.CRM.Models;

namespace customer_integration.Infrastructure.CRM.Contact
{
    public class ContactCrmClient : IContactCrmClient
    {
        private readonly ServiceClient _serviceClient;


        public ContactCrmClient(ServiceClient serviceClient)
        {
            _serviceClient = serviceClient;
        }


        public async Task<EGC.Contact?> GetContactByNameAsync(string name, CancellationToken cancellationToken)
           => (await _serviceClient.RetrieveMultipleAsync(new QueryExpression
           {
               EntityName = EGC.Contact.EntityLogicalName,
               ColumnSet = new ColumnSet(EGC.Contact.Fields.LastName),
               Criteria =
               {
                   Conditions =
                   {
                       new ConditionExpression(EGC.Contact.Fields.LastName, ConditionOperator.Equal, name)
                   }
               }
           }, cancellationToken))?.Entities.Select(x => x.ToEntity<EGC.Contact>()).FirstOrDefault();
    }
}
