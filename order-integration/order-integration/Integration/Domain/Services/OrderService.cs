using Integration.Application.Models;
using Integration.Domain.Services.Interfaces;
using Integration.Infrastructure.CRM.Order;

namespace Integration.Domain.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderCrmClient _orderClient;


        public OrderService(IOrderCrmClient orderClient)
        {
            _orderClient = orderClient;
        }


        public async Task<Guid> CreateOrderAsync(List<Invoice> invoices, CancellationToken cancellationToken = default)
            => await _orderClient.CreateOrderAsync(invoices, cancellationToken);
    }
}
