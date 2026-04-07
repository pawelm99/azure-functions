using Integration.Domain.Services.Interfaces;
using Integration.Infrastructure.CRM.Configuration;
using EGC = CrmEarlyBound;

namespace Integration.Domain.Services
{
    public class ConfigurationService : IConfigurationService
    {
        private readonly IConfigurationCrmClient _configurationClient;


        public ConfigurationService(IConfigurationCrmClient configurationClient)
        {
            _configurationClient = configurationClient;
        }


        public async Task<EGC.dev_Configuration?> RetrieveConfigurationAsync(CancellationToken cancellationToken = default)
            => await _configurationClient.RetrieveConfigurationAsync(cancellationToken);


        public async Task UpdateOrderNumber(EGC.dev_Configuration configuration, CancellationToken cancellationToken = default)
            => await _configurationClient.UpdateOrderNumber(configuration, cancellationToken);
    }
}
