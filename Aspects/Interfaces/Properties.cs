namespace Aspects.Interfaces
{
    public interface IGeneratesPublicDataMemberPropertyFromFieldAttribute 
    {
        string PropertyName(string fieldName);
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
