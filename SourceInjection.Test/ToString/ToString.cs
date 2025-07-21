using System.Reflection;
using System.Text;

namespace SourceInjection.Test.ToString
{
    internal static class ToString
    {
        public static string Member(Type containingType, string memberName, string? label = null, string? format = null, bool coalesce = false)
        {
            label = string.IsNullOrEmpty(label)
                ? memberName
                : label;

            var member = containingType.GetMember(memberName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)[0];

            var attribute = member.GetCustomAttribute<FormatProviderAttribute>();
            var formatProvider = GetFormatProvider(attribute);

            return $"{label}: {MemberValue(memberName, format, formatProvider, coalesce)}";
        }

        private static string? GetFormatProvider(FormatProviderAttribute? formatProviderAttribute)
        {
            if (formatProviderAttribute == null)
                return null;

            if (string.IsNullOrEmpty(formatProviderAttribute.Property))
                return $"new {formatProviderAttribute.Type}()";

            return $"{formatProviderAttribute.Type}.{formatProviderAttribute.Property}";
        }

        private static string MemberValue(string memberName, string? format = null, string? formatProvider = null, bool coalesce = false)
        {
            if (!string.IsNullOrEmpty(formatProvider))
            {
                format = format == null ? "null" : $"\"{format}\"";
                var op = coalesce ? "?" : string.Empty;

                return $"{{{memberName}{op}.ToString({format}, {formatProvider})}}";
            }
            if (!string.IsNullOrEmpty(format))
                return $"{{{memberName}:{format}}}";
            return $"{{{memberName}}}";
        }
        
        public static string Body(Type type, params string[] memberToString)
        {
            var sb = new StringBuilder($"{{ return $\"({type.Name})");
            if(memberToString.Length > 0)
            {
                sb.Append("{{");
                sb.Append(memberToString[0]);

                for (int i = 1; i < memberToString.Length; i++)
                    sb.Append($", {memberToString[i]}");
                sb.Append("}}");
            }
            sb.Append("\"; }");
            return sb.ToString();
        }
    }
}
