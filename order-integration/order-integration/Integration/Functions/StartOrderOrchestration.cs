using Microsoft.Azure.Functions.Worker;
using Microsoft.DurableTask.Client;
using Microsoft.Extensions.Logging;

namespace Integration.Functions;

public class StartOrderOrchestration
{
    private readonly ILogger _logger;


    public StartOrderOrchestration(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<StartOrderOrchestration>();
    }


    [Function(nameof(StartOrderOrchestration))]
    public async Task Run([ServiceBusTrigger("orders-queue", Connection = "ServiceBusConnection")] string message,
        [DurableClient] DurableTaskClient client)
    {
        var instanceId = await client.ScheduleNewOrchestrationInstanceAsync("OrderOrchestrator", message);
        _logger.LogInformation($"Started orchestration with ID = {instanceId}");
    }
}