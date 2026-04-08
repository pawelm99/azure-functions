using Integration.Infrastructure.CRM.Models;

namespace Integration.Domain.Services.Interfaces
{
    public interface IConfigurationService
    {
        public Task<dev_Configuration?> RetrieveConfigurationAsync(CancellationToken cancellationToken = default);
        public Task UpdateOrderNumber(dev_Configuration configuration, CancellationToken cancellationToken = default);
    }
}
