using customer_integration.Application.Models;
using customer_integration.Domain.Services.Interfaces;
using customer_integration.Infrastructure.CRM.Case;
using customer_integration.Infrastructure.CRM.Models;

namespace customer_integration.Domain.Services
{
    public class CaseService : ICaseService
    {
        public readonly ICaseCrmClient _caseClient;


        public CaseService(ICaseCrmClient caseClient)
        {
            _caseClient = caseClient;
        }


        public async Task<Guid> CreateCaseAsync(CaseState caseState, CancellationToken cancellationToken = default)
          => await _caseClient.CreateCaseAsync(caseState, cancellationToken);


        public async Task UpdateCaseAsync(Guid id, string priority, CancellationToken cancellationToken = default)
            => await _caseClient.UpdateCaseAsync(id, priority, cancellationToken);


        public async Task<dev_Case> GetCaseAsync(Guid caseId, CancellationToken cancellationToken)
            => await _caseClient.GetCaseAsync(caseId, cancellationToken);


        public async Task<List<dev_Case?>> GetAllCaseAsync(CancellationToken cancellationToken)
          => await _caseClient.GetAllCaseAsync(cancellationToken);
    }
}
