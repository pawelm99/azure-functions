using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;
using Moq;

namespace Integration.Test.Helpers;

/// <summary>
/// Helper class to create mocked IOrganizationServiceAsync2 for testing.
/// Since ServiceClient cannot be instantiated without a real connection,
/// tests will need to use a different approach - see individual test files.
/// </summary>
public static class ServiceClientFactory
{
    /// <summary>
    /// Creates a mock of IOrganizationServiceAsync2 which is the interface that ServiceClient implements.
    /// Note: This cannot be used directly with services that depend on ServiceClient.
    /// Services would need to be refactored to accept IOrganizationServiceAsync2 instead.
    /// </summary>
    public static Mock<IOrganizationServiceAsync2> CreateMock()
    {
        return new Mock<IOrganizationServiceAsync2>();
    }
}
