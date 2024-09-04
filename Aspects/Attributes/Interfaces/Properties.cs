using Microsoft.CodeAnalysis;

namespace Aspects.Attributes.Interfaces
{
    public interface IUseOnPropertyGenerationAttribute { }

    public interface IGeneratesPublicDataMemberPropertyFromFieldAttribute 
    {
        string PropertyName(IFieldSymbol field);
    }

    public interface INotifyPropertyChangedAttribute : IGeneratesPublicDataMemberPropertyFromFieldAttribute
    {
        bool EqualityCheck { get; }
    }

    public interface INotifyPropertyChangingAttribute : IGeneratesPublicDataMemberPropertyFromFieldAttribute
    {
        bool EqualityCheck { get; }
    }
}
