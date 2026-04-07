using Integration.Application.Models;
using Integration.Domain.Services;
using Integration.Infrastructure.CRM.Account;
using Moq;

namespace Integration.Test.Services
{
    public class AccountServiceTests
    {
        private readonly Mock<IAccountCrmClient> _crmMock = new();
        private readonly AccountService _service;

        public AccountServiceTests()
        {
            _service = new AccountService(_crmMock.Object);
        }


        [Fact]
        public async Task Should_Create_Account()
        {
            // Arrange
            var vendor = new Vendor { Name = "Test Vendor" };

            _crmMock
                .Setup(x => x.CreateSellerAsync(vendor, It.IsAny<CancellationToken>()))
                .ReturnsAsync(Guid.NewGuid());

            // Act
            var result = await _service.CreateSellerAsync(vendor);

            // Assert
            Assert.NotNull(result);
        }
    }
}
