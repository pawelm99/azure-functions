using Integration.Application.Models;
using Integration.Domain.Services;
using Integration.Infrastructure.CRM.Invoice;
using Moq;

namespace Integration.Test.Services
{
    public class InvoiceServiceTests
    {
        private readonly Mock<IInvoiceCrmClient> _crmMock = new();
        private readonly InvoiceService _service;

        public InvoiceServiceTests()
        {
            _service = new InvoiceService(_crmMock.Object);
        }


        [Fact]
        public async Task Should_Create_Invoice()
        {
            // Arrange
            var invoice = new Invoice
            {
                Id = Guid.NewGuid(),
                InvoiceId = "INV-001",
                Customer = new Customer { Id = Guid.NewGuid(), Name = "Test Customer" },
                Vendor = new Vendor { Id = Guid.NewGuid(), Name = "Test Vendor" },
                InvoiceDate = DateTimeOffset.UtcNow,
                PaymentTerm = "Net 30"
            };

            _crmMock
                .Setup(x => x.CreateInvoiceAsync(invoice, It.IsAny<CancellationToken>()))
                .ReturnsAsync(Guid.NewGuid());

            // Act
            var result = await _service.CreateInvoiceAsync(invoice);

            // Assert
            Assert.NotEqual(Guid.Empty, result);
        }
    }
}
