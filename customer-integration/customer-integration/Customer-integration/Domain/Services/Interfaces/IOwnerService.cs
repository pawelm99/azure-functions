using customer_integration.Infrastructure.CRM.Models;

namespace customer_integration.Domain.Services.Interfaces
{
    public interface IOwnerService
    {
        Task<dev_owner?> GetOwnerAsync(Guid ownerId, CancellationToken cancellationToken);
    }
}
