using customer_integration.Application.Models;
using customer_integration.Domain.Services.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using EGC = customer_integration.Infrastructure.CRM.Models;

namespace customer_integration.Activities
{
    public class CreateAppointmentActivity
    {
        private readonly IAppointmentService _appointmentService;
        private readonly ICaseService _caseService;


        public CreateAppointmentActivity(IAppointmentService appointmentService, ICaseService caseService)
        {
            _appointmentService = appointmentService;
            _caseService = caseService;
        }


        [Function(nameof(CreateAppointmentActivity))]
        public async Task<Guid> Run([ActivityTrigger] CaseState caseState, FunctionContext executionContext,
            CancellationToken cancellationToken = default)
        {
            ILogger logger = executionContext.GetLogger(nameof(CreateAppointmentActivity));

            EGC.dev_Case dev_Case = await _caseService.GetCaseAsync(caseState.CaseId!.Value);

            var createAppointment = new CreateAppointmentModel(dev_Case.dev_title, dev_Case.dev_customer.Id,
                dev_Case.dev_apartment.Id, DateTime.UtcNow.AddDays(4), dev_Case.dev_description);

            Guid apointmentGuid = await _appointmentService.CreateAppointmentAsync(createAppointment, cancellationToken);
            logger.LogInformation($"Create appointment with id: {apointmentGuid}.");

            return apointmentGuid;
        }
    }
}
