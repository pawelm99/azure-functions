using customer_integration.Application.Models;
using customer_integration.Domain.Services.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using EGC = customer_integration.Infrastructure.CRM.Models;

namespace customer_integration.Activities
{
    public class SendEmailActivity
    {
        private const string TemplateName = "Meeting Scheduled Regarding Apartment";


        private readonly IEmailService _emailService;
        private readonly IApartmentService _apartmentService;
        private readonly IAppointmentService _appointmentService;
        private readonly IOwnerService _ownerService;
        private readonly IContactService _contactService;


        public SendEmailActivity(IEmailService emailService, IApartmentService apartmentService, IAppointmentService appointmentService,
            IOwnerService ownerService, IContactService contactService)
        {
            _emailService = emailService;
            _apartmentService = apartmentService;
            _ownerService = ownerService;
            _appointmentService = appointmentService;
            _contactService = contactService;
        }


        [Function(nameof(SendEmailActivity))]
        public async Task<Guid?> Run([ActivityTrigger] CaseState caseState, FunctionContext executionContext,
            CancellationToken cancellationToken = default)
        {
            ILogger logger = executionContext.GetLogger(nameof(SendEmailActivity));

            EGC.dev_Apartment? apartment = await _apartmentService.GetApartmentByNameAsync(caseState.Apartment, cancellationToken);
            if (apartment == null || apartment.dev_appartment == null) return null;

            EGC.dev_Appointment appointment = await _appointmentService.GetAppointmentAsync(caseState.AppointmentId!.Value, cancellationToken);
            EGC.dev_owner? apartmentOwner = await _ownerService.GetOwnerAsync(apartment.dev_appartment.Id, cancellationToken);
            if (apartmentOwner == null) return null;

            EGC.Template? template = await _emailService.GetTemplateAsync(TemplateName.Replace(" ", ""), cancellationToken);
            if (template == null)
            {
                logger.LogWarning($"Email template with name {TemplateName} not found.");
                return null;
            }

            EGC.Contact? ownerContact = await _contactService.GetContactByName(apartmentOwner.dev_name, cancellationToken);
            if (ownerContact == null) return null;

            EGC.Contact? customerContact = await _contactService.GetContactByName(caseState.Customer, cancellationToken);
            if (customerContact == null) return null;

            var templateBody = template.Body.Replace("{Customer:Name}", customerContact.LastName)
                .Replace("{Owner:Name}", ownerContact.LastName)
                .Replace("{Meeting:Date}", appointment.dev_date.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss"))
                .Replace("{Apartment:Name}", apartment.dev_name);

            var createEmailModel = new CreateEmailModel(customerContact.Id, ownerContact.Id,
                TemplateName, templateBody);

            Guid emailId = await _emailService.CreateEmailAsync(createEmailModel, cancellationToken);
            logger.LogInformation($"Craete email with id: {emailId}.");
            return emailId;
        }
    }
}
