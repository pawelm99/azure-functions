using customer_integration.Domain.Services;
using customer_integration.Domain.Services.Interfaces;
using customer_integration.Infrastructure.CRM;
using customer_integration.Infrastructure.CRM.Apartment;
using customer_integration.Infrastructure.CRM.Appointment;
using customer_integration.Infrastructure.CRM.Case;
using customer_integration.Infrastructure.CRM.Contact;
using customer_integration.Infrastructure.CRM.Email;
using customer_integration.Infrastructure.CRM.Owner;
using customer_integration.Infrastructure.CRM.SystemUser;
using customer_integration.Infrastructure.SQL.IssuePriorityRule;
using Data.Context;
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
            services.AddDomainServices();
            services.AddCrmClients();
            services.AddRepositories();
            services.AddDataverseServiceClient();

            return services;
        }


        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration config)
        {
            var connectionString = config.GetConnectionString("SqlConnectionString");
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
            return services;
        }
  

        public static IServiceCollection AddDomainServices(this IServiceCollection services)
        {
            services.AddScoped<IApartmentService, ApartmentService>();
            services.AddScoped<IAppointmentService, AppointmentService>();
            services.AddScoped<ICaseService, CaseService>();
            services.AddScoped<IContactService, ContactService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IIssuePriorityRuleService, IssuePriorityRuleService>();
            services.AddScoped<IOwnerService, OwnerService>();
            return services;
        }


        public static IServiceCollection AddCrmClients(this IServiceCollection services)
        {
            services.AddScoped<IApartmentCrmClient, ApartmentCrmClient>();
            services.AddScoped<IAppointmentCrmClient, AppointmentCrmClient>();
            services.AddScoped<ICaseCrmClient, CaseCrmClient>();
            services.AddScoped<IContactCrmClient, ContactCrmClient>();
            services.AddScoped<IEmailCrmClient, EmailCrmClient>();
            services.AddScoped<IOwnerCrmClient, OwnerCrmClient>();
            services.AddScoped<ISystemUserClient, SystemUserClient>();
            return services;
        }


        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IIssuePriorityRuleRepository, IssuePriorityRuleRepository>();
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
