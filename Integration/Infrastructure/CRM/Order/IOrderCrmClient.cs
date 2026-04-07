namespace Integration.Infrastructure.CRM.Order
{
    public interface IOrderCrmClient
    {
        Task<Guid> CreateOrderAsync(List<Application.Models.Invoice> invoices, CancellationToken cancellationToken = default);
    }
}
