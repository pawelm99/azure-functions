using Integration.Domain.Services;
using Integration.Infrastructure.CRM.Currency;
using Moq;
using EGC = Integration.Infrastructure.CRM.Models;

namespace Integration.Test.Services
{
    public class CurrencyServiceTests
    {
        private readonly Mock<ICurrencyCrmClient> _crmMock = new();
        private readonly CurrencyService _service;

        public CurrencyServiceTests()
        {
            _service = new CurrencyService(_crmMock.Object);
        }


        [Fact]
        public async Task Should_Retrieve_Currency_By_Symbol()
        {
            // Arrange
            var code = "USD";
            var expectedCurrency = new EGC.TransactionCurrency { Id = Guid.NewGuid() };

            _crmMock
                .Setup(x => x.RetrieveTransactionCurrencyBySymbolAsync(code, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedCurrency);

            // Act
            var result = await _service.RetrieveTransactionCurrencyBySymbolAsync(code);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedCurrency.Id, result.Id);
        }


        [Fact]
        public async Task Should_Return_Null_When_Currency_Not_Found()
        {
            // Arrange
            var code = "INVALID";

            _crmMock
                .Setup(x => x.RetrieveTransactionCurrencyBySymbolAsync(code, It.IsAny<CancellationToken>()))
                .ReturnsAsync((EGC.TransactionCurrency?)null);

            // Act
            var result = await _service.RetrieveTransactionCurrencyBySymbolAsync(code);

            // Assert
            Assert.Null(result);
        }
    }
}
