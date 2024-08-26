namespace Aspects.Attributes.Interfaces
{
    public interface IHashCodeConfigAttribute 
    {
        DataMemberKind DataMemberKind { get; }

        bool ForceIncludeBase { get; }
    }

    public interface IHashCodeAttribute { }

    public interface IHashCodeExcludeAttribute { }
}
