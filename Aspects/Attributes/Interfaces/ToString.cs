namespace Aspects.Attributes.Interfaces
{
    public interface IToStringConfigAttribute : IBasicMethodConfigAttribute 
    {
        bool CollectionsEnabled { get; }
    }

    public interface IToStringAttribute { }

    public interface IToStringExcludeAttribute { }
}
