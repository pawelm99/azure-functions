using customer_integration.Application.Models;
using customer_integration.Domain.Services.Interfaces;
using customer_integration.Infrastructure.CRM.Email;
using customer_integration.Infrastructure.CRM.Models;

namespace customer_integration.Domain.Services
{
    public class EmailService : IEmailService
    {
        public readonly IEmailCrmClient _emailCrmClient;


        public EmailService(IEmailCrmClient emailCrmClient)
        {
            _emailCrmClient = emailCrmClient;
        }


        public async Task<Guid> CreateEmailAsync(CreateEmailModel createEmailModel, CancellationToken cancellationToken)
            => await _emailCrmClient.CreateEmailAsync(createEmailModel, cancellationToken);


        public async Task<Template?> GetTemplateAsync(string templateName, CancellationToken cancellationToken)
            => await _emailCrmClient.GetTemplateAsync(templateName, cancellationToken);

    }
}
