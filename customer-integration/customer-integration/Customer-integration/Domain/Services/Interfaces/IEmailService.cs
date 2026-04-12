using customer_integration.Application.Models;
using customer_integration.Infrastructure.CRM.Models;

namespace customer_integration.Domain.Services.Interfaces
{
    public interface IEmailService
    {
        Task<Guid> CreateEmailAsync(CreateEmailModel createEmailModel, CancellationToken cancellationToken);
        Task<Template?> GetTemplateAsync(string templateName, CancellationToken cancellationToken);
    }
}
