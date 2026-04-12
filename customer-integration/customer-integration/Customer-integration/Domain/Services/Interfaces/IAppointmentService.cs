using customer_integration.Application.Models;
using customer_integration.Infrastructure.CRM.Models;

namespace customer_integration.Domain.Services.Interfaces
{
    public interface IAppointmentService
    {
        Task<Guid> CreateAppointmentAsync(CreateAppointmentModel createAppointment, CancellationToken cancellationToken);
        Task<dev_Appointment> GetAppointmentAsync(Guid appointmentId, CancellationToken cancellationToken);
    }
}
