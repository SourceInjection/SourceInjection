namespace Aspects.Interfaces
{
    public interface IAutoToStringAttribute 
    {
        DataMemberKind DataMemberKind { get; }
    }

    public interface IToStringAttribute { }

    public interface IToStringExcludeAttribute { }
}
