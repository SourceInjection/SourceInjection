using SourceInjection.Interfaces;
using SourceInjection.SourceGeneration.Base;
using Microsoft.CodeAnalysis;
using System.Text;
using TypeInfo = SourceInjection.SourceGeneration.Common.TypeInfo;
using SourceInjection.SourceGeneration;
using System.Linq;
using System.Collections.Generic;
using SourceInjection.SourceGeneration.Common;
using SourceInjection.SourceGeneration.DataMembers;
using SourceInjection.Util;
using SourceInjection.CodeAnalysis;

#pragma warning disable IDE0130

namespace SourceInjection
{
    [Generator(LanguageNames.CSharp)]
    internal class SGToString
        : ObjectMethodSourceGeneratorBase<IAutoToStringAttribute, IToStringAttribute, IToStringExcludeAttribute>
    {
        protected internal override string Name { get; } = nameof(ToString);

        protected override DataMemberPriority Priority { get; } = DataMemberPriority.Property;

        protected override IAutoToStringAttribute DefaultConfigAttribute => new AutoToStringAttribute();

        protected override IToStringAttribute DefaultMemberConfigAttribute => new ToStringAttribute();

        protected override string TypeBody(TypeInfo typeInfo)
        {
            var config = GetConfigAttribute(typeInfo);

            var sb = new StringBuilder();
            sb.AppendLine($"public override string {nameof(ToString)}()");
            sb.AppendLine("{");

            sb.Append(Text.Indent($"return $\"({typeInfo.Name})"));

            var allowedAccessibilities = GetAllowedAccessibilities(config.Accessibility).ToArray();

            if(allowedAccessibilities.Length > 0)
            {
                var members = typeInfo.Members(true)
                    .Where(sy => SymbolIsAllowed(sy, allowedAccessibilities));

                var symbols = GetSymbols(typeInfo, members, config.DataMemberKind);

                if (symbols.Count > 0)
                {
                    sb.Append("{{");
                    sb.Append(MemberToString(symbols[0]));

                    for (int i = 1; i < symbols.Count; i++)
                        sb.Append($", {MemberToString(symbols[i])}");
                    sb.Append("}}");
                }
            }

            sb.AppendLine("\";");
            sb.Append('}');

            return sb.ToString();
        }

        private string MemberToString(DataMemberSymbolInfo member)
        {
            var config = GetMemberConfigAttribute(member);
            var formatProviderConfig = GetFormatProviderAttribute(member);

            return Snippets.MemberToString(member, config, formatProviderConfig);
        }

        private static FormatProviderAttribute GetFormatProviderAttribute(DataMemberSymbolInfo member)
        {
            var attData = member.AttributesOfType<FormatProviderAttribute>().FirstOrDefault();
            if (attData == null || !AttributeFactory.TryCreate<FormatProviderAttribute>(attData, out var fpc))
                return null;
            return fpc;
        }

        private static IEnumerable<AccessModifier> GetAllowedAccessibilities(Accessibilities accessibility)
        {
            if (accessibility.HasFlag(Accessibilities.Public))            yield return AccessModifier.Public;
            if (accessibility.HasFlag(Accessibilities.Internal))          yield return AccessModifier.Internal;
            if (accessibility.HasFlag(Accessibilities.Protected))         yield return AccessModifier.Protected;
            if (accessibility.HasFlag(Accessibilities.Private))           yield return AccessModifier.Private;
            if (accessibility.HasFlag(Accessibilities.ProtectedInternal)) yield return AccessModifier.ProtectedInternal;
            if (accessibility.HasFlag(Accessibilities.ProtectedPrivate))  yield return AccessModifier.ProtectedPrivate;
        }

        private static bool SymbolIsAllowed(ISymbol symbol, AccessModifier[] accessibilities)
        {
            ITypeSymbol type;
            if (symbol is IFieldSymbol field)
                type = field.Type;
            else if (symbol is IPropertySymbol property)
                type = property.Type;
            else return false;

            return !symbol.HasAttributeOfType<IToStringExcludeAttribute>() && (
                symbol.HasAttributeOfType<IToStringAttribute>() || (
                    HasRequiredAccessibility(symbol, accessibilities) && (!type.IsEnumerable() || type.OverridesToString())));
        }

        private static bool HasRequiredAccessibility(ISymbol symbol, AccessModifier[] accessibilities)
        {
            AccessModifier accessibility;
            if(symbol is IPropertySymbol property)
            {
                if (property.GetMethod == null)
                    return false;
                accessibility = MergePropertyAccessibility(property.DeclaredAccessibility.ToAccessModifier(), property.GetMethod.DeclaredAccessibility.ToAccessModifier());
            }
            else if (!(symbol is IFieldSymbol field && field.HasAttributeOfType<IGeneratesDataMemberPropertyFromFieldAttribute>()))
            {
                accessibility = symbol.DeclaredAccessibility.ToAccessModifier();
            }
            else
            {
                var attribute = AttributeFactory.Create<IGeneratesDataMemberPropertyFromFieldAttribute>(
                    field.AttributesOfType<IGeneratesDataMemberPropertyFromFieldAttribute>().First());
                accessibility = MergePropertyAccessibility(attribute.Modifier, attribute.GetterModifier);
            }

            if (accessibility == AccessModifier.None)
                accessibility = AccessModifier.Private;

            return accessibilities.Contains(accessibility);
        }

        private static AccessModifier MergePropertyAccessibility(AccessModifier declaredAccessibility, AccessModifier getterAccessibility)
        {
            if (getterAccessibility == AccessModifier.None)
                return declaredAccessibility;
            return getterAccessibility;
        }
    }
}

#pragma warning restore IDE0130