using Microsoft.CodeAnalysis;
using SourceInjection.CodeAnalysis;
using SourceInjection.Interfaces;
using SourceInjection.SourceGeneration.Common;
using SourceInjection.SourceGeneration.DataMembers;
using System;
using System.Linq;
using System.Reflection;

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
            if (!string.IsNullOrEmpty(formatProviderConfig?.Class))
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
            if (string.IsNullOrEmpty(formatProviderAttribute.Member))
                return $"new {formatProviderAttribute.Class}()";

            var members = TypeCollector.GetTypes(formatProviderAttribute.Class)
                .SelectMany(cl => cl.Members())
                .Where(m => m.Name == formatProviderAttribute.Member && m.IsStatic)
                .ToArray();

            if(members.Length > 0)
            {
                if (!Array.Exists(members, m => m.Kind == SymbolKind.Property) && Array.Exists(members, m => m.Kind == SymbolKind.Method))
                    return $"{formatProviderAttribute.Class}.{formatProviderAttribute.Member}()";
            }
            else if(Type.GetType(formatProviderAttribute.Class) is Type type)
            {
                var flags = BindingFlags.Static | BindingFlags.Public;
                var allMembers = type.GetMembers(flags);

                if (!Array.Exists(allMembers, m => m.MemberType == MemberTypes.Property) && Array.Exists(allMembers, m => m.MemberType == MemberTypes.Method))
                    return $"{formatProviderAttribute.Class}.{formatProviderAttribute.Member}()";
            }

            return $"{formatProviderAttribute.Class}.{formatProviderAttribute.Member}";
        }
    }
}
