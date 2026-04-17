using customer_integration.Domain.Services.Interfaces;
using customer_integration.Infrastructure.CRM.Models;
using Data.Context;
using Data.Domain;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace customer_integration.Reporting;

public class ReportFunction
{
    private readonly ILogger _logger;
    private readonly ICaseService _caseService;
    private readonly AppDbContext _appDbContext;


    public ReportFunction(ILoggerFactory loggerFactory, ICaseService caseService, AppDbContext appDbContext)
    {
        _logger = loggerFactory.CreateLogger<ReportFunction>();
        _caseService = caseService;
        _appDbContext = appDbContext;
    }


    [Function(nameof(ReportFunction))]
    public async Task Run([TimerTrigger("0 * * * * *")] TimerInfo myTimer, CancellationToken cancellationToken)
    {
        _logger.LogInformation("C# Timer trigger function executed at: {executionTime}", DateTime.Now);

        if (myTimer.ScheduleStatus is not null)
        {
            _logger.LogInformation("Next timer schedule at: {nextSchedule}", myTimer.ScheduleStatus.Next);
        }

        var cases = await _caseService.GetAllCaseAsync(cancellationToken);
        if (!cases.Any())
        {
            _logger.LogWarning("The report cannot be created because the cases do not exist.");
            return;
        }

        var total = cases.Count;
        var high = cases.Count(c => c.dev_priority == dev_Case_dev_priority.High);
        var closed = cases.Count(c => c.StateCode == dev_CaseState.Inactive);

        var crmReportToSave = new CrmReport(Guid.NewGuid(), DateTime.UtcNow, total, high, closed);
        await _appDbContext.AddAsync(crmReportToSave, cancellationToken);
        await _appDbContext.SaveChangesAsync(cancellationToken);
    }
}