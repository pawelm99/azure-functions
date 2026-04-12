using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace customer_integration.Extensions
{
    public static class ServiceClientExtensions
    {
        public async static Task<T?> RetrieveRecordByAttribute<T>(this ServiceClient serviceClient, string attribute, object value,
            CancellationToken cancellationToken = default) where T : Entity, new()
        {
            var records = new T();

            var result = await (serviceClient.RetrieveMultipleAsync(new QueryExpression
            {
                EntityName = records.LogicalName,
                ColumnSet = new ColumnSet(true),
                Criteria = new FilterExpression
                {
                    Conditions =
                    {
                        new ConditionExpression(attribute, ConditionOperator.Equal, value)
                    }
                }
            }, cancellationToken));

            return result?.Entities?.Select(x => x?.ToEntity<T>())?.FirstOrDefault();
        }
    }
}
