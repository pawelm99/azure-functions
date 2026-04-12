using customer_integration.Infrastructure.CRM.Models;

namespace customer_integration.Infrastructure.CRM.Owner
{
    public interface IOwnerCrmClient
    {
        Task<dev_owner?> GetOwnerAsync(Guid ownerId, CancellationToken cancellationToken);
    }
}
