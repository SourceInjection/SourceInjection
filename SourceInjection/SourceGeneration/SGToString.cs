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
            return Snippets.MemberToString(member, config);
        }

        private static IEnumerable<Microsoft.CodeAnalysis.Accessibility> GetAllowedAccessibilities(Accessibility accessibility)
        {
            if (accessibility.HasFlag(Accessibility.Public))            yield return Microsoft.CodeAnalysis.Accessibility.Public;
            if (accessibility.HasFlag(Accessibility.Internal))          yield return Microsoft.CodeAnalysis.Accessibility.Internal;
            if (accessibility.HasFlag(Accessibility.Protected))         yield return Microsoft.CodeAnalysis.Accessibility.Protected;
            if (accessibility.HasFlag(Accessibility.Private))           yield return Microsoft.CodeAnalysis.Accessibility.Private;
            if (accessibility.HasFlag(Accessibility.ProtectedInternal)) yield return Microsoft.CodeAnalysis.Accessibility.ProtectedOrInternal;
            if (accessibility.HasFlag(Accessibility.ProtectedPrivate))  yield return Microsoft.CodeAnalysis.Accessibility.ProtectedAndInternal;
        }

        private static bool SymbolIsAllowed(ISymbol symbol, Microsoft.CodeAnalysis.Accessibility[] accessibilities)
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

        private static bool HasRequiredAccessibility(ISymbol symbol, Microsoft.CodeAnalysis.Accessibility[] accessibilities)
        {
            Microsoft.CodeAnalysis.Accessibility accessibility;
            if(symbol is IPropertySymbol property)
            {
                if (property.GetMethod == null)
                    return false;
                accessibility = MergePropertyAccessibility(property.DeclaredAccessibility, property.GetMethod.DeclaredAccessibility);
            }
            else if (!(symbol is IFieldSymbol field && field.HasAttributeOfType<IGeneratesDataMemberPropertyFromFieldAttribute>()))
            {
                accessibility = symbol.DeclaredAccessibility;
            }
            else
            {
                var attribute = AttributeFactory.Create<IGeneratesDataMemberPropertyFromFieldAttribute>(
                    field.AttributesOfType<IGeneratesDataMemberPropertyFromFieldAttribute>().First());
                accessibility = MergePropertyAccessibility(attribute.Accessibility, attribute.GetterAccessibility);
            }

            if (accessibility == Microsoft.CodeAnalysis.Accessibility.NotApplicable)
                accessibility = Microsoft.CodeAnalysis.Accessibility.Private;

            return accessibilities.Contains(accessibility);
        }

        private static Microsoft.CodeAnalysis.Accessibility MergePropertyAccessibility(
            Microsoft.CodeAnalysis.Accessibility declaredAccessibility, 
            Microsoft.CodeAnalysis.Accessibility getterAccessibility)
        {
            if (getterAccessibility == Microsoft.CodeAnalysis.Accessibility.NotApplicable)
                return declaredAccessibility;
            return getterAccessibility;
        }
    }
}

#pragma warning restore IDE0130