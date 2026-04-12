using customer_integration.Infrastructure.CRM.Models;

namespace customer_integration.Domain.Services.Interfaces
{
    public interface IApartmentService
    {
        Task<dev_Apartment?> GetApartmentByNameAsync(string name, CancellationToken cancellationToken);
    }
}
