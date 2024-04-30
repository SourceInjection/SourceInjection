using Microsoft.CodeAnalysis;

namespace Aspects.Attributes.Interfaces
{
    public enum PropertyEvent { Changing, Changed };

    public interface IGeneratesPropertyFromFieldAttribute
    {
        string PropertyName(IFieldSymbol field);
    }

    public interface IPropertyEventGenerationAttribute : IGeneratesPropertyFromFieldAttribute 
    {
        bool EqualityCheck { get; }

        PropertyEvent PropertyEvent { get; }
    }

    public interface INotifyPropertyChangedAttribute : IPropertyEventGenerationAttribute { }

    public interface INotifyPropertyChangingAttribute : IPropertyEventGenerationAttribute { }
}
