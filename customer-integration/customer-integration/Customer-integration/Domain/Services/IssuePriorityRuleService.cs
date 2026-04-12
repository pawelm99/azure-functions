using customer_integration.Domain.Services.Interfaces;
using customer_integration.Infrastructure.SQL.IssuePriorityRule;
using Data.Domain;

namespace customer_integration.Domain.Services
{
    public class IssuePriorityRuleService : IIssuePriorityRuleService
    {
        public readonly IIssuePriorityRuleRepository _issuePriorityRuleRepository;


        public IssuePriorityRuleService(IIssuePriorityRuleRepository issuePriorityRuleRepository)
        {
            _issuePriorityRuleRepository = issuePriorityRuleRepository;
        }


        public async Task<IssuePriorityRule?> RetrieveIssuePriorityRuleAsync(string issueType, CancellationToken cancellationToken = default)
            => await _issuePriorityRuleRepository.RetrieveIssuePriorityRuleAsync(issueType, cancellationToken);
    }
}
