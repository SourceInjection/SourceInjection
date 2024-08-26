namespace Aspects.Attributes.Interfaces
{
    public interface IEqualsConfigAttribute 
    {
        DataMemberKind DataMemberKind { get; }

        bool ForceIncludeBase { get; }
    }

    public interface IEqualsAttribute { }

    public interface IEqualsExcludeAttribute { }
}
