
namespace SourceInjection.Interfaces
{
    using Microsoft.CodeAnalysis;
    using Accessibility = Microsoft.CodeAnalysis.Accessibility;

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

    public interface INotifyPropertyChangedAttribute : IGeneratesDataMemberPropertyFromFieldAttribute
    {
        /// <summary>
        /// Determines if a equality check is generated.
        /// </summary>
        bool EqualityCheck { get; }
    }

    public interface INotifyPropertyChangingAttribute : IGeneratesDataMemberPropertyFromFieldAttribute
    {
        /// <summary>
        /// Determines if a equality check is generated.
        /// </summary>
        bool EqualityCheck { get; }
    }
}
