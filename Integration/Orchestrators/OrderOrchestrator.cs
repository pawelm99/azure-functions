using Integration.Activities;
using Integration.Application.Models;
using Integration.Configuration;
using Microsoft.Azure.Functions.Worker;
using Microsoft.DurableTask;
using Microsoft.Extensions.Logging;

namespace Integration.Orchestrators;

public class OrderOrchestrator
{
    private readonly ILogger _logger;
    private readonly ITaskOptionsProvider _taskOptionsProvider;


    public OrderOrchestrator(ILoggerFactory loggerFactory, ITaskOptionsProvider taskOptionsProvider)
    {
        _logger = loggerFactory.CreateLogger<OrderOrchestrator>();
        _taskOptionsProvider = taskOptionsProvider;
    }


    [Function(nameof(OrderOrchestrator))]
    public async Task RunOrchestrator(
        [OrchestrationTrigger] TaskOrchestrationContext context, CancellationToken cancellationToken = default)
    {
        var taskOptions = _taskOptionsProvider.GetDefaultTaskOptions();
        var message = context.GetInput<string>();


        List<Invoice>? invoices = await CallActivityWithFailureHandling<List<Invoice>?>(context, nameof(ProcessDocumentActivity), message, taskOptions);
        invoices = await CallActivityWithFailureHandling<List<Invoice>>(context, nameof(CreateSellersActivity), invoices, taskOptions);
        invoices = await CallActivityWithFailureHandling<List<Invoice>>(context, nameof(CreateCustomersActivity), invoices, taskOptions);
        invoices = await CallActivityWithFailureHandling<List<Invoice>>(context, nameof(CreateInvoicesActivity), invoices, taskOptions);
        invoices = await CallActivityWithFailureHandling<List<Invoice>>(context, nameof(CreateProductsActivity), invoices, taskOptions);
        Guid orderId = await CallActivityWithFailureHandling<Guid>(context, nameof(CreateOrderActivity), invoices, taskOptions);
    }


    private async Task<T> CallActivityWithFailureHandling<T>(
        TaskOrchestrationContext context,
        string activityName,
        object? input,
        TaskOptions taskOptions)
    {
        try
        {
            _logger.LogInformation("Invoking step: {ActivityName}", activityName);
            return await context.CallActivityAsync<T>(activityName, input, taskOptions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Step {ActivityName} failed for instance {InstanceId}", activityName, context.InstanceId);

            await context.CallActivityAsync(nameof(SendFailureToQueueActivity), new ErrorDetails
            {
                Step = activityName,
                InstanceId = context.InstanceId,
                ErrorMessage = ex.Message
            });

            throw;
        }
    }
}