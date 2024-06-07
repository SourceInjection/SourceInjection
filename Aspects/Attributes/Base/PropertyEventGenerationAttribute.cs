using Aspects.Attributes.Interfaces;
using Aspects.SourceGenerators.Common;
using Microsoft.CodeAnalysis;
using System;


namespace Aspects.Attributes.Base
{
    public abstract class PropertyEventGenerationAttribute : Attribute, IPropertyEventGenerationAttribute
    {
        protected PropertyEventGenerationAttribute(bool equalityCheck)
        {
            EqualityCheck = equalityCheck;
        }

        public bool EqualityCheck { get; }

        public string PropertyName(IFieldSymbol field)
        {
            return CodeSnippets.PropertyNameFromField(field);
        }
    }
}
