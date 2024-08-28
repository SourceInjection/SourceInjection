using Microsoft.CodeAnalysis;

namespace Aspects.Attributes.Interfaces
{
    public interface IGeneratesPublicPropertyFromFieldAttribute 
    {
        string PropertyName(IFieldSymbol field);
    }

    public interface INotifyPropertyChangedAttribute : IGeneratesPublicPropertyFromFieldAttribute
    {
        bool EqualityCheck { get; }
    }

    public interface INotifyPropertyChangingAttribute : IGeneratesPublicPropertyFromFieldAttribute
    {
        bool EqualityCheck { get; }
    }
}
