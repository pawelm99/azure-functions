using customer_integration.Infrastructure.CRM.Models;

namespace customer_integration.Domain.Services.Interfaces
{
    public interface IContactService
    {
        Task<Contact?> GetContactByName(string dev_name, CancellationToken cancellationToken);
    }
}
