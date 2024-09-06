using Aspects.SourceGenerators;
using CompileUnits.CSharp;
using System.Text;

namespace Aspects.Test.HashCode
{
    internal class HashCodeMethod
    {
        public static IMethod FromType<T>()
        {
            var cu = CodeAnalysis.CompileUnit.FromGeneratedCode<HashCodeSourceGenerator, T>();

            return cu.AllChildren()
                .OfType<IMethod>()
                .Single(IsGetHashCodeMethod);
        }

        private static bool IsGetHashCodeMethod(IMethod m)
        {
            return m.Name == nameof(GetHashCode)
                && m.Parameters.Count == 0;
        }

        public static string CombinedHashCode(string memberName)
            => $"Aspects.Collections.Enumerable.CombinedHashCode({memberName})";

        public static string DeepCombinedHashCode(string memberName)
            => $"Aspects.Collections.Enumerable.DeepCombinedHashCode({memberName})";

        public static string HashCodeCombine(string typeName, bool includeBase = false, bool storehashCode = false, params string[] memberHashs)
        {
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

        public static string HashCodeHash(string typeName, bool includeBase = false, bool storeHashCode = false, params string[] memberHashs)
        {
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

            foreach(var member in memberHashs)
                sb.AppendLine(member);

            if(!storeHashCode)
                sb.Append("return #i.ToHashCode(); }");
            else
            {
                sb.AppendLine("#i = #i.ToHashCode();");
                sb.AppendLine("return #i; }");
            }
            return sb.ToString();
        }
    }
}
