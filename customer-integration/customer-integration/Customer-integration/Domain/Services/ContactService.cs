using customer_integration.Domain.Services.Interfaces;
using customer_integration.Infrastructure.CRM.Contact;
using customer_integration.Infrastructure.CRM.Models;

namespace customer_integration.Domain.Services
{
    public class ContactService : IContactService
    {
        public readonly IContactCrmClient _contactCrmClient;


        public ContactService(IContactCrmClient crmClient)
        {
            _contactCrmClient = crmClient;
        }



        public async Task<Contact?> GetContactByName(string name, CancellationToken cancellationToken)
            => await _contactCrmClient.GetContactByNameAsync(name, cancellationToken);
    }
}
