using SourceInjection.CodeAnalysis;
using SourceInjection.Interfaces;
using SourceInjection.SourceGeneration.DataMembers;

namespace SourceInjection.SourceGeneration.SnippetsHelper
{
    internal static class ToStringSnippets
    {
        public static string MemberToString(DataMemberSymbolInfo member, IToStringAttribute config)
        {
            var label = string.IsNullOrEmpty(config.Label) 
                ? member.Name 
                : config.Label;

            return $"{label}: {MemberValue(member, config)}";
        }

        private static string MemberValue(DataMemberSymbolInfo member, IToStringAttribute config)
        {
            if (!string.IsNullOrEmpty(config.FormatProvider))
            {
                var format = config.Format == null ? "null" : $"\"{config.Format}\"";
                var op = member.Type.IsReferenceType || member.Type.HasNullableAnnotation()
                    ? "?" 
                    : string.Empty;

                return $"{{{member.Name}{op}.ToString({format}, new {config.FormatProvider}())}}";
            }

            if (string.IsNullOrEmpty(config.Format))
                return $"{{{member.Name}}}";
            return $"{{{member.Name}:{config.Format}}}";
        }
    }
}
