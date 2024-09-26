using Aspects.SourceGeneration.SnippetsHelper;
using System.Text;

namespace Aspects.Test.Equals
{
    internal static class Equalization
    {
        public static string Operator(string memberName)
            => EqualizationSnippets.OperatorEquality(memberName, $"#i.{memberName}", false);

        public static string Equals(string memberName, bool nullSafe)
            => EqualizationSnippets.MethodEquality(memberName, $"#i.{memberName}", nullSafe, false);

        public static string LinqCollection(string memberName, bool nullSafe)
            => EqualizationSnippets.LinqSequenceEquality(memberName, $"#i.{memberName}", nullSafe, false);

        public static string AspectsCollection(string memberName, bool nullSafe)
            => EqualizationSnippets.AspectsSequenceEquality(memberName, $"#i.{memberName}", nullSafe, false);

        public static string AspectsArray(string memberName, bool nullSafe)
            => EqualizationSnippets.AspectsArrayEquality(memberName, $"#i.{memberName}", nullSafe, false);

        public static string Comparer(Type containingType, string memberName, bool nullSafe)
            => EqualizationSnippets.ComparerEquality(
                EqualityComparer.FromMember(containingType, memberName), memberName, $"#i.{memberName}", nullSafe, false);

        public static string ComparerNullableNonReferenceType(Type containingType, string memberName, bool nullSafe)
            => EqualizationSnippets.ComparerNullableNonReferenceTypeEquality(
                EqualityComparer.FromMember(containingType, memberName), memberName, $"#i.{memberName}", nullSafe, false);

        public static Action Build(Type type, string propertyName)
        {
            return () =>
            {
                var lhs = Activator.CreateInstance(type);
                var rhs = Activator.CreateInstance(type);

                var prop = type.GetProperty(propertyName);

                if (lhs is null || rhs is null || prop is null)
                    throw new TypeLoadException($"could not get necessary information.");

                prop.SetValue(lhs, null);
                prop.SetValue(rhs, null);

                lhs.Equals(rhs);
            };
        }

        public static string Body(Type type, bool includeBase = false, params string[] memberEquals)
        {
            var sb = new StringBuilder("{ return ");
            if (type.IsClass)
                sb.Append("#i == this || ");
            sb.Append("#i != null ");

            if (includeBase && type.IsClass)
                sb.Append("&& base.Equals(#i) ");

            foreach (var member in memberEquals)
            {
                if (member.Contains("||"))
                    sb.Append($"&& ({member}) ");
                else sb.Append($"&& {member} ");
            }
            sb.Append("; }");
            return sb.ToString();
        }
    }
}
