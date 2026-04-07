using Integration.Application.Models;

namespace Integration.Domain.Services.Interfaces
{
    public interface IOrderService
    {
        Task<Guid> CreateOrderAsync(List<Invoice> invoices, CancellationToken cancellationToken = default);
    }
}
