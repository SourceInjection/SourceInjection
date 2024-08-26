namespace Aspects.Attributes.Interfaces
{
    public interface IToStringConfigAttribute 
    {
        DataMemberKind DataMemberKind { get; }
    }

    public interface IToStringAttribute { }

    public interface IToStringExcludeAttribute { }
}
