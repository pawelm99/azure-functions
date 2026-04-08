using Microsoft.Extensions.Configuration;
using Microsoft.PowerPlatform.Dataverse.Client;

namespace Integration.Infrastructure.CRM
{
    public class ServiceClientWrapper
    {
        public readonly ServiceClient ServiceClient;


        public ServiceClientWrapper(IConfiguration configuration)
        {
            ServiceClient = new ServiceClient(configuration.GetConnectionString("DynamicConnectionString"));
        }
    }
}
