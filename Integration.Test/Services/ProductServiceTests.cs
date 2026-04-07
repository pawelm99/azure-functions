using Data.Domain;
using Integration.Domain.Services;
using Integration.Infrastructure.CRM.Product;
using Moq;
using EGC = CrmEarlyBound;

namespace Integration.Test.Services
{
    public class ProductServiceTests
    {
        private readonly Mock<IProductCrmClient> _crmMock = new();
        private readonly ProductService _service;

        public ProductServiceTests()
        {
            _service = new ProductService(_crmMock.Object);
        }


        [Fact]
        public async Task Should_Create_Product()
        {
            // Arrange
            var product = new Product { Name = "Test Product", Price = 100.00, Quantity = 10, CurrencyCode = "USD" };

            _crmMock
                .Setup(x => x.CreateProductAsync(product, It.IsAny<CancellationToken>()))
                .ReturnsAsync(Guid.NewGuid());

            // Act
            var result = await _service.CreateProductAsync(product);

            // Assert
            Assert.NotEqual(Guid.Empty, result);
        }


        [Fact]
        public async Task Should_Retrieve_Product()
        {
            // Arrange
            var description = "Test Product";
            var expectedProduct = new EGC.dev_Produkt { Id = Guid.NewGuid() };

            _crmMock
                .Setup(x => x.RetrieveProductAsync(description, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedProduct);

            // Act
            var result = await _service.RetrieveProductAsync(description, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedProduct.Id, result.Id);
        }
    }
}
