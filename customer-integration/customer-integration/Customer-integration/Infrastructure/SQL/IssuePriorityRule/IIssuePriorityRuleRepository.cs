
namespace customer_integration.Infrastructure.SQL.IssuePriorityRule
{
    public interface IIssuePriorityRuleRepository
    {
        public Task<Data.Domain.IssuePriorityRule?> RetrieveIssuePriorityRuleAsync(string issueType, CancellationToken cancellationToken);
    }
}
