using customer_integration.Application.Models;
using customer_integration.Infrastructure.CRM.Models;

namespace customer_integration.Infrastructure.CRM.Case
{
    public interface ICaseCrmClient
    {
        Task<Guid> CreateCaseAsync(CaseState caseState, CancellationToken cancellationToken);
        Task<dev_Case> GetCaseAsync(Guid caseId, CancellationToken cancellationToken);
        Task UpdateCaseAsync(Guid id, string priority, CancellationToken cancellationToken);
    }
}
