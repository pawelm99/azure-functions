using customer_integration.Application.Models;
using customer_integration.Infrastructure.CRM.Models;

namespace customer_integration.Infrastructure.CRM.Appointment
{
    public interface IAppointmentCrmClient
    {
        Task<Guid> CreateAppointmentAsync(CreateAppointmentModel createAppointment, object cancellationToken);
        Task<dev_Appointment> GetAppointmentAsync(Guid appointmentId, CancellationToken cancellationToken);
    }
}
