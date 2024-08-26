using Microsoft.CodeAnalysis;

namespace Aspects.Attributes.Interfaces
{
    public interface IGeneratesPropertyFromFieldAttribute 
    {
        string PropertyName(IFieldSymbol field);
    }

    public interface INotifyPropertyChangedAttribute : IGeneratesPropertyFromFieldAttribute
    {
        bool EqualityCheck { get; }
    }

    public interface INotifyPropertyChangingAttribute : IGeneratesPropertyFromFieldAttribute
    {
        bool EqualityCheck { get; }
    }
}
