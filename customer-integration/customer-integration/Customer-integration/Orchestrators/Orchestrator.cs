using customer_integration.Activities;
using customer_integration.Application.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.DurableTask;
using Microsoft.Extensions.Logging;

namespace customer_integration.Orchestrators;

public static class Orchestrator
{
    [Function(nameof(Orchestrator))]
    public static async Task RunOrchestrator(
        [OrchestrationTrigger] TaskOrchestrationContext context)
    {
        ILogger logger = context.CreateReplaySafeLogger(nameof(Orchestrator));

        var createCaseModel = context.GetInput<CreateCaseModel>();
        if (createCaseModel == null) return;

        var state = new CaseState(createCaseModel.Title,
            createCaseModel.Customer,
            createCaseModel.Apartment,
            createCaseModel.Status,
            createCaseModel.Description);

        state.CaseId = await context.CallActivityAsync<Guid>(nameof(CreateTicketActivity), state);
        logger.LogInformation("Call activity return case id: {CaseId}", state.CaseId);

        state.Priority = await context.CallActivityAsync<string>(nameof(SetPriorityActivity), state);
        logger.LogInformation("Call activity return priority: {Priority}", state.Priority);

        state.AppointmentId = await context.CallActivityAsync<Guid>(nameof(CreateAppointmentActivity), state);
        logger.LogInformation("Call activity return appointment id: {AppointmentId}", state.AppointmentId);

        state.EmailId = await context.CallActivityAsync<Guid?>(nameof(SendEmailActivity), state);
        logger.LogInformation("Call activity return emailId: {EmailId}", state.EmailId);


    }
}