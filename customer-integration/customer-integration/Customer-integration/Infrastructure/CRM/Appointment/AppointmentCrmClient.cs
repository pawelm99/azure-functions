using customer_integration.Application.Models;
using customer_integration.Infrastructure.CRM.Models;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace customer_integration.Infrastructure.CRM.Appointment
{
    public class AppointmentCrmClient : IAppointmentCrmClient
    {
        private readonly ServiceClient _serviceClient;

        public AppointmentCrmClient(ServiceClient serviceClient)
        {
            _serviceClient = serviceClient;
        }


        public async Task<Guid> CreateAppointmentAsync(CreateAppointmentModel createAppointment, object cancellationToken)
            => await _serviceClient.CreateAsync(new dev_Appointment
            {
                dev_name = createAppointment.Name,
                dev_customer = new EntityReference(dev_Customer.EntityLogicalName, createAppointment.CustomerId),
                dev_apartment = new EntityReference(dev_Apartment.EntityLogicalName, createAppointment.Apartment),
                dev_date = createAppointment.Date,
                dev_notes = createAppointment.Notes
            });


        public async Task<dev_Appointment> GetAppointmentAsync(Guid appointmentId, CancellationToken cancellationToken)
             => (await _serviceClient.RetrieveAsync(dev_Appointment.EntityLogicalName, appointmentId,
                 new ColumnSet(dev_Appointment.Fields.dev_date), cancellationToken)).ToEntity<dev_Appointment>();
    }
}
