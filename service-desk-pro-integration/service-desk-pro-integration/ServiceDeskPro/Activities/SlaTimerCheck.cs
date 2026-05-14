using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using ServiceDeskPro_integration.Extensions;
using EGC = ServiceDeskPro_integration.Infrastructure.CRM.Models;

namespace ServiceDeskPro_integration.Activities;

public class SlaTimerCheck
{
    private readonly ILogger _logger;
    private readonly IConfiguration _configuration;


    public SlaTimerCheck(ILoggerFactory loggerFactory, IConfiguration configuration)
    {
        _logger = loggerFactory.CreateLogger<SlaTimerCheck>();
        _configuration = configuration;
    }


    [Function("SlaTimerCheck")]
    public async Task Run([TimerTrigger("0 */30 * * * *")] TimerInfo myTimer, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("SLA check started at {executionTime}", DateTime.Now);
        var connectionString = _configuration.GetConnectionString("DataverseConnectionString");
        using var client = new ServiceClient(connectionString);

        if (myTimer.ScheduleStatus is not null)
        {
            _logger.LogInformation("Next timer schedule at: {nextSchedule}", myTimer.ScheduleStatus.Next);
        }

        List<EGC.sdp_case2>? cases = await RetrieveCases(client, cancellationToken);
        if (cases == null)
        {
            _logger.LogInformation("No cases found for SLA check.");
            return;
        }

        int breachedCount = 0;
        foreach (EGC.sdp_case2 @case in cases)
        {
            string? sla = @case.GetAttributeValueFromAliasedValue<string?>(EGC.sdp_slarule.EntityLogicalName
                + "." + EGC.sdp_slarule.Fields.sdp_resolutiontimehours);

            bool isParsed = int.TryParse(sla, out int slaHours);
            if (isParsed && slaHours > 0 && @case.sdp_openedon.HasValue && (DateTime.UtcNow - @case.sdp_openedon).Value.TotalHours > slaHours)
            {
                var update = new EGC.sdp_case2
                {
                    sdp_case2Id = @case.Id,
                    sdp_Slabreached = true
                };

                await client.UpdateAsync(update, cancellationToken);
                breachedCount++;
                _logger.LogInformation($"Marked case {@case.Id} as breached");
            }
        }
        _logger.LogInformation($"SLA check complete. Breached: {breachedCount}");
    }


    private async Task<List<EGC.sdp_case2>?> RetrieveCases(ServiceClient client, CancellationToken cancellationToken = default)
        => (await client.RetrieveMultipleAsync(new QueryExpression
        {
            EntityName = EGC.sdp_case2.EntityLogicalName,
            ColumnSet = new ColumnSet(EGC.sdp_case2.Fields.sdp_openedon),
            Criteria =
            {
                Conditions =
                {
                    new ConditionExpression(EGC.sdp_case2.Fields.sdp_Slabreached, ConditionOperator.Equal, false),
                    new ConditionExpression(EGC.sdp_case2.Fields.StatusCode, ConditionOperator.NotEqual, (int)EGC.sdp_case2_StatusCode.Resolved)
                }
            },
            LinkEntities =
            {
                new LinkEntity
                {
                    EntityAlias = EGC.sdp_slarule.EntityLogicalName,
                    JoinOperator = JoinOperator.Inner,
                    LinkFromEntityName = EGC.sdp_case2.EntityLogicalName,
                    LinkFromAttributeName = EGC.sdp_case2.Fields.sdp_slaruleid,
                    LinkToEntityName = EGC.sdp_slarule.EntityLogicalName,
                    LinkToAttributeName = EGC.sdp_slarule.Fields.sdp_slaruleId,
                    Columns = new ColumnSet(EGC.sdp_slarule.Fields.sdp_resolutiontimehours)
                }
            }
        })).Entities?.Select(x => x.ToEntity<EGC.sdp_case2>())?.ToList();
}