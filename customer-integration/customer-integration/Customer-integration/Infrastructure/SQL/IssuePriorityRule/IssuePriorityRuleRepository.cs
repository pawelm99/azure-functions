using Data.Context;
using Microsoft.EntityFrameworkCore;

namespace customer_integration.Infrastructure.SQL.IssuePriorityRule
{
    public class IssuePriorityRuleRepository : IIssuePriorityRuleRepository
    {
        public readonly AppDbContext _dbContext;


        public IssuePriorityRuleRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<Data.Domain.IssuePriorityRule?> RetrieveIssuePriorityRuleAsync(string issueType, CancellationToken cancellationToken = default)
            => await _dbContext.IssuePriorityRules.AsNoTracking().FirstOrDefaultAsync(p => p.IssueType == issueType, cancellationToken);
    }
}
