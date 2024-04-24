using Aspects.Attributes.Interfaces;
using Aspects.SourceGenerators.Base;
using Aspects.SourceGenerators.Common;
using Aspects.Util;
using Microsoft.CodeAnalysis;
using System.Linq;
using System.Text;
using TypeInfo = Aspects.SourceGenerators.Common.TypeInfo;

namespace Aspects.SourceGenerators
{
    [Generator]
    public sealed class HashCodeSourceGenerator : BasicMethodOverrideSourceGeneratorBase<IHashCodeConfigAttribute, IHashCodeAttribute, IHashCodeExcludeAttribute>
    {
        private protected override string Name { get; } = nameof(GetHashCode);

        private protected override string ClassBody(TypeInfo typeInfo)
        {
            const int hashCodeCombineMaxArgs = 8;

            var sb = new StringBuilder();
            sb.AppendLine($"public override int {Name}()");
            sb.AppendLine("{");

            var symbols = GetRelevantSymbols(typeInfo).ToArray();
            var includeBase = ShouldIncludeBase(typeInfo);
            var length = symbols.Length + (includeBase ? 1 : 0);

            if (length <= hashCodeCombineMaxArgs)
                sb.AppendLine(HashCodeCombine(symbols, includeBase));
            else sb.AppendLine(HashCodeHash(symbols, includeBase));

            sb.Append('}');
            return sb.ToString();
        }

        private string HashCodeHash(ISymbol[] symbols, bool includeBase)
        {
            var sb = new StringBuilder();
            sb.AppendLine("\tvar hash = new System.HashCode();");

            if (includeBase)
                sb.AppendLine($"\thash.Add(base.{Name}());");

            for (int i = 0; i < symbols.Length; i++)
                sb.AppendLine($"\thash.Add({MemberHash(symbols[i])});");

            sb.Append("\treturn hash.ToHashCode();");
            return sb.ToString();
        }

        private string HashCodeCombine(ISymbol[] symbols, bool includeBase)
        {
            var sb = new StringBuilder();
            sb.Append("\treturn System.HashCode.Combine(");

            if (includeBase)
            {
                sb.AppendLine();
                sb.Append($"\t\tbase.{Name}()");
                if (symbols.Length > 0)
                    sb.Append(',');
            }

            if (symbols.Length > 0)
            {
                sb.AppendLine();
                sb.Append($"\t\t{MemberHash(symbols[0])}");

                for (var i = 1; i < symbols.Length; i++)
                {
                    sb.AppendLine(",");
                    sb.Append($"\t\t{MemberHash(symbols[i])}");
                }
            }
            sb.Append(");");
            return sb.ToString();
        }

        private string MemberHash(ISymbol symbol)
        {
            if (!MustUseCombinedHashCode(symbol))
                return symbol.Name;
            return SourceCode.CombinedHashCodeMethod(symbol.Name);
        }

        private bool ShouldIncludeBase(TypeInfo typeInfo)
        {
            return typeInfo.Symbol.HasAttributeOfType<IHashCodeConfigAttribute>()
                || typeInfo.Symbol.OverridesGetHashCode()
                || typeInfo.Symbol.Inheritance().SelectMany(cl => cl.GetMembers())
                    .Any(m => m.HasAttributeOfType<IHashCodeAttribute>());
        }

        private bool MustUseCombinedHashCode(ISymbol symbol)
        {
            ITypeSymbol type;
            if (symbol is IPropertySymbol p)
                type = p.Type;
            else if(symbol is IFieldSymbol f)
                type = f.Type;
            else return false;

            return type.IsEnumerable() && !type.OverridesGetHashCode();
        }
    }
}
