using Azure.Messaging.EventGrid;
using Azure.Messaging.ServiceBus;
using Data.Context;
using Integration.Configuration;
using Integration.Domain.Services;
using Integration.Domain.Services.Interfaces;
using Integration.Infrastructure.CRM;
using Integration.Infrastructure.CRM.Account;
using Integration.Infrastructure.CRM.Configuration;
using Integration.Infrastructure.CRM.Currency;
using Integration.Infrastructure.CRM.Invoice;
using Integration.Infrastructure.CRM.Order;
using Integration.Infrastructure.CRM.Product;
using Integration.Infrastructure.SQL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Integration.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIntegrationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddDatabase(config);
            services.AddServiceBus(config);
            services.AddEventGrid(config);
            services.AddDomainServices();
            services.AddCrmClients();
            services.AddRepositories();
            services.AddDataverseServiceClient();
            services.AddOrchestratorOptions(config);

            return services;
        }

        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration config)
        {
            var connectionString = config.GetConnectionString("AzIntegrationDB");
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
            return services;
        }

        public static IServiceCollection AddServiceBus(this IServiceCollection services, IConfiguration config)
        {
            var connectionString = config.GetConnectionString("ServiceBusConnection");
            services.AddSingleton(new ServiceBusClient(connectionString));
            return services;
        }

        public static IServiceCollection AddEventGrid(this IServiceCollection services, IConfiguration config)
        {
            var topicEndpoint = config["Values:EventGridTopicEndpoint"]!;
            var topicKey = config["Values:EventGridTopicKey"]!;
            services.AddSingleton(new EventGridPublisherClient(new Uri(topicEndpoint), new Azure.AzureKeyCredential(topicKey)));
            return services;
        }

        public static IServiceCollection AddDomainServices(this IServiceCollection services)
        {
            services.AddScoped<IDocumentService, DocumentService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IInvoiceService, InvoiceService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICurrencyService, CurrencyService>();
            services.AddScoped<IConfigurationService, ConfigurationService>();
            return services;
        }

        public static IServiceCollection AddCrmClients(this IServiceCollection services)
        {
            services.AddScoped<IAccountCrmClient, AccountCrmClient>();
            services.AddScoped<IInvoiceCrmClient, InvoiceCrmClient>();
            services.AddScoped<IOrderCrmClient, OrderCrmClient>();
            services.AddScoped<IProductCrmClient, ProductCrmClient>();
            services.AddScoped<ICurrencyCrmClient, CurrencyCrmClient>();
            services.AddScoped<IConfigurationCrmClient, ConfigurationCrmClient>();
            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IProductRepository, SqlProductRepository>();
            return services;
        }

        public static IServiceCollection AddOrchestratorOptions(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<OrchestratorRetryOptions>(config.GetSection("OrchestratorRetry"));
            services.AddSingleton<ITaskOptionsProvider, TaskOptionsProvider>();
            return services;
        }

        public static IServiceCollection AddDataverseServiceClient(this IServiceCollection services)
            => services
            .AddSingleton<ServiceClientWrapper>()
            .AddScoped(provider =>
            {
                var logger = provider.GetRequiredService<ILogger<Program>>();
                var wrapper = provider.GetRequiredService<ServiceClientWrapper>();
                logger.LogInformation("Cloned client at {time}.", DateTimeOffset.UtcNow);

                return wrapper.ServiceClient.Clone();
            });
    }
}
