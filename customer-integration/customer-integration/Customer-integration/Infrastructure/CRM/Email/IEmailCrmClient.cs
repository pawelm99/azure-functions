using customer_integration.Application.Models;
using customer_integration.Infrastructure.CRM.Models;

namespace customer_integration.Infrastructure.CRM.Email
{
    public interface IEmailCrmClient
    {
        Task<Guid> CreateEmailAsync(CreateEmailModel createEmailModel, CancellationToken cancellationToken);
        Task<Template?> GetTemplateAsync(string templateName, CancellationToken cancellationToken);
    }
}
