namespace Aspects.Attributes.Interfaces
{
    public interface IGeneratesPropertyFromFieldAttribute { }

    public interface IPropertyEventGenerationAttribute : IGeneratesPropertyFromFieldAttribute 
    {
        bool EqualityCheck { get; }
    }

    public interface INotifyPropertyChangedAttribute : IPropertyEventGenerationAttribute { }

    public interface INotifyPropertyChangingAttribute : IPropertyEventGenerationAttribute { }

    public interface INotifyPropertyEventsAttribute : INotifyPropertyChangedAttribute, INotifyPropertyChangingAttribute { }
}
