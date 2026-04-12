using customer_integration.Domain.Services.Interfaces;
using customer_integration.Infrastructure.CRM.Models;
using customer_integration.Infrastructure.CRM.Owner;

namespace customer_integration.Domain.Services
{
    public class OwnerService : IOwnerService
    {
        public readonly IOwnerCrmClient _ownerClient;


        public OwnerService(IOwnerCrmClient ownerClient)
        {
            _ownerClient = ownerClient;
        }


        public async Task<dev_owner?> GetOwnerAsync(Guid ownerId, CancellationToken cancellationToken)
            => await _ownerClient.GetOwnerAsync(ownerId, cancellationToken);
    }
}
