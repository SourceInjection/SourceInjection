﻿using Microsoft.CodeAnalysis;
using SourceInjection.Util;
using System.Collections.Immutable;

namespace SourceInjection.SourceGeneration.DataMembers
{
    internal class FieldSymbolInfo : DataMemberSymbolInfo
    {
        protected FieldSymbolInfo(
            string name,
            AccessModifier declaredModifier,
            ITypeSymbol containingType,
            ImmutableArray<AttributeData> attributes,
            ITypeSymbol type

            ) : base(

            name: name,
            declaredModifier: declaredModifier,
            containingType: containingType,
            attributes: attributes,
            type: type)
        { }

        public static FieldSymbolInfo Create(IFieldSymbol field)
        {
            return new FieldSymbolInfo(
                name: field.Name,
                declaredModifier: field.DeclaredAccessibility.ToAccessModifier(),
                containingType: field.ContainingType,
                attributes: field.GetAttributes(),
                type: field.Type);
        }
    }
}
