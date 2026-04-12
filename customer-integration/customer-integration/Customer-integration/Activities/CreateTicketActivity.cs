using customer_integration.Application.Models;
using customer_integration.Domain.Services.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace customer_integration.Activities
{
    public class CreateTicketActivity
    {
        private readonly ICaseService _caseService;


        public CreateTicketActivity(ICaseService caseService)
        {
            _caseService = caseService;
        }


        [Function(nameof(CreateTicketActivity))]
        public async Task<Guid> Run([ActivityTrigger] CaseState CaseState, FunctionContext executionContext,
            CancellationToken cancellationToken = default)
        {
            ILogger logger = executionContext.GetLogger(nameof(CreateTicketActivity));

            Guid caseId = await _caseService.CreateCaseAsync(CaseState, cancellationToken);
            logger.LogInformation($"Craete case with id: {caseId}.");

            return caseId;
        }
    }
}
