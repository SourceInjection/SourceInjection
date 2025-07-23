using Microsoft.CodeAnalysis;
using System.Collections.Generic;

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

        AccessModifier Modifier { get; }

        AccessModifier GetterModifier { get; }
    }

    public interface IPropertyEventGenerationAttribute : IGeneratesDataMemberPropertyFromFieldAttribute
    {
        /// <summary>
        /// Determines if a inequality check is generated.
        /// </summary>
        bool InEqualityCheck { get; }

        NullSafety NullSafety { get; }

        AccessModifier SetterModifier { get; }

        /// <summary>
        /// Also raises a event for the defined properties.
        /// </summary>
        IEnumerable<string> RelatedProperties { get; }
    }

    public interface INotifyPropertyChangedAttribute : IPropertyEventGenerationAttribute { }

    public interface INotifyPropertyChangingAttribute : IPropertyEventGenerationAttribute { }
}
