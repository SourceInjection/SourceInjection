using Aspects.Attributes;
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
    internal class HashCodeSourceGenerator 
        : ObjectMethodSourceGeneratorBase<IAutoHashCodeAttribute, IHashCodeAttribute, IHashCodeExcludeAttribute>
    {
        protected internal override string Name { get; } = nameof(GetHashCode);

        protected override DataMemberPriority Priority { get; } = DataMemberPriority.Field;

        protected override DataMemberKind DataMemberKindFromAttribute(IAutoHashCodeAttribute attr)
        {
            return attr.DataMemberKind;
        }

        protected override string TypeBody(TypeInfo typeInfo)
        {
            const int hashCodeCombineMaxArgs = 8;
            var config = GetConfigAttribute(typeInfo);

            var sb = new StringBuilder();

            sb.AppendLine();
            sb.Append("//");
            sb.AppendLine(string.Join("\r\n//", typeInfo.SyntaxNode.GetLeadingTrivia().Select(t => t.ToFullString())));
            sb.AppendLine();

            sb.AppendLine($"public override int {Name}()");
            sb.AppendLine("{");

            var symbols = GetLocalTargetedSymbols(typeInfo).ToArray();
            var includeBase = ShouldIncludeBase(typeInfo, config);
            var length = symbols.Length + (includeBase ? 1 : 0);

            if (length <= hashCodeCombineMaxArgs)
                sb.AppendLine(HashCodeCombine(symbols, includeBase));
            else sb.AppendLine(HashCodeAppend(symbols, includeBase));

            sb.Append('}');
            return sb.ToString();
        }

        private string HashCodeAppend(ISymbol[] symbols, bool includeBase)
        {
            var sb = new StringBuilder();
            sb.AppendLine(Code.Indent("var hash = new System.HashCode();"));

            if (includeBase)
                sb.AppendLine(Code.Indent($"hash.Add(base.{Name}());"));

            for (int i = 0; i < symbols.Length; i++)
                sb.AppendLine(Code.Indent($"hash.Add({MemberHash(symbols[i])});"));

            sb.Append(Code.Indent("return hash.ToHashCode();"));
            return sb.ToString();
        }

        private string HashCodeCombine(ISymbol[] symbols, bool includeBase)
        {
            var sb = new StringBuilder();
            sb.Append(Code.Indent("return System.HashCode.Combine("));

            if (includeBase)
            {
                sb.AppendLine();
                sb.Append(Code.Indent($"base.{Name}()", 2));
                if (symbols.Length > 0)
                    sb.Append(',');
            }

            if (symbols.Length > 0)
            {
                sb.AppendLine();
                sb.Append(Code.Indent($"{MemberHash(symbols[0])}", 2));

                for (var i = 1; i < symbols.Length; i++)
                {
                    sb.AppendLine(",");
                    sb.Append(Code.Indent($"{MemberHash(symbols[i])}", 2));
                }
            }
            sb.Append(");");
            return sb.ToString();
        }

        private static string MemberHash(ISymbol symbol)
        {
            if (!MustUseCombinedHashCode(symbol))
                return symbol.Name;
            return $"{Paths.AspectsDeepCombinedHashCode}({symbol.Name})";
        }

        private static bool ShouldIncludeBase(TypeInfo typeInfo, IAutoHashCodeAttribute config)
        {
            return typeInfo.Symbol.IsReferenceType && ( 
                config.BaseCall == BaseCall.On || config.BaseCall == BaseCall.Auto 
                    && typeInfo.Symbol.BaseType is ITypeSymbol syBase && (
                        syBase.HasAttributeOfType<IAutoHashCodeAttribute>() 
                        || syBase.OverridesGetHashCode() 
                        || syBase.GetMembers().Any(m => m.HasAttributeOfType<IHashCodeAttribute>())));
        }

        private static bool MustUseCombinedHashCode(ISymbol symbol)
        {
            ITypeSymbol type;
            if (symbol is IPropertySymbol p)
                type = p.Type;
            else if(symbol is IFieldSymbol f)
                type = f.Type;
            else return false;

            return type.IsEnumerable() && !type.OverridesGetHashCode();
        }

        private static IAutoHashCodeAttribute GetConfigAttribute(TypeInfo typeInfo)
        {
            var attData = typeInfo.Symbol.AttributesOfType<IAutoHashCodeAttribute>().FirstOrDefault();
            if (attData is null || !AttributeFactory.TryCreate<IAutoHashCodeAttribute>(attData, out var config))
                return new AutoHashCodeAttribute();
            return config;
        }
    }
}
