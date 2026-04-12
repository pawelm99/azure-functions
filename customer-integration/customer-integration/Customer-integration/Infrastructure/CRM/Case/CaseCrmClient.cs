using customer_integration.Application.Models;
using customer_integration.Extensions;
using customer_integration.Infrastructure.CRM.Models;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk.Query;
using EGC = customer_integration.Infrastructure.CRM.Models;

namespace customer_integration.Infrastructure.CRM.Case
{
    public class CaseCrmClient : ICaseCrmClient
    {
        private readonly ServiceClient _serviceClient;


        public CaseCrmClient(ServiceClient serviceClient)
        {
            _serviceClient = serviceClient;
        }


        public async Task<Guid> CreateCaseAsync(CaseState caseState, CancellationToken cancellationToken)
           => await _serviceClient.CreateAsync(new EGC.dev_Case
           {
               dev_title = caseState.Title,
               StatusCode = EGC.dev_Case_StatusCode.Active,
               dev_description = caseState.Description,
               dev_customer = string.IsNullOrWhiteSpace(caseState.Customer) ? null
               : (await _serviceClient.RetrieveRecordByAttribute<EGC.dev_Customer>(EGC.dev_Customer.Fields.dev_name, caseState.Customer))
               ?.ToEntityReference(),
               dev_apartment = string.IsNullOrWhiteSpace(caseState.Apartment) ? null
               : (await _serviceClient.RetrieveRecordByAttribute<EGC.dev_Apartment>(EGC.dev_Customer.Fields.dev_name, caseState.Apartment))
               ?.ToEntityReference()
           }, cancellationToken);


        public async Task<dev_Case> GetCaseAsync(Guid caseId, CancellationToken cancellationToken)
            => (await _serviceClient.RetrieveAsync(dev_Case.EntityLogicalName, caseId,
                new ColumnSet(dev_Case.Fields.dev_description, 
                    dev_Case.Fields.dev_customer, 
                    dev_Case.Fields.dev_title, 
                    dev_Case.Fields.dev_apartment),
                cancellationToken))
            .ToEntity<dev_Case>();


        public async Task UpdateCaseAsync(Guid id, string priority, CancellationToken cancellationToken)
          => await _serviceClient.UpdateAsync(new EGC.dev_Case
          {
              Id = id,
              dev_priority = MapToOptionSet(priority)
          }, cancellationToken);


        private dev_Case_dev_priority? MapToOptionSet(string priority)
            => priority switch
            {
                "HIGH" => dev_Case_dev_priority.High,
                "MEDIUM" => dev_Case_dev_priority.Medium,
                "LOW" => dev_Case_dev_priority.Low,
                _ => null
            };
    }
}
