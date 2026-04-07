using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk.Query;
using EGC = CrmEarlyBound;

namespace Integration.Infrastructure.CRM.Configuration
{
    internal class ConfigurationCrmClient : IConfigurationCrmClient
    {
        private readonly ServiceClient _serviceClient;


        public ConfigurationCrmClient(ServiceClient serviceClient)
        {
            _serviceClient = serviceClient;
        }


        public async Task<EGC.dev_Configuration?> RetrieveConfigurationAsync(CancellationToken cancellationToken = default)
            => (await _serviceClient.RetrieveMultipleAsync(new QueryExpression
            {
                TopCount = 1,
                EntityName = EGC.dev_Configuration.EntityLogicalName,
                ColumnSet = new ColumnSet(EGC.dev_Configuration.Fields.dev_ordernumber),
                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(EGC.dev_Configuration.Fields.dev_ordernumber, ConditionOperator.NotNull),
                        new ConditionExpression(EGC.dev_Configuration.Fields.StateCode, ConditionOperator.Equal,(int)EGC.dev_ConfigurationState.Active),
                        new ConditionExpression(EGC.dev_Configuration.Fields.StatusCode, ConditionOperator.Equal, (int)EGC.dev_Configuration_StatusCode.Active),
                    }
                }
            }, cancellationToken)).Entities
            .Select(ac => ac.ToEntity<EGC.dev_Configuration>())
            .FirstOrDefault();


        public async Task UpdateOrderNumber(EGC.dev_Configuration configuration, CancellationToken cancellationToken = default)
        {
            string value = configuration.dev_ordernumber;

            int number = int.Parse(value);
            number++;

            await _serviceClient.UpdateAsync(new EGC.dev_Configuration
            {
                Id = configuration.Id,
                dev_ordernumber = number.ToString($"D{value.Length}")
            });
        }
    }
}
