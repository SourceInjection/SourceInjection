using SourceInjection.CodeAnalysis;
using SourceInjection.Interfaces;
using SourceInjection.SourceGeneration.DataMembers;

namespace SourceInjection.SourceGeneration.SnippetsHelper
{
    internal static class ToStringSnippets
    {
        public static string MemberToString(DataMemberSymbolInfo member, IToStringAttribute config, FormatProviderAttribute formatProviderConfig)
        {
            var label = string.IsNullOrEmpty(config.Label) 
                ? member.Name 
                : config.Label;

            return $"{label}: {MemberValue(member, config, formatProviderConfig)}";
        }

        private static string MemberValue(DataMemberSymbolInfo member, IToStringAttribute config, FormatProviderAttribute formatProviderConfig)
        {
            if (!string.IsNullOrEmpty(formatProviderConfig?.Type))
            {
                var format = config.Format == null ? "null" : $"\"{config.Format}\"";
                var op = member.Type.IsReferenceType || member.Type.HasNullableAnnotation()
                    ? "?" 
                    : string.Empty;

                return $"{{{member.Name}{op}.ToString({format}, {FormatProvider(formatProviderConfig)})}}";
            }

            if (string.IsNullOrEmpty(config.Format))
                return $"{{{member.Name}}}";
            return $"{{{member.Name}:{config.Format}}}";
        }

        private static string FormatProvider(FormatProviderAttribute formatProviderAttribute)
        {
            if (string.IsNullOrEmpty(formatProviderAttribute.Property))
                return $"new {formatProviderAttribute.Type}()";

            return $"{formatProviderAttribute.Type}.{formatProviderAttribute.Property}";
        }
    }
}
