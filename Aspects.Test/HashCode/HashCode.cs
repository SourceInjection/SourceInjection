using Aspects.SourceGeneration.SnippetsHelper;
using System.Text;

namespace Aspects.Test.HashCode
{
    internal static class HashCode
    {
        public static string Comparer(Type containingType, string memberName, bool nullSafe)
            => HashCodeSnippets.ComparerHashCode(memberName, EqualityComparer.FromMember(containingType, memberName), nullSafe);

        public static string ComparerNullableNonReferenceType(Type containingType, string memberName, bool nullSafe)
            => HashCodeSnippets.ComparerNullableNonReferenceTypeHashCode(memberName, EqualityComparer.FromMember(containingType, memberName), nullSafe);

        public static string DeepCombined(string memberName, bool nullSafe)
            => HashCodeSnippets.DeepCombinedHashCode(memberName, nullSafe);

        public static string Combined(string memberName, bool nullSafe)
            => HashCodeSnippets.CombinedHashCode(memberName, nullSafe);

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
    }
}
