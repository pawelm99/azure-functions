using Integration.Domain.Services;
using Integration.Infrastructure.CRM.Configuration;
using Moq;
using EGC = CrmEarlyBound;

namespace Integration.Test.Services
{
    public class ConfigurationServiceTests
    {
        private readonly Mock<IConfigurationCrmClient> _crmMock = new();
        private readonly ConfigurationService _service;

        public ConfigurationServiceTests()
        {
            _service = new ConfigurationService(_crmMock.Object);
        }


        [Fact]
        public async Task Should_Retrieve_Configuration()
        {
            // Arrange
            var expectedConfiguration = new EGC.dev_Configuration
            {
                Id = Guid.NewGuid(),
                dev_ordernumber = "00001"
            };

            _crmMock
                .Setup(x => x.RetrieveConfigurationAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedConfiguration);

            // Act
            var result = await _service.RetrieveConfigurationAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedConfiguration.Id, result.Id);
            Assert.Equal("00001", result.dev_ordernumber);
        }


        [Fact]
        public async Task Should_Update_Order_Number()
        {
            // Arrange
            var configuration = new EGC.dev_Configuration
            {
                Id = Guid.NewGuid(),
                dev_ordernumber = "00001"
            };

            _crmMock
                .Setup(x => x.UpdateOrderNumber(configuration, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await _service.UpdateOrderNumber(configuration);

            // Assert
            _crmMock.Verify(x => x.UpdateOrderNumber(configuration, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
