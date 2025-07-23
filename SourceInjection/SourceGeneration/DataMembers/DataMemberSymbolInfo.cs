using SourceInjection.CodeAnalysis;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace SourceInjection.SourceGeneration.DataMembers
{
    internal abstract class DataMemberSymbolInfo
    {
        protected DataMemberSymbolInfo(
            string name,
            AccessModifier declaredModifier,
            ITypeSymbol containingType,
            ImmutableArray<AttributeData> attributes,
            ITypeSymbol type)
        {
            Name = name;
            DeclaredModifier = declaredModifier;
            ContainingType = containingType;
            Attributes = attributes;
            Type = type;
        }

        public string Name { get; }

        public AccessModifier DeclaredModifier { get; }

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
