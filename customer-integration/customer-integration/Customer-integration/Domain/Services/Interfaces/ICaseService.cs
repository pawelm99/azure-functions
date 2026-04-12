using customer_integration.Application.Models;
using customer_integration.Infrastructure.CRM.Models;

namespace customer_integration.Domain.Services.Interfaces
{
    public interface ICaseService
    {
        Task<Guid> CreateCaseAsync(CaseState caseState, CancellationToken cancellationToken = default);
        Task<dev_Case> GetCaseAsync(Guid caseId, CancellationToken cancellationToken = default);
        Task UpdateCaseAsync(Guid id, string priority, CancellationToken cancellationToken = default);
    }
}
