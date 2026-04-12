using customer_integration.Application.Models;
using customer_integration.Orchestrators;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.DurableTask.Client;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace customer_integration.Functions
{
    public class StartOrchestration
    {
        [Function("Orchestrator_HttpStart")]
        public static async Task<HttpResponseData> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req,
            [DurableClient] DurableTaskClient client, FunctionContext executionContext, CancellationToken cancellationToken)
        {
            ILogger logger = executionContext.GetLogger("Orchestrator_HttpStart");
            var requestBody = await req.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(requestBody)) throw new InvalidDataException("Input cannot be null!");

            var createCaseModel = JsonSerializer.Deserialize<CreateCaseModel>(requestBody, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            string instanceId = await client.ScheduleNewOrchestrationInstanceAsync(
                nameof(Orchestrator), createCaseModel);

            logger.LogInformation("Started orchestration with ID = '{instanceId}'.", instanceId);

            return await client.CreateCheckStatusResponseAsync(req, instanceId, cancellationToken);
        }
    }
}
