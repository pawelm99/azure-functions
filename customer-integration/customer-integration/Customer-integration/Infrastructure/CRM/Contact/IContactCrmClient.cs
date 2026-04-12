namespace customer_integration.Infrastructure.CRM.Contact
{
    public interface IContactCrmClient
    {
        Task<Models.Contact?> GetContactByNameAsync(string name, CancellationToken cancellationToken);
    }
}
