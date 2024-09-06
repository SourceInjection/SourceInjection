using Aspects.Attributes;
using Aspects.Attributes.Interfaces;
using Aspects.SourceGenerators.Base;
using Aspects.SourceGenerators.Base.DataMembers;
using Aspects.SourceGenerators.Common;
using Aspects.Util;
using Aspects.Util.SymbolExtensions;
using Microsoft.CodeAnalysis;
using System.Linq;
using System.Text;
using TypeInfo = Aspects.SourceGenerators.Common.TypeInfo;

namespace Aspects.SourceGenerators
{
    [Generator]
    internal class HashCodeSourceGenerator : ObjectMethodSourceGeneratorBase<IAutoHashCodeAttribute, IHashCodeAttribute, IHashCodeExcludeAttribute>
    {
        private const string StoredHashCode = "_storedHashCode";

        protected internal override string Name { get; } = nameof(GetHashCode);

        protected override DataMemberPriority Priority { get; } = DataMemberPriority.Field;

        protected override IAutoHashCodeAttribute DefaultConfigAttribute => new AutoHashCodeAttribute();

        protected override IHashCodeAttribute DefaultMemberConfigAttribute => new HashCodeAttribute();

        protected override string TypeBody(TypeInfo typeInfo)
        {
            const int hashCodeCombineMaxArgs = 8;

            var config = GetConfigAttribute(typeInfo);
            var includeBase = ShouldIncludeBase(typeInfo, config);
            var symbols = GetSymbols(typeInfo, typeInfo.Symbol.GetMembers(), config.DataMemberKind)
                .ToArray();
            var length = symbols.Length + 1 + (includeBase ? 1 : 0);
            var name = typeInfo.Symbol.ToDisplayString();

            var sb = new StringBuilder();

            if(config.StoreHashCode)
            {
                sb.AppendLine($"private int? {StoredHashCode};");
                sb.AppendLine();
            }

            sb.AppendLine($"public override int {nameof(GetHashCode)}()");
            sb.AppendLine("{");

            if (length <= hashCodeCombineMaxArgs)
                sb.AppendLine(HashCodeCombine(name, symbols, includeBase, typeInfo.HasNullableEnabled, config.StoreHashCode));
            else sb.AppendLine(HashCodeAppend(name, symbols, includeBase, typeInfo.HasNullableEnabled, config.StoreHashCode));

            sb.Append('}');
            return sb.ToString();
        }

        private string HashCodeAppend(string name, DataMemberSymbolInfo[] symbols, bool includeBase, bool nullableEnabled, bool storeHashCode)
        {
            var sb = new StringBuilder();
            if (storeHashCode)
            {
                sb.AppendLine(Code.Indent($"if({StoredHashCode}.HasValue)"));
                sb.AppendLine(Code.Indent($"return {StoredHashCode}.Value;", 2));
            }
            sb.AppendLine(Code.Indent("var hash = new System.HashCode();"));
            sb.AppendLine(Code.Indent($"hash.Add(\"{name}\");"));

            if (includeBase)
                sb.AppendLine(Code.Indent($"hash.Add(base.{nameof(GetHashCode)}());"));

            for (int i = 0; i < symbols.Length; i++)
                sb.AppendLine(Code.Indent($"hash.Add({MemberHash(symbols[i], nullableEnabled)});"));

            if(!storeHashCode)
                sb.Append(Code.Indent("return hash.ToHashCode();"));
            else
            {
                sb.AppendLine(Code.Indent($"{StoredHashCode} = hash.ToHashCode();"));
                sb.Append(Code.Indent($"return {StoredHashCode}.Value;"));
            }
            return sb.ToString();
        }

        private string HashCodeCombine(string name, DataMemberSymbolInfo[] symbols, bool includeBase, bool nullableEnabled, bool storeHashCode)
        {
            const int tabs = 2;

            var sb = new StringBuilder();
            if (!storeHashCode)
                sb.Append("return");
            else sb.Append($"{StoredHashCode} ??=");

            sb.Append(Code.Indent(" System.HashCode.Combine("));

            sb.AppendLine();
            sb.Append(Code.Indent($"\"{name}\"", tabs));
            if (includeBase || symbols.Length > 0)
                sb.AppendLine(",");

            if (includeBase)
            {
                sb.Append(Code.Indent($"base.{nameof(GetHashCode)}()", tabs));
                if (symbols.Length > 0)
                    sb.Append(',');
            }

            if (symbols.Length > 0)
            {
                sb.AppendLine();
                sb.Append(Code.Indent($"{MemberHash(symbols[0], nullableEnabled)}", tabs));

                for (var i = 1; i < symbols.Length; i++)
                {
                    sb.AppendLine(",");
                    sb.Append(Code.Indent($"{MemberHash(symbols[i], nullableEnabled)}", tabs));
                }
            }
            sb.Append(");");
            if (storeHashCode)
            {
                sb.AppendLine();
                sb.Append($"return {StoredHashCode}.Value;");
            }
            return sb.ToString();
        }

        private string MemberHash(DataMemberSymbolInfo symbolInfo, bool nullableEnabled)
        {
            var memberConfig = GetEqualityConfigAttribute(symbolInfo);
            var nullSafe = HasNullSafeConfig(symbolInfo, memberConfig, nullableEnabled);

            return Code.GetHashCode(symbolInfo.Type, symbolInfo.Name, nullSafe, memberConfig.EqualityComparer);
        }

        private static bool HasNullSafeConfig(DataMemberSymbolInfo symbol, IEqualityComparerAttribute memberConfig, bool nullableEnabled)
        {
            var nullSafe = GetNullSafety(symbol, memberConfig);
            if (nullSafe == NullSafety.Off)
                return false;
            if (nullSafe == NullSafety.On)
                return symbol.Type.IsReferenceType;

            return !nullableEnabled || symbol.Type.HasNullableAnnotation();
        }

        private static NullSafety GetNullSafety(DataMemberSymbolInfo symbol, IEqualityComparerAttribute memberConfig)
        {
            if (memberConfig.NullSafety != NullSafety.Auto)
                return memberConfig.NullSafety;
            if (symbol.HasNotNullAttribute())
                return NullSafety.Off;
            if (symbol.HasMaybeNullAttribute())
                return NullSafety.On;
            return NullSafety.Auto;
        }

        private IEqualityComparerAttribute GetEqualityConfigAttribute(DataMemberSymbolInfo symbolInfo)
        {
            var attribute = symbolInfo.AttributesOfType<EqualityComparerAttribute>()
                .FirstOrDefault();

            if (attribute != null && AttributeFactory.TryCreate<EqualityComparerAttribute>(attribute, out var config))
                return config;
            return GetMemberConfigAttribute(symbolInfo);
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
    }
}
