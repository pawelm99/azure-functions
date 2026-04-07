using Integration.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureAppConfiguration((context, config) =>
    {
        var directory = AppDomain.CurrentDomain.BaseDirectory;
        config.SetBasePath(directory)
              .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
              .AddEnvironmentVariables();
    })
    .ConfigureServices((context, services) =>
    {
        services.AddIntegrationServices(context.Configuration);
    })
    .Build();

host.Run();



