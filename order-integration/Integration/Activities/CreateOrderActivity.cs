using Integration.Application.Models;
using Integration.Domain.Services.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Integration.Activities
{
    public class CreateOrderActivity
    {
        public readonly IOrderService _orderService;
        public readonly ILogger _logger;


        public CreateOrderActivity(IOrderService orderService, ILoggerFactory loggerFactory)
        {
            _orderService = orderService;
            _logger = loggerFactory.CreateLogger<CreateOrderActivity>();
        }


        [Function(nameof(CreateOrderActivity))]
        public async Task<Guid> Run([ActivityTrigger] List<Invoice> invoices, CancellationToken cancellationToken = default)
        {
            Guid orderId = await _orderService.CreateOrderAsync(invoices, cancellationToken);
            _logger.LogInformation($"Created order with id: {orderId}");
            return orderId;
        }
    }
}
