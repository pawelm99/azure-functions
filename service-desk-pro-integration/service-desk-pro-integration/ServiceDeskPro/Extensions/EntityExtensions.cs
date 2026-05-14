using Microsoft.Xrm.Sdk;

namespace ServiceDeskPro_integration.Extensions
{
    public static class EntityExtensions
    {
        public static T GetAttributeValueFromAliasedValue<T>(this Entity entity, string key)
        {
            AliasedValue aliasedValue = entity.GetAttributeValue<AliasedValue>(key);
            if (aliasedValue?.Value == null)
            {
                return default;
            }
            return (T)aliasedValue?.Value;
        }
    }
}
