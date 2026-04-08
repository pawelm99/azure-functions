using Integration.Extensions;
using Integration.Providers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureAppConfiguration((context, config) =>
    {
        var directory = AppDomain.CurrentDomain.BaseDirectory;
        var configuration = config.SetBasePath(directory)
              .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
              .AddEnvironmentVariables()
              .AddAzureKeyVault(Environment.GetEnvironmentVariable("AzureKeyVault")!)
              .Build();
    })
    .ConfigureServices((context, services) =>
    {
        services.AddIntegrationServices(context.Configuration);
    })
   .ConfigureLogging((context, logging) =>
   {
       var appInsightsConnectionString = context.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]!;
       var azureWebJobsStorage = context.Configuration["AzureWebJobsStorage"]!;

           logging.AddApplicationInsights(
              configureTelemetryConfiguration: config =>
                  config.ConnectionString = appInsightsConnectionString,
              configureApplicationInsightsLoggerOptions: _ => { });
       logging.AddProvider(new BlobLoggerProvider(
           azureWebJobsStorage,
           "function-logs"));
   })
    .Build();

host.Run();



