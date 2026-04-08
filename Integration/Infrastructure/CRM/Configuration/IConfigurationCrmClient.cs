using EGC =  Integration.Infrastructure.CRM.Models;

namespace Integration.Infrastructure.CRM.Configuration
{
    public interface IConfigurationCrmClient
    {
        Task<EGC.dev_Configuration?> RetrieveConfigurationAsync(CancellationToken cancellationToken = default);
        Task UpdateOrderNumber(EGC.dev_Configuration configuration, CancellationToken cancellationToken = default);
    }
}
