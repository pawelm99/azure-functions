using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using ServiceDeskPro_integration.Infrastructure.CRM.Models;
using ServiceDeskPro_integration.Models;
using System.Net;
using System.Text.Json;
using EGC = ServiceDeskPro_integration.Infrastructure.CRM.Models;


namespace ServiceDeskPro_integration.Activities;

public class ExternalTicketWebhook
{
    private readonly ILogger _logger;
    private readonly IConfiguration _configuration;


    public ExternalTicketWebhook(ILoggerFactory loggerFactory, IConfiguration configuration)
    {
        _configuration = configuration;
        _logger = loggerFactory.CreateLogger<ExternalTicketWebhook>();
    }


    [Function("ExternalTicketWebhook")]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "post")]
        HttpRequestData requestData, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Webhook received from external system");

        var body = await requestData.ReadAsStringAsync();
        var ticket = JsonSerializer.Deserialize<ExternalTicket>(body,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (ticket == null)
        {
            var badReq = requestData.CreateResponse(HttpStatusCode.BadRequest);
            await badReq.WriteStringAsync("Invalid payload");
            return badReq;
        }

        var connectionString = _configuration.GetConnectionString("DataverseConnectionString");
        using var client = new ServiceClient(connectionString);

        var caseToCreate = new EGC.sdp_case2
        {
            sdp_casenumber = ticket.ExternalId,
            sdp_Name = ticket.Subject,
            sdp_Description = ticket.Description,
            sdp_Origin = EGC.sdp_case2_sdp_Origin.origin,
            sdp_Priority = MapPriority(ticket.Priority),
            sdp_slaruleid = string.IsNullOrWhiteSpace(ticket.SlaRuleName) ? null 
                : (await RetrieveSlaRule(client, ticket))?.ToEntityReference()
        };


        Guid caseId = await client.CreateAsync(caseToCreate, cancellationToken);

        _logger.LogInformation($"Created case {caseId} from external ticket {ticket.ExternalId}");
        var response = requestData.CreateResponse(HttpStatusCode.OK);

        await response.WriteStringAsync(JsonSerializer.Serialize(new { caseId, status = "created" }));
        return response;
    }


    private async Task<EGC.sdp_slarule?> RetrieveSlaRule(ServiceClient client, ExternalTicket ticket)
        => (await client.RetrieveMultipleAsync(new QueryExpression
        {
            EntityName = EGC.sdp_slarule.EntityLogicalName,
            ColumnSet = new ColumnSet(false),
            Criteria =
            {
                Conditions =
                {
                    new ConditionExpression(EGC.sdp_slarule.Fields.sdp_rulename, ConditionOperator.Equal, ticket.SlaRuleName)
                }
            }
        })).Entities?.FirstOrDefault()?.ToEntity<EGC.sdp_slarule>();


    private sdp_case2_sdp_Priority? MapPriority(string? priority)
        => priority switch
        {
            "High" => sdp_case2_sdp_Priority.High,
            "Medium" => sdp_case2_sdp_Priority.Medium,
            "Low" => sdp_case2_sdp_Priority.Low,
            _ => null
        };
}