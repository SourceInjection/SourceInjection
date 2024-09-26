using Microsoft.CodeAnalysis;

namespace Aspects.Interfaces
{
    public interface IGeneratesDataMemberPropertyFromFieldAttribute 
    {
        string PropertyName(string fieldName);

        Accessibility Accessibility { get; }

        Accessibility GetterAccessibility { get; }
    }

    public interface INotifyPropertyChangedAttribute : IGeneratesDataMemberPropertyFromFieldAttribute
    {
        bool EqualityCheck { get; }
    }

    public interface INotifyPropertyChangingAttribute : IGeneratesDataMemberPropertyFromFieldAttribute
    {
        bool EqualityCheck { get; }
    }
}
