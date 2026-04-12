using customer_integration.Application.Models;
using customer_integration.Domain.Services.Interfaces;
using customer_integration.Infrastructure.SQL.IssuePriorityRule;
using Data.Domain;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace customer_integration.Activities
{
    public class SetPriorityActivity
    {
        private readonly IIssuePriorityRuleService _iissuePriorityRuleService;
        private readonly ICaseService _caseService;


        public SetPriorityActivity(ICaseService caseService, IIssuePriorityRuleService iissuePriorityRuleService)
        {
            _iissuePriorityRuleService = iissuePriorityRuleService;
            _caseService = caseService;
        }


        [Function(nameof(SetPriorityActivity))]
        public async Task<string?> Run([ActivityTrigger] CaseState caseState, FunctionContext executionContext,
            CancellationToken cancellationToken = default)
        {
            ILogger logger = executionContext.GetLogger(nameof(SetPriorityActivity));

            IssuePriorityRule? issuePriorityRule = await _iissuePriorityRuleService.RetrieveIssuePriorityRuleAsync(caseState.Title, cancellationToken);
            if (issuePriorityRule == null)
            {
                logger.LogWarning($"No issue priority rule found for priority: {caseState.Title}.");
                return null;
            }

            await _caseService.UpdateCaseAsync(caseState.CaseId!.Value, issuePriorityRule.Priority, cancellationToken);
            logger.LogInformation($"Update case with id: {caseState.CaseId}.");
            return issuePriorityRule.Priority;
        }
    }
}
