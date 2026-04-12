
namespace customer_integration.Infrastructure.CRM.SystemUser
{
    public interface ISystemUserClient
    {
        Task<Models.SystemUser?> GetTechnicalUserAsync(CancellationToken cancellationToken);
    }
}
