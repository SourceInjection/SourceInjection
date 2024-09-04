using Aspects.Util;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Aspects.SourceGenerators.Base.DataMembers
{
    internal abstract class DataMemberSymbolInfo
    {
        protected DataMemberSymbolInfo(
            string name,
            Accessibility
            declaredAccessibility,
            ITypeSymbol containingType,
            ImmutableArray<AttributeData> attributes,
            ITypeSymbol type)
        {
            Name = name;
            DeclaredAccessibility = declaredAccessibility;
            ContainingType = containingType;
            Attributes = attributes;
            Type = type;
        }

        public string Name { get; }

        public Accessibility DeclaredAccessibility { get; }

        public ITypeSymbol ContainingType { get; }

        public ImmutableArray<AttributeData> Attributes { get; }

        public ITypeSymbol Type { get; }

        public IEnumerable<AttributeData> AttributesOfType<T>()
        {
            var name = typeof(T).FullName;
            return Attributes.Where(a => a.AttributeClass.Inheritance().Any(ac => ac.ToDisplayString() == name)
                || a.AttributeClass.AllInterfaces.Any(i => i.ToDisplayString() == name));
        }

        public bool HasNotNullAttribute()
        {
            return Attributes
                .Any(a => IsNotNullAttribute(a.AttributeClass.ToDisplayString()));
        }

        public bool HasMaybeNullAttribute()
        {
            return Attributes
                .Any(a => IsMaybeNullAttribute(a.AttributeClass.ToDisplayString()));
        }

        private static bool IsNotNullAttribute(string attributeName)
        {
            const string notNullAttribute = "System.Diagnostics.CodeAnalysis.NotNullAttribute";
            return attributeName == notNullAttribute;
        }

        private static bool IsMaybeNullAttribute(string attributeName)
        {
            const string maybeNullAttribute = "System.Diagnostics.CodeAnalysis.MaybeNullAttribute";
            return attributeName == maybeNullAttribute;
        }
    }
}
