
using Data.Domain;

namespace customer_integration.Domain.Services.Interfaces
{
    public interface IIssuePriorityRuleService
    {
        Task<IssuePriorityRule?> RetrieveIssuePriorityRuleAsync(string issueType, CancellationToken cancellationToken = default);
    }
}
