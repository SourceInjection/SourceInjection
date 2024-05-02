namespace Aspects.Attributes.Interfaces
{
    public enum PropertyEvent { Changing, Changed };

    public interface IGeneratesPropertyFromFieldAttribute { }

    public interface IPropertyEventGenerationAttribute : IGeneratesPropertyFromFieldAttribute 
    {
        bool EqualityCheck { get; }

        PropertyEvent PropertyEvent { get; }
    }

    public interface INotifyPropertyChangedAttribute : IPropertyEventGenerationAttribute { }

    public interface INotifyPropertyChangingAttribute : IPropertyEventGenerationAttribute { }
}
