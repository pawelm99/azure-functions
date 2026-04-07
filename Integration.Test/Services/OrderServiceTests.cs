using Integration.Application.Models;
using Integration.Domain.Services;
using Integration.Infrastructure.CRM.Order;
using Moq;

namespace Integration.Test.Services
{
    public class OrderServiceTests
    {
        private readonly Mock<IOrderCrmClient> _crmMock = new();
        private readonly OrderService _service;

        public OrderServiceTests()
        {
            _service = new OrderService(_crmMock.Object);
        }


        [Fact]
        public async Task Should_Create_Order()
        {
            // Arrange
            var invoices = new List<Invoice>
            {
                new Invoice
                {
                    Id = Guid.NewGuid(),
                    InvoiceId = "INV-001",
                    Customer = new Customer { Id = Guid.NewGuid(), Name = "Test Customer" },
                    Vendor = new Vendor { Id = Guid.NewGuid(), Name = "Test Vendor" }
                }
            };

            _crmMock
                .Setup(x => x.CreateOrderAsync(invoices, It.IsAny<CancellationToken>()))
                .ReturnsAsync(Guid.NewGuid());

            // Act
            var result = await _service.CreateOrderAsync(invoices);

            // Assert
            Assert.NotEqual(Guid.Empty, result);
        }
    }
}
