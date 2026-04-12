using customer_integration.Application.Models;
using customer_integration.Infrastructure.CRM.Email;
using customer_integration.Infrastructure.CRM.Models;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using EGC = customer_integration.Infrastructure.CRM.Models;

namespace customer_integration.Infrastructure.CRM.SystemUser
{
    public class SystemUserClient : ISystemUserClient
    {
        private readonly string TechnicalUser = "# Development-Crm";


        private readonly ServiceClient _serviceClient;


        public SystemUserClient(ServiceClient serviceClient)
        {
            _serviceClient = serviceClient;
        }


        public async Task<EGC.SystemUser?> GetTechnicalUserAsync(CancellationToken cancellationToken)
            => (await _serviceClient.RetrieveMultipleAsync(new QueryExpression
            {
                EntityName = EGC.SystemUser.EntityLogicalName,
                ColumnSet = new ColumnSet(false),
                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(EGC.SystemUser.Fields.FullName, ConditionOperator.Equal, TechnicalUser)
                    }
                }
            }, cancellationToken))?.Entities?.Select(x => x?.ToEntity<EGC.SystemUser>())?.FirstOrDefault();
    }
}
