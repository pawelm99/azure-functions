using Azure.Identity;
using Microsoft.Extensions.Configuration;

namespace Integration.Extensions
{
    public static class ConfigurationBuilderExtensions
    {
        public static IConfigurationBuilder AddAzureKeyVault(this IConfigurationBuilder builder, string keyVaultAddress)
            => builder.AddAzureKeyVault
            (
                new Uri(keyVaultAddress),
                new DefaultAzureCredential(new DefaultAzureCredentialOptions
                {
                    ExcludeEnvironmentCredential = true,
                    ExcludeInteractiveBrowserCredential = true,
                    ExcludeAzurePowerShellCredential = true,
                    ExcludeVisualStudioCredential = true,
                    ExcludeAzureCliCredential = false,
                    ExcludeManagedIdentityCredential = false,
                })
            );
    }
}
