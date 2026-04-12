using customer_integration.Domain.Services.Interfaces;
using customer_integration.Infrastructure.CRM.Apartment;
using customer_integration.Infrastructure.CRM.Models;

namespace customer_integration.Domain.Services
{
    public class ApartmentService : IApartmentService
    {
        public readonly IApartmentCrmClient _apartmentClient;


        public ApartmentService(IApartmentCrmClient apartmentClient)
        {
            _apartmentClient = apartmentClient;
        }


        public async Task<dev_Apartment?> GetApartmentByNameAsync(string name, CancellationToken cancellationToken = default)
          => await _apartmentClient.GetApartmentByNameAsync(name, cancellationToken);
    }
}
