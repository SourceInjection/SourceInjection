using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System;

namespace SourceInjection.Interfaces
{
    public interface IGeneratesDataMemberPropertyFromFieldAttribute 
    {
        /// <summary>
        /// Evaluates the generated property name from the field name.
        /// </summary>
        /// <param name="field">The field from which the property is generated.</param>
        /// <returns>The name of the generated property.</returns>
        string PropertyName(IFieldSymbol field);

        Accessibility Accessibility { get; }

        Accessibility GetterAccessibility { get; }
    }

    public interface IPropertyEventGenerationAttribute : IGeneratesDataMemberPropertyFromFieldAttribute, IEqualityComparerAttribute
    {
        /// <summary>
        /// Determines if a equality check is generated.
        /// </summary>
        bool EqualityCheck { get; }

        /// <summary>
        /// Determines if a <see cref="ArgumentNullException"/> is thrown when <see langword="value"/> is <see langword="null"/>.
        /// </summary>
        bool ThrowIfValueIsNull { get; }

        Accessibility SetterAccessibility { get; }

        /// <summary>
        /// Also raises a event for the defined properties.
        /// </summary>
        IEnumerable<string> RelatedProperties { get; }
    }

    public interface INotifyPropertyChangedAttribute : IPropertyEventGenerationAttribute { }

    public interface INotifyPropertyChangingAttribute : IPropertyEventGenerationAttribute { }
}
