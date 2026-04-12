using Integration.Extensions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

var directory = AppDomain.CurrentDomain.BaseDirectory;
var configuration = builder.Configuration.SetBasePath(directory)
              .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
              .AddEnvironmentVariables()
              .Build();

builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .AddIntegrationServices(configuration)
    .ConfigureFunctionsApplicationInsights();

builder.Build().Run();
