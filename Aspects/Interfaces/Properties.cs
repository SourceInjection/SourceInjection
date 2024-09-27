
namespace Aspects.Interfaces
{
    using Microsoft.CodeAnalysis;
    using Accessibility = Microsoft.CodeAnalysis.Accessibility;

    public interface IGeneratesDataMemberPropertyFromFieldAttribute 
    {
        string PropertyName(IFieldSymbol field);

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
