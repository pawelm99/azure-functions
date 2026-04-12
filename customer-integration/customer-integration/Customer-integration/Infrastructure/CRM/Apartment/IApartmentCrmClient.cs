using customer_integration.Infrastructure.CRM.Models;

namespace customer_integration.Infrastructure.CRM.Apartment
{
    public interface IApartmentCrmClient
    {
        Task<dev_Apartment?> GetApartmentByNameAsync(string name, CancellationToken cancellationToken);
    }
}