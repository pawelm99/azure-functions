using customer_integration.Application.Models;
using customer_integration.Domain.Services.Interfaces;
using customer_integration.Infrastructure.CRM.Appointment;
using customer_integration.Infrastructure.CRM.Models;

namespace customer_integration.Domain.Services
{
    public class AppointmentService : IAppointmentService
    {
        public readonly IAppointmentCrmClient _apartmentClient;


        public AppointmentService(IAppointmentCrmClient apartmentClient)
        {
            _apartmentClient = apartmentClient;
        }


        public async Task<Guid> CreateAppointmentAsync(CreateAppointmentModel createAppointment, CancellationToken cancellationToken)
            => await _apartmentClient.CreateAppointmentAsync(createAppointment, cancellationToken);


        public async Task<dev_Appointment> GetAppointmentAsync(Guid appointmentId, CancellationToken cancellationToken)
            => await _apartmentClient.GetAppointmentAsync(appointmentId, cancellationToken);
    }
}
