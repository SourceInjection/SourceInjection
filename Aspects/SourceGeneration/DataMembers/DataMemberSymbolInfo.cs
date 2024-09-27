using Aspects.CodeAnalysis;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Aspects.SourceGeneration.DataMembers
{
    internal abstract class DataMemberSymbolInfo
    {
        protected DataMemberSymbolInfo(
            string name,
            Microsoft.CodeAnalysis.Accessibility
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

        public Microsoft.CodeAnalysis.Accessibility DeclaredAccessibility { get; }

        public ITypeSymbol ContainingType { get; }

        public ImmutableArray<AttributeData> Attributes { get; }

        public ITypeSymbol Type { get; }

        public IEnumerable<AttributeData> AttributesOfType<T>()
        {
            var name = typeof(T).FullName;
            return Attributes.Where(a => a.AttributeClass.Inheritance().Any(ac => ac.ToDisplayString() == name)
                || a.AttributeClass.AllInterfaces.Any(i => i.ToDisplayString() == name));
        }

        public virtual bool HasNotNullAttribute()
        {
            return Attributes
                .Any(a => a.IsNotNullAttribute());
        }

        public virtual bool HasMaybeNullAttribute()
        {
            return Attributes
                .Any(a => a.IsMayBeNullAttribute());
        }
    }
}
