using Aspects.SourceGeneration.Common;
using System.Reflection;
using System.Text;

namespace Aspects.Test.HashCode
{
    internal static class HashCode
    {
        public static string Comparer(Type containingType, string memberName, bool nullSafe)
            => new HashCodeCodeInfo(memberName).ComparerHashCode(GetComparer(containingType, memberName), nullSafe);

        public static string DeepCombined(string memberName, bool nullSafe)
            => new HashCodeCodeInfo(memberName).DeepCombinedHashCode(nullSafe);

        public static string Combined(string memberName, bool nullSafe)
            => new HashCodeCodeInfo(memberName).CombinedHashCode(nullSafe);

        public static string CombineMethodBody(Type type, bool includeBase = false, bool storehashCode = false, params string[] memberHashs)
        {
            var typeName = type.FullName;

            var sb = new StringBuilder($"{{ ");
            if (!storehashCode)
                sb.Append("return ");
            else sb.Append("#i ??= ");

            sb.Append($"System.HashCode.Combine(\"{typeName}\"");
            if (includeBase || memberHashs.Length > 0)
                sb.Append(',');

            if (includeBase)
            {
                sb.Append("base.GetHashCode()");
                if (memberHashs.Length > 0)
                    sb.Append(", ");
            }

            sb.Append(string.Join(", ", memberHashs));
            sb.Append("); ");

            if (storehashCode)
                sb.Append("return #i.Value; ");

            sb.Append('}');
            return sb.ToString();
        }

        public static string SequencialMethodBody(Type type, bool includeBase = false, bool storeHashCode = false, params string[] memberHashs)
        {
            var typeName = type.FullName;

            var sb = new StringBuilder("{ ");
            if (storeHashCode)
            {
                sb.AppendLine("if(#i.HasValue)");
                sb.AppendLine("return #i.Value;");
            }
            sb.AppendLine("var #i = new System.HashCode();");
            sb.AppendLine($"#i.Add(\"{typeName}\");");

            if (includeBase)
                sb.AppendLine("#i.Add(base.GetHashCode());");

            foreach (var memberHash in memberHashs)
                sb.AppendLine($"#i.Add({memberHash});");

            if (!storeHashCode)
                sb.Append("return #i.ToHashCode(); }");
            else
            {
                sb.AppendLine("#i = #i.ToHashCode();");
                sb.AppendLine("return #i.Value; }");
            }
            return sb.ToString();
        }

        private static string GetComparer(Type type, string member)
        {
            var comparerName = type.GetMember(member, BindingFlags.Instance | BindingFlags.Public)[0].GetCustomAttributesData()
                .First(attData => attData.ConstructorArguments.Any(ca => ca.ArgumentType == typeof(Type)))!.ConstructorArguments
                .First(ca => ca.ArgumentType == typeof(Type)).ToString();

            if (comparerName.StartsWith("typeof("))
                comparerName = comparerName[7..];
            comparerName = comparerName.TrimEnd(')');

            return comparerName.Replace('+', '.');
        }
    }
}
