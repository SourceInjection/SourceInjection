using System.Text;

namespace Aspects.Test.HashCode
{
    internal class HashCodeMethod
    {
        public static string CombinedHashCode(string memberName)
            => $"Aspects.Collections.Enumerable.CombinedHashCode({memberName})";

        public static string DeepCombinedHashCode(string memberName)
            => $"Aspects.Collections.Enumerable.DeepCombinedHashCode({memberName})";

        public static string HashCodeCombine(bool includeBase = false, params string[] memberHashs)
        {
            var sb = new StringBuilder("return System.HashCode.Combine(");
            if (includeBase)
            {
                sb.Append("base.GetHashCode()");
                if (memberHashs.Length > 0)
                    sb.Append(", ");
            }

            sb.Append(string.Join(", ", memberHashs));
            sb.Append(");");

            return sb.ToString();
        }

        public static string HashCodeHash(bool includeBase = false, params string[] memberHashs)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"var #i = new System.HashCode();");

            if (includeBase)
                sb.AppendLine($"#i.Add(base.GetHashCode());");

            foreach(var member in memberHashs)
                sb.AppendLine(member);

            sb.Append($"return #i.ToHashCode();");

            return sb.ToString();
        }
    }
}
