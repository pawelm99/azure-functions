using EGC = customer_integration.Infrastructure.CRM.Models;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace customer_integration.Infrastructure.CRM.Owner
{
    public class OwnerCrmClient : IOwnerCrmClient
    {
        private readonly ServiceClient _serviceClient;


        public OwnerCrmClient(ServiceClient serviceClient)
        {
            _serviceClient = serviceClient;
        }

        public async Task<EGC.dev_owner?> GetOwnerAsync(Guid ownerId, CancellationToken cancellationToken)
           => (await _serviceClient.RetrieveAsync(EGC.dev_owner.EntityLogicalName, ownerId, new ColumnSet(EGC.dev_owner.Fields.dev_name), cancellationToken))
            ?.ToEntity<EGC.dev_owner>();
    }
}
